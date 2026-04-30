using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerAnim : MonoBehaviour
{
    //private const string IS_WALKING_LEFT = "isMovingLeft";
    //private const string IS_WALKING_RIGHT = "isMovingRight";
    //private const string IS_UNFOCUSED = "isAttackUnfocused"; // trigger version
    //private const string IS_UNFOCUSED1 = "isAttackUnfocus";
   // private const string IS_FOCUSED = "isAttackFocused";
    //private const string IS_SPECIAL = "isSpecial";
    public Animator animator;
    int m_BounceStateHash;
    int IS_WALKING_RIGHT;
    int IS_WALKING_LEFT;
    int IS_UNFOCUSED1;
    int IS_FOCUSED;
    int IS_SPECIAL;

    void Start()
    {
        animator = GetComponent<Animator>();
        //m_BounceStateHash = Animator.StringToHash("Base Layer.Bounce");
        IS_WALKING_RIGHT = Animator.StringToHash("isMovingRight");
        IS_WALKING_LEFT = Animator.StringToHash("isMovingLeft");
        IS_UNFOCUSED1 = Animator.StringToHash("isAttackUnfocus");
        IS_FOCUSED = Animator.StringToHash("isAttackFocused");
        IS_SPECIAL = Animator.StringToHash("isSpecial");
    }

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

        

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            animator.SetBool(IS_UNFOCUSED1, true);
        }
        else
        {
            animator.SetBool(IS_UNFOCUSED1, false);
        }
        if (Keyboard.current.cKey.isPressed)
        {
            animator.SetBool(IS_FOCUSED, true);
        }
        else
        {
            animator.SetBool(IS_FOCUSED, false);
        }

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            animator.SetBool(IS_SPECIAL, true);
        }
        else
        {
            animator.SetBool(IS_SPECIAL, false);
        }
    }

}
