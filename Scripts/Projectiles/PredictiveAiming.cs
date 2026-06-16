using System.Collections.Generic;
using UnityEngine;


public static class PredictiveAiming
{
    private static readonly float TRAJECTORY_ANGLE = 6;

    private static readonly float TIME_PER_CHECK = 0.04f;
    private static readonly int MAX_ITERATIONS = 80;

    private static readonly float HIT_THRESHHOLD = 0.5f;

    private static Dictionary<float, Vector3> predictedTargetPositionAfterTime = new Dictionary<float, Vector3>();
    private static List<Trajectory> trajectories = new List<Trajectory>();


    public static Vector3 GetTargetDirection(Transform shooter, Transform target, int wallBounces, float bulletSpeed, out float minDist)
    {
        PredictTargetPositions(target);
        CalculatePossibleTrajectories(shooter, target, wallBounces, bulletSpeed);

        float currentDistance = Vector3.Distance(target.position, shooter.position);
        float expectedTimeToHit = currentDistance / bulletSpeed;

        minDist = float.PositiveInfinity;
        Trajectory bestTrajectory = null;

        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            float passedTime = i * TIME_PER_CHECK;
            if (passedTime < expectedTimeToHit / 2)
                continue;

            float minDistStep = FindClosestTrajectoryAfterTime(passedTime, out Trajectory t);
            if (minDistStep < minDist)
            {
                minDist = minDistStep;
                bestTrajectory = t;
            }
            if (minDist < HIT_THRESHHOLD)
            {
                break;
            }
        }

        //Debug.Log("Best Trajectory has a distance of " + minDist);
        return bestTrajectory.StartDirection;
    }

    private static float FindClosestTrajectoryAfterTime(float passedTime, out Trajectory trajectory)
    {
        float minDist = float.PositiveInfinity;
        float dist;
        trajectory = null;

        for (int i = 0; i < trajectories.Count; i++)
        {
            Trajectory t = trajectories[i];
            if (t.MaxTravelTime < passedTime)
            {
                trajectories.Remove(t);
                dist = Vector3.Distance(t.GetPositionAfterTime(t.MaxTravelTime), predictedTargetPositionAfterTime[passedTime]);
            }
            else
            {
                dist = Vector3.Distance(t.GetPositionAfterTime(passedTime), predictedTargetPositionAfterTime[passedTime]);
            }

            if (dist < minDist)
            {
                minDist = dist;
                trajectory = t;
            }
        }

        return minDist;
    }

    private static void CalculatePossibleTrajectories(Transform shooter, Transform target, int wallBounces, float bulletSpeed)
    {
        trajectories.Clear();

        // direct line
        Ray ray = new Ray(shooter.position + Vector3.up * 0.2f, target.position - shooter.position);
        TryAddPossibleTrajectory(ray, wallBounces, bulletSpeed);

        // increasing tilts in both directions
        for (int i = 1; i < 180/TRAJECTORY_ANGLE; i++)
        {
            Vector3 leftTiltedDirection = Quaternion.Euler(0, -i * TRAJECTORY_ANGLE, 0) * ray.direction;
            TryAddPossibleTrajectory(new Ray(ray.origin, leftTiltedDirection), wallBounces, bulletSpeed);

            Vector3 rightTiltedDirection = Quaternion.Euler(0, i * TRAJECTORY_ANGLE, 0) * ray.direction;
            TryAddPossibleTrajectory(new Ray(ray.origin, rightTiltedDirection), wallBounces, bulletSpeed);
        }
    }

    private static void TryAddPossibleTrajectory(Ray ray, int wallBounces, float bulletSpeed)
    {
        Trajectory trajectory = new Trajectory(ray.origin, ray.direction, wallBounces, bulletSpeed);
        if (trajectory.finalHitObject.CompareTag("Enemy") == false)
            trajectories.Add(trajectory);
    }

    private static void PredictTargetPositions(Transform target)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        predictedTargetPositionAfterTime.Clear();
        float checkTimeTotal;
        Vector3 predictedTargetPosition;
        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            checkTimeTotal = i * TIME_PER_CHECK;
            predictedTargetPosition = target.position + (targetRb.velocity * checkTimeTotal);
            predictedTargetPositionAfterTime.Add(checkTimeTotal, predictedTargetPosition);
        }
    }

}