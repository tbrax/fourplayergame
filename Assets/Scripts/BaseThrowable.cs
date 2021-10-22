using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseThrowable : MonoBehaviour
{

    protected float lifeTimer = 0;
    protected float lifeTimerMax = 10;
    protected Vector2 veloc;

    bool is_paused;
    float local_time = 1.0f;

    protected EntityList entity_parent;
    protected Vector2 my_velocity = new Vector2(0, 0);
    protected Vector2 gravity_velocity = new Vector2(0, 0);
    List<GameObject> currentCollisions = new List<GameObject>();
    List<string> soundsToStop;

    protected bool is_dying;
    private bool m_Grounded;
    bool collideBottom;
    bool collideTop;
    bool collideLeft;
    bool collideRight;
    private void Awake()
    {
        GameObject e = GameObject.Find("Entities");
        transform.SetParent(e.transform.Find("Throw"));
        entity_parent = e.GetComponent<EntityList>();
        entity_parent.addThrow(this);
        soundsToStop = new List<string>();
        is_dying = true;
        m_Grounded = false;
        collideBottom = false;
    }

    public void setPause(bool p)
    {
        is_paused = p;
    }

    public void setLocalTime(float t)
    {
        local_time = t;
    }

    public float getLocalTime()
    {
        return local_time;
    }

    public void addShake(float time)
    {
        entity_parent.addShake(time);
    }

    public void playSound(string name)
    {
        entity_parent.playSound(name);
    }


    protected void dealDamageToPlayer(Transform hit, float damage)
    {
        hit.GetComponent<PlayerStats>().takeDamage(damage);
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




    public void addCollision(GameObject g)
    {
        currentCollisions.Add(g);
    }

    public void removeCollision(GameObject g)
    {
        currentCollisions.Remove(g);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //currentCollisions.Add(col.gameObject);
        if (!is_paused)
        {
            //doCollision(col);
        }
    }

    void OnCollisionExit(Collision col)
    {
        //currentCollisions.Remove(col.gameObject);
    }




    void OnTriggerEnter2D(Collider2D col)
    {
        if (!is_paused)
        {
            doTrigger(col);
        }
    }
    public List<PlayerMove> getAllPlayers()
    {
        return entity_parent.getPlayers();
    }

    public List<PlayerMove> getPlayerInAreaCircle(Vector2 center, float radius)
    {
        List<PlayerMove> all = getAllPlayers();

        List<PlayerMove> t = new List<PlayerMove>();
        foreach (PlayerMove b in all)
        {
            if (b)
            {
                if (Vector2.Distance(center, new Vector2(b.transform.position.x, b.transform.position.y)) <= radius)
                {
                    t.Add(b);
                }
            }
        }
        return t;
    }



    protected bool checkHitFloor(Collision2D col)
    {
        if (col.transform.parent)
        {
            if (col.transform.parent.name == "Terrain" || col.transform.parent.parent.name == "Terrain")
            {
                return true;
            }
        }
        return false;
    }

    protected bool checkHitFloor(Collider2D col)
    {
        if (col.transform.parent)
        {
            if (col.transform.parent.name == "Terrain")
            {

                return true;
            }
        }
        return false;
    }

    protected bool checkHitFloor(GameObject col)
    {
        if (col.transform.parent)
        {
            if (col.transform.parent.name == "Terrain")
            {

                return true;
            }
        }
        return false;
    }




    protected bool checkHitPlayer(Collision col)
    {
        if (col.transform.parent)
        {
            if (col.transform.parent.name.StartsWith("Player"))
            {
                return true;
            }
        }
        return false;
    }

    protected bool checkHitPlayer(Collider2D col)
    {
        if (col.transform.parent)
        {
            if (col.transform.parent.name.StartsWith("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public void resetVelocity()
    {
        my_velocity.x = 0;
        my_velocity.y = 0;

    }

    protected virtual void doCollision(Collision2D col)
    {
        if (checkHitFloor(col))
        {
            resetVelocity();
        }
    }

    protected virtual void doTrigger(Collider2D col)
    {
        if (checkHitFloor(col))
        {
            resetVelocity();
        }

    }


    public void addVelocity(Vector2 v)
    {
        my_velocity += v;
    }

    protected void applyVelocity(float f)
    {

        if (!m_Grounded)
        {
            gravity_velocity.y -= (float)(8 * f);
        }
        
        transform.position += (Vector3)gravity_velocity * f;
        transform.position += (Vector3)my_velocity * f;

    }


    public void checkTouching()
    {
        currentCollisions.RemoveAll(item => item == null);

        foreach (GameObject g in currentCollisions)
        {
            if (checkHitFloor(g))
            {
                resetVelocity();
            }
        }
    }


    void collideWithGround()
    {
        gravity_velocity.x = 0;
        if (gravity_velocity.y < 0)
        {
            gravity_velocity.y = 0;
        }
    }

    void collideCheck()
    {
        m_Grounded = false;

        if (collideBottom)
        {
            m_Grounded = true;
            collideWithGround();
        }


    }

    public virtual void updateMovement(float t)
    {
        collideCheck();
        
        checkTouching();
        applyVelocity(t);
    }

    public virtual void doTimers(float t)
    {


    }

    public void playSoundContinue(string i)
    {
        soundsToStop.Add(i);
        entity_parent.playSoundContinue(i);
    }

    public void stopSoundContinue(string i)
    {
        entity_parent.stopSoundContinue(i);
    }

    void endContinueSounds()
    {
        foreach (string s in soundsToStop)
        {
            stopSoundContinue(s);
        }
    }

    public virtual void myDeath()
    {
        endContinueSounds();

        entity_parent.removeThrow(this);
        Destroy(transform.gameObject);
    }
    private void updateLife(float t)
    {
        
        if (lifeTimerMax > -10 && is_dying)
        {
            lifeTimer += t;
            if (lifeTimer >= lifeTimerMax)
            {
                lifeTimer = lifeTimerMax;
                myDeath();
            }
        }

    }




    // Update is called once per frame
    void Update()
    {
        if (!is_paused)
        {
            float t = Time.deltaTime * local_time;
            doTimers(t);
            updateMovement(t);
            updateLife(t);
        }
    }
}
