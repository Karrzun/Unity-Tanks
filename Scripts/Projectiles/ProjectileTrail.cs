using UnityEngine;

public class ProjectileTrail : MonoBehaviour
{
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private Vector3 vfxOffset;

    private GameObject trailVfx;
    private float vfxLifetime;

    private void Awake()
    {
        vfxLifetime = vfxPrefab.GetComponent<ParticleSystem>().main.startLifetime.constant;
    }

    public void StartProjectileTrail()
    {
        trailVfx = Instantiate(vfxPrefab, transform);
        trailVfx.transform.localPosition = vfxOffset;
    }

    public void StopProjectileTrail()
    {
        trailVfx.GetComponent<ParticleSystem>().Stop();
        trailVfx.transform.SetParent(null, true);
        trailVfx.GetComponent<VfxKiller>().KillInSeconds(vfxLifetime);
    }

}