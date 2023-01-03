using UnityEngine;

namespace ShopUpgradeSystem
{
    [System.Serializable]
    public class ShopData
    {
        public ShopItem[] shopItems;
    }

    [System.Serializable]
    public class ShopItem
    {
        public string carName;             //name of item
        public bool isUnlocked;             //bool to check unlock status
        public int unlockCost;              //cost of unlock
        //public CarUpdate carUpdate;
        public CarLevelData[] carLevelsData;//array of all unlockable car levels
        public int unlockedHealthLevel = 0;       //level of Health
        public int unlockedBodyLevel = 0;       //level of Body
        public int unlockedAttackSpeedLevel = 0;       //level of AttackSpeed
    }

    //[System.Serializable]
    //public class CarUpdate
    //{
        
    //}
    [System.Serializable]
    public class CarLevelData
    {
        public int helthUnlockCost;
        public int health;
        public int bodyUnlockCost;
        public int body;
        public int attackSpeedUnlockCost;
        public int attackSpeed;

    }
}