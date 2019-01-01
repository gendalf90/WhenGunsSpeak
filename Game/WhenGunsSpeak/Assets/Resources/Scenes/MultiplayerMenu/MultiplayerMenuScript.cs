using Configuration;
using Messages;
using Server;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Multiplayer
{
    public class MultiplayerMenuScript : MonoBehaviour
    {
        private Observable observable;
        private Parameters parameters;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
        }

        private void Start()
        {
            observable.Publish(new ConnectToRoomsServiceCommand());
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void Create()
        {
            parameters.Remove("RoomOwnerId");
            SceneManager.LoadScene("ArenaOne", LoadSceneMode.Single);
        }
    }
}
