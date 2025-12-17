using UnityEngine;

namespace AGAPI.Systems
{

    public class SoundDataRecord : ISaveRecord
    {
        public bool IsActive;
    }

    public class SoundState : IPersistable<SoundDataRecord>
    {
        private bool _isActive;

        // ---- Public API ----
        public bool IsActive => _isActive;

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        // ---- IPersistable ----

        public SoundDataRecord GetRecordSnapshot()
        {
            return new SoundDataRecord
            {
                IsActive = _isActive,
            };
        }

        public void LoadRecord(SoundDataRecord record)
        {
            if (record == null)
            {
                _isActive = true;
                return;
            }

            _isActive = record.IsActive;
        }
    }
}
