using System;
using UniRx;

namespace Weapon.Common
{
    public class AfterShotActionDecorator : IShot
    {
        private readonly IShot shot;
        private readonly Action action;

        public AfterShotActionDecorator(IShot shot, Action action)
        {
            this.shot = shot;
            this.action = action;
        }

        public void Start()
        {
            shot.Start();
        }

        public void Stop()
        {
            shot.Stop();
        }

        public IDisposable Subscribe(IObserver<ShotInfo> observer)
        {
            return shot
                .Do(shotInfo => action.Invoke())
                .Subscribe(observer);
        }

        public void Update()
        {
            shot.Update();
        }
    }
}
