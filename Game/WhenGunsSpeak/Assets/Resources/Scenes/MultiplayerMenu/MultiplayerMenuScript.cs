using Messages;
using Server;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Menu.Multiplayer
{
    public class MultiplayerMenuScript : MonoBehaviour
    {
        private RoomsList rooms;
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            rooms = GetComponentInChildren<RoomsList>();
        }

        private void Start()
        {
            observable.Publish(new ConnectCommand());
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void Create()
        {
            SceneManager.LoadScene("ArenaOne", LoadSceneMode.Single);
        }

        public void Connect()
        {
            var selectedItem = rooms.GetSelectedItem();

            if(selectedItem == null)
            {
                return;
            }

            GlobalStorage.Parameters["RoomOwnerID"] = selectedItem.OwnerId.ToString();
            SceneManager.LoadScene("ArenaOne", LoadSceneMode.Single);
        }
    }
}
