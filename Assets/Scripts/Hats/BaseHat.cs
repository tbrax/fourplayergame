using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHat : BaseThrowable
{
    int wearState;
    PlayerMove myPlayer;
    int stuckState;

    int hatType;
    const float maxLife = 60;

    const int wear_state_wearing = 1;
    float ammo;
    float ammoUsage;


    private void Start()
    {
        wearState = 0;
        stuckState = 0;
        hatType = 1;
        lifeTimerMax = maxLife;
        ammo = 1;
        setupHatType(hatType);
    }

    void setupHatType(int i)
    {

        switch (i)
        {
            case 1:
                setAmmoUseTimes(5);
                break;

            default:
                setAmmoUseTimes(5);
                break;
        }
    }

    void setAmmoUseTimes(float f)
    {
        ammoUsage = 1 / f;
    }


    public int getHatType()
    {
        return hatType;
    }

    public virtual void useAbility(PlayerMove p)
    {
        myPlayer.useAbility(hatType);
        useAmmo();
    }

    void useAmmo()
    {
        ammo -= ammoUsage;
        if (ammo <= 0)
        {
            ammo = 0;
            myDeath();
        }
    }

    public void wear(PlayerMove p)
    {
        myPlayer = p;
        wearState = wear_state_wearing;
        stuckState = 0;
        lifeTimer = 0;
        is_dying = false;
    }

    public void drop()
    {
        wearState = 0;
        lifeTimer = 0;
        is_dying = true;
    }

    void takeOffHat()
    {
        if (myPlayer != null && wearState == wear_state_wearing)
        {
            myPlayer.takeOffHat(this);
        }
    }

    void removeHats()
    {
        takeOffHat();
        entity_parent.removeHat(this);
    }

    public override void myDeath()
    {
        removeHats();
        base.myDeath();
    }


    


    public override void updateMovement(float t)
    {
        if (wearState == 0)
        {
            if (stuckState == 0)
            {
                base.updateMovement(t);
            }
        }
    }
}
