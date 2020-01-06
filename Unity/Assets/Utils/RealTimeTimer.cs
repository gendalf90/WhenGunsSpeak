using System;
using UniRx;
using UnityEngine;

namespace Utils
{
    public class InvokeInfo
    {
        public TimeSpan ActualTimeElapsed { get; set; }
    }

    public class RealTimeTimer : IObservable<InvokeInfo>
    {
        private readonly TimeSpan period;

        private float? startTimeInSeconds;

        private readonly Subject<InvokeInfo> subject = new Subject<InvokeInfo>();

        public RealTimeTimer(TimeSpan period)
        {
            this.period = period;
        }

        public void Start()
        {
            startTimeInSeconds = Time.realtimeSinceStartup;
        }

        public void Stop()
        {
            startTimeInSeconds = null;
        }

        public void Update()
        {
            if (!HasStarted || !IsTimeToInvoke)
            {
                return;
            }

            InvokeAction();
            Stop();
        }

        public void TryStartAndUpdate()
        {
            StartIfHasNotStarted();
            Update();
        }

        private void StartIfHasNotStarted()
        {
            if (!HasStarted)
            {
                Start();
            }
        }

        private bool HasStarted => startTimeInSeconds.HasValue;

        private bool IsTimeToInvoke => Time.realtimeSinceStartup - startTimeInSeconds.Value > period.TotalSeconds;

        private void InvokeAction()
        {
            subject.OnNext(new InvokeInfo
            {
                ActualTimeElapsed = TimeSpan.FromSeconds(Time.realtimeSinceStartup - startTimeInSeconds.Value)
            });
        }

        public IDisposable Subscribe(IObserver<InvokeInfo> observer)
        {
            return subject.Subscribe(observer);
        }
    }
}
