using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Theme
{
    public string name;
    public List<string> bgMusic;
    BackgroundEvent bg;


    // Start is called before the first frame update
    public void setup(LevelBackground l)
    {
        bg = loadEvent(name);
        bg.mainSetup(l);

    }

    public string getMusicName()
    {
        int i = Random.Range(0, bgMusic.Count);
        return bgMusic[i];
    }


    BackgroundEvent loadEvent(string name)
    {
        switch (name)
        {
            case "beach":
                return new BackgroundBeach();
            default:
                return new BackgroundBeach();
        }
    }

    public void update(float t)
    {
        bg.update(t);
    }


}
