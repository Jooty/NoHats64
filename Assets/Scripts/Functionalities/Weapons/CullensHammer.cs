using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullensHammer : PlayerAttackFunctionality
{

    public AudioClip[] swingSoundEffects;

    public float hitboxDistance = 1.4f;
    public float hitboxRadius = 1.75f;

    public SpriteRenderer bloodSprite;
    private Sequence bloodSpriteFadeoutSequence;

    int fireHash;

    private void Start()
    {
        this.fireHash = Animator.StringToHash("fire");

        bloodSpriteFadeoutSequence = DOTween.Sequence() // fade out
            .PrependInterval(5.0f) // wait 5 seconds before starting fade out
            .Append(bloodSprite.DOFade(0.0f, 4.0f))
            .SetEase(Ease.InOutExpo);
        bloodSpriteFadeoutSequence.TogglePause();
    }

    public override void Fire()
    {
        attack.animator.SetTrigger(fireHash);
    }

    public override void FireAimed(RaycastHit hitInfo)
        => throw new System.NotImplementedException();

    public void HammerSwingEvent01()
    {
        // effects
        attack.aud.PlayOneShot(swingSoundEffects[0]);
        var recoilVector = new Vector3(0, 0, base.attack.attackStats.recoilStrength);
        base.cameraRecoil.DoRecoil(recoilVector);

        HitDetection();
    }

    public void HammerSwingEvent02()
    {
        // effects
        attack.aud.PlayOneShot(swingSoundEffects[1]);
        var recoilVector = new Vector3(0, base.attack.attackStats.recoilStrength, 0);
        base.cameraRecoil.DoRecoil(recoilVector);

        HitDetection();
    }

    private void HitDetection()
    {
        var origin = base.attack.player.transform.position + base.attack.player.transform.forward * hitboxDistance;
        Collider[] hitColliders = Physics.OverlapSphere(origin, hitboxRadius);

        if (hitColliders.Length == 0)
            return;

        bool didHit = false;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].name.Contains("CB")) // we've hit a characterbody
            {
                var character = hitColliders[i].transform.GetComponentInParent<Character>();
                if (character.team != attack.player.team)
                {
                    character.TakeDamage(attack.player, attack.GetDamage());

                    didHit = true;
                }
            }
        }

        if (didHit)
        {
            // effects
            bloodSprite.color = new Color(bloodSprite.color.r, bloodSprite.color.g, bloodSprite.color.b, 1.0f);
            bloodSpriteFadeoutSequence.Restart();
            // hitstop
            // base.HitStop(0.1f);
        }
    }

    // draw hitbox
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(source.transform.position + source.transform.forward * hitboxDistance, hitboxRadius);
    //}

}
