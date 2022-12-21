using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject UpgradeButtons ;
    [SerializeField] List<ButtonScriptableObject> Upgrades;



    //public List<GameObject> _UpgradeButttttttons ;
    private void Start()
    {
        for (int i =0; i < 3; i++)
        {
            GameObject button= Instantiate(UpgradeButtons,transform);
            button.GetComponent<ButtonConfig>().myconfig = Upgrades[i];
        }
    }

    //void GetUpdgrade()
    //{

    //    int random;
    //    int upgradeLength = UpgradeButtons.Count;
    //    for (int i = 0; i<3; i++)
    //    {
    //         random = Random.Range(0, 3) ;
    //        GameObject button= Instantiate(_UpgradeButttttttons[random], transform);
    //        _UpgradeButttttttons.Remove(_UpgradeButttttttons[random]);
    //    }
    //    _UpgradeButttttttons = UpgradeButtons;

    //}

    
}
