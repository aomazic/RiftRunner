using System.Collections;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject spawnInEffect;
    [SerializeField] GameObject enemy;
    [SerializeField] AudioClip spawnInAudio;
    [SerializeField] float minSpawnDelay = 0.1f;  
    [SerializeField] float maxSpawnDelay = 2f;  

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
   public void Spawn()
    {
        float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        StartCoroutine(SpawnEnemyAfterDelay(randomDelay));
   
    }

    IEnumerator SpawnEnemyAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        if (spawnInEffect)
        {
            Instantiate(spawnInEffect, transform.position, Quaternion.identity);
        }

        if (audioSource && spawnInAudio)
        {
            audioSource.PlayOneShot(spawnInAudio);
        }

        Instantiate(enemy, transform.position, Quaternion.identity);
        StartCoroutine(DestrpySpawnPoint());
    }
    IEnumerator DestrpySpawnPoint()
    {

        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

}
