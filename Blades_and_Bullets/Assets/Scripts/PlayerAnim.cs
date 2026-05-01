using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAnim : MonoBehaviour
{
    private const string IS_WALKING_LEFT = "isMovingLeft";
    private const string IS_WALKING_RIGHT = "isMovingRight";
    private const string IS_UNFOCUSED = "isAttackUnfocused"; // trigger version
    private const string IS_UNFOCUSED1 = "isAttackUnfocus";
    private const string IS_FOCUSED = "isAttackFocused";
    private const string IS_SPECIAL = "isSpecial";
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

        if (Keyboard.current.spaceKey.isPressed && Keyboard.current.shiftKey.isPressed)
        {
            animator.SetBool(IS_FOCUSED, true);
        }
        else
        {
            animator.SetBool(IS_FOCUSED, false);
        }

        if (Keyboard.current.spaceKey.isPressed && !Keyboard.current.shiftKey.isPressed)
        {
            animator.SetBool(IS_UNFOCUSED1, true);
        }
        else
        {
            animator.SetBool(IS_UNFOCUSED1, false);
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            animator.SetBool(IS_SPECIAL, true);
        }
        else
        {
            animator.SetBool(IS_SPECIAL, false);
        }

        if (Player.Instance.moveState == Player.MoveState.Death)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        } else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

}
