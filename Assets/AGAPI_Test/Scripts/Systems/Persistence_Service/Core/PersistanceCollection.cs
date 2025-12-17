using System;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Systems
{

    [Serializable]
    public class FilesCollection<TPayloadSerializer>
    {
        public Dictionary<string, PersistanceFile<TPayloadSerializer>> Files = new Dictionary<string, PersistanceFile<TPayloadSerializer>>();
    }

    [Serializable]
    public class PersistanceFile<TPayloadSerializer>
    {
        public Dictionary<string, Envelope<TPayloadSerializer>> Envelopes = new Dictionary<string, Envelope<TPayloadSerializer>>();
    }

    [Serializable] public class TextFilesCollection : FilesCollection<string> { }
    [Serializable] public class BinaryFilesCollection : FilesCollection<byte[]> { }

}
