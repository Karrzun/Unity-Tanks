using UnityEngine;

public class TankTrailDrawer : MonoBehaviour
{
    private Transform myTransform;
    private Vector3 lastDrawPos;

    private void Awake()
    {
        myTransform = transform;
        lastDrawPos = myTransform.position;
    }

    void Update()
    {
        if (Vector3.Distance(lastDrawPos, myTransform.position) < 0.25f)
            return;

        lastDrawPos = myTransform.position;
        TrailPool.Instance.GetPooledObject(out GameObject trail);
        trail.transform.SetPositionAndRotation(myTransform.position, myTransform.rotation);
        trail.SetActive(true);
    }

}