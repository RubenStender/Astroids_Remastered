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

    private void Awake()
    {
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

        for (int i = 0; i < splitCount; i++)
        {
            Instantiate(
                splitPrefab,
                transform.position,
                Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))
            );
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