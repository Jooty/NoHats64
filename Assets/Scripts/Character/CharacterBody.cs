using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBody : MonoBehaviour
{

    // locals
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterEffects characterEffects;

    public void SetupBody()
    {
        this.animator         = this.GetComponent<Animator>();
        this.characterEffects = this.GetComponent<CharacterEffects>();
    }

}
