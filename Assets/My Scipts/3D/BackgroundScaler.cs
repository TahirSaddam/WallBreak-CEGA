using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    Camera cam;
    float lastAspect;

    void OnEnable()
    {
        cam = Camera.main;
        ScaleBackground();
    }

    void ScaleBackground()
    {
        if (cam == null) return;

        lastAspect = cam.aspect;

        // Distance from camera to quad
        float distance = Mathf.Abs(transform.position.z - cam.transform.position.z);

        float height = 2f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float width = height * cam.aspect;

        transform.localScale = new Vector3(width, height, 1f);

        // Optional: keep centered with camera
        transform.position = new Vector3(
            cam.transform.position.x,
            cam.transform.position.y,
            transform.position.z
        );
    }
}