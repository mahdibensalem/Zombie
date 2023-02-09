using UnityEngine;
using UnityEngine.AI;

public class characterSave : MonoBehaviour
{
    [SerializeField] GameObject character;
    NavMeshAgent myCharacterAgent;
    float viewRadius;
    public bool canReachCar = false;
    public LayerMask enemyMask;
    public GameObject arrow;






    Vector3 targetPosition;
    RectTransform PointerArrow;

    private void Awake()
    {
        
    }

    private void Start()
    {
        GameObject mycharacter = Instantiate(character, transform);
        myCharacterAgent = mycharacter.GetComponent<NavMeshAgent>();
        viewRadius = GetComponent<SphereCollider>().radius;

    }
    private void Update()
    {

        ///////////////////////////////////

        //Transform carPos = CarMouvment.instance.GetTransform();
        //var forward = carPos.forward;
        //Vector3 direction = (transform.position - carPos.position);
        ////float angle = Vector3.Angle(direction, carPos.position);
        //float angle = Mathf.Atan2(-direction.x, direction.z)*Mathf.Rad2Deg;

        ////Debug.Log("angle: " + angle);
        //arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        //////////////////////////////////

        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        PointerArrow.localEulerAngles = new Vector3(0, 0, angle);
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen= targetPositionScreenPoint.x<=0 || targetPositionScreenPoint.x >= Screen.width|| targetPositionScreenPoint.y <= 0 || targetPositionScreenPoint.y >= Screen.height;


        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask); 
        if (targetsInViewRadius.Length!=0)
        {
            canReachCar = false;
        }
        else
        {
            canReachCar = true;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (!canReachCar) 
        {
            myCharacterAgent.SetDestination(transform.position);
            myCharacterAgent.speed = 10f;
            return; 
        }
        if (transform.childCount != 0)
        {

            if (other.CompareTag("Player"))
            {
                if( myCharacterAgent != null)
                {
                    myCharacterAgent.SetDestination(other.transform.position);
                    myCharacterAgent.speed = 3.5f;
                }
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myCharacterAgent.SetDestination(transform.position);
            myCharacterAgent.speed = 10f;
        }
    }

}
