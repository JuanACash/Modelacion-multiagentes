using UnityEngine;
using System.Collections.Generic;

public class LeaderSpline : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float minDistance = 0.1f;
    [HideInInspector] public List<Vector3> pathPoints = new List<Vector3>();

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 0;
        lineRenderer.widthMultiplier = 0.1f;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;

        if (pathPoints.Count == 0 || Vector3.Distance(pathPoints[pathPoints.Count - 1], currentPos) > minDistance)
        {
            pathPoints.Add(currentPos);
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(pathPoints.ToArray());
        }
    }
}
