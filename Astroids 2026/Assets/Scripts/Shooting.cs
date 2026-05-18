using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f;        // seconds between shots
    public float bulletSpeed = 12f;
    private Rigidbody2D rb;

    private float nextFireTime;
    void HandleShooting()
    {
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bRb = bullet.GetComponent<Rigidbody2D>();
        if (bRb != null)
        {
            // Inherit ship velocity so bullets don't feel floaty
            bRb.linearVelocity = rb.linearVelocity + (Vector2)(transform.up * bulletSpeed);
        }
    }
}
