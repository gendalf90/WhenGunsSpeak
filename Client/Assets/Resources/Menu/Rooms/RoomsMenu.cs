using Rooms;
using UnityEngine.UI;
using System.Linq;
using Messages;
using UnityEngine.SceneManagement;
using System;

namespace MenuOfRooms
{
    class RoomsMenu : MessageHandlerMonoBehaviour
    {
        private RoomsList list;
        private Button connectButton;
        private Button createButton;
        private Button backButton;

        protected override void Awake()
        {
            base.Awake();

            list = FindObjectOfType<RoomsList>();
            connectButton = FindObjectsOfType<Button>().Single(x => x.name == "ConnectButton");
            createButton = FindObjectsOfType<Button>().Single(x => x.name == "CreateButton");
            backButton = FindObjectsOfType<Button>().Single(x => x.name == "BackButton");
        }

        protected override void Start()
        {
            base.Start();

            connectButton.onClick.AddListener(ConnectToRoom);
            createButton.onClick.AddListener(CreateNewRoom);
            backButton.onClick.AddListener(BackToMainMenu);

            for(int i = 0; i < 100; i++)
                SendMessage(new PostRoomInfoMessage(Guid.NewGuid(), "Test" + i.ToString()));
        }

        private void Update()
        {
            UpdateSelectedRoom();
        }

        private void UpdateSelectedRoom()
        {
            var selectedRoomInfo = GetSelectedRoomInfo();

            if (selectedRoomInfo != null)
            {
                connectButton.interactable = true;
            }
            else
            {
                connectButton.interactable = false;
            }
        }

        protected override void OnMessageHandle(MessageEventArgs args)
        {
            args.Message.As<AllRoomsMessage>().Do(message =>
            {
                var messageRoomIds = message.Rooms.Select(room => room.Id);
                var listRoomIds = list.Items.Select(item => item.Info.Id);
                var newIds = messageRoomIds.Except(listRoomIds);
                var newRooms = message.Rooms.Join(newIds, room => room.Id, newId => newId, (room, newId) => room);

                foreach(var newRoom in newRooms)
                {
                    var newItem = list.Add();
                    newItem.Text = newRoom.Description;
                    newItem.Info = newRoom;
                }

                var oldRoomIds = listRoomIds.Except(messageRoomIds);
                var oldItems = list.Items.Join(oldRoomIds, item => item.Info.Id, oldId => oldId, (item, oldId) => item);

                foreach(var oldItem in oldItems)
                {
                    Destroy(oldItem.gameObject);
                }
            });
        }

        public IRoomInfo GetSelectedRoomInfo()
        {
            var selected = list.Items.FirstOrDefault(x => x.IsChanged);

            if (selected != null)
            {
                return selected.Info;
            }

            return null;
        }

        public void CreateNewRoom()
        {
            //SceneManager.LoadScene("CreateRoomMenu", LoadSceneMode.Single);
        }

        public void ConnectToRoom()
        {
            var selectedRoomInfo = GetSelectedRoomInfo();

            if (selectedRoomInfo != null)
            {
                //SceneManager.LoadScene("Test", LoadSceneMode.Single);
            }
        }

        public void BackToMainMenu()
        {
            //SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}