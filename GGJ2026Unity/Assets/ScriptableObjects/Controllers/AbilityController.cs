using System.ComponentModel;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

// TODO: handle cooldowns for mask abilities

public class AbilityController : MonoBehaviour
{
    public GameObject Self; 
    public GameObject Hitbox;
    public Canvas UI;

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

    private GameObject[] CreateHitbox(float Height, float Width, Vector2 Offset, AnimatorController HitboxVFX)
    {
        //VISUALS
        float velocity = Self.GetComponent<Rigidbody2D>().linearVelocityX;
        Debug.Log(velocity);
        int direction = 1;
        if(velocity < 0)
        {
            direction = -1;
        }

        Vector2 SpritePos = new Vector2(Self.transform.position.x * direction, Self.transform.position.y * direction);
        Vector2 HitboxSpawn = new Vector2(SpritePos.x + Offset.x, SpritePos.y + Offset.y);

        GameObject NewHitbox = Instantiate(Hitbox, new Vector3(HitboxSpawn.x, HitboxSpawn.y, 0), Quaternion.identity);
        NewHitbox.transform.localScale = new Vector3(Width, Height, 1);
        NewHitbox.GetComponent<Animator>().runtimeAnimatorController = HitboxVFX;


        if(direction == -1)
        {
            NewHitbox.GetComponent<SpriteRenderer>().flipX = true;
        }
    
        // return an array of other things colliding (filter out self)
        Collider2D[] Colliders = Physics2D.OverlapCapsuleAll(HitboxSpawn, new Vector2(Width, Height), CapsuleDirection2D.Horizontal, 0f); // todo rotation value

        GameObject[] CharactersHit = {};
        foreach(Collider2D Box in Colliders)
        {
            if(Box.gameObject.layer != Self.layer && Box.gameObject.layer != 1 && Box.gameObject.layer != 2 && Box.gameObject.layer != 3 && Box.gameObject.layer != 4 && Box.gameObject.layer != 5 && Box.gameObject.layer != 6)
            {
                CharactersHit.Append(Box.gameObject);
            }
        }

        return CharactersHit;
    }

    private void HandleMaskDamage(NewMask Mask)
    {
        GameObject[] CharactersHit = CreateHitbox(Mask.HitboxHeight, Mask.HitboxWidth, Mask.HitboxSpawnOffset, Mask.HitboxVfx);

        foreach(GameObject Character in CharactersHit)
        {
            // GET THE HEALTH COMPONENT AND DEAL DAMAGE AND APPLY STATUS EFFECTS
            HealthComponent CharacterHealth = Character.GetComponent<HealthComponent>();
            CharacterHealth.DecreaseHealthBy(Mask.AbilityDamage);

            // add any debuffs the ability will inflict
            foreach(HealthComponent.StatusEffect Debuff in Mask.Debuffs){CharacterHealth.AddToCurrentStatus(Debuff); }
        }
    }

    private void BasicAttack(NewWeapon WeaponToUse)
    {
        if(WeaponToUse.WeaponType == NewWeapon.Weapon.Melee)
        {
            // MELEE ATTACK
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

        Self.transform.position = new Vector3(Self.transform.position.x + 5, Self.transform.position.y + 2, Self.transform.position.z);
        Debug.Log(Mask.AbilityName);
    }
}

//TODO: Hitbox destroy after vfx animation is complete, vfx can flip correctly, can get the direction the player is facing not reliant on the velocity