using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void DemoButtonClick()
        {
            SceneManager.LoadScene("DemoStage", LoadSceneMode.Single);
        }

        public void QuitButtonClick()
        {
            Application.Quit();
        }
    }
}
