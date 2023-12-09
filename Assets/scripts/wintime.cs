using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winTimeText;

    void Start()
    {
        if (PlayerPrefs.HasKey("WinTime"))
        {
            float winTime = PlayerPrefs.GetFloat("WinTime");
            winTimeText.text = "Win Time: " + winTime.ToString("F2");
        }
        else
        {
            winTimeText.text = "Win Time: N/A";
        }
    }
}