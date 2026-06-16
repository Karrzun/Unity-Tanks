using UnityEngine;


/* Green tank uses
 *     - Stationary Movement
 *     - Predictive Aiming
 */
public class GreenTank : EnemyTank
{
    [SerializeField] private ProjectileSpeed projectileSpeedForPredictiveAiming;

    private static readonly float MAXIMUM_PROJECTILE_DISTANCE = 0.8f;

    private Vector3 targetDirection;
    private float minProjectileDistance;

    private const float SECONDS_EBTWEEN_DIRECTION_UPDATES = 0.8f;
    private float secondsSinceLastUpdate = 0.0f;
    private static float updateOffset = 0.0f;


    protected override void Awake()
    {
        base.Awake();

        secondsSinceLastUpdate = updateOffset;
        updateOffset += 0.1f;
    }

    protected override void Aim() // predictive aiming
    {
        secondsSinceLastUpdate += Time.deltaTime;
        if (secondsSinceLastUpdate > SECONDS_EBTWEEN_DIRECTION_UPDATES)
        {
            targetDirection = PredictiveAiming.GetTargetDirection(transform, playerTransform, 2, projectileSpeedForPredictiveAiming.Value, out minProjectileDistance);
            secondsSinceLastUpdate = 0.0f;
        }
        aiming.AimIntoDirection(targetDirection);
    }

    protected override void TryShoot()
    {
        if (IsAimingAtTargetPosition() && minProjectileDistance <= MAXIMUM_PROJECTILE_DISTANCE)
        {
            shooting.Shoot();
        }
    }

    private bool IsAimingAtTargetPosition()
    {
        // dot product --> 1 means facing, -1 is facing opposite
        float dot = Vector3.Dot(headTransform.forward, targetDirection);
        return (Mathf.Approximately(dot, 1f))
            ? true
            : false;
    }

}