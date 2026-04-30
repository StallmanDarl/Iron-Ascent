using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RunManager : MonoBehaviour
{
    // Singleton allows global access
    public static RunManager Instance;

    [Header("Run Progress (Resets Each Run)")]
    public int arenasCompleted = 0;
    public int metaUpgradeThreshold = 3; // After 3 arenas
    public int metaTier = 0;

    [Header("Arena Rotation")]
    [SerializeField] string[] arenaSceneNames = { "ColosseumArena", "GrandStadiumArena", "AscendingArena", "TowerArena", /*"BoxArena", "BoxJumpArena"*/ };
    [SerializeField] int recentArenaMemory = 1;

    readonly List<string> recentArenaHistory = new List<string>();

    void Awake()
    {
        // Ensure only one RunManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists between scenes
            Debug.Log("RunManager created.");
            GameManager.EnsureExists();
            UpgradeManager.EnsureExists();
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
            GameManager.EnsureExists();
            UpgradeManager.EnsureExists();
        }
    }

    // Called when an arena is cleared
    public void ArenaCompleted()
    {
        arenasCompleted++;
        Debug.Log("Arena Count: " + arenasCompleted);
    }

    // Called by doors to determine next scene
    public void LoadNextArena(string fallbackArenaName = null)
    {
        if (arenasCompleted >= metaUpgradeThreshold)
        {
            TransitionManager.Instance.LoadSceneWithSpawn("MetaUpgradeScene", "MetaRoom");
        }
        else
        {
            string nextArenaName = GetRandomArenaName(fallbackArenaName);
            TransitionManager.Instance.LoadSceneWithSpawn(nextArenaName, "Arena");
        }
    }

    public void MetaUpgradeCollected()
    {
        Debug.Log("Meta Upgrade Complete - Starting New Run");
        metaTier++;
        ResetRunProgress();
        TransitionManager.Instance.LoadSceneWithSpawn("HomeBase", "FromScene");
        PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth;
    }

    // Called when run ends
    public void ResetRunProgress()
    {
        arenasCompleted = 0;
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ResetRunUpgrades();
        }
        Debug.Log("Run progress reset.");
    }

    string GetRandomArenaName(string fallbackArenaName)
    {
        List<string> validArenaNames = GetValidArenaNames();

        if (validArenaNames.Count == 0)
        {
            if (!string.IsNullOrWhiteSpace(fallbackArenaName))
            {
                Debug.LogWarning("Arena rotation list is empty or invalid. Falling back to: " + fallbackArenaName);
                return fallbackArenaName;
            }

            Debug.LogError("No valid arena scenes are configured on RunManager.");
            return SceneManager.GetActiveScene().name;
        }

        List<string> candidateArenaNames = new List<string>();
        for (int i = 0; i < validArenaNames.Count; i++)
        {
            string arenaName = validArenaNames[i];
            if (!recentArenaHistory.Contains(arenaName))
            {
                candidateArenaNames.Add(arenaName);
            }
        }

        if (candidateArenaNames.Count == 0 && validArenaNames.Count > 1 && recentArenaHistory.Count > 0)
        {
            string mostRecentArena = recentArenaHistory[recentArenaHistory.Count - 1];

            for (int i = 0; i < validArenaNames.Count; i++)
            {
                string arenaName = validArenaNames[i];
                if (arenaName != mostRecentArena)
                {
                    candidateArenaNames.Add(arenaName);
                }
            }
        }

        if (candidateArenaNames.Count == 0)
        {
            candidateArenaNames.AddRange(validArenaNames);
        }

        string selectedArena = candidateArenaNames[Random.Range(0, candidateArenaNames.Count)];
        RememberArena(selectedArena, validArenaNames.Count);

        Debug.Log("Loading next arena: " + selectedArena);
        return selectedArena;
    }

    List<string> GetValidArenaNames()
    {
        List<string> validArenaNames = new List<string>();

        for (int i = 0; i < arenaSceneNames.Length; i++)
        {
            string arenaName = arenaSceneNames[i];

            if (string.IsNullOrWhiteSpace(arenaName))
            {
                continue;
            }

            if (!Application.CanStreamedLevelBeLoaded(arenaName))
            {
                Debug.LogWarning("Arena scene is not in Build Settings and will be skipped: " + arenaName);
                continue;
            }

            if (!validArenaNames.Contains(arenaName))
            {
                validArenaNames.Add(arenaName);
            }
        }

        return validArenaNames;
    }

    void RememberArena(string arenaName, int validArenaCount)
    {
        recentArenaHistory.Remove(arenaName);
        recentArenaHistory.Add(arenaName);

        int maxHistory = Mathf.Clamp(recentArenaMemory, 0, Mathf.Max(0, validArenaCount - 1));
        while (recentArenaHistory.Count > maxHistory)
        {
            recentArenaHistory.RemoveAt(0);
        }
    }
}
