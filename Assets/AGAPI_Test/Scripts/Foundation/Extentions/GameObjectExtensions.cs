
using UnityEngine;

namespace AGAPI.Foundation
{
    public static class GameObjectExtensions
    {
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            // Try to get the component
            T component = gameObject.GetComponent<T>();

            // If the component doesn't exist, add it
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}
