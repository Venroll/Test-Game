using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _failMenu;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private PlayerHealth _player;
    [SerializeField] Behaviour _cameraMovement;
    [SerializeField] Behaviour _playerMovement;

    private bool playerDead;

    private bool IsPaused = false;

    private void Awake()
    {
        playerDead = _player.IsDead;
    }
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !playerDead)
        {
            playerDead = _player.IsDead;
            if (!playerDead)
            {
                if (!IsPaused)
                {
                    _pauseMenu.SetActive(true);
                    ActivatePause();
                }
                else
                {
                    _pauseMenu.SetActive(false);
                    DeactivatePause();
                }
            }
        }
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void ActivatePause()
    {
        Time.timeScale = 0f;
        IsPaused = true;
        _cameraMovement.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void DeactivatePause()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        _cameraMovement.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ActivateFailMenu()
    {
        _failMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        MusicManager.Instance.PlayDefeatMusic();
    }
    public void ActivateWinMenu()
    {
        _winMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
        _playerMovement.enabled = false;
        _cameraMovement.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        MusicManager.Instance.PlayVictoryMusic();
    }
}
