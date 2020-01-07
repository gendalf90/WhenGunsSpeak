using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utils;
using Weapon.Common;

namespace Weapon
{
    public class AKM : MonoBehaviour, IFlippable, IShootable, IListable, ISpawnable
    {
        private const string Name = "AKM";
        private const string ShellKey = "7dot62";

        private static readonly TimeSpan beetwenShotsDelay = TimeSpan.FromMilliseconds(800);

        private Transform bodyTransform;
        private Transform barrelTransform;

        private IShot shot;

        public AKM()
        {
            var actionShot = new OnStartActionShot(MakeShot);
            var delayShot = new AfterStartDelayDecorator(actionShot, beetwenShotsDelay);

            shot = delayShot;

            shot.Subscribe(shotInfo => shot.Start());
        }

        private void Awake()
        {
            bodyTransform = transform.Find("Body");
            barrelTransform = transform.Find("Barrel");
        }

        private void Update()
        {
            shot.Update();
        }

        public void AddIfFirstWeapon(IList<string> list)
        {
            list.Add(Name);
        }

        public void FlipToLeft()
        {
            bodyTransform.SetFlipY(true);
        }

        public void FlipToRight()
        {
            bodyTransform.SetFlipY(false);
        }

        public void StartShooting()
        {
            shot.Start();
        }

        public void StopShooting()
        {
            shot.Stop();
        }

        private void MakeShot()
        {
            //отдача, вспышка и т.д.
        }

        public void SpawnIfNameIs(string name)
        {
            if(name == Name)
            {
                gameObject.SetActive(true);
            }
        }

        public void UnspawnIfNameIs(string name)
        {
            if (name == Name)
            {
                gameObject.SetActive(false);
            }
        }

        public IDisposable Subscribe(IObserver<ShotData> observer)
        {
            return shot
                .Select(shotInfo => new ShotData
                {
                    ShellId = IdGenerator.Generate(),
                    ShellKey = ShellKey,
                    //Position = barrelTransform.position,
                    //Rotation = transform.rotation.z
                })
                .Subscribe(observer);
        }
    }
}
