using UnityEngine;


public abstract class ObjectPool : MonoBehaviour
{
    [SerializeField] protected GameObject pooledObject;
    [SerializeField] protected int poolSize = 1;

    protected GameObject[] pooledObjects;
    public int PoolSize => pooledObjects.Length;


    protected virtual void Awake() => InitPool();

    protected virtual void InitPool()
    {
        pooledObjects = new GameObject[poolSize];
        AddPooledObjects();
    }

    protected virtual void AddPooledObjects()
    {
        GameObject tmp;
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(pooledObject, transform.parent);
            tmp.SetActive(false);
            tmp.name = pooledObject.name + " (" + i + ")";
            pooledObjects[i] = tmp;
        }
    }

    public abstract bool GetPooledObject(out GameObject gameObject);
}