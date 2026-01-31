using Unity.VisualScripting;
using UnityEngine;

// TODO: handle cooldowns for mask abilities

public class AbilityController : MonoBehaviour
{
    public GameObject Self; 

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

                // check if the mask unlocks a new weapon
                if (Mask.WeaponUnlocked){PlayerWeaponSlot1 = Mask.WeaponUnlocked;}

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

    public void TriggerAbility1()
    {
        if(!PlayerMaskSlot1){return;} // some kind of feedback that there is no ability?
        RunMaskAbility(PlayerMaskSlot1);

    }

    public void TriggerAbility2()
    {
        if(!PlayerMaskSlot2){return;} // some kind of feedback that there is no ability?
        RunMaskAbility(PlayerMaskSlot2);

    }

    private void CreateHitbox(int Height, int Width, Vector2 Offset)
    {
        Vector2 SpritePos = new Vector2(Self.transform.position.x, Self.transform.position.y);
        Vector2 HitboxSpawn = new Vector2(SpritePos.x + Offset.x, SpritePos.y + Offset.y);

        // return an array of other things colliding (filter out self)
        Collider2D[] CharactersHit = Physics2D.OverlapCapsuleAll(HitboxSpawn, new Vector2(Height, Width), CapsuleDirection2D.Horizontal, 0f); // sprite position + offset
        Debug.Log(CharactersHit);
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
        CreateHitbox(Mask.HitboxHeight, Mask.HitboxWidth, Mask.HitboxSpawnOffset);
        // handle cooldowns
        Debug.Log(Mask.AbilityName);
    }

    private void Blast(NewMask Mask)
    {
        Debug.Log(Mask.AbilityName);
    }

    private void Inferno(NewMask Mask)
    {
        Debug.Log(Mask.AbilityName);
    }
    

    private void Blizzard(NewMask Mask)
    {
        Debug.Log(Mask.AbilityName);
    }
    
    
    private void Teleport(NewMask Mask)
    {
        Debug.Log(Mask.AbilityName);
    }


}