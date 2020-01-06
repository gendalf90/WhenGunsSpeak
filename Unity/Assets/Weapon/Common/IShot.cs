using System;

namespace Weapon.Common
{
    public class ShotInfo
    {
    }

    public interface IShot : IObservable<ShotInfo>
    {
        void Start();

        void Stop();

        void Update();
    }
}
