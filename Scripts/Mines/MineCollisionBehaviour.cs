using UnityEngine;

public class MineCollisionBehaviour : MonoBehaviour, IExplodable
{
    private MineExplode explode;

    public int OwnerID { get; private set; }

    private void Awake()
    {
        explode = gameObject.GetComponent<MineExplode>();
    }

    public void SetOwner(int id) => OwnerID = id;

    private void OnTriggerEnter(Collider other)
    {
        IExplodable explodable = other.GetComponent<IExplodable>();
        if (explodable == null || other.gameObject.GetInstanceID() == OwnerID)
            return;

        explodable.OnExplode();
        explode.Explode();
    }

    public void OnExplode()
    {
        if (gameObject.activeSelf == false)
            return;

        explode.Explode();
    }
}