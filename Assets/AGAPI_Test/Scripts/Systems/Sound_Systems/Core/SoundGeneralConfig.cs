
using UnityEngine;

namespace AGAPI.Systems
{
    [CreateAssetMenu(fileName = "SoundGeneralConfig", menuName = SystemsCreateAssetsMenuPaths.Root + "SoundGeneralConfig", order = 1)]
    public class SoundGeneralConfig : ScriptableObject
    {

        [Header("Pooling Config")]
        [SerializeField] SoundEmitter soundEmitterPrefab;
        [SerializeField] int prewarmPoolSize = 10;

        [Header("Emitters Instances")]
        [SerializeField] int maxSoundEmitterInstances = 10;
        [SerializeField] int minInstancesAssignedToFrequentSounds = 5;

        [Header("Random Pitch")]
        [SerializeField] float randomPitchIncrementMin = -0.05f;
        [SerializeField] float randomPitchIncrementMax = 0.05f;

        [Header("Stack Pitch")]
        [SerializeField] private float stackPitchStartValue;
        [SerializeField] private float stackPitchEndValue;
        [SerializeField] private float stackPitchIncrementValue;




        public SoundEmitter SoundEmitterPrefab => soundEmitterPrefab;
        public int PrewarmPoolSize => prewarmPoolSize;
        public int MaxSoundEmitterInstances => maxSoundEmitterInstances;
        public int MinInstancesAssignedToFrequentSounds => minInstancesAssignedToFrequentSounds;
        public float RandomPitchIncrementMin => randomPitchIncrementMin;
        public float RandomPitchIncrementMax => randomPitchIncrementMax;
        public float StackPitchStartValue => stackPitchStartValue;
        public float StackPitchEndValue => stackPitchEndValue;
        public float StackPitchIncrementValue => stackPitchIncrementValue;
    }
}
