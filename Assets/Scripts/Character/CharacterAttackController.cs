using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackController : MonoBehaviour
{

    private bool canAttack;

    // locals
    private Character character;
    private AIController controller;
    private CharacterAnimator anim;

    void Awake()
    {
        this.character  = this.GetComponent<Character>();
        this.controller = this.GetComponent<AIController>();
        this.anim       = this.GetComponent<CharacterAnimator>();

        canAttack = true;
    }

    public void AttemptMeleeAttack ()
    {
        if (canAttack)
        {
            controller.playerCharacter.TakeDamage(character, character.characterData.baseDamage);
            StartCoroutine(WaitAttackTimer());

            // effects
            anim.TriggerAttack();
            character.characterBodyManager.GetBodyEffects().PlayAttackSound();
        }
        else
            return;
    }

    private IEnumerator WaitAttackTimer()
    {
        canAttack = false;

        yield return new WaitForSeconds(character.characterData.baseAttackSpeed);

        canAttack = true;

        yield return null;
    }

}
