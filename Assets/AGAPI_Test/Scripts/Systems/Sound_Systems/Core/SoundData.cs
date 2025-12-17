using System;
using UnityEngine;

namespace AGAPI.Systems
{
    [Serializable]
    public class SoundData
    {
        [SerializeField] private bool frequentSound;
        [SerializeField] private float minSoundPlayInterval;
        [SerializeField] private PitchModes pitchMode;
        [SerializeField] private float stackPitchResetInterval = .2f;

        [Header("Stack Pitch")]
        [SerializeField] private bool hasCustomStackPitchValues;
        [SerializeField] private float stackPitchStartValue;
        [SerializeField] private float stackPitchEndValue;
        [SerializeField] private float stackPitchIncrementValue;

        [Header("AudioSourceData")]
        [SerializeField] SoundAudioSourceData soundAudioSourceData;


        public bool FrequentSound => frequentSound;
        public float MinSoundPlayInterval => minSoundPlayInterval;
        public PitchModes PitchMode => pitchMode;
        public float StackPitchResetInterval => stackPitchResetInterval;
        public bool HasCustomStackPitchValues => hasCustomStackPitchValues;
        public float StackPitchStartValue => stackPitchStartValue;
        public float StackPitchEndValue => stackPitchEndValue;
        public float StackPitchIncrementValue => stackPitchIncrementValue;
        public SoundAudioSourceData SoundAudioSourceData => soundAudioSourceData;
    }
}
