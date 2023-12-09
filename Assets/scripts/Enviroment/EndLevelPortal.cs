using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelPortal : MonoBehaviour
{
    [SerializeField] AudioClip teleport;
    [SerializeField] AudioSource audioSource;
    bool entered = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !entered)
        {
            entered = true;
            if (teleport != null && audioSource != null)
            {
                audioSource.clip = teleport;
                audioSource.Play();
                GameManager.Instance.currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
                GameManager.Instance.startingGameTime = GameManager.Instance.gameTime;
                GameManager.Instance.startingScore = GameManager.Instance.playerScore;
                if (GameManager.Instance.currentLevelIndex == 6)
                    GameManager.Instance.LoadSceneAfterDelay("WinScreen", teleport.length);
                else
                    GameManager.Instance.LoadSceneAfterDelay("StatsScene", teleport.length);
            }
            else 
            {
                GameManager.Instance.LoadSceneAfterDelay("StatsScene", 0f);
            }
        }
    }
}
