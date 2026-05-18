using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public GameObject asteroidPrefab;
    public int asteroidAmount = 5;
    public float spawnRadius = 8f;
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    void Start()
    {
        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        for (int i = 0; i < asteroidAmount; i++)
        {
            Vector2 spawnPos = Random.insideUnitCircle * spawnRadius;

            GameObject asteroid = Instantiate(
                asteroidPrefab,
                spawnPos,
                Quaternion.Euler(0, 0, Random.Range(0f, 360f))
            );

            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = Random.insideUnitCircle.normalized;
                float speed = Random.Range(minSpeed, maxSpeed);

                rb.linearVelocity = direction * speed;
                rb.angularVelocity = Random.Range(-50f, 50f);
            }
        }
    }
}