using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class BulletAttackRadius : MonoBehaviour
{
    public static BulletAttackRadius Instance;
    public int viewRadius;
    [Range(0, 360)]
    public int viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public List<Transform> visibleTargets = new List<Transform>();
    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    [SerializeField] Vector3 offset;
    public Bullet BulletPrefab;
    public int maxBullet = 5;

    private ObjectPool BulletPool;
    private Bullet bullet;
    public Transform rocket;

    public List<IDamageable> Damageables = new List<IDamageable>();

    public float AttackDelay = 0.5f;
    //public delegate void AttackEvent(IDamageable Target);
    //public AttackEvent OnAttack;
    protected Coroutine AttackCoroutine;

    protected virtual void Awake()
    {
        BulletPool = ObjectPool.CreateInstance(transform, BulletPrefab, maxBullet);
        PoolableObject poolableObject = BulletPool.GetObject();
        Instance = this;

    }
    void Start()
    {

        rocket = gameObject.transform.parent.GetChild(0);
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        viewMeshFilter.gameObject.transform.position = new Vector3(0,0,0) ;
        AttackCoroutine = StartCoroutine(FindTargetsWithDelay());
    }

    IEnumerator FindTargetsWithDelay()
    {
        while (true)
        {
            FindVisibleTargets();
            yield return new WaitForSeconds(AttackDelay);
        }
    }
    //private void Update()
    //{
    //    FindVisibleTargets();
    //}
    public void AddDamage(int value)
    {
        BulletPool.AddDamagePoolableObject(value);
    }
    public void AddRangeRadius(int value)
    {
        viewRadius += value;
    }
    public void AddAngle(int value)
    {
        viewAngle += value;
        viewAngle = Mathf.Clamp(viewAngle, 0, 360);

    }















    void Attacke()
    {
        IDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;

        if (Damageables.Count > 0)
        {
            Debug.Log("attack");
            for (int i = 0; i < Damageables.Count; i++)
            {
                Transform damageableTransform = Damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.localPosition, damageableTransform.localPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = Damageables[i];
                }
            }

            if (closestDamageable != null)
            {

                PoolableObject poolableObject = BulletPool.GetObject();
                if (poolableObject != null)
                {
                    bullet = poolableObject.GetComponent<Bullet>();

                    Vector3 rocketRotation = closestDamageable.GetTransform().position;
                    rocketRotation.y = 3f;

                    bullet.transform.position = transform.position;
                    rocket.LookAt(rocketRotation);
                    bullet.transform.LookAt(closestDamageable.GetTransform().position+offset);
                }
                //OnAttack?.Invoke(closestDamageable);
                //closestDamageable.TakeDamage(Damage);
            }

            //closestDamageable = null;
            //closestDistance = float.MaxValue;

            //Damageables.RemoveAll(DisabledDamageables);
        }


    }

    //public IEnumerator Attack()
    //{
    //    //WaitForSeconds Wait = new WaitForSeconds(AttackDelay);


    //    IDamageable closestDamageable = null;
    //    float closestDistance = float.MaxValue;

    //    while (Damageables.Count > 0)
    //    {
    //        for (int i = 0; i < Damageables.Count; i++)
    //        {
    //            Transform damageableTransform = Damageables[i].GetTransform();
    //            float distance = Vector3.Distance(transform.position, damageableTransform.position);

    //            if (distance < closestDistance)
    //            {
    //                closestDistance = distance;
    //                closestDamageable = Damageables[i];
    //            }
    //        }

    //        if (closestDamageable != null)
    //        {
    //            //transform.LookAt(closestDamageable.GetTransform().position);
    //            PoolableObject poolableObject = BulletPool.GetObject();
    //            if (poolableObject != null)
    //            {
    //                bullet = poolableObject.GetComponent<Bullet>();

    //                Vector3 rocketRotation = closestDamageable.GetTransform().position;
    //                rocketRotation.y = 3f;

    //                bullet.transform.position = transform.position;
    //                rocket.LookAt(rocketRotation);
    //                bullet.transform.LookAt(closestDamageable.GetTransform().position);
    //                //bullet.Spawn(transform.forward, Damage, closestDamageable.GetTransform());
    //            }
    //            //OnAttack?.Invoke(closestDamageable);
    //            //closestDamageable.TakeDamage(Damage);
    //        }

    //        closestDamageable = null;
    //        closestDistance = float.MaxValue;

    //        yield return null;

    //        Damageables.RemoveAll(DisabledDamageables);
    //    }

    //    AttackCoroutine = null;
    //}
    void FindVisibleTargets()
    {
        Damageables.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            IDamageable damageable = targetsInViewRadius[i].GetComponent<IDamageable>();
            Transform target = damageable.GetTransform();
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    //visibleTargets.Add(target);

                    Damageables.Add(damageable);
                    //if (AttackCoroutine == null)
                    //{

                    //    //AttackCoroutine = StartCoroutine(Attack());
                    //}
                }
            }

        }
        Attacke();
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {

        angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    protected bool DisabledDamageables(IDamageable Damageable)
    {
        return Damageable != null && !Damageable.GetTransform().gameObject.activeSelf;
    }

    public void UpdateBullet(float addedValue)
    {
        StopAllCoroutines();
        AttackCoroutine = null;
        AttackDelay -= addedValue;
        StartCoroutine(FindTargetsWithDelay());


    }


    private void LateUpdate()
    {
        DrawFieldOfView();
        //FindVisibleTargets();
    }

    ///////////////////////////////////**********************************
    /// <summary>
    /// 
    /// 
    /// </summary>


    //void Start()
    //{
    //    StartCoroutine("FindTargetsWithDelay", .2f);
    //}


    //IEnumerator FindTargetsWithDelay(float delay)
    //{
    //    while (true)

    //    {
    //        yield return new WaitForSeconds(delay);
    //        FindVisibleTargets();
    //    }
    //}








    ////////////////////// view angle 
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}
