using UnityEngine;

[RequireComponent(typeof(Supply))]
public class Mining : MonoBehaviour
{
    [SerializeField] private EffectObject mineDeployed;
    [SerializeField] private float reloadTimer = 1.0f;
    private float lastDeployTime;

    private Supply supply;

    private void Awake()
    {
        lastDeployTime = Time.time;
        supply = gameObject.GetComponent<Supply>();
    }

    public void Deploy()
    {
        if (Time.time - lastDeployTime < reloadTimer)
            return;

        if (!supply.GetPooledObject(out GameObject mine))
            return;

        lastDeployTime = Time.time;
        mine.transform.position = transform.position;
        mine.SetActive(true);
        EventManager<int>.TriggerEvent(EventKey.MineDeployed, gameObject.GetInstanceID());
        EffectManager.Instance.PlayEffect(mineDeployed, gameObject);
    }

}