using Unity.VisualScripting;
using UnityEngine;

public class AbilityController : MonoBehaviour
{

    //references to all the masks in the game
    [Header("Masks")]
    public ScriptableObject[] AllMasks;

    [Header("Weapons")]
    public ScriptableObject[] AllWeapons;

    [Header("Boss Only")]
    public ScriptableObject[] ActiveMasks;

    // enums
    public enum Ability
    {
        None = 0,
        Slash = 1,
        Slam = 2,
        Teleport = 3,
    }

    // Private attributes
    private ScriptableObject PlayerMaskSlot1;
    private ScriptableObject PlayerMaskSlot2;

    // player only
    public void CollectMask(string _maskName, int _slotNumber)
    {
        foreach(NewMask Mask in AllMasks)
        {
            if(Mask.MaskName == _maskName)
            {
                // assign to correct slot (presumable an input from a ui choice)
                if(_slotNumber == 1){PlayerMaskSlot1 = Mask;}
                else{PlayerMaskSlot2 = Mask;}

                return;
            }
        }
    }

    public void TriggerAbility1()
    {
        if(!PlayerMaskSlot1){return;} // some kind of feedback that there is no ability?
    }

    public void TriggerAbility2()
    {
        if(!PlayerMaskSlot2){return;} // some kind of feedback that there is no ability?
    }


    // ability functions 
    private void Teleport()
    {
        Debug.Log("TELEPORT ABILITY");
    }

    private void Slam()
    {
        Debug.Log("SLAM ABILITY");
    }

}
