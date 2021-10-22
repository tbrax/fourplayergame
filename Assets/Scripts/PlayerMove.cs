using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

public class PlayerMove : MonoBehaviour
{

    public Transform m_GameParentPlace;

    //private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private float m_MaxSpeed = 7f;
    [SerializeField] private float m_JumpForce = 10f;
    [SerializeField] private float m_SlideJumpForce = 5f;


    private Transform m_GroundCheck;
    private Transform m_CeilingCheck;
    private bool m_Grounded;


    private TitleControlObjectScript title_control;

    private bool collideLeft;
    private bool collideRight;
    private bool collideSide;
    private bool collideTop;
    private bool collideBottom;
    private bool sliding;
    private bool running;

    private float jumpWindow = 0;
    private float jumpWindowLimit = 0.2f;
    private int slideJumps = 0;
    [SerializeField] private int slideJumpsLimit = 3;
    [SerializeField] private float maxSlideSpeed = 0;

    //[SerializeField] private float slideSlow = 1.0f;


    private EntityList entity_parent;
    Transform images_player;

    private PlayerStats m_stats;

    int numPlayer = 0;

    public bool is_paused = false;

    public float local_time = 1.0f;

    float deadZone = 0.0f;

    public float gravityScale = -2;
    public float gravityFallIncrease = 1.5f;
    Vector2 gravity;

    Vector2 my_velocity = new Vector2(0, 0);

    Vector2 gravity_velocity = new Vector2(0, 0);

    Vector2 control_velocity = new Vector2(0, 0);


    

    List<BaseHat> hats = new List<BaseHat>();
    BaseHat currentHat;

    int hatLimit;

    List<GameObject> currentCollisions = new List<GameObject>();


    int flipSide = 0;


    public List<Sprite> holdSprites;


    bool control_enabled = true;

    bool player_hidden = false;

    bool player_dead = false;

    AnimationPlayer anim;

    public List<float> equipAnimationTimes;
    float lockFlipTimer;

    public void takeDamage(float t)
    {
        m_stats.takeDamage(t);
    }

    bool flipped_for_slide = false;
    public void setTitleControl(TitleControlObjectScript t)
    {
        title_control = t;
    }

    public void takeOffHat(BaseHat h)
    {

    }

    void setPlayTimer(float f)
    {
        anim.setPlayTimer(f);
    }
    int getFlipDirection()
    {
        int f = 0;
        
        if (flipSide == 0)
        {
            f = 1;
        }
        else if (flipSide == 1)
        {
            f = -1;
        }
        if (flipped_for_slide)
        {
            f *= -1;
        }
        return f;
    }

    public TitleControlObjectScript getTitleControl()
    {
       return title_control;
    }

    public int getTitleControlType()
    {
        return title_control.getMyControl();
    }

    public void setTitleControlType(int i)
    {
        title_control.setMyControl(i);
    }

    void hidePlayer()
    {
        player_hidden = true;

        transform.position = new Vector3(500, 500, 0);
    }

    void unHidePlayer(Vector2 v)
    {
        player_hidden = false;

        transform.position = new Vector3(v.x, v.y, 0);
    }


    void enableInput()
    {
        control_enabled = true;
    }
    void disableInput()
    {
        control_enabled = false;
    }

    public void deathChange()
    {
        player_dead = true;
        hidePlayer();

        disableInput();

        entity_parent.playerDeath(this);
    }

    public void setDeathTime(float f)
    {
        title_control.setDeathTime(f);
    }


    /*public static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }*/

    public bool getAlive()
    {
        return m_stats.getAlive();
    }

    public void addCollision(GameObject g)
    {
        currentCollisions.Add(g);
    }

    public void removeCollision(GameObject g)
    {
        currentCollisions.Remove(g);
    }

    private void Awake()
    {
        startPlayer();

    }


    void startPlayer()
    {
        anim = transform.GetComponent<AnimationPlayer>();
        control_enabled = true;
        player_hidden = false;
        player_dead = false;
        gravity = new Vector2(0, gravityScale);
        m_stats = GetComponent<PlayerStats>();
        //m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Grounded = false;
        running = false;
        GameObject e = GameObject.Find("Entities");
        entity_parent = e.GetComponent<EntityList>();
        entity_parent.addPlayer(this);
        transform.SetParent(e.transform.Find("Players"));
        lockFlipTimer = 0;
        images_player = transform.Find("Images");
    }

    public void setupPlayer()
    {
        numPlayer = title_control.getMyPlayer();
    }


    List<BaseThrowable> getAllThrowable()
    {
        return entity_parent.getthrows();
    }

    List<BaseThrowable> getTouchingThrow()
    {
        List<BaseThrowable> t = new List<BaseThrowable>();
        List<BaseThrowable> a = getAllThrowable();
        foreach (BaseThrowable b in a)
        {
            foreach (GameObject g in currentCollisions)
            {
                if (g.transform.parent)
                {
                    if (g.transform.parent.GetComponent<BaseThrowable>())
                    {
                        if (g.transform.parent.GetComponent<BaseThrowable>() == b)
                        {
                            if (t.IndexOf(b) < 0)
                            {
                                t.Add(b);
                            }
                        }
                    }
                }
            }

        }

        return t;
    }

