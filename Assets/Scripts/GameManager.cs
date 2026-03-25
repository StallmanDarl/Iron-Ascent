using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Permanent upgrades
    public int permanentHealthLevel = 0;
    public int permanentDamageLevel = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists forever
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when player earns meta upgrade
    public void ApplyMetaUpgrade(string upgradeType)
    {
        if (upgradeType == "Health")
            permanentHealthLevel++;

        if (upgradeType == "Damage")
            permanentDamageLevel++;

        Debug.Log("Permanent Upgrade Applied: " + upgradeType);
    }
}