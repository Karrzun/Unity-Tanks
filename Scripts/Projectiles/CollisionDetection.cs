using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private bool autoSyncTransforms;
    [SerializeField] private int solverIterations = 6;

    private ProjectileMove projectileMove;

    public List<Wall> NewCollisions => newCollisions;
    private List<Wall> newCollisions = new List<Wall>();

    private List<Wall> handledCollisions = new List<Wall>();
    public bool IsCollidingWithWall => newCollisions.Count > 0;


    private void Awake()
    {
        Physics.autoSyncTransforms = autoSyncTransforms;
        GetComponent<Rigidbody>().solverIterations = solverIterations;

        projectileMove = GetComponent<ProjectileMove>();
    }

    private void OnDisable()
    {
        newCollisions.Clear();
        handledCollisions.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IHittable target = collision.collider.gameObject.GetComponentInChildren<IHittable>();
        if (target == null) return;

        // Will this collision cause the projectile to explode?
        bool isWall = target.GetType() == typeof(Wall) || target.GetType() == typeof(DestructibleWall);
        if (projectileMove.HasBouncesRemaining() == false || isWall == false)
        {
            target.OnHit(collision, gameObject);
            return;
        }

        Wall wall = (Wall)target;
        RememberCollision(wall);
    }

    private void OnCollisionExit(Collision collision)
    {
        IHittable target = collision.collider.gameObject.GetComponentInChildren<IHittable>();
        if (target == null) return;

        if (target.GetType() == typeof(Wall))
        {
            Wall wall = (Wall)target;
            if (handledCollisions.Contains(wall) == false)
                Debug.LogError("CollisionExit was called before collision was handled");

            handledCollisions.Remove(wall);
        }
    }

    private void RememberCollision(Wall wall)
    {
        if (newCollisions.Contains(wall) || handledCollisions.Contains(wall)) return;
        newCollisions.Add(wall);
    }

    public void MarkCollisionAsHandled(Wall wall)
    {
        if (newCollisions.Contains(wall))
        {
            newCollisions.Remove(wall);
            handledCollisions.Add(wall);
        }
    }

}