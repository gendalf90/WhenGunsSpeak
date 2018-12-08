using Configuration;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Main
{
    class MainMenuScript : MonoBehaviour
    {
        private Parameters parameters;

        private void Awake()
        {
            parameters = FindObjectOfType<Parameters>();

            parameters.SetLocal("Login", "test_login");
            parameters.SetLocal("RoomsServiceAddress", new Uri("http://localhost:50557"));
        }

        public void MultiplayerMenu()
        {
            SceneManager.LoadScene("MultiplayerMenu", LoadSceneMode.Single);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
