using System;
using UnityEngine;

public enum TankBehaviour
{
    Stationary,
    Defensive,
    Offensive,
    Incautious
}

public abstract class EnemyTank : MonoBehaviour, IHittable, IExplodable
{
    [Header("Behaviour")]
    [SerializeField] protected TankBehaviour behaviour = TankBehaviour.Defensive;
    [SerializeField] protected Transform headTransform;

    [Header("Effects")]
    [SerializeField] protected EffectObject tankExplosion;

    protected Transform playerTransform;

    protected Aiming aiming;
    protected Shooting shooting;
    protected Mining mining;
    protected bool canDeployMines = false;
    
    protected virtual void Awake()
    {
        aiming = gameObject.GetComponent<Aiming>();
        shooting = gameObject.GetComponent<Shooting>();
        mining = gameObject.GetComponent<Mining>();
        AddTankBehaviourComponents();
    }

    protected virtual void Start()
    {
        playerTransform = PlayerController.Instance.transform;
        canDeployMines = (behaviour != TankBehaviour.Stationary)
            && gameObject.GetComponent<Supply>() != null
            && gameObject.GetComponent<Supply>().PoolSize > 0;
    }

    protected virtual void OnEnable() => GameManager.Instance.RegisterEnemyTank();
    protected virtual void OnDisable() => GameManager.Instance.DeregisterEnemyTank();


    protected virtual void Update()
    {
        Aim();
        //Debug.DrawRay(headTransform.position, headTransform.forward * 5f, Color.green);
        TryShoot();
        TryDeployMine();
    }

    protected void AddTankBehaviourComponents()
    {
        if (behaviour == TankBehaviour.Stationary)
            return;

        gameObject.AddComponent<ObstacleAvoidance>();
        gameObject.AddComponent<MineAvoidance>();
        gameObject.AddComponent<MovementIntervalls>();
        gameObject.AddComponent<RandomMovement>();

        gameObject.AddComponent(BehaviourFactory.GetBehaviour(behaviour.ToString() + "Behaviour"));
    }

    protected virtual void Aim() // non-predictive aiming
    {
        Vector3 targetDirection = playerTransform.position - headTransform.position;
        aiming.AimIntoDirection(targetDirection);
    }

    protected virtual void TryShoot()
    {
        Ray ray = new Ray(headTransform.position, headTransform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo))
            return;

        // Only shoot if the first thing hit is the player
        if (hitInfo.transform.CompareTag("Player"))
            shooting.Shoot();
    }

    protected virtual void TryDeployMine()
    {
        if (canDeployMines == false)
            return;

        float chance = UnityEngine.Random.Range(0f, 10f) - Vector3.Distance(transform.position, playerTransform.position);
        if (chance < 0)
            return;

        // Prevent deploying a mine directly beneath an other enemy tank or close to other mines
        Collider[] colliders = Physics.OverlapSphere(position: transform.position, radius: MineData.EXPLOSION_RADIUS + 0.1f);
        foreach (Collider collider in colliders)
        {
            if ((collider.CompareTag("Enemy") && collider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
                || collider.CompareTag("Mine"))
                return;
        }

        mining.Deploy();
    }

    public void OnHit(Collision collision, GameObject projectile)
    {
        projectile.GetComponent<DeactivateProjectile>().DestroyProjectile();
        Die();
    }

    public void OnExplode() => Die();

    private void Die()
    {
        if (gameObject.activeSelf == false)
            return;

        gameObject.SetActive(false);
        EventManager<GameObject>.TriggerEvent(EventKey.EnemyDied, gameObject);
        EffectManager.Instance.PlayEffect(tankExplosion, gameObject);
    }

}
