using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    string targetSpawnName;
    bool isTransitioning;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadSceneWithSpawn(string sceneName, string spawnName)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        targetSpawnName = spawnName;

        SceneManager.LoadScene(sceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScene")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        StartCoroutine(PositionPlayer());
    }

    IEnumerator PositionPlayer()
    {
        yield return null; // wait 1 frame for scene objects

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            isTransitioning = false;
            yield break;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();

        // Freeze physics
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        GameObject spawn = GameObject.Find(targetSpawnName);

        if (spawn != null)
        {
            player.transform.position = spawn.transform.position;
            player.transform.rotation = spawn.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Spawn point not found: " + targetSpawnName);
        }

        yield return new WaitForSeconds(0.1f); // small safety delay

        rb.isKinematic = false;

        // FUTURE CUTSCENE HOOK
        // CutsceneManager.Instance.PlayEntrance(scene.name);

        isTransitioning = false;
    }
}