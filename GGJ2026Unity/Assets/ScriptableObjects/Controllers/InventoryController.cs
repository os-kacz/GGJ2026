using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class InventoryController : MonoBehaviour
{
    [Header("Information")]
    public GameObject Self;
    public Canvas UI;

    [Header("Masks")]
    public NewMask[] AllMasks;
    public NewMask PlayerMaskSlot1;
    public NewMask PlayerMaskSlot2;
 
    [Header("Weapons")]
    public NewWeapon[] AllWeapons;
    public NewWeapon PlayerWeaponSlot1; //overwrite this if the mask unlocks a weapon


    private List<NewMask> AllPlayerOwnedMasks = new List<NewMask>();
    private List<NewWeapon> AllPlayerOwnedWeapons = new List<NewWeapon>();

    public void Start()
    {
        // INIT IF PLAYER STARTS WITH WEAPONS/ MASKS
        if(PlayerWeaponSlot1){EquipWeapon(PlayerWeaponSlot1);}
        if(PlayerMaskSlot1){EquipMask(PlayerMaskSlot1, 1);}
        if(PlayerWeaponSlot1){EquipMask(PlayerMaskSlot2, 2);}
    }

    private void SetUISlotMask(GameObject MaskSlot, NewMask MaskToEquip)
    {
        UnityEngine.UI.Image Border          = MaskSlot.transform.Find("SlotOuter").gameObject.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image Icon            = MaskSlot.transform.Find("SlotIcon").gameObject.GetComponent<UnityEngine.UI.Image>();
        TextMeshProUGUI Text                 = MaskSlot.transform.Find("AbilityName").gameObject.GetComponent<TextMeshProUGUI>();
        UnityEngine.UI.Image ButtonPrompt    =  MaskSlot.transform.Find("SlotButton").gameObject.GetComponent<UnityEngine.UI.Image>();

        Border.color         = MaskToEquip.UIColour;
        Icon.sprite          = MaskToEquip.MaskIcon;
        Text.text            = MaskToEquip.AbilityName;
        Text.color           = MaskToEquip.UIColour;
        ButtonPrompt.color   = MaskToEquip.UIColour;
    }

    private void SetUISlotWeapon(NewWeapon WeaponToEquip)
    {
        PlayerWeaponSlot1            = WeaponToEquip;   
        GameObject WeaponSlot        = UI.transform.Find("BottomPanel").gameObject.transform.Find("WeaponSlot").gameObject;
        UnityEngine.UI.Image Border  = WeaponSlot.transform.Find("SlotOuter").gameObject.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image Icon    = WeaponSlot.transform.Find("SlotIcon").gameObject.GetComponent<UnityEngine.UI.Image>();
        TextMeshProUGUI Text         = WeaponSlot.transform.Find("WeaponName").gameObject.GetComponent<TextMeshProUGUI>();

        Border.color     = PlayerWeaponSlot1.UIColour;
        Icon.sprite      = PlayerWeaponSlot1.WeaponIcon;
        Text.text        = PlayerWeaponSlot1.WeaponName;   
        Text.color       = PlayerWeaponSlot1.UIColour;
    }

    //PUBLIC METHOD WHEN THE PLAYER CLICKS THE EQUIP BUTTON
    public void EquipMask(NewMask MaskToEquip, int SlotNum)
    {
        if(SlotNum == 1){PlayerMaskSlot1 = MaskToEquip;}
        else if(SlotNum == 2){PlayerMaskSlot2 = MaskToEquip;}

        GameObject MaskSlot = UI.transform.Find("BottomPanel").gameObject.transform.Find("MaskSlot" + SlotNum).gameObject;
        SetUISlotMask(MaskSlot, MaskToEquip);
    }

    //PUBLIC METHOD WHEN THE PLAYER CLICKS THE EQUIP BUTTON
    public void EquipWeapon(NewWeapon WeaponToEquip)
    {
        PlayerWeaponSlot1 = WeaponToEquip;
        SetUISlotWeapon(WeaponToEquip);
    }

    // PUBLIC METHOD WHEN THE PLAYER OBTAINS A MASK THROUGH GAMEPLAY
    public void AddMaskToInventory(string _maskName)
    {
        foreach(NewMask Mask in AllMasks)
        {
            if(Mask.MaskName == _maskName)
            { 
                AllPlayerOwnedMasks.Add(Mask);
                // check if the mask unlocks a new weapon
                if (Mask.WeaponUnlocked) {AllPlayerOwnedWeapons.Add(Mask.WeaponUnlocked);}
                return;
            }
        }
    }
}