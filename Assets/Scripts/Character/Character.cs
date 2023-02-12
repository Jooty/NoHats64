using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour 
{

    public CharacterData characterData;
    public CharacterStats characterStats;
    public Team team = Team.allied;
    public bool isDead;
    public bool isAI = true;
    public SpawnPoint spawnPoint;

    public CharacterBodyManager characterBodyManager;

    // locals
    public CharacterAnimator characterAnimator;
    private AIController aiController;

    private void Awake() 
    {
        this.characterAnimator = this.GetComponent<CharacterAnimator>();
        if (isAI)
            this.aiController = this.GetComponent<AIController>();

        characterStats = new CharacterStats(characterData);
    }

    public void SetupCharacter(CharacterData data)
    {
        characterData = data;
        ChangeCharacter(data.Name);
    }

    public virtual void TakeDamage(Character source, float damage)
    {
        if (isDead) return;

        if (characterStats.currentHealth <= damage)
        {
            // play brutal execution
            if (damage > characterStats.currentHealth + 100)
            {
                characterAnimator.TriggerBrutalDeath();
                characterBodyManager.GetBodyEffects().DoExecutedEffects();
            }
            else
            {
                characterAnimator.TriggerDeath();
                characterBodyManager.GetBodyEffects().PlayDeathSound();
            }

            Die(source);
        }
        else
        {
            if (characterData.doesFlinch)
                aiController.FlinchTimer();

            characterStats.currentHealth -= damage;

            // effects
            characterAnimator.TriggerDamage();
            characterBodyManager.GetBodyEffects().PlayDamagedEffect();

            // lower walk speed
            var newSpeed = Util.ConvertNumberRangeRatio(characterStats.currentHealth, 0, characterStats.maximumHealth, 0.0f, characterStats.moveSpeed);
            var normalizedDifference = Util.NormalizeValue(newSpeed, 0, characterStats.moveSpeed);
            characterAnimator.SetSpeedModifier(normalizedDifference);
            characterStats.moveSpeed = newSpeed;

            if (isAI)
                aiController.UpdateWalkSpeed();
        }
    }

    public virtual void Die(Character killer) 
    {
        if (isAI) aiController.Stop();

        isDead = true;
        KillLog.AddKill(new KillContext(Time.time, killer, this));

        StartCoroutine(DoDespawn());
    }

    public virtual void ChangeCharacter(string characterName)
    {
        characterBodyManager.ChangeCharacterBody(characterName);
    }

    private IEnumerator DoDespawn()
    {
        yield return new WaitForSeconds(0.1f);
        // do effects
        characterBodyManager.GetBodyEffects().DoDespawnEffects();
        Destroy(gameObject);

        yield return null;
    }

}
