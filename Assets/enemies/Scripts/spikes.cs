using UnityEngine;

public class spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<vida_jugador>())
        {
            collision.collider.GetComponent<vida_jugador>().tomardaño(4);
        }
    }
}
    
