using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullensUltimate : PlayerAttackFunctionality
{

    public float hitboxRadius = 4.0f;

    public GameObject hammerPrefab;

    public override void Fire() => throw new System.NotImplementedException();

    public override void FireAimed(RaycastHit hitInfo)
    {
        attack.animator.SetTrigger("throw");

        var hammer = Instantiate(hammerPrefab, hitInfo.point, Quaternion.identity);
        hammer.GetComponent<CullensUltimateHammer>().Init(this);

        Destroy(hammer, 7.0f);
    }

}
