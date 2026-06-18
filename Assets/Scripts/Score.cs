using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    private void Start()
    {
        if (GameManager.Instance == null) return;

        // Subs ke eventnya
        GameManager.Instance.OnScoreChanged.AddListener(UpdateScore);
        GameManager.Instance.OnHighScoreChanged.AddListener(UpdateHighScore);

        UpdateScore(GameManager.Instance.Score);
        UpdateHighScore(GameManager.Instance.HighScore);
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
    }

    private void UpdateHighScore (int hScore)
    {
        if (highscoreText != null) highscoreText.text = $"High Score: {hScore}";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged.RemoveListener(UpdateScore);
            GameManager.Instance.OnHighScoreChanged.RemoveListener(UpdateHighScore);
        }
    }
}