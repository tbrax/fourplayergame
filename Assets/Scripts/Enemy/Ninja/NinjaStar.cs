using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaStar : BaseThrowable
{

    Transform stuckTo;
    int stuckState = 0;
    Vector2 stuckOffset;
    float damage = 1;

    private void Start()
    {
        my_velocity.x = Random.Range(-5f, 5f);
        my_velocity.y = Random.Range(1f, 5f);

    }

    public void setDamage(float f)
    {
        damage = f;
    }

    protected override void doTrigger(Collider2D col)
    {
        if (stuckState == 0)
        {

            if (checkHitPlayer(col))
            {
                stuckState = 1;

                stuckTo = col.transform.parent;
                stuckOffset = transform.position - stuckTo.position;
                dealDamageToPlayer(stuckTo, damage);

                resetVelocity();
                playSound("starhit");
            }
            else if (checkHitFloor(col))
            {
                resetVelocity();
                stuckState = 2;
                playSound("starhit");
            }
        }
    }

    public override void updateMovement(float t)
    {
        if (stuckState == 0)
        {
            base.updateMovement(t);
            transform.Rotate(0, 0, 400 * my_velocity.x * t);
        }
        else if (stuckState == 1)
        {
            transform.position = (Vector2)stuckTo.position + stuckOffset;
        }
    }
}
