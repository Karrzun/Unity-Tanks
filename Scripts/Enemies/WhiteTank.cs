using System.Collections;
using UnityEngine;

/* White tank uses
 *     - Offensive Movement
 *     - Non-Predictive Aiming (default)
 */
public class WhiteTank : EnemyTank
{
    [SerializeField] protected EffectObject enemyVanished;

    protected WaitForSeconds initialDisplayTime;
    protected MeshRenderer[] renderers;


    protected override void Awake()
    {
        base.Awake();
        initialDisplayTime = new WaitForSeconds(0.1f);
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(MakeTransparentCoroutine());
    }

    private IEnumerator MakeTransparentCoroutine()
    {
        SetMeshRenderersActive(true);
        yield return initialDisplayTime;
        SetMeshRenderersActive(false);
        EventManager<GameObject>.TriggerEvent(EventKey.EnemyVanished, gameObject);
        EffectManager.Instance.PlayEffect(enemyVanished, gameObject);
    }

    private void SetMeshRenderersActive(bool active)
    {
        foreach (MeshRenderer mr in renderers)
        {
            mr.enabled = active;
        }
    }

}