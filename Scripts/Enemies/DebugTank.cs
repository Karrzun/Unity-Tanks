using UnityEngine;

public class DebugTank : EnemyTank
{
    [SerializeField] private ProjectileSpeed projectileSpeedForPredictiveAiming;

    private Vector3 targetDirection;
    private float minProjectileDistance;

    protected override void Aim() // predictive aiming
    {
        targetDirection = PredictiveAiming.GetTargetDirection(transform, playerTransform, 2, projectileSpeedForPredictiveAiming.Value, out minProjectileDistance);
        aiming.AimIntoDirection(targetDirection);
    }

    protected override void TryShoot()
    {
        if (IsAimingAtTargetPosition() && minProjectileDistance < 1.5f)
        {
            shooting.Shoot();
        }
    }

    private bool IsAimingAtTargetPosition()
    {
        // dot product --> 1 means facing, -1 is facing opposite
        float dot = Vector3.Dot(headTransform.forward, targetDirection);
        return (dot > 0.99f)
            ? true
            : false;
    }

}