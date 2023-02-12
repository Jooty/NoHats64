using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour 
{

    /*
     * For the current time being, enemies use distance
     * and line-of-sight to "spot" the player.
     */

    public Player playerCharacter;
    private LevelManager levelManager;

    // locals
    private NavMeshAgent nav;
    private Character character;
    private CharacterAttackController attackController;
    private CharacterAnimator anim;

    private bool canMove;

    private void Awake()
    {
        this.nav              = this.GetComponent<NavMeshAgent>();
        this.character        = this.GetComponent<Character>();
        this.attackController = this.GetComponent<CharacterAttackController>();
        this.anim             = this.GetComponent<CharacterAnimator>();

        canMove = true;
        UpdateWalkSpeed();

        // TODO: CHANGE LATER; DEBUG PURPOSES
        playerCharacter = FindObjectOfType<Player>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Update()
    {
        if (character.isDead) return;
        else if (!canMove)
        {
            nav.isStopped = true;
            nav.velocity = Vector3.zero;
            return;
        }
        else if (canMove)
        {
            nav.isStopped = false;
        }

        if (IsPlayerInDetectionDistance() || levelManager.hasLevelStarted)
        {
            if (HasPlayerLineOfSight() || levelManager.hasLevelStarted)
            {
                nav.SetDestination(playerCharacter.transform.position);
            }
        }

        if (character.characterData.isMelee && IsPlayerInAttackDistance())
        {
            attackController.AttemptMeleeAttack();
        }

        anim.SetWalk(nav.remainingDistance > 0.1f);
    }

    public void Stop()
    {
        nav.isStopped = true;
    }

    public void UpdateWalkSpeed()
    {
        nav.speed = character.characterStats.moveSpeed;
    }

    private bool IsPlayerInAttackDistance()
    {
        return Vector3.Distance(transform.position, playerCharacter.transform.position) < character.characterStats.attackRange;
    }

    private bool IsPlayerInDetectionDistance()
    {
        return Vector3.Distance(transform.position, playerCharacter.transform.position) < character.characterData.detectionRange;
    }

    private bool HasPlayerLineOfSight()
    {
        if (Physics.Raycast(transform.position, (playerCharacter.transform.position - transform.position), out var hit))
        {
            if (hit.transform.CompareTag("Player"))
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public void FlinchTimer() => StartCoroutine(flinchTimer());
    private IEnumerator flinchTimer()
    {
        canMove = false;
        yield return new WaitForSeconds(0.75f);
        canMove = true;
    }

}
