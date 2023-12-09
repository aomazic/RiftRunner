using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilControll : MonoBehaviour
{

    [SerializeField] private GameObject explosion;
    [SerializeField] private LayerMask whatIsEnemies;

    [SerializeField] private float bounciness;
    [SerializeField] private bool useGravity;

    [SerializeField] private float explosionDamage;
    [SerializeField] private float explosionRange;
    [SerializeField] private float explosionForce;

    [SerializeField] private int maxCollisions;
    [SerializeField] private float maxLifetime;

    [SerializeField] private AudioClip enemyCollisionSound;
    [SerializeField] private AudioClip collisionSound;

    [SerializeField] private GameObject collisionEffect;
    [SerializeField] private GameObject enemyCollisionEffect;


    private void FixedUpdate()
    {
        // Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {

        if (explosion != null)
        {
            GameObject instantiatedExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            ParticleSystem particleSystem = instantiatedExplosion.GetComponent<ParticleSystem>();

            var main = particleSystem.main;

  
            float exponentialFactor = Mathf.Pow(1f+explosionRange, 4); 
            float minSpeed = 5f + exponentialFactor * 0.5f;  
            float maxSpeed = 10f + exponentialFactor * 1f;   

            main.startSpeed = Random.Range(minSpeed, maxSpeed);

            particleSystem.Play();
        }


        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        foreach (Collider enemy in enemies)
        {
            PartDamageControll partDamageControll = enemy.GetComponent<PartDamageControll>();
            if (partDamageControll != null)
            {
                partDamageControll.applyDamage((int)explosionDamage);
            }

            Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collisionEffect != null)
        {
            if ((collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player")) && enemyCollisionEffect != null)
            {
                if(enemyCollisionSound != null)
                AudioSource.PlayClipAtPoint(enemyCollisionSound, transform.position);
                Instantiate(enemyCollisionEffect, collision.contacts[0].point, Quaternion.identity);

            }
            else
            {
               AudioSource.PlayClipAtPoint(collisionSound, transform.position);
               Instantiate(collisionEffect, collision.contacts[0].point, Quaternion.identity);
            }

        }

            Explode();
    }
    public void ModifyBullet(float damageMultiplier, float damageRangeMultiplier)
    {
        explosionDamage *= damageMultiplier;
        explosionRange *= damageRangeMultiplier;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
