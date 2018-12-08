using System;
using UnityEngine;

namespace Utils
{
    public class EventTimer
    {
        private double periodInSeconds;
        private float startTimeInSeconds;
        private bool isStarted;

        public void Start(TimeSpan period)
        {
            startTimeInSeconds = Time.realtimeSinceStartup;
            periodInSeconds = period.TotalSeconds;
            isStarted = true;
        }

        public event Action Action;

        public void Update()
        {
            if(!isStarted)
            {
                return;
            }

            var isItTime = Time.realtimeSinceStartup - startTimeInSeconds > periodInSeconds;

            if(!isItTime)
            {
                return;
            }

            startTimeInSeconds = Time.realtimeSinceStartup;
            Action?.Invoke();
        }

        public void Stop()
        {
            isStarted = false;
        }
    }
}
