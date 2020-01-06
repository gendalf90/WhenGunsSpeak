using System;
using UniRx;
using Utils;

namespace Weapon.Common
{
    public class BeforeStartDelayDecorator : IShot
    {
        private readonly IShot shot;
        private readonly RealTimeTimer delayTimer;

        public BeforeStartDelayDecorator(IShot shot, TimeSpan delay)
        {
            this.shot = shot;

            delayTimer = new RealTimeTimer(delay);
        }

        public void Start()
        {
            delayTimer.Start();
        }

        public void Stop()
        {
            shot.Stop();
            delayTimer.Stop();
        }

        public IDisposable Subscribe(IObserver<ShotInfo> observer)
        {
            return delayTimer
                .Do(invokeInfo => shot.Start())
                .ContinueWith(shot)
                .Subscribe(observer);
        }

        public void Update()
        {
            delayTimer.Update();
            shot.Update();
        }
    }
}
