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
    float addedValue;
    int addedValueInt;


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
        addedValueInt = (int)addedValue;
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
        if(imageIndex <= myconfig.images.Count-1)
        image.sprite = myconfig.images[imageIndex];



        if (myconfig.action == ButtonScriptableObject.actionMethode.addHealth)
        {
            CarMouvment.instance.health += addedValue;
            CarMouvment.instance.UpgradeHealthBar();
        }
        else if (myconfig.action == ButtonScriptableObject.actionMethode.addAttackSpeed)
        {

            BulletAttackRadius.Instance.StopCoroutine(BulletAttackRadius.Instance.Attack());
            BulletAttackRadius.Instance.AttackDelay -= addedValue;
            BulletAttackRadius.Instance.StartCoroutine(BulletAttackRadius.Instance.Attack());

            ///////////////////////////////////////////////////
        }
        else if (myconfig.action == ButtonScriptableObject.actionMethode.addDamage)
        {
            BulletAttackRadius.Instance.AddDamage(addedValueInt);
        }
        else if (myconfig.action == ButtonScriptableObject.actionMethode.addRange)
        {
            BulletAttackRadius.Instance.AddRangeRadius(addedValueInt);
        }
        else if (myconfig.action == ButtonScriptableObject.actionMethode.addViewAngle)
        {
            BulletAttackRadius.Instance.AddAngle(addedValueInt);
        }
        
        OnExit();
    }
    public void OnExit()
    {
        button.transform.parent.gameObject.SetActive(false);
        if (imageIndex >= myconfig.images.Count )
        {

            Destroy(gameObject);
            return;
        }
    }

}
