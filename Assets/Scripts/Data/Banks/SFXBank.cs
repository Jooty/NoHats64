using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SFXBank
{

    private static readonly AudioClip[] bootsConcrete = Resources.LoadAll<AudioClip>("SFX/Footsteps/Boots/Concrete");

    public static AudioClip GetRandomFootstepSFX(SFXFootstepType type)
    {
        switch(type)
        {
            case SFXFootstepType.concrete:
                return bootsConcrete[UnityEngine.Random.Range(0, bootsConcrete.Length)];
            default:
                return null;
        }
    }

}