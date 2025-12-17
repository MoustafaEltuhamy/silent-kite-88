
using UnityEngine;

namespace AGAPI.Systems
{
    public interface IPoolableHandlesProvider
    {
        PoolableHandle<T> GetHandle<T>(T prefab) where T : MonoBehaviour, IPoolable<T>;
    }
}
