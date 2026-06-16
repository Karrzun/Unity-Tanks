using UnityEngine;

public class DeactivateProjectile : MonoBehaviour, IHittable, IExplodable
{
    [SerializeField] private EffectObject projectileExplosion;

    private ProjectileTrail projectileTrail;

    private void Awake()
    {
        projectileTrail = gameObject.GetComponent<ProjectileTrail>();
    }

    public void OnHit(Collision collision, GameObject projectile)
    {
        projectile.GetComponent<DeactivateProjectile>().DestroyProjectile();
        DestroyProjectile();
    }

    public void OnExplode() => DestroyProjectile();

    public void DestroyProjectile()
    {
        EffectManager.Instance.PlayEffect(projectileExplosion, gameObject);
        projectileTrail.StopProjectileTrail();
        gameObject.SetActive(false);
    }
}