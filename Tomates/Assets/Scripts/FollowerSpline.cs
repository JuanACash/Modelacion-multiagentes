using UnityEngine;
using System.Collections.Generic;

public class FollowerSpline : MonoBehaviour
{
    public LeaderSpline leader; 
    public float followSpeed = 3f;
    public int currentIndex = 0;

    public List<Vector3> Path => leader != null ? leader.pathPoints : null;

    // NEW - public method that RobotDestroyerSimple can call
    public void FollowPathTick()
    {
        if (leader == null || leader.pathPoints.Count == 0) return;
        if (currentIndex >= leader.pathPoints.Count) return;

        Vector3 targetPos = leader.pathPoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            currentIndex++;
    }

    public void ResetSpline()
    {
        currentIndex = 0;
    }
}
