using UnityEngine;

public class SierraMovimiento : MonoBehaviour
{
    public float speed;
    public int startingpoint;
    public Transform[] points;

    private int i;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[startingpoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            { 
                i = 0; 
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<vida_jugador>())
        {
            collision.collider.GetComponent<vida_jugador>().tomardaño(4);
        }
    }
}
