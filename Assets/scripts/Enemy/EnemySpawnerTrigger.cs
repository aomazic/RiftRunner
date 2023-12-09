using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] spawners;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject spawner in spawners)
            {
                spawner.GetComponent<SpawnEnemy>().Spawn();
            }
            Destroy(gameObject);
        }
    }
}
