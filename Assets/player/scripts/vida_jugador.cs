using UnityEngine;
using UnityEngine.SceneManagement;

public class vida_jugador : MonoBehaviour
{
    public int cantidaddevida = 3;
    public int vidasMaximas = 3;
    public GameObject pantallaGameOver;

    private Vector3 posicionInicial;
    private int vidasActuales;

    void Start()
    {
        posicionInicial = transform.position;
        vidasActuales = vidasMaximas;
        pantallaGameOver.SetActive(false);
    }

    public void tomardaño(int daño)
    {
        cantidaddevida -= daño;
        if (cantidaddevida <= 0)
            Morir();
    }

    void Morir()
    {
        vidasActuales--;
        Debug.Log("Vidas restantes: " + vidasActuales);

        if (vidasActuales <= 0)
        {
            Time.timeScale = 0f;
            pantallaGameOver.SetActive(true);
        }
        else
        {
            cantidaddevida = vidasMaximas;
            transform.position = posicionInicial;
        }
    }
}