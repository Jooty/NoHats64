using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class UIAssetBank
{

    private static Sprite[] allHeads = Resources.LoadAll<Sprite>("UI/CharacterHeads");
    private static Sprite[] allWeaponIcons = Resources.LoadAll<Sprite>("UI/WeaponIcons");
    private static Sprite[] allAbilityIcons = Resources.LoadAll<Sprite>("UI/AbilityIcons");

    public static bool TryGetCharacterHead(string characterName, out Sprite prefab)
    {
        var head = allHeads
            .First(x => x.name.Contains(GetProperSearchName(characterName)));
        if (head)
        {
            prefab = head;
            return true;
        }
        else
        {
            prefab = null;
            Debug.LogWarning($"Attempted to get character head for \"{characterName}\", but it does not exist!");
            return false;
        }
    }

    public static bool TryGetAllCharacterHeads(string characterName, out Sprite[] heads)
    {
        var _heads = allHeads
            .Where(x => x.name.Contains(GetProperSearchName(characterName)));
        if (_heads.Count() <= 0)
        {
            heads = null;
            Debug.LogWarning($"Attempted to get character heads for \"{characterName}\", but it does not exist!");
            return false;
        }
        else
        {
            heads = _heads.ToArray();
            return true;
        }
    }

    public static bool TryGetWeaponIcon(string weaponName, out Sprite weapon)
    {
        var _weapon = allWeaponIcons
            .First(x => x.name == GetProperSearchName(weaponName));
        if (_weapon)
        {
            weapon = _weapon;
            return true;
        }
        else
        {
            weapon = null;
            Debug.LogWarning($"Attempted to get weapon icon for \"{weaponName}\", but it does not exist!");
            return false;
        }
    }

    public static bool TryGetAbilityIcon(string abilityName, out Sprite prefab)
    {
        var ability = allAbilityIcons
            .First(x => x.name == GetProperSearchName(abilityName));
        if (ability)
        {
            prefab = ability;
            return true;
        }
        else
        {
            prefab = null;
            Debug.LogWarning($"Attempted to get ability icon for \"{abilityName}\", but it does not exist!");
            return false;
        }
    }

    public static string GetProperSearchName(string name)
    {
        var properSearchName = new string(name.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());
        return properSearchName;
    }

}