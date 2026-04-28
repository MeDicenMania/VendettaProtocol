using UnityEngine;

public class WinZone : MonoBehaviour
{
    public GameObject pantallaWin;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            pantallaWin.SetActive(true);
        }
    }
}