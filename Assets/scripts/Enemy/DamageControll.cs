using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageControll : MonoBehaviour
{
    public float health;
    [SerializeField] GameObject deathExplosion;
    [SerializeField] bool isEnemy;
    [SerializeField] bool isBarrel = false;
    [SerializeField] bool isEnviroment = false;
    [SerializeField] AudioClip deathSound;
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) 
        {

            if (isEnemy)
            {

                
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null && deathSound != null)
                {
                    audioSource.clip = deathSound;
                    audioSource.Play();
                }
                if (deathExplosion != null)
                    Instantiate(deathExplosion, transform.position, Quaternion.identity);
                StartCoroutine(DestroyEnemyObject(0.01f));
            }

            else if (isBarrel)
            {


                AudioSource audioSource = GetComponent<AudioSource>();
                if (deathSound != null)
                {
                    GetComponent<Barrel>().StartBarrelExplosion(deathSound);
                }
            }
            else if (isEnviroment)
            {
                if (deathExplosion != null)
                    Instantiate(deathExplosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null && deathSound != null)
                {
                    audioSource.clip = deathSound;
                    audioSource.Play();
                }
                if (deathExplosion != null)
                {
                    deathExplosion.transform.localScale = new Vector3(3f,3f,3f);
                    Instantiate(deathExplosion, transform.position, Quaternion.identity);
                  
                }
                GameObject.Find("GameManager").GetComponent<GameManager>().Restart();
            
            }

        }
        
    }
    private IEnumerator DestroyEnemyObject(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        FindAnyObjectByType<GameManager>().KillEnemy();
    }

}
