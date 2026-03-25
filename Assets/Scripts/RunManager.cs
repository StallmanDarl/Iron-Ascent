using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    // Singleton allows global access
    public static RunManager Instance;

    [Header("Run Progress (Resets Each Run)")]
    public int arenasCompleted = 0;
    public int metaUpgradeThreshold = 3; // After 3 arenas
    public int metaTier = 0;

    void Awake()
    {
        // Ensure only one RunManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists between scenes
            Debug.Log("RunManager created.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Creates RunManager automatically if missing
    public static void EnsureExists()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("RunManager");
            Instance = obj.AddComponent<RunManager>();
        }
    }

    // Called when an arena is cleared
    public void ArenaCompleted()
    {
        arenasCompleted++;
        Debug.Log("Arena Count: " + arenasCompleted);
    }

    // Called by doors to determine next scene
    public void LoadNextArena(string nextArenaName)
    {
        if (arenasCompleted >= metaUpgradeThreshold)
        {
            TransitionManager.Instance.LoadSceneWithSpawn("MetaUpgradeScene", "MetaRoom");
        }
        else
        {
            TransitionManager.Instance.LoadSceneWithSpawn(nextArenaName, "Arena");
        }
    }

    public void MetaUpgradeCollected()
    {
        Debug.Log("Meta Upgrade Complete - Starting New Run");

        ResetRunProgress();
        TransitionManager.Instance.LoadSceneWithSpawn("HomeBase", "FromScene");
        PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth;
    }

    // Called when run ends
    public void ResetRunProgress()
    {
        arenasCompleted = 0;
        Debug.Log("Run progress reset.");
    }
}