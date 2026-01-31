using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Timers;
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

    public bool isDead = false;

    private float burningDuration = 3f;
    private float stunnedDuration = 1f;
    private float frozenDuration = 2f;
    private float electrifiedDuration = 4f;
    private float bleedingDuration = 6f;

    private float timerBurn;
    private float timerStun;
    private float timerFrozen;
    private float timerElec;
    private float timerBleed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEffect.HasFlag(StatusEffect.Burning))
        {
            timerBurn -= Time.deltaTime;
            if (timerBurn < 0)
            {
                currentEffect &= ~StatusEffect.Burning;
            }
            //Debug.Log(timerBurn % 0.5);
            if (timerBurn % 0.5 <= 0.01f)
            {
                Debug.Log("OOF!");
            }
            // burning
        }

        if (currentEffect.HasFlag(StatusEffect.Stunned))
        {
            timerStun -= Time.deltaTime;
            if (timerStun < 0)
            {
                currentEffect &= ~StatusEffect.Stunned;
            }

            // stunned
        }

        if (currentEffect.HasFlag(StatusEffect.Frozen))
        {
            timerFrozen -= Time.deltaTime;
            if (timerFrozen < 0)
            {
                currentEffect &= ~StatusEffect.Frozen;
            }
            // frozen
        }

        if (currentEffect.HasFlag(StatusEffect.Electrified))
        {
            timerElec -= Time.deltaTime;
            if (timerElec < 0)
            {
                currentEffect &= ~StatusEffect.Electrified;
            }
            // electrified
        }

        if (currentEffect.HasFlag(StatusEffect.Bleeding))
        {
            timerBleed -= Time.deltaTime;
            if (timerBleed < 0)
            {
                currentEffect &= ~StatusEffect.Bleeding;
            }
            // bleeding
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

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void AddToCurrentStatus(StatusEffect statusEffect)
    {
        currentEffect = currentEffect | statusEffect;
        switch (statusEffect)
        {
            case StatusEffect.Burning:
                timerBurn = burningDuration;
                break;
            case StatusEffect.Stunned:
                timerStun = stunnedDuration;
                break;
            case StatusEffect.Electrified:
                timerElec = electrifiedDuration;
                break;
            case StatusEffect.Frozen:
                timerFrozen = frozenDuration;
                break;
            case StatusEffect.Bleeding:
                timerBleed = bleedingDuration;
                break;
        }
    }
}
