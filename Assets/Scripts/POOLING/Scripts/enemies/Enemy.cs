using UnityEngine.AI;
using System.Collections;
using UnityEngine;
public class Enemy : PoolableObject, IDamageable
{
    public EnemyMovement Movement;
    public AttackRadius AttackRadius;
    public NavMeshAgent Agent;
    public EnemyScriptableObject EnemyScriptableObject;
    [SerializeField] int Health;


    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";
    private void Awake()
    {
        AttackRadius.OnAttack += OnAttack;
    }
    private void OnAttack(IDamageable Target)
    {
        //Animator.SetTrigger(ATTACK_TRIGGER);

        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }


    public virtual void OnEnable()
    {
        SetUpAgentConfiguration();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }


    /// ********************  IDamageable
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public virtual void SetUpAgentConfiguration()
    {
        Health = EnemyScriptableObject.health;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
