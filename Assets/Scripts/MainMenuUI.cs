using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
        newGameButton.onClick.AddListener(OnPlayClicked);
        // settingsButton.onClick.AddListener(OnSettingsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

        if (GameManager.Instance != null)
        {
            bool hasProgress = GameManager.Instance.SavedProgress();
            continueButton.gameObject.SetActive(hasProgress);
            GameManager.Instance.OnContinueAvailable.AddListener(SetContinueButton);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnContinueAvailable.RemoveListener(SetContinueButton);
    }

    private void SetContinueButton(bool available)
    {
        continueButton.gameObject.SetActive(available);
    }

    public void OnContinueClicked()
    {
        GameManager.Instance.ContinueGame();
    }

    public void OnPlayClicked()
    {
        GameManager.Instance.StartNewGame();
    }

    public void OnSettingsClicked()
    {
        settingsPanel.SetActive(true);
    }

    public void OnQuitClicked()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}