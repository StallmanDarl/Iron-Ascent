using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        RunManager.EnsureExists();
        RunManager.Instance.ResetRunProgress();
        TransitionManager.Instance.LoadSceneWithSpawn("HomeBase", "FromScene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}