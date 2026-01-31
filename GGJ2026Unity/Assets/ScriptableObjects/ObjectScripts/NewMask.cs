using System;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMask", menuName = "Create new items/Mask")]
public class NewMask : ScriptableObject
{
    public enum Element
    {
        None = 0,
        Fire = 1,
        Ice = 2,
        Electric = 3,
    }

    public enum Ability
    {
        None = 0,
        Slam = 1,
        Blast = 2,
        Inferno = 3,
        Blizzard = 4,
        Teleport = 5,
    }

    public enum AnimationState
    {
        None = 0,
        Slam = 1,
    }

    [Header("Information")]
    public string MaskName;
    public Sprite MaskIcon;
    public Color UIColour;
    public AnimationState PlayAnimation;


    [Header("Ability Data")]
    public Ability AbilityID;
    public string AbilityName;
    [Tooltip("Damage per second")]
    public int AbilityDamage; 
    public int AbilityKnockback;
    public int Duration;
    public int Cooldown;
    public Element ElementType;
    public HealthComponent.StatusEffect[] Debuffs;

    [Header("Hitbox Data")]
    public float HitboxHeight;
    public float HitboxWidth;
    public Vector2 HitboxSpawnOffset;
    public AnimatorController HitboxVfx;

    [Header("Unlock Data")]
    public NewWeapon WeaponUnlocked;
}
