using UnityEngine;

public class Bala : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;



    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        rb.linearVelocity = dir * speed;
    }

    

    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("piso"))
        {
            Destroy(gameObject);
        }
       
    }



}