
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
      Game,
      Menu,
}

public static class InputManager
{
      private static readonly Dictionary<ActionType, ISet<string>> ActionsByType =
      new Dictionary<ActionType, ISet<string>> {
            { ActionType.Game, new HashSet<string> {
                  "Mouse X", "Mouse Y", "Horizontal", "Vertical", "Sprint", "Roll", "Jump", "LightAttack", "HeavyAttack",
                  "Interact", "ToggleInventory"
            }},
            { ActionType.Menu, new HashSet<string> {
                  "ToggleInventory", "InventoryItemAction", "InventoryToggleEquipped", "MenuLeft", "MenuRight", "MenuUp",
                  "MenuDown"
            }}
      };

      public static ActionType CurrentActionType = ActionType.Game;

      public static bool GetButtonDown(string action)
      {
            if (!ActionsByType[CurrentActionType].Contains(action)) return false;
            return Input.GetButtonDown(action);
      }

      public static bool GetButton(string action)
      {
            if (!ActionsByType[CurrentActionType].Contains(action)) return false;
            return Input.GetButton(action);
      }

      public static float GetAxis(string axis)
      {
            if (!ActionsByType[CurrentActionType].Contains(axis)) return 0f;
            return Input.GetAxis(axis);
      }
}