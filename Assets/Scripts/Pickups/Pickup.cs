using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupData data;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(PlayerEffects.Instance);
        if (!other.CompareTag("Player")) return;

        ApplyPickup();
        Destroy(gameObject);
    }

    void ApplyPickup()
    {
        if (data == null)
        {
            Debug.LogWarning("Pickup has no data assigned!");
            return;
        }
    
        if (PlayerEffects.Instance != null)
        {
            Debug.Log("Calling PlayerEffects");
            PlayerEffects.Instance.ApplyEffect(data);
        }
    }
}