using UnityEngine.AI;
using System.Collections;
using UnityEngine;
public class Enemy : PoolableObject, IDamageable
{
    public EnemyMovement Movement;
    public AttackRadius AttackRadius;
    public NavMeshAgent Agent;
    public Rigidbody rb;
    public int Health;
    public float xp;
    [SerializeField] Animator animator;
    float attackDelay;

    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";
    
    private void Awake()
    {
        AttackRadius.OnAttack += OnAttack;
        
    }
    private void OnAttack(IDamageable Target)
    {


        if (LookCoroutine != null)
        {

            StopCoroutine(LookCoroutine);
        }
        Debug.Log("lookat");
        LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;
        if (Health != 0)
        {
            animator.SetTrigger(ATTACK_TRIGGER);
        }
        while (time<attackDelay)
        {

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime ;
            yield return null;
        }
        transform.rotation = lookRotation;
    }



    public override void OnDisable()
    {
        base.OnDisable();
        Agent.enabled = false;
    }


    /// ********************  IDamageable
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(OnDie());
        }
    }
    IEnumerator OnDie()
    {
        Agent.enabled = false;
        animator.SetTrigger("Dead");
        progressLVL.Instance.OnFillProgressXP(xp);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);

    }
    public Transform GetTransform()
    {
        return transform;
    }

}
