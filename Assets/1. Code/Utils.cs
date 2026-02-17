using UnityEngine;

namespace CleanRoom
{
    public static class Utils
    {
        public static T FindComponentUp<T>(this Transform origin) where T : Component
        {
            Transform parent = origin.parent;

            while (!ReferenceEquals(null, parent))
            {
                T component = parent.GetComponent<T>();

                if (!ReferenceEquals(null, component))
                {
                    return component;
                }

                parent = parent.parent;
            }

            throw new MissingComponentException($"Could not find component<b>{typeof(T).FullName}</b> in parents");
        }
    }
}