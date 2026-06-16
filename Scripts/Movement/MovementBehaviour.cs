using UnityEngine;


public abstract class MovementBehaviour : MonoBehaviour
{
    public Vector3 WeightedDirection => CalculateWeightedDirection();
    
    protected Move moving;


    protected virtual void Awake()
    {
        moving = gameObject.GetComponent<Move>();
        enabled = (moving != null);
    }

    protected void OnEnable() => moving.Behaviours.Add(this);
    protected void OnDisable() => moving.Behaviours.Remove(this);


    protected abstract Vector3 CalculateWeightedDirection();

}