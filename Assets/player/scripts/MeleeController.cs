using UnityEngine;
using System.Collections;

public class MeleeController : MonoBehaviour
{
    public GameObject meleeHitbox;
    public float attackDuration = 0.15f;
    public float attackCooldown = 0.5f;

    private bool canAttack = true;

    void Start()
    {
        meleeHitbox.SetActive(false);
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
        meleeHitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        meleeHitbox.SetActive(false);
        yield return new WaitForSeconds(attackCooldown - attackDuration);
        canAttack = true;
    }
}