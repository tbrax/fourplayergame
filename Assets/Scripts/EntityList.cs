using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EntityList : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform m_ui;

    public Transform e_prefab;
    public Transform hat_prefab;
    List<PlayerMove> players = new List<PlayerMove>();
    int numPlayers = 0;

    List<BaseEnemy> enemies = new List<BaseEnemy>();

    List<BaseThrowable> throws = new List<BaseThrowable>();

    List<BaseHat> hats = new List<BaseHat>();

    public Transform main_camera;
    Vector3 main_camera_original_pos;


    float enemyTimer = 0;
    float enemyRate = 10;
    float enemyRateMin = 8;
    float enemyRateMax = 20;

    int enemyMax = 5;

    float hatTimer = 0;
    float hatRate = 2;
    int hatMax = 3;

    float totalTime = 0;

    float pauseTimer = 0;
    float pauseTimerLimit = 0.5f;
    bool is_paused = false;


    float local_time = 1.0f;

    int pauseOptionSelect = 0;

    float pauseMenuMoveTimer = 0;

    float difficulty = 1;

    int control_type_enemy = 3;
    int control_type_player = 1;

    AudioManager audio_manager;

    public List<int> validEnemyTypeList;


    float shakeTimer;

    float levelMinX = -6;
    float levelMaxX = 6;
    float levelMinY = -5;
    float levelMaxY = 5;

    LevelBackground background;

    public void Awake()
    {
        startGame();
    }

    public bool checkHitPlayer(Collision2D col)
    {
        Debug.Log(col.transform.tag);
        if (col.transform.tag == "Player")
        {
            return true;
        }

        return false;
    }

    public bool checkHitThrow(Collision2D col)
    {
        if (col.transform.parent)
        {
            if (col.transform.tag == "Throw")
            {
                return true;
            }
        }
        return false;
    }


    public List<float> levelBounds()
    {
        List<float> ls = new List<float>();
        float camX = main_camera_original_pos.x;
        float camY = main_camera_original_pos.y;

        ls.Add(camX + levelMinX);
        ls.Add(camX + levelMaxX);
        ls.Add(camY + levelMinY);
        ls.Add(camY + levelMaxY);
        return ls;
    }


    public void playSound(string name)
    {
        audio_manager.Play(name);
    }

    public void playSoundContinue(string name)
    {
        audio_manager.playContinue(name);
    }

    public void stopSoundContinue(string name)
    {
        audio_manager.stopContinue(name);
    }


    void changeInputManager()
    {
        GameObject inp = GameObject.Find("MainInputManager");
        inp.GetComponent<PlayerInputManager>().DisableJoining();


    }

    public void playerDeath(PlayerMove player)
    {
        player.setDeathTime(totalTime);


        CheckForGameEnd();
    }


    public List<BaseThrowable> getthrows()
    {
        return throws;
    }

    public List<PlayerMove> getPlayers()
    {
        return players;
    }

    void spawnPlayersForEachInput()
    {
        Transform playerParent = GameObject.Find("Controls").transform;

        foreach (Transform child in playerParent)
        {
            child.GetComponent<TitleControlObjectScript>().mainGameStart(this);
        }

    }


    void setPauseToEntities()
    {
        foreach (PlayerMove p in players)
        {
            p.setPause(is_paused);
        }

        foreach (BaseEnemy p in enemies)
        {
            p.setPause(is_paused);
        }

        foreach (BaseThrowable p in throws)
        {
            p.setPause(is_paused);
        }

    }


    void setPauseTrue()
    {
        is_paused = true;
        showPauseWindow();
        pauseOptionSelect = 0;
        movePauseSelectBox();
    }

    void setPauseFalse()
    {
        is_paused = false;
        hidePauseWindow();
    }

    void changePauseState()
    {
        if (is_paused)
        {
            setPauseFalse();
        }
        else
        {
            setPauseTrue();

        }


    }
    public void togglePause()
    {
        if (pauseTimer > pauseTimerLimit)
        {
            pauseTimer = 0;
            changePauseState();
            setPauseToEntities();
        }
    }




    void setupAudio()
    {
        if (GameObject.Find("AudioManagerEnabled").GetComponent<AudioManager>())
        {
            audio_manager = GameObject.Find("AudioManagerEnabled").GetComponent<AudioManager>();
        } 
    }


    void setupBackground()
    {
        background = transform.GetComponent<LevelBackground>();
        background.startBackground();
    }

    void startGame()
    {
        
        setupAudio();
        changeInputManager();
        spawnPlayersForEachInput();
        hidePauseWindow();
        setupPlayers();
        shakeTimer = 0;
        main_camera_original_pos = main_camera.position;
        setupBackground();
    }


    public void addShake(float time)
    {
        if (shakeTimer < time)
        {
            shakeTimer = time;
        }
    }

    void doShake(float t)
    {

        if (shakeTimer > 0)
        {
            shakeTimer -= t;
            float magnitude = 0.5f;
            float offsetX = Random.Range(-0.5f, 0.5f) * magnitude;
            float offsetY = Random.Range(-0.5f, 0.5f) * magnitude;
            main_camera.position = new Vector3(offsetX, offsetY, main_camera_original_pos.z);
        }

        else
        {
            shakeTimer = 0;
            main_camera.position = main_camera_original_pos;
        }
    }


    void setupPlayers()
    {
        foreach(PlayerMove p in players)
        {
            p.setupPlayer();
        }
    }

    public void addPlayer(PlayerMove p)
    {
        players.Add(p);
        numPlayers++;
    }

    void doGlobalTimer(float f)
    {
        totalTime += f;
    }


    public void addEnemy(BaseEnemy e)
    {
        enemies.Add(e);
    }



    List<TitleControlObjectScript> getAllControllers()
    {
        List<TitleControlObjectScript> c = new List<TitleControlObjectScript>();

        foreach(PlayerMove p in players)
        {
            c.Add(p.getTitleControl());
        }
        return c;
    }

    void removeEnemyControl(BaseEnemy e)
    {
        List<TitleControlObjectScript> controls = getAllControllers();

        foreach(TitleControlObjectScript t in controls)
        {
            if (t.getMyControl() == control_type_enemy && t.getEnemyControl() == e)
            {
                t.setMyControl(control_type_player);
            }
        }
    }

    public void removeEnemy(BaseEnemy e)
    {
        removeEnemyControl(e);
        enemies.Remove(e);
    }

    public void addThrow(BaseThrowable e)
    {
        throws.Add(e);
    }

    public void removeThrow(BaseThrowable e)
    {
        throws.Remove(e);
    }

    public void addHat(BaseHat e)
    {
        hats.Add(e);
    }

    public void removeHat(BaseHat e)
    {
        hats.Remove(e);
    }

    List<PlayerMove> getDeadPlayers()
    {
        List < PlayerMove > p = new List<PlayerMove>();

        foreach(PlayerMove pm in players)
        {
            if (!pm.transform.GetComponent<PlayerStats>().getAlive())
            {
                p.Add(pm);
            }
        }

        return p;
    }

    List<TitleControlObjectScript> getDeadPlayersForEnemyControl()
    {
        List<PlayerMove> p = getDeadPlayers();

        List<TitleControlObjectScript> pWant = new List<TitleControlObjectScript>();

        foreach(PlayerMove pm in p)
        {
            TitleControlObjectScript control = pm.getTitleControl();

            if (control.getMyControl() == control_type_player)
            {
                pWant.Add(control);
            }
        }
        return pWant;
    }



    int getValidEnemyType()
    {
        int i = Random.Range(0, validEnemyTypeList.Count);
        return validEnemyTypeList[i];
    }

    void regularSpawn()
    {
        int type = getValidEnemyType();

        int max = enemyMax - enemies.Count;
        int numSpawn = Random.Range(1, 4);
        spawnEnemyGroup(type, Mathf.Min(max,numSpawn));
    }

    void spawnEnemyGroup(int type, int numToSpawn)
    {

        for (int i = 0; i< numToSpawn; i++)
        {
            spawnEnemy(type);
        }
    }



    void giveEnemyControl(BaseEnemy e, TitleControlObjectScript control)
    {
        e.controlByPlayer(control);
        control.setEnemyControl(e, control_type_enemy);
    }

    void checkIfPlayerWantEnemyControl(BaseEnemy e)
    {
        List<TitleControlObjectScript> wants = getDeadPlayersForEnemyControl();

        if (wants.Count > 0)
        {
            int idx = Random.Range(0, wants.Count);
            giveEnemyControl(e, wants[idx]);
        }


    }

    void spawnEnemy(int etype)
    {
        Transform t = Instantiate(e_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        t.SetParent(transform.Find("Enemies"));
        BaseEnemy e_scr = t.GetComponent<BaseEnemy>();
        e_scr.setEntity(this);
        e_scr.setupEnemy(etype);
        
        e_scr.setDifficulty(difficulty);

        enemies.Add(e_scr);
        checkIfPlayerWantEnemyControl(e_scr);
    }

    void spawnHat()
    {
        Transform t = Instantiate(hat_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        t.SetParent(transform.Find("Throw"));

        BaseThrowable b_scr = t.GetComponent<BaseThrowable>();
        throws.Add(b_scr);

        BaseHat h_scr = t.GetComponent<BaseHat>();
        hats.Add(h_scr);


    }


    void checkSpawnHat()
    {
        if (hats.Count < hatMax)
        {
            spawnHat();
        }

    }

    void checkSpawnEnemy()
    {
        enemyRate = Random.Range(enemyRateMin, enemyRateMax);

        if (enemies.Count < enemyMax)
        {
            regularSpawn();
        }

    }

    void doEnemyTimer(float f)
    {
        enemyTimer += f;
        if (enemyTimer > enemyRate)
        {
            enemyTimer = 0;
            checkSpawnEnemy();
        }
    }

    public int getMyNum(PlayerMove pm)
    {
        int i = 0;
        foreach (PlayerMove p in players)
        {
            i++;
            if (pm = p)
            {
                return i;
            }
        }
        return 0;
    }



    void showPauseWindow()
    {
        m_ui.Find("Pause").gameObject.SetActive(true);


    }
    void hidePauseWindow()
    {
        m_ui.Find("Pause").gameObject.SetActive(false);
    }



    void movePauseSelectBox()
    {
        if (pauseOptionSelect == 0)
        {
            m_ui.Find("Pause").Find("Box").position = m_ui.Find("Pause").Find("Resume").position;
        }
        else if (pauseOptionSelect == 1)
        {
            m_ui.Find("Pause").Find("Box").position = m_ui.Find("Pause").Find("Exit").position;
        }
    }

    void changePauseOptionSelect(int amt)
    {
        int opts = 2;

        pauseOptionSelect += amt;
        if (pauseOptionSelect > opts-1)
        {
            pauseOptionSelect = 0;
        }
        else if (pauseOptionSelect < 0)
        {
            pauseOptionSelect = opts - 1;
        }

        movePauseSelectBox();
    }


    void pauseSelectMove(Vector2 move)
    {
        if (pauseMenuMoveTimer > 0.5)
        {
            pauseMenuMoveTimer = 0;
            if (move.y >= 0)
            {
                changePauseOptionSelect(1);
            }
            else if (move.y <= 0)
            {
                changePauseOptionSelect(-1);
            }
        }
    }


    public void takeInputMove(Vector2 move)
    {
        if (is_paused)
        {
            pauseSelectMove(move);

        }
    }

    public void takeInputFire()
    {



    }

    public void takeInputJump()
    {
        if (is_paused)
        {
            pauseConfirm();
        }
    }

    void pauseConfirm()
    {
        if (pauseOptionSelect == 0)
        {
            togglePause();
        }
        else if (pauseOptionSelect == 1)
        {
            exitGame();
        }
    }

    void exitGame()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }


    void gameWinnerChange()
    {
        audio_manager.stopAllSounds();
        SceneManager.LoadScene("WinnerScene", LoadSceneMode.Single);
    }


    int numAlivePlayers()
    {
        int p_num = 0;

        foreach(PlayerMove p in players)
        {
            if (p.getAlive())
            {
                p_num++;
            }
        }

        return p_num;
    }

    void CheckForGameEnd()
    {
        if (numAlivePlayers() == 0)
        {
            gameWinnerChange();
        }

    }

    public void stateChangePlayer()
    {
        updateHealth();
        
    }

    void updateHealth()
    {
        Transform hp_ui = m_ui.Find("Health");
        foreach (PlayerMove p in players)
        {
            string str_num = p.myNum().ToString();

            float f_hp = p.getHealth();

            string hp = "";

            if (f_hp < 1 && f_hp > 0)
            {
                hp = f_hp.ToString("F1");
            }
            else
            {
                hp = f_hp.ToString("F0");
            }
            

            if (hp_ui.Find(str_num))
            {
                hp_ui.Find(str_num).Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = hp;
            }
            
        }
    }

    void Start()
    {
        
    }

    void doHatTimer(float f)
    {
        hatTimer += f;
        if (hatTimer > hatRate)
        {
            hatTimer = 0;
            checkSpawnHat();
        }


    }


    void doTimers(float f)
    {
        background.update(f);
        doEnemyTimer(f);
        doHatTimer(f);
        doShake(f);
    }
    // Update is called once per frame
    void Update()
    {

        float f = Time.deltaTime;
        pauseTimer += f;
        pauseMenuMoveTimer += f;

        if (!is_paused)
        {
            doGlobalTimer(f);

            float fg = f * local_time;
            doTimers(fg);
        }
    }
}
