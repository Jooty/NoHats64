using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    public GameObject SpawnEffectPrefab;

    /// <summary>
    /// If true, this spawnpoint is unique to a character.
    /// </summary>
    public bool isPlayerSpawn;
    public GameObject uniqueSpawnPrefab;
    [HideInInspector] public bool isReadyToSpawn;

    void Awake()
    {
        isReadyToSpawn = true;
    }

    public void SpawnCharacter(GameObject character)
    {
        if (this.isPlayerSpawn)
        {
            Debug.LogError("This is a unique spawnpoint! Use SpawnUniqueCharacter() instead!");
            return;
        }

        Instantiate(character, transform.position, transform.rotation);
    }

    public void Cooldown(float time) => StartCoroutine(cooldown(time));
    private IEnumerator cooldown(float time)
    {
        isReadyToSpawn = false;
        yield return new WaitForSeconds(time);
        isReadyToSpawn = true;
    }

}
