using System;
using Input;
using Messages;
using Shells;
using UnityEngine;
using Soldier.Rotation;

namespace Soldier.Weapon
{
    public class AKM : MonoBehaviour
    {
        [SerializeField]
        private float shotIntervalInSeconds;

        [SerializeField]
        private float reloadTimeInSeconds;

        private Transform bodyTransform;
        private Transform triggerTransform;
        private Transform forendTransform;
        private Transform barrelTransform;

        private Observable observable;
        private SimpleTimer shootTimer;

        private bool isShooting;

        private void Awake()
        {
            bodyTransform = transform.Find("Body");
            triggerTransform = bodyTransform.Find("Trigger");
            forendTransform = bodyTransform.Find("Forend");
            barrelTransform = transform.Find("Barrel");

            observable = FindObjectOfType<Observable>();
        }

        private void Start()
        {
            shootTimer = SimpleTimer.StartNew(shotIntervalInSeconds);
        }

        private void OnEnable()
        {
            observable.Subscribe<StartFireEvent>(StartFire);
            observable.Subscribe<EndFireEvent>(EndFire);
            observable.Subscribe<LookEvent>(LookHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<StartFireEvent>(StartFire);
            observable.Unsubscribe<EndFireEvent>(EndFire);
            observable.Unsubscribe<LookEvent>(LookHandle);
        }

        private void LookHandle(LookEvent e)
        {
            if (e.Session != Session)
            {
                return;
            }

            if (e.Direction == LookDirection.Left)
            {
                bodyTransform.SetFlipY(true);
            }

            if (e.Direction == LookDirection.Right)
            {
                bodyTransform.SetFlipY(false);
            }
        }

        private void StartFire(StartFireEvent e)
        {
            isShooting = true;
        }

        private void EndFire(EndFireEvent e)
        {
            isShooting = false;
        }

        private void Update()
        {
            UpdateShooting();
        }

        private void UpdateShooting()
        {
            if(!isShooting || !shootTimer.ItIsTime)
            {
                return;
            }

            observable.Publish(new Throw7dot62Command(Guid.NewGuid(), BarrelPosition, Rotation));
            shootTimer.Restart();
        }

        private Vector2 BarrelPosition
        {
            get
            {
                return barrelTransform.position;
            }
        }

        private float Rotation
        {
            get
            {
                return transform.rotation.eulerAngles.z;
            }
            set
            {
                transform.rotation = Quaternion.Euler(0, 0, value);
            }
        }

        public string Session { get; set; }
    }
}