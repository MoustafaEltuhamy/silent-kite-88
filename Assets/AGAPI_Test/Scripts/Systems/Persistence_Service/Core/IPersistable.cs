using UnityEngine;

namespace AGAPI.Systems
{
    public interface IPersistable<T> where T : ISaveRecord
    {
        public T GetRecordSnapshot();
        public void LoadRecord(T record);
    }
}
