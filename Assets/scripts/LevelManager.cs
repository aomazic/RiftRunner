using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float delayInSeconds = 0.4f;
    [SerializeField] private ParticleSystem particleSystemToModify;
    [SerializeField] private AudioClip teleport;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1.0f;

    private float targetParticleSpeed = 40f;
    private float speedIncreaseRate = 10f;  

    public void LoadNextLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

    public void LoadFirstLevel()
    {
        DisableAllButtons();
        SwitchAndPlayAudio();
        StartCoroutine(IncreaseParticleSpeed());
        StartCoroutine(LoadLevelFirstLevel());
    }

    public void LoadQuit()
    {
        StartCoroutine(QuitAfterDelay());
    }

    public void LoadLose()
    {
        StartCoroutine(LoadLevelNameAfterDelay("Lose"));
    }

    public void LoadStart()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(LoadLevelNameAfterDelay("Start"));
    }

    private IEnumerator IncreaseParticleSpeed()
    {
        if (particleSystemToModify != null)
        {
            var main = particleSystemToModify.main;
            float currentSpeed = main.startSpeed.constant;
            while (currentSpeed < targetParticleSpeed)
            {
                currentSpeed += speedIncreaseRate * Time.deltaTime;
                main.startSpeed = currentSpeed;
                yield return null;
            }
            main.startSpeed = targetParticleSpeed;
        }
    }

    private IEnumerator LoadLevelFirstLevel()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Level1");
    }

    private IEnumerator LoadLevelNameAfterDelay(string levelName)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(levelName);
    }

    private IEnumerator QuitAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        Application.Quit();
    }
    private void SwitchAndPlayAudio()
    {
        AudioSource mainCameraAudioSource = Camera.main.GetComponent<AudioSource>();
        if (mainCameraAudioSource != null && teleport != null)
        {
            mainCameraAudioSource.clip = teleport;
            mainCameraAudioSource.Play();
        }
    }
    private void DisableAllButtons()
    {
        StartCoroutine(FadeOutCanvas());
    }
    private IEnumerator FadeOutCanvas()
    {
        float startAlpha = canvasGroup.alpha;

        for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, normalizedTime);
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;  
    }
}
