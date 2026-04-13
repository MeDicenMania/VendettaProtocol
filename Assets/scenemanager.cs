using UnityEngine;
using UnityEngine.SceneManagement;
public class scenemanager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public  void cambio(string escena)
    {
        SceneManager.LoadScene(escena);
    }

    public void salir()
    {
        Application.Quit();
    }
}