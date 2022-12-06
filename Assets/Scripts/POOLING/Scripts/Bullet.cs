using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : AutoDestroyPoolableObject
{
    public Rigidbody RigidBody;
    public float Speed = 80f;

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        transform.localPosition += transform.forward * Speed * Time.deltaTime;
    }
    public override void OnEnable()
    {

        base.OnEnable();

        //transform.rotation = Quaternion.LookRotation(RigidBody.velocity);
        //
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            gameObject.SetActive(false);
            other.GetComponent<Enemy>().Health--;
        }
    }
    public override void OnDisable()
    {
        base.OnDisable();

        RigidBody.velocity = Vector2.zero;
    }
}