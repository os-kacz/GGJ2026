using System;
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

    public enum StatusEffect
    {
        None = 0,
        Burning = 1,
        Stun = 2,
        Frozen = 4,
        Electrified = 8,
        Bleeding = 16
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

    [Header("Information")]
    public string MaskName;
    public Sprite MaskIcon;
    public Color UIColour;


    [Header("Ability Data")]
    public Ability AbilityID;
    public string AbilityName;
    [Tooltip("Damage per second")]
    public int AbilityDamage; 
    public int AbilityKnockback;
    public int Duration;
    public int Cooldown;
    public Element ElementType;

    [Header("Hitbox Data")]
    public int HitboxHeight;
    public int HitboxWidth;
    public Vector2 HitboxSpawnOffset;

    [Header("Unlock Data")]
    public NewWeapon WeaponUnlocked;
}
