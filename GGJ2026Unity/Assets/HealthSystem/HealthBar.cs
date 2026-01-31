using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public GameObject healthBar;
    public GameObject previousHealthBar;

    private Slider healthBarSlider;
    private Slider previousHealthBarSlider;

    public GameObject character;
    private HealthComponent healthComponent;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBarSlider = healthBar.GetComponent<Slider>();
        previousHealthBarSlider = previousHealthBar.GetComponent<Slider>();

        healthComponent = character.GetComponent<HealthComponent>();

        healthBarSlider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        healthBarSlider.value = healthComponent.GetHealth() / healthComponent.GetMaxHealth();

     //if (healthComponent.damageDoneLastUpdate != 0)
     //{
     //    previousHealthBarSlider.value = (healthComponent.GetHealth() + healthComponent.damageDoneLastUpdate) / healthComponent.GetMaxHealth();
     //}

    }

    
}
