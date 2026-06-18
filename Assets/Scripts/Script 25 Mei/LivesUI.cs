using UnityEngine;
using UnityEngine.UI;

public class LivesUIOld : MonoBehaviour
{
    public Image[] heartImages; // Assign 3 gambar hati di Inspector

    public void UpdateHearts(int lives)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = (i < lives);
        }
    }
}