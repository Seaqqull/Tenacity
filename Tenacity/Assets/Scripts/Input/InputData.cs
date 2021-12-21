using System;


namespace Tenacity.Input.Data
{
    public enum HorizontalButton : byte
    {
        Nothing, LeftButton, RightButton
    }
    public enum VerticalButton : byte
    {
        Nothing, TopButton = 4, BottomButton = 8
    }

    [Flags]
    public enum PressedButton : byte
    {
        Nothing = 0, 
        LeftButton = HorizontalButton.LeftButton, 
        RightButton = HorizontalButton.RightButton,
        TopButton = VerticalButton.TopButton, 
        BottomButton = VerticalButton.BottomButton, 
        Interact = 16,
        Space = 32,
        Back = 64,
        Shift = 128
    }
}