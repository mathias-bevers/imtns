using System.Collections.Generic;
using UnityEngine;

namespace CleanRoom.Utils
{
    public static class ExtensionMethods
    {
        public static string ToBackingField(this string source) => string.Concat('<', source, '>', "k__BackingField");

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

        public static T GetRandomElement<T>(this IList<T> collection) => collection[Random.Range(0, collection.Count)];
        
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (ReferenceEquals(null, collection))
            {
                return true;
            }

            return collection.Count == 0;
        }

        public static Vector2 GetRandomPointInBounds(this Bounds bounds)
        {
            float minX = bounds.size.x * -0.5f;
            float minY = bounds.size.y * -0.5f;

            float x = Random.Range(minX, -minX);
            float y = Random.Range(minY, -minY);

            return new Vector2(x, y);
        }
    }
}