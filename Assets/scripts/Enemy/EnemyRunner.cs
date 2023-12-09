using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRunner : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    AudioSource audioSource;

    [SerializeField] LayerMask whatIsPlayer;

    // Attacking
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] AudioClip[] attackAudioClips;
    [SerializeField] AudioClip chaseAudioClip;
    [SerializeField] int damage = 40;
    [SerializeField] Transform attackPoint;
    Transform playerTransform;

    bool alreadyAttacked;

    // States
    [SerializeField] float attackRange;
    [SerializeField] float detectionRange;
    bool playerInAttackRange;
    bool playerInDetectionRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerTransform = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(attackPoint.position, attackRange, whatIsPlayer);
        playerInDetectionRange = Physics.CheckSphere(attackPoint.position, detectionRange, whatIsPlayer);

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
        animator.SetBool("attack", false);
    }


    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
        animator.SetBool("isRunning", true);
        animator.SetBool("attack", false);
        PlayChaseAudio();
    }

    private void AttackPlayer()
    {
        animator.SetBool("attack", true);
        animator.SetBool("isRunning", false);

        agent.SetDestination(transform.position);
        Vector3 directionToPlayer = (playerTransform.position - attackPoint.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
   

        transform.rotation = targetRotation;

        if (!alreadyAttacked)
        {
            RaycastHit hit;
            if (Physics.SphereCast(attackPoint.position, 0.5f, transform.forward, out hit, attackRange, whatIsPlayer))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    hit.collider.gameObject.GetComponent<PartDamageControll>().applyDamage(damage);
                }
            }
            PlayAttackAudio();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void PlayAttackAudio()
    {
        if (audioSource != null && attackAudioClips != null && attackAudioClips.Length > 0)
        {
            AudioClip randomClip = attackAudioClips[UnityEngine.Random.Range(0, attackAudioClips.Length)];
            audioSource.clip = randomClip;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    private void PlayChaseAudio()
    {
        if (audioSource != null && chaseAudioClip != null)
        {
            if (!audioSource.isPlaying || audioSource.clip != chaseAudioClip) 
            {
                audioSource.loop = true;
                audioSource.clip = chaseAudioClip;
                audioSource.Play();
            }
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
}
