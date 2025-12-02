using UnityEngine;
using System.Collections;

public class RobotDestroyerSimple : MonoBehaviour
{
    [Header("Destruction Settings")]
    public float destroyDistance = 3.0f;

    [Header("Fallback Settings")]
    public float fallbackDelay = 5f;
    public string fallbackTag = "Cargador";

    private bool fallbackTimerRunning = false;
    private float noInfectadoTimer = 0f;

    private FollowerSpline followerSpline;

    void Start()
    {
        followerSpline = GetComponent<FollowerSpline>();
    }

    void Update()
    {
        GameObject[] infectados = GameObject.FindGameObjectsWithTag("Infectado");

        TryDestroyNearbyInfectado();

        if (infectados.Length == 0)
        {
            noInfectadoTimer += Time.deltaTime;

            if (noInfectadoTimer >= fallbackDelay)
            {
                StartCoroutine(GoToCargador());
            }
        }
        else
        {
            noInfectadoTimer = 0f;

            if (followerSpline != null)
                followerSpline.FollowPathTick();
        }
    }

    void TryDestroyNearbyInfectado()
    {
        GameObject[] infectados = GameObject.FindGameObjectsWithTag("Infectado");

        foreach (var inf in infectados)
        {
            float dist = Vector3.Distance(transform.position, inf.transform.position);

            if (dist <= destroyDistance)
            {
                Destroy(inf);
                return;
            }
        }
    }

    IEnumerator GoToCargador()
    {
        if (fallbackTimerRunning) yield break;
        fallbackTimerRunning = true;

        if (followerSpline != null && followerSpline.leader != null)
            followerSpline.leader.pathPoints.Clear();

        GameObject cargador = GameObject.FindGameObjectWithTag(fallbackTag);
        if (cargador != null)
        {
            while (Vector3.Distance(transform.position, cargador.transform.position) > 0.2f)
            {
                Vector3 dir = (cargador.transform.position - transform.position).normalized;
                transform.position += dir * followerSpline.followSpeed * Time.deltaTime;

                yield return null;
            }
        }

        fallbackTimerRunning = false;
    }
}