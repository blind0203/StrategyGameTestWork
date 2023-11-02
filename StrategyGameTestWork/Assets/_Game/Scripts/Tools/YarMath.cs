using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YarMath
{
    public static int LoopedIncrease(this int currentValue, int maxValue) {
        return (currentValue + 1) % maxValue;
    }

    public static int LoopedIncreaseRandom(this int currentValue, int randomMinStep, int randomMaxStep, int maxValue) {
        return (currentValue + Random.Range(randomMinStep, randomMaxStep)) % maxValue;
    }
}
