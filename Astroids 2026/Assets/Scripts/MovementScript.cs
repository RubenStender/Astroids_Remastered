using UnityEngine;

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
        HandleRotation();
        ToggleThruster();
        HandleThrust();
        ApplyDrag();
        ClampSpeed();
    }

    private void FixedUpdate()
    {
        Vector2 newPos = rb.position + velocity * Time.fixedDeltaTime;

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

        rb.MovePosition(newPos);
    }

    private void HandleRotation()
    {
        float input = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) input = 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) input = -1f;
        transform.Rotate(0f, 0f, input * rotationSpeed * Time.deltaTime);
    }

    private void ToggleThruster()
    {
        bool thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        if (thruster != null) thruster.SetActive(thrusting);
    }

    private void HandleThrust()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            velocity += (Vector2)transform.up * thrustForce * Time.deltaTime;
    }

    private void ApplyDrag() => velocity *= (1f - drag);

    private void ClampSpeed()
    {
        if (velocity.magnitude > maxSpeed) velocity = velocity.normalized * maxSpeed;
    }

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