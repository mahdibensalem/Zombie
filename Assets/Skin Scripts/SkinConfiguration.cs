using UnityEngine;


[CreateAssetMenu(fileName = "SkinConfiguration", menuName = "ScriptableObject/Skin Configuration")]
public class SkinConfiguration : ScriptableObject
{
    public ShopItem[] shopItems;

}
[System.Serializable]

public class ShopItem
{
    public string itemName;
    public bool isUnlocked;
    public int inlockedCost;
    public ItemScriptableObject carInfo;

}
[System.Serializable]
public class CarInfo
{
    public string name;
    public InlockedLVL[] inlockedLVL;

}
[System.Serializable]

public class InlockedLVL
{
    public int unlockedCost;
    public int value;
    public PropertyType propertyType;
}
[System.Serializable]

public class PropertyType
{

}
