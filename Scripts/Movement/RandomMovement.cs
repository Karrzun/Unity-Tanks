using UnityEngine;

/*
 * Weight of this MovementBehaviour is 1
 */
public class RandomMovement : MovementBehaviour
{
    private Vector3 direction = Vector3.zero;
    private readonly float weight = 1f;

    private new void OnEnable()
    {
        base.OnEnable();
        gameObject.GetComponent<Move>().OnCanMoveToggled += ChooseNewDirection;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        gameObject.GetComponent<Move>().OnCanMoveToggled -= ChooseNewDirection;
    }

    protected override Vector3 CalculateWeightedDirection() => direction;

    public void ChooseNewDirection()
    {
        Vector3 randomDirection = Random.Range(-1f, 1f) * Vector3.right + Random.Range(-1f, 1f) * Vector3.forward;
        direction = randomDirection.normalized * weight;
    }
}