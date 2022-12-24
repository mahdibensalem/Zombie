using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform Player;
    public int NumberOfEnemiesToSpawn = 50;
    public float SpawnDelay = 1f;
    public List<EnemyScriptableObject> Enemies = new List<EnemyScriptableObject>();
    public SpawnMethod EnemySpawnMethod = SpawnMethod.RoundRobin;

   [SerializeField] private NavMeshTriangulation Triangulation;
    [SerializeField] private List<Vector3> ValidTriangulation;
   [SerializeField] private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    [SerializeField] float MaxRange;


    public bool win = false;


    private void Start()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(transform,Enemies[i].Prefab, NumberOfEnemiesToSpawn));
        }
        Triangulation = NavMesh.CalculateTriangulation();
        for (int j = 0; j < 10; j++)
        {
            SpawnRandomEnemy();

        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);

        int SpawnedEnemies = 1;
        
        while (!win)
        {
            if (EnemySpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(SpawnedEnemies);
            }
            else if (EnemySpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }

            SpawnRandomEnemy();
            SpawnedEnemies++;

            yield return Wait;
        }
    }

    private void SpawnRoundRobinEnemy(int SpawnedEnemies)
    {
        int SpawnIndex = SpawnedEnemies % Enemies.Count;

        DoSpawnEnemy(SpawnIndex);
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, Enemies.Count));
    }

    private void DoSpawnEnemy(int SpawnIndex)
    {
        int i = 0;
        ValidTriangulation.Clear();
        PoolableObject poolableObject = EnemyObjectPools[SpawnIndex].GetObject();
        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();
            Enemies[SpawnIndex].SetupEnemy(enemy);
            for (int j =0; j < Triangulation.vertices.Length; j++)
            {
                if(Vector3.Distance(Triangulation.vertices[j],transform.position)<MaxRange)
                {
                    ValidTriangulation.Add(Triangulation.vertices[j]);
                    i++;
                }
            }
            int VertexIndex = Random.Range(0, ( ValidTriangulation.Count));
            //Debug.Log(("Triangulation.vertices.Length :") + Triangulation.vertices.Length); ///=5720
            //Debug.Log(("ValidTriangulation.Count :") + ValidTriangulation.Count); 
            //Vector3 randomPoint = transform.position + (Random.insideUnitSphere * MaxRange);
            NavMeshHit Hit;
            if (NavMesh.SamplePosition(ValidTriangulation[VertexIndex], out Hit, 0, 1))
            //if (NavMesh.SamplePosition(randomPoint , out Hit, MaxRange, 1))
            {
                enemy.Agent.Warp(Hit.position);
                // enemy needs to get enabled and start chasing now.
                enemy.Movement.Player = Player;
                enemy.Agent.enabled = true;
                enemy.Movement.StartChasing();
            }
            else
            {
                //Debug.LogError($"Unable to place NavMeshAgent on NavMesh. Tried to use {Triangulation.vertices[VertexIndex]}");
            }
        }
        else
        {
            return;
            Debug.LogError($"Unable to fetch enemy of type {SpawnIndex} from object pool. Out of objects?");
        }
    }


    public enum SpawnMethod
    {
        RoundRobin,
        Random
        // Other spawn methods can be added here
    }
}
