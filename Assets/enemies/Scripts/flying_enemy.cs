using UnityEngine;

public class flying_enemy : MonoBehaviour
{
    public float speed;
    public bool chase = false;
    public Transform startingpoint;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
            return;

        if (chase)
            Chase();
        else
            ReturnStartPoint();
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position,player.transform.position,speed * Time.deltaTime);
    }

    private void ReturnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position,startingpoint.position,speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<vida_jugador>())
        {
            collision.collider.GetComponent<vida_jugador>().tomardaño(4);
            
        }
    }
}