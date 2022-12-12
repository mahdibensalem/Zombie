using UnityEngine.AI;
using System.Collections;
using UnityEngine;
public class Enemy : PoolableObject, IDamageable
{
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public int Health;
    private void Awake()
    {
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
    public Transform GetTransform()
    {
        return transform;
    }
}
