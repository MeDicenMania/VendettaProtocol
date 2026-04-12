using UnityEngine;

public class vida_jugador : MonoBehaviour
{
    public int cantidaddevida;


    public void tomardaño(int daño)
    {
        cantidaddevida -= daño;
        if (cantidaddevida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
