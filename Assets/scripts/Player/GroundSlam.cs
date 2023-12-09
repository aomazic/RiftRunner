using System.Collections;
using UnityEngine;

public class GroundSlam : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;
    private PlayerMovement pm;

    [SerializeField] LayerMask whatIsEnemies;

    [Header("Slaming")]
    [SerializeField] float slamForce;
    private float currentSlamTime;
    private bool slamProcessed = true;

    [Header("Cooldown")]
    [SerializeField] float slamCd;
    private float slamCdTimer;

    [Header("Input")]
    [SerializeField] KeyCode slamKey = KeyCode.LeftControl;

    [Header("Explosion")]
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;
    [SerializeField] int explosionDamage;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] AudioClip slamAudioClip;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        SlamState();
    }

    private void SlamState()
    {
        if (Input.GetKeyDown(slamKey) && !pm.grounded)
        {
            Slam();
        }

        if (Input.GetKeyUp(slamKey) || pm.grounded)
            pm.slaming = false;

        if (slamCdTimer > 0)
            slamCdTimer -= Time.deltaTime;
    }

    private void Slam()
    {
        if (slamCdTimer > 0) return;
        else slamCdTimer = slamCd;

        pm.slaming = true;
        currentSlamTime = 1f;

        
    }

    private void FixedUpdate()
    {
        if (pm.slaming)
        {
            currentSlamTime += Time.fixedDeltaTime;

            Vector3 direction = Vector3.down;
            Vector3 forceToApply = direction * slamForce * currentSlamTime / 2;

            rb.velocity = Vector3.zero;
            rb.AddForce(forceToApply * 15 * 10f, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (pm.slaming && slamProcessed)
        {
            Explode();
            StartCoroutine(SlamTimer());
        }
    }
    private IEnumerator SlamTimer()
    {
        slamProcessed = false;
        yield return new WaitForSeconds(1f); 
        EndSlam();
    }

    public void EndSlam()
    {
        pm.slaming = false;
        slamProcessed = true;  
    }

    private void Explode()
    {
        AudioSource.PlayClipAtPoint(slamAudioClip, transform.position, 0.5f);
        Instantiate(explosionPrefab, transform.position, Quaternion.Euler(-90, 0, 0));

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius + currentSlamTime * 2, whatIsEnemies);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float force = Mathf.Clamp(currentSlamTime * 25f * Mathf.Sqrt(rb.mass), 0f, 160f);
                rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            }
            hit.GetComponent<PartDamageControll>().applyDamage((int)(explosionDamage * currentSlamTime * 2));
        }
    }
}
