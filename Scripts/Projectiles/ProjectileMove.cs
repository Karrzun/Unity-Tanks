using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    [SerializeField] private ProjectileSpeed projectileSpeed;

    [SerializeField] private int wallBounces = 1;
    private int remainingBounces;

    [Header("Debug")]
    [SerializeField] private bool pauseOnEnable = false;

    private new Rigidbody rigidbody;
    private CollisionDetection collisionDetection;


    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        collisionDetection = gameObject.GetComponent<CollisionDetection>();
    }

    private void OnEnable()
    {
        remainingBounces = wallBounces;

#if UNITY_EDITOR
        if (pauseOnEnable)
        {
            Debug.Log($"[{gameObject.name}]: Pausing game");
            EditorApplication.isPaused = true;
            Selection.objects = new Object[] { gameObject };
        }
#endif
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (collisionDetection.IsCollidingWithWall)
            HandleMovementAfterMultiCollision();

        Vector3 expectedVelocity = projectileSpeed.Value * transform.forward;
        //expectedVelocity = speed * Time.deltaTime * transform.forward; // Yes, .deltaTime is correct
        rigidbody.AddForce(expectedVelocity - rigidbody.velocity, ForceMode.VelocityChange);
    }

    private void HandleMovementAfterMultiCollision()
    {
        int collisionPointCount = collisionDetection.NewCollisions.Count;
        //Debug.Log("#Collision Points: " + collisionPointCount);

        Vector3 cumulatedNormal = Vector3.zero;
        for (int i = collisionPointCount - 1; i >= 0 ; i--)
        {
            Wall hitWall = collisionDetection.NewCollisions[i];
            Vector3 hitDirection = hitWall.gameObject.transform.position - transform.position;
            Ray ray = new Ray(transform.position, hitDirection);
            Debug.DrawRay(transform.position, hitDirection, Color.blue, 0.1f);
            Physics.Raycast(ray, out RaycastHit hitInfo);
            cumulatedNormal += hitInfo.normal;
            collisionDetection.MarkCollisionAsHandled(hitWall);
        }
//#if UNITY_EDITOR
//        EditorApplication.isPaused = true;
//#endif
        BounceOffWall(cumulatedNormal.normalized);
    }

    public bool HasBouncesRemaining() => remainingBounces > 0;

    public void BounceOffWall(Vector3 surfaceNormal)
    {
        remainingBounces--;
        Vector3 newDirection = Vector3.Reflect(transform.forward, surfaceNormal);
        Quaternion targetQuaternion = Quaternion.LookRotation(newDirection);
        rigidbody.MoveRotation(targetQuaternion);
    }
}