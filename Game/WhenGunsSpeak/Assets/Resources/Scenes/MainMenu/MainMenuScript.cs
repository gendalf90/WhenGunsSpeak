using Configuration;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityRandom = UnityEngine.Random;

namespace Menu.Main
{
    class MainMenuScript : MonoBehaviour
    {
        private Parameters parameters;

        private void Awake()
        {
            parameters = FindObjectOfType<Parameters>();

            parameters.Set("Login", "test_login_" + DateTime.Now.ToString("mm_ss"));
            parameters.Set("RoomsServiceAddress", new Uri("http://localhost:50557"));
            parameters.Set("MessagesListeningPort", new IPEndPoint(IPAddress.Any, UnityRandom.Range(50000, 60000)));
            parameters.Set("NatFuckerAddress", new IPEndPoint(IPAddress.Loopback, 39748));
            parameters.Set("NatFuckingPeriod", TimeSpan.FromSeconds(2));
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
