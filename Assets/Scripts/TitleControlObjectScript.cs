using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class TitleControlObjectScript : MonoBehaviour
{

    MenuScript menu_parent;
    int my_player = 0;


    int my_control = 0;

    const int control_type_enemy = 3;
    const int control_type_player = 1;


    Transform p_prefab;
    private PlayerController playerControl;

    BaseEnemy enemy_control;

    int scene_state = 0;

    Color my_color;

    EntityList entity_parent;

    int score;
    float deathTime;


    Transform showIcon;

    WinnerSceneScript winner_parent;

    public int getMyControl()
    {
        return my_control;
    }


    void setShowIcon(Transform t)
    {
        
        showIcon = t;
    } 


    void colorShowIcon()
    {

        showIcon.Find("Arrow").GetComponent<Image>().color = my_color;
    }

    void findAndSetShowIcon()
    {

        string n = my_player.ToString();
        Transform find = GameObject.Find("UI").transform.Find("Canvas").Find("Icon").Find("PlayerIcons").Find(n);
        if (find != null)
        {
            setShowIcon(find);
            colorShowIcon();
        }

    }

    public void setMyControl(int i)
    {
        my_control = i;
    }

    public void setEnemyControl(BaseEnemy e, int controlType)
    {
        enemy_control = e;
        my_control = controlType;
    }

    public BaseEnemy getEnemyControl()
    {
        return enemy_control;
    }

    public string getName()
    {
        return "Player " + my_player.ToString();
    }


    public int getMyPlayer()
    {
        return my_player;
    }

    void spawnPlayerPrefab()
    {
        Transform t = Instantiate(p_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        playerControl = t.GetComponent<PlayerController>();

        //entity_parent.addPlayer(t.GetComponent<PlayerMove>());
    }

    public void spawnMyPlayer()
    {
        spawnPlayerPrefab();
        
    }

    public float getDeathTime()
    {
        return deathTime;
    }

    public void setDeathTime(float f)
    {
        deathTime = f;
    }

    public void menuGameStart(MenuScript m)
    {
        menu_parent = m;

        scene_state = 0;
        if (scene_state == 0)
        {
            
        }

        my_control = 0;

    }

    public void mainGameStart(EntityList e)
    {
        
        my_control = control_type_player;
        entity_parent = e;
        scene_state = 1;
        spawnMyPlayer();
        setupPlayer();
        findAndSetShowIcon();
        hideIcon();
    }


    public void winnerGameStart(WinnerSceneScript m)
    {
        winner_parent = m;

        scene_state = 2;

        my_control = 2;

    }

    void Awake()
    {
        scene_state = 0;
        p_prefab = Resources.Load<Transform>("Prefabs/Player");
        GameObject e = GameObject.Find("Menu");
        menu_parent = e.GetComponent<MenuScript>();

        menu_parent.addPlayer(this);

        my_player = menu_parent.getPlayer();

        GameObject c = GameObject.Find("Controls");
        transform.SetParent(c.transform);
        transform.name = my_player.ToString();
        my_control = 0;


        chooseColor();
    }


    void setupPlayer()
    {
        playerControl.setTitleControl(this);

        colorPlayer();
    }


    void chooseColor()
    {
        switch (my_player)
        {
            case 1:
                my_color = new Color(1, 0, 0);
                break;
            case 2:
                my_color = new Color(0, 0, 1);
                break;
            case 3:
                my_color = new Color(0, 1, 0);
                break;
            case 4:
                my_color = new Color(1, 1, 0);
                break;
            default:
                my_color = new Color(1, 1, 1);
                break;
        }
    }

    void colorPlayer()
    {
        playerControl.colorPlayer(my_color);
    }


    void OnMove(InputValue value)
    {
        Vector2 i_movement = value.Get<Vector2>();


        if (entity_parent)
        {
            entity_parent.takeInputMove(i_movement);
        }
        
        switch (my_control)
            {
                case 0:
                    menu_parent.OnMove(i_movement, my_player);
                    break;
                case control_type_player:
                    playerControl.OnMove(i_movement);
                    
                    break;
                case control_type_enemy:
                        enemy_control.OnMove(i_movement);
                    break;

        }
    }

    void OnJump(InputValue value)
    {
        float jmp = value.Get<float>();

        

        if (entity_parent)
        {
            entity_parent.takeInputJump();
        }

        switch (my_control)
            {
                case 0:
                    menu_parent.OnJump(jmp, my_player);
                    break;
                case control_type_player:
                    playerControl.OnJump(jmp);
                    
                    break;
                case 2:
                    winner_parent.endScreen();
                    break;
                case control_type_enemy:
                    enemy_control.OnJump(jmp);
                    break;
        }
    }

    void OnSwap(InputValue value)
    {
        float jmp = value.Get<float>();
        switch (my_control)
        {
            case control_type_player:
                playerControl.OnSwap(jmp);
                break;
            case control_type_enemy:
                enemy_control.OnSwap(jmp);
                break;
        }
    }

    void OnFire(InputValue value)
    {
        
        float jmp = value.Get<float>();
        switch (my_control)
        {
            case control_type_player:
                playerControl.OnFire(jmp);
                break;
            case control_type_enemy:
                enemy_control.OnFire(jmp);
                break;
        }
    }

    void OnFire2(InputValue value)
    {
        float jmp = value.Get<float>();
        switch (my_control)
        {
            case control_type_player:
                playerControl.OnFire2(jmp);
                break;
            case control_type_enemy:
                enemy_control.OnFire2(jmp);
                break;
        }
    }

    private void OnPause(InputValue value)
    {
        
        float jmp = value.Get<float>();
        
        if (jmp > 0)
        {
            if (entity_parent)
            {
                entity_parent.togglePause();
            }
        }


    }


    void hideIcon()
    {
        showIcon.position = new Vector3(500, 500, 0);
    }

    void moveIconIfNeeded()
    {
        if (my_control == control_type_enemy)
        {
            showIcon.position = enemy_control.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveIconIfNeeded();
    }
}
