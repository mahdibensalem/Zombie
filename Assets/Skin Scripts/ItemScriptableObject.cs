using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Item Config", menuName = "ScriptableObject/Item Configuration")]

public class ItemScriptableObject : ScriptableObject
{
    public Types type;
    












    public enum Types { health, bodyCar, speed, Weapon }
}
