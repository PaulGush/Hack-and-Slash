using System;
using System.Collections.Generic;
using System.Linq;
using Code.Gameplay;
using Code.Mobs.Enemy;
using Code.Mobs.Enemy.Animation;
using UnityEngine;
using UnityEngine.AI;

namespace Code.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class GOAPAgent : MonoBehaviour
    {
        [Header("Sensors")]
        [SerializeField] private Sensor m_chaseSensor;
        [SerializeField] private Sensor m_attackSensor;
        
        [Header("References")]
        [SerializeField] private NavMeshAgent m_agent;
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private HealthSystem m_healthSystem;
        [SerializeField] private Enemy m_enemy;
        [SerializeField] private EnemyAnimationController m_enemyAnimationController;
        
        private CountdownTimer m_timer;
        private IGoapPlanner m_goalPlanner;
        
        GameObject m_target;
        Vector3 m_destination;
        
        AgentGoal m_previousGoal;
        public AgentGoal CurrentGoal;
        public ActionPlan ActionPlan;
        public AgentAction CurrentAction;

        public Dictionary<string, AgentBelief> Beliefs;
        public HashSet<AgentAction> Actions;
        public HashSet<AgentGoal> Goals;

        private void Awake()
        {
            m_goalPlanner = new GoapPlanner();
        }

        private void Start()
        {
            SetupTimers();
            SetupBeliefs();
            SetupActions();
            SetupGoals();
        }

        private void SetupTimers()
        {
            
        }

        private void SetupBeliefs()
        {
            Beliefs = new Dictionary<string, AgentBelief>();
            BeliefFactory factory = new BeliefFactory(this, Beliefs);
            
            factory.AddBelief("Nothing", () => false);
            
            factory.AddBelief("AgentIdle", () => !m_agent.hasPath);
            factory.AddBelief("AgentMoving", () => m_agent.hasPath);
            
            factory.AddBelief("HealthIsLow", () => m_healthSystem.GetCurrentHealth() < 25);
            factory.AddBelief("HealthIsFine", () => m_healthSystem.GetCurrentHealth() > 50);
            
            factory.AddSensorBelief("PlayerInChaseRange", m_chaseSensor);
            factory.AddSensorBelief("PlayerInAttackRange", m_attackSensor);
            factory.AddBelief("AttackingPlayer", () => false);
        }

        private void SetupActions()
        {
            Actions = new HashSet<AgentAction>();

            Actions.Add(new AgentAction.Builder("Relax")
                .WithStrategy(new IdleStrategy(5))
                .AddEffect(Beliefs["Nothing"])
                .Build());
            
            Actions.Add(new AgentAction.Builder("Wander Around")
                .WithStrategy(new WanderStrategy(m_agent, 10))
                .AddEffect(Beliefs["AgentMoving"])
                .Build());

            Actions.Add(new AgentAction.Builder("ChasePlayer")
                .WithStrategy(new MoveStrategy(m_agent, () => Beliefs["PlayerInChaseRange"].Location))
                .AddPrecondition(Beliefs["PlayerInChaseRange"])
                .AddEffect(Beliefs["PlayerInAttackRange"])
                .Build());

            Actions.Add(new AgentAction.Builder("AttackPlayer")
                .WithStrategy(new AttackStrategy(m_enemy.GetAbilityAnimationClip(), m_enemyAnimationController))
                .AddPrecondition(Beliefs["PlayerInAttackRange"])
                .AddEffect(Beliefs["AttackingPlayer"])
                .Build());
        }

        private void SetupGoals()
        {
            Goals = new HashSet<AgentGoal>();

            Goals.Add(new AgentGoal.Builder("Chill out")
                .WithPriority(1)
                .WithDesiredEffect(Beliefs["Nothing"])
                .Build());

            Goals.Add(new AgentGoal.Builder("Wander")
                .WithPriority(1)
                .WithDesiredEffect(Beliefs["AgentMoving"])
                .Build());

            Goals.Add(new AgentGoal.Builder("SeekAndDestroy")
                .WithPriority(3)
                .WithDesiredEffect(Beliefs["AttackingPlayer"])
                .Build());
        }

        private void OnEnable() => m_chaseSensor.OnTargetChanged += HandleTargetChanged;
        private void OnDisable() => m_chaseSensor.OnTargetChanged -= HandleTargetChanged;

        private void HandleTargetChanged()
        {
            CurrentAction = null;
            CurrentGoal = null;
        }

        private void Update()
        {
            if (CurrentAction == null)
            {
                CalculatePlan();

                if (ActionPlan != null && ActionPlan.Actions.Count > 0)
                {
                    m_agent.ResetPath();
                    
                    CurrentGoal = ActionPlan.Goal;
                    CurrentAction = ActionPlan.Actions.Pop();

                    if (CurrentAction.Preconditions.All(b => b.Evaluate()))
                    {
                        CurrentAction.Start();
                    }
                    else
                    {
                        CurrentAction = null;
                        CurrentGoal = null;
                    }
                }
            }

            if (ActionPlan != null && CurrentAction != null)
            {
                CurrentAction.Update(Time.deltaTime);

                if (CurrentAction.Complete)
                {
                    CurrentAction.Stop();
                    CurrentAction = null;

                    if (ActionPlan.Actions.Count == 0)
                    {
                        m_previousGoal = CurrentGoal;
                        CurrentGoal = null;
                    }
                }
            }
        }

        private void CalculatePlan()
        {
            var priorityLevel = CurrentGoal?.Priority ?? 0;

            HashSet<AgentGoal> goalsToCheck = Goals;
            
            //If we have a current goal, we only want to check goals with a higher priority
            if (CurrentGoal != null)
            {
                Debug.Log("Current goal exists, checking for goals with higher priority.");
                goalsToCheck = new HashSet<AgentGoal>(Goals.Where(g => g.Priority > priorityLevel));
            }

            var potentialPlan = m_goalPlanner.Plan(this, goalsToCheck, m_previousGoal);
            if (potentialPlan != null)
            {
                ActionPlan = potentialPlan;
            }
        }
    }
}