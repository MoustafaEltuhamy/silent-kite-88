
using UnityEngine.Audio;

namespace AGAPI.Foundation
{
    public static class AudioMixerUtils
    {
        public static void UpdateMixerMute(bool active, AudioMixerGroup mixerGroup)
        {
            string parameterName = mixerGroup.name + "Volume"; // Auto-generate parameter name
            mixerGroup.audioMixer.SetFloat(parameterName, active ? 0f : -80f);
        }
    }
}
