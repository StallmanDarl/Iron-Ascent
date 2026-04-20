using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        RunManager.EnsureExists();
        UpgradeManager.EnsureExists();
        RunManager.Instance.ResetRunProgress();
        TransitionManager.Instance.LoadSceneWithSpawn("HomeBase", "FromScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}