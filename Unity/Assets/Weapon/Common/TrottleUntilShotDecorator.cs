using System;
using UniRx;

namespace Weapon.Common
{
    public class TrottleUntilShotDecorator : IShot
    {
        private readonly IShot shot;
        private bool isStartIgnoring;
        private bool isStopping;
        private bool hasShot;

        public TrottleUntilShotDecorator(IShot shot)
        {
            this.shot = shot;
        }

        public void Start()
        {
            if (isStartIgnoring)
            {
                return;
            }

            shot.Start();

            isStartIgnoring = true;
        }

        public void Stop()
        {
            isStopping = true;
        }

        public IDisposable Subscribe(IObserver<ShotInfo> observer)
        {
            return shot
                .Do(shotInfo => StopStartIgnoring())
                .Do(shotInfo => StopIfNeeded())
                .Subscribe(observer);
        }

        private void StopStartIgnoring()
        {
            isStartIgnoring = false;
        }

        private void StopIfNeeded()
        {
            if (!isStopping)
            {
                return;
            }

            shot.Stop();

            isStopping = false;
        }

        public void Update()
        {
            shot.Update();
        }
    }
}
