using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 5f;
    float _timeToDestroy;
    [SerializeField] float speed;
 
    void Start()
    {
        _timeToDestroy = timeToDestroy;
        speed = 80f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.localPosition += transform.forward * speed * Time.deltaTime;
            _timeToDestroy -= Time.deltaTime;

            if (_timeToDestroy <= 0)
            {
                gameObject.SetActive(false);
                _timeToDestroy = timeToDestroy;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            Debug.Log("triiger enemy");
            other.GetComponent<ZombieController>().health--;
            gameObject.SetActive(false);
        }
    }
}
