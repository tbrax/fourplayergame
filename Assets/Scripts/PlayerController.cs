using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMove m_Character;

    Vector2 i_movement;
    bool i_jump;
    bool i_fire;
    bool i_fire2;
    bool i_swap;

    private void Awake()
    {
        m_Character = GetComponent<PlayerMove>();
    }


    public void setTitleControl(TitleControlObjectScript t)
    {
        m_Character.setTitleControl(t);
    }

    public void colorPlayer(Color c)
    {
        Color black = new Color(0, 0, 0);
        transform.Find("Images").Find("arms").Find("animation_arm").GetComponent<SpriteRenderer>().color = c;
        transform.Find("Images").Find("body").GetComponent<SpriteRenderer>().color = c;
        transform.Find("Images").Find("legs").Find("animation_leg").GetComponent<SpriteRenderer>().color = black;
    }

    private void FixedUpdate()
    {

        m_Character.Move(i_movement, i_jump, i_fire, i_swap, i_fire2);
        i_jump = false;
        i_swap = false;
        i_fire = false;
        i_fire2 = false;
    }

    public void OnMove(Vector2 value)
    {
        //i_movement = value.Get<Vector2>();
        i_movement = value;
    }

    public void OnJump(float jmp)
    {
        if (jmp >= 1.0)
        {
            i_jump = true;
        }
        else
        {
            i_jump = false;
        }
    }

    public void OnFire(float jmp)
    {
        if (jmp >= 1.0)
        {
            i_fire = true;
        }
        else
        {
            i_fire = false;
        }
    }

    public void OnSwap(float jmp)
    {
        if (jmp >= 1.0)
        {
            i_swap = true;
        }
        else
        {
            i_swap = false;
        }
    }

    public void OnFire2(float jmp)
    {
        if (jmp >= 1.0)
        {
            i_fire2 = true;
        }
        else
        {
            i_fire2 = false;
        }
    }

}
