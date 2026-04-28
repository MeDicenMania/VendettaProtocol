using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public float attackDuration = 0.15f;
    public float attackCooldown = 0.5f;

    private bool canAttack = true;

    void Start()
    {
        gameObject.SetActive(false); // apaga el objeto completo al inicio
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && canAttack)
        {
            Debug.Log("Ataque melee ejecutado");
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        canAttack = false;
        gameObject.SetActive(true); // activa el objeto
        Debug.Log("Collider activado");
        yield return new WaitForSeconds(attackDuration);
        gameObject.SetActive(false); // desactiva el objeto
        Debug.Log("Collider desactivado");
        yield return new WaitForSeconds(attackCooldown - attackDuration);
        canAttack = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Toque a: " + other.name + " con tag: " + other.tag);
        if (other.CompareTag("Enemigo"))
        {
            Debug.Log("Enemigo destruido: " + other.name);
            Destroy(other.gameObject);
        }
    }
}