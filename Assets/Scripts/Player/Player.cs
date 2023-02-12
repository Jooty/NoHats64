using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Character 
{

    [HideInInspector] public PlayerAttackController playerAttackController;

    [Header("Cheats")]
    public bool godmode = false;
    public bool infiniteDamage = false;

    private CameraRecoil cameraRecoil;

    private void Start() 
    {
        this.cameraRecoil = FindObjectOfType<CameraRecoil>();
        this.playerAttackController = this.GetComponent<PlayerAttackController>();
    }

    public override void TakeDamage(Character source, float damage)
    {
        if (base.isDead || godmode) return;

        cameraRecoil.DoRecoil(new Vector3(50.0f, 50.0f, 50.0f));

        if (characterStats.currentHealth <= damage) 
        { 
            Die(source); 
        }
        else
        {
            characterStats.currentHealth -= damage;
        }
    }

    public override void Die(Character source) 
    {
        Debug.Log("Player is dead!");
        base.isDead = true;
    }

    public override void ChangeCharacter(string characterName)
    {
        characterBodyManager.ChangeCharacterBody(characterName);
    }

}
