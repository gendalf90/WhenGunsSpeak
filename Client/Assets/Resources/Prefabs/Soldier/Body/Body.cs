using UnityEngine;
using Server;
using System;
using Network;
using System.Linq;
using Messages;
using Weapons;

namespace Body
{
    public class SetBodyPositionMessage
    {
        public SetBodyPositionMessage(Guid parentId, Vector2 position)
        {
            ParentId = parentId;
            Position = position;
        }

        public Guid ParentId { get; private set; }

        public Vector2 Position { get; private set; }
    }

    class Body : ClientServerMonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Transform handsTransform;
        private Transform shoulderTransform;

        private float currentRotateAngle;
        private Vector2 currentAimPosition;

        protected override void Awake()
        {
            base.Awake();

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            handsTransform = transform.FindChild("Hands");
            shoulderTransform = transform.FindChild("Shoulder");
        }

        private void Update()
        {
            UpdatePlayer();
            UpdateWeapon();
        }

        private void UpdatePlayer()
        {
            if (IsPlayer)
            {
                currentAimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (IsServer)
                {
                    AimToPosition(currentAimPosition);
                }
            }
        }

        private void UpdateWeapon()
        {
            SendMessage(new SetWeaponPositionMessage(Id, HandsPosition, currentRotateAngle, IsFlip));
        }

        private void SetRotateAngle(float angle)
        {
            ResetHandsRotate();
            currentRotateAngle = angle;
            RotateHands();
            SetLookDirection();
        }

        private void AimToPosition(Vector2 destination)
        {
            var angle = ShoulderPosition.GetAngle(destination);
            SetRotateAngle(angle);
        }

        private void ResetHandsRotate()
        {
            handsTransform.RotateAround(ShoulderPosition, Vector3.forward, -currentRotateAngle);
            handsTransform.rotation = Quaternion.identity;
        }

        private void RotateHands()
        {
            handsTransform.RotateAround(ShoulderPosition, Vector3.forward, currentRotateAngle);
            handsTransform.rotation = Quaternion.Euler(0, 0, currentRotateAngle);
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

        private Vector2 ShoulderPosition
        {
            get
            {
                return shoulderTransform.position;
            }
        }

        private Vector2 HandsPosition
        {
            get
            {
                return handsTransform.position;
            }
        }

        public bool IsPlayer
        {
            get
            {
                return LocalSessionId == PlayerSessionId;
            }
        }

        public Guid PlayerSessionId { get; set; }

        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        private void SetLookDirection()
        {
            if (currentRotateAngle <= 90 || currentRotateAngle >= 270)
            {
                SetLookAtRight();
            }
            else
            {
                SetLookAtLeft();
            }
        }

        private bool IsFlip
        {
            get
            {
                return spriteRenderer.flipX;
            }
            set
            {
                spriteRenderer.flipX = value;
            }
        }

        private void SetLookAtLeft()
        {
            IsFlip = true;
        }

        private void SetLookAtRight()
        {
            IsFlip = false;
        }

        protected override void ClientSend(SendEventArgs args)
        {
            if (IsPlayer)
            {
                args.Send(new InputData
                {
                    Id = Id,
                    AimTo = currentAimPosition
                }, GetServerIPEndPoint());
            }
        }

        protected override void ServerSend(SendEventArgs args)
        {
            args.Send(new ServerData
            {
                Id = Id,
                Rotation = currentRotateAngle
            }, GetClientsIPEndPoints());
        }

        protected override void ClientReceive(ReceiveEventArgs args)
        {
            var data = args.GetTypedBinaryDataOfType<ServerData>().FirstOrDefault(x => x.Id == Id);

            if (data != null)
            {
                SetRotateAngle(data.Rotation);
            }
        }

        protected override void ServerReceive(ReceiveEventArgs args)
        {
            if (!IsPlayer)
            {
                var data = args.GetTypedBinaryDataOfType<InputData>().FirstOrDefault(x => x.Id == Id);

                if (data != null)
                {
                    AimToPosition(data.AimTo);
                }
            }
        }

        protected override void OnMessageHandle(MessageEventArgs args)
        {
            args.Message.As<SetBodyPositionMessage>().Do(message =>
            {
                if (message.ParentId == ParentId)
                {
                    Position = message.Position;
                }
            });
        }

        [PacketTypeGuid("79F21839-7B98-4927-ABE9-446955119861")]
        private class InputData : ITypedBinaryData
        {
            public Guid Id { get; set; }
            public Vector2 AimTo { get; set; }

            public void FromBytes(byte[] bytes)
            {
                using (var reader = new BinaryDataReader(bytes))
                {
                    Id = reader.ReadGuid();
                    AimTo = reader.ReadVector();
                }
            }

            public byte[] ToBytes()
            {
                return new BinaryDataBuilder().Write(Id)
                                              .Write(AimTo)
                                              .Build();
            }
        }

        [PacketTypeGuid("50294A34-193C-4E7A-B532-9986C78FAEC0")]
        private class ServerData : ITypedBinaryData
        {
            public Guid Id { get; set; }
            public float Rotation { get; set; }

            public byte[] ToBytes()
            {
                return new BinaryDataBuilder().Write(Id)
                                              .Write(Rotation)
                                              .Build();
            }

            public void FromBytes(byte[] bytes)
            {
                using (var reader = new BinaryDataReader(bytes))
                {
                    Id = reader.ReadGuid();
                    Rotation = reader.ReadFloat();
                }
            }
        }
    }
}