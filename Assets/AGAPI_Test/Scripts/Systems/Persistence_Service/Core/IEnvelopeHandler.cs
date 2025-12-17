using UnityEngine;

namespace AGAPI.Systems
{
    public interface IEnvelopeHandler<TPayloadSerializer>
    {
        T GetPayload<T>(Envelope<TPayloadSerializer> envelope) where T : ISaveRecord;
        Envelope<TPayloadSerializer> CreateEnvelope<T>(Key dataKey, T payload) where T : ISaveRecord;
    }
}
