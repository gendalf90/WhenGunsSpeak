using System;
using UnityEngine;

namespace Utils
{
    public class EventTimer
    {
        private double? periodInSeconds;
        private float? startTimeInSeconds;
        private bool isStartImmediately;

        public void Start(TimeSpan period)
        {
            periodInSeconds = period.TotalSeconds;
            startTimeInSeconds = null;
            isStartImmediately = false;
        }

        public void StartImmediately(TimeSpan period)
        {
            periodInSeconds = period.TotalSeconds;
            startTimeInSeconds = null;
            isStartImmediately = true;
        }

        public event Action Action;

        public void Update()
        {
            if(!IsStarted)
            {
                return;
            }

            if(IsFirstUpdate)
            {
                UpdateStartTime();
            }

            if(!IsTimeToInvoke)
            {
                return;
            }

            InvokeAction();
            UpdateStartTime();
            StopImmediatelyInvoking();
        }

        private bool IsStarted => periodInSeconds.HasValue;

        private bool IsFirstUpdate => !startTimeInSeconds.HasValue;

        private void UpdateStartTime()
        {
            startTimeInSeconds = Time.realtimeSinceStartup;
        }

        private bool IsTimeToInvoke => Time.realtimeSinceStartup - startTimeInSeconds > periodInSeconds || isStartImmediately;

        private void InvokeAction()
        {
            Action?.Invoke();
        }

        private void StopImmediatelyInvoking()
        {
            isStartImmediately = false;
        }

        public void Stop()
        {
            periodInSeconds = null;
        }
    }
}
