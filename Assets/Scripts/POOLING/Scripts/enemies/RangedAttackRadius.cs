using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttackRadius : AttackRadius
{
    public NavMeshAgent Agent;
    public Bullet BulletPrefab;
    public Vector3 BulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask Mask;

    float SpherecastRadius = 0.1f;
    private RaycastHit Hit;
    private IDamageable targetDamageable;
    private Bullet bullet;
    private void Start()
    {
        //CreateBulletPool();
        SpherecastRadius = Collider.radius;
    }

    public void CreateBulletPool()
    {
        if (BulletPool == null)
        {
            BulletPool = ObjectPool.CreateInstance(transform, BulletPrefab, Mathf.CeilToInt((1 / AttackDelay) * BulletPrefab.AutoDestroyTime));
        }
    }

    protected override IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        //Debug.Log("Damageables.Count" + Damageables.Count);

        while (Damageables.Count > 0)
        {
            for (int i = 0; i < Damageables.Count; i++)
            {
                if (HasLineOfSightTo(Damageables[0].GetTransform()))
                {

                    targetDamageable = Damageables[0];
                    OnAttack?.Invoke(targetDamageable);
                    Agent.speed = 0;

                    break;
                }
            }

            if (Damageables != null)
            {
                //Debug.Log("Damageables.Count!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + Damageables[0].GetTransform().gameObject.name);
                PoolableObject poolableObject = BulletPool.GetObject();
                if (poolableObject != null)
                {
                    anim.SetTrigger("Attack");
                    //bullet = poolableObject.GetComponent<Bullet>();

                    //bullet.transform.position = transform.position;
                    //bullet.transform.rotation = Agent.transform.rotation;



                    //bullet.transform.localRotation = Quaternion.LookRotation(-targetDamageable.GetTransform().forward);

                    //bullet.Spawn(Agent.transform.forward, Damage, targetDamageable.GetTransform());
                }
            }
            else
            {
                Agent.speed = 8; // no target in line of sight, keep trying to get closer
                targetDamageable = null;

            }

            yield return Wait;

            if (targetDamageable == null || !HasLineOfSightTo(targetDamageable.GetTransform()))
            {
                Agent.speed = 8;

            }
            Damageables.RemoveAll(DisabledDamageables);
        }

        Agent.enabled = true;
        AttackCoroutine = null;
    }
    //public void Attacke()
    //{
    //    Debug.Log("null");
    //    PoolableObject poolableObject = BulletPool.GetObject();
    //    Debug.Log("null1");
    //    bullet = poolableObject.GetComponent<Bullet>();
    //    if (bullet != null)
    //    {
    //        Debug.Log("dqsds");
    //        bullet.transform.localPosition = gameObject.transform.position;
    //        bullet.transform.rotation = Agent.transform.rotation;
    //    }
    //    else Debug.Log("null");
    //}
    private bool HasLineOfSightTo(Transform Target)
    {
        //Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, 60, targetMask);
        //if (targetsInViewRadius.Length > 0)
        if (Physics.SphereCast(transform.position, .1f, ((Target.position) - (transform.position)).normalized, out Hit, SpherecastRadius, Mask))
        {


            IDamageable damageable;
            if (Hit.collider.TryGetComponent<IDamageable>(out damageable))
            {
                return damageable.GetTransform() == Target;
            }
        }

        return false;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (AttackCoroutine == null)
        {
            Agent.speed = 8;

        }


    }
}
