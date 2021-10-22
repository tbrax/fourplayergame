using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNinja : GenericEnemy
{


    Transform star;

    float ninjaSpeed = 5f;

    float wallStuckTimer = 0f;
    //float xMin = -6;
    //float xMax = 6;
    //float yMin = 0;
    //float yMax = 5;
    


    float fireTimer = 0;
    float calcFireTimer = 0.2f;
    //float baseFireTimer = 0.4f;


    bool do_fire;

    float damageTimer = 0;

    float baseDamageSpin = 3;
    float baseDamageStar = 4;
    float spinProcsSecond = 4;

    float calcDamageSpin;
    float calcDamageStar;

    float spinSoundTimer;
    float spinSoundRate = 0.4f;

    void Start()
    {
        star = Resources.Load<Transform>("Prefabs/NinjaStar");
        
    }

    public override string enemyName()
    {
        return "Ninja";
    }


    void startSpin()
    {
        do_fire = true;
        damageTimer = 0;
        spinSoundTimer = 0;
    }

    void endSpin()
    {
        do_fire = false;
    }

    public override void OnFire(float mv)
    {
        if (mv > 0)
        {
            startSpin();
        }
        else
        {
            endSpin();
        }
    }

    void fireStar()
    {
        Transform bullet = Instantiate(star, transform.position, Quaternion.identity);
        bullet.GetComponent<NinjaStar>().setDamage(calcDamageStar);
    }


    public override void calcDifficulty(float f)
    {
        calcDamageSpin = modDamageFlat(baseDamageSpin, f, 0.3f);
        calcDamageStar = modDamageFlat(baseDamageStar, f, 0.3f);
    }

    void checkFire(float t)
    {
        fireTimer += t;
        damageTimer += t;
        if (fireTimer > calcFireTimer)
        {
            fireTimer = 0;
            fireStar();
        }

        if (damageTimer > (1/spinProcsSecond))
        {
            damageTimer = 0;
            dealDamageToPlayersInCircle(transform.position, 2, calcDamageSpin / spinProcsSecond);
        }
    }

    void wallTurn()
    {
        if (wallStuckTimer > 1)
        {
            float randx = Random.Range(level_bounds[0], level_bounds[1]);
            float randy = Random.Range(level_bounds[2], level_bounds[3]);
            Vector2 want = new Vector2(randx, randy);
            transform.up = want - (Vector2)transform.position;
            wallStuckTimer = 0;
        }

    }
    void checkWall(float t)
    {
        wallStuckTimer += t;

        if (transform.position.x > level_bounds[1] ||
            transform.position.x < level_bounds[0] ||
            transform.position.y > level_bounds[3] ||
            transform.position.y < level_bounds[2]
            )
        {
            wallTurn();
        }
    }

    void doSound(float t)
    {
        spinSoundTimer += t;
        if (spinSoundTimer >= spinSoundRate)
        {
            spinSoundTimer = 0;
            playSound("whip");
        }
    }

    // Update is called once per frame
    public override void enemyBehavior(float t)
    {
        if (do_fire)
        {
            transform.Rotate(0, 0, 1000 * t);
            checkFire(t);
            doSound(t);
            
        }
        else
        {
            transform.position += transform.up * t * ninjaSpeed;
        }
        checkWall(t);
    }
}
