using Unity.VisualScripting;
using UnityEngine;

public class Suction : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public movimiento mov;
    public colisiones coll;


    [Header("Partes")]
    public Camera m_camera;
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;
    [SerializeField] private GameObject balaPrefab;
    public ParticleSystem ps;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D rb;
    private Vector2 force;


    [Header("Variables")]
    [SerializeField] private float gunForce;
    [SerializeField] Vector2 mousePos;
 
    private int bullets;
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private int pellets = 3; 
    [SerializeField] private float spreadAngle = 15f; 

    [Header("Timers")]
    private float shootCd;


    [Header("Bools")]
    private bool soytgay = true;
    public bool isSucc;

    void Start()
    {
        bullets = 3;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        shootCd -= Time.deltaTime;

        RotateGun(mousePos);
       
        

        if (coll.enPiso)
        {
            bullets = 3;
        }

        if (canShoot() && Input.GetMouseButtonDown(1))
        {
            Repell();
        }
    }



    public void Repell()
    {
        shootCd = shootCooldown;
        bullets--;

        Vector2 baseDir = (mousePos - (Vector2)firePoint.position).normalized;


        if (!coll.enPiso && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.2f);
        }


        float currentX = rb.linearVelocity.x;


        rb.linearVelocity = new Vector2(currentX, rb.linearVelocity.y);


        float airMultiplier = coll.enPiso ? 1f : 1.5f;

        rb.AddForce(-baseDir * gunForce * airMultiplier, ForceMode2D.Impulse);


        float step = (spreadAngle * 2) / (pellets - 1);
        float startAngle = -spreadAngle;

        for (int i = 0; i < pellets; i++)
        {
            float angleOffset = startAngle + (step * i);

            Vector2 direccion = Quaternion.Euler(0, 0, angleOffset) * baseDir;

            GameObject bala = Instantiate(balaPrefab, firePoint.position, Quaternion.identity);

            bala.GetComponent<Bala>().SetDirection(direccion);

            Rigidbody2D balaRb = bala.GetComponent<Rigidbody2D>();
            balaRb.AddForce(direccion * bulletForce, ForceMode2D.Impulse);

            float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

            bala.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }


    public void Succ()
    {
       
        if (ps == null)
        {
            Debug.LogError("NO hay ParticleSystem asignado");
        }

        if (Input.GetMouseButton(1)) // click derecho
        {
            //Debug.Log("gay");
            //if (!ps.isPlaying)
            //    ps.Play();
        }
        else
        {
            //if (ps.isPlaying)
            //    ps.Stop();
        }

    
    }


    private void RotateGun(Vector3 lookPoint)
    {

        Vector3 distanceVector = lookPoint - gunPivot.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public bool canShoot()
    {
        return shootCd <= 0 && bullets > 0;
    }

}
