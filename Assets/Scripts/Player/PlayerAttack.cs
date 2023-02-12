using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    /// <summary>
    /// The scriptable object that contains all base data value for this attack.
    /// </summary>
    public AttackData attackData;
    /// <summary>
    /// The object that contains all current stats for this attack.
    /// </summary>
    public AttackStats attackStats;
    [HideInInspector] public Player player;
    [HideInInspector] public PlayerAttackFunctionality attackFunction;

    [HideInInspector] public int currentMagazineSize;
    [HideInInspector] public float currentReloadTime;

    // locals
    [HideInInspector] public Animator animator;
    [HideInInspector] public AudioSource aud;

    private void Awake()
    {
        this.player         = gameObject.GetComponentInParent<Player>();
        this.attackFunction = GetComponent<PlayerAttackFunctionality>();
        this.animator       = GetComponent<Animator>();
        this.aud            = GetComponent<AudioSource>();

        if (animator)
            animator.keepAnimatorControllerStateOnDisable = true;

        attackStats = new AttackStats(attackData);
    }

    public void Fire()
        => attackFunction.Fire();

    public void FireAimed(RaycastHit hitInfo)
        => attackFunction.FireAimed(hitInfo);

    public float GetDamage()
    {
        if (player.infiniteDamage) return float.MaxValue;

        return player.characterStats.damage + attackStats.damage;
    }

}
