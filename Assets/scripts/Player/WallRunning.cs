
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] float wallRunForce;
    [SerializeField] float wallJumpUpForce;
    [SerializeField] float wallJumpSideForce;
    private Vector3 wallNormal;
    private Vector3 wallForward;
    

    [Header("Input")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;


    [Header("Exiting")]
    private bool exitingWall;


    [Header("References")]
    [SerializeField] Transform orientation;
    private PlayerMovement pm;
    private Rigidbody rb;
    [SerializeField] PlayerCam cam;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
       
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }


    private void StateMachine()
    {
   
        // State 1 - Wallrunning
        if ((pm.wallLeft || pm.wallRight) && pm.verticalInput > 0 && !pm.grounded && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();

            // wall jump
            if (Input.GetKeyDown(jumpKey)) WallJump();
        }

        // State 2 - Exiting
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();
                exitingWall = false;
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        rb.useGravity = false;
        
        wallNormal = pm.wallRight ? pm.rightWallhit.normal : pm.leftWallhit.normal;
        wallForward = Vector3.Cross(wallNormal, transform.up).normalized;
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
       
        }
        cam.DoFov(90f, 0.25f);
        if (pm.wallLeft) cam.DoTilt(-8f, 0.25f);
        if (pm.wallRight) cam.DoTilt(8f, 0.25f);
        // cam.wallRunCamSetLimit(orientation.eulerAngles.y , pm.wallRight);
     
    }

    private void WallRunningMovement()
    {


        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // forward force
        rb.AddForce(wallForward * pm.wallrunSpeed * 10f, ForceMode.Force);

        // push to wall force
        rb.AddForce(-wallNormal * 100, ForceMode.Force);

    }

    private void StopWallRun()
    {
        pm.wallrunning = false;
        cam.DoFov(80f, 0.25f);
        cam.DoTilt(0f, 0.25f);

    }

    private void WallJump()
    {
        // enter exiting wall state
        exitingWall = true;

        Vector3 wallNormal = pm.wallRight ? pm.rightWallhit.normal : pm.leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
