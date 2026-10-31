using UnityEngine;

public class GameplayMusicStart : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayGameplayMusic();
    }
}
