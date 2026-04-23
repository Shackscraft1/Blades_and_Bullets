// using UnityEngine;
// using UnityEngine.InputSystem;

// public class GameInput : MonoBehaviour
// {
//     public static GameInput Instance {get; private set;}
//     private InputSystem_Actions inputSystem_Actions;

//     private void Awake()
//     {
//         Instance = this;
//         inputSystem_Actions = new InputSystem_Actions();
//         inputSystem_Actions.Enable();
//     }

//     public Vector2 GetMovementVectorNormalized()
//     {
//         Vector2 inputVector = InputSystem_Actions.Player.Move.ReadValue<Vector2>();
//         inputVector = inputVector.normalized;
//         return inputVector;
//     }

// }
