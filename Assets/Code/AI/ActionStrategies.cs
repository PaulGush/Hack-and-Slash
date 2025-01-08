using System;
using Code.Mobs.Enemy.Animation;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils;
using Random = UnityEngine.Random;

namespace Code.AI
{
    public interface IActionStrategy
    {
        bool CanPerform { get; }
        bool Complete { get; }

        void Start()
        {
        }

        void Update(float deltaTime)
        {
        }

        void Stop()
        {
        }
    }

    public class AttackStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        private readonly CountdownTimer timer;
        private readonly EnemyAnimationController enemyAnimationController;
        private readonly AnimationClip m_attackAnimationClip;

        public AttackStrategy(AnimationClip clip, EnemyAnimationController enemyAnimationController)
        {
            this.enemyAnimationController = enemyAnimationController;
            m_attackAnimationClip = clip;
            
            timer = new CountdownTimer(m_attackAnimationClip.length);
            timer.OnTimerStart += () => Complete = false;
            timer.OnTimerStop += () => Complete = true;
        }

        public void Start()
        {
            timer.Start();
            enemyAnimationController.PlayAnimation(m_attackAnimationClip);
        }
        
        public void Update(float deltaTime) => timer.Tick(deltaTime);
    }

    public class MoveStrategy : IActionStrategy
    {
        readonly NavMeshAgent agent;
        readonly Func<Vector3> destination;
        public bool CanPerform => !Complete;
        public bool Complete => agent.remainingDistance <= 2f && !agent.pathPending;

        public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination)
        {
            this.agent = agent;
            this.destination = destination;
        }

        public void Start() => agent.SetDestination(destination());
        public void Stop() => agent.ResetPath();
    }

    public class WanderStrategy : IActionStrategy
    {
        readonly NavMeshAgent agent;
        readonly float wanderRadius;

        public bool CanPerform => !Complete;
        public bool Complete => agent.remainingDistance <= 2f && !agent.pathPending;

        public WanderStrategy(NavMeshAgent agent, float wanderRadius)
        {
            this.agent = agent;
            this.wanderRadius = wanderRadius;
        }

        public void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomDirection = (Random.insideUnitSphere * wanderRadius).With(y: 0);
                NavMeshHit hit;

                if (NavMesh.SamplePosition(agent.transform.position + randomDirection, out hit, wanderRadius, 1))
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }
    }

    public class IdleStrategy : IActionStrategy
    {
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        readonly CountdownTimer timer;

        public IdleStrategy(float duration)
        {
            timer = new CountdownTimer(duration);
            timer.OnTimerStart += () => Complete = false;
            timer.OnTimerStop += () => Complete = true;
        }
        
        public void Start() => timer.Start();
        public void Update(float deltaTime) => timer.Tick(deltaTime);
    }
}