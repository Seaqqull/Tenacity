using UnityScenes = UnityEngine.SceneManagement;


namespace Tenacity.Managers
{
    public class SceneManager : Base.SingleBehaviour<SceneManager>
    {
        public void LoadMainMenu()
        {
            UnityScenes.SceneManager.LoadScene(0);
        }

        public void LoadMainGame()
        {
            UnityScenes.SceneManager.LoadScene(1);
        }
    }
}