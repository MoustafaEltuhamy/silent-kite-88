using UnityEngine;

namespace AGAPI.Systems
{
    public class PoolableHandle<T> where T : MonoBehaviour, IPoolable<T>
    {
        public readonly T Prefab;
        public readonly int Key;

        public PoolableHandle(T prefab, int key)
        {
            this.Prefab = prefab;
            this.Key = key;
        }
    }
}
