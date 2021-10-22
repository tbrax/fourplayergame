using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GenericController : MonoBehaviour
{
    public Transform p_prefab;

    private PlayerController playerControl;

    private BaseEnemy enemyControl;

    private int myControl = 1;

    void spawnPlayer()
    {
        Transform t = Instantiate(p_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        playerControl = t.GetComponent<PlayerController>();
    }

    void startGame()
    {
        spawnPlayer();
    }

    private void Start()
    {
        startGame();
    }


    void OnMove(InputValue value)
    {
        Vector2 i_movement = value.Get<Vector2>();

        if (playerControl)
        {

            switch (myControl)
            {
                case 1:
                    playerControl.OnMove(i_movement);
                    break;

            }
        } 
    }

    void OnJump(InputValue value)
    {
        float jmp = value.Get<float>();

        if (playerControl)
        {
            switch (myControl)
            {
                case 1:
                    playerControl.OnJump(jmp);
                    break;
            }
        }
    }

    void OnFire(InputValue value)
    {
        float jmp = value.Get<float>();
    }

    void OnSwap(InputValue value)
    {
        float jmp = value.Get<float>();
    }

}
