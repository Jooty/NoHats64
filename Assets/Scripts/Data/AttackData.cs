using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Attack", order = 1)]
public class AttackData : BaseData
{

    // if you add or change any of these, you must also
    // change the editor for this script, otherwise you'll get compile errors

    public AttackDataType attackType;
    public int baseDamage;
    public float baseFireRate;
    public bool isSingleFire;
    public float baseRecoilStrength;
    public int baseMaxMagazineSize;
    public float baseReloadTime;
    public float baseDuration;
    public float baseCooldown;
    public bool hasPreAim;
    public float baseCastTime;
    public int baseChargeCount;

    public Vector3 RecoilVector()
    {
        return new Vector3(baseRecoilStrength, 0, 0);
    }

}
