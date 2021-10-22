using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //Public Variables
    [Header("Transform Variables")]
    public float RunSpeed = 0.1f;
    public float TurnSpeed = 6.0f;


    Animator animator;

    void Start()
    {
        /**
        * Initialize the animator that is attached on the current game object i.e. on which you will attach this script.
        */
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        /**
        * The Update() function will get the bool parameters from the animator state machine and set the values provided by the user.
        * Here, I have only added animation for Run and Idle. When the Up key is pressed, Run animation is played. When we let go, Idle is played.
        */

        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("PerformRun", true);
            animator.SetBool("PerformIdle", false);
        }
        else
        {
            animator.SetBool("PerformRun", false);
            animator.SetBool("PerformIdle", true);
        }
    }

    void OnAnimatorMove()
    {
        /**
         * OnAnimatorMove() function will shadow the "Apply Root Motion" on the animator. Your game objects psoition will now be determined 
         * using this fucntion.
         */
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * RunSpeed);
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(Vector3.up * Time.deltaTime * TurnSpeed);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(-Vector3.up * Time.deltaTime * TurnSpeed);
            }
        }

    }



}
