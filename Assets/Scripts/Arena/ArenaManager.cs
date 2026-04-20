using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance;

    int totalPockets;
    int clearedPockets;
    bool arenaComplete = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        totalPockets = FindObjectsOfType<EnemyPocket>().Length;
        Debug.Log("Total Pockets: " + totalPockets);
    }

    public void PocketCleared()
    {
        clearedPockets++;

        if (clearedPockets >= totalPockets && !arenaComplete)
        {
            arenaComplete = true;
            ArenaCleared();
        }
    }

    void ArenaCleared()
    {
        Debug.Log("Arena Cleared!");
        RunManager.Instance.ArenaCompleted();

        UpgradeManager.EnsureExists();
        UpgradeManager.Instance.ShowArenaUpgradeChoices(UnlockDoors);
    }

    void UnlockDoors()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("ArenaDoor");

        foreach (GameObject door in doors)
            door.GetComponent<ArenaDoor>().Unlock();
    }
}
