using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CullensRampage : PlayerAttackFunctionality
{

    public int neededKills = 5;
    public float timeToKill = 3.0f;
    public float attackSpeedMultiplier = 1.5f;
    public float attackDamageMultiplier = 1.2f;

    private bool killCountdownStarted;
    private int countdownKills = 0;
    private bool isRampaging = false;
    private bool isInCooldown = false;
    private List<float> countedTimes = new List<float>();

    // implement abstract class
    public override void Fire()                        => throw new System.NotImplementedException();
    public override void FireAimed(RaycastHit hitInfo) => throw new System.NotImplementedException();

    private void OnEnable()
    {
        KillLog.OnKillAdded += KillLog_OnKillAdded;
    }

    private void OnDisable()
    {
        KillLog.OnKillAdded -= KillLog_OnKillAdded;
    }

    private void KillLog_OnKillAdded(KillLogEventAddedArgs e)
    {
        if (isRampaging || isInCooldown) return;

        if (!killCountdownStarted && e.GetContext().CompareKiller(base.attack.player))
        {
            StartCoroutine(KillCountdown());
            countdownKills++;
        }
        else if (killCountdownStarted && e.GetContext().CompareKiller(base.attack.player))
        {
            countdownKills++;
        }
    }

    private void Update()
    {
        if (isRampaging || isInCooldown) return;

        if (countdownKills >= neededKills)
        {
            // !!! RAMPAGE !!!
            StartCoroutine(Rampage());
        }
    }

    private IEnumerator Rampage()
    {
        StartCoroutine(RampageCooldown());

        attack.player.characterStats.attackSpeedMultiplier *= attackSpeedMultiplier;
        attack.player.characterStats.damage *= attackDamageMultiplier;
        yield return new WaitForSeconds(attack.attackData.baseDuration);
        attack.player.characterStats.attackSpeedMultiplier *= 1.0f;
        attack.player.characterStats.damage *= 1.0f;

        yield return null;
    }

    private IEnumerator KillCountdown()
    {
        killCountdownStarted = true;

        yield return new WaitForSeconds(timeToKill);

        countdownKills = 0;
        killCountdownStarted = false;

        yield return null;
    }

    private IEnumerator RampageCooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(attack.attackData.baseCooldown);
        isInCooldown = false;

        yield return null;
    }

}
