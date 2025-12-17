using System;
using System.Collections.Generic;
using System.IO;
using AGAPI.Foundation;

namespace AGAPI.Systems
{
    public class PersistenceService_StringEnv_StringPay : IPersistenceService, IDisposable
    {

        private Dictionary<Key, bool> _loadedFiles = new Dictionary<Key, bool>();
        private FilesCollection<string> _filesCollection = new FilesCollection<string>();


        private readonly string _saveRootDirectory;
        private readonly string _saveFileExtension;
        private readonly IEnvelopeHandler<string> _IEnvelopeHandler;
        private readonly ISerializer<string> _ISerializer;
        private readonly List<Key> _preLoadFilesKeys = new List<Key>();
        private readonly SessionEvents _sessionEvents;

        public PersistenceService_StringEnv_StringPay(
            string saveRootDirectory,
            string saveFileExtension,
            IEnvelopeHandler<string> envelopeHandler,
            ISerializer<string> serializer,
            SessionEvents sessionEvents)
        {
            _saveRootDirectory = saveRootDirectory;
            _saveFileExtension = saveFileExtension;
            _IEnvelopeHandler = envelopeHandler;
            _ISerializer = serializer;
            _sessionEvents = sessionEvents;

            Initialize();
        }

        void Initialize()
        {
            SubescripeEvents();
            PreLoadFiles();
        }

        void IDisposable.Dispose()
        {
            UnsubescripeEvents();
        }


        //--------Public API's/ IPersistenceService Implementation-------
        public T Load<T>(Key fileKey, Key DataKey) where T : ISaveRecord
        {
            LazyPersistanceCollectionLoad(fileKey);

            if (_filesCollection.Files.TryGetValue(fileKey.ToString(), out var persistanceFile))
            {
                if (persistanceFile.Envelopes.TryGetValue(DataKey.ToString(), out var envelope))
                {
                    T record = _IEnvelopeHandler.GetPayload<T>(envelope);
                    return record;
                }
            }

            return default;
        }

        public IPersistable<T> Load<T>(Key fileKey, Key DataKey, IPersistable<T> dataOwner) where T : ISaveRecord
        {
            T record = Load<T>(fileKey, DataKey);
            dataOwner.LoadRecord(record);
            return dataOwner;
        }

        public void MarkDirty<T>(Key fileKey, Key DataKey, T data) where T : ISaveRecord
        {
            LazyPersistanceCollectionLoad(fileKey);

            if (!_filesCollection.Files.TryGetValue(fileKey.ToString(), out var persistanceFile))
            {
                persistanceFile = new PersistanceFile<string>();
                _filesCollection.Files[fileKey.ToString()] = persistanceFile;
            }

            var envelope = _IEnvelopeHandler.CreateEnvelope(DataKey, data);
            persistanceFile.Envelopes[DataKey.ToString()] = envelope;
        }

        public void MarkDirty<T>(Key fileKey, Key DataKey, IPersistable<T> dataOwner) where T : ISaveRecord
        {
            T record = dataOwner.GetRecordSnapshot();
            MarkDirty<T>(fileKey, DataKey, record);
        }

        public void Save()
        {
            // Ensure root directory exists
            if (!Directory.Exists(_saveRootDirectory))
            {
                Directory.CreateDirectory(_saveRootDirectory);
            }

            foreach (var fileEntry in _filesCollection.Files)
            {
                string filePath = GetFilePath(new Key(fileEntry.Key));
                var persistanceFile = fileEntry.Value;
                string serializedContent = _ISerializer.Serialize(persistanceFile);
                File.WriteAllText(filePath, serializedContent);
            }
        }


        //------------ private methodes------------
        private void PreLoadFiles()
        {
            foreach (var fileKey in _preLoadFilesKeys)
            {
                LazyPersistanceCollectionLoad(fileKey);
            }
        }

        private void LazyPersistanceCollectionLoad(Key fileKey)
        {
            if (!_loadedFiles.ContainsKey(fileKey) || !_loadedFiles[fileKey])
            {
                string filePath = GetFilePath(fileKey);
                if (File.Exists(filePath))
                {
                    string fileContent = File.ReadAllText(filePath);
                    var persistanceCollection = _ISerializer.Deserialize<PersistanceFile<string>>(fileContent);
                    _filesCollection.Files[fileKey.ToString()] = persistanceCollection;
                }
                _loadedFiles[fileKey] = true;
            }
        }

        private string GetFilePath(Key fileKey)
        {
            return Path.Combine(_saveRootDirectory, fileKey.ToString() + _saveFileExtension);
        }

        void SubescripeEvents()
        {
            _sessionEvents.Subscribe<SessionEvents.OnApplicationPause>(OnApplicationPause);
            _sessionEvents.Subscribe<SessionEvents.OnApplicationQuit>(OnApplicationQuit);
        }
        void UnsubescripeEvents()
        {
            _sessionEvents.Unsubscribe<SessionEvents.OnApplicationPause>(OnApplicationPause);
            _sessionEvents.Unsubscribe<SessionEvents.OnApplicationQuit>(OnApplicationQuit);
        }

        void OnApplicationPause(SessionEvents.OnApplicationPause pause)
        {
            Save();
        }
        void OnApplicationQuit(SessionEvents.OnApplicationQuit quit)
        {
            Save();
        }
    }
}
