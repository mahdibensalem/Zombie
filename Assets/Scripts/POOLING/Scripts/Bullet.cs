using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : PoolableObject
{
    public float AutoDestroyTime = 5f;
    public Rigidbody RigidBody;
    public float Speed = 80f;
    public int Damage = 1;
    protected Transform Target;


    protected const string DISABLE_METHOD_NAME = "Disable";
    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    protected virtual void OnEnable()
    {

        CancelInvoke(DISABLE_METHOD_NAME);
        Invoke(DISABLE_METHOD_NAME, AutoDestroyTime);
    }
    public virtual void Spawn(Vector3 Forward, int Damage, Transform Target)
    {
        this.Damage = Damage;
        //this.Target = Target;
        //RigidBody.AddForce(Forward * Speed, ForceMode.VelocityChange);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable;
        if (other.TryGetComponent<IDamageable>(out damageable))
        {   if (other.CompareTag("Player")) return;
            damageable.TakeDamage(Damage);
            Disable();
        }
    }

    protected void Disable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        RigidBody.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}