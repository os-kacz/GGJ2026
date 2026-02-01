using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// TODO: handle cooldowns for mask abilities

public class AbilityController : MonoBehaviour
{
    public GameObject Self; 
    public GameObject Hitbox;
    public Canvas UI;

    public List<GameObject> IntersectingColliders = new List<GameObject>();

    //references to all the masks in the game
    [Header("Masks")]
    public NewMask[] AllMasks;

    [Header("Weapons")]
    public NewWeapon[] AllWeapons;
    public NewWeapon PlayerWeaponSlot1; //overwrite this if the mask unlocks a weapon

    [Header("Boss Only")]
    public NewMask[] UsableMasks;
    public NewWeapon EnemyWeapon;


    // Private attributes
    private NewMask PlayerMaskSlot1;
    private NewMask PlayerMaskSlot2;

    public void Start()
    {
        if(PlayerWeaponSlot1)
        {
            GameObject WeaponSlot = UI.transform.Find("BottomPanel").gameObject.transform.Find("WeaponSlot").gameObject;
            UnityEngine.UI.Image Border = WeaponSlot.transform.Find("SlotOuter").gameObject.GetComponent<UnityEngine.UI.Image>();
            UnityEngine.UI.Image Icon = WeaponSlot.transform.Find("SlotIcon").gameObject.GetComponent<UnityEngine.UI.Image>();
            TextMeshProUGUI Text = WeaponSlot.transform.Find("WeaponName").gameObject.GetComponent<TextMeshProUGUI>();

            Border.color = PlayerWeaponSlot1.UIColour;
            Icon.sprite = PlayerWeaponSlot1.WeaponIcon;
            Text.text = PlayerWeaponSlot1.WeaponName;
        }
    }

    // player only
    public void CollectMask(string _maskName, int _slotNumber)
    {
        foreach(NewMask Mask in AllMasks)
        {
            if(Mask.MaskName == _maskName)
            {
                // assign to correct slot (presumable an input from a ui choice)
                if(_slotNumber == 1)
                {
                    PlayerMaskSlot1 = Mask;

                    GameObject MaskSlot = UI.transform.Find("BottomPanel").gameObject.transform.Find("MaskSlot1").gameObject;
                    UnityEngine.UI.Image Border = MaskSlot.transform.Find("SlotOuter").gameObject.GetComponent<UnityEngine.UI.Image>();
                    UnityEngine.UI.Image Icon = MaskSlot.transform.Find("SlotIcon").gameObject.GetComponent<UnityEngine.UI.Image>();
                    TextMeshProUGUI Text = MaskSlot.transform.Find("AbilityName").gameObject.GetComponent<TextMeshProUGUI>();
                    UnityEngine.UI.Image ButtonPrompt =  MaskSlot.transform.Find("SlotButton").gameObject.GetComponent<UnityEngine.UI.Image>();

                    Border.color = Mask.UIColour;
                    Icon.sprite = Mask.MaskIcon;
                    Text.text = Mask.AbilityName;
                    ButtonPrompt.color = Mask.UIColour;
                }   

                else
                {
                    PlayerMaskSlot2 = Mask;
                    GameObject MaskSlot = UI.transform.Find("BottomPanel").gameObject.transform.Find("MaskSlot2").gameObject;
                    UnityEngine.UI.Image Border = MaskSlot.transform.Find("SlotOuter").gameObject.GetComponent<UnityEngine.UI.Image>();
                    UnityEngine.UI.Image Icon = MaskSlot.transform.Find("SlotIcon").gameObject.GetComponent<UnityEngine.UI.Image>();
                    TextMeshProUGUI Text = MaskSlot.transform.Find("AbilityName").gameObject.GetComponent<TextMeshProUGUI>();
                    UnityEngine.UI.Image ButtonPrompt =  MaskSlot.transform.Find("SlotButton").gameObject.GetComponent<UnityEngine.UI.Image>();


                    Border.color = Mask.UIColour;
                    Icon.sprite = Mask.MaskIcon;
                    Text.text = Mask.AbilityName;
                    ButtonPrompt.color = Mask.UIColour;

                }

                // check if the mask unlocks a new weapon
                if (Mask.WeaponUnlocked)
                {
                    PlayerWeaponSlot1 = Mask.WeaponUnlocked;

                    GameObject WeaponSlot = UI.transform.Find("BottomPanel").gameObject.transform.Find("WeaponSlot").gameObject;
                    UnityEngine.UI.Image Border = WeaponSlot.transform.Find("SlotOuter").gameObject.GetComponent<UnityEngine.UI.Image>();
                    UnityEngine.UI.Image Icon = WeaponSlot.transform.Find("SlotIcon").gameObject.GetComponent<UnityEngine.UI.Image>();
                    TextMeshProUGUI Text = WeaponSlot.transform.Find("WeaponName").gameObject.GetComponent<TextMeshProUGUI>();

                    Border.color = PlayerWeaponSlot1.UIColour;
                    Icon.sprite = PlayerWeaponSlot1.WeaponIcon;
                    Text.text = PlayerWeaponSlot1.WeaponName;
                }

                return;
            }
        }
    }

