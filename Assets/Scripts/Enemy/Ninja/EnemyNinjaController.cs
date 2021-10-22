using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNinjaController : GenericEnemyController
{
    float spinTimer = 0;
    float spinTimerLimit = 2;

    float spinTimerLimitMin = 2;
    float spinTimerLimitMax = 6;
    int state = 0;



    void Start()
    {
        
    }


    void setLimit()
    {
        spinTimerLimit = Random.Range(spinTimerLimitMin, spinTimerLimitMax);
    }

    void toggleSpin()
    {
        if (state == 0)
        {
            state = 1;
            OnFire(1.0f);
        }
        else if (state == 1)
        {
            state = 0;
            OnFire(0.0f);
        }
        setLimit();

    }

    void checkSpin(float t)
    {
        spinTimer += t;

        if (spinTimer > spinTimerLimit)
        {
            spinTimer = 0;
            toggleSpin();
        }
    }


    public override void EnemyUpdate(float t)
    {
        checkSpin(t);
    }
}
