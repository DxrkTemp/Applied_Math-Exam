using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform spawnPoint;
    public float killY = -20f;

    CharacterController cc;

    void Awake() => cc = GetComponent<CharacterController>();

    void Start() { if (spawnPoint) Respawn(); }

    void Update()
    {
        if (transform.position.y < killY) Respawn();
    }

    public void Respawn()
    {
        cc.enabled = false;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        cc.enabled = true;
    }
}