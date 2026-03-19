using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom
{
    public static class Utils
    {
        public static T GetComponentInParents<T>(this Transform origin) where T : Component
        {
            Transform parent = origin.parent;

            while (!ReferenceEquals(null, parent))
            {
                if (parent.TryGetComponent(out T component))
                {
                    return component;
                }

                parent = parent.parent;
            }

            throw new MissingComponentException($"Could not find component<b>{typeof(T).FullName}</b> in parents");
        }

        public static T[] GetComponentsInAllChildren<T>(this Transform parent, List<T> components = null)
            where T : Component
        {
            components ??= new List<T>();

            for (int i = 0; i < parent.childCount; ++i)
            {
                Transform child = parent.GetChild(i);
                child.GetComponentsInAllChildren(components);

                if (child.TryGetComponent(out T component))
                {
                    components.Add(component);
                }
            }
            
            return components.ToArray();
        }

        public static void DestroyAllChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; --i)
            {
                Transform child = parent.GetChild(i);

                if (ReferenceEquals(null, child))
                {
                    continue;
                }

                Object.DestroyImmediate(child.gameObject);
            }
        }

        public static bool PercentChance(int percent)
        {
            return Random.Range(0, 101) < percent;
        }
    }
}