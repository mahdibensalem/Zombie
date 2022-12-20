using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = " Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    public Enemy Prefab;
    public int health;
    public AttackScriptableObject AttackConfiguration;
    public float XP;
    public float speed;
    public void SetupEnemy(Enemy enemy)
    {
        enemy.Health = health;
        enemy.Agent.speed = speed;
        enemy.Agent.stoppingDistance = AttackConfiguration.AttackRadius;
        enemy.AttackRadius.Damage = AttackConfiguration.Damage;
        enemy.xp = XP;
        enemy.AttackRadius.AttackDelay = AttackConfiguration.AttackDelay;
        AttackConfiguration.SetupEnemy(enemy);
    }   
}
