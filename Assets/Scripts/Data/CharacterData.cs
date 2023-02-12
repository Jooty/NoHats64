using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Character", order = 1)]
public class CharacterData : BaseData
{

    public int          baseHealth = 100;
    public bool         isMelee;
    public float        baseDamage = 25;
    public float        baseAttackSpeed = 2.0f;
    public float        baseAttackRange = 1.5f;
    public float        detectionRange = 10.0f;
    public float        baseMoveSpeed = 12.0f;
    public float        baseJumpForce = 5.0f;
    public bool         canDoubleJump = false;
    public AttackData   SignatureWeapon;
    public AttackData   PassiveAbility;
    public AttackData   UltimateAbility;
    public bool         doesFlinch;
    [Space()]

    [Header("Sounds")]
    public bool doSounds = true;
    public bool doIdleSounds = true;
    [Range(0.5f, 5.0f)] public float idleSoundsMinimumWait;
    [Range(5.0f, 10.0f)] public float idleSoundsMaximumWait;
    [Space()]

    [Header("Sound Effects")]
    // attack, damage, brutal damage, idle
    public List<AudioClip> attackAudioClips;
    public List<AudioClip> damagedAudioClips;
    public List<AudioClip> brutalDamageAudioClips;
    public List<AudioClip> IdleAudioClips;
    public AudioClip despawnSoundEffect;

}
