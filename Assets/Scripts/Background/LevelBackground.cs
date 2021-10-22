using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBackground : MonoBehaviour
{


    Theme currentTheme;
    public List<Theme> allThemes;

    EntityList entity_parent;

    string playingMusic;


    // Start is called before the first frame update
    void Start()
    {

    }


    void pickTheme()
    {
        int i = Random.Range(0, allThemes.Count);
        currentTheme = allThemes[i];

    }
    
    public List<float> levelBounds()
    {
        return entity_parent.levelBounds();
    }

    public void startBackground()
    {
        pickTheme();
        entity_parent = transform.GetComponent<EntityList>();
        currentTheme.setup(this);
        playMusic(currentTheme.getMusicName());
    }

    public void stopMusic()
    {
        entity_parent.stopSoundContinue(playingMusic);
    }

    public void playMusic(string s)
    {
        playingMusic = s;
        entity_parent.playSoundContinue(s);
    }

    public void update(float t)
    {
        currentTheme.update(t);
    }


}
