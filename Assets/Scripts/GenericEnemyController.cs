using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemyController : MonoBehaviour
{

    private BaseEnemy myEnemy;
    private bool enableControl = true;

    
    public void disableControlled()
    {
        enableControl = false;
    }

    public void enableControlled()
    {
        enableControl = true;
    }

    public virtual void EnemyUpdate(float t)
    {

    }

    public void OnMove(Vector2 value)
    {
        if (enableControl)
        {
            myEnemy.OnMove(value);
        }
    }

    public void OnJump(float value)
    {
        if (enableControl)
            myEnemy.OnJump(value);
    }

    public void OnFire(float value)
    {
        if (enableControl)
            myEnemy.OnFire(value);
    }

    public void OnSwap(float value)
    {
        if (enableControl)
            myEnemy.OnSwap(value);
    }


    public void setEnemy(BaseEnemy b)
    {
        myEnemy = b;
    }
}
