using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleanRoom.Menus
{
    public class MenuManager : Singleton<MenuManager>
    {
        private readonly HashSet<Menu> menus = new();

        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
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

        public T GetMenuOfType<T>(bool loadFromResources) where T : Menu
        {
            T menu = menus.FirstOrDefault(menu => menu is T) as T;
            if (!ReferenceEquals(null, menu))
            {
                return menu;
            }

            if (!loadFromResources)
            {
                return null;
            }

            foreach (Menu resource in Resources.LoadAll<Menu>(""))
            {
                if (resource is not T t)
                {
                    continue;
                }

                T loadedMenu = Instantiate(t, transform, true);
                loadedMenu.gameObject.name = "FORCED_" + t.GetType().Name;
                return loadedMenu;
            }

            return null;
        }
    }
}