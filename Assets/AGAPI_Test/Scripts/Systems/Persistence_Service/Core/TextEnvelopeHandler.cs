using System;
using AGAPI.Foundation;
using UnityEngine;

namespace AGAPI.Systems
{
    public class TextEnvelopeHandler : IEnvelopeHandler<string>
    {


        private readonly ISerializer<string> _ISerializer;

        public TextEnvelopeHandler(ISerializer<string> serializer)
        {
            _ISerializer = serializer;
        }

        public Envelope<string> CreateEnvelope<T>(Key dataKey, T payload) where T : ISaveRecord
        {
            var envelope = new TextEnvelope
            {
                dataKey = dataKey.ToString(),
                createdUtc = DateTimeOffset.UtcNow,
                payload = _ISerializer.Serialize(payload)
            };
            return envelope;
        }

        public T GetPayload<T>(Envelope<string> envelope) where T : ISaveRecord
        {
            return _ISerializer.Deserialize<T>(envelope.payload);
        }
    }
}
