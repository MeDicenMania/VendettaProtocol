using UnityEngine;

public class PistolaSimple : MonoBehaviour
{
    [Header("Referencias")]
    public Camera camara;
    public Transform pivotArma;
    public Transform puntoDisparo;
    public GameObject prefabBala;
    public ParticleSystem particulas;

    [Header("Disparo")]
    [SerializeField] private float fuerzaBala = 10f;
    [SerializeField] private float cooldownDisparo = 0.2f;

    private Vector2 posicionMouse;
    private float tiempoDisparo;

    void Update()
    {
        posicionMouse = camara.ScreenToWorldPoint(Input.mousePosition);
        tiempoDisparo -= Time.deltaTime;

        RotarArma(posicionMouse);
        ControlParticulas();

        if (PuedeDisparar() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Disparar();
        }
    }

    public void Disparar()
    {
        tiempoDisparo = cooldownDisparo;

        Vector2 direccion = (posicionMouse - (Vector2)puntoDisparo.position).normalized;

        GameObject bala = Instantiate(prefabBala, puntoDisparo.position, Quaternion.identity);

        Bala scriptBala = bala.GetComponent<Bala>();
        if (scriptBala != null)
        {
            scriptBala.SetDirection(direccion);
        }

        Rigidbody2D rbBala = bala.GetComponent<Rigidbody2D>();
        if (rbBala != null)
        {
            rbBala.AddForce(direccion * fuerzaBala, ForceMode2D.Impulse);
        }

        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        bala.transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
    }

    public void ControlParticulas()
    {
        if (particulas == null) return;

        if (Input.GetMouseButton(1 ))
        {
            if (!particulas.isPlaying)
                particulas.Play();
        }
        else
        {
            if (particulas.isPlaying)
                particulas.Stop();
        }
    }

    private void RotarArma(Vector3 objetivo)
    {
        Vector3 direccion = objetivo - pivotArma.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        pivotArma.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
    }

    public bool PuedeDisparar()
    {
        return tiempoDisparo <= 0;
    }
}