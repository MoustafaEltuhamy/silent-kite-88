using System;
using UnityEngine;

namespace AGAPI.Systems
{
    [Serializable]
    public abstract class PrefabPoolableConfig<T> where T : MonoBehaviour, IPoolable<T>
    {
        public T Poolable => poolable;
        [SerializeField] private T poolable;

        public int PrewarmCount => prewarmCount;
        [SerializeField] int prewarmCount = 1;
    }

}