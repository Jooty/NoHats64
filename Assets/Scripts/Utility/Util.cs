using System;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = 0; i < list.Count; i++)
        {
            int rnd = random.Next(i + 1);

            T value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }
    
    /// <summary>
    /// Normalizes a value between it's minimum and maxiumum range to a value between 0-1.
    /// </summary>
    /// <param name="value">The dependant</param>
    /// <param name="min">The minimum value the dependant can achieve.</param>
    /// <param name="max">The maximum value the dependant can achieve.</param>
    /// <returns></returns>
    public static float NormalizeValue(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    /// <summary>
    /// Convert a number range into a new number range, maintaining the ratio. Returns the new ratio'd value;
    /// </summary>
    /// <param name="value">The value between oMin and oMax.</param>
    /// <param name="oMin">The minimum value of the first range.</param>
    /// <param name="oMax">The maximum value of the first range.</param>
    /// <param name="nMin">The minimum value of the second range.</param>
    /// <param name="nmax">The maxmimum value of the second range.</param>
    /// <returns></returns>
    public static float ConvertNumberRangeRatio(float value, float oMin, float oMax, float nMin, float nMax)
    {
        return ((value - oMin) / (oMax - oMin)) * (nMax - nMin) + nMin;
    }

}