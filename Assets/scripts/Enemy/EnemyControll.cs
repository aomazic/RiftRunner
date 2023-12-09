
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControll : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    AudioSource audioSource;
    [SerializeField] Transform attackPoint;

    [SerializeField] LayerMask whatIsPlayer;

    // Attacking
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] GameObject projectile;
    [SerializeField] AudioClip shootAudioClip;
    [SerializeField] float shootingForce;

    bool alreadyAttacked;

    // States
    [SerializeField] float attackRange;
    [SerializeField] float detectionRange;
    bool playerInAttackRange;
    bool playerInDetectionRange;

    private void Awake()
    {
        this.Delay(0.4f);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInDetectionRange = Physics.CheckSphere(transform.position, detectionRange, whatIsPlayer);

        if (!playerInDetectionRange)
        {
            Idle();
        }
        else if (!playerInAttackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    private void Idle()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("isRunning", false);
        animator.SetBool("shoot", false);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(GameObject.Find("Player").transform.position);
        animator.SetBool("isRunning", true);
        animator.SetBool("shoot", false);
    }

    private void AttackPlayer()
    {
        animator.SetBool("shoot", true);
        animator.SetBool("isRunning", false);

        agent.SetDestination(transform.position);
        Vector3 directionToPlayer = (GameObject.Find("Player").transform.position - attackPoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

        transform.rotation = targetRotation;

        if (!alreadyAttacked)
        {
            // Instantiate the projectile at the attack point with the correct rotation
            GameObject bulletInstance = Instantiate(projectile, attackPoint.position, targetRotation);

            // Get the Rigidbody component of the instantiated bullet
            Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();

            // Apply the shooting force to the bullet
            bulletRigidbody.AddForce(directionToPlayer * shootingForce, ForceMode.Impulse);

            PlayShootAudio();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void PlayShootAudio()
    {
        if (audioSource != null && shootAudioClip != null)
        {
            audioSource.clip = shootAudioClip;
            audioSource.Play();
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    private IEnumerator Delay(float delay)
    {
       yield return new WaitForSeconds(delay);
    }
}
