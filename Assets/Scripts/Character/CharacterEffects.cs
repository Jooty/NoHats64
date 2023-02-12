using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterEffects : MonoBehaviour
{

    public GameObject despawnEffectPrefab;

    // blood
    public GameObject headPrefab;
    public GameObject bloodFountainPrefab;
    public GameObject bloodHitPrefab;
    public GameObject bloodSplatterPrefab;
    public SpriteRenderer bloodSprite;

    public float gibSpeed = 10.0f;
    public GameObject[] gibPrefabs;

    public Transform headFountainLoc;

    private Character character;
    private CharacterData data;

    private bool canDoIdleSound;

    // locals
    private AudioSource aud;

    private void Awake()
    {
        this.aud = this.GetComponent<AudioSource>();

        // reset default values
        canDoIdleSound = true;
    }

    private void Start()
    {
        character = transform.GetComponentInParent<Character>();
        data = character.characterData;
    }
    
    private void Update()
    {
        if (canDoIdleSound && !character.isDead)
            DoIdleSound();
    }

    public void DoExecutedEffects()
    {
        PlayDeathSound();

        // can add back later but i wont delete it for now
        //var fountain = Instantiate(bloodFountainPrefab, headFountainLoc.position, headFountainLoc.rotation);
        //Destroy(fountain, 3.0f);

        // head popoff
        var head = Instantiate(headPrefab, headFountainLoc.position, headFountainLoc.rotation);
        var rigid = head.GetComponent<Rigidbody>();
        rigid.AddForce(head.transform.up * Random.Range(100, 200));
        rigid.AddForce(-head.transform.forward * Random.Range(100, 200));
        Destroy(head, 6.0f);

        // spawn random amount of splatters
        for (int i = 0; i < Random.Range(2, 4); i++)
        {
            var _splatter = Instantiate(bloodSplatterPrefab, headFountainLoc.position, headFountainLoc.rotation);
            Destroy(_splatter, 20.0f);
        }
    }

    public void DoDespawnEffects()
    {
        aud.PlayOneShot(data.despawnSoundEffect);
        DoGibs();
    }

    private void DoGibs()
    {
        for (int i = 0; i < Random.Range(10, 15); i++)
        {
            var _gib = Instantiate(gibPrefabs[Random.Range(0, gibPrefabs.Length)], transform.position, transform.rotation);
            _gib.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * gibSpeed);

            Destroy(_gib, 1f);
        }
    }

    private void DoIdleSound()
    {
        if (!data.doSounds || !data.doIdleSounds) return;
        // choose random clip
        var clip = data.IdleAudioClips[Random.Range(0, data.IdleAudioClips.Count)];

        aud.PlayOneShot(clip);
        StartCoroutine(WaitForIdleSoundTimer());
    }

    public void FootstepEvent()
    {
        // TODO: finish later
        var clip = SFXBank.GetRandomFootstepSFX(SFXFootstepType.concrete);
        aud.PlayOneShot(clip);
    }

    public void PlayAttackSound()
    {
        if (!data.doSounds) return;
        // choose random clip
        var clip = data.attackAudioClips[Random.Range(0, data.attackAudioClips.Count)];

        aud.PlayOneShot(clip);
    }

    public void PlayDamagedEffect()
    {
        if (!data.doSounds) return;

        // choose random clip
        var clip = data.damagedAudioClips[Random.Range(0, data.damagedAudioClips.Count)];
        aud.PlayOneShot(clip);

        // splatter
        var splatter = Instantiate(bloodSplatterPrefab, transform.position, transform.rotation);
        Destroy(splatter, 20.0f);

        // blood on sprite
        if (bloodSprite)
        {
            float alphaVal = Util.NormalizeValue(character.characterStats.currentHealth, character.characterStats.maximumHealth, 0);
            bloodSprite.color = new Color(bloodSprite.color.r, bloodSprite.color.g, bloodSprite.color.b, alphaVal);
        }
    }

    public void PlayDeathSound()
    {
        if (!data.doSounds) return;
        // choose random clip
        var clip = data.brutalDamageAudioClips[Random.Range(0, data.brutalDamageAudioClips.Count)];

        aud.PlayOneShot(clip);
    }

    private IEnumerator WaitForIdleSoundTimer()
    {
        canDoIdleSound = false;

        var randomTime = Random.Range(data.idleSoundsMinimumWait, data.idleSoundsMaximumWait);
        yield return new WaitForSeconds(randomTime);

        canDoIdleSound = true;

        yield return null;
    }

}
