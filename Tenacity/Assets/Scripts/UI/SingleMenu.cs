using Tenacity.Managers;
using UnityEngine;


namespace Tenacity.UI
{
    public class SingleMenu<T> : Menu where T : SingleMenu<T>
    {
        public static T Instance 
        { 
            get; private set;
        }


        protected override void Awake()
        {
            base.Awake();

            Instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }


        protected static void Open()
        {
            if (Instance is null)
                MenuManager.Instance.CreateInstance<T>();
            else
                Instance.GameObject.SetActive(true);

            MenuManager.Instance.OpenMenu(Instance);
        }

        protected static void Close()
        {
            if (Instance is null)
            {
                Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", typeof(T));
                return;
            }

            MenuManager.Instance.CloseMenu(Instance);
        }


        public static void Show()
        {
            Open();
        }

        public static void Hide()
        {
            Close();
        }


        public override void OnBackAction()
        {
            Close();
        }

    }
}