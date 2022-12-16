using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Bullet BulletPrefab;
    public int maxBullet = 5;
    private ObjectPool BulletPool;


    public float spawnTimer;
    float _spawnTimer;
    bool findTarget;
    private void Awake()
    {
        BulletPool = ObjectPool.CreateInstance(transform, BulletPrefab, maxBullet);
    }

    private void Start()
    {
        //spawnTimer = 1f / RateOfFire;
        _spawnTimer = 0;
        //StartCoroutine(Fire());
    }
    private void Update()
    {
        if (findTarget)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= spawnTimer)
            {
                Fire();
            }
        }
    }
    private void Fire()
    {
        _spawnTimer = 0;


        PoolableObject instance = BulletPool.GetObject();

        if (instance != null)
        {

            //instance.transform.SetParent(transform, false);
            instance.transform.position = transform.position;
            instance.transform.localRotation = Quaternion.LookRotation(gameObject.transform.forward);
            instance.gameObject.SetActive(true);

        }
    }
}

