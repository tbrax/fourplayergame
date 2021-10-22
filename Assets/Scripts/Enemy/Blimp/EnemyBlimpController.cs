using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlimpController : GenericEnemyController
{
    Vector2 wantPos;

    float xMin = -6;
    float xMax = 6;
    float yMin = 0;
    float yMax = 5;
    float waitTimer;
    float waitTimerMin = 5;
    float waitTimerMax = 10;
    float waitTimerCurrent;


    void Start()
    {
        
        calcWantSpot();
    }

    void calcWantSpot()
    {
        waitTimer = 0;
        waitTimerCurrent = Random.Range(waitTimerMin, waitTimerMax);
        float randx = Random.Range(xMin, xMax);
        float randy = Random.Range(yMin, yMax);
        wantPos = new Vector2(randx, randy);


    }

    void moveTimer(float t)
    {
        waitTimer += t;
        if (waitTimer > waitTimerCurrent)
        {
            calcWantSpot();
        }
    }

    void moveControl(float t)
    {
        Vector2 diff = wantPos - (Vector2)transform.position;
        if (diff.magnitude > 0.3)
        {
            Vector2 control = diff;
            control.Normalize();
            OnMove(control);
        }
        else
        {
            OnMove(new Vector2(0,0));
        }
    }

    void fireTimer()
    {
        OnFire(1.0f);
    }

    public override void EnemyUpdate(float t)
    {
        fireTimer();
        moveTimer(t);
        moveControl(t);
    }
}
