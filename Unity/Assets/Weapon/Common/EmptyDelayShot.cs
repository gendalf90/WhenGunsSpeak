using System;
using System.Linq;
using UniRx;
using Utils;

namespace Weapon.Common
{
    public class EmptyDelayShot : IShot
    {
        private readonly RealTimeTimer delayTimer;

        public EmptyDelayShot(TimeSpan delay)
        {
            delayTimer = new RealTimeTimer(delay);
        }

        public void Start()
        {
            delayTimer.Start();
        }

        public void Stop()
        {
            delayTimer.Stop();
        }

        public IDisposable Subscribe(IObserver<ShotInfo> observer)
        {
            return delayTimer
                .Select(invokeInfo => new ShotInfo())
                .Subscribe(observer);
        }

        public void Update()
        {
            delayTimer.Update();
        }
    }
}
