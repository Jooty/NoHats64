using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodGun : PlayerAttackFunctionality
{

    public Transform muzzleFlashLocation;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletHolePrefab;
    public AudioClip soundEffect;
    private GameObject currentMuzzleFlash;

    public override void Fire()
    {
        DoEffects();
        FireBullet();
        cameraRecoil.DoRecoil(attack.attackData.RecoilVector());
    }

    public override void FireAimed(RaycastHit hitInfo)
         => throw new System.NotImplementedException();

    private void FireBullet()
    {
        Ray rayOrigin = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(rayOrigin, out var hit))
        {
            if (hit.collider.name.Contains("CB")) // we've hit a characterbody
            {
                var character = hit.collider.GetComponentInParent<Character>();
                if (character.team != attack.player.team)
                {
                    character.TakeDamage(base.attack.player, base.attack.GetDamage());
                }
            }
            else
            {
                SpawnBulletHole(hit);
            }
        }
    }

    private void SpawnBulletHole(RaycastHit hit)
    {
        var hole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(hole, 15.0f);
    }

    private void DoEffects()
    {
        // effects
        attack.aud.PlayOneShot(soundEffect);
        attack.animator.SetTrigger("Fire");

        // muzzle flash
        var desiredRot = mainCam.transform.rotation;
        currentMuzzleFlash = Instantiate(muzzleFlashPrefab, muzzleFlashLocation.position, desiredRot, transform);
        Destroy(currentMuzzleFlash, 1f);
    }

}
