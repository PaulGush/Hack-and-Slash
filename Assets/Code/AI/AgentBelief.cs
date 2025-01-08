using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.AI
{
    public class BeliefFactory
    {
        readonly GOAPAgent agent;
        readonly Dictionary<string, AgentBelief> beliefs;

        public BeliefFactory(GOAPAgent agent, Dictionary<string, AgentBelief> beliefs)
        {
            this.agent = agent;
            this.beliefs = beliefs;
        }

        public void AddBelief(string key, Func<bool> condition)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(condition)
                .Build());
        }

        public void AddSensorBelief(string key, Sensor sensor)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => sensor.IsTargetInRange)
                .WithLocation(() => sensor.GetTargetPosition)
                .Build());
        }
        
        bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(agent.transform.position, pos) < range;

        public void AddLocationBelief(string key, float distance, Transform locationCondition)
        {
            AddLocationBelief(key, distance, locationCondition);
        }

        public void AddLocationBelief(string key, float distance, Vector3 locationCondition)
        {
            beliefs.Add(key, new AgentBelief.Builder(key)
                .WithCondition(() => InRangeOf(locationCondition, distance))
                .WithLocation(() => locationCondition)
                .Build());
        }
    }

    public class AgentBelief
    {
        public string Name { get; }

        private Func<bool> m_condition = () => false;
        private Func<Vector3> m_observedLocation = () => Vector3.zero;
        
        public Vector3 Location => m_observedLocation();

        AgentBelief(string name)
        {
            Name = name;
        }

        public bool Evaluate() => m_condition();

        public class Builder
        {
            readonly AgentBelief belief;

            public Builder(string name)
            {
                belief = new AgentBelief(name);
            }

            public Builder WithCondition(Func<bool> condition)
            {
                belief.m_condition = condition;
                return this;
            }

            public Builder WithLocation(Func<Vector3> observedLocation)
            {
                belief.m_observedLocation = observedLocation;
                return this;
            }

            public AgentBelief Build()
            {
                return belief;   
            }
        }
    }
}