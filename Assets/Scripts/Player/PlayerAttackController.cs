using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{

    [HideInInspector] public PlayerAttack currentAttack;
    [HideInInspector] public List<PlayerAttack> Attacks = new List<PlayerAttack>();
    public GameObject attackHolder;
    public float weaponSwapTime = 1.0f;

    private bool isSwapping;
    private bool hasFired;
    private bool isReloading;
    private float lastShotTime;
    private bool indicatorActive;
    private GameObject indicator;
    private RaycastHit currentHitInfo;
    private int currentCharges;

    private Camera mainCam;

    // locals
    private Player player;

    private void Awake()
    {
        this.player = this.GetComponent<Player>();
        // TODO: change later
        mainCam = Camera.main;

        // reset default values
        lastShotTime = 0.0f;
        hasFired = false;
        isSwapping = false;
        isReloading = false;
    }

    private void Start()
    {
        GiveSignatureWeapon(player.characterData.SignatureWeapon);
        GivePassiveAbility(player.characterData.PassiveAbility);
        GiveUltimateAbility(player.characterData.UltimateAbility);

        ChangeAttack(0);
    }

    private void Update()
    {
        player.characterAnimator.SetAttackSpeedModifier(player.characterStats.attackSpeedMultiplier);
        CalculateInputs();

        if (indicatorActive)
            MoveAimIndicator();
    }

    private void CalculateInputs()
    {
        if (Input.GetMouseButton(0))
        {
            if (CanFire())
            {
                if (!currentAttack.attackData.hasPreAim) 
                {
                    Fire();
                }
                else if (currentAttack.attackData.hasPreAim && indicatorActive)
                {
                    FireAimed(currentHitInfo);
                }

                if (currentAttack.attackData.attackType == AttackDataType.ranged)
                    StartCoroutine(ReloadTimer());
            }
        }

        if (hasFired && Input.GetMouseButtonUp(0))
        {
            hasFired = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CastUltimate();
        }
    }

    private void Fire()
    {
        hasFired = true;
        lastShotTime = Time.time;
        currentAttack.Fire();
    }

    private void FireAimed(RaycastHit hitInfo)
    {
        if (currentCharges <= 0) return;
        else
        {
            currentAttack.FireAimed(hitInfo);

            // if this is our last charge
            if (currentCharges - 1 == 0)
            {
                ToggleIndicator(false);
                currentCharges = 0;

                StartCoroutine(WaitToSwap(0));

                return;
            }

            currentCharges--;
        }
    }

    private void CastUltimate()
    {
        // if already active, cancel
        if (indicatorActive)
        {
            ToggleIndicator(false);
            ChangeAttack(0);

            return;
        }

        PlayerAttack ultimate = GetUltimateAbility();
        ChangeAttack(ultimate);
        if (ultimate.attackData.hasPreAim)
        {
            ToggleIndicator(true);
        }
    }

    private void MoveAimIndicator()
    {
        Ray rayOrigin = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayOrigin, out currentHitInfo, Mathf.Infinity, 1))
        {
            indicator.transform.position = currentHitInfo.point;
        }
    }

    private void ToggleIndicator(bool val)
    {
        if (val)
        {
            // TODO: Change later, this is for debug purposes only.
            indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            indicator.GetComponent<SphereCollider>().enabled = false;
            indicator.layer = 12;
            indicatorActive = true;
        }
        else
        {
            Destroy(indicator);
            indicatorActive = false;
        }
    }

    private void GiveSignatureWeapon(AttackData weapon)
    {
        if (WeaponBank.TryGetWeapon(weapon.name, out var _weapon))
        {
            var _w = Instantiate(_weapon, attackHolder.transform);

            AddAttack(_w.GetComponent<PlayerAttack>());
        }
    }

    private void GivePassiveAbility(AttackData passive)
    {
        if (WeaponBank.TryGetPassive(passive.name, out var _passive))
        {
            var _p = Instantiate(_passive, attackHolder.transform);

            AddAttack(_p.GetComponent<PlayerAttack>());
        }
    }

    private void GiveUltimateAbility(AttackData ultimate)
    {
        if (WeaponBank.TryGetUltimate(ultimate.name, out var _ultimate))
        {
            var _u = Instantiate(_ultimate, attackHolder.transform);

            AddAttack(_u.GetComponent<PlayerAttack>());
        }
    }

    private void AddAttack(PlayerAttack newAttack)
    {
        Attacks.Add(newAttack);
        if (newAttack.attackData.attackType != AttackDataType.passive)
            newAttack.gameObject.SetActive(false);
    }

    private void ChangeAttack(int weaponIndex)
    {
        currentAttack = Attacks[weaponIndex];
        // disable all other weapons except for passives
        foreach (var attack in Attacks.Where(x => x != currentAttack))
        {
            if (attack.attackData.attackType == AttackDataType.passive) continue;
            attack.gameObject.SetActive(false);
        }
        currentAttack.gameObject.SetActive(true);

        currentCharges = currentAttack.attackData.baseChargeCount;

        // reset weapon animator
        if (currentAttack.animator)
        {
            currentAttack.animator.Rebind();
            currentAttack.animator.Update(0f);
        }
    }

    private void ChangeAttack(PlayerAttack attack)
    {
        currentAttack = Attacks.First(x => x == attack);
        // disable all other weapons except for passives
        foreach (var _attack in Attacks.Where(x => x != attack))
        {
            if (_attack.attackData.attackType == AttackDataType.passive) continue;
            _attack.gameObject.SetActive(false);
        }
        currentAttack.gameObject.SetActive(true);

        currentCharges = currentAttack.attackData.baseChargeCount;

        // reset weapon animator
        if (currentAttack.animator)
        {
            currentAttack.animator.Rebind();
            currentAttack.animator.Update(0f);
        }
    }

    private bool CanFire()
    {
        if (isSwapping || isReloading) return false;

        if (currentAttack.attackData.isSingleFire)
        {
            return (!hasFired && Time.time > currentAttack.attackStats.firerate + lastShotTime);
        }
        else
        {
            return (Time.time > currentAttack.attackStats.firerate + lastShotTime);
        }
    }

    private PlayerAttack GetUltimateAbility()
    {
        return Attacks.First(x => x.attackData.attackType == AttackDataType.ultimate);
    }

    private IEnumerator WaitToSwap(int weaponIndex)
    {
        isSwapping = true;
        yield return new WaitForSeconds(weaponSwapTime);
        isSwapping = false;

        ChangeAttack(weaponIndex);

        yield return null;
    }

    private IEnumerator ReloadTimer()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentAttack.attackStats.reloadTime);
        isReloading = false;

        yield return null;
    }

}