    List<BaseHat> getTouchingHats()
    {
        List<BaseHat> h = new List<BaseHat>();

        List<BaseThrowable> ths = getTouchingThrow();

        foreach(BaseThrowable bt in ths)
        {
            if (bt.gameObject.GetComponent<BaseHat>())
            {
                h.Add(bt.gameObject.GetComponent<BaseHat>());
            }
        }


        return h;
    }


    BaseHat closestHat(List<BaseHat> touch_hats)
    {
        float short_dist = -1;
        BaseHat close = null;

        foreach(BaseHat h in touch_hats)
        {
            if (hats.IndexOf(h) == -1)
            {
                float dist = Vector3.Distance(transform.position, h.transform.position);
                if (short_dist == -1 || dist < short_dist)
                {
                    short_dist = dist;
                    close = h;
                }
            }
        }
        return close;
    }


    void wearThisHat(BaseHat hat){
        if (hat)
        {
            hats.Add(hat);
            hat.wear(this);
        }
    }


    void calcHatSpot(BaseHat hat, int pos)
    {
        Vector3 offsetH = new Vector3(0, 0.35f, 0);

        offsetH.y += 0.4f * pos;
        if (hat)
        {
            hat.transform.position = transform.position + offsetH;
        }
        
    }

    void moveHats(float f)
    {
        int pos = 0;
        foreach(BaseHat hat in hats)
        {
            calcHatSpot(hat, pos);
            pos++;
        }
    }


    BaseHat getFirstHat()
    {
        if (hats.Count > 0)
        {
            return hats[0];
        }

        return null;
    }

    void useHat()
    {

        BaseHat b = getFirstHat();
        if (b)
        {
            int type = b.getHatType();

            useAbility(type);
        }
    }


    void hideItem()
    {
        setHoldSprite(0);
    }

    void showItem()
    {
        //images_player.Find("hold").gameObject.SetActive(true);
    }


    List<BaseThrowable> getThrowInAreaSquare(Vector2 center, Vector2 size)
    {
        List<BaseThrowable> all = getAllThrowable();
        List<BaseThrowable> t = new List<BaseThrowable>();
        foreach (BaseThrowable b in all)
        {
            if (b)
            {
                if (b.transform.position.x > center.x - size.x &&
                    b.transform.position.y > center.y - size.y &&
                    b.transform.position.x < center.x + size.x &&
                    b.transform.position.y < center.y + size.y
                    )
                {
                    t.Add(b);
                }
            }
        }
        return t;
    }

    List<BaseThrowable> getThrowInAreaCircle(Vector2 center, float radius)
    {
        List<BaseThrowable> all = getAllThrowable();
        List<BaseThrowable> t = new List<BaseThrowable>();
        foreach (BaseThrowable b in all)
        {
            if (b)
            {
                if (Vector2.Distance(center,new Vector2(b.transform.position.x, b.transform.position.y)) <= radius)
                {
                    t.Add(b);
                }
            }
        }
        return t;
    }

    public void useAbility(int ab)
    {
        switch(ab)
        {
            case 1:
                abilityBatHit();
                break;
        }
    }

    void abilityBatHit()
    {
        //float bat_radius = 1.5f;
        Vector2 box = new Vector2(1,1);
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        Vector2 add_velocity = new Vector2(0, 0);
        float vel_amount_x = 10;

        float sideMove = 0.4f;
        
        start.x += sideMove * getFlipDirection();
        add_velocity.x = vel_amount_x * getFlipDirection();


        List<BaseThrowable> hit = getThrowInAreaSquare(start, box);

        foreach (BaseThrowable throw_in_area in hit)
        {
            throw_in_area.resetVelocity();
            throw_in_area.addVelocity(add_velocity);
        }

        if (hit.Count > 0)
        {
            playSound("woosh");
            
        }
        else
        {
            playSound("woosh");
        }

        setPlayTimer(equipAnimationTimes[1]);
        setLockFlipTimer(equipAnimationTimes[1]);


    }



    void setHoldSprite(int sp)
    {
        anim.setEquip(sp);
        //images_player.Find("hold").GetComponent<SpriteRenderer>().sprite = holdSprites[sp];

    }
    void setFirstHatHold()
    {

        if (hats.Count > 0)
        {
            BaseHat b = getFirstHat();
            int type = b.getHatType();

            setHoldSprite(type);

        }
        else
        {
            hideItem();
        }
    }


    void carouselHats()
    {

    }

    public void pickupHat()
    {
        List<BaseHat> touch_hats = getTouchingHats();

        if (touch_hats.Count > 0)
        {
            BaseHat close = closestHat(touch_hats);
            wearThisHat(close);
        }
        else
        {
            carouselHats();
        }

        setFirstHatHold();
    }

