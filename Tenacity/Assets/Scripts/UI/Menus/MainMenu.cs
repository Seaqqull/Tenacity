using UnityEngine.SceneManagement;
using Tenacity.Managers;
using UnityEditor;


namespace Tenacity.UI.Menus
{
    public class MainMenu : SingleMenu<MainMenu>
    {
        private void Start()
        {
            MenuManager.Instance.OpenMenu(this);
        }


        public void OnStartAction()
        {
            MenuManager.Instance.CloseMenu(this);
            SceneManager.LoadScene(1);// Change later for dedicated class
        }

        public void OnSettingsAction()
        {
            SettingsMenu.Show();
        }
        
        public void OnCredentialsAction()
        {
            AuthorsMenu.Show();
        }


        public override void OnBackAction()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();   
#endif
        }
    }
}