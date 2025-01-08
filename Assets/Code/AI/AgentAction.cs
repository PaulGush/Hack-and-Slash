using System.Collections.Generic;
using Unity.VisualScripting;

namespace Code.AI
{
    public class AgentAction
    {
        public string Name { get; }
        public float Cost { get; private set; }

        public HashSet<AgentBelief> Preconditions { get; } = new();
        public HashSet<AgentBelief> Effects { get; } = new();

        AgentAction(string name)
        {
            Name = name;
        }

        private IActionStrategy m_strategy;

        public bool Complete => m_strategy.Complete;
        
        public void Start() => m_strategy.Start();

        public void Update(float deltaTime)
        {
            if (m_strategy.CanPerform)
            {
                m_strategy.Update(deltaTime);
            }

            if (!m_strategy.Complete) return;

            foreach (var effect in Effects)
            {
                effect.Evaluate();
            }
        }
        
        public void Stop() => m_strategy.Stop();

        public class Builder
        {
            readonly AgentAction m_agentAction;

            public Builder(string name)
            {
                m_agentAction = new AgentAction(name)
                {
                    Cost = 1
                };
            }

            public Builder WithCost(float cost)
            {
                m_agentAction.Cost = cost;
                return this;
            }
            
            public Builder WithStrategy(IActionStrategy strategy)
            {
                m_agentAction.m_strategy = strategy;
                return this;
            }
            
            public Builder AddPrecondition(AgentBelief precondition)
            {
                m_agentAction.Preconditions.Add(precondition);
                return this;
            }
            
            public Builder AddEffect(AgentBelief effect)
            {
                m_agentAction.Effects.Add(effect);
                return this;
            }
            
            public AgentAction Build() => m_agentAction;
        }
    }
}