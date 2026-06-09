using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPauseManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainButton;

    [Header("Scene Settings")]
    [SerializeField] private string[] allowedScenes = { "level1", "level2", "level3" };

    private void Start()
    {
        // Hubungkan tombol
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        mainButton.onClick.AddListener(OnMainClicked);
        resumeButton.onClick.AddListener(OnResumeButtonClicked);

        // Dengarkan event dari GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPaused.AddListener(ShowPausePanel);
            GameManager.Instance.OnResumed.AddListener(HidePausePanel);
        }

        // Tentukan visibilitas awal berdasarkan scene saat ini
        UpdateVisibility(SceneManager.GetActiveScene().name);

        // Dengarkan setiap kali pindah scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!IsSceneAllowed()) return;
        if (GameManager.Instance == null) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.IsPaused)
                GameManager.Instance.ResumeGame();
            else 
                GameManager.Instance.PauseGame();
        }
    }

    private bool IsSceneAllowed()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        foreach (string s in allowedScenes)
        {
            if (s == sceneName) return true;
        }
        return false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateVisibility(scene.name);
        // Pastikan panel pause selalu tersembunyi saat pindah scene
        HidePausePanel();
    }

    private void UpdateVisibility(string sceneName)
    {
        bool allowed = false;
        foreach (string s in allowedScenes)
        {
            if (s == sceneName)
            {
                allowed = true;
                break;
            }
        }

        pauseButton.gameObject.SetActive(allowed);
    }

    private void OnPauseButtonClicked()
    {
        Debug.Log("Button pressed.");
        GameManager.Instance.PauseGame();
    }

    private void OnResumeButtonClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    public void OnQuitClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnMainClicked()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.IsPaused)
                GameManager.Instance.ResumeGame();
        }
        GameManager.Instance.SaveProgress();
        
        HidePausePanel();
        SceneManager.LoadScene("MainMenu");
    }

    private void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    private void HidePausePanel()
    {
        pausePanel.SetActive(false);
        UpdateVisibility(SceneManager.GetActiveScene().name);
    }
}