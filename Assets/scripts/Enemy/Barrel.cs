using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] int explosionDamage;
    [SerializeField] float explosionRange;
    [SerializeField] float explosionForce;
    [SerializeField] LayerMask whatIsEnemies;
    [SerializeField] GameObject deathExplosion;
    [SerializeField] GameObject smoke;
     private AudioClip explosionSound;

    void Start()
    {

        smoke.GetComponent<ParticleSystem>().Pause();
    }

    public void StartBarrelExplosion(AudioClip explosionSound)
    {
        this.explosionSound = explosionSound;
        smoke.GetComponent<ParticleSystem>().Play();
        Invoke("BarrelExplode", 2);
    }

    private void BarrelExplode()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        foreach (Collider enemy in enemies)
        {
            DamageControll enemyDamageControl = enemy.GetComponent<DamageControll>();
            if (enemyDamageControl != null)
            {
                enemyDamageControl.TakeDamage(explosionDamage);
            }

            if (enemy.GetComponent<Rigidbody>())
            {
                enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }

        if (deathExplosion != null)
        {
            Instantiate(deathExplosion, transform.position, Quaternion.identity);
        }

        AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        Destroy(gameObject);
    }
}
