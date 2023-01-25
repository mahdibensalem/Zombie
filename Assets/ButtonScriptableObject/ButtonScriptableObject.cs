using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Upgrade Button Configuration", menuName = "ScriptableObject/Upgrade Button Configuration")]
public class ButtonScriptableObject : ScriptableObject
{
    public List<Sprite> images;
    public string description;
    public actionMethode action;
    public float addedValue;
    public enum actionMethode
    {
        addDamage,
        addHealth,
        addAttackSpeed,
        addRange,
        addViewAngle
    }


}
