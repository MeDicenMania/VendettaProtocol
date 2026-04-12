using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo_Enemigo : MonoBehaviour
{
    public Transform controladordisparo;
    public float distancialinea;
    public LayerMask capajugador;
    public bool jugadorenrango;
    public GameObject balaenemigo;
    public float tiempoentredisparos;
    public float tiempoultimodisparo;
    public float tiempoesperadisparo;
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(controladordisparo.position, transform.right, distancialinea, capajugador);

        jugadorenrango = hit.collider != null;

        if (jugadorenrango) {
            if (Time.time > tiempoentredisparos + tiempoultimodisparo)
            {
                tiempoultimodisparo = Time.time;
                Invoke(nameof(Disparar), tiempoesperadisparo);
            }
        }
    }
    private void Disparar()
    {
        Instantiate(balaenemigo, controladordisparo.position, controladordisparo.rotation);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladordisparo.position, controladordisparo.position + transform.right * distancialinea);

    }
}
