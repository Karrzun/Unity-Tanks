using UnityEngine;

[RequireComponent(typeof(Ammo))]
public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform headTransform;
    [SerializeField] private FireRate fireRate;
    [SerializeField] private EffectObject projectileShot;

    private float reloadTimer = 1.0f;
    private float lastShotTime;
    public static readonly float BULLET_OFFSET = 0.6f;

    private Ammo ammo;


    private void Awake()
    {
        reloadTimer = 10.0f / fireRate.Value;
        lastShotTime = Time.time;
        ammo = gameObject.GetComponent<Ammo>();
    }

    public void Shoot()
    {
#if UNITY_EDITOR
        reloadTimer = 10.0f / fireRate.Value;
#endif
        if (Time.time - lastShotTime < reloadTimer)
            return;

        if (ammo.GetPooledObject(out GameObject projectile) == false)
            return;

        Debug.DrawRay(headTransform.position, headTransform.forward * BULLET_OFFSET, Color.blue, 3.0f);

        if (Physics.Raycast(headTransform.position, headTransform.forward, out RaycastHit hitInfo, BULLET_OFFSET))
        {
            if (hitInfo.distance < 0.8f && hitInfo.transform.GetComponent<Wall>() != null)
                return;
        }

        lastShotTime = Time.time;
        projectile.transform.SetPositionAndRotation(headTransform.position + headTransform.forward * BULLET_OFFSET, headTransform.rotation);
        projectile.SetActive(true);
        projectile.GetComponent<ProjectileTrail>().StartProjectileTrail();
        EffectManager.Instance.PlayEffect(projectileShot, gameObject);
    }

}