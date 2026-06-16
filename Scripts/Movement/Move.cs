using System;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private MovementSpeed movementSpeed;
    private float speed;

    private Vector3 direction = Vector3.zero;
    private new Rigidbody rigidbody;

    public readonly List<MovementBehaviour> Behaviours = new List<MovementBehaviour>();

    private bool canMove = true;
    public Action OnCanMoveToggled;


    private void Awake()
    {
        speed = movementSpeed.Value * (0.1f / Time.fixedDeltaTime); // make the speed independant of the Physics timestep
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update() => direction = GetDirection();

    private void FixedUpdate()
    {
        if (!canMove || direction.magnitude == 0f)
            return;

        Quaternion orientation1 = Quaternion.LookRotation(direction);
        Quaternion orientation2 = Quaternion.LookRotation(-direction);

        Quaternion targetOrientation = (Quaternion.Angle(transform.rotation, orientation1) < Quaternion.Angle(transform.rotation, orientation2))
            ? orientation1
            : orientation2;

        Quaternion auxiliaryRotation = Quaternion.Lerp(transform.rotation, targetOrientation, 0.2f);
        rigidbody.MoveRotation(auxiliaryRotation);

#if UNITY_EDITOR
        speed = movementSpeed.Value * (0.1f / Time.fixedDeltaTime);
#endif
        Vector3 expectedVelocity = speed * Time.deltaTime * direction; // Yes, .deltaTime is correct
        rigidbody.AddForce(expectedVelocity - rigidbody.velocity, ForceMode.VelocityChange);
    }

    private Vector3 GetDirection()
    {
        Vector3 result = Vector3.zero;
        foreach (MovementBehaviour behaviour in Behaviours)
        {
            result += behaviour.WeightedDirection;
        }
        return result.normalized;
    }

    public void ToggleCanMove()
    {
        canMove = !canMove;
        OnCanMoveToggled?.Invoke();
    }

}
