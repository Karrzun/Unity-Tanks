using UnityEngine;


public class Supply : CappedPool
{

    protected override void InitPool()
    {
        pooledObjects = new GameObject[poolSize];
        GameObject tmp;
        int ownerID = gameObject.GetInstanceID();
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(pooledObject);
            tmp.GetComponent<MineCollisionBehaviour>().SetOwner(ownerID);
            tmp.SetActive(false);
            pooledObjects[i] = tmp;
        }
    }

}