    private void RunMaskAbility(NewMask Mask)
    {
        switch (Mask.AbilityID)
        {
            case NewMask.Ability.Slam:
            Slam(Mask);
            break;

            case NewMask.Ability.Blast:
            Blast(Mask);
            break;

            case NewMask.Ability.Inferno:
            Inferno(Mask);
            break;

            case NewMask.Ability.Blizzard:
            Blizzard(Mask);
            break;

            case NewMask.Ability.Teleport:
            Teleport(Mask);
            break;
        }
    }

    public void PlayerAttack()
    {
       BasicAttack(PlayerWeaponSlot1);
    }

    public void EnemyAttack()
    {
        BasicAttack(EnemyWeapon);
    }

    public void EnemyUseMask()
    {
        RunMaskAbility(UsableMasks[0]); // TEMP FOR TESTING
        //cycle through the masks the enemy can use, if they are not on cooldown then add them to available masks and pick a random one to use from that set
    }

    public NewMask.AnimationState TriggerAbility1()
    {
        if(!PlayerMaskSlot1){return NewMask.AnimationState.None;} // some kind of feedback that there is no ability?
        RunMaskAbility(PlayerMaskSlot1);

        return PlayerMaskSlot1.PlayAnimation;
    }

    public NewMask.AnimationState TriggerAbility2()
    {
        if(!PlayerMaskSlot2){return NewMask.AnimationState.None;} // some kind of feedback that there is no ability?
        RunMaskAbility(PlayerMaskSlot2);

        return PlayerMaskSlot2.PlayAnimation;

    }

    IEnumerator Delay(float Lifetime)
    {
        yield return new WaitForSeconds(Lifetime);
    }

    private  void CreateHitbox(float Height, float Width, Vector2 Offset, AnimatorController HitboxVFX)
    {
       
    }

    private void HandleMaskDamage(NewMask Mask)
    {
        int direction = 1;
        if(Self.GetComponent<SpriteRenderer>().flipX){direction = -1;}

        Vector2 SpritePos = new Vector2(Self.transform.Find("AttackPosition").gameObject.transform.position.x, Self.transform.Find("AttackPosition").gameObject.transform.position.y);
        Vector2 HitboxSpawn = new Vector2(SpritePos.x + Mask.HitboxSpawnOffset.x * direction, SpritePos.y + Mask.HitboxSpawnOffset.y);

        //VISUAL ONLY
        GameObject NewHitbox = Instantiate(Hitbox, new Vector3(HitboxSpawn.x, HitboxSpawn.y, 0), Quaternion.identity);
        NewHitbox.transform.localScale = new Vector3(Mask.HitboxWidth, Mask.HitboxHeight, 1);
        NewHitbox.GetComponent<Animator>().runtimeAnimatorController = Mask.HitboxVfx;
        if(direction == -1){NewHitbox.GetComponent<SpriteRenderer>().flipX = true;}

        //SETS UP HITBOX COLLIDER
        BoxCollider2D HitboxOverlap = NewHitbox.GetComponent<BoxCollider2D>();
        HitboxOverlap.size = new Vector2(Mask.HitboxWidth, Mask.HitboxHeight);
        NewHitbox.GetComponent<HitboxTrigger>().abilityController = this;

        float HitboxLifetime = NewHitbox.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        Destroy(NewHitbox, HitboxLifetime); 

        StartCoroutine(Delay(HitboxLifetime));

        foreach(GameObject Character in IntersectingColliders)
        {
            if (Character)
            {
                if(Character.layer != Self.layer)
                {
                    Debug.Log("DEAL DAMAGE TO " + Character.name);
                    HealthComponent CharacterHealth = Character.GetComponent<HealthComponent>();
                    CharacterHealth.DecreaseHealthBy(Mask.AbilityDamage, HealthComponent.StatusEffect.None);

                    // add any debuffs the ability will inflict
                    foreach(HealthComponent.StatusEffect Debuff in Mask.Debuffs){CharacterHealth.AddToCurrentStatus(Debuff); }
                }
            }
        }
    }

