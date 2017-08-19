//using UnityEngine;
//using System;
//using System.Linq;
//using Server;
//using Messages;

//namespace Soldier
//{
//    class Head1 : MonoBehaviour
//    {
//        private Observable observable;
//        private SpriteRenderer spriteRenderer;

//        private void Awake()
//        {
//            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
//        }

//        private void Update()
//        {
//            UpdateState();
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
//            if (IsConnected && IsPlayer)
//            {
//                args.Send(currentState, GetIPEndPointsExeptThis());
//            }
//        }

//        protected override void AfterReceive(ReceiveEventArgs args)
//        {
//            if (IsConnected && !IsPlayer)
//            {
//                var state = args.GetTypedBinaryDataOfType<HeadData>()
//                                .Where(data => data.Id == Id)
//                                .FirstOrDefault();

//                if (state != null)
//                {
//                    currentState = state;
//                }
//            }
//        }

        
//    }
//}