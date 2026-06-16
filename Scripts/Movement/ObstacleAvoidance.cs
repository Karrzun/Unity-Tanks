using UnityEngine;

/*
 * Weight of this MovementBehaviour is 0 - 2
 */
public class ObstacleAvoidance : MovementBehaviour
{
    private Ray[] rays = new Ray[8];

    private Vector3 sum;
    private float weight;

    protected override Vector3 CalculateWeightedDirection()
    {
        InitRays();

        sum = Vector3.zero;
        for (int i = 0; i < rays.Length; i++)
        {
            sum += GetWeightedDirectionForDirection(i);
        }
        return (sum / rays.Length);
    }

    private void InitRays()
    {
        int numRays = rays.Length;
        float angleStep = 360 / numRays;
        Vector3 origin = transform.position + Vector3.up * 0.2f;
        Vector3 direction;

        for (int i = 0; i < numRays; i++)
        {
            direction = Quaternion.Euler(0, i * angleStep, 0) * Vector3.right;
            Ray r = new Ray(origin, direction);
            rays[i] = r;
        }
    }

    private Vector3 GetWeightedDirectionForDirection(int i)
    {
        // Ignore obstacles that are further away than given distance
        if (!Physics.Raycast(rays[i], out RaycastHit hitInfo, 2.0f))
            return Vector3.zero;

        // Only check for GameObjects with an Obstacle component
        Obstacle obstacle = hitInfo.transform.GetComponentInChildren<Obstacle>();
        if (obstacle == null)
            return Vector3.zero;

        // TODO get the angle with which the obstacle was hit
        //float angle = Vector3.SignedAngle(rays[i].direction, hitInfo.normal, Vector3.up); // ?
        weight = 2 / (hitInfo.distance + 1);
        return -1 * rays[i].direction.normalized * weight;
    }

}