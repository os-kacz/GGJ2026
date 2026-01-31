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

    public string MaskName;
    public Element ElementType;
    public AbilityFunctions MaskAbility;
    public Sprite MaskIcon;
}
