using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{

    Animator legs;
    Animator arms;
    Animator equip;
    Animator torso;
    bool isPlaying;
    float playTimer;

    //float base_time_legs_walk = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        isPlaying = false;
        legs = transform.Find("Images").Find("legs").Find("animation_leg").GetComponent<Animator>();
        arms = transform.Find("Images").Find("arms").Find("animation_arm").GetComponent<Animator>();
        equip = transform.Find("Images").Find("equip").Find("animation_equip").GetComponent<Animator>();
    }

    public void setPlayTimer(float t)
    {
        isPlaying = true;
        playTimer = t;
        equip.SetBool("isPlaying", isPlaying);
    }

    public void setEquip(int i)
    {
        equip.SetInteger("equip",i);
    }

    void animationTimer(float t)
    {
        if (isPlaying)
        {
            playTimer -= t;
            if (playTimer <= 0)
            {
                playTimer = 0;
                isPlaying = false;
                equip.SetBool("isPlaying", isPlaying);
            }
        }
    }

    public void updateAnimationStateLegs(bool grounded, bool running, bool sliding)
    {
        legs.SetBool("grounded", grounded);
        legs.SetBool("running", running);
        legs.SetBool("sliding", sliding);
    }

    public void updateAnimationStateArms(int equip)
    {

    }

    public void updateAnimationStateEquip(int equip)
    {

    }

    public void updateAnimationTime(float t)
    {
        animationTimer(t);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