    private void HandleWeaponDamage(NewWeapon weapon)
    {
        int direction = 1;
        if(Self.GetComponent<SpriteRenderer>().flipX){direction = -1;}

        Vector2 SpritePos = new Vector2(Self.transform.position.x, Self.transform.position.y);
        Vector2 HitboxSpawn = new Vector2(SpritePos.x + 0.1f * direction, SpritePos.y);

        //VISUAL ONLY
        GameObject NewHitbox = Instantiate(Hitbox, new Vector3(HitboxSpawn.x, HitboxSpawn.y, 0), Quaternion.identity);
        NewHitbox.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        // NewHitbox.GetComponent<Animator>().runtimeAnimatorController = Mask.HitboxVfx;
        // if(direction == -1){NewHitbox.GetComponent<SpriteRenderer>().flipX = true;}

        //SETS UP HITBOX COLLIDER
        BoxCollider2D HitboxOverlap = NewHitbox.GetComponent<BoxCollider2D>();
        HitboxOverlap.size = new Vector2(1.5f, 1.5f);
        NewHitbox.GetComponent<HitboxTrigger>().abilityController = this;

        float HitboxLifetime = NewHitbox.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        Destroy(NewHitbox, HitboxLifetime); 

        StartCoroutine(Delay(HitboxLifetime));

        foreach(GameObject Character in IntersectingColliders)
        {
            if (Character)
            {
                if(Character.layer != Self.layer)
                {
                    Debug.Log("DEAL DAMAGE TO " + Character.name);
                    HealthComponent CharacterHealth = Character.GetComponent<HealthComponent>();
                    CharacterHealth.DecreaseHealthBy(weapon.Damage, HealthComponent.StatusEffect.None);

                    // add any debuffs the ability will inflict
                    // foreach(HealthComponent.StatusEffect Debuff in Mask.Debuffs){CharacterHealth.AddToCurrentStatus(Debuff); }
                }
            }
        }
    }

    private void BasicAttack(NewWeapon WeaponToUse)
    {
        if(WeaponToUse.WeaponType == NewWeapon.Weapon.Melee)
        {
            // MELEE ATTACK
            BasicAttack(PlayerWeaponSlot1);
        }
        else if(WeaponToUse.WeaponType == NewWeapon.Weapon.Ranged)
        {
            // RANGED PROJECTILE
        }
    }

    // ability functions 
    private void Slam(NewMask Mask)
    {
        HandleMaskDamage(Mask);
        // handle cooldowns
        Debug.Log(Mask.AbilityName);
    }

    private void Blast(NewMask Mask)
    {
        HandleMaskDamage(Mask);
        Debug.Log(Mask.AbilityName);
    }

    private void Inferno(NewMask Mask)
    {
        HandleMaskDamage(Mask);
        Debug.Log(Mask.AbilityName);
    }
    

    private void Blizzard(NewMask Mask)
    {
        HandleMaskDamage(Mask);
        Debug.Log(Mask.AbilityName);
    }
    
    
    private void Teleport(NewMask Mask)
    {
        HandleMaskDamage(Mask);

        int direction = 1;
        if(Self.GetComponent<SpriteRenderer>().flipX){direction = -1;}

        Self.transform.position = new Vector3(Self.transform.position.x + 5 * direction, Self.transform.position.y + 2, Self.transform.position.z);
        Debug.Log(Mask.AbilityName);
    }
}

//TODO: Hitbox destroy after vfx animation is complete, vfx can flip correctly, can get the direction the player is facing not reliant on the velocity