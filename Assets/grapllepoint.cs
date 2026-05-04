using UnityEngine;

public class grapplepoint : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Color colorCercano = Color.green;
    [SerializeField] private Color colorLejano = Color.gray;
    [SerializeField] private Color colorActivo = Color.yellow; // cuando estás colgado de este punto

    [Header("Debug")]
    [SerializeField] private float distancia;

    // Referencias
    private Tutorial_GrapplingGun grapple;
    private Transform player;
    private SpriteRenderer render;

    public bool canGrapple { get; private set; }
    public bool isActive { get; private set; } // true = el jugador está colgado de este punto

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        render = GetComponent<SpriteRenderer>();

        // Buscar el GrapplingGun en el player automáticamente
        grapple = player.GetComponentInChildren<Tutorial_GrapplingGun>();

        if (grapple == null)
            Debug.LogWarning($"[grapplepoint] No se encontró Tutorial_GrapplingGun en el Player.", this);
    }

    void Update()
    {
        distancia = Vector2.Distance(transform.position, player.position);
        canGrapple = distancia < grapple.maxDistnace;

        // Detectar si el jugador está colgado exactamente de este punto
        isActive = grapple.isGrabing &&
                   Vector2.Distance(grapple.grapplePoint, transform.position) < 0.1f;

        ActualizarColor();
    }

    private void ActualizarColor()
    {
        if (isActive)
            render.color = colorActivo;
        else if (canGrapple)
            render.color = colorCercano;
        else
            render.color = colorLejano;
    }

    private void OnDrawGizmosSelected()
    {
        if (grapple == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}