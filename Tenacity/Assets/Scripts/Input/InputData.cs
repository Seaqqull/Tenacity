using UnityEngine.Events;
using System;


namespace Tenacity.Input.Data
{
    public enum HorizontalButton : byte
    {
        Nothing, Left, Right
    }
    public enum VerticalButton : byte
    {
        Nothing, Top = 4, Bottom = 8
    }

    public enum MouseButton
    {
        Left = 256, Middle = 512, Right = 1024
    }

    [Flags]
    public enum PressedButton
    {
        Nothing = 0, 
        LeftButton = HorizontalButton.Left, 
        RightButton = HorizontalButton.Right,
        TopButton = VerticalButton.Top, 
        BottomButton = VerticalButton.Bottom, 
        Interact = 16,
        Space = 32,
        Back = 64,
        Shift = 128,
        LeftMouseButton = MouseButton.Left,
        MiddleMouseButton = MouseButton.Middle,
        RightMouseButton = MouseButton.Right
    }
    
    [Serializable]
    public class KeyPressEvent : UnityEvent<PressedButton> {}
}