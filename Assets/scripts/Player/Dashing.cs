using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    private Rigidbody rb;
    private PlayerMovement pm;
    [SerializeField] AudioClip dashSound;

    [Header("Dashing")]
    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;
    [SerializeField] float dashLerpTime; 
    private Vector3 forceToApply;
    private Vector3 currentVelocity; 

    [Header("Cooldown")]
    public float dashCd;
    public float dashCdTimer;
    public int remaningDashes;
    [SerializeField] int dashCount;

    [Header("Input")]
    [SerializeField] KeyCode dashKey = KeyCode.LeftShift;


    [Header("Camera")]
    [SerializeField] PlayerCam cam;

    private void Start()
    {
        remaningDashes = dashCount;
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        DashState();
    }

    private void FixedUpdate()
    {
        if (remaningDashes < dashCount)
        {
            if (dashCdTimer < dashCd)
                dashCdTimer += Time.deltaTime;

            else
            {
                dashCdTimer = 0;
                remaningDashes++;
            }
        }

        // new code for lerping
        if (pm.dashing)
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity, forceToApply, ref currentVelocity, dashLerpTime);
        }
    }

    private void DashState()
    {
        if (Input.GetKeyDown(dashKey) && remaningDashes > 0)
        {
            remaningDashes--;
            Dash();
        }
    }

    private void Dash()
    {
        AudioSource.PlayClipAtPoint(dashSound, transform.position, 1f);
        pm.dashing = true;

        Transform forwardT;
        forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);
        cam.DoFov(90f, 0.5f);
        forceToApply = direction * dashForce;

        rb.useGravity = false;

        if (!pm.grounded)
            forceToApply += transform.up * 2;

        Invoke(nameof(DelayedDashForce), 0.015f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedDashForce()
    {
       

        // remove instant velocity assignment and add current velocity instead
        currentVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
    }

    private void ResetDash()
    {
        pm.dashing = false;
        cam.DoFov(80f, 0.5f);
        rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        Vector3 direction = new Vector3();

        direction = forwardT.forward * pm.verticalInput + forwardT.right * pm.horizontalInput;

        if (pm.verticalInput == 0 && pm.horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
}
