using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : BaseThrowable
{
    float timer;
    const float maxTimer = 5;
    //Transform stuckTo;
    float damage;
    float stuckTimer;
    float stuckTimerMax = 0.8f;

    //float stickCheckTimer = 0;
    //float stickCheckTimerMax = 0.001f;

    float stickRadius = 0.3f;
    int stickState = 0;

    const int stick_state_player = 1;

    PlayerMove stuckToPlayer;

    Transform explode_prefab;
    Transform hand;
    bool has_exploded = false;


    const float alarm_ratio = 0.84f;
    bool set_alarm;
    public void setDamage(float d)
    {
        damage = d;
    }

    // Start is called before the first frame update
    void Start()
    {
        has_exploded = false;
        //stickCheckTimer = 0f;
        stickState = 0;
        timer = 0;
        lifeTimerMax = maxTimer;
        hand = transform.Find("Image").Find("tnthand");
        explode_prefab = Resources.Load<Transform>("Prefabs/EnemyMisc/Explosion");
        playSoundContinue("ticktock");
        set_alarm = false;
    }

    void stickToPlayer(PlayerMove p)
    {
        stuckTimer = 0;
        stuckToPlayer = p;
        stickState = stick_state_player;
    }

    void drop()
    {
        stickState = 0;
        stuckToPlayer = null;
        stuckTimer = 0;
    }

    void checkStickPlayers(List<PlayerMove> ps)
    {
        float dist = stickRadius *2;
        PlayerMove want = null;

        foreach (PlayerMove p in ps)
        {
            float dist_to = Vector2.Distance(transform.position, p.transform.position);

            if (p != stuckToPlayer)
            {
                if (dist_to < dist)
                {
                    dist = dist_to;
                    want = p;
                }
            }
        }

        if (want != null)
        {
            stickToPlayer(want);
        }
    }


    protected override void doCollision(Collision2D col)
    {
        if (checkHitFloor(col))
        {
            resetVelocity();
        }
    }


    void stickToPlayers(float t)
    {
        stuckTimer += t;

        if (stuckTimer > stuckTimerMax)
        {
            List<PlayerMove> ps = getPlayerInAreaCircle(transform.position, stickRadius);
            checkStickPlayers(ps);
        }
        
    }

    void explode()
    {
        Transform bullet = Instantiate(explode_prefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Explosion>().setDamage(damage);
        playSound("explosion0");
        addShake(0.2f);
    }

    void bombHand()
    {
        float rot = (timer / maxTimer) * 360;
        hand.eulerAngles = new Vector3(0, 0, -rot);
    }

    void tickDown(float t)
    {
        timer += t;
        bombHand();

        if (!set_alarm && timer > (maxTimer * alarm_ratio))
        {
            set_alarm = true;
            playSound("alarmbell");
        }


        if (timer > maxTimer)
        {
            if (!has_exploded)
            {
                has_exploded = true;
                explode();
            }
        }
    }

    public override void updateMovement(float t)
    {
        if (stickState == 0)
        {
            base.updateMovement(t);
        }
        else if (stickState == stick_state_player)
        {
            transform.position = (Vector2)stuckToPlayer.transform.position;
        }
    }

    public override void doTimers(float t)
    {
        stickToPlayers(t);
        tickDown(t);
    }

}
