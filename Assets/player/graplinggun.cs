using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;
    public movimiento mov;
    public colisiones coll;
    [Header("Layers Settings:")]
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D rb;

    private Vector2 force;


    [Header("Distance:")]

    [SerializeField] private bool hasMaxDistance = false;
     public float maxDistnace;
    private Vector2 targetPos;
    [SerializeField] private float dashSpeed;
    [Header("Grapplepoint")]
    [SerializeField] private float dashTimer;
    [SerializeField] private float dashTime = 5f;
    [SerializeField] private float dash;
    [SerializeField] private float speedBuildUp;
    public  bool isDashing = false;
    private Vector2 dashTarget;
    private GameObject[] allGrapplePoints;
    [SerializeField] private float gunForce;
    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

  
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    [SerializeField] Vector2 mousePos;
    private float shootCd;
    private int bullets;

    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private int pellets = 3; // cantidad tipo escopeta
    [SerializeField] private float spreadAngle = 15f; // apertura del cono
    [SerializeField] private float bulletForce = 10f;
    [SerializeField] private float shootCooldown = 0.5f;



    private void Start()
    {
        bullets = 3;
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        allGrapplePoints = GameObject.FindGameObjectsWithTag("grapplepoint");
    }

    private void Update()
    {
       shootCd -= Time.deltaTime;

        if(coll.enPiso)
        {
            bullets = 3;
        }

        mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        RotateGun(mousePos);
        if (Input.GetKey(KeyCode.Mouse1))
        {
            SetGrapplePoint();
        }

   

        if (Vector2.Distance(rb.position, dashTarget) < launchSpeed || dashTimer <= 0)
        {
            StopDash();
            Debug.Log("Dash completado al punto del gancho");
        }

        if (canShoot() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            Gun();
        }
    }

   

    void SetGrapplePoint()
    {
        Transform nearestPoint = GetNearestGrapplePoint(firePoint.position);

        if (nearestPoint != null)
        {
            grapplePoint = nearestPoint.position;
            grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
            grappleRope.enabled = true;
           
        }
        else
        {
            Debug.Log("No hay ningún punto de agarre dentro del rango.");
        }
    }


    private void RotateGun(Vector3 lookPoint)
    {

        Vector3 distanceVector = lookPoint - gunPivot.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Gun()
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
    public void Grapple()
    {
        
        m_springJoint2D.autoConfigureDistance = false;
        if (launchToPoint)
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    {
                        m_springJoint2D.enabled = true;
                        m_springJoint2D.connectedAnchor = grapplePoint;
                        float dist = Vector2.Distance(transform.position, grapplePoint);
                        m_springJoint2D.distance = dist * 0.7f;
                        m_springJoint2D.frequency = launchSpeed;
                        m_springJoint2D.dampingRatio = 0.3f;

                        break;
                    }
                case LaunchType.Transform_Launch:
                    {
                        rb.gravityScale = 1;
                        rb.linearVelocity = Vector2.zero;
                        dashTimer = dashTime;
                        isDashing = true;
                        dashTarget = grapplePoint;
                        break;
                    }
            }

        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;


            Vector2 dir = (dashTarget - rb.position).normalized;
            float forceMagnitude = Mathf.MoveTowards(dashSpeed, dash, Time.deltaTime * speedBuildUp);
            rb.AddForce(dir * forceMagnitude, ForceMode2D.Force);
        }
    }
    private void StopDash()
    {

        dashTimer = 1;
        isDashing = false;
        rb.gravityScale = 3;
        grappleRope.enabled = false;
    }
    private Transform GetNearestGrapplePoint(Vector2 fromPosition)
    {
        Transform nearest = null;
        float minDist = maxDistnace;

        foreach (GameObject gp in GameObject.FindGameObjectsWithTag("grapplepoint"))
        {
            float dist = Vector2.Distance(fromPosition, gp.transform.position);
            if (dist <= minDist)
            {
                minDist = dist;
                nearest = gp.transform;
            }
        }

        return nearest;
    }
    public bool canShoot()
    {
        return shootCd <= 0 && bullets > 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

}
