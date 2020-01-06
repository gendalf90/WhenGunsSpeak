using System;
using UniRx;

namespace Weapon.Common
{
    public class OnStartActionShot : IShot
    {
        private readonly Action action;
        private readonly Subject<ShotInfo> subject = new Subject<ShotInfo>();

        public OnStartActionShot(Action action)
        {
            this.action = action;
        }

        public void Start()
        {
            action.Invoke();
            subject.OnNext(new ShotInfo());
        }

        public void Stop()
        {
        }

        public IDisposable Subscribe(IObserver<ShotInfo> observer)
        {
            return subject.Subscribe(observer);
        }

        public void Update()
        {
        }
    }
}
