using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Systems
{
    public class SoundBuilder
    {
        private readonly ISoundManager _iSoundManager;
        private Vector3 _position = Vector3.zero;

        public SoundBuilder(ISoundManager iSoundManager)
        {
            _iSoundManager = iSoundManager;
        }

        public SoundBuilder WithPosition(Vector3 position)
        {
            this._position = position;
            return this;
        }

        public void Play(SoundData soundData)
        {
            if (soundData == null)
            {
                Debug.LogError("SoundData is null");
                return;
            }

            if (!_iSoundManager.IsSoundIntervalPassed(soundData))
                return;

            SoundEmitter soundEmitter = _iSoundManager.GetEmitter(soundData);
            soundEmitter.Initialize(soundData);
            soundEmitter.transform.position = _position;

            if (soundData.PitchMode == PitchModes.RandomPitch)
            {
                float randomPitchIncrement = _iSoundManager.GetRandomPitchIncrement();
                soundEmitter.WithRandomPitch(randomPitchIncrement);
            }

            if (soundData.PitchMode == PitchModes.CumulativePitch)
            { // Apply cumulative stacking pitch
                float stackedPitch = _iSoundManager.GetCumulativeStackPitchValue(soundData);
                soundEmitter.WithStackedPitch(stackedPitch);
            }

            soundEmitter.Play();
        }
    }
}
