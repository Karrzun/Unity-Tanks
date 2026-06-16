using UnityEngine;

public class MineExplode : MonoBehaviour
{
    [SerializeField] private EffectObject mineExplosion;

    public void Explode()
    {
        EffectManager.Instance.PlayEffect(mineExplosion, gameObject);
        gameObject.SetActive(false);

        //DebugUtility.DrawSphere(transform.position, MineData.EXPLOSION_RADIUS, Color.green, 3.0f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, MineData.EXPLOSION_RADIUS);
        foreach (Collider collider in colliders)
        {
            IExplodable explodable = collider.gameObject.GetComponent<IExplodable>();
            explodable?.OnExplode();
        }
    }

}