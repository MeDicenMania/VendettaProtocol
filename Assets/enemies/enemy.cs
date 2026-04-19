using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject balaPrefab;
    public Transform firePoint;

    public float velocidadBala = 10f;
    public float tiempoEntreDisparos = 1f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tiempoEntreDisparos)
        {
            Disparar();
            timer = 0f;
        }
    }

    void Disparar()
    {
        GameObject bala = Instantiate(balaPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();
        // CLAVE: usa la dirección de la mira
        rb.linearVelocity = -firePoint.right * velocidadBala;
    }
}