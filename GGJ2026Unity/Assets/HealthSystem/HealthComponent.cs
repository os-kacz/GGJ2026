using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Timers;
using Unity.VisualScripting;
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

    [SerializeField] private float maxHealth;

    [SerializeField] private float currentHealth;

    [SerializeField] public StatusEffect currentEffect = new();

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

    [Header("Status Effect Tick Rate")]
    [SerializeField] private float burningInterval = 0.5f;
    [SerializeField] private float electrifiedInterval = 0.2f;
    [SerializeField] private float bleedingInterval = 1f;

    private float elapsedBurn;
    private float elapsedElec;
    private float elapsedBleed;

    [Header("Status Effect Damage")]
    [SerializeField] private int burnDamage = 3;
    [SerializeField] private int electrifiedDamage = 1;
    [SerializeField] private int bleedDamage = 5;

    private float accumulateDamageDone;

    [Header("Damage Done Last Frame")]
    public float damageDoneLastUpdate;

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
            elapsedBurn += Time.deltaTime;
            if (timerBurn < 0)
            {
                currentEffect &= ~StatusEffect.Burning;
            }
            if (elapsedBurn > burningInterval)
            {
                DecreaseHealthBy(burnDamage);
                elapsedBurn = 0;
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
            elapsedElec += Time.deltaTime;
            if (timerElec < 0)
            {
                currentEffect &= ~StatusEffect.Electrified;
            }
            if (elapsedElec > electrifiedInterval)
            {
                DecreaseHealthBy(electrifiedDamage);
                elapsedElec = 0;
            }
            // electrified
        }

        if (currentEffect.HasFlag(StatusEffect.Bleeding))
        {
            timerBleed -= Time.deltaTime;
            elapsedBleed += Time.deltaTime;
            if (timerBleed < 0)
            {
                currentEffect &= ~StatusEffect.Bleeding;
            }
            if (elapsedBleed > bleedingInterval)
            {
                DecreaseHealthBy(bleedDamage);
                elapsedBleed = 0;
            }
            // bleeding
        }
    }

    private void LateUpdate()
    {
        accumulateDamageDone = damageDoneLastUpdate;
    }

    // todo - calculate white bar damage taken since last frame 0.5f and on damage refresh
    // - particle effects for damage numbers and statuseffect

    public void DecreaseHealthBy(int damageDealt)
    {
        currentHealth -= damageDealt;
        accumulateDamageDone += damageDealt;
        if (currentHealth < 0)
        {
            isDead = true;
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
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
