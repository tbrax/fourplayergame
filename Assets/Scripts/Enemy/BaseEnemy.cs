using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    float lifeTimer = 60;
    float hard;
    private EntityList entity_parent;

    private int inputType = 0;

    private GenericEnemy enemyType;
    private GenericEnemyController enemyControl;

    public bool is_paused;
    float local_time = 1.0f;
    List<string> soundsToStop;


    void Awake()
    {
        //GameObject e = GameObject.Find("Entities");
        //entity_parent = e.GetComponent<EntityList>();
        //entity_parent.addEnemy(BaseEnemy);
        soundsToStop = new List<string>();
    }


    public void controlByPlayer(TitleControlObjectScript control)
    {
        enemyControl.disableControlled();
    }


    public void addShake(float time)
    {
        entity_parent.addShake(time);
    }

    public void playSound(string i)
    {
        entity_parent.playSound(i);
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


    public void setDifficulty(float f)
    {
        enemyType.setDifficulty(f);
    }

    public void setEntity(EntityList e)
    {
        entity_parent = e;
    }

    public EntityList getEntity()
    {
        return entity_parent;
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


    public List<BaseThrowable> getAllThrowable()
    {
        return entity_parent.getthrows();
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

    public void addSpecificEnemyImage()
    {

        string name = transform.GetComponent<GenericEnemy>().enemyName();

        string path = "Prefabs/EnemySpecific/" + name;

        Transform enemy_prefab = Resources.Load<Transform>(path);

        Transform enemy_spawn = Instantiate(enemy_prefab, new Vector3(0, 0, 0), Quaternion.identity);

        enemy_spawn.parent = transform.Find("EnemyBox");
    }


    public void setupEnemy(int enemyTypeNum)
    {
        addEnemyScript(enemyTypeNum);
        addSpecificEnemyImage();
        enemyType.spawnIn();
    }

    public void addEnemyScript(int enemyAddType)
    {

        switch (enemyAddType)
        {
            case 1:
                transform.gameObject.AddComponent<EnemyNinja>();
                enemyType = transform.GetComponent<EnemyNinja>();

                transform.gameObject.AddComponent<EnemyNinjaController>();
                enemyControl = transform.GetComponent<EnemyNinjaController>();
                break;
            case 2:
                transform.gameObject.AddComponent<EnemyBlimp>();
                enemyType = transform.GetComponent<EnemyBlimp>();

                transform.gameObject.AddComponent<EnemyBlimpController>();
                enemyControl = transform.GetComponent<EnemyBlimpController>();
                break;
            case 3:
                transform.gameObject.AddComponent<EnemySaw>();
                enemyType = transform.GetComponent<EnemySaw>();

                transform.gameObject.AddComponent<EnemySawController>();
                enemyControl = transform.GetComponent<EnemySawController>();
                break;
        }


        enemyControl.setEnemy(this);

    }

    public void setLifeTimer(float t)
    {
        lifeTimer = t;
    }

    void endContinueSounds()
    {
        foreach(string s in soundsToStop)
        {
            stopSoundContinue(s);
        }
    }

    private void myDeath()
    {
        endContinueSounds();
        entity_parent.removeEnemy(this);
        Destroy(transform.gameObject);
    }

    private void updateLife(float t)
    {
        lifeTimer -= t;
        if (lifeTimer <= 0)
        {
            lifeTimer = 0;
            myDeath();
        }

    }

    public void OnMove(Vector2 value)
    {
        enemyType.OnMove(value);
    }

    public void OnJump(float value)
    {
        enemyType.OnJump(value);
    }

    public void OnFire(float value)
    {
        enemyType.OnFire(value);
    }

    public void OnFire2(float value)
    {
        enemyType.OnFire2(value);
    }

    public void OnSwap(float value)
    {
        enemyType.OnSwap(value);
    }


    void enemyBehavior(float t)
    {
        enemyType.enemyBehavior(t);
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_paused)
        {
            float t = Time.deltaTime * local_time;
            updateLife(t);
            enemyBehavior(t);
            if (inputType == 0)
            {
                enemyControl.EnemyUpdate(t);
            }
        }
    }
}
