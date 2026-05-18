using UnityEngine;

/// <summary>
/// Controls bullet movement, lifetime and asteroid collision.
/// Attach to the Bullet prefab.
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Lifetime")]
    public float lifetime = 0.67f;

    [Header("Movement")]
    public float speed = 20f;

    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AsteroidDamage asteroid = other.GetComponent<AsteroidDamage>();
        if (asteroid != null)
        {
            asteroid.TakeHit();
            Destroy(gameObject);
        }
    }
}