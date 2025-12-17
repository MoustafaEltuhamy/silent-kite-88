using System;
using UnityEngine;
using UnityEngine.Audio;


namespace AGAPI.Systems
{
    [Serializable]
    public class SoundAudioSourceData
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField] private bool loop;
        [SerializeField] private bool playOnAwake;
        [SerializeField] private bool mute;
        [SerializeField] private bool bypassEffects;
        [SerializeField] private bool bypassListenerEffects;
        [SerializeField] private bool bypassReverbZones;
        [SerializeField] private int priority = 128;
        [SerializeField] private float volume = 1f;
        [SerializeField] private float pitch = 1f;
        [SerializeField] private float panStereo;
        [SerializeField] private float spatialBlend;
        [SerializeField] private float reverbZoneMix = 1f;
        [SerializeField] private float dopplerLevel = 1f;
        [SerializeField] private float spread;
        [SerializeField] private float minDistance = 1f;
        [SerializeField] private float maxDistance = 500f;
        [SerializeField] private bool ignoreListenerVolume;
        [SerializeField] private bool ignoreListenerPause;
        [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;




        public AudioClip Clip => clip;
        public AudioMixerGroup MixerGroup => mixerGroup;
        public bool Loop => loop;
        public bool PlayOnAwake => playOnAwake;
        public bool Mute => mute;
        public bool BypassEffects => bypassEffects;
        public bool BypassListenerEffects => bypassListenerEffects;
        public bool BypassReverbZones => bypassReverbZones;
        public int Priority => priority;
        public float Volume => volume;
        public float Pitch => pitch;
        public float PanStereo => panStereo;
        public float SpatialBlend => spatialBlend;
        public float ReverbZoneMix => reverbZoneMix;
        public float DopplerLevel => dopplerLevel;
        public float Spread => spread;
        public float MinDistance => minDistance;
        public float MaxDistance => maxDistance;
        public bool IgnoreListenerVolume => ignoreListenerVolume;
        public bool IgnoreListenerPause => ignoreListenerPause;
        public AudioRolloffMode RolloffMode => rolloffMode;

    }
}
