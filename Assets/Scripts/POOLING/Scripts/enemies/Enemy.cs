using UnityEngine.AI;
using UnityEngine;
public class Enemy : PoolableObject
{
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public int Health;
    private void Awake()
    {
        Health = 1;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        Health = 1;
        Agent.enabled = false;
    }
    private void LateUpdate()
    {
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }

    }
}
