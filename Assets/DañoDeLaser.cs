using UnityEngine;

public class laser_damage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        vida_jugador vida = collision.collider.GetComponent<vida_jugador>();

        if (vida != null)
        {
            vida.tomardaño(4);
            Destroy(gameObject);
        }
    }
}
