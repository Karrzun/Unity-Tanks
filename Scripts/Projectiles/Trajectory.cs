using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trajectory
{
    private static readonly float SPHERECAST_RADIUS = 0.1f;

    private readonly int maxBounces;
    private readonly float speed;
    private readonly Dictionary<float, Ray> rayCollisionTimes;
    public GameObject finalHitObject { get; private set; }
    public Vector3 StartDirection { get; private set; }

    private Dictionary<float, Vector3> positionAtTime = new Dictionary<float, Vector3>();

    public float MaxTravelTime = 0f;

    public Trajectory(Ray ray, int wallBounces, float bulletSpeed)
    {
        rayCollisionTimes = new Dictionary<float, Ray>();
        maxBounces = wallBounces;
        speed = bulletSpeed;
        StartDirection = ray.direction;

        CreateTrajectory(ray, 0f);
    }

    public Trajectory(Vector3 origin, Vector3 direction, int wallBounces, float bulletSpeed) : this(new Ray(origin, direction), wallBounces, bulletSpeed) { }

    private void CreateTrajectory(Ray ray, float startTime)
    {
        if (!Physics.SphereCast(ray, SPHERECAST_RADIUS, out RaycastHit hitInfo))
        {
            Debug.LogWarning("Trajectory ray doesn't hit anything. This should not happen as there at least should be walls surrounding the arena.");
            return;
        }

        float travelTime = startTime + (hitInfo.distance / speed);
        MaxTravelTime = travelTime;
        rayCollisionTimes.Add(travelTime, ray);
        finalHitObject = hitInfo.transform.gameObject;
        //Debug.DrawRay(ray.origin, ray.direction.normalized * hitInfo.distance, Color.red);

        if (IsNotAWall(finalHitObject.transform))
        {
            //Debug.Log("Collided with something else than a wall. End of trajectory.");
            return;
        }

        if (rayCollisionTimes.Count > maxBounces)
        {
            //Debug.Log("Max bounces reached. End of trajectory.");
            return;
        }

        Vector3 newStartPoint = ray.origin + ray.direction.normalized * hitInfo.distance;
        Vector3 hitNormal = (newStartPoint - hitInfo.point) / SPHERECAST_RADIUS; // hitInfo.normal points from the hitpoint towards the center of the spherecast, not in normal direction, creating a slight offset
        Vector3 newDirection = Vector3.Reflect(ray.direction, hitNormal);
        Ray newRay = new Ray(newStartPoint, newDirection);
        CreateTrajectory(newRay, travelTime);
    }

    public Vector3 GetPositionAfterTime(float time)
    {
        if (time > MaxTravelTime)
            throw new System.ArgumentOutOfRangeException("Parameter must be lower than or equal to MaxTravelTime");

        if (positionAtTime.ContainsKey(time))
            return positionAtTime[time];

        //List<float> collisionTimes = new List<float>(rayCollisionTimes.Keys);
        //collisionTimes.Sort(); // ascending collision times
        float[] collisionTimes = rayCollisionTimes.Keys.ToArray();
        Array.Sort(collisionTimes);

        for (int i = 0; i < collisionTimes.Length; i++)
        {
            if (time < collisionTimes[i])
            {
                Ray r = rayCollisionTimes[collisionTimes[i]];
                float rayStartTime = (i == 0)
                    ? 0f
                    : collisionTimes[i - 1];
                float timeLeft = time - rayStartTime;

                Vector3 pos = r.origin + speed * timeLeft * r.direction;
                positionAtTime.Add(time, pos);
                return pos;
            }
        }

        return -Vector3.one;
    }

    private bool IsNotAWall(Transform tf)
    {
        return tf.GetComponent<Wall>() == null && (tf.parent == null || tf.parent.gameObject.GetComponent<Wall>() == null);
    }

}