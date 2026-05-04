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

    [Header("Swing")]
    [SerializeField] private float swingForce = 4.5f;
    [SerializeField] private float swingForceScale = 0.5f;
    [SerializeField] private float maxSwingForce = 12f;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float ropeShortSpeed = 2f;
    [SerializeField] private float ropeLenghSpeed = 2f;
    [SerializeField] private float minRopeLength = 1f;
    [SerializeField] private float maxExitSpeed = 12f;
    private float currentRopeLength;

    [Header("Ground Pull")]
    [SerializeField] private float groundPullMaxDistance = 8f;
    [SerializeField] private float groundPullForce = 15f;
    [SerializeField] private float groundPullStopDistance = 1.5f;
    private bool isPulling = false;

    [Header("Obstacle")]
    [SerializeField] private LayerMask obstacleMask;
    private bool hayObstaculo;

    [Header("Grapple")]
    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    public bool isGrabing;

    // guardamos si estuvo en piso en los últimos frames
    private bool wasOnGroundRecently = false;
    private float groundCoyoteTime = 0.15f; // margen de tiempo
    private float groundTimer = 0f;

    // ─── Lifecycle ────────────────────────────────────────────────────────────

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {
        // Coyote time para enPiso
        if (coll.enPiso)
        {
            groundTimer = groundCoyoteTime;
            wasOnGroundRecently = true;
        }
        else
        {
            groundTimer -= Time.deltaTime;
            if (groundTimer <= 0f)
                wasOnGroundRecently = false;
        }

        ActualizarRaycastObstaculo();

        if (Input.GetKeyDown(KeyCode.E) && !hayObstaculo)
            SetGrapplePoint();

        if (Input.GetKeyUp(KeyCode.E) && isGrabing)
            ReleaseGrapple();

        if (isGrabing)
        {
            if (isPulling)
            {
                float dist = Vector2.Distance(rb.position, grapplePoint);

                if (dist <= groundPullStopDistance)
                {
                    isPulling = false;
                    rb.gravityScale = 3f;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                }
                else
                {
                    Vector2 dir = (grapplePoint - rb.position).normalized;
                    rb.AddForce(dir * groundPullForce, ForceMode2D.Force);
                }
            }
            else
            {
                HandleSwingInput();
            }

            if (Input.GetKeyDown(KeyCode.Space))
                LaunchFromSwing();
        }

        if (hayObstaculo && isGrabing)
            CancelGrapple();
    }

    // ─── Swing input ──────────────────────────────────────────────────────────

    private void HandleSwingInput()
    {
        float speed = rb.linearVelocity.magnitude;
        float forceMagnitude = Mathf.Clamp(swingForce + speed * swingForceScale, swingForce, maxSwingForce);

        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector2.right * forceMagnitude, ForceMode2D.Force);

        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector2.left * forceMagnitude, ForceMode2D.Force);

        if (Input.GetKey(KeyCode.W))
        {
            currentRopeLength = Mathf.Max(minRopeLength,
                currentRopeLength - ropeShortSpeed * Time.deltaTime);
            m_springJoint2D.distance = currentRopeLength;
        }

        if (Input.GetKey(KeyCode.S))
        {
            currentRopeLength += ropeLenghSpeed * Time.deltaTime;
            m_springJoint2D.distance = currentRopeLength;
        }
    }

    // ─── Launch (Space mientras cuelga) ───────────────────────────────────────

    private void LaunchFromSwing()
    {
        Vector2 launchDir = rb.linearVelocity.normalized;

        if (launchDir.sqrMagnitude < 0.1f)
            launchDir = Vector2.up;

        launchDir = (launchDir + Vector2.up * 0.5f).normalized;

        rb.AddForce(launchDir * launchForce, ForceMode2D.Impulse);

        ExitGrab();
    }

    // ─── Grapple point ────────────────────────────────────────────────────────

    public void SetGrapplePoint()
    {
        Transform nearestPoint = GetNearestGrapplePoint(firePoint.position);

        if (nearestPoint != null)
        {
            grapplePoint = nearestPoint.position;
            grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
            AttachSwing();
        }
        else
        {
            Debug.Log("No hay punto de agarre");
        }
    }

    // ─── Swing attach / detach ────────────────────────────────────────────────

    void AttachSwing()
    {
        grappleRope.enabled = true;

        currentRopeLength = Vector2.Distance(rb.position, grapplePoint);

        m_springJoint2D.enabled = true;
        m_springJoint2D.connectedAnchor = grapplePoint;
        m_springJoint2D.distance = currentRopeLength;
        m_springJoint2D.frequency = 0f;
        m_springJoint2D.dampingRatio = 0f;
        m_springJoint2D.enableCollision = true;

        rb.gravityScale = 3f;
        isGrabing = true;

        float distToPoint = Vector2.Distance(rb.position, grapplePoint);

        // Usar wasOnGroundRecently en vez de coll.enPiso directo
        if (wasOnGroundRecently && distToPoint <= groundPullMaxDistance)
        {
            isPulling = true;
            rb.gravityScale = 0f;
        }
        else
        {
            isPulling = false;
        }
    }

    void ExitGrab()
    {
        isGrabing = false;
        isPulling = false;
        m_springJoint2D.enabled = false;
        grappleRope.enabled = false;
        rb.gravityScale = 3f;
        grapplePoint = Vector2.zero;

        if (rb.linearVelocity.magnitude > maxExitSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxExitSpeed;
    }

    void ReleaseGrapple()
    {
        ExitGrab();
    }

    void CancelGrapple()
    {
        isGrabing = false;
        isPulling = false;
        isDashing_unused();
        m_springJoint2D.enabled = false;
        grappleRope.enabled = false;
        rb.gravityScale = 3f;
        grapplePoint = Vector2.zero;

        if (rb.linearVelocity.magnitude > maxExitSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxExitSpeed;
    }

    private void isDashing_unused() { }

    // ─── Obstacle check ───────────────────────────────────────────────────────

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

    // ─── Nearest grapple point ────────────────────────────────────────────────

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

    // ─── Gizmos ───────────────────────────────────────────────────────────────

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }
}