using UnityEngine;


public class TrailPool : InfinitePool
{
    public static TrailPool Instance { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void AddPooledObjects()
    {
        GameObject tmp;
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(pooledObject, transform);
            tmp.SetActive(false);
            pooledObjects[i] = tmp;
        }
    }

}