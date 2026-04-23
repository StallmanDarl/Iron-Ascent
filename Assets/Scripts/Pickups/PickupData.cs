using UnityEngine;

public enum PickupType
{
    Health,
    Stamina,
    Damage,
    Speed
}

[CreateAssetMenu(fileName = "NewPickup", menuName = "Pickups/Temporary Pickup")]
public class PickupData : ScriptableObject
{
    public string pickupName;
    public PickupType type;

    [Header("Effect Values")]
    public float amount;      // positive or negative
    public float duration;    // seconds

    [Header("Visuals")]
    public Sprite icon;
    public GameObject pickupPrefab;
}