using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy : PoolableObject, IDamageable
{
    public EnemyMovement Movement;
    public AttackRadius attackRadius;
    public NavMeshAgent Agent;
    public Rigidbody rb;
    public int Health;
    float maxHealth;
    public Image healthBar;
    [NonSerialized] public float xp;
    [SerializeField] Animator animator;
    float attackDelay;

    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";

    private void Awake()
    {
        attackRadius.OnAttack += OnAttack;
        maxHealth = (float)Health;

    }
    public void Start()
    {
        maxHealth = (float)Health;
        UpgradeHealthBar(Health);
    }
    private void OnAttack(IDamageable Target)
    {


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
        while (time < attackDelay)
        {

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = lookRotation;
    }

    public void attak()
    {
        PoolableObject poolableObject = attackRadius.BulletPool.GetObject();
        Debug.Log("null1");
        var  bullet = poolableObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            Debug.Log("dqsds");
            bullet.transform.localPosition = gameObject.transform.position;
            bullet.transform.rotation = Agent.transform.rotation;
        }

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
        UpgradeHealthBar(Health);
        if (Health <= 0.1f)
        {
            GetComponent<CapsuleCollider>().enabled = false;

            StartCoroutine(OnDie());
        }
    }
    public void UpgradeHealthBar(int value)
    {

        healthBar.fillAmount = ((float)value) / maxHealth;

    }

    IEnumerator OnDie()
    {
        attackRadius.StopAllCoroutines();
        Agent.enabled = false;
        rb.isKinematic = false;
        animator.SetTrigger("Dead");
        progressLVL.Instance.OnFillProgressXP(xp);

        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        

    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void Update()
    {
        animator.SetFloat("Speed", (Agent.velocity.magnitude) / 10);

    }
}
