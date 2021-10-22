using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySaw : GenericEnemy
{
    float damage;
    bool goingRight;

    Vector2 crawlDirection;
    Vector2 gravityDirection;

    int spinState;
    float spinStateTimer;
    float spinProcsSecond = 5;
    float damageTimer = 0;

    float baseDamageSpin = 20;
    float calcDamageSpin;
    float speed = 2;
    bool isMoving;
    bool m_Grounded;

    float gravity = 8;

    float fall_speed;


    int spinAroundType;

    bool changeWallState = false;

    int simpleDirection;
    Transform blade;
    float spinSpeed = -2000;
    List<float> bounds;
    bool isDamage;
    void Start()
    {
        isMoving = true;
        goingRight = true;
        isDamage = true;
        spinState = 0;
        spinStateTimer = 0;
        fall_speed = 0;
        spinAroundType = 0;
        playSoundContinue("sawloop");
        blade = transform.Find("EnemyBox").Find("Saw(Clone)").Find("EnemyType").Find("Images").Find("Main");
        //gravityDirection = new Vector2(0, 0);
    }

    void pickGravity(int i)
    {
        gravityDirection = new Vector2(0, -1);
    }




    public override void calcDifficulty(float f)
    {
        calcDamageSpin = modDamageFlat(baseDamageSpin, f, 0.3f);
    }

    public override string enemyName()
    {
        return "Saw";
    }

    public override void spawnIn()
    {
        calcBounds();
        randomSpawnInBounds();
    }

    void calcBounds()
    {
        switch (spinAroundType)
        {
            case 0:
                bounds = level_bounds;
                break;
            case 1:
                break;

        }
    }

    private void move(float t)
    {

        Vector2 move = new Vector2(0,0);
        if (goingRight)
        {
            move = Vector2.Perpendicular(-gravityDirection);

        }
        if (goingRight)
        {
            move = Vector2.Perpendicular(-gravityDirection);
            move *= -1;
        }


        if (m_Grounded)
        {
            transform.position += (Vector3)move * t * speed;
        }
        
    }

    void checkFire(float t)
    {
        damageTimer += t;


        if (damageTimer > (1 / spinProcsSecond))
        {
            damageTimer = 0;
            dealDamageToPlayersInCircle(transform.position, 2f * transform.localScale.x, calcDamageSpin / spinProcsSecond);
        }
    }

    public override void OnFire(float mv)
    {
        goingRight = !goingRight;
    }

    public override void OnJump(float mv)
    {
        if (mv > 0)
        {
            isMoving = !isMoving;
        }
    }

    Vector3 newAngle(float angle)
    {
        //Vector2 v = Quaternion.AngleAxis(angle, Vector3.forward) * gravityDirection;
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
    }

    void checkCollide()
    {
        m_Grounded = false;

        if (collideBottom)
        {
            m_Grounded = true;
            fall_speed = 0;
        }

        if (!collideRight)
        {
            changeWallState = true;
        }

        if (collideBottom && collideRight)
        {
            if (changeWallState)
            {
                
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 90);

                Vector3 v = newAngle(transform.eulerAngles.z);
                gravityDirection = new Vector2(v.x, v.y);
            }
        }
        //float f = Vector2.Angle(Vector2.down, gravityDirection);
        
    }


    Vector3 getRandomSpawn()
    {
        int r = Random.Range(0, 3);
        Vector3 s = new Vector3(0, 0, 0);
        switch (r)
        {
            case 0:
                //Lower left corner
                simpleDirection = 0;
                s.x = bounds[0];
                s.y = bounds[2];
                break;
            case 1:
                //Lower right corner
                simpleDirection = 1;
                s.x = bounds[1];
                s.y = bounds[3];
                break;
            case 2:
                //upper right
                simpleDirection = 2;
                s.x = bounds[1];
                s.y = bounds[3];
                break;
            case 3:
                //upper left
                simpleDirection = 3;
                s.x = bounds[0];
                s.y = bounds[3];
                break;
            case 4:
                //lower middle
                simpleDirection = 0;
                s.x = (bounds[0] + bounds[1])/2;
                s.y = bounds[2];
                break;
            case 5:
                //right middle
                simpleDirection = 1;
                s.x = bounds[1];
                s.y = (bounds[2] + bounds[3]) / 2;
                break;
            case 6:
                //upper middle
                simpleDirection = 2;
                s.x = (bounds[0] + bounds[1]) / 2;
                s.y = bounds[3];
                break;
            case 7:
                //left middle
                simpleDirection = 3;
                s.x = bounds[0];
                s.y = (bounds[2] + bounds[3]) / 2;
                break;


        }
        return s;
    }


    void randomSpawnInBounds()
    {
        transform.position = getRandomSpawn();




    }

    void moveInBounds()
    {
        if (transform.position.x >= bounds[1])
        {
            transform.position = new Vector3(bounds[1], transform.position.y, 0);
        }
        if (transform.position.y >= bounds[3])
        {
            transform.position = new Vector3(transform.position.x, bounds[3], 0);
        }
        if (transform.position.x <= bounds[0])
        {
            transform.position = new Vector3(bounds[0], transform.position.y, 0);
        }

        if (transform.position.y <= bounds[2])
        {
            transform.position = new Vector3(transform.position.x, bounds[2], 0);
        }

    }

    int spinDirection()
    {
        if (!isMoving)
        {
            return 0;
        }

        if (!goingRight)
        {
            return -1;
        }
        return 1;
    }

    void spinAroundLevel(float t)
    {
        //Debug.Log(simpleDirection);
        switch (simpleDirection)
        {
            case 0:
                transform.position += new Vector3(1,0,0) * t * speed * spinDirection();
                if (transform.position.x >= bounds[1])
                {
                    //transform.position = new Vector3(bounds[0], transform.position.y, 0);
                    simpleDirection = 1;
                }
                
                break;
            case 1:
                transform.position += new Vector3(0, 1, 0) * t * speed * spinDirection();
                if (transform.position.y >= bounds[3])
                {
                    //transform.position = new Vector3(transform.position.x, bounds[3], 0);
                    simpleDirection = 2;
                }
                

                break;
            case 2:
                transform.position += new Vector3(-1, 0, 0) * t * speed * spinDirection();
                if (transform.position.x <= bounds[0])
                {
                    //transform.position = new Vector3(bounds[1], transform.position.y, 0);
                    simpleDirection = 3;
                }
                
                break;
            case 3:
                transform.position += new Vector3(0, -1, 0) * t * speed * spinDirection();
                if (transform.position.y <= bounds[2])
                {
                    //transform.position = new Vector3(transform.position.x, bounds[2] , 0);
                    simpleDirection = 0;
                }
                

                break;

        }
        moveInBounds();

    }

    void spinAroundBoxCollide(float t)
    {

    }

    void imageSpin(float f)
    {
        if (isDamage)
        {

            blade.Rotate(0.0f, 0.0f, spinSpeed * f * spinDirection());
        }
    }

    void simpleSpin(float t)
    {
        imageSpin(t);
        switch (spinAroundType)
        {
            case 0:
                spinAroundLevel(t);
                break;
            case 1:
                spinAroundLevel(t);
                //spinAroundBoxCollide(t);
                break;
        }
    }

    void doGravity(float f)
    {
        if (!m_Grounded)
        {
            fall_speed += f * gravity;

            transform.position += (Vector3)gravityDirection * f * fall_speed;
        }
    }


    void advancedSpin(float t)
    {
        checkCollide();
        doGravity(t);
    }

    public override void enemyBehavior(float t)
    {
        simpleSpin(t);
        checkFire(t);
    }
}
