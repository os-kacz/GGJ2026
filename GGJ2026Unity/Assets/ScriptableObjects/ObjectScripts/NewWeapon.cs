using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Create new items/Weapon")]
public class NewWeapon : ScriptableObject
{
    public enum Weapon
    {
        Melee = 0,
        Ranged = 1,
    }

    public enum Element
    {
        None = 0,
        Fire = 1,
        Ice = 2,
        Electric = 3,
    }

    [Header("Weapon Information")]
    public string WeaponName;
    public Weapon WeaponType;
    public Sprite WeaponIcon;
    public Color UIColour;

    [Header("Weapon Values")]
    public int Damage;
    public int Knockback;
    public int AttackSpeed;

    [Header("Elemental Data")]
    public Element ElementType;
}
