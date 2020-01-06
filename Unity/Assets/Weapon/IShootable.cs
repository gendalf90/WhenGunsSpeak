using System;
using UnityEngine;

namespace Weapon
{
    public class ShotData
    {
        public string ShellId { get; set; }

        public string ShellKey { get; set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }
    }

    public interface IShootable : IObservable<ShotData>
    {
        void StartShooting();

        //можно покрыть кейсы с одиночным огнем
        void StopShooting();
    }
}
