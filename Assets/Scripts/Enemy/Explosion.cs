using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : BaseThrowable
{


    float damage;
    float delay = 0;
    
    float explodeRadius = 1;
    bool has_dealt_damage = false;



    private void Start()
    {
        lifeTimerMax = 0.5f;
        has_dealt_damage = false;
    }
    public void setDamage(float t)
    {
        damage = t;
    }

    void dealDamage()
    {
        has_dealt_damage = true;
        List<PlayerMove> ps = getPlayerInAreaCircle(transform.position, explodeRadius);
        
        foreach(PlayerMove p in ps)
        {
            p.takeDamage(damage);
        }

    }

    void clockTick()
    {

    }


    public override void updateMovement(float t)
    {
        
        clockTick();
        if (lifeTimer > delay)
        {
            if (!has_dealt_damage)
            {
                dealDamage();
                has_dealt_damage = true;
            }
        }
    }
}
