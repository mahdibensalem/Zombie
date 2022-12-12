using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class progressLVL : MonoBehaviour
{
   public static progressLVL Instance;
    public Image progressLvl;
    public Image progressXP;
    public TextMeshProUGUI XpTXT;

    private void Awake()
    {
        Instance = this;
        XpTXT.text = "0";
    }
    public void OnFillProgressLVL()
    {
        progressLvl.fillAmount += 0.01f;
        
    }
    public void OnFillProgressXP(float amount)
    {
        if (progressXP.fillAmount < 1)
        {
            progressXP.fillAmount += amount;
        }
        else
        {
            progressXP.fillAmount = 0f;
            progressXP.fillAmount += amount;
            XpTXT.text=(float.Parse((XpTXT.text))+1).ToString();
        }
    }
}
