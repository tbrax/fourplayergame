using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public List<Sound> sounds;

    public int audioLimit = 10;

    [Range(0f, 1f)]
    public float sound_global;

    [Range(0f, 1f)]
    public float sound_effect;

    [Range(0f, 1f)]
    public float sound_music;


    const int sound_type_effect = 0;
    const int sound_type_music = 1;

    List<string> continousSounds;
    List<int> continousSoundsNums;

    void makeSoundHolder(string name, int amt, Sound s)
    {
        GameObject baseAudio = new GameObject(name);
        baseAudio.transform.SetParent(transform);

        for (int i = 0; i < amt; i++)
        {
            GameObject baseAudioNum = new GameObject(i.ToString());
            baseAudioNum.transform.SetParent(baseAudio.transform);

            AudioSource sd = baseAudioNum.AddComponent<AudioSource>();
            sd.playOnAwake = false;
            sd.clip = s.clip;
            sd.volume = s.volume;
            sd.pitch = s.pitch;
            sd.loop = s.loop;
            s.sourceList.Add(sd);

        }
    }


    void makeSounds()
    {
        foreach (Sound s in sounds)
        {
            makeSoundHolder(s.name, s.amount, s);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        makeSounds();
        applySoundVolume();
        continousSounds = new List<string>();
        continousSoundsNums = new List<int>();
    }


    public void stopAllSounds()
    {

        foreach (Sound s in sounds)
        {
            foreach (AudioSource auds in s.sourceList)
            {
                auds.Stop();
            }
        }

        for (int i = 0; i < continousSounds.Count; i++)
        {
            continousSoundsNums[i] = 0;
        }

    }

    void applySoundVolume()
    {
        foreach (Transform i in transform)
        {
            string title = i.name;
            Sound s = getSoundWithName(title);

            int type = s.type;

            float vol = sound_global * s.volume;

            if (type == sound_type_effect)
            {
                vol *= sound_effect;
            }
            else if (type == sound_type_music)
            {
                vol *= sound_music;
            }

            if (s != null)
            {
                foreach (Transform j in i)
                {
                    j.GetComponent<AudioSource>().volume = vol;
                }
            }
        }
    }


    Sound getSoundWithName(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                return s;
            }
        }
        return null;
    }

    void getFreeSourceAndPlay(Sound s)
    {
        bool played = false;
        int i = 0;
        int max = s.sourceList.Count;
        while (!played && i < max)
        {
            if (!s.sourceList[i].isPlaying)
            {
                played = true;
                s.sourceList[i].Play();
            }
            i++;
        }
    }

    void stopPlayingSound(Sound s)
    {
        foreach (AudioSource auds in s.sourceList)
        {
            if (auds.isPlaying)
            {
                auds.Stop();
            }
        }
    }



    int getIndexOfContSound(string name)
    {
        if (continousSounds.IndexOf(name) == -1)
        {
            continousSounds.Add(name);
            continousSoundsNums.Add(0);
        }

        return continousSounds.IndexOf(name);
    }

    public void playContinue(string name)
    {
        Sound sn = getSound(name);
        if (sn != null)
        {
            int i = getIndexOfContSound(name);
            if (continousSoundsNums[i] == 0)
            {
                getFreeSourceAndPlay(sn);
            }
            continousSoundsNums[i] += 1;
        }
    }

    public void stopContinue(string name)
    {
        Sound sn = getSound(name);
        if (sn != null)
        {
            int i = getIndexOfContSound(name);
            continousSoundsNums[i] -= 1;
            if (continousSoundsNums[i] <= 0)
            {
                continousSoundsNums[i] = 0;
                stopPlayingSound(sn);
            }
        }
    }


    Sound getSound(string name)
    {
        Sound sn = null;
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                sn = s;
            }
        }
        return sn;
    }

    public void Play(string name)
    {
        Sound sn = getSound(name);
        if (sn != null)
        {
            getFreeSourceAndPlay(sn);
        }
    }
}
