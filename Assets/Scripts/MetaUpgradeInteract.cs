using UnityEngine;

public class MetaUpgradeInteract : MonoBehaviour
{
    public GameObject interactPrompt;

    void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactPrompt != null && UpgradeManager.Instance != null && !UpgradeManager.Instance.IsShowingUpgradeCards())
        {
            interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (interactPrompt == null)
        {
            return;
        }

        if (UpgradeManager.Instance != null && UpgradeManager.Instance.IsShowingUpgradeCards())
        {
            interactPrompt.SetActive(false);
            return;
        }

        if (interactPrompt.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            UpgradeManager.EnsureExists();
            UpgradeManager.Instance.ShowMetaUpgradeChoices();
        }
    }
}
