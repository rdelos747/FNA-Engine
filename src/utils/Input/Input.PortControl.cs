using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Utils {

  public static partial class Input {

    private class PortControl {
      /*
      Properties
      */
      public InputType InputType = InputType.Keyboard;

      public Dictionary<string, Keys> KeyboardMap = new Dictionary<string, Keys>();
      public Dictionary<string, Buttons> GamePadMap = new Dictionary<string, Buttons>();

      public GamePadState GamePadState;
      public GamePadState LastGamePadState;

      /*
      Has action
      */
      public bool HasAction(string action) {
        return KeyboardMap.ContainsKey(action) || GamePadMap.ContainsKey(action);
      }

      /*
      Setting actions
      */
      public void SetKeyboardAction(string changeKey, Keys val) {
        if (InputType != InputType.Keyboard) {
          return;
        }
        string temp = null;
        foreach (string iterKey in KeyboardMap.Keys) {
          if (KeyboardMap[iterKey] == val) {
            temp = iterKey;
          }
        }
        if (temp != null) {
          KeyboardMap[temp] = KeyboardMap[changeKey];
        }
        KeyboardMap[changeKey] = val;
      }

      public void SetGamePadAction(string changeKey, Buttons val) {
        if (InputType != InputType.GamePad) {
          return;
        }

        string temp = null;
        foreach (string iterKey in GamePadMap.Keys) {
          if (GamePadMap[iterKey] == val) {
            temp = iterKey;
          }
        }
        if (temp != null) {
          GamePadMap[temp] = GamePadMap[changeKey];
        }
        GamePadMap[changeKey] = val;
      }

      /*
      Action down
      */
      public bool IsActionDown(string action) {
        if (InputType == InputType.GamePad) {
          return GamePadMap.ContainsKey(action) && GamePadState.IsButtonDown(GamePadMap[action]);
        }

        return KeyboardMap.ContainsKey(action) && KeyboardState.IsKeyDown(KeyboardMap[action]);
      }

      /*
      Action up
      */
      public bool IsActionUp(string action) {
        if (InputType == InputType.GamePad) {
          return GamePadMap.ContainsKey(action) && GamePadState.IsButtonDown(GamePadMap[action]);
        }

        return KeyboardMap.ContainsKey(action) && KeyboardState.IsKeyDown(KeyboardMap[action]);
      }

      /*
      Action pressed
      */
      public bool ActionPressed(string action) {
        if (InputType == InputType.GamePad) {
          return (
            GamePadMap.ContainsKey(action) &&
            GamePadState.IsButtonDown(GamePadMap[action]) &&
            !LastGamePadState.IsButtonDown(GamePadMap[action])
          );
        }

        return (
          KeyboardMap.ContainsKey(action) &&
          KeyboardState.IsKeyDown(KeyboardMap[action]) &&
          !LastKeyboardState.IsKeyDown(KeyboardMap[action])
        );
      }
    }
  }
}