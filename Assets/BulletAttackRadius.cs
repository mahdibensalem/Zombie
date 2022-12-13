using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BulletAttackRadius : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public List<Transform> visibleTargets = new List<Transform>();



    public Bullet BulletPrefab;
    public int maxBullet = 5;

    private ObjectPool BulletPool;
    private Bullet bullet;

    public SphereCollider Collider;

    public List<IDamageable> Damageables = new List<IDamageable>();
    public int Damage = 10;
    public float AttackDelay = 0.5f;
    public delegate void AttackEvent(IDamageable Target);
    public AttackEvent OnAttack;
    protected Coroutine AttackCoroutine;

    protected virtual void Awake()
    {
        Collider = GetComponent<SphereCollider>();
        BulletPool = ObjectPool.CreateInstance(BulletPrefab, maxBullet);

    }
    //void Start()
    //{
    //    StartCoroutine("FindTargetsWithDelay", .2f);
    //}

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    private void Update()
    {
        FindVisibleTargets();
    }

    void Attacke()
    {
        IDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;
        if (Damageables.Count > 0)
        {
            Debug.Log("attack");
            for (int i = 0; i < Damageables.Count; i++)
            {
                Transform damageableTransform = Damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = Damageables[i];
                }
            }

            if (closestDamageable != null)
            {
                Debug.Log("ee2");
                PoolableObject poolableObject = BulletPool.GetObject();
                if (poolableObject != null)
                {
                    bullet = poolableObject.GetComponent<Bullet>();

                    bullet.transform.position = transform.position;
                    bullet.transform.localRotation = Quaternion.LookRotation(-closestDamageable.GetTransform().forward);
                    bullet.Spawn(transform.forward, Damage, closestDamageable.GetTransform());
                }
                //OnAttack?.Invoke(closestDamageable);
                //closestDamageable.TakeDamage(Damage);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            Damageables.RemoveAll(DisabledDamageables);
        }

        AttackCoroutine = null;


    }
    protected virtual IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        yield return Wait;

        IDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;

        while (Damageables.Count > 0)
        {
            Debug.Log("attack");
            for (int i = 0; i < Damageables.Count; i++)
            {
                Transform damageableTransform = Damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = Damageables[i];
                }
            }

            if (closestDamageable != null)
            {
                Debug.Log("ee2");
                PoolableObject poolableObject = BulletPool.GetObject();
                if (poolableObject != null)
                {
                    bullet = poolableObject.GetComponent<Bullet>();

                    bullet.transform.position = transform.position;
                    bullet.transform.localRotation = Quaternion.LookRotation(-closestDamageable.GetTransform().forward);
                    bullet.Spawn(transform.forward, Damage, closestDamageable.GetTransform());
                }
                OnAttack?.Invoke(closestDamageable);
                //closestDamageable.TakeDamage(Damage);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return Wait;

            Damageables.RemoveAll(DisabledDamageables);
        }

        AttackCoroutine = null;
    }
    void FindVisibleTargets()
    {
        Damageables.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        Debug.Log("targetsInViewRadius" + targetsInViewRadius.Length);
        for (int i = 0; i < targetsInViewRadius.Length; i++) 
        {
            IDamageable damageable = targetsInViewRadius[i].GetComponent<IDamageable>();
            Transform target = damageable.GetTransform();
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    //visibleTargets.Add(target);
                    Damageables.Add(damageable);
                    if (AttackCoroutine == null)
                    {
                        AttackCoroutine = StartCoroutine(Attack());
                    }
                }
                else
                {
                    if(AttackCoroutine!=null)
                    StopCoroutine(AttackCoroutine);
                    AttackCoroutine = null;
                    return;
                }
            }
            
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {

        angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    protected bool DisabledDamageables(IDamageable Damageable)
    {
        return Damageable != null && !Damageable.GetTransform().gameObject.activeSelf;
    }




    //protected virtual void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("damagable"+Damageables.Count);
    //    IDamageable damageable = other.GetComponent<IDamageable>();
    //    if (damageable != null)
    //    {
    //        Transform target = damageable.GetTransform();
    //        Vector3 dirToTarget = (target.position - transform.position).normalized;
    //        if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
    //        {
    //            float dstToTarget = Vector3.Distance(transform.position, target.position);
    //            if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
    //            {
    //                Damageables.Add(damageable);
    //                Debug.Log("name of target : " + target.gameObject.name);
    //            }
    //        }
    //        if (AttackCoroutine == null)
    //        {
    //            AttackCoroutine = StartCoroutine(Attack());
    //        }
    //    }
    //}

    //protected virtual void OnTriggerExit(Collider other)
    //{
    //    IDamageable damageable = other.GetComponent<IDamageable>();
    //    if (damageable != null)
    //    {
    //        Damageables.Remove(damageable);
    //        if (Damageables.Count == 0 )
    //        {
    //            StopCoroutine(AttackCoroutine);
    //            AttackCoroutine = null;
    //        }
    //    }
    //}







    ///////////////////////////////////**********************************
    /// <summary>
    /// 
    /// 
    /// </summary>


    //void Start()
    //{
    //    StartCoroutine("FindTargetsWithDelay", .2f);
    //}


    //IEnumerator FindTargetsWithDelay(float delay)
    //{
    //    while (true)

    //    {
    //        yield return new WaitForSeconds(delay);
    //        FindVisibleTargets();
    //    }
    //}

}
