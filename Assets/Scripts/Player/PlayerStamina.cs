using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public static PlayerStamina Instance;

    public int maxStamina = 100;
    public float currentStamina;

    [Header("Regen Settings")]
    public int regenRatePerSecond = 25;
    public float regenDelay = 1.2f;

    float lastUseTime;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        // Regenerate stamina if enough time passed
        if (Time.time > lastUseTime + regenDelay && currentStamina < maxStamina)
        {
            currentStamina += regenRatePerSecond * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    public bool HasStamina(int amount)
    {
        return currentStamina >= amount;
    }

    public void UseStamina(int amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Max(currentStamina, 0);
        lastUseTime = Time.time;
    }
}