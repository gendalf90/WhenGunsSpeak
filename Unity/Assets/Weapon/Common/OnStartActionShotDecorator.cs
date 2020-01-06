using System;

namespace Weapon.Common
{
    public class OnStartActionShotDecorator : IShot
    {
        private readonly IShot shot;
        private readonly Action action;

        public OnStartActionShotDecorator(IShot shot, Action action)
        {
            this.shot = shot;
            this.action = action;
        }

        public void Start()
        {
            action.Invoke();
            shot.Start();
        }

        public void Stop()
        {
            shot.Stop();
        }

        public IDisposable Subscribe(IObserver<ShotInfo> observer)
        {
            return shot.Subscribe(observer);
        }

        public void Update()
        {
            shot.Update();
        }
    }
}
