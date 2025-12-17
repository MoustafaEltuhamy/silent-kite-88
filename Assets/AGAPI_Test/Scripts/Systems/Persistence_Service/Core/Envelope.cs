using System;
using UnityEngine;

namespace AGAPI.Systems
{
    public class Envelope<PayloadT>
    {
        public string dataKey;
        public DateTimeOffset createdUtc;
        public PayloadT payload;      // string (readable) OR byte[] (compact)
    }

    public class TextEnvelope : Envelope<string> { } // string (readable)
    public class BinaryEnvelope : Envelope<byte[]> { } // byte[] (compact)
}
