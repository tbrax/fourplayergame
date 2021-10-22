using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MonoBehaviour
{

    BaseEnemy myBase;
    float difficulty;

    protected Vector2 controlWant;
    protected List<float> level_bounds;
    protected bool collideTop;
    protected bool collideBottom;
    protected bool collideLeft;
    protected bool collideRight;
    List<GameObject> currentCollisions = new List<GameObject>();
    public void Awake()
    {
        myBase = transform.GetComponent<BaseEnemy>();
        difficulty = 1;
        level_bounds = getEntity().levelBounds();
        //moveToRandomPos();
    }

    protected void moveToRandomPos()
    {
        Vector2 want = getRandomScreenPos();
        transform.position = want;
    }

    public virtual void spawnIn()
    {
        moveToRandomPos();
    }

    public void addCollision(GameObject g)
    {
        currentCollisions.Add(g);
    }

    public void removeCollision(GameObject g)
    {
        currentCollisions.Remove(g);
    }

    public void CollideGet(string colliderName, bool type)
    {


        switch (colliderName)
        {
            case "CeilingCollide":
                collideTop = type;
                break;
            case "FloorCollide":
                collideBottom = type;
                break;
            case "SideLeftCollide":
                collideLeft = type;
                break;
            case "SideRightCollide":
                collideRight = type;
                break;
        }
    }

    protected Vector2 getRandomScreenPos()
    {
        float randx = Random.Range(level_bounds[0], level_bounds[1]);
        float randy = Random.Range(level_bounds[2], level_bounds[3]);
        Vector2 want = new Vector2(randx, randy);
        return want;
    }

    public EntityList getEntity()
    {
        return myBase.getEntity();
    }

    public void setDifficulty(float f)
    {
        difficulty = f;
        calcDifficulty(difficulty);
    }

    protected void pushInBounds()
    {
        if (transform.position.x < level_bounds[0])
        {
            transform.position = new Vector2(level_bounds[0], transform.position.y);
        }
        if (transform.position.x > level_bounds[1])
        {
            transform.position = new Vector2(level_bounds[1], transform.position.y);
        }
        if (transform.position.y > level_bounds[3])
        {
            transform.position = new Vector2(transform.position.x, level_bounds[2]);
        }
        if (transform.position.y < level_bounds[2])
        {
            transform.position = new Vector2(transform.position.x, level_bounds[3]);
        }
    }

    public float modDamageFlat(float base_damage, float mod, float scale = 1)
    {
        float ret = base_damage;
        float add = (mod-1) * scale * base_damage;
        ret += add;
        return ret;
    }

    public void playSound(string i)
    {
        myBase.playSound(i);
    }

    public void playSoundContinue(string i)
    {
        myBase.playSoundContinue(i);
    }


    public void stopSoundContinue(string i)
    {
        myBase.stopSoundContinue(i);
    }

    public void addShake(float time)
    {
        myBase.addShake(time);
    }

    public List<PlayerMove> getPlayerInAreaCircle(Vector2 center, float radius)
    {
        return myBase.getPlayerInAreaCircle(center, radius);
    }


    public void dealDamageToPlayersInCircle(Vector2 center, float radius, float damage)
    {
        List <PlayerMove> ps = getPlayerInAreaCircle(center, radius);

        dealDamageToEach(ps, damage);

    }

    void dealDamageToEach(List<PlayerMove> ps, float damage)
    {
        foreach(PlayerMove p in ps)
        {
            p.transform.GetComponent<PlayerStats>().takeDamage(damage);
        }
    }

    

    public virtual void enemyBehavior(float t)
    {
        doMove(t);
    }

    public virtual void OnMove(Vector2 mv)
    {
        controlWant = mv;
    }

    public virtual void OnJump(float mv)
    {

    }

    public virtual void OnFire(float mv)
    {

    }

    public virtual void OnFire2(float mv)
    {

    }

    public virtual void OnSwap(float mv)
    {

    }


    public virtual void calcDifficulty(float f)
    {

    }


    public virtual void doMove(float t)
    {
    }

    public virtual void stopSounds()
    {

    }

    public virtual List<string> getSoundsToStop()
    {
        List<string> s = new List<string>();
        return s;
    }


    public virtual string enemyName()
    {
        return "";
    }
}
