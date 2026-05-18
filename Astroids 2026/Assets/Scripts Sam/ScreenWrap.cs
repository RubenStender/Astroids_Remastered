using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);

        if (viewportPos.x > 1)
        {
            viewportPos.x = 0;
        }
        else if (viewportPos.x < 0)
        {
            viewportPos.x = 1;
        }

        if (viewportPos.y > 1)
        {
            viewportPos.y = 0;
        }
        else if (viewportPos.y < 0)
        {
            viewportPos.y = 1;
        }

        transform.position = cam.ViewportToWorldPoint(viewportPos);
    }
}