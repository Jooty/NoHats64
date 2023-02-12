using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{

    private Animator anim;

    int isWalkingHash,
        attackHash,
        hitHash,
        deathHash,
        overkillDeathHash,
        speedModifierHash,
        attackSpeedModifierHash;

    // locals
    private Character character;

    private void Awake()
    {
        this.character    = this.GetComponent<Character>();

        isWalkingHash           = Animator.StringToHash("isWalking");
        attackHash              = Animator.StringToHash("attackHash");
        hitHash                 = Animator.StringToHash("hit");
        deathHash               = Animator.StringToHash("death");
        overkillDeathHash       = Animator.StringToHash("overkillDeath");
        speedModifierHash       = Animator.StringToHash("speedModifier");
        attackSpeedModifierHash = Animator.StringToHash("attackSpeedModifier");
    }

    public void SetNewAnimator(Animator anim)
    {
        this.anim = anim;
    }

    public void SetWalk(bool walk)
    {
        if (!anim) return;
        anim.SetBool(isWalkingHash, walk);
    }

    public void TriggerAttack()
    {
        if (!anim) return;
        anim.SetTrigger(attackHash);
    }

    public void TriggerDamage()
    {
        if (!anim) return;
        anim.SetTrigger(hitHash);
    }

    public void TriggerDeath()
    {
        if (!anim) return;
        anim.SetTrigger(deathHash);
    }

    public void TriggerBrutalDeath()
    {
        if (!anim) return;
        anim.SetTrigger(overkillDeathHash);
    }

    public void SetSpeedModifier(float mod)
    {
        if (!anim) return;
        anim.SetFloat(speedModifierHash, mod);
    }

    public void SetAttackSpeedModifier(float mod)
    {
        if (!anim) return;
        anim.SetFloat(attackSpeedModifierHash, mod);
    }

}
