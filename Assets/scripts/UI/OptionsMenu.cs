
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] Slider mouseSensitivitySlider;
    [SerializeField] TextMeshProUGUI sensValue;

    private void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 100f); 
        mouseSensitivitySlider.value = savedSensitivity;
        sensValue.text = savedSensitivity.ToString();
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        sensValue.text = sensitivity.ToString();
    }
}
