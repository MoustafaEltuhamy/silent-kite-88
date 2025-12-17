using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Systems
{
    public interface IPoolingManager
    {
        PoolableHandlesProvider DefaultHandelsProvider { get; }
        void CreatePrewarmObject<T>(PoolableHandle<T> poolableHandel, int prewarmCount) where T : MonoBehaviour, IPoolable<T>;
        T GetPooledObject<T>(PoolableHandle<T> poolableHandel, Vector3 position, Quaternion rotation, Transform parent = null) where T : MonoBehaviour, IPoolable<T>;
        T GetPooledObject<T>(PoolableHandle<T> poolableHandel, Transform parent = null) where T : MonoBehaviour, IPoolable<T>;
    }
}
