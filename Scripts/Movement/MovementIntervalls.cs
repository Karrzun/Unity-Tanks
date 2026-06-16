using UnityEngine;

public class MovementIntervalls : MonoBehaviour
{
    private Move moving;
    private float timer = 0.3f;


    private void Awake() => moving = gameObject.GetComponent<Move>();
    private void Update() => ToggleIntervall();

    private void ToggleIntervall()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = Random.Range(0.1f, 2.0f);
            moving.ToggleCanMove();
        }
    }

}