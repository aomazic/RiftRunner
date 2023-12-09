using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float typeSpeed = 0.05f;  
    [SerializeField] private TextMeshProUGUI pressAnyKeyText;
    private bool canProceed = false;  
    private float fadeSpeed = 0.5f;
    private bool isFadingIn = true;

    private void Start()
    {

        StartCoroutine(DisplayStats());
    }
    void Update()
    {
        if (canProceed && Input.anyKeyDown)
        {
            StartCoroutine(FadeOutText(pressAnyKeyText));
            SceneManager.LoadScene(GameManager.Instance.currentLevelIndex + 1);
        }
    }


    private IEnumerator DisplayStats()
    {
        yield return StartCoroutine(TypeText(GameManager.Instance.playerScore.ToString(), scoreText));

        string formattedTime = GameManager.Instance.gameTime.ToString("F2");
        yield return StartCoroutine(TypeText(formattedTime, timeText));

        canProceed = true;
        yield return StartCoroutine(PingPongFade(pressAnyKeyText));


     
    }

    private IEnumerator PingPongFade(TextMeshProUGUI textElement)
    {
        while (true)
        {
            if (isFadingIn)
            {
                yield return StartCoroutine(FadeInText(textElement));
                isFadingIn = false;
            }
            else
            {
                yield return StartCoroutine(FadeOutText(textElement));
                isFadingIn = true;
            }
        }
    }

    private IEnumerator FadeInText(TextMeshProUGUI textElement)
    {
        Color textColor = textElement.color;

        while (textColor.a < 1.0f)
        {
            textColor.a += fadeSpeed * Time.deltaTime;
            textElement.color = textColor;
            yield return null;
        }
    }

    private IEnumerator FadeOutText(TextMeshProUGUI textElement)
    {
        Color textColor = textElement.color;

        while (textColor.a > 0.0f)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textElement.color = textColor;
            yield return null;
        }
    }


    private IEnumerator TypeText(string text, TextMeshProUGUI targetText)
    {
        targetText.text = ""; 

        foreach (char letter in text.ToCharArray())
        {
            targetText.text += letter;  
            yield return new WaitForSeconds(typeSpeed); 
        }
    }
}
