using UnityEngine;

/// <summary>
/// Controls bullet lifetime and asteroid collision.
/// Attach to the Bullet prefab.
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Lifetime")]
    public float lifetime = 10.5f;       // auto-destroy after this many seconds

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // change asteroid component to sams component name
        AstroidDamage asteroid = other.GetComponent<AstroidDamage>();
        if (asteroid != null)
        {
            asteroid.TakeHit();
            Destroy(gameObject);
        }
    }
}
