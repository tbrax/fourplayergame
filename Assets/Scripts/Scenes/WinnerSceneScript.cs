using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WinnerSceneScript : MonoBehaviour
{

    List<TitleControlObjectScript> title_controllers;

    // Start is called before the first frame update
    void Start()
    {
        swapControls();
        getTitleControllers();
        setupStats();
    }



    static int SortByTime(TitleControlObjectScript p1, TitleControlObjectScript p2)
    {
        return p1.getDeathTime().CompareTo(p2.getDeathTime());
    }

    void setupStats()
    {
        hideAllPlayerSpots();

        int numPlayers = 0;

        List<TitleControlObjectScript> orderControl = new List<TitleControlObjectScript>();

        foreach (TitleControlObjectScript t in title_controllers)
        {
            numPlayers++;
            showPlayerSpot(numPlayers, true);
            orderControl.Add(t);

        }

        orderControl.Sort(SortByTime);

        int rankPlace = 0;
        for (int i = orderControl.Count-1; i >= 0; i--)
        {
            rankPlace++;
            Transform rankSpot = getRankSpot(rankPlace);
            rankSpot.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = orderControl[i].getName();
            float deathTime = orderControl[i].getDeathTime();
            string deathText = deathTime.ToString("F2") + " seconds";
            rankSpot.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = deathText;
        }

    }

    Transform getRankSpot(int i)
    {
        return GameObject.Find("Canvas").transform.Find("Ranking").Find(i.ToString());
    }

    void hideAllPlayerSpots()
    {
        for (int i = 1; i < 5; i++)
        {
            showPlayerSpot(i, false);
        }
    }

    void showPlayerSpot(int i, bool toHide)
    {
        Transform sp = getRankSpot(i);

        if (sp)
        {
            sp.gameObject.SetActive(toHide);
        }

    }

    void swapControls()
    {
        Transform playerParent = GameObject.Find("Controls").transform;

        foreach (Transform child in playerParent)
        {
            child.GetComponent<TitleControlObjectScript>().winnerGameStart(this);
        }
    }


    void loadMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void endScreen()
    {
        loadMenu();
    }

    public void getTitleControllers()
    {

        title_controllers = new List<TitleControlObjectScript>();
        Transform ps = GameObject.Find("Controls").transform;
        if (ps)
        {
            foreach (Transform child in ps)
            {
                if (child.GetComponent<TitleControlObjectScript>())
                {
                    title_controllers.Add(child.GetComponent<TitleControlObjectScript>());
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
