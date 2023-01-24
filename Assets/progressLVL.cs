using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class progressLVL : MonoBehaviour
{
    public static progressLVL Instance;
    public Image progressLvl;
    public Image progressXP;
    public TextMeshProUGUI XpTXT;
    public int MaxXP;
    [SerializeField] GameObject updatePanel;

    public int numberOfWave;

    public int maxNumberOfWave;
    public List<int> NumberofZombiesInwave;
    public List<Transform> PosSpawnCharacter;
    public TextMeshProUGUI missionTXT;

    public GameObject character;
    public GameObject arrow;
    private void Awake()
    {
        Instance = this;
        XpTXT.text = "0";
        missionTXT.text = "KILL " + NumberofZombiesInwave[0] + " Zombies";

    }
    private void Start()
    {
        OnKillMissionUpdate();
    }
    public void OnFillProgressLVL()
    {
        progressLvl.fillAmount += (1f / maxNumberOfWave);

    }
    public void OnFillProgressXP(float amount)
    {
        if (updatePanel != null)
        {
            if (progressXP.fillAmount <= 0.9f)
            {
                progressXP.fillAmount += amount / MaxXP;

            }
            else
            {
                progressXP.fillAmount = 0f;
                XpTXT.text = (float.Parse((XpTXT.text)) + 1).ToString();

                updatePanel.SetActive(true);


            }
        }
        if (numberOfWave % 2 == 0)
        {
            OnKillMissionUpdate();
        }

    }
    public void OnKillMissionUpdate()
    {
        arrow.SetActive(false);
        missionTXT.text = "KILL " + NumberofZombiesInwave[0] + " Zombies";
        NumberofZombiesInwave[0]--;
        if (NumberofZombiesInwave[0] == 0)
        {
            numberOfWave++;
            NumberofZombiesInwave.Remove(0);
            OnFillProgressLVL();
            OnSaveCharacter();
        }

    }
    void OnSaveCharacter()
    {

        arrow.SetActive(true);
        PosSpawnCharacter[0].GetComponent<SphereCollider>().enabled = true;
        PosSpawnCharacter[0].GetComponent<characterSave>().enabled = true;
        missionTXT.text = "Save The Character";
    }

    public void removePosSpawnCharacter()
    {
        PosSpawnCharacter.Remove(PosSpawnCharacter[0]);
    }
}
