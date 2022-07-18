using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelObjectives.Objectives
{
    [Serializable]
    public class ObjectivesQueue
    {
        [SerializeField]private List<Objective> _objectives;

        public ObjectivesQueue() { }
        
        public ObjectivesQueue(int size)
        {
            _objectives = new List<Objective>(size);
        }
        
        public ObjectivesQueue(Objective[] objectivesArray)
        {
            _objectives = new List<Objective>(objectivesArray);
        }
        
        public ObjectivesQueue(List<Objective> objectivesList)
        {
            _objectives = new List<Objective>(objectivesList);
        }

        public void EnQueue(Objective objective)
        {
            _objectives.Add(objective);
        }
        
        public Objective DeQueue()
        {
            Objective objectiveToRemove = _objectives[0];
            _objectives.Remove(objectiveToRemove);
            return objectiveToRemove;
        }
        
        public int GetLength()
        {
            return _objectives.Count;
        }

        public Objective Peek()
        {
            return _objectives[0];
        }

        public bool Contains(Objective objective)
        {
            return _objectives.Contains(objective);
        }

        public Objective[] ToArray()
        {
            return _objectives.ToArray();
        }

        public List<Objective> ToList()
        {
            return _objectives;
        }
        
        public Queue<Objective> ToQueue()
        {
            return new Queue<Objective>(_objectives);
        }
    }
}