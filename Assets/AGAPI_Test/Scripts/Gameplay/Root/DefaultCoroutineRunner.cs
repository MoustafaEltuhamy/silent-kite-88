using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AGAPI.Gameplay
{
    public class DefaultCoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public Coroutine Run(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void Stop(Coroutine routine)
        {
            StopCoroutine(routine);
        }
    }
}
