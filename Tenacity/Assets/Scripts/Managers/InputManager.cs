using Tenacity.Input.Data;
using Tenacity.Base;
using System;
using Tenacity.Input;
using UnityEngine;


namespace Tenacity.Managers
{
    public class InputManager : SingleBehaviour<InputManager>
    {
        [UnityEngine.SerializeField]
        private Input.Data.PressedButton _pressedButtons;

        // Specific buttons
        private static Action<bool> _mouseLeftButtonAction;
        private static Action<bool> _interactButtonAction;
        private static Action<bool> _spaceButtonAction;
        private static Action<bool> _shiftButtonAction;
        private static Action<bool> _backButtonAction;

        // Base buttons
        private static Action<Input.Data.HorizontalButton> _horizontalButtonAction;
        private static Action<Input.Data.VerticalButton> _verticalButtonAction;
        
        public static event Action<bool> MouseLeftButtonAction
        {
            add => _mouseLeftButtonAction += value;
            remove => _mouseLeftButtonAction -= value;
        }
        public static event Action<Input.Data.HorizontalButton> HorizontalButtonAction
        {
            add => _horizontalButtonAction += value;
            remove => _horizontalButtonAction -= value;
        }
        public static event Action<Input.Data.VerticalButton> VerticalButtonAction
        {
            add => _verticalButtonAction += value;
            remove => _verticalButtonAction -= value;
        }
        public static event Action<bool> InteractionButtonAction
        {
            add => _interactButtonAction += value;
            remove => _interactButtonAction -= value;
        }
        public static event Action<bool> SpaceButtonAction
        {
            add => _spaceButtonAction += value;
            remove => _spaceButtonAction -= value;
        }
        public static event Action<bool> BackButtonAction
        {
            add => _backButtonAction += value;
            remove => _backButtonAction -= value;
        }
        
        public static bool BottomButtonPressed
        {
            get => (Instance._pressedButtons & Input.Data.PressedButton.BottomButton) != 0;
        }
        public static bool RightButtonPressed
        {
            get => (Instance._pressedButtons & Input.Data.PressedButton.RightButton) != 0;
        }
        public static bool LeftButtonPressed
        {
            get => (Instance._pressedButtons & Input.Data.PressedButton.LeftButton) != 0;
        }
        public static bool TopButtonPressed
        {
            get => (Instance._pressedButtons & Input.Data.PressedButton.TopButton) != 0;
        }
        
        public static bool InteractionButtonPressed
        {
            get => (Instance._pressedButtons & Input.Data.PressedButton.Interact) != 0;
        }
        public static bool SpaceButtonPressed
        {
            get => (Instance._pressedButtons & Input.Data.PressedButton.Space) != 0;
        }
        public static bool ShiftButtonPressed
        {
            get => (Instance._pressedButtons & Input.Data.PressedButton.Shift) != 0;
        }
        public static bool BackButtonPressed
        {
            get => (Instance._pressedButtons & Input.Data.PressedButton.Back) != 0;
        }
        public static bool RightMousePressed
        {
            get => OrdinalInputHandler.Instance.RightMouseButton;
        }
        public static bool LeftMousePressed
        {
            get => OrdinalInputHandler.Instance.LeftMouseButton;
        }

        public static Vector2 MousePosition
        {
            get => OrdinalInputHandler.Instance.MousePosition;
        }


        private void Update()
        {
            // Base buttons
            var horizontal = (UnityEngine.Mathf.Approximately(Input.OrdinalInputHandler.Instance.Horizontal, 1.0f)) ? 
                HorizontalButton.Right : 
                UnityEngine.Mathf.Approximately(Input.OrdinalInputHandler.Instance.Horizontal, -1.0f) ?
                    HorizontalButton.Left :
                    HorizontalButton.Nothing;
            var horizontalNum = (int)horizontal;
            var vertical = (UnityEngine.Mathf.Approximately(Input.OrdinalInputHandler.Instance.Vertical, 1.0f)) ? 
                VerticalButton.Top : 
                UnityEngine.Mathf.Approximately(Input.OrdinalInputHandler.Instance.Vertical, -1.0f) ?
                    VerticalButton.Bottom :
                    VerticalButton.Nothing;
            var verticalNum = (int)vertical;

            if (!InputHasValue(horizontalNum))
            {
                // If previously input had horizontal value 
                if((horizontal == HorizontalButton.Nothing) && InputHasValue((int)HorizontalButton.Right))
                {
                    _pressedButtons = SwitchInput((int)HorizontalButton.Right);
                    _horizontalButtonAction?.Invoke(horizontal);
                }
                else if((horizontal == HorizontalButton.Nothing) && InputHasValue((int)HorizontalButton.Left))
                {
                    _pressedButtons = SwitchInput((int)HorizontalButton.Left);
                    _horizontalButtonAction?.Invoke(horizontal);
                }
                else if((horizontal == HorizontalButton.Right))
                {
                    // if(InputHasValue((int)VerticalButton.Bottom))
                    //     _pressedButtons = SwitchInput((int)(HorizontalButton.Right | HorizontalButton.Left));
                    // else
                        _pressedButtons = AddInput((int)HorizontalButton.Right);
                    _horizontalButtonAction?.Invoke(horizontal);
                }
                else if((horizontal == HorizontalButton.Left))
                {
                    // if(InputHasValue((int)VerticalButton.Bottom))
                    //     _pressedButtons = SwitchInput((int)(HorizontalButton.Right | HorizontalButton.Left));
                    // else
                        _pressedButtons = AddInput((int)HorizontalButton.Left);
                    _horizontalButtonAction?.Invoke(horizontal);
                }
            }
            if (!InputHasValue(verticalNum))
            {
                // If previously input had vertical value 
                if((vertical == VerticalButton.Nothing) && InputHasValue((int)VerticalButton.Top))
                {
                    _pressedButtons = SwitchInput((int)VerticalButton.Top);
                    _verticalButtonAction?.Invoke(vertical);
                }
                else if((vertical == VerticalButton.Nothing) && InputHasValue((int)VerticalButton.Bottom))
                {
                    _pressedButtons = SwitchInput((int)VerticalButton.Bottom);
                    _verticalButtonAction?.Invoke(vertical);
                }
                else if((vertical == VerticalButton.Top))
                {
                    if(InputHasValue((int)VerticalButton.Bottom))
                        _pressedButtons = SwitchInput((int)(VerticalButton.Top | VerticalButton.Bottom));
                    else
                        _pressedButtons = AddInput((int)VerticalButton.Top);
                    _verticalButtonAction?.Invoke(vertical);
                }
                else if((vertical == VerticalButton.Bottom))
                {
                    if(InputHasValue((int)VerticalButton.Bottom))
                        _pressedButtons = SwitchInput((int)(VerticalButton.Top | VerticalButton.Bottom));
                    else
                        _pressedButtons = AddInput((int)VerticalButton.Bottom);
                    _verticalButtonAction?.Invoke(vertical);
                }
            }
            
            // Special buttons
            var interaction = Input.OrdinalInputHandler.Instance.E;
            var space = Input.OrdinalInputHandler.Instance.Space;
            var shift = Input.OrdinalInputHandler.Instance.Shift;
            var back = Input.OrdinalInputHandler.Instance.Esc;

            if (interaction != InputHasValue((int)PressedButton.Interact))
            {
                _pressedButtons = SwitchInput((int)PressedButton.Interact);
                _interactButtonAction?.Invoke(interaction);
            }
            if (space != InputHasValue((int)PressedButton.Space))
            {
                _pressedButtons = SwitchInput((int)PressedButton.Space);
                _spaceButtonAction?.Invoke(space);
            }
            if (back != InputHasValue((int)PressedButton.Back))
            {
                _pressedButtons = SwitchInput((int)PressedButton.Back);
                _backButtonAction?.Invoke(back);
            }
            if (shift != InputHasValue((int)PressedButton.Shift))
            {
                _pressedButtons = SwitchInput((int)PressedButton.Shift);
                _shiftButtonAction?.Invoke(shift);
            }
            
            // Mouse
            var mouseLeft = Input.OrdinalInputHandler.Instance.LeftMouseButton;
            if (mouseLeft !=  InputHasValue((int)PressedButton.LeftMouseButton))
            {
                _pressedButtons = SwitchInput((int)PressedButton.LeftMouseButton);
                _mouseLeftButtonAction?.Invoke(mouseLeft);
            }
            
            var mouseRight = Input.OrdinalInputHandler.Instance.RightMouseButton;
            if (mouseRight !=  InputHasValue((int)PressedButton.RightMouseButton))
            {
                _pressedButtons = SwitchInput((int)PressedButton.RightMouseButton);
                _mouseLeftButtonAction?.Invoke(mouseRight);
            }
        }
        
        
        private bool InputHasValue(int valueToCompare)
        {
            return  ((int)_pressedButtons & valueToCompare) != 0;
        }

        private Input.Data.PressedButton SwitchInput(int valueToUpdate)
        {
            return (PressedButton)((int)_pressedButtons ^ valueToUpdate);
        }
        
        private Input.Data.PressedButton AddInput(int valueToUpdate)
        {
            return (PressedButton)((int)_pressedButtons | valueToUpdate);
        }
    }
}