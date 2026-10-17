using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] Behaviour _cameraMovement;

    private bool IsPaused = false;
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
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
    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
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
}
