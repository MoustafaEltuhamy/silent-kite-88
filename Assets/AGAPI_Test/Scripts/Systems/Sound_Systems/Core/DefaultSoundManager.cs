using System;
using System.Collections.Generic;
using AGAPI.Foundation;
using UnityEngine;
using UnityEngine.Audio;

namespace AGAPI.Systems
{
    public class DefaultSoundManager : ISoundManager
    {
        // Aliases; 
        private static readonly Key FileKey = PresistanceKeys.Audio.FileKey;
        private static readonly Key SoundDataKey = PresistanceKeys.Audio.SoundDataKey;

        public bool IsActive => _soundState == null ? false : _soundState.IsActive;
        private readonly SoundGeneralConfig _soundGeneralConfig;
        private readonly IPersistenceService _persistenceService;
        private readonly IPoolingManager _iPoolingManager;
        private readonly IPoolableHandlesProvider _poolableHandlesProvider;
        private readonly HashSet<AudioMixerGroup> _audioMixers = new();
        private readonly Dictionary<SoundData, CumulativePitchStackData> _cumulativePitchStackDataCollection = new();
        private readonly Dictionary<SoundData, float> _soundDataLastPlayedTimeCollection = new();
        private readonly LinkedList<SoundEmitter> _activeSoundEmitters = new();
        private readonly LinkedList<LinkedListNode<SoundEmitter>> _frequentSoundEmittersNodes = new();
        private SoundState _soundState;


        public DefaultSoundManager(IPoolingManager poolingManager, SoundGeneralConfig soundGeneralConfig, IPersistenceService persistenceService)
        {
            _iPoolingManager = poolingManager;
            _poolableHandlesProvider = _iPoolingManager.DefaultHandelsProvider;
            _soundGeneralConfig = soundGeneralConfig;
            _persistenceService = persistenceService;

            Initialize();
        }

        void Initialize()
        {
            _soundState = (SoundState)_persistenceService.Load(FileKey, SoundDataKey, new SoundState());
        }

        public SoundBuilder CreateSoundBuilder() => new SoundBuilder(this);

        public bool IsSoundIntervalPassed(SoundData soundData)
        {
            var currentTime = Time.time;
            var lastTime = _soundDataLastPlayedTimeCollection.GetValueOrDefault(soundData, 0);
            if (currentTime - lastTime < soundData.MinSoundPlayInterval)
                return false;

            _soundDataLastPlayedTimeCollection[soundData] = currentTime;
            return true;
        }

        public SoundEmitter GetEmitter(SoundData soundData)
        {
            RegisterMixer(soundData.SoundAudioSourceData.MixerGroup);

            var handle = _poolableHandlesProvider.GetHandle(_soundGeneralConfig.SoundEmitterPrefab);
            var emitter = _iPoolingManager.GetPooledObject(
                handle,
                Vector3.zero,
                Quaternion.identity);
            emitter.Config(this);

            TryStopEmitterIfNeeded();

            emitter.Node = _activeSoundEmitters.AddLast(emitter);

            if (soundData.FrequentSound)
            {
                _frequentSoundEmittersNodes.AddLast(emitter.Node);
            }

            return emitter;
        }

        public void SwitchToggle()
        {
            SetActive(!_soundState.IsActive);
        }
        public void SetToggle(bool active)
        {
            SetActive(active);
        }

        public void OnEmitterFinished(SoundEmitter emitter, SoundData soundData)
        {
            _activeSoundEmitters.Remove(emitter);
            if (soundData.FrequentSound)
                _frequentSoundEmittersNodes.Remove(emitter.Node);
        }

        public float GetRandomPitchIncrement()
        {
            var min = _soundGeneralConfig.RandomPitchIncrementMin;
            var max = _soundGeneralConfig.RandomPitchIncrementMax;
            return UnityEngine.Random.Range(min, max);
        }

        public float GetCumulativeStackPitchValue(SoundData soundData)
        {
            bool useCustomValues = soundData.HasCustomStackPitchValues;
            var min = useCustomValues ? soundData.StackPitchStartValue : _soundGeneralConfig.StackPitchStartValue;
            var max = useCustomValues ? soundData.StackPitchEndValue : _soundGeneralConfig.StackPitchEndValue;
            var increment = useCustomValues ? soundData.StackPitchIncrementValue : _soundGeneralConfig.StackPitchIncrementValue;

            var currentTime = Time.time;
            var stackData = _cumulativePitchStackDataCollection.GetValueOrDefault(soundData, new CumulativePitchStackData(0, currentTime));

            var timeSinceLastPlay = currentTime - stackData.LastPlayTime;
            if (timeSinceLastPlay > soundData.StackPitchResetInterval)
            {
                stackData.StackCount = 0;
            }
            stackData.StackCount++;
            stackData.LastPlayTime = currentTime;

            var pitch = Mathf.Clamp(min + (increment * stackData.StackCount), min, max);

            // IMPORTANT: ensure the updated value is saved back
            _cumulativePitchStackDataCollection[soundData] = stackData;

            return pitch;
        }

        private void RegisterMixer(AudioMixerGroup mixerGroup)
        {
            // If the mixer group is new, update its mute state.
            if (_audioMixers.Add(mixerGroup))
            {
                AudioMixerUtils.UpdateMixerMute(_soundState.IsActive, mixerGroup);
            }
        }

        private void TryStopEmitterIfNeeded()
        {
            // If we have reached the maximum number of sound emitter instances...
            if (_activeSoundEmitters.Count < _soundGeneralConfig.MaxSoundEmitterInstances)
                return;

            // Choose which emitter to stop:
            // If there are more frequent sounds than the configured minimum, stop the earliest frequent sound emitter.
            // Otherwise, stop the very first active emitter.
            var nodeToStop = _frequentSoundEmittersNodes.Count > _soundGeneralConfig.MinInstancesAssignedToFrequentSounds
                ? _frequentSoundEmittersNodes.First.Value
                : _activeSoundEmitters.First;

            nodeToStop.Value.Stop();
        }

        private void SetActive(bool active)
        {
            _soundState.SetActive(active);
            _persistenceService.MarkDirty(FileKey, SoundDataKey, _soundState);
            foreach (var mixerGroup in _audioMixers)
            {
                AudioMixerUtils.UpdateMixerMute(active, mixerGroup);
            }
        }

    }
}
