using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullensUltimateHammer : MonoBehaviour
{

    public AudioClip landSoundEffect;

    private CullensUltimate source;

    // locals
    private AudioSource aud;

    private void Awake()
    {
        this.aud = this.GetComponent<AudioSource>();
    }

    public void Init(CullensUltimate source)
    {
        this.source = source;
    }

    public void LandEvent()
    {
        aud.PlayOneShot(landSoundEffect);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, source.hitboxRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].name.Contains("CB")) // we've hit a characterbody
            {
                var character = hitColliders[i].transform.root.GetComponent<Character>();
                if (character.team != source.attack.player.team)
                {
                    character.TakeDamage(source.attack.player, source.attack.GetDamage());
                }
            }
        }
    }

}
