using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData : ScriptableObject
{

    public string Name;
    [TextArea(5, 20)] public string Description;

}
