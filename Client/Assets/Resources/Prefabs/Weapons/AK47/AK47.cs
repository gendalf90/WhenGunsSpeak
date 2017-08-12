using UnityEngine;
using System;
using Hands;
using Server;
using Network;
using Messages;
using Shells;
using System.Linq;
using UnityRandom = UnityEngine.Random;

namespace Weapons
{
    class AK47 : ClientServerMonoBehaviour, IWeapon
    {
        [SerializeField]
        private float shotIntervalInSeconds;

        [SerializeField]
        private float reloadTimeInSeconds;

        private Transform bodyTransform;
        private Transform triggerTransform;
        private Transform forendTransform;
        private Transform barrelTransform;
        private Transform gateTransform;
        private Transform magazineTransform;

        private InputData input;
        private float lastShotTime;
        private float reloadingStartTime;
        private bool hasMagazine;

        public AK47()
        {
            input = new InputData();
        }

        protected override void Awake()
        {
            base.Awake();

            bodyTransform = transform.Find("Body");
            triggerTransform = bodyTransform.Find("Trigger");
            forendTransform = bodyTransform.Find("Forend");
            gateTransform = bodyTransform.Find("Gate");
            magazineTransform = bodyTransform.Find("Magazine");
            barrelTransform = transform.Find("Barrel");
        }

        private void CreateMagazine()
        {
            SendMessage(new CreateMagazineMessage(MagazineType.AK47, Guid.NewGuid(), Id));
        }

        private void Update()
        {
            UpdatePlayer();
            UpdateServer();
        }

        private void UpdatePlayer()
        {
            if (IsPlayer)
            {
                input.IsShoot = Input.GetKey(KeyCode.Mouse0);
                input.IsReload = Input.GetKeyDown(KeyCode.R);
            }
        }

        private void UpdateServer()
        {
            if(IsServer)
            {
                UpdateInput();
            }
        }

        private void UpdateInput()
        {
            if (!IsReloading)
            {
                if (!hasMagazine)
                {
                    CreateMagazine();
                    hasMagazine = true;
                }

                if (input.IsShoot && IsReadyToShot)
                {
                    Shot();
                }

                if (input.IsReload)
                {
                    Reload();
                }
            }
        }

        private bool IsReloading
        {
            get
            {
                return Time.realtimeSinceStartup - reloadingStartTime <= reloadTimeInSeconds;
            }
        }

        private void StartReloadingTime()
        {
            reloadingStartTime = Time.realtimeSinceStartup;
        }

        private bool IsReadyToShot
        {
            get
            {
                return Time.realtimeSinceStartup - lastShotTime > shotIntervalInSeconds;
            }
        }

        private void UpdateShotTime()
        {
            lastShotTime = Time.realtimeSinceStartup;
        }

        private void Shot()
        {
            UpdateShotTime();
            ThrowBullet();
            ThrowSleeve();
        }

        private void ThrowBullet()
        {
            SendMessage(new CreateAndThrowShellMessage(ShellType.caliber7dot62Bullet, Guid.NewGuid(), BarrelPosition, GetBulletStartRotation()));
        }

        private float GetBulletStartRotation()
        {
            float range = UnityRandom.Range(-5f, 5f);
            return RotationInDegrees + range;
        }

        private void ThrowSleeve()
        {
            SendMessage(new CreateAndThrowShellMessage(ShellType.caliber7dot62Sleeve, Guid.NewGuid(), GatePosition, GetSleeveStartRotation()));
        }

        private float GetSleeveStartRotation()
        {
            float flipModificator = bodyTransform.IsFlipY() ? -1f : 1f;
            float rotationFromRange = UnityRandom.Range(85f, 95f);
            return RotationInDegrees + (flipModificator * rotationFromRange);
        }

        private void Reload()
        {
            StartReloadingTime();
            ThrowMagazine();
            hasMagazine = false;
        }

        private void ThrowMagazine()
        {
            SendMessage(new ThrowMagazineMessage(Id));
        }

        private Vector2 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
            }
        }

        private Vector2 BarrelPosition
        {
            get
            {
                return barrelTransform.position;
            }
        }

        private Vector2 GatePosition
        {
            get
            {
                return gateTransform.position;
            }
        }

        private float RotationInDegrees
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

        public bool IsPlayer
        {
            get
            {
                return LocalSessionId == PlayerSessionId;
            }
        }

        public WeaponType Type
        {
            get
            {
                return WeaponType.AK47;
            }
        }

        public Guid PlayerSessionId { get; set; }
        
        public Guid Id
        {
            get
            {
                return input.Id;
            }
            set
            {
                input.Id = value;
            }
        }

        public Guid ParentId { get; set; }

        protected override void ClientSend(SendEventArgs args)
        {
            if (IsPlayer)
            {
                args.Send(input, GetServerIPEndPoint());
            }
        }

        protected override void ServerReceive(ReceiveEventArgs args)
        {
           args.GetTypedBinaryDataOfType<InputData>()
               .FirstOrDefault(x => x.Id == Id)
               .Do(data => input = data);
        }

        protected override void OnMessageHandle(MessageEventArgs args)
        {
            args.Message.As<SetWeaponPositionMessage>().Do(message =>
            {
                if(message.ParentId == ParentId)
                {
                    Position = message.Position;
                    RotationInDegrees = message.Rotation;
                    bodyTransform.SetFlipY(message.IsFlip);

                    SendMessage(new SetHandPositionMessage(HandType.First, Id, triggerTransform.position, RotationInDegrees, message.IsFlip));
                    SendMessage(new SetHandPositionMessage(HandType.Second, Id, forendTransform.position, RotationInDegrees, message.IsFlip));
                    SendMessage(new SetMagazinePositionMessage(Id, magazineTransform.position, RotationInDegrees, message.IsFlip));
                }
            });
        }

        [PacketTypeGuid("73A23C28-8683-4448-88C4-0569137E767D")]
        private class InputData : ITypedBinaryData
        {
            public Guid Id { get; set; }
            public bool IsShoot { get; set; }
            public bool IsReload { get; set; }

            public void FromBytes(byte[] bytes)
            {
                using (var reader = new BinaryDataReader(bytes))
                {
                    Id = reader.ReadGuid();
                    IsShoot = reader.ReadBoolean();
                    IsReload = reader.ReadBoolean();
                }
            }

            public byte[] ToBytes()
            {
                return new BinaryDataBuilder().Write(Id)
                                              .Write(IsShoot)
                                              .Write(IsReload)
                                              .Build();
            }
        }

        //[PacketTypeGuid("62EB476A-7F87-498F-8A5F-906D1CEC21B6")]
        //private class AK47Data : ITypedBinaryData
        //{
        //    public Guid Id { get; set; }

        //    public void FromBytes(byte[] bytes)
        //    {
        //        using (var reader = new BinaryDataReader(bytes))
        //        {
        //            Id = reader.ReadGuid();
        //        }
        //    }

        //    public byte[] ToBytes()
        //    {
        //        return new BinaryDataBuilder().Write(Id)
        //                                      .Build();
        //    }
        //}
    }
}