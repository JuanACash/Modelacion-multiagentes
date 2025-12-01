using UnityEngine;
using System.Collections;

public class RobotDestroyerSimple : MonoBehaviour
{
    [Header("Destruction Settings")]
    public float destroyDistance = 3.0f; // Increased range (change as needed)

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

        // 1️⃣ Destroy nearby infectados
        TryDestroyNearbyInfectado();

        // 2️⃣ Check if there are any infectados left
        if (infectados.Length == 0)
        {
            // Increase the timer
            noInfectadoTimer += Time.deltaTime;

            // Pause spline movement while waiting
            // (robot stands still but can rotate toward direction)
            // DO NOT MOVE along the spline
            // Just do nothing here

            // 3️⃣ If more than X seconds without infectados → break spline
            if (noInfectadoTimer >= fallbackDelay)
            {
                StartCoroutine(GoToCargador());
            }
        }
        else
        {
            // Reset timer if there ARE infectados
            noInfectadoTimer = 0f;

            // Continue following the spline normally
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
        // Prevent repeated triggering
        if (fallbackTimerRunning) yield break;
        fallbackTimerRunning = true;

        // Clear spline (stop following)
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