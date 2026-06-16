using UnityEngine;


public abstract class InfinitePool : ObjectPool
{
    public override bool GetPooledObject(out GameObject gameObject)
    {
        gameObject = null;
        int size = pooledObjects.Length;
        for (int i = 0; i < size; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                gameObject = pooledObjects[i];
                return true;
            }
        }

        AddPooledObjects();
        gameObject = pooledObjects[size];
        return true;
    }

}