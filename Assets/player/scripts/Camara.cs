using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Follow")]
    public float smoothX = 5f;
    public float smoothY = 5f;

    [Header("Offset")]
    public float offsetX = 2f;
    public float offsetY = 1f;

    [Header("Zoom")]
    public float zoomSize = 5f;
    public float zoomSmooth = 3f;

    [Header("Limites")]
    public bool useLimits = true;
    public float minX = -100f;
    public float maxX = 100f;
    public float minY = -5f;
    public float maxY = 10f;

    [Header("Death Delay")]
    public float deathSmoothDuration = 2f;
    private float deathSmoothTimer = 0f;
    private bool isDelaying = false;

    private float targetOffsetX;
    private int lastDirection = 1;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam != null && cam.orthographic)
            cam.orthographicSize = zoomSize;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Zoom
        if (cam != null && cam.orthographic)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, Time.deltaTime * zoomSmooth);

        // Direccion del jugador
        float scaleX = player.localScale.x;
        if (scaleX != 0)
            lastDirection = scaleX > 0 ? 1 : -1;

        targetOffsetX = Mathf.Lerp(targetOffsetX, offsetX * lastDirection, Time.deltaTime * smoothX);

        float targetX = player.position.x + targetOffsetX;
        float targetY = player.position.y + offsetY;

        // Death delay
        float currentSmooth = smoothX;
        if (isDelaying)
        {
            deathSmoothTimer -= Time.deltaTime;
            float t = 1f - Mathf.Clamp01(deathSmoothTimer / deathSmoothDuration);
            currentSmooth = Mathf.Lerp(0.5f, smoothX, t);
            if (deathSmoothTimer <= 0f)
                isDelaying = false;
        }

        float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * currentSmooth);
        float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * smoothY);

        // Limites
        if (useLimits)
        {
            newX = Mathf.Clamp(newX, minX, maxX);
            newY = Mathf.Clamp(newY, minY, maxY);
        }

        transform.position = new Vector3(newX, newY, transform.position.z);
        transform.rotation = Quaternion.identity;
    }

    public void OnPlayerDied()
    {
        isDelaying = true;
        deathSmoothTimer = deathSmoothDuration;
    }
}