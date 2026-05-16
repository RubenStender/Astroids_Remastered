using UnityEngine;
using UnityEngine.InputSystem;

public class AsteroidsMovement : MonoBehaviour
{
    [Header("Thrust")]
    public float thrustForce = 5f;
    public float maxSpeed = 8f;

    [Range(0f, 1f)]
    public float drag = 0.02f;

    [Header("Rotatie")]
    public float rotationSpeed = 180f;

    [Header("Screen Wrap")]
    public float wrapPadding = 0.74f;

    [Header("Referenties")]
    public GameObject thruster;

    private Vector2 velocity = Vector2.zero;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Keyboard kb => Keyboard.current;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (kb == null) return;
        HandleRotation();
        ToggleThruster();
        HandleThrust();
        ApplyDrag();
        ClampSpeed();
    }

    private void FixedUpdate()
    {
        // 1. Bereken nieuwe positie
        Vector2 newPos = rb.position + velocity * Time.fixedDeltaTime;

        // 2. Wrap de nieuwe positie
        float camH = mainCamera.orthographicSize;
        float camW = camH * mainCamera.aspect;
        float minX = mainCamera.transform.position.x - camW - wrapPadding;
        float maxX = mainCamera.transform.position.x + camW + wrapPadding;
        float minY = mainCamera.transform.position.y - camH - wrapPadding;
        float maxY = mainCamera.transform.position.y + camH + wrapPadding;

        if (newPos.x > maxX) newPos.x = minX;
        else if (newPos.x < minX) newPos.x = maxX;
        if (newPos.y > maxY) newPos.y = minY;
        else if (newPos.y < minY) newPos.y = maxY;

        // 3. MovePosition naar gewrapte positie
        rb.MovePosition(newPos);
    }

    private void HandleRotation()
    {
        float input = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input = 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input = -1f;
        transform.Rotate(0f, 0f, input * rotationSpeed * Time.deltaTime);
    }

    private void ToggleThruster()
    {
        bool thrusting = kb.wKey.isPressed || kb.upArrowKey.isPressed;
        if (thruster != null) thruster.SetActive(thrusting);
    }

    private void HandleThrust()
    {
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)
            velocity += (Vector2)transform.up * thrustForce * Time.deltaTime;
    }

    private void ApplyDrag() => velocity *= (1f - drag);
    private void ClampSpeed() { if (velocity.magnitude > maxSpeed) velocity = velocity.normalized * maxSpeed; }

    private void OnDrawGizmosSelected()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;
        float camH = mainCamera.orthographicSize + wrapPadding;
        float camW = camH * mainCamera.aspect;
        Vector3 c = mainCamera.transform.position; c.z = 0f;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(c, new Vector3(camW * 2f, (mainCamera.orthographicSize + wrapPadding) * 2f, 0f));
    }
}