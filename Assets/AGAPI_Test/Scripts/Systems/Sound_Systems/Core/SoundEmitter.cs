using System.Collections;
using System.Collections.Generic;
using AGAPI.Foundation;
using UnityEngine;

namespace AGAPI.Systems
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour, IPoolable<SoundEmitter>
    {
        public ObjectPool<SoundEmitter> Pool { get; set; }
        public LinkedListNode<SoundEmitter> Node { get; set; }


        private SoundData _soundData;
        private ISoundManager _iSoundManager;
        private AudioSource _audioSource;
        private Coroutine _playingCoroutine;


        public void Config(ISoundManager iSoundManager)
        {
            _iSoundManager = iSoundManager;
        }

        void Awake()
        {
            _audioSource = gameObject.GetOrAdd<AudioSource>();
        }

        public void Initialize(SoundData soundData)
        {
            _soundData = soundData;

            _audioSource.clip = soundData.SoundAudioSourceData.Clip;
            _audioSource.outputAudioMixerGroup = soundData.SoundAudioSourceData.MixerGroup;
            _audioSource.loop = soundData.SoundAudioSourceData.Loop;
            _audioSource.playOnAwake = soundData.SoundAudioSourceData.PlayOnAwake;

            _audioSource.mute = soundData.SoundAudioSourceData.Mute;
            _audioSource.bypassEffects = soundData.SoundAudioSourceData.BypassEffects;
            _audioSource.bypassListenerEffects = soundData.SoundAudioSourceData.BypassListenerEffects;
            _audioSource.bypassReverbZones = soundData.SoundAudioSourceData.BypassReverbZones;

            _audioSource.priority = soundData.SoundAudioSourceData.Priority;
            _audioSource.volume = soundData.SoundAudioSourceData.Volume;
            _audioSource.pitch = soundData.SoundAudioSourceData.Pitch;
            _audioSource.panStereo = soundData.SoundAudioSourceData.PanStereo;
            _audioSource.spatialBlend = soundData.SoundAudioSourceData.SpatialBlend;
            _audioSource.reverbZoneMix = soundData.SoundAudioSourceData.ReverbZoneMix;
            _audioSource.dopplerLevel = soundData.SoundAudioSourceData.DopplerLevel;
            _audioSource.spread = soundData.SoundAudioSourceData.Spread;

            _audioSource.minDistance = soundData.SoundAudioSourceData.MinDistance;
            _audioSource.maxDistance = soundData.SoundAudioSourceData.MaxDistance;

            _audioSource.ignoreListenerVolume = soundData.SoundAudioSourceData.IgnoreListenerVolume;
            _audioSource.ignoreListenerPause = soundData.SoundAudioSourceData.IgnoreListenerPause;

            _audioSource.rolloffMode = soundData.SoundAudioSourceData.RolloffMode;
        }

        public void Play()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
            }

            _audioSource.Play();
            _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            Stop();
        }

        public void Stop()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _playingCoroutine = null;
            }

            _audioSource.Stop();
            Pool.ReturnObject(this);
        }

        public void WithRandomPitch(float pitchIncrement)
        {
            _audioSource.pitch += pitchIncrement;
        }

        public void WithStackedPitch(float stackedPitch)
        {
            _audioSource.pitch = stackedPitch;
        }

        void IPoolable<SoundEmitter>.OnGetFromPool()
        {
        }
        void IPoolable<SoundEmitter>.OnReturnToPool()
        {
            _iSoundManager.OnEmitterFinished(this, _soundData);
        }
    }
}
