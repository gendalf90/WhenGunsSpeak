using UnityEngine;
using Soldier;
using System;
using Head;
using Body;
using Server;
using Legs;
using Weapons;
using Hands;
using Messages;

namespace Rooms
{
    public class SkirmishRoom : ClientServerMonoBehaviour
    {
        private const float CameraZ = -10f;

        private GameObject respawnOne;

        protected override void Awake()
        {
            base.Awake();

            respawnOne = GameObject.Find("RespawnOne");
        }

        protected override void OnConnect()
        {
            if (IsServer)
            {
                var soldierId = Guid.NewGuid();
                var headId = Guid.NewGuid();
                var bodyId = Guid.NewGuid();
                var legsId = Guid.NewGuid();
                var weaponId = Guid.NewGuid();
                var firstHandId = Guid.NewGuid();
                var secondHandId = Guid.NewGuid();
                SendMessage(new CreateHandMessage(HandType.Second, HandFormType.OnForend, secondHandId, weaponId));
                SendMessage(new CreateHandMessage(HandType.First, HandFormType.OnTrigger, firstHandId, weaponId));
                SendMessage(new CreateWeaponMessage(WeaponType.AK47, weaponId, LocalSessionId, bodyId));
                SendMessage(new CreateSoldierMessage(soldierId, LocalSessionId));
                SendMessage(new CreateHeadMessage(headId, LocalSessionId, soldierId));
                SendMessage(new CreateBodyMessage(bodyId, LocalSessionId, soldierId));
                SendMessage(new CreateLegsMessage(legsId, LocalSessionId, soldierId));
                SendMessage(new SetSoldierPositionMessage(soldierId, respawnOne.transform.position));
            }
        }

        protected override void OnClientConnect(ClientConnectEventArgs args)
        {
            //var soldierId = Guid.NewGuid();
            //var headId = Guid.NewGuid();
            //var bodyId = Guid.NewGuid();
            //var legsId = Guid.NewGuid();
            //var weaponId = Guid.NewGuid();
            //var firstHandId = Guid.NewGuid();
            //var secondHandId = Guid.NewGuid();
            //SendMessage(new CreateHandMessage(HandType.Second, HandFormType.OnForend, secondHandId, weaponId));
            //SendMessage(new CreateHandMessage(HandType.First, HandFormType.OnTrigger, firstHandId, weaponId));
            //SendMessage(new CreateWeaponMessage(WeaponType.AK47, weaponId, args.ClientSession.Id, bodyId));
            //SendMessage(new CreateSoldierMessage(soldierId, args.ClientSession.Id));
            //SendMessage(new CreateHeadMessage(headId, args.ClientSession.Id, soldierId));
            //SendMessage(new CreateBodyMessage(bodyId, args.ClientSession.Id, soldierId));
            //SendMessage(new CreateLegsMessage(legsId, args.ClientSession.Id, soldierId));
            //SendMessage(new SetSoldierPositionMessage(soldierId, respawnOne.transform.position));

            //var soldierId = Guid.NewGuid();
            //var headId = Guid.NewGuid();
            //var bodyId = Guid.NewGuid();
            //SendMessage(new CreateSoldierMessage(soldierId, args.ClientSession.Id));
            //SendMessage(new CreateHeadMessage(headId, args.ClientSession.Id, soldierId));
            //SendMessage(new CreateBodyMessage(bodyId, args.ClientSession.Id, soldierId));
            //SendMessage(new SetSoldierPositionMessage(soldierId, respawnTwo.transform.position));
        }

        protected override void OnMessageHandle(MessageEventArgs args)
        {
            args.Message.As<SetCameraPositionMessage>().Do(message =>
            {
                Camera.main.transform.position = new Vector3(message.Position.x, message.Position.y, CameraZ);
            });
        }

        protected override void OnConnectFailed()
        {
            Debug.Log("Connect failed");
        }

        protected override void OnDisconnect()
        {
            Debug.Log("Disconnect");
        }
    }
}