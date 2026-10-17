using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartButton is pressed");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Debug.Log("ExitButton is pressed");
        Application.Quit();
    }
}
