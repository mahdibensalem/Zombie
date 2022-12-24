using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConfig : MonoBehaviour
{

    public ButtonScriptableObject myconfig;

    [SerializeField] TextMeshProUGUI Description;
    Button button;
    public Image image;
    public List<Sprite> images;
    int addedValue;



    int imageIndex ;
    private void Awake()
    {
        imageIndex = 0;

        button = GetComponent<Button>();
        image = GetComponent<Image>();

    }
    private void Start()
    {
        Description.text = myconfig.description;
        image.sprite = myconfig.images[imageIndex];
        addedValue = myconfig.addedValue;
        button.onClick.AddListener(action);
        foreach (Sprite sprite in myconfig.images)
        {
            images.Add(sprite);
        }
    }
    void action()
    {

        Time.timeScale = 1;
        imageIndex++;
        image.sprite = myconfig.images[imageIndex];
        if (myconfig.action == ButtonScriptableObject.actionMethode.addHealth)
        {
            CarMouvment.instance.health += addedValue;
        }
        else if (myconfig.action == ButtonScriptableObject.actionMethode.addAttackSpeed)
        {
/////////////////////////////////////////////////////
        }
        else if (myconfig.action == ButtonScriptableObject.actionMethode.addDamage)
        {
            BulletAttackRadius.Instance.Damage += addedValue;
        }
        OnExit();
    }
    public void OnExit()
    {
        button.transform.parent.gameObject.SetActive(false);
    }

}
