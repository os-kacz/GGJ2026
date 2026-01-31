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
    public enum AbilityFunctions
    {
        None = 0,
        Teleport = 1,
    }

    [Header("Information")]
    public string MaskName;
    public Sprite MaskIcon;
    public Color UIColour;

     [Header("Unlock Data")]
    public AbilityFunctions MaskAbility;
    public ScriptableObject WeaponUnlocked;

    [Header("Elemental Data")]
    public Element ElementType;
}
