using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Rigidbody))]
public class Bullet1 : PoolableObject
{
    public float AutoDestroyTime = 5f;
    public Rigidbody RigidBody;
    public float Speed = 80f;
    public int Damage = 1;
    protected Transform Target;
    public LayerMask targetMask;
    public float radiusHit;
    Collider[] Hits;
    public int MaxHits = 25;
    public int MaxDamage = 5;
    public int MinDamage = 1;
    public float ExplosiveForce;
    protected const string DISABLE_METHOD_NAME = "Disable";
    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        Hits = new Collider[MaxHits];
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
        {
            if (other.CompareTag("Player")) return;


            int hits = Physics.OverlapSphereNonAlloc(transform.position, radiusHit, Hits, targetMask);
            List<IDamageable> Damageables = new List<IDamageable>();
            for(int i = 0; i < hits; i++)
            {
                Damageables.Add(Hits[i].GetComponent<IDamageable>());

                Debug.Log(Damageables[i]);

            }
            for (int i = 0; i < hits; i++)
            {
                //IDamageable Damageable = targetsInViewRadius[i].GetComponent<IDamageable>();
                if (Hits[i].TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
                {
                    float distance = Vector3.Distance(transform.position, Hits[i].transform.position);
                    rigidbody.AddExplosionForce(ExplosiveForce, transform.position, radiusHit);
                    Damageables[i].TakeDamage(Mathf.FloorToInt(Mathf.Lerp(MaxDamage, MinDamage, distance / radiusHit)));
                    Debug.Log($"Would hit {rigidbody.name} for {Mathf.FloorToInt(Mathf.Lerp(MaxDamage, MinDamage, distance / radiusHit))}");
                    //if (Hits[i].GetComponent<Enemy>().Health<=0)
                    //{


                    //    //Damageable.TakeDamage(Mathf.FloorToInt(Mathf.Lerp(MaxDamage, MinDamage, distance / radiusHit)));
                    //    //Damageable.TakeDamage(Damage);

                    //}
                }
            }

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