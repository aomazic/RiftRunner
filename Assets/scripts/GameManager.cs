using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;
    public float playerScore;
    public float startingScore; 
    public float gameTime;
    public int currentLevelIndex = 0;
    public float startingGameTime;
    private bool stats;
    private bool shouldResetStats = false;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "StatsScene" && scene.name != "WinScreen" && scene.name != "Start")
        {
            stats = false;
            FindReferences();

            if (shouldResetStats)
            {
                ResetScore();
                shouldResetStats = false;
            }
        }
        else
        {
            stats = true;
        }
    }


    private void FindReferences()
    {
        // Find references during runtime.
        if (!timerText)
            timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();

        if (!scoreText)
            scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();

        gameTime = 0; 
        UpdateUI();
    }
    void Update()
    {
        if(!stats)
        { 
            gameTime += Time.deltaTime;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        timerText.text = gameTime.ToString("F2");
        scoreText.text = playerScore.ToString();
    }

    public void KillEnemy()
    {
        playerScore += 100;
        UpdateUI();
    }

    public void ResetScore()
    {
        playerScore = startingScore;
        gameTime = startingGameTime;
        UpdateUI();
    }

    public void LoadSceneAfterDelay(string sceneName, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, delay));
    }

    public void Restart()
    {
        shouldResetStats = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
    public void SaveHighScore()
    {
        float bestScore = PlayerPrefs.GetFloat("HighScore", 0);
        if (playerScore > bestScore)
        {
            PlayerPrefs.SetFloat("HighScore", playerScore);
        }
    }

    public float LoadHighScore()
    {
        return PlayerPrefs.GetFloat("HighScore", 0);
    }

    public void SaveBestTime()
    {
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        if (gameTime < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", gameTime);
        }
    }

    public float LoadBestTime()
    {
        return PlayerPrefs.GetFloat("BestTime", float.MaxValue);
    }

}


