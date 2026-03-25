using UnityEngine;

public class MetaUpgradeInteract : MonoBehaviour
{
    public GameObject interactPrompt;

    void Start()
    {
        interactPrompt.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (interactPrompt.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Meta Upgrade Collected");

            // Example: Always give health upgrade for now
            GameManager.Instance.ApplyMetaUpgrade("Health");

            // Later add a transition sequence between this and the first arena
            RunManager.Instance.MetaUpgradeCollected();
        }
    }
}