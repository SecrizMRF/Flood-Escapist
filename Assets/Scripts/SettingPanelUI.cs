using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanelUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        // Inisialisasi slider
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Setup dropdown
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(new System.Collections.Generic.List<string> {
            "640x360",
            "1280x720",
            "1920x1080"
        });
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0: Screen.SetResolution(640, 360, false); break;
            case 1: Screen.SetResolution(1280, 720, false); break;
            case 2: Screen.SetResolution(1920, 1080, false); break;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}