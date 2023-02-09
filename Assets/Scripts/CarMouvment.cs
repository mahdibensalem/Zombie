using UnityEngine;
using UnityEngine.AI;
using ShopUpgradeSystem;
using UnityEngine.UI;
public class CarMouvment : MonoBehaviour, IDamageable
{
    public static CarMouvment instance;
    [SerializeField] FloatingJoystick joystick;
    GameObject myCar;
    public float MoveSpeed = 50;
    public float MaxSpeed = 15;
    public float SteerAngle = 20;
    Rigidbody rb;
    Vector3 movedir;
    [SerializeField] float speedStop;
    public GameObject[] myCars;
    public Vector3[] myPosCar;
    private float maxHealth;
    public float health = 100;
    public GameObject[] fire;
    int fireType;
    GameObject firePrefab;
    public float carBody;
    public float attackSpeed;
    
    public Image healthBar;
    public Image shieldBar;
    //public BulletAttackRadius Fire;
    bool canDamage;
    // Variables
    //float Drag = 0.98f;
    //float Traction = 1;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        myCar = Instantiate(myCars[PlayerPrefs.GetInt("SelectedItem")], myPosCar[PlayerPrefs.GetInt("SelectedItem")],Quaternion.identity, transform);
        //myCar.transform.position = myPosCar[PlayerPrefs.GetInt("SelectedItem")];
        SetCarUpgrade();

    }
    
    void SetCarUpgrade()
    {
        int dataIncluded = ShopUI.Instance?.shopData.shopItems.Length ?? 0;

        if (dataIncluded == 0)
        {
            carBody = 1;
            maxHealth = health = 100;
            fireType=0;
        }
        else
        {
            carBody = ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].
            carLevelsData[(ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].unlockedBodyLevel)].body;
            for(int i =0; i <= (ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].unlockedBodyLevel); i++)
            {
                myCar.transform.GetChild(i + 1).gameObject.SetActive(true); 
            }

           maxHealth=health = ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].
            carLevelsData[(ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].unlockedHealthLevel)].health; ;

            attackSpeed = ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].
            carLevelsData[(ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].unlockedAttackSpeedLevel)].attackSpeed; ;

            fireType = ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].
            carLevelsData[(ShopUI.Instance.shopData.shopItems[PlayerPrefs.GetInt("SelectedItem")].unlockedFireTypeLevel)].fireType; 

        }
        firePrefab = Instantiate(fire[fireType], myCar.transform.GetChild(0));
    }

    private void Start()
    {

        firePrefab.GetComponent<BulletAttackRadius>().AttackDelay = attackSpeed;

        UpgradeHealthBar(health);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //MoveForce += movedir;
        //transform.position += MoveForce * Time.fixedDeltaTime;
        //MoveForce *= Drag;
        //MoveForce = Vector3.Lerp(MoveForce.normalized, -movedir, Traction * Time.fixedDeltaTime) * MoveForce.magnitude;
        //rb.AddForce(movedir, ForceMode.Acceleration);

        rb.AddForce(transform.forward * ((Mathf.Abs(joystick.Vertical) + Mathf.Abs(joystick.Horizontal)) % 2) * MoveSpeed);
        movedir = new Vector3(joystick.Horizontal * MoveSpeed, 0, joystick.Vertical * MoveSpeed);
        if (movedir != Vector3.zero)
        {
            Quaternion toRot = Quaternion.LookRotation(movedir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, SteerAngle);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime*speedStop);
        }
        //transform.rotation = Quaternion.LookRotation(rb.velocity);
        //Debug.Log(transform.forward * ((Mathf.Abs(joystick.Vertical) + Mathf.Abs(joystick.Horizontal)) % 2) * MoveSpeed);

        //// Moving


        //MoveForce += transform.forward * MoveSpeed * joystick.Vertical* Time.deltaTime;
        //transform.position += MoveForce * Time.deltaTime;

        //// Steering
        //float steerInput = joystick.Horizontal;
        //transform.Rotate(transform.up * joystick.Horizontal  * SteerAngle * Time.deltaTime);

        //// Drag and max speed limit
        //MoveForce *= Drag;


        //// Traction
        //Debug.DrawRay(transform.position, MoveForce.normalized * 3);
        //Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
        //MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;

        if (rb.velocity.magnitude > 15f)
        {
            canDamage = true;
        }
        else canDamage = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPoint = contact.point;
            Vector3 direction = hitPoint - transform.position;
            if (canDamage)
            {
                enemy.StopAllCoroutines();
                enemy.Movement.StopAllCoroutines();
                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.GetComponent<Rigidbody>().isKinematic = false;
                enemy.rb.AddForce(5 * direction * rb.velocity.magnitude);
                enemy.TakeDamage(enemy.Health);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage / carBody;
        UpgradeHealthBar(health);
    }
    public void UpgradeHealthBar(float value)
    {
        if (value <= maxHealth)
        {
            healthBar.fillAmount = value / maxHealth;
            shieldBar.fillAmount = 0f;
        }
        else
        {
            healthBar.fillAmount = 1f;
            shieldBar.fillAmount = (value % maxHealth) / maxHealth;
        }
    }
}
