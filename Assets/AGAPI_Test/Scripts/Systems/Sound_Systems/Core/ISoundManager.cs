using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Systems
{
    public interface ISoundManager
    {
        public bool IsActive { get; }
        SoundBuilder CreateSoundBuilder();
        SoundEmitter GetEmitter(SoundData soundData);
        void SwitchToggle();
        void SetToggle(bool active);
        void OnEmitterFinished(SoundEmitter emitter, SoundData soundData);

        float GetRandomPitchIncrement();
        float GetCumulativeStackPitchValue(SoundData soundData);
        bool IsSoundIntervalPassed(SoundData soundData);

    }
}
