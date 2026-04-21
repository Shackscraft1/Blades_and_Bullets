using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAnim : MonoBehaviour
{
    private const string IS_WALKING_LEFT = "isMovingLeft";
    private const string IS_WALKING_RIGHT = "isMovingRight";
    private const string IS_WALKING_UP = "isMovingUp";
    private const string IS_WALKING_DOWN = "isMovingDown";
    public Animator animator;

    void Update()
    {
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            animator.SetBool(IS_WALKING_RIGHT, true);
        }
        else
        {
            animator.SetBool(IS_WALKING_RIGHT, false);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            animator.SetBool(IS_WALKING_LEFT, true);
        }
        else
        {
            animator.SetBool(IS_WALKING_LEFT, false);
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            animator.SetBool(IS_WALKING_UP, true);
        }
        else
        {
            animator.SetBool(IS_WALKING_UP, false);
        }

        if (Keyboard.current.downArrowKey.isPressed)
        {
            animator.SetBool(IS_WALKING_DOWN, true);
        }
        else
        {
            animator.SetBool(IS_WALKING_DOWN, false);
        }
    }

}
