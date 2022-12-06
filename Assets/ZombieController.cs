using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieController : MonoBehaviour
{
    [SerializeField] Transform player;
    NavMeshAgent agent;
    [SerializeField] float Distance;

    [Header("health")]
    public float health = 1;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        bool playerIsCloseEnough = distance <= Distance;
        if (playerIsCloseEnough)
        {
            agent.SetDestination(player.transform.position);
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }
}
