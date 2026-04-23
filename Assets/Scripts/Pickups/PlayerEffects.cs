using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEffects : MonoBehaviour
{
    public static PlayerEffects Instance;

    PlayerMovement movement;
    PlayerAttackTester attack;
    PlayerHealth health;
    PlayerStamina stamina;

    Dictionary<PickupType, Coroutine> activeEffects = new Dictionary<PickupType, Coroutine>();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttackTester>();
        health = GetComponent<PlayerHealth>();
        stamina = GetComponent<PlayerStamina>();
    }

    public void ApplyEffect(PickupData data)
    {
        Debug.Log("ApplyEffect called: " + data.type);

        if (data.duration <= 0)
        {
            Apply(data.amount, data.type);
            return;
        }

        if (activeEffects.ContainsKey(data.type))
        {
            StopCoroutine(activeEffects[data.type]);
        }

        activeEffects[data.type] = StartCoroutine(HandleEffect(data));
    }

    IEnumerator HandleEffect(PickupData data)
    {
        Debug.Log("Effect START");

        Apply(data.amount, data.type);

        yield return new WaitForSeconds(data.duration);

        Debug.Log("Effect END");

        Apply(-data.amount, data.type);

        activeEffects.Remove(data.type);
    }

    void Apply(float amount, PickupType type)
    {
        switch (type)
        {
            case PickupType.Health:
                if (health != null)
                {
                    health.currentHealth += (int)amount;
                    health.currentHealth = Mathf.Clamp(
                        health.currentHealth,
                        0,
                        health.maxHealth
                    );
                }
                break;

            case PickupType.Stamina:
                if (stamina != null)
                {
                    stamina.currentStamina += amount;
                    stamina.currentStamina = Mathf.Clamp(
                        stamina.currentStamina,
                        0,
                        stamina.maxStamina
                    );
                }
                break;

            case PickupType.Damage:
                if (attack != null)
                {
                    attack.damage += (int)amount;
                }
                break;

            case PickupType.Speed:
                if (movement != null)
                {
                    movement.moveSpeed += amount;
                    movement.sprintSpeed += amount;
                }
                break;
        }
    }
}