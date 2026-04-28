using UnityEngine;

public class Bala : MonoBehaviour
{
    private Vector2 direccion;
    public float tiempoVida = 3f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    public void SetDirection(Vector2 dir)
    {
        direccion = dir;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}