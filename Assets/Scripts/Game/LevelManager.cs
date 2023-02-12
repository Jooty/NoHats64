using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LevelManager : MonoBehaviour
{

    public static LevelManager singleton;

    [SerializeField] private GameObject emptyPlayerPrefab;
    [SerializeField] private GameObject emptyCharacterPrefab;
    public Level CurrentLevel { get; private set; }
    [HideInInspector] public bool hasLevelStarted;
    [HideInInspector] public Player localPlayer;

    public GameObject preLevelStartCamera;
    private List<SpawnPoint> nonPlayerSpawnPoints;
    private List<SpawnPoint> playerSpawnPoints;

    private bool canTryNewSpawn;
    private const float TRY_NEW_RESPAWN_TIMER = 1.0f;

    private void Awake()
    {
        if (singleton) Destroy(this);
        singleton = this;

        SetAllSpawnPoints();

        // reset default values
        canTryNewSpawn = true;
    }

    private void Update()
    {
        if (canTryNewSpawn && hasLevelStarted)
        {
            if (CurrentLevel.enemiesLeftToSpawn > 0)
            {
                TrySpawnNewEnemy();
            }

            // we don't need to check spawns EVERY frame, so do it every x seconds.
            // x being canTryNewSpawnTimer
            StartCoroutine(WaitForTryNewSpawn());
        }
    }

    public void SetLevel(Level level)
    {
        this.CurrentLevel = level;
    }

    public void StartLevel()
    {
        if (preLevelStartCamera) 
            preLevelStartCamera.SetActive(false);

        SpawnPlayer();

        hasLevelStarted = true;
    }

    private void SpawnPlayer()
    {
        // TODO: change later, debug purposes
        if (CharacterDataBank.TryGetPlayerData("Cullen", out var data))
        {
            var spawnpoint = playerSpawnPoints[0];
            var player = Instantiate(emptyPlayerPrefab, spawnpoint.transform.position, spawnpoint.transform.rotation);
            localPlayer = player.GetComponent<Player>();
            localPlayer.SetupCharacter(data);
        }
        else
            return;
    }

    private void TrySpawnNewEnemy()
    {
        var spawnPoint = GetAvailableSpawnpoint();
        if (spawnPoint)
        {
            var enemyToSpawn = CurrentLevel.GetRandomEnemyToSpawn();
            spawnPoint.Cooldown(CurrentLevel.GetRandomSpawnCooldown());

            var enemy = Instantiate(emptyCharacterPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            enemy.GetComponent<Character>().SetupCharacter(enemyToSpawn);
        }
        else
            return;
    }

    private void SetAllSpawnPoints()
    {
        var allSpawnGOs = GameObject.FindGameObjectsWithTag("SpawnPoint");
        List<SpawnPoint> allSpawns = new List<SpawnPoint>();
        for (int i = 0; i < allSpawnGOs.Count(); i++)
        {
            allSpawns.Add(allSpawnGOs[i].GetComponent<SpawnPoint>());
        }

        nonPlayerSpawnPoints = allSpawns.Where(x => x.isPlayerSpawn == false).ToList();
        playerSpawnPoints = allSpawns.Where(x => x.isPlayerSpawn == true).ToList();
    }

    private SpawnPoint GetAvailableSpawnpoint()
    {
        var available = nonPlayerSpawnPoints
            .Where(x => x.isReadyToSpawn)
            .ToList();
        if (available.Count <= 0) return null;
        else
            return available[Random.Range(0, available.Count())];
    }

    private IEnumerator WaitForTryNewSpawn()
    {
        canTryNewSpawn = false;
        yield return new WaitForSeconds(TRY_NEW_RESPAWN_TIMER);
        canTryNewSpawn = true;

        yield return null;
    }

}