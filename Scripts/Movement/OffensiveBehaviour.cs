using UnityEngine;

/*
 * Weight of this MovementBehaviour is 0 - 2
 */
public class OffensiveBehaviour : MovementBehaviour
{
    protected Transform playerTransform;

    private Vector3 direction;
    private float distance, weight;


    protected void Start() => playerTransform = PlayerController.Instance.transform;

    // TODO this is basically IncautiousBehaviour but with a lower weight
    protected override Vector3 CalculateWeightedDirection()
    {
        direction = playerTransform.position - transform.position;
        distance = Mathf.Max(Vector3.Distance(transform.position, playerTransform.position) - 2f, 0f);
        weight = (-2 / (distance + 1)) + 2;
        return direction.normalized * weight;
    }

}