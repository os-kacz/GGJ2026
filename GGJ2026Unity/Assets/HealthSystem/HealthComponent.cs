using System;
using UnityEditor.Rendering;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Flags]
    public enum StatusEffect
    {
        None = 0,
        Burning = 1,
        Stunned = 2,
        Frozen = 4,
        Electrified = 8,
        Bleeding = 16,
    }

    [SerializeField] private int maxHealth;

    [SerializeField] private int currentHealth;

    [SerializeField] private StatusEffect currentEffect = new();

    private bool isDead = false;

    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(currentEffect);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentEffect.HasFlag(StatusEffect.Burning))
        {
            // burning
            Debug.Log("Burning");
        }

        if (currentEffect.HasFlag(StatusEffect.Stunned))
        {
            // stunned
            Debug.Log("Stunned");
        }

        if (currentEffect.HasFlag(StatusEffect.Frozen))
        {
            // frozen
            Debug.Log("Frozen");
        }

        if (currentEffect.HasFlag(StatusEffect.Electrified))
        {
            // electrified
            Debug.Log("Electrified");
        }

        if (currentEffect.HasFlag(StatusEffect.Bleeding))
        {
            // bleeding
            Debug.Log("Bleeding");
        }
    }

    public void DecreaseHealth(int damageDealt)
    {
        currentHealth -= damageDealt;
        if (currentHealth < 0)
        {
            isDead = true;
        }
    }

    public void AddToCurrentStatus(StatusEffect statusEffect)
    {
        currentEffect = statusEffect;
    }
}
