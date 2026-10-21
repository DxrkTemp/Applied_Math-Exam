using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public Rigidbody projectilePrefab;
    public float force = 15f;
    public float interval = 2f;
    public float range = 40f;       
    public float turnSpeed = 5f;   

    Transform target;
    float t;

    void Start()
    {
        // Finds the object tagged "Player" automatically
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player) target = player.transform;
        else Debug.LogWarning("ProjectileSpawner: no object tagged 'Player' found.");
    }

    void Update()
    {
        if (!target) return;

        Vector3 toPlayer = (target.position + Vector3.up) - transform.position;
        if (toPlayer.magnitude > range) return;

        Quaternion look = Quaternion.LookRotation(toPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);

        t += Time.deltaTime;
        if (t >= interval) { t = 0; Fire(); }
    }

    void Fire()
    {
        Rigidbody p = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
        p.linearVelocity = transform.forward * force;
        Destroy(p.gameObject, 6f);
    }
}