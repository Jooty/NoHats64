using System;
using System.Linq;
using UnityEngine;

public static class CharacterDataBank
{

    private static readonly CharacterData[] allEnemies = Resources.LoadAll<CharacterData>("Data/Characters/Enemies");
    private static readonly CharacterData[] allPlayers = Resources.LoadAll<CharacterData>("Data/Characters/Players");

    public static bool TryGetEnemyData(string characterName, out CharacterData data)
    {
        var temp = allEnemies.First(x => x.Name == characterName);
        if (temp)
        {
            data = temp;
            return true;
        }
        else
        {
            data = null;
            Debug.LogWarning($"Tried to get enemy data for \"{characterName}\", but did not exist in data bank!");
            return false;
        }
    }

    public static bool TryGetPlayerData(string characterName, out CharacterData data)
    {
        var temp = allPlayers.First(x => x.Name == characterName);
        if (temp)
        {
            data = temp;
            return true;
        }
        else
        {
            data = null;
            Debug.LogWarning($"Tried to get enemy data for \"{characterName}\", but did not exist in data bank!");
            return false;
        }
    }

}