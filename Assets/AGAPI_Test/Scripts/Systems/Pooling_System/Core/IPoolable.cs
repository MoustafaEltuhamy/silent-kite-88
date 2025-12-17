using UnityEngine;

namespace AGAPI.Systems
{
    public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
    {
        ObjectPool<T> Pool { get; set; }

        void OnGetFromPool();

        void OnReturnToPool();
    }
}