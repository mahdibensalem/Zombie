using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
namespace ShopUpgradeSystem
{
    public class ShopUI : MonoBehaviour
    {
        public static ShopUI Instance;
        [SerializeField] private int totalCoins = 5000;
        [SerializeField] private SaveLoadData saveLoadData;

        public GameObject[] carList;                       //list to all the 3D models of items
        public ShopData shopData;                 //ref to ShopSaveScriptable asset
        public Text unlockBtnText, carNameText,totalCoinsText; //ref to important text components
        public TextMeshProUGUI upgradeHealthBtnText, upgradeBodyBtnText, upgradeAttackSpeedBtnText;
        public TextMeshProUGUI HealthlevelText,BodyLevelText, AttackSpeedLevelText;
        public Button unlockBtn, upgradeHealthBtn, upgradeBodyBtn, upgradeAttackSpeedBtn, nextBtn, previousButton,playButton;   //ref to important Buttons.
        public Image[] LVLIndexHealthImage, LVLIndexBodyImage, LVLIndexAttackSpeedImage;

        private int currentIndex = 0;                       //index of current item showing in the shop 
        private int selectedIndex;                          //actual selected item index
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            
            saveLoadData.Initialize();                      //Initialize , load or save default data and load data
            selectedIndex = PlayerPrefs.GetInt("SelectedItem", 0);  //get the selectedIndex from PlayerPrefs
            currentIndex = selectedIndex;                           //set the currentIndex
            if (!PlayerPrefs.HasKey("Coins"))
            {
                PlayerPrefs.SetInt("Coins", totalCoins);
            }
            
            totalCoins = PlayerPrefs.GetInt("Coins");
            totalCoinsText.text = "" + totalCoins;
            if (totalCoins >= 1000)
            {
                totalCoinsText.text = (totalCoins / 1000) + "K";
            }
            SetCarInfo();

            unlockBtn.onClick.AddListener(() => UnlockSelectButton());      //add listner to button
            upgradeHealthBtn.onClick.AddListener(() => UpgradeHealthButton());          //add listner to button
            upgradeBodyBtn.onClick.AddListener(() => UpgradeBodyButton());          //add listner to button
            upgradeAttackSpeedBtn.onClick.AddListener(() => UpgradeAttackSpeedButton());          //add listner to button
            
            nextBtn.onClick.AddListener(() => NextButton());                //add listner to button
            previousButton.onClick.AddListener(() => PreviousButton());     //add listner to button
            playButton.onClick.AddListener(() => PlayButton());
            if (currentIndex == 0) previousButton.interactable = false;     //dont interact previousButton if currentIndex is 0
            //dont interact previousButton if currentIndex is shopItemList.shopItems.Length - 1
            if (currentIndex == shopData.shopItems.Length - 1) nextBtn.interactable = false;

            carList[currentIndex].SetActive(true);                         //activate the object at currentIndex
            UnlockButtonStatus();                                           
            UpgradeButtonStatus();
        }

        void SetCarInfo()
        {
            carNameText.text = shopData.shopItems[currentIndex].carName;
            int currentHealthLevel = shopData.shopItems[currentIndex].unlockedHealthLevel;
            int currentBodyLevel = shopData.shopItems[currentIndex].unlockedBodyLevel;
            int currentAttackSpeedLevel = shopData.shopItems[currentIndex].unlockedAttackSpeedLevel;


            HealthlevelText.text = "LEVEL "+ (currentHealthLevel+1)/*+ shopData.shopItems[currentIndex].carLevelsData[currentHealthLevel].health*/;  //level start from zero we add 1
            BodyLevelText.text = "LEVEL " + (currentBodyLevel+1)  ;
            AttackSpeedLevelText.text = "LEVEL " + (currentAttackSpeedLevel+1);
            foreach(Image img in LVLIndexHealthImage)
            {
                img.color = Color.white;
            }
            foreach(Image img in LVLIndexBodyImage)
            {
                img.color = Color.white;
            }
            foreach(Image img in LVLIndexAttackSpeedImage)
            {
                img.color = Color.white;
            }
            for(int i=0;i<=currentHealthLevel; i++)
            {
                LVLIndexHealthImage[i].color = Color.red;
            } 
            for(int i=0;i<=currentBodyLevel; i++)
            {
                LVLIndexBodyImage[i].color = Color.red;
            }   
            for(int i=0;i<=currentAttackSpeedLevel; i++)
            {
                LVLIndexAttackSpeedImage[i].color = Color.red;
            }
        }

        /// <summary>
        /// Method called on Next button click
        /// </summary>
        private void NextButton()
        {
            //check if currentIndex is less than the total shope items we have - 1
            if (currentIndex < shopData.shopItems.Length - 1)
            {
                carList[currentIndex].SetActive(false);                     //deactivate old model
                currentIndex++;                                             //increase count by 1
                carList[currentIndex].SetActive(true);                      //activate the new model
                SetCarInfo();                                               //set car information

                //check if current index is equal to total items - 1
                if (currentIndex == shopData.shopItems.Length - 1)
                {
                    nextBtn.interactable = false;                           //then set nextBtn interactable false
                }

                if (!previousButton.interactable)                           //if previousButton is not interactable
                {
                    previousButton.interactable = true;                     //then set it interactable
                }

                UnlockButtonStatus();
                UpgradeButtonStatus();
            }
        }

        /// <summary>
        /// Method called on Previous button click
        /// </summary>
        private void PreviousButton()
        {
            if (currentIndex > 0)                           //we check is currentIndex i more than 0
            {
                carList[currentIndex].SetActive(false);     //deactivate old model
                currentIndex--;                             //reduce count by 1
                carList[currentIndex].SetActive(true);      //activate the new model
                SetCarInfo();                               //set car information

                if (currentIndex == 0)                      //if currentIndex is 0
                {
                    previousButton.interactable = false;    //set previousButton interactable to false
                }

                if (!nextBtn.interactable)                  //if nextBtn interactable is false
                {
                    nextBtn.interactable = true;            //set nextBtn interactable to true
                }
                UnlockButtonStatus();
                UpgradeButtonStatus();
            }
        }

        /// <summary>
        /// Method called on Unlock button click
        /// </summary>
        private void UnlockSelectButton()
        {
            bool yesSelected = false;   //local bool
            if (shopData.shopItems[currentIndex].isUnlocked)    //if shop item at currentIndex is already unlocked
            {
                yesSelected = true;                             //set yesSelected to true
            }
            else if (!shopData.shopItems[currentIndex].isUnlocked)  //if shop item at currentIndex is not unlocked
            {
                //check if we have enough coins to unlock it
                if (totalCoins >= shopData.shopItems[currentIndex].unlockCost)
                {
                    //if yes then reduce the cost coins from our total coins
                    UpgradeCoins(shopData.shopItems[currentIndex].unlockCost);    //set the coins text
                    yesSelected = true;                             //set yesSelected to true
                    shopData.shopItems[currentIndex].isUnlocked = true; //mark the shop item unlocked
                    UpgradeButtonStatus();
                    saveLoadData.SaveData();
                }
            }

            if (yesSelected)
            {
                unlockBtnText.text = "Selected";                    //set the unlockBtnText text to Selected
                selectedIndex = currentIndex;                       //set the selectedIndex to currentIndex
                PlayerPrefs.SetInt("SelectedItem", selectedIndex);  //save the selectedIndex
                unlockBtn.interactable = false;                     //set unlockBtn interactable to false
            }

        }

        /// <summary>
        /// Method called on Upgrade button click
        /// </summary>

        private void UpgradeHealthButton()//upgrade button is interactable only if we have any level left to upgrade
        {
            //get the next level index
            int nextLevelIndex = shopData.shopItems[currentIndex].unlockedHealthLevel + 1;
            //we check if we have enough coins
            if (totalCoins >= shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].helthUnlockCost)
            {
                UpgradeCoins(shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].attackSpeedUnlockCost);         //set the coins text
                //if yes we increate the unlockedLevel by 1
                shopData.shopItems[currentIndex].unlockedHealthLevel++;

                //we check if are not at max level
                if (shopData.shopItems[currentIndex].unlockedHealthLevel < shopData.shopItems[currentIndex].carLevelsData.Length - 1)
                {
                    upgradeHealthBtnText.text = 
                        (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex + 1].helthUnlockCost/1000)+"K";
                }
                else    //we check if we are at max level
                {
                    upgradeHealthBtn.interactable = false;            //set upgradeBtn interactable to false
                    upgradeHealthBtnText.text = "Max";    //set the btn text
                }

                SetCarInfo();
                saveLoadData.SaveData();
            }
            UpgradeButtonStatus();

        }
        private void UpgradeBodyButton()//upgrade button is interactable only if we have any level left to upgrade
        {
            //get the next level index
            int nextLevelIndex = shopData.shopItems[currentIndex].unlockedBodyLevel + 1;
            //we check if we have enough coins
            if (totalCoins >= shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].bodyUnlockCost)
            {
                UpgradeCoins(shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].attackSpeedUnlockCost);         //set the coins text
                //if yes we increate the unlockedLevel by 1
                shopData.shopItems[currentIndex].unlockedBodyLevel++;

                //we check if are not at max level
                if (shopData.shopItems[currentIndex].unlockedBodyLevel < shopData.shopItems[currentIndex].carLevelsData.Length - 1)
                {
                    upgradeBodyBtnText.text = 
                       (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex + 1].bodyUnlockCost / 1000)+"K";
                }
                else    //we check if we are at max level
                {
                    upgradeBodyBtn.interactable = false;            //set upgradeBtn interactable to false
                    upgradeBodyBtnText.text = "Max";    //set the btn text
                }

                SetCarInfo();
                saveLoadData.SaveData();
            }
            UpgradeButtonStatus();
        }
        private void UpgradeAttackSpeedButton()//upgrade button is interactable only if we have any level left to upgrade
        {
            //get the next level index
            int nextLevelIndex = shopData.shopItems[currentIndex].unlockedAttackSpeedLevel + 1;
            //we check if we have enough coins
            if (totalCoins >= shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].attackSpeedUnlockCost)
            {
                UpgradeCoins(shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].attackSpeedUnlockCost);         //set the coins text
                //if yes we increate the unlockedLevel by 1
                shopData.shopItems[currentIndex].unlockedAttackSpeedLevel++;

                //we check if are not at max level
                if (shopData.shopItems[currentIndex].unlockedAttackSpeedLevel < shopData.shopItems[currentIndex].carLevelsData.Length - 1)
                {
                    upgradeAttackSpeedBtnText.text = 
                        (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex + 1].attackSpeedUnlockCost / 1000)+"K";
                }
                else    //we check if we are at max level
                {
                    upgradeAttackSpeedBtn.interactable = false;            //set upgradeBtn interactable to false
                    upgradeAttackSpeedBtnText.text = "Max";    //set the btn text
                }

                SetCarInfo();
                saveLoadData.SaveData();
            }
            UpgradeButtonStatus();
        }

        /// <summary>
        /// This method is called when we are changing the current item in the shop
        /// This method set the interactablity and text of unlock btn
        /// </summary>
        private void UnlockButtonStatus()
        {
            //if current item is unlocked
            if (shopData.shopItems[currentIndex].isUnlocked)
            {
                //if selectedIndex is not equal to currentIndex set unlockBtn interactable false else make it true
                unlockBtn.interactable = selectedIndex != currentIndex ? true : false;
                //set the text
                unlockBtnText.text = selectedIndex == currentIndex ? "Selected" : "Select";
            }
            else if (!shopData.shopItems[currentIndex].isUnlocked) //if current item is not unlocked
            {
                unlockBtn.interactable = true;  //set the unlockbtn interactable

                unlockBtnText.text =( shopData.shopItems[currentIndex].unlockCost / 1000) + "K"; //set the text as cost of item
            }
        }

        /// <summary>
        /// Method to set Upgrade button interactable and text sttus
        /// </summary>
        private void UpgradeButtonStatus()
        {
            //if current item is unlocked
            if (shopData.shopItems[currentIndex].isUnlocked)
            {
 #region Health
                //if unlockLevel of current item is less than its max level
                if (shopData.shopItems[currentIndex].unlockedHealthLevel < shopData.shopItems[currentIndex].carLevelsData.Length - 1)
                {
                    upgradeHealthBtn.interactable = true;                     //make upgradeBtn interactable true
                    int nextLevelIndex = shopData.shopItems[currentIndex].unlockedHealthLevel + 1;
                    //set the next level as value of upgrade button text
                    upgradeHealthBtnText.text = /*"Upgrade Cost:" +*/
                        (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].helthUnlockCost / 1000)+"K";
                    if (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].helthUnlockCost > totalCoins) upgradeHealthBtn.interactable = false;

                }
                else   //if unlockLevel of current item is equal to max level
                {
                    upgradeHealthBtn.interactable = false;                    //make upgradeBtn interactable false
                    upgradeHealthBtnText.text = "Max";
                }
#endregion
 #region Body
                //if unlockLevel of current item is less than its max level
                if (shopData.shopItems[currentIndex].unlockedBodyLevel < shopData.shopItems[currentIndex].carLevelsData.Length - 1)
                {
                    upgradeBodyBtn.interactable = true;                     //make upgradeBtn interactable true
                    int nextLevelIndex = shopData.shopItems[currentIndex].unlockedBodyLevel + 1;
                    //set the next level as value of upgrade button text
                    upgradeBodyBtnText.text = /*"Upgrade Cost:" +*/
                        (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].bodyUnlockCost / 1000)+"K";
                    if (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].bodyUnlockCost > totalCoins) upgradeBodyBtn.interactable = false;

                }
                else   //if unlockLevel of current item is equal to max level
                {
                    upgradeBodyBtn.interactable = false;                    //make upgradeBtn interactable false
                    upgradeBodyBtnText.text = "Max";
                }
#endregion
 #region Attack Speed
                //if unlockLevel of current item is less than its max level
                if (shopData.shopItems[currentIndex].unlockedAttackSpeedLevel < shopData.shopItems[currentIndex].carLevelsData.Length - 1)
                {
                    upgradeAttackSpeedBtn.interactable = true;                     //make upgradeBtn interactable true
                    int nextLevelIndex = shopData.shopItems[currentIndex].unlockedAttackSpeedLevel + 1;
                    //set the next level as value of upgrade button text
                    upgradeAttackSpeedBtnText.text = 
                        (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].attackSpeedUnlockCost / 1000)+"K";
                    if (shopData.shopItems[currentIndex].carLevelsData[nextLevelIndex].attackSpeedUnlockCost > totalCoins) upgradeAttackSpeedBtn.interactable = false;
                }
                else   //if unlockLevel of current item is equal to max level
                {
                    upgradeAttackSpeedBtn.interactable = false;                    //make upgradeBtn interactable false
                    upgradeAttackSpeedBtnText.text = "Max";
                }
#endregion




            }
            else if (!shopData.shopItems[currentIndex].isUnlocked)  //if current item is not unlocked
            {
                upgradeHealthBtn.interactable = false;
                upgradeBodyBtn.interactable = false;
                upgradeAttackSpeedBtn.interactable = false;                        //make upgradeBtn interactable false
                upgradeHealthBtnText.text = ""; upgradeBodyBtnText.text = ""; upgradeAttackSpeedBtnText.text = "";
            }
        }
        private void UpgradeCoins(int value)
        {
            totalCoins -= value;
            if (totalCoins >= 1000)
            {
                totalCoinsText.text = (totalCoins / 1000) + "K";
            }
            else totalCoinsText.text = totalCoins.ToString();

            PlayerPrefs.SetInt("Coins", totalCoins);

            
        }
        void PlayButton()
        {
            SceneManager.LoadScene(1);
        }
    }
    
}