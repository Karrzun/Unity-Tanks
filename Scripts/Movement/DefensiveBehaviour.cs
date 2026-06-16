using UnityEngine;

/*
 * Weight of this MovementBehaviour is 0 - 2
 */
public class DefensiveBehaviour : MovementBehaviour
{
    protected Transform playerTransform;

    private Vector3 direction;
    private float distance, weight;


    protected void Start() => playerTransform = PlayerController.Instance.transform;

    protected override Vector3 CalculateWeightedDirection()
    {
        direction = transform.position - playerTransform.position;
        distance = Vector3.Distance(transform.position, playerTransform.position);
        weight = 2 / (distance + 1);
        return direction.normalized * weight;
    }

}