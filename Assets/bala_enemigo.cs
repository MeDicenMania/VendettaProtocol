using UnityEngine;

public class bala_enemigo : MonoBehaviour
{
    public float velocidad;
    public int daño;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out vida_jugador vidajugador))
        {
            vidajugador.tomardaño(daño);
            Destroy(gameObject);
        }
    }
}
