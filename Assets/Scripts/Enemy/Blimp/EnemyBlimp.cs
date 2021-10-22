using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlimp : GenericEnemy
{


    float baseDamageBomb = 20;
    float calcDamageBomb;

    float bombTimer = 0;
    float calcBombTimer = 5f;

    float speed = 1;


    Transform bomb;




    // Start is called before the first frame update
    void Start()
    {
        bomb = Resources.Load<Transform>("Prefabs/EnemyMisc/Blimp/Bomb");
        randomFlip();
        playSoundContinue("blimploop");
    }

    void randomFlip()
    {
        int flip = Random.Range(0, 2);

        if (flip == 1)
        {
            transform.Rotate(0, 180, 0);
        }
    }




    public override string enemyName()
    {
        return "Blimp";
    }

    public override void calcDifficulty(float f)
    {
        calcDamageBomb = modDamageFlat(baseDamageBomb, f, 0.3f);
    }

    void dropBomb()
    {
        Transform bullet = Instantiate(bomb, transform.position, Quaternion.identity);
        bullet.GetComponent<Bomb>().setDamage(calcDamageBomb);
    }


    void countBomb(float t)
    {
        bombTimer += t;
        
    }

    public override void OnFire(float mv)
    {
        if (bombTimer > calcBombTimer)
        {
            bombTimer = 0;
            dropBomb();
        }
    }

    public override void doMove(float t)
    {
        Vector2 cl = controlWant;
        cl.Normalize();
        transform.position += (Vector3)cl * t * speed;
        pushInBounds();
    }

    public override void enemyBehavior(float t)
    {
        countBomb(t);
        doMove(t);
    }
}
