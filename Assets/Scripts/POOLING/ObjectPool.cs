using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private PoolableObject Prefab;
    private int Size;
    private List<PoolableObject> AvailableObjectsPool;
    private List<PoolableObject> MYAllObjectsPool;

    private ObjectPool(PoolableObject Prefab, int Size)
    {
        this.Prefab = Prefab;
        this.Size = Size;
        MYAllObjectsPool = AvailableObjectsPool = new List<PoolableObject>(Size);
    }

    public static ObjectPool CreateInstance(Transform parent, PoolableObject Prefab, int Size)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);

        GameObject poolGameObject = new GameObject(Prefab + " Pool");
        //poolGameObject.transform.parent = parent;
        pool.CreateObjects(poolGameObject);
        
        return pool;
    }

    private void CreateObjects(GameObject parent)
    {
        for (int i = 0; i < Size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
        }
    }
    public void AddDamagePoolableObject(int amount)
    {
        foreach (PoolableObject fire in MYAllObjectsPool)
        {
            fire.gameObject.GetComponent<Bullet>().Damage += amount;
            //else if (fire.gameObject.TryGetComponent<Bullet1>(out Bullet1 bullet1)) bullet1.Damage += amount;
            
        }

    }

    public PoolableObject GetObject()
    {
        if (AvailableObjectsPool.Count == 0) return null;

        PoolableObject instance = AvailableObjectsPool[0];

        AvailableObjectsPool.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;

    }
    public void ReturnObjectToPool(PoolableObject Object)
    {
        AvailableObjectsPool.Add(Object);
    }
}