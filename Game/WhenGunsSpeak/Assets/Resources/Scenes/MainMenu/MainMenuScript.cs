using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Main
{
    public class MainMenuScript : MonoBehaviour
    {
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
