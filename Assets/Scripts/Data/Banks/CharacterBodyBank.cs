using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CharacterBodyBank
{

    private static GameObject[] allBodies = Resources.LoadAll<GameObject>("CharacterBodies");

    public static bool TryGetBody(string characterName, out GameObject prefab)
    {
        var _body = allBodies.First(x => x.name == GetBodyNameFromCharacterName(characterName));
        if (_body)
        {
            prefab = _body;
            return true;
        }
        else
        {
            prefab = null;
            Debug.LogWarning($"Attempted to get CharacterBody \"{characterName}\", but it does not exist!");
            return false;
        }
    }

    public static string GetBodyNameFromCharacterName(string characterName)
    {
        var properSearchName = new string(characterName.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());
        return $"CB_{properSearchName}";
    }

    public static GameObject[] GetAllBodies()
    {
        return allBodies;
    }

}
