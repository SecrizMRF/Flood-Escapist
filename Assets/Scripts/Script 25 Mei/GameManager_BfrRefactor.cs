using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBefore : MonoBehaviour
{
    [SerializeField] private string[] levelNames = { "level1", "level2", "level3" };
    private int lives;
    private int score;
    private string currentLevelName;
    private LivesUI livesUI;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        NewGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        livesUI = FindFirstObjectByType<LivesUI>();
        if (livesUI != null)
        {
            livesUI.UpdateHearts(lives);
        }
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        LoadLevel(levelNames[0]);
    }

    private void LoadLevel(string sceneName)
    {
        currentLevelName = sceneName;

        Camera camera = Camera.main;

        if (camera != null)
        {
            camera.cullingMask = 0;
        }
        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(currentLevelName);
    }

    public void LevelComplete()
    {
        score += 1000;

        int currentIndex = System.Array.IndexOf(levelNames, currentLevelName);
        int nextIndex = (currentIndex + 1) % levelNames.Length;
        LoadLevel(levelNames[nextIndex]);
    }

    public void LevelFailed()
    {
        lives--;

        if (livesUI != null) livesUI.UpdateHearts(lives);

        if (lives <= 0)
        {
            NewGame();
        }
        else
        {
            LoadLevel(currentLevelName);
        }
    }
}
