using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Timers;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class HealthComponent : MonoBehaviour
{
    public UnityEvent E_EntityHasDied;

    public UnityEvent E_EntityHasBeenDamaged;

    public GameObject DamageNumberPrefab;

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

    [Header("Status Effect Duration")]
    [SerializeField] private float burningDuration = 3f;
    [SerializeField] private float stunnedDuration = 1f;
    [SerializeField] private float frozenDuration = 2f;
    [SerializeField] private float electrifiedDuration = 4f;
    [SerializeField] private float bleedingDuration = 6f;

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

    [Header("Damage Done Last Instance")]
    public float accumulateDamageDone;
    public float damageDoneLastInstance;

    private float damageTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        if (E_EntityHasDied == null)
        {
            E_EntityHasDied = new UnityEvent();
        }

        if (DamageNumberPrefab == null)
        {
            DamageNumberPrefab = GameObject.FindGameObjectWithTag("DamageNumber");
        }
    }

    // Update is called once per frame
    void Update()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer > 1f)
        {
            damageTimer = 0;
            damageDoneLastInstance = accumulateDamageDone;
            accumulateDamageDone = 0;
        }

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
                DecreaseHealthBy(bleedDamage, StatusEffect.Bleeding);
                elapsedBleed = 0;
            }
            // bleeding
        }
    }

    // - particle effects for damage numbers and statuseffect

    public void DecreaseHealthBy(int damageDealt, StatusEffect statusEffect = StatusEffect.None)
    {
        if (!isDead)
        {
            currentHealth -= damageDealt;
            accumulateDamageDone += damageDealt;
            damageTimer = 0f;
            CreateDamageNumber(damageDealt, statusEffect);
            E_EntityHasBeenDamaged.Invoke();
            if (currentHealth < 0)
            {
                E_EntityHasDied.Invoke();
            }
            if (damageDealt > 0)
            {
                E_EntityHasBeenDamaged.Invoke();
            }
        }
    }

    private void CreateDamageNumber(float damage, StatusEffect statusEffect)
    {
        var textColour = Color.white;
        if (DamageNumberPrefab != null)
        {
            switch (statusEffect)
            {
                case StatusEffect.Burning:
                    textColour = Color.orangeRed;
                    break;
                case StatusEffect.Electrified:
                    textColour = Color.aliceBlue;
                    break;
                case StatusEffect.Bleeding:
                    textColour = Color.darkRed;
                    break;
            }
            var go = Instantiate(DamageNumberPrefab, transform.position, Quaternion.identity, transform);
            go.GetComponent<TextMeshPro>().text = damage.ToString();
            go.GetComponent<TextMeshPro>().color = textColour;
        }
        else
        {
            Debug.Log("No damage number hooked up in prefab slot");
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
