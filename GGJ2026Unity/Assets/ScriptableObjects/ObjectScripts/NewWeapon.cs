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

    public string WeaponName;
    public Weapon WeaponType;
    public int Damage;
    public Element ElementType;
    public Sprite WeaponIcon;
}
