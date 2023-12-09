using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] Slider mouseSensitivitySlider;
    [SerializeField] TextMeshProUGUI sensValue;
    private bool isPaused = false;

    private void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 100f);
        mouseSensitivitySlider.value = savedSensitivity;
        sensValue.text = savedSensitivity.ToString();
        gamePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0; 
        gamePanel.SetActive(false);
        optionsPanel.SetActive(true); 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; 
        gamePanel.SetActive(true);
        optionsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        isPaused = false;
    }
    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        PlayerCam playerCam = FindObjectOfType<PlayerCam>();
        if (playerCam != null)
        {
            playerCam.updateSens();
        }
        sensValue.text = sensitivity.ToString();
    }
}
