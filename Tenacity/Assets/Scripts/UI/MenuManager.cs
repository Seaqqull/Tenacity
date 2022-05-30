using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.UI
{
    public class MenuManager : Base.SingleBehaviour<MenuManager>
    {
        /*
         * Contains menu fields that can be showed or hidden
        */
        [SerializeField] private List<Menu> _menus;

        private Stack<Menu> _openMenus;
        private Transform _transform;


        protected override void Awake()
        {
            base.Awake();

            _openMenus = new Stack<Menu>();
            _transform = transform;
        }


        private T GetPrefab<T>() where T : Menu
        {
            foreach (var menuItem in _menus)
            {
                var prefab = menuItem as T;
                if (prefab != null) return prefab;
            }

            throw new MissingReferenceException("Prefab of type (" + typeof(T) + ") not found.");
        }


        public void CloseAllMenus()
        {
            while(_openMenus.Count != 0)
            {
                var currentMenu = _openMenus.Peek();
                currentMenu.OnBackAction();
            }
        }

        public void CloseTopMenu()
        {
            var instance = _openMenus.Pop();

            if (instance.DestroyOnClose)
                Destroy(instance.gameObject);
            else
                instance.gameObject.SetActive(false);

            // Re-activate top menu
            // If a re-activated menu is an overlay we need to activate the menu under it
            foreach (var menu in _openMenus)
            {
                menu.gameObject.SetActive(true);

                if (menu.DisableBelowMenus) break;
            }
        }

        public void CreateInstance<T>() where T : Menu
        {
            var newMenu = GetPrefab<T>();

            Instantiate(newMenu, _transform);
        }

        public void CloseMenu(Menu menu)
        {
            if (_openMenus.Count == 0)
            {
                Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
                return;
            }

            if (_openMenus.Peek() != menu)
            {
                Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
                return;
            }

            CloseTopMenu();
        }

        public void OpenMenu(Menu instance)
        {
            // De-activate top menu
            if (_openMenus.Count > 0)
            {
			    if (instance.DisableBelowMenus)
			    {
				    foreach (var menu in _openMenus)
				    {
					    menu.gameObject.SetActive(false);

					    if (menu.DisableBelowMenus)
						    break;
				    }
			    }

                var topCanvas = instance.GetComponent<Canvas>();
                var previousCanvas = _openMenus.Peek().GetComponent<Canvas>();
			    topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
            }

            _openMenus.Push(instance);
        }
        
    }
}
