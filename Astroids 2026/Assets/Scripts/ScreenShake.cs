using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPosition;
    private float shakeTimer;
    private float shakeIntensity;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeIntensity;

            transform.localPosition = new Vector3(
                originalPosition.x + randomOffset.x,
                originalPosition.y + randomOffset.y,
                originalPosition.z
            );

            shakeTimer -= Time.deltaTime;

            // Return to normal when done
            if (shakeTimer <= 0)
            {
                transform.localPosition = originalPosition;
            }
        }
    }
    public void Shake(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeTimer = duration;
    }
}