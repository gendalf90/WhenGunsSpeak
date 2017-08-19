//using UnityEngine;
//using System;
//using Hands;
//using Server;
//using Network;
//using Messages;
//using Shells;
//using System.Linq;

//namespace Weapons
//{
//    class AK47magazine : ClientServerMonoBehaviour, IMagazine
//    {
//        [SerializeField]
//        private float destroyTimeInSeconds;

//        private Rigidbody2D rigidbody2d;
//        private Collider2D collider2d;

//        private float startThrowedTime;
//        private bool isThrowed;

//        protected override void Awake()
//        {
//            base.Awake();

//            rigidbody2d = GetComponent<Rigidbody2D>();
//            collider2d = GetComponentInChildren<PolygonCollider2D>();
//        }

//        private void Update()
//        {
//            if(isThrowed && IsTimeToDestroy)
//            {
//                SendMessage(new RemoveWeaponMessage(Id));
//            }
//        }

//        public Guid Id { get; set; }

//        public Guid ParentId { get; set; }

//        public MagazineType Type
//        {
//            get
//            {
//                return MagazineType.AK47;
//            }
//        }

//        private void SetThrowedState()
//        {
//            rigidbody2d.isKinematic = false;
//            collider2d.enabled = true;
//            startThrowedTime = Time.realtimeSinceStartup;
//            isThrowed = true;
//        }

//        private bool IsTimeToDestroy
//        {
//            get
//            {
//                return Time.realtimeSinceStartup - startThrowedTime > destroyTimeInSeconds;
//            }
//        }

//        public Vector2 Position
//        {
//            get
//            {
//                return transform.position;
//            }
//            set
//            {
//                transform.position = value;
//            }
//        }

//        public float Rotation
//        {
//            get
//            {
//                return transform.rotation.eulerAngles.z;
//            }
//            set
//            {
//                transform.rotation = Quaternion.Euler(0, 0, value);
//            }
//        }

//        protected override void ServerSend(SendEventArgs args)
//        {
//            if (isThrowed)
//            {
//                args.Send(new MagazinePosition
//                {
//                    Id = Id,
//                    Position = Position,
//                    Rotation = Rotation
//                }, GetClientsIPEndPoints());
//            }
//        }

//        protected override void ClientReceive(ReceiveEventArgs args)
//        {
//            var data = args.GetTypedBinaryDataOfType<MagazinePosition>().FirstOrDefault(x => x.Id == Id);

//            if (data != null)
//            {
//                isThrowed = true;
//                Position = data.Position;
//                Rotation = data.Rotation;
//            }
//        }

//        protected override void OnMessageHandle(MessageEventArgs args)
//        {
//            if (!isThrowed)
//            {
//                args.Message.As<SetMagazinePositionMessage>().Do(message =>
//                {
//                    if (message.ParentId == ParentId)
//                    {
//                        Position = message.Position;
//                        Rotation = message.Rotation;
//                        transform.SetFlipY(message.IsFlip);
//                    }
//                });

//                args.Message.As<ThrowMagazineMessage>().Do(message =>
//                {
//                    if (message.ParentId == ParentId && IsServer)
//                    {
//                        SetThrowedState();
//                    }
//                });
//            }
//        }

//        [PacketTypeGuid("7784A7E0-27D3-4A8F-98A7-176790312EC4")]
//        private class MagazinePosition : ITypedBinaryData
//        {
//            public Guid Id { get; set; }
//            public Vector2 Position { get; set; }
//            public float Rotation { get; set; }

//            public void FromBytes(byte[] bytes)
//            {
//                using (var reader = new BinaryDataReader(bytes))
//                {
//                    Id = reader.ReadGuid();
//                    Position = reader.ReadVector();
//                    Rotation = reader.ReadFloat();
//                }
//            }

//            public byte[] ToBytes()
//            {
//                return new BinaryDataBuilder().Write(Id)
//                                              .Write(Position)
//                                              .Write(Rotation)
//                                              .Build();
//            }
//        }
//    }
//}