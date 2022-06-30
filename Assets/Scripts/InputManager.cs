
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
      public static PlayerInput Input;

      public void Awake()
      {
            Input = GetComponent<PlayerInput>();
      }

      public static InputAction Action(string name)
      {
            return Input.actions[name];
      }
}