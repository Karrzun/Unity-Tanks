using UnityEngine;

public abstract class CappedPool : ObjectPool
{
    public override bool GetPooledObject(out GameObject gameObject)
    {
        gameObject = null;
        for (int i = 0; i < poolSize; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                gameObject = pooledObjects[i];
                return true;
            }
        }
        return false;
    }
}