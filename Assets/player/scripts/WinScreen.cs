using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main menu");
    }
}