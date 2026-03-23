using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance;

    int totalPockets;
    int clearedPockets;

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

        if (clearedPockets >= totalPockets)
            ArenaCleared();
    }

    void ArenaCleared()
    {
        Debug.Log("Arena Cleared!");
        UnlockDoors();
    }

    void UnlockDoors()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("ArenaDoor");

        foreach (GameObject door in doors)
            door.GetComponent<ArenaDoor>().Unlock();
    }
}