using UnityEngine;

public class ArenaDoor : MonoBehaviour
{
    public GameObject doorPrompt;
    public string nextArenaName;
    public string spawnPointName;
    public bool unlocked = false;

    void Start()
    {
        doorPrompt.SetActive(false);
    }

    public void Unlock()
    {
        unlocked = true;
        Debug.Log("Door unlocked!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && unlocked)
        {
            doorPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (!unlocked) return;

        if (doorPrompt.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            RunManager.Instance.LoadNextArena(nextArenaName);
        }
    }
}
