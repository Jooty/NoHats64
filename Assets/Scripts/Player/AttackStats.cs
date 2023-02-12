using UnityEngine;

public class AttackStats
{

    public int damage;
    public float firerate;
    public float recoilStrength;
    public int magazine;
    public int maxMagazine;
    public float reloadTime;
    public float duration;
    public float cooldown;
    public float castTime;
    public int chargeCount;

    public AttackStats(AttackData data)
    {
        this.damage = data.baseDamage;
        this.firerate = data.baseFireRate;
        this.recoilStrength = data.baseRecoilStrength;
        this.magazine = data.baseMaxMagazineSize;
        this.maxMagazine = data.baseMaxMagazineSize;
        this.reloadTime = data.baseReloadTime;
        this.duration = data.baseDuration;
        this.cooldown = data.baseCooldown;
        this.castTime = data.baseCastTime;
        this.chargeCount = data.baseChargeCount;
    }

    public Vector3 RecoilVector()
    {
        return new Vector3(recoilStrength, 0, 0);
    }

}