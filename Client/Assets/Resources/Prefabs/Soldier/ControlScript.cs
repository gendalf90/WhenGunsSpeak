using System;
using System.Linq;
using UnityEngine;
using Server;
using Network;

namespace Soldier
{
    class ControlScript : ClientServerMonoBehaviour
    {
        private Soldier soldier;

        private bool isMoveLeftCache;
        private bool isMoveRightCache;
        private bool isJumpCache;
        private bool isFlightCache;

        protected override void Awake()
        {
            base.Awake();

            soldier = GetComponent<Soldier>();
        }

        void Update()
        {
            UpdateControlsInput();
        }

        private void UpdateControlsInput()
        {
            if (soldier.IsPlayer)
            {
                if (IsClient)
                {
                    UpdateInputCache();
                }

                if (IsServer)
                {
                    UpdateMoving(IsMoveRightInput, IsMoveLeftInput, IsJumpInput, IsFlightInput);
                }
            }
        }

        private void UpdateInputCache()
        {
            isMoveRightCache = isMoveRightCache || IsMoveRightInput;
            isMoveLeftCache = isMoveLeftCache || IsMoveLeftInput;
            isJumpCache = isJumpCache || IsJumpInput;
            isFlightCache = isFlightCache || IsFlightInput;
        }

        private void ClearInputCache()
        {
            isMoveRightCache = isMoveLeftCache = isJumpCache = isFlightCache = false;
        }

        private void UpdateMoving(bool isMoveRight, bool isMoveLeft, bool isJump, bool isFlight)
        {
            IsMoveRight = isMoveRight;
            IsMoveLeft = isMoveLeft;
            IsJump = isJump;
            IsFlight = isFlight;
        }

        private bool IsMoveRightInput
        {
            get
            {
                return Input.GetAxisRaw("Horizontal") > 0;
            }
        }

        private bool IsMoveLeftInput
        {
            get
            {
                return Input.GetAxisRaw("Horizontal") < 0;
            }
        }

        private bool IsJumpInput
        {
            get
            {
                return Input.GetKeyDown(KeyCode.Space);
            }
        }

        private bool IsFlightInput
        {
            get
            {
                return Input.GetKey(KeyCode.Mouse1);
            }
        }

        public bool IsMoveRight { get; private set; }

        public bool IsMoveLeft { get; private set; }

        public bool IsJump { get; private set; }

        public bool IsFlight { get; private set; }

        protected override void ServerReceive(ReceiveEventArgs args)
        {
            if (!soldier.IsPlayer)
            {
                var data = args.GetTypedBinaryDataOfType<InputData>().FirstOrDefault(x => x.Id == soldier.Id);

                if (data != null)
                {
                    UpdateMoving(data.IsMoveRight, data.IsMoveLeft, data.IsJump, data.IsFlight);
                }
            }
        }

        protected override void ClientSend(SendEventArgs args)
        {
            if (soldier.IsPlayer)
            {
                args.Send(new InputData
                {
                    Id = soldier.Id,
                    IsMoveRight = isMoveRightCache,
                    IsMoveLeft = isMoveLeftCache,
                    IsJump = isJumpCache,
                    IsFlight = isFlightCache
                }, GetServerIPEndPoint());

                ClearInputCache();
            }
        }

        [PacketTypeGuid("7201F0BF-927E-4A4A-974F-B202663BF8A2")]
        private class InputData : ITypedBinaryData
        {
            public Guid Id { get; set; }
            public bool IsMoveRight { get; set; }
            public bool IsMoveLeft { get; set; }
            public bool IsJump { get; set; }
            public bool IsFlight { get; set; }

            public void FromBytes(byte[] bytes)
            {
                using (var reader = new BinaryDataReader(bytes))
                {
                    Id = reader.ReadGuid();
                    IsMoveRight = reader.ReadBoolean();
                    IsMoveLeft = reader.ReadBoolean();
                    IsJump = reader.ReadBoolean();
                    IsFlight = reader.ReadBoolean();
                }
            }

            public byte[] ToBytes()
            {
                return new BinaryDataBuilder().Write(Id)
                                              .Write(IsMoveRight)
                                              .Write(IsMoveLeft)
                                              .Write(IsJump)
                                              .Write(IsFlight)
                                              .Build();
            }
        }
    }
}