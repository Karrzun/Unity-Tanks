using UnityEngine;

/* Brown tank uses
 *     - Stationary Movement
 *     - Random Aiming
 */
public class BrownTank : EnemyTank
{
    private int direction = 0;
    private float timer;


    protected new void Awake()
    {
        base.Awake();
        timer = Random.Range(1.0f, 2.0f);
    }

    protected override void Aim() // random aiming
    {
        aiming.Rotate(direction);

        if (timer >= 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        timer = Random.Range(1.0f, 2.0f);
        direction = Random.Range(-1, 2);
    }

}