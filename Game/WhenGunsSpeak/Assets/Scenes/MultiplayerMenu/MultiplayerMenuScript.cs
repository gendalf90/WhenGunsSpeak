using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Multiplayer
{
    public class MultiplayerMenuScript : MonoBehaviour
    {
        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
