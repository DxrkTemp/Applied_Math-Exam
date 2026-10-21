using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float boostMultiplier = 2f;
    public float rotateSpeed = 90f;
    public float scaleStep = 0.5f;
    public float minScale = 0.5f, maxScale = 3f;
    public float gravity = -20f;

    public bool Boosted { get; private set; }
    public float CurrentSpeed => baseSpeed * (Boosted ? boostMultiplier : 1f);

    CharacterController cc;
    float yVel;
    bool scalingUp = true;

    void Awake() => cc = GetComponent<CharacterController>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) Boosted = !Boosted;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 move = transform.TransformDirection(input.normalized) * CurrentSpeed;

        if (cc.isGrounded) yVel = -1f; else yVel += gravity * Time.deltaTime;
        move.y = yVel;
        cc.Move(move * Time.deltaTime);

        if (Input.GetKey(KeyCode.X))
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        if (Input.GetKeyDown(KeyCode.Y)) scalingUp = !scalingUp;
        if (Input.GetKey(KeyCode.Y))
        {
            float s = transform.localScale.x + (scalingUp ? 1 : -1) * scaleStep * Time.deltaTime;
            s = Mathf.Clamp(s, minScale, maxScale);
            transform.localScale = Vector3.one * s;
        }
    }
}