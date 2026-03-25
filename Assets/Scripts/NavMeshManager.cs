using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshUpdater : MonoBehaviour
{
    public static NavMeshUpdater Instance;
    public NavMeshSurface surface;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateNavMesh()
    {
        Debug.Log("Updating NavMesh...");
        surface.BuildNavMesh();
    }
}