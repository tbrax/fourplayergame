using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerMove m_Character;
    float health;

    bool isAlive;
    float baseHealth = 100f;
    private void Awake()
    {
        m_Character = GetComponent<PlayerMove>();

        resetCharacter();
    }


    void resetCharacter()
    {
        health = baseHealth;
        isAlive = true;
        stateChangePlayer();
    }


    public bool getAlive()
    {
        return isAlive;
    }

    void stateChangePlayer()
    {
        m_Character.stateChangePlayer();
    }

    public float getHealth()
    {
        return health;
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        checkDead();
        stateChangePlayer();
    }

    private void myDeath()
    {
        stateChangePlayer();
        isAlive = false;
        m_Character.deathChange();

        
    }

    private void checkDead()
    {
        if (health <= 0)
        {
            health = 0;

            if (isAlive)
            {
                myDeath();
            }
        }
    }



}
