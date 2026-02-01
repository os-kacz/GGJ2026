using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
    public AbilityController abilityController;
    private void Start()
    {
        abilityController.IntersectingColliders.Clear();
    }
    private void OnTriggerEnter2D(Collider2D otherPart)
    {
        abilityController.IntersectingColliders.Add(otherPart.gameObject);
    }

    private void OnDestroy()
    {
        abilityController = null;
    }
}
