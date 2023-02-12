using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WeaponBank
{

    private static GameObject[] allWeapons = Resources.LoadAll<GameObject>("Weapons");

    public static bool TryGetWeapon(string weaponName, out GameObject weapon)
    {
        var properSearchName = new string(weaponName.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());

        var _weapon = allWeapons.First(x => x.name == $"Weapon_{properSearchName}");
        if (_weapon)
        {
            weapon = _weapon;
            return true;
        }
        else
        {
            weapon = null;
            Debug.LogError($"Could not find weapon in resources with name \"{weaponName}\"!");
            return false;
        }
    }

    public static bool TryGetPassive(string passiveName, out GameObject passive)
    {
        var properSearchName = new string(passiveName.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());

        var _passive = allWeapons.First(x => x.name == $"Passive_{properSearchName}");
        if (_passive)
        {
            passive = _passive;
            return true;
        }
        else
        {
            passive = null;
            Debug.LogError($"Could not find passive in resources with name \"{passiveName}\"!");
            return false;
        }
    }

    public static bool TryGetUltimate(string ultimateName, out GameObject ultimate)
    {
        var properSearchName = new string(ultimateName.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());

        var _ultimate = allWeapons.First(x => x.name == $"Ultimate_{properSearchName}");
        if (_ultimate)
        {
            ultimate = _ultimate;
            return true;
        }
        else
        {
            ultimate = null;
            Debug.LogError($"Could not find ultimate in resources with name \"{ultimateName}\"!");
            return false;
        }
    }

}
