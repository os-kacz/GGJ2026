using UnityEngine;
using UnityEngine.UI;

public class ElecIcon : MonoBehaviour
{
    private GameObject player;
    private HealthComponent healthComponent;
    private Image icon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthComponent = player.GetComponent<HealthComponent>();
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthComponent.currentEffect.HasFlag(HealthComponent.StatusEffect.Electrified))
        {
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }
    }
}
