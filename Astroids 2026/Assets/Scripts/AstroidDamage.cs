using UnityEngine;

public class AsteroidDamage : MonoBehaviour
{
    public enum AsteroidSize { Large, Medium, Small }

    [Header("Size")]
    public AsteroidSize size = AsteroidSize.Large;

    [Header("Splitting into")]
    public GameObject splitPrefab;

    [Range(2, 4)]
    public int splitCount = 2;

    [Header("Scale sizes")]
    public float largeScale = 1.0f;
    public float mediumScale = 0.55f;
    public float smallScale = 0.28f;

    [Header("Movement Settings")]
    [SerializeField] private float minSpeed = 2f;
    [SerializeField] private float maxSpeed = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ApplyScale();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            TakeHit();
        }
    }

    public void TakeHit()
    {
        Explode();
    }

    private void Explode()
    {
        SpawnChildren();
        Destroy(gameObject);
    }

    private void SpawnChildren()
    {
        // Small asteroids do not split
        if (size == AsteroidSize.Small || splitPrefab == null)
            return;

        // Determine the next size down
        AsteroidSize nextSize = size == AsteroidSize.Large ? AsteroidSize.Medium : AsteroidSize.Small;

        for (int i = 0; i < splitCount; i++)
        {
            // 1. Spawn at the current position with a random rotation
            GameObject child = Instantiate(
                splitPrefab,
                transform.position,
                Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))
            );

            // 2. Set the correct smaller size
            AsteroidDamage childScript = child.GetComponent<AsteroidDamage>();
            if (childScript != null)
            {
                childScript.size = nextSize;
            }

            // 3. Kick off their movement so they fly apart!
            Rigidbody2D childRb = child.GetComponent<Rigidbody2D>();
            if (childRb != null)
            {
                // Create a random direction vector
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                float randomSpeed = Random.Range(minSpeed, maxSpeed);

                // Apply velocity directly
                childRb.linearVelocity = randomDirection * randomSpeed;

                // Note: If you are using an older version of Unity (before 2023), 
                // change 'linearVelocity' to just 'velocity'
            }
        }
    }

    private void ApplyScale()
    {
        float scale = size switch
        {
            AsteroidSize.Large => largeScale,
            AsteroidSize.Medium => mediumScale,
            AsteroidSize.Small => smallScale,
            _ => 1f
        };

        transform.localScale = Vector3.one * scale;
    }
}