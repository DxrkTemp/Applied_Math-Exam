using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Player"))
            c.collider.GetComponent<PlayerRespawn>()?.Respawn();
        Destroy(gameObject);
    }
}