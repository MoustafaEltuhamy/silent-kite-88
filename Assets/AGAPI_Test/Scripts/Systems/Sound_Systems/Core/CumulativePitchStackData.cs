using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAPI.Systems
{
    public struct CumulativePitchStackData
    {
        public int StackCount;
        public float LastPlayTime;

        public CumulativePitchStackData(int stackCount, float lastPlayTime)
        {
            StackCount = stackCount;
            LastPlayTime = lastPlayTime;
        }
    }
}
