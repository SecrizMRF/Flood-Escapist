using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ===== Singleton =====
    public static GameManager Instance { get; private set; }

    // ===== Events (Observer) =====
    public UnityEvent<int> OnLivesChanged = new();
    public UnityEvent<int> OnScoreChanged = new();
    public UnityEvent OnLevelComplete = new();
    public UnityEvent OnGameOver = new();
    public UnityEvent OnPaused = new();
    public UnityEvent OnResumed = new();
    public UnityEvent<bool> OnContinueAvailable = new();

    // === State ===
    private bool isPaused = false;
    public bool IsPaused => isPaused;

    // ===== Config =====
    [SerializeField] private string[] levelNames = { "level1", "level2", "level3" };

    // ===== State =====
    private int lives;
    private int score;
    public int Lives => lives;
    public int Score => score;
    private string currentLevelName;

    // ===== High Score =====
    private int highScore;
    public int HighScore => highScore;
    public UnityEvent<int> OnHighScoreChanged = new();

    // == Update MAIN MENU UI
    private enum GameState { MainMenu, Playing }
    private GameState currentState = GameState.MainMenu;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // === Update MAIN MENU
    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        OnHighScoreChanged?.Invoke(highScore); // kirim ke UI saat awal

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Preload")
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        if (sceneName == "MainMenu")
        {
            currentState = GameState.MainMenu;
            Debug.Log("Game state => MainMenu");
        }
        else
        {
            currentState = GameState.Playing;
            NewGame();
        }
    }

    // ===== Method Pause & Resume =====
    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0f;                       // bekukan semua gerakan
        OnPaused?.Invoke();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1f;                       // kembalikan kecepatan normal
        OnResumed?.Invoke();
    }

    private void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); // pastikan data tertulis
            OnHighScoreChanged?.Invoke(highScore);
            Debug.Log($"High Score baru: {highScore}");
        }
    }

    // === Update MAIN MENU
    public void StartNewGame()
    {
        ClearProgress();
        currentState = GameState.Playing;
        NewGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Tidak perlu mencari LivesUI lagi karena UI akan subscribe sendiri
        // Kirimkan update lives saat scene baru dimuat
        OnLivesChanged?.Invoke(lives);
        OnScoreChanged?.Invoke(score);
    }

    private void NewGame()
    {
        Time.timeScale = 1f;
        lives = 3;
        score = 0;
        // Pastikan UI mendapat nilai awal
        OnLivesChanged?.Invoke(lives);
        OnScoreChanged?.Invoke(score);

        LoadLevel(levelNames[0]);
    }

    // === Update Save Data
    public void SaveProgress()
    {
        if (currentState != GameState.Playing) return;
        PlayerPrefs.SetString("CurrentLevel", currentLevelName);
        PlayerPrefs.SetInt("CurrentLives", lives);
        PlayerPrefs.SetInt("CurrentScore", score);
        PlayerPrefs.Save();
        Debug.Log($"Berhasil simpan data {currentLevelName}, {lives}, {score}");
    }

    public void ClearProgress()
    {
        PlayerPrefs.DeleteKey("CurrentLevel");
        PlayerPrefs.DeleteKey("CurrentLives");
        PlayerPrefs.DeleteKey("CurrentScore");
        PlayerPrefs.Save();
        OnContinueAvailable?.Invoke(false);
    }

    public bool SavedProgress()
    {
        return PlayerPrefs.HasKey("CurrentLevel");
    }

    public void ContinueGame()
    {
        if(!SavedProgress()) return;
        currentState = GameState.Playing;
        lives = PlayerPrefs.GetInt("CurrentLives", 3);
        score = PlayerPrefs.GetInt("CurrentScore", 0);
        string levelToLoad = PlayerPrefs.GetString("CurrentLevel", levelNames[0]);
        OnLivesChanged?.Invoke(lives);
        OnScoreChanged?.Invoke(score);
        LoadLevel(levelToLoad);
    }

    private void LoadLevel(string sceneName)
    {
        currentLevelName = sceneName;
        Camera camera = Camera.main;
        if (camera != null)
            camera.cullingMask = 0;

        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(currentLevelName);
    }

    public void LevelComplete()
    {
        score += 1000;
        OnScoreChanged?.Invoke(score);
        OnLevelComplete?.Invoke();
        UpdateHighScore();

        int currentIndex = System.Array.IndexOf(levelNames, currentLevelName);
        if (currentIndex == 2)
        {
            ClearProgress();
            SceneManager.LoadScene("MainMenu");
        } 
        int nextIndex = currentIndex + 1;
        LoadLevel(levelNames[nextIndex]);
        SaveProgress();
    }

    public void LevelFailed()
    {
        lives--;
        OnLivesChanged?.Invoke(lives);

        if (lives <= 0)
        {
            UpdateHighScore();
            OnGameOver?.Invoke();
            ClearProgress();
            NewGame();
        }
        else
        {
            LoadLevel(currentLevelName);
            SaveProgress();
        }
    }
}