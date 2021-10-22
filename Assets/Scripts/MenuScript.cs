using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public Transform ui_menu;
    int numPlayers;
    int selectedItem;

    int maxPlayers = 4;

    public Transform prefab_input;

    //float controlDeadZone = 0.5f;

    string joinTxStart = "Joined as player";

    float menuSwitchTimer;
    float menuSwitchTimerMax = 0.3f;

    // Start is ca
    // lled before the first frame update
    void Start()
    {
        menuSwitchTimer = 0;
        numPlayers = 0;
        selectedItem = 0;
        addControlIfNotExists();
        swapControls();
        setupExistingPlayers();

        setupAudioManager();
    }

    void exitGame()
    {
        Application.Quit();
    }
    void setupAudioManager()
    {
        if (!GameObject.Find("AudioManagerEnabled"))
        {
            Transform b = GameObject.Find("AudioManagerBase").transform;

            Transform t = Instantiate(b, new Vector3(0, 0, 0), Quaternion.identity);
            t.name = "AudioManagerEnabled";
            DontDestroyOnLoad(t);
        }
    }

    public int getPlayer()
    {
        return numPlayers;
    }

    void addControlIfNotExists()
    {
        if (!GameObject.Find("Controls"))
        {
            GameObject c = new GameObject("Controls");

            DontDestroyOnLoad(c);
        }

        if (!GameObject.Find("MainInputManager"))
        {
            Transform t = Instantiate(prefab_input, new Vector3(0, 0, 0), Quaternion.identity);

            t.name = "MainInputManager";
            DontDestroyOnLoad(t);
        }

        GameObject inp = GameObject.Find("MainInputManager");
        inp.GetComponent<PlayerInputManager>().EnableJoining();

    }

    void swapControls()
    { 
            Transform playerParent = GameObject.Find("Controls").transform;

            foreach (Transform child in playerParent)
            {
                numPlayers++;
                child.GetComponent<TitleControlObjectScript>().menuGameStart(this);

            }
    }


    void setupExistingPlayers()
    {
        Transform ps = GameObject.Find("Controls").transform;
        if (ps)
        {
            foreach (Transform child in ps)
            {
                int i;
                bool success = int.TryParse(child.name, out i);
                if (success)
                {
                    setupPlayer(i);
                }
            }
        }
    }

    void setupPlayer(int pnum)
    {
        string p = pnum.ToString();
        Transform ps = ui_menu.Find("PlayerSpots").Find(p);
        string tx_start = joinTxStart + " " + p;
        ps.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = tx_start;
    }

    public void addPlayer(TitleControlObjectScript t)
    {
        if (numPlayers < maxPlayers)
        {
            numPlayers++;
            setupPlayer(numPlayers);
        }
    }


    void hideSelectBoxes()
    {
        Transform box_s = ui_menu.Find("Options").Find("StartGame");
        box_s.Find("Highlight").gameObject.SetActive(false);

        Transform box_q = ui_menu.Find("Options").Find("Quit");
        box_q.Find("Highlight").gameObject.SetActive(false);
    }




    void refreshSelectedItem()
    {
        hideSelectBoxes();
        switch (selectedItem)
        {
            case 1:
                Transform box_s = ui_menu.Find("Options").Find("StartGame");
                box_s.Find("Highlight").gameObject.SetActive(true);
                break;
            case 2:
                Transform box_q = ui_menu.Find("Options").Find("Quit");
                box_q.Find("Highlight").gameObject.SetActive(true);
                break;

        }

    }


    void menuControlMove(Vector2 i_movement)
    {
        int maxSelected = 2;


        if (i_movement.y != 0 && menuSwitchTimer > menuSwitchTimerMax)
        {
            menuSwitchTimer = 0;

            if (i_movement.y <= 0)
            {
                selectedItem = selectedItem - 1;
                if (selectedItem < 0)
                {
                    selectedItem = maxSelected;
                }
            }

            if (i_movement.y >= 0)
            {
                selectedItem = selectedItem + 1;
                if (selectedItem > maxSelected)
                {
                    selectedItem = 0;
                }
            }

        }

        refreshSelectedItem();
    }


    void loadStartGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }


    void menuControlSelect(float f)
    {
        if (f >= 1.0)
        {
            switch (selectedItem)
            {
                case 1:
                    loadStartGame();
                    break;
                case 2:
                    exitGame();
                    break;
            }
        }
    }


    public void OnMove(Vector2 i_movement, int playerSentBy)
    {
        if (playerSentBy == 1)
        {
            menuControlMove(i_movement);
        }
    }

    public void OnJump(float i_movement, int playerSentBy)
    {
        if (playerSentBy == 1)
        {
            menuControlSelect(i_movement);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float f = Time.deltaTime;

        menuSwitchTimer += f;
    }
}
