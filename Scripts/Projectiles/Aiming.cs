using UnityEngine;


public class Aiming : MonoBehaviour
{
    [SerializeField] private Transform headTransform;
    [SerializeField] private float rotationSpeed = 15f;


    public void AimIntoDirection(Vector3 targetDirection)
    {
        float angle = Vector3.SignedAngle(headTransform.forward, targetDirection, Vector3.up);
        float sign = Mathf.Sign(angle);
        float direction = Mathf.Abs(angle) < Mathf.Abs(sign)
            ? angle
            : sign;
        Rotate(direction);
    }

    // positive values rotate right, negative values rotate left
    public void Rotate(float direction)
    {
        float rotation = direction * rotationSpeed * Time.deltaTime;
        headTransform.Rotate(0, rotation, 0);
    }

}