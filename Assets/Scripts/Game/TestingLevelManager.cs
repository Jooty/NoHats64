using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TestingLevelManager : MonoBehaviour
{

    private LevelManager levelManager;

    [SerializeField] private GameObject testingPanel;
    [SerializeField] private Toggle virginEnabled;
    [SerializeField] private TMP_InputField virginCount;
    [SerializeField] private Toggle godmode;
    [SerializeField] private Toggle infiniteDamage;

    private void Start()
    {
        levelManager = LevelManager.singleton;

        testingPanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameManager.instance.RestartScene();
        }
    }

    public void BTN_ParametersReturn()
    {
        GameManager.instance.ChangeScene("MainMenu");
    }

    public void BTN_ParametersStart()
    {
        // make level spawn data
        List<LevelEnemySpawnData> spawnData = new List<LevelEnemySpawnData>();
        if (virginEnabled.isOn)
        {
            if (CharacterDataBank.TryGetEnemyData("Virgin", out var data))
            {
                if (int.TryParse(virginCount.text, out var result))
                {
                    spawnData.Add(new LevelEnemySpawnData(data, result));
                }
                else
                {
                    Debug.LogError("Virgin count was not a number!");
                }
            }
        }

        // set the level
        Level level = new Level(spawnData.ToArray(), 5.0f, 10.0f);
        levelManager.SetLevel(level);
        levelManager.StartLevel();

        // cheats
        levelManager.localPlayer.godmode = godmode.isOn;
        levelManager.localPlayer.infiniteDamage = infiniteDamage.isOn;
    }


}
