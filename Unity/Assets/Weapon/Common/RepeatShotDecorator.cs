using System;
using UniRx;

namespace Weapon.Common
{
    public class RepeatShotDecorator : IShot
    {
        private readonly IShot shot;
        private bool needRepeat;
        private bool hasStarted;

        public RepeatShotDecorator(IShot shot)
        {
            this.shot = shot;
        }

        public void Start()
        {
            shot.Start();

            hasStarted = true;
        }

        public void Stop()
        {
            shot.Stop();

            hasStarted = false;
        }

        public IDisposable Subscribe(IObserver<ShotInfo> observer)
        {
            return shot
                .Do(shotInfo => needRepeat = hasStarted)
                .Subscribe(observer);
        }

        public void Update()
        {
            shot.Update();

            if (hasStarted && needRepeat)
            {
                shot.Start();
            }

            needRepeat = false;
        }
    }
}
