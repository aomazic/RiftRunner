using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private float typeSpeed = 0.05f;

    public static TutorialManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisplayTutorial(string message)
    {
        StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string text)
    {
        tutorialText.text = "";
        foreach (char letter in text.ToCharArray())
        {
            tutorialText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void HideTutorial()
    {
        tutorialText.text = "";
    }
}