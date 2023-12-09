
using UnityEngine;

public class sliding : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerModel;
    [SerializeField] PlayerCam cam;

    [Header("FX")]
    [SerializeField] AudioClip slidingSound;
    [SerializeField] GameObject slidingVFX;
    private GameObject slidingEffectInstance;
    AudioSource playerAudioSource;
    private Rigidbody rb;
    private PlayerMovement playerMovement;


    // Start is called before the first frame update


    [Header("Sliding")]
    [SerializeField] float slideForce;

    private float startYScale;


    [Header("Input")]
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;

    private Vector3 slideDirection;
    private Vector3 fowardDirection;

   

    void Start()
    {
     
        playerAudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();   

        startYScale = playerModel.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        SLideState();
    }

    private void SLideState()
    {

        if (Input.GetKeyDown(slideKey) && (playerMovement.horizontalInput != 0 || playerMovement.verticalInput != 0) && playerMovement.grounded)
        {
            fowardDirection = orientation.forward * playerMovement.verticalInput;
            StartSlide();
        }
        if (Input.GetKeyUp(slideKey) || !playerMovement.grounded)
            StopSlide();

    }

    private void FixedUpdate()
    {
        if (playerMovement.isSliding)
            SlidingMovment();
    }

    private void StartSlide() {
        cam.DoFov(90f, 0.5f);
        playerAudioSource.clip = slidingSound;
        playerAudioSource.Play();
        slidingEffectInstance = Instantiate(slidingVFX, transform.position, Quaternion.identity, transform);
        playerMovement.isSliding = true;
        playerModel.localScale = new Vector3(playerModel.localScale.x , 0.3f, playerModel.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    private void StopSlide()
    {
        cam.DoFov(80f, 0.5f);
        playerAudioSource.Stop();
        Destroy(slidingEffectInstance);
        playerMovement.isSliding = false;
        playerModel.localScale = new Vector3(playerModel.localScale.x, startYScale, playerModel.localScale.z);

    }

    private void SlidingMovment() 
    {
        slideDirection = fowardDirection +  orientation.right * playerMovement.horizontalInput * 0.5f;

        // sliding normal
        if (!playerMovement.OnSlope() || rb.velocity.y > -0.1f)
            {
                rb.AddForce(slideDirection.normalized * slideForce, ForceMode.Force);

            }

            // sliding down a slope
        else
        {
            rb.AddForce(playerMovement.GetSlopeMoveDirection(slideDirection) * slideForce, ForceMode.Force);
        }

    }
}
