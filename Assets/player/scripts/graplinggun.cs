using UnityEngine;

public class Tutorial_GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;
    public movimiento mov;
    public colisiones coll;

    [Header("Camera")]
    public Camera m_camera;

    [Header("Transforms")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D rb;

    [Header("Distance")]
    public float maxDistnace = 10f;
    public float minDistance = 0.5f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.5f;
    private float dashTimer;
    public bool isDashing = false;
    private Vector2 dashTarget;

    [Header("Obstacle")]
    [SerializeField] private LayerMask obstacleMask;
    private bool hayObstaculo;

    [Header("Grapple")]
    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    public bool isGrabing;
    public Vector2 forceJump;
    public float forceGrab;
    [SerializeField] private int exitGrabSide;
    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {
       
        ActualizarRaycastObstaculo();
        if (Input.GetKeyDown(KeyCode.E) && !hayObstaculo)
        {
            SetGrapplePoint();
        }

   
        if (isDashing)
        {
            float dist = Vector2.Distance(rb.position, dashTarget);

            if (dist <= minDistance)
            {
                StopDash();
            }
        }

        if ((Input.GetKeyDown(KeyCode.D)))
        {
            exitGrabSide = 1;
        }
        if ((Input.GetKeyDown(KeyCode.A)))
        {
            exitGrabSide = -1;
        }


        if (isGrabing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector2 jump = new Vector2(forceJump.x, forceJump.y);

                // aplicar dirección UNA sola vez
                jump.x *= exitGrabSide;

                // corregir momentum
                if (Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(jump.x))
                {
                    jump.x -= rb.linearVelocity.x;
                }
                if (rb.linearVelocity.y < 0)
                {
                    jump.y -= rb.linearVelocity.y;
                }

                // aplicar fuerza
                rb.AddForce(jump * forceGrab, ForceMode2D.Impulse);

                ExitGrab();
            }
        }
     
        if (hayObstaculo)
        {
            CancelGrapple();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;

            Vector2 dir = (dashTarget - rb.position);
            float dist = dir.magnitude;

            if (dist <= minDistance)
            {
                StopDash();
                isGrabing = true;
                return;
            }

            dir.Normalize();

        
            rb.MovePosition(rb.position + dir * dashSpeed * Time.fixedDeltaTime);

            if (dashTimer <= 0f)
            {
                StopDash();
            }
        }
    }

   

    public  void SetGrapplePoint()
    {
        Transform nearestPoint = GetNearestGrapplePoint(firePoint.position);

        if (nearestPoint != null)
        {
            grapplePoint = nearestPoint.position;
            grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;

            grappleRope.enabled = true;

            StartDash();
        }
        else
        {
            Debug.Log("No hay punto de agarre");
        }
    }
    private void ExitGrab()
    {
        isGrabing = false;

        rb.gravityScale = 3;
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashTime;

        dashTarget = grapplePoint;

        rb.gravityScale = 0f; 
    }

    void StopDash()
    {
        isDashing = false;
        grappleRope.enabled = false;

        isGrabing = true;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }

    void CancelGrapple()
    {
        isDashing = false;
        grappleRope.enabled = false;
        rb.gravityScale = 3f;
    }

    

    private void ActualizarRaycastObstaculo()
    {
        if (grapplePoint != Vector2.zero)
        {
            Vector2 dir = grapplePoint - (Vector2)rb.position;
            float dist = dir.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(rb.position, dir.normalized, dist, obstacleMask);
            hayObstaculo = hit.collider != null;
            Debug.DrawLine(rb.position, grapplePoint, hayObstaculo ? Color.red : Color.green);
        }
        else
        {
            hayObstaculo = false;
        }
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

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }
}