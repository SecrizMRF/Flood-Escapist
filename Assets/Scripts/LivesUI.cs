using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Image[] heartImages;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged.AddListener(UpdateHearts);
            UpdateHearts(GameManager.Instance.Lives);
        }
    }

    public void UpdateHearts(int lives)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = (i < lives);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe untuk mencegah memory leak
        if (GameManager.Instance != null)
            GameManager.Instance.OnLivesChanged.RemoveListener(UpdateHearts);
    }
}