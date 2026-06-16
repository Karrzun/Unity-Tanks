using UnityEngine;

/*
 * Weight of this MovementBehaviour is 0 - MineData.DETECTION_RADIUS
 */
public class MineAvoidance : MovementBehaviour
{
    private int playerID;
    private bool canDeployMines;

    protected void Start()
    {
        playerID = PlayerController.Instance.gameObject.GetInstanceID();
        canDeployMines = gameObject.GetComponent<Supply>() != null && gameObject.GetComponent<Supply>().PoolSize > 0;
    }

    protected override Vector3 CalculateWeightedDirection()
    {
        Vector3 result = Vector3.zero;
        Vector3 direction;
        float distance;
        float maxWeight = 0;

        // Detect all mines within a given radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, MineData.DETECTION_RADIUS);
        foreach (Collider col in colliders)
        {
            if (col.gameObject.GetComponent<MineCollisionBehaviour>() == null)
                continue;

            MineCollisionBehaviour mine = col.gameObject.GetComponent<MineCollisionBehaviour>();

            if (CanDetectMine(mine) == false)
                continue;

            direction = (transform.position - col.transform.position).normalized;
            distance = Vector3.Distance(transform.position, col.transform.position);
            float weight = MineData.DETECTION_RADIUS / (distance + 1f);
            maxWeight = Mathf.Max(weight, maxWeight);
            result += direction * weight;
        }

        return result.normalized * maxWeight;
    }

    // Enemy tanks' mines can always be detected. If this tank can deploy
    // mines itself, it can also detect the player's mines
    private bool CanDetectMine(MineCollisionBehaviour mine) => mine.OwnerID != playerID || canDeployMines;

}