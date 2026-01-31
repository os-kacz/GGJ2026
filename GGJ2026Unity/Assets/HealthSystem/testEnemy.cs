using UnityEngine;

public class testEnemy : MonoBehaviour
{
    HealthComponent healthComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthComponent = GetComponent<HealthComponent>();

        healthComponent.AddToCurrentStatus(HealthComponent.StatusEffect.Burning);
        healthComponent.AddToCurrentStatus(HealthComponent.StatusEffect.Electrified);

        healthComponent.E_EntityHasDied.AddListener(PoopDoer);
    }

    // Update is called once per frame
    void Update()
    {
        healthComponent.DecreaseHealthBy(50);
    }

    void PoopDoer()
    {
        Debug.Log("Ementy has dieded");
    }
}
