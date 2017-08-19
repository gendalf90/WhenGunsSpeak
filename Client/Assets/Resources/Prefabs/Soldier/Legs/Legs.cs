//using UnityEngine;
//using Server;
//using Network;
//using Messages;
//using System;
//using System.Linq;

//namespace Legs
//{
//    public class SetLegsPositionMessage
//    {
//        public SetLegsPositionMessage(Guid parentId, Vector2 position)
//        {
//            ParentId = parentId;
//            Position = position;
//        }

//        public Guid ParentId { get; private set; }

//        public Vector2 Position { get; private set; }
//    }

//    class Legs : ClientServerMonoBehaviour
//    {
//        private Animator animator;
//        private Transform legsTransform;

//        private LegsData currentState;

//        public Legs()
//        {
//            currentState = new LegsData();
//        }

//        protected override void Awake()
//        {
//            base.Awake();

//            animator = GetComponentInChildren<Animator>();
//            legsTransform = transform.FindChild("Legs");
//        }

//        private void Update()
//        {
//            UpdateState();
//        }

//        private void UpdateState()
//        {
//            if(IsPlayer)
//            {
//                currentState.IsFlip = GetIsFlip();
//                currentState.AnimationSpeed = GetAnimationSpeed();
//            }

//            legsTransform.SetFlipX(currentState.IsFlip);
//            animator.SetBool("IsRun", currentState.AnimationSpeed != 0);
//            animator.SetFloat("Speed", currentState.AnimationSpeed);
//        }

//        private bool GetIsFlip()
//        {
//            var currentLookAt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            return currentLookAt.x < transform.position.x;
//        }

//        private float GetAnimationSpeed()
//        {
//            float result = 0f;
//            float horizontalAxisRaw = Input.GetAxisRaw("Horizontal");
            
//            if(horizontalAxisRaw > 0)
//            {
//                result = 1f;
//            }
//            else if(horizontalAxisRaw < 0)
//            {
//                result = -1f;
//            }

//            if (legsTransform.IsFlipX())
//            {
//                result *= -1f;
//            }

//            return result;
//        }

//        public bool IsPlayer
//        {
//            get
//            {
//                return LocalSessionId == PlayerSessionId;
//            }
//        }

//        public Guid PlayerSessionId { get; set; }

//        public Guid Id
//        {
//            get
//            {
//                return currentState.Id;
//            }
//            set
//            {
//                currentState.Id = value;
//            }
//        }

//        public Guid ParentId { get; set; }

//        protected override void BeforeSend(SendEventArgs args)
//        {
//            if(IsConnected && IsPlayer)
//            {
//                args.Send(currentState, GetIPEndPointsExeptThis());
//            }
//        }

//        protected override void AfterReceive(ReceiveEventArgs args)
//        {
//            if(IsConnected && !IsPlayer)
//            {
//                var state = args.GetTypedBinaryDataOfType<LegsData>()
//                                .Where(data => data.Id == Id)
//                                .FirstOrDefault();

//                if(state != null)
//                {
//                    currentState = state;
//                }
//            }
//        }

//        private Vector2 Position
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

//        protected override void OnMessageHandle(MessageEventArgs args)
//        {
//            args.Message.As<SetLegsPositionMessage>().Do(message =>
//            {
//                if (message.ParentId == ParentId)
//                {
//                    Position = message.Position;
//                }
//            });
//        }

//        [PacketTypeGuid("2B74A831-B4F7-4C76-A7F0-CF6D45EA7F2B")]
//        private class LegsData : ITypedBinaryData
//        {
//            public Guid Id { get; set; }
//            public bool IsFlip { get; set; }
//            public float AnimationSpeed { get; set; }

//            public byte[] ToBytes()
//            {
//                return new BinaryDataBuilder().Write(Id)
//                                              .Write(IsFlip)
//                                              .Write(AnimationSpeed)
//                                              .Build();
//            }

//            public void FromBytes(byte[] bytes)
//            {
//                using (var reader = new BinaryDataReader(bytes))
//                {
//                    Id = reader.ReadGuid();
//                    IsFlip = reader.ReadBoolean();
//                    AnimationSpeed = reader.ReadFloat();
//                }
//            }
//        }
//    }
//}