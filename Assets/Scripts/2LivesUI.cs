using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Image[] heartImages;

    private void Start()
    {
        // Subscribe ke event GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged.AddListener(UpdateHearts);
            // Tampilkan lives saat ini
            UpdateHearts(GameManager.Instance.Lives); // butuh getter? 
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