using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Loads Arena scene
    public void LoadArena()
    {
        // Later: Link the start of the game 
        SceneManager.LoadScene("TestScene");
    }

    // Loads Tutorial scene (optional later)
    public void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    // Quit game
    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }
}