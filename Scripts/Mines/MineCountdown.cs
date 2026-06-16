using System.Collections;
using UnityEngine;

public class MineCountdown : MonoBehaviour
{
    [Header("Timer Durations (seconds)")]
    [SerializeField] private float deploymentToCountdown = 8.0f;
    [SerializeField] private float countdownToExplosion = 2.0f;

    private Material mat;
    private WaitForSeconds phaseOne;
    private MineExplode explode;


    private void Awake()
    {
        mat = gameObject.GetComponentInChildren<MeshRenderer>().material;
        phaseOne = new WaitForSeconds(deploymentToCountdown);
        explode = gameObject.GetComponent<MineExplode>();
    }

    private void OnEnable()
    {
        mat.color = Color.yellow;
        StartCoroutine(StartMineCoroutine());
    }

    private IEnumerator StartMineCoroutine()
    {
        yield return phaseOne;
        for (float t = 0; t < countdownToExplosion; t += Time.deltaTime)
        {
            mat.color = Color.Lerp(Color.yellow, Color.red, t / countdownToExplosion);
            yield return null;
        }
        explode.Explode();
    }

}