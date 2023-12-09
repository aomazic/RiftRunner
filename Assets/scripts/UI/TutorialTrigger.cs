using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialM : MonoBehaviour
{
    [SerializeField] string tutorialMessage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialManager.Instance.DisplayTutorial(tutorialMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialManager.Instance.HideTutorial();
        }
    }
}
