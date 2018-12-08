using Configuration;
using Messages;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu.Multiplayer
{
    class ConnectToRoomButton : MonoBehaviour
    {
        private Button button;
        private Parameters parameters;
        private Observable observable;
        private Guid selectedRoomOwnerId;

        private void Awake()
        {
            button = GetComponent<Button>();
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
        }

        private void OnEnable()
        {
            observable.Subscribe<RoomIsSelectedEvent>(OnRoomIsSelectedHandler);
            observable.Subscribe<AllRoomsAreUnselectedEvent>(OnAllRoomsAreUnselectedHandler);
        }

        private void OnRoomIsSelectedHandler(RoomIsSelectedEvent e)
        {
            selectedRoomOwnerId = e.OwnerId;
            button.interactable = true;
        }

        private void OnAllRoomsAreUnselectedHandler(AllRoomsAreUnselectedEvent e)
        {
            button.interactable = false;
        }

        public void Connect()
        {
            parameters.SetLocal("RoomOwnerId", selectedRoomOwnerId);
            SceneManager.LoadScene("ArenaOne", LoadSceneMode.Single);
        }
    }
}
