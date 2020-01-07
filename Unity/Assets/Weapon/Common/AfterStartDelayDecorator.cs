using System;
using UniRx;
using Utils;

namespace Weapon.Common
{
    public class AfterStartDelayDecorator : IShot
    {
        private readonly IShot shot;
        private readonly RealTimeTimer delayTimer;

        public AfterStartDelayDecorator(IShot shot, TimeSpan delay)
        {
            this.shot = shot;

            delayTimer = new RealTimeTimer(delay);
        }

        public void Start()
        {
            shot.Start();
        }

        public void Stop()
        {
            shot.Stop();
            delayTimer.Stop();
        }

        public IDisposable Subscribe(IObserver<ShotInfo> observer)
        {
            return shot
                .Do(shotInfo => delayTimer.Start())
                .SelectMany(delayTimer)
                .Select(invokeInfo => new ShotInfo())
                .Subscribe(observer);
        }

        public void Update()
        {
            delayTimer.Update();
            shot.Update();
        }
    }
}
