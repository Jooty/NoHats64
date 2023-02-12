using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudController : MonoBehaviour
{

    [Header("HUD elements")]
    [SerializeField] private Image healthOrb;
    [SerializeField] private Image characterSprite;
    [SerializeField] private Image weaponIconSprite;
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI maxAmmoText;
    [Space()]

    private Player localPlayer;

    private bool hasFoundPlayer;

    private Material healthOrbMaterial;
    private Sprite[] currentCharacterIcons;
    private Sprite currentWeaponSprite;

    private void Start()
    {
        this.healthOrbMaterial = this.healthOrb.material;

        hasFoundPlayer = false;
    }

    private void Update()
    {
        if (!hasFoundPlayer)
            SearchForAndSetPlayer();

        if (localPlayer)
        {
            SetPlayerStatus();
            SetWeaponStatus();
        }
    }

    private void SearchForAndSetPlayer()
    {
        localPlayer = FindObjectOfType<Player>();
        if (!localPlayer) return;

        if (UIAssetBank.TryGetAllCharacterHeads(localPlayer.characterData.Name, out var heads))
        {
            currentCharacterIcons = heads;
        }

        // i'm not sure why this results in an error,
        // but it works as intended so im not changing it.
        if (UIAssetBank.TryGetWeaponIcon(localPlayer.playerAttackController.currentAttack.attackData.name, out var weapon))
        {
            currentWeaponSprite = weapon;
        }
    }

    private void SetPlayerStatus()
    {
        // set health orb
        // im so bad at math
        var val = Util.ConvertNumberRangeRatio(localPlayer.characterStats.currentHealth, 0, localPlayer.characterStats.maximumHealth, -2.0f, 2.0f);
        healthOrbMaterial.SetFloat("PositionUV_Y_1", -val);

        // set character icon
        var currentHealth = localPlayer.characterStats.currentHealth;
        var healthPercentage = (currentHealth / localPlayer.characterStats.maximumHealth) * 100;
        if (healthPercentage > 70)
        {
            characterSprite.sprite = currentCharacterIcons[0];
        }
        else if (healthPercentage < 70 && healthPercentage > 20)
        {
            characterSprite.sprite = currentCharacterIcons[1];
        }
        else if (healthPercentage < 20)
        {
            characterSprite.sprite = currentCharacterIcons[2];
        }
    }

    private void SetWeaponStatus()
    {
        weaponIconSprite.sprite = currentWeaponSprite;

        // ∞
        // do ammo counter
        var currentAttackStats = localPlayer.playerAttackController.currentAttack.attackStats;

        if (localPlayer.playerAttackController.currentAttack.attackData.attackType == AttackDataType.ranged)
        {
            currentAmmoText.text = currentAttackStats.magazine.ToString();
            maxAmmoText.text = currentAttackStats.maxMagazine.ToString();
        }
        else
        {
            currentAmmoText.text = "∞";
            maxAmmoText.text = "∞";
        }
    }

}
