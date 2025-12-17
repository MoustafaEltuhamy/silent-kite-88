using UnityEngine;

namespace AGAPI.Systems
{
    public interface IPersistenceService
    {
        public void Save();
        public T Load<T>(Key fileKey, Key dataKey) where T : ISaveRecord;
        IPersistable<T> Load<T>(Key fileKey, Key DataKey, IPersistable<T> dataOwner) where T : ISaveRecord;
        public void MarkDirty<T>(Key fileKey, Key dataKey, T data) where T : ISaveRecord;
        void MarkDirty<T>(Key fileKey, Key DataKey, IPersistable<T> dataOwner) where T : ISaveRecord;
    }
}
