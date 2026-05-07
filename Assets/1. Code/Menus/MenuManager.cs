using System.Collections.Generic;
using System.Linq;

namespace CleanRoom.Menus
{
    public class MenuManager : Singleton<MenuManager>
    {
        private readonly HashSet<Menu> menus = new();

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterMenu(Menu menu)
        {
            menus.Add(menu);
        }

        public void UnregisterMenu(Menu menu)
        {
            if (!menus.Contains(menu))
            {
                return;
            }

            menus.Remove(menu);
        }

        public T OpenMenu<T>() where T : Menu
        {
            T[] possibleToOpen = GetMenusOfType<T>();
            if (possibleToOpen.Length < 1)
            {
                return null;
            }

            for (int i = 0; i < possibleToOpen.Length; ++i)
            {
                T menu = possibleToOpen[i];

                if (menu.IsOpen)
                {
                    continue;
                }

                menu.Open();
                return menu;
            }

            return null;
        }

        public T CloseMenu<T>() where T : Menu
        {
            T[] possibleToClose = GetMenusOfType<T>();
            if (possibleToClose.Length < 1)
            {
                return null;
            }

            for (int i = 0; i < possibleToClose.Length; ++i)
            {
                T menu = possibleToClose[i];
                if (!menu.IsOpen)
                {
                    continue;
                }

                menu.Close();
                return menu;
            }

            return null;
        }

        private T[] GetMenusOfType<T>() where T : Menu => menus.OfType<T>().ToArray();
        public T GetMenuOfType<T>() where T : Menu => menus.FirstOrDefault(menu => menu is T) as T;
    }
}