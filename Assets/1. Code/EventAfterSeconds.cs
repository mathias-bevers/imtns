using System;
using System.Collections.Generic;
using CleanRoom.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom
{
    public class EventAfterSeconds : MonoBehaviour
    {
        
        [SerializeField] private List<SerializablePair<UnityEvent, float>> events;
        private readonly List<int> completedEventIndexes = new();
        
        private float timer = 0;

        private void Update()
        {
            if (events.IsNullOrEmpty())
            {
                enabled = false;
                return;
            }

            timer += Time.deltaTime;

            for (int i = events.Count - 1; i >= 0; --i)
            {
                if (completedEventIndexes.Contains(i))
                {
                    continue;
                }
                
                SerializablePair<UnityEvent, float> eventTimePair = events[i];
                
                if (timer < eventTimePair.Second)
                {
                    continue;
                }

                eventTimePair.First.Invoke();
                completedEventIndexes.Add(i);
            }
        }

        public void AddEvent(UnityEvent @event, float delay)
        {
            events.Add(new SerializablePair<UnityEvent, float>
            {
                First = @event,
                Second = delay + timer
            });

            enabled = true;
        }

        public void ResetEvents()
        {
            timer = 0;
            completedEventIndexes.Clear();
        }
    }
}