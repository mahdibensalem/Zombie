using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject UpgradeButtons ;
    [SerializeField] List<ButtonScriptableObject> Upgrades;

    int y = 0;

    //public List<GameObject> _UpgradeButttttttons ;
    private void Start()
    {
        for (int i =0; i < Upgrades.Count; i++)
        {
            GameObject button= Instantiate(UpgradeButtons,transform);
            button.GetComponent<ButtonConfig>().myconfig = Upgrades[i];
            button.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(y).gameObject.SetActive(true);
            y+=2;
            y = y % Upgrades.Count-1;
        }
    }
    public void OnActive()
    {

        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(y).gameObject.SetActive(true);
            y += 2;
            y = y % Upgrades.Count - 1;
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < Upgrades.Count; i++)
        {
            transform.GetChild(y).gameObject.SetActive(false);

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
