//using UnityEngine;
//using System;
//using Messages;

//namespace Hands
//{
//    public enum HandType
//    {
//        First = 1,
//        Second = 2
//    }

//    public class SetHandPositionMessage
//    {
//        public SetHandPositionMessage(HandType type, Guid parentId, Vector2 position, float rotation, bool isFlip)
//        {
//            Type = type;
//            ParentId = parentId;
//            Position = position;
//            Rotation = rotation;
//            IsFlip = isFlip;
//        }

//        public HandType Type { get; private set; }

//        public Guid ParentId { get; private set; }

//        public Vector2 Position { get; private set; }

//        public float Rotation { get; private set; }

//        public bool IsFlip { get; set; }
//    }

//    class Hand : MessageHandlerMonoBehaviour
//    {
//        private SpriteRenderer spriteRenderer;

//        protected override void Awake()
//        {
//            base.Awake();

//            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

//        private float Rotation
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

//        private bool IsFlip
//        {
//            get
//            {
//                return spriteRenderer.flipY;
//            }
//            set
//            {
//                spriteRenderer.flipY = value;
//            }
//        }

//        public HandType Type { get; set; }

//        public HandFormType FormType { get; set; }
        
//        public Guid Id { get; set; }

//        public Guid ParentId { get; set; }

//        protected override void OnMessageHandle(MessageEventArgs args)
//        {
//            args.Message.As<SetHandPositionMessage>().Do(message =>
//            {
//                if (message.ParentId == ParentId && message.Type == Type)
//                {
//                    Position = message.Position;
//                    Rotation = message.Rotation;
//                    IsFlip = message.IsFlip;
//                }
//            });
//        }
//    }
//}