using UnityEngine;

[CreateAssetMenu(fileName = "Attack Configuration", menuName = "ScriptableObject/Attack Configuration")]
public class AttackScriptableObject : ScriptableObject
{
    public bool IsRanged = false;
    public int Damage = 5;
    public float AttackRadius = 1.5f;
    public float AttackDelay = 1.5f;

    // Ranged Configs
    public Bullet BulletPrefab;
    public Vector3 BulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask LineOfSightLayers;
    public float StoppingDistance;
    public void SetupEnemy(Enemy enemy)
    {
        (
        enemy.attackRadius.Collider == null ? enemy.attackRadius.GetComponent<SphereCollider>() : enemy.attackRadius.Collider).radius = AttackRadius ;
        enemy.attackRadius.AttackDelay = AttackDelay ;
        enemy.attackRadius.Damage = Damage ;
        enemy.Agent.stoppingDistance = StoppingDistance ;
        if (IsRanged)
        {
            RangedAttackRadius rangedAttackRadius = enemy.attackRadius.GetComponent<RangedAttackRadius>();
            enemy.Agent.stoppingDistance = AttackRadius;
            rangedAttackRadius.BulletPrefab = BulletPrefab;
            rangedAttackRadius.BulletSpawnOffset = BulletSpawnOffset;
            rangedAttackRadius.Mask = LineOfSightLayers;

            rangedAttackRadius.CreateBulletPool();
        }
    }
}