    public void doSwap()
    {
        pickupHat();
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

    public float getHealth()
    {
        return m_stats.getHealth();
    }


    public EntityList getEntParent()
    {
        return entity_parent;
    }


    public int myNum()
    {
        return numPlayer;
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

    public void stateChangePlayer()
    {
        entity_parent.stateChangePlayer();
    }

    void collideCheck()
    {
        
        jumpWindow += Time.deltaTime * local_time;
        if (jumpWindow > jumpWindowLimit)
        {
            m_Grounded = false;
        }


        if (collideBottom)
        {
            m_Grounded = true;
            jumpWindow = 0;
            slideJumps = 0;
            collideWithGround();
        }

        if (collideTop && !(collideBottom))
        {
            collideWithCeiling();
        }

        collideSide = false;
        if (collideLeft || collideRight)
        {
            collideSide = true;
        }

        sliding = false;
        if (collideSide && !collideBottom)
        {
            sliding = true;
        }

    }

    void playSound(string name)
    {
        entity_parent.playSound(name);
    }

    void slideMarks()
    {
        if (sliding)
        {

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

    void collideWithCeiling()
    {
        if (gravity_velocity.y > 0)
        {
            gravity_velocity.y = 0;
        }
    }

    bool isSliding()
    {
        return (!m_Grounded && collideSide);
    }

    private void applyGravity(float f)
    {
        if (!m_Grounded)
        {
            Vector2 grav = gravity;
            if (grav.y < 0)
            {
                grav *= gravityFallIncrease;
            }
            gravity_velocity += grav * f;
        }
    }

    void applyVelocity(float f)
    {
        my_velocity.x = 0;
        my_velocity.y = 0;


        if (collideLeft)
        {
            if (control_velocity.x < 0)
            {
                control_velocity.x = 0;
            }
        }

        if (collideRight)
        {
            if (control_velocity.x > 0)
            {
                control_velocity.x = 0;
            }
        }


        my_velocity += control_velocity;
        my_velocity += gravity_velocity;

        if (isSliding())
        {
            slideGravity();
            /*if (my_velocity.y < maxSlideSpeed)
            {
                my_velocity.y = maxSlideSpeed;
            }*/
        }
        transform.position += (Vector3)my_velocity * f;
    }


    private void slideGravity()
    {
        gravity_velocity.x = 0;
        if (gravity_velocity.y < maxSlideSpeed)
        {
            gravity_velocity.y = maxSlideSpeed;
        }
    }

    void otherTimers(float t)
    {
        lockFlipTimer -= t;
    }

    private void otherMovement()
    {
        float f = Time.deltaTime * local_time;
        applyGravity(f);
        applyVelocity(f);
        moveHats(f);
        updateAnimationState();
        otherTimers(f);
        anim.updateAnimationTime(f);
    }

    void updateAnimationState()
    {
        //ClearLog();

        anim.updateAnimationStateLegs(m_Grounded, running, isSliding());
    }

    private void FixedUpdate()
    {
        if (!is_paused && !player_hidden && !player_dead)
        {


            collideCheck();
            otherMovement();
            collideCheck();
            slideMarks();
        }
    }

    public void Move(Vector2 movement, bool jump, bool fire, bool swapHat, bool fire2)
    {
        if (!is_paused && control_enabled)
        {
            doMove(movement, jump, fire, swapHat, fire2);
        }
    }


    void checkSlideFlip()
    {

        if (flipped_for_slide)
        {
            if (isSliding())
            {
                
            }
            else
            {
                images_player.Rotate(0, 180, 0);
                flipped_for_slide = false;
            }

        }
        else
        {
            if (isSliding())
            {
                images_player.Rotate(0, 180, 0);
                flipped_for_slide = true;
            }
            else
            {
                
            }
        }
    }

    void setLockFlipTimer(float t)
    {
        lockFlipTimer = t;
    }

    void checkWalkFlip(float x)
    {
        if (lockFlipTimer <= 0)
        {

            if (flipSide == 0)
            {
                if (x < -0.1)
                {

                    images_player.Rotate(0, 180, 0);
                    flipSide = 1;

                }
            }
            else if (flipSide == 1)
            {
                if (x > 0.1)
                {
                    images_player.Rotate(0, 180, 0);
                    flipSide = 0;
                }

            }
        }

    }

    void checkSpriteFlip(float x)
    {
        checkWalkFlip(x);
        checkSlideFlip();
    }

    public void doMove(Vector2 movement, bool jump, bool fire, bool swapHat, bool fire2)
    {
        if (Mathf.Abs(movement.x) > deadZone)
        {
            running = true;
            control_velocity.x = movement.x * m_MaxSpeed;
        }
        else
        {
            running = false;
            control_velocity.x = 0;
        }
        
        checkSpriteFlip(control_velocity.x);


        if (swapHat)
        {
            doSwap();
        }
        if (fire)
        {
            useHat();
        }

        if (jump)
        {
            if (m_Grounded)
            {
                gravity_velocity.y = m_JumpForce;
            }
            else
            {
                if (collideSide && (slideJumps < slideJumpsLimit))
                {
                    slideJumps++;
                    gravity_velocity.y = m_SlideJumpForce;
                    
                    //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_SlideJumpForce);
                }
            }
        }
    }
}
