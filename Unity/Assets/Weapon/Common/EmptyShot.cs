using System;
using UniRx;

namespace Weapon.Common
{
    public class EmptyShot : IShot
    {
        private readonly Subject<ShotInfo> subject = new Subject<ShotInfo>();

        public void Start()
        {
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
