using UnityEngine;

public class PatrolHazard : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3f;
    int i = 0, dir = 1;

    void Update()
    {
        if (waypoints.Length < 2) return;

        Vector3 target = waypoints[i].position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        Vector3 look = target - transform.position;
        look.y = 0;
        if (look.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(look);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            if (i + dir < 0 || i + dir >= waypoints.Length) dir = -dir;
            i += dir;
        }
    }
}