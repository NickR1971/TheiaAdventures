using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public enum MyButton
{
    CrossUp=0, CrossDown=1, CrossLeft=2, CrossRight=3,
    Y=4, A=5, B=6, X=7,
    LStick=8, Rstick=9,
    LBumper=10, RBumper=11,
    LTrigger=12, RTrigger=13,
    Select=14, Start=15
}

public interface IInputController : IService
{
    bool IsPressed(MyButton _code);
    bool IsPressed(Key _key);
    void GetLeftStick(out float _horizontal, out float _vertical);
    void GetRightStick(out float _horizontal, out float _vertical);
    bool IsPressedEnter();
    bool IsPressedEscape();
}

public class MyInputController : MonoBehaviour, IInputController
{
    private Gamepad gamepad;
    private Keyboard keyboard;
    private readonly GamepadButton[] buttons = 
    {
        GamepadButton.DpadUp, GamepadButton.DpadDown, GamepadButton.DpadLeft, GamepadButton.DpadRight,
        GamepadButton.Y, GamepadButton.A, GamepadButton.B, GamepadButton.X,
        GamepadButton.LeftStick, GamepadButton.RightStick,
        GamepadButton.LeftShoulder, GamepadButton.RightShoulder,
        GamepadButton.LeftTrigger, GamepadButton.RightTrigger,
        GamepadButton.Select, GamepadButton.Start
    };

    private readonly Key[] keys =
    {
        Key.UpArrow, Key.DownArrow, Key.LeftArrow, Key.RightArrow,
        Key.Space, Key.Enter, Key.Escape, Key.Backspace,
        Key.LeftShift, Key.Numpad5,
        Key.Tab, Key.Backslash,
        Key.Q, Key.E,
        Key.End, Key.Home
    };

    private void Start()
    {
        keyboard = Keyboard.current;
        if (keyboard == null) Debug.Log("No keyboard found");

        gamepad = Gamepad.current;
        if (gamepad == null) Debug.Log("No gamepad found");
        else
        {
            Debug.Log(gamepad.name);
        }
    }

    //-------------------------------------
    // IInputController

    public bool IsPressed(MyButton _code)
    {
        bool rt = false;
        if (gamepad != null)
        {
            if (gamepad[buttons[(int)_code]].wasPressedThisFrame) rt = true;
        }
        if (keyboard != null)
        {
            if (keyboard[keys[(int)_code]].wasPressedThisFrame) rt = true;
        }
        return rt;
    }
    public bool IsPressed(Key _key) => keyboard[_key].wasPressedThisFrame;

    public void GetLeftStick(out float _horizontal, out float _vertical)
    {
        _horizontal = 0;
        _vertical = 0;

        if (gamepad != null)
        {
            Vector2 v;

            v = gamepad.leftStick.ReadValue();
            _horizontal = v.x;
            _vertical = v.y;
        }

        if (keyboard[Key.A].IsPressed()) _horizontal = -1;
        if (keyboard[Key.D].IsPressed()) _horizontal = 1;    
        if (keyboard[Key.S].IsPressed()) _vertical = -1;
        if (keyboard[Key.W].IsPressed()) _vertical = 1;

    }

    public void GetRightStick(out float _horizontal, out float _vertical)
    {
        _horizontal = 0;
        _vertical = 0;
        if (gamepad != null)
        {
            Vector2 v;

            v = gamepad.rightStick.ReadValue();
            _horizontal = v.x;
            _vertical = v.y;
        }

        if (keyboard[Key.Numpad4].IsPressed()) _horizontal = -1;
        if (keyboard[Key.Numpad6].IsPressed()) _horizontal = 1;
        if (keyboard[Key.Numpad2].IsPressed()) _vertical = -1;
        if (keyboard[Key.Numpad8].IsPressed()) _vertical = 1;

    }

    public bool IsPressedEnter()
    {
        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) return true;

        if (keyboard[Key.Enter].wasPressedThisFrame) return true;
        if (keyboard[Key.NumpadEnter].wasPressedThisFrame) return true;

        if (IsPressed(MyButton.A)) return true;

        return false;
    }

    public bool IsPressedEscape()
    {
        //if (Input.GetKeyDown(KeyCode.Escape)) return true;

        if (keyboard[Key.Escape].wasPressedThisFrame) return true;

        if (IsPressed(MyButton.B)) return true;

        return false;
    }
}
