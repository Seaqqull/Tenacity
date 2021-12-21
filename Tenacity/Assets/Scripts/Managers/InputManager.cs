using Tenacity.Utility.Base;
using Tenacity.Input.Data;
using System;


namespace Tenacity.Managers
{
    public class InputManager : SingleBehaviour<InputManager>
    {
        [UnityEngine.SerializeField]
        private Input.Data.PressedButton _pressedButtons;

        // Specific buttons
        private static Action<bool> _interactButtonAction;
        private static Action<bool> _spaceButtonAction;
        private static Action<bool> _shiftButtonAction;
        private static Action<bool> _backButtonAction;

        // Base buttons
        private static Action<Input.Data.HorizontalButton> _horizontalButtonAction;
        private static Action<Input.Data.VerticalButton> _verticalButtonAction;
        
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


        private void Update()
        {
            // Base buttons
            var horizontal = (UnityEngine.Mathf.Approximately(Input.OrdinalInputHandler.Instance.Horizontal, 1.0f)) ? 
                HorizontalButton.RightButton : 
                UnityEngine.Mathf.Approximately(Input.OrdinalInputHandler.Instance.Horizontal, -1.0f) ?
                    HorizontalButton.LeftButton :
                    HorizontalButton.Nothing;
            var horizontalNum = (byte)horizontal;
            var vertical = (UnityEngine.Mathf.Approximately(Input.OrdinalInputHandler.Instance.Vertical, 1.0f)) ? 
                VerticalButton.TopButton : 
                UnityEngine.Mathf.Approximately(Input.OrdinalInputHandler.Instance.Vertical, -1.0f) ?
                    VerticalButton.BottomButton :
                    VerticalButton.Nothing;
            var verticalNum = (byte)vertical;

            if (!InputHasValue(horizontalNum))
            {
                // If previously input had horizontal value 
                if((horizontal == HorizontalButton.Nothing) && InputHasValue((byte)HorizontalButton.RightButton))
                {
                    _pressedButtons = SwitchInput((byte)HorizontalButton.RightButton);
                    _horizontalButtonAction?.Invoke(horizontal);
                }
                else if((horizontal == HorizontalButton.Nothing) && InputHasValue((byte)HorizontalButton.LeftButton))
                {
                    _pressedButtons = SwitchInput((byte)HorizontalButton.LeftButton);
                    _horizontalButtonAction?.Invoke(horizontal);
                }
                else if((horizontal == HorizontalButton.RightButton))
                {
                    if(InputHasValue((byte)VerticalButton.BottomButton))
                        _pressedButtons = SwitchInput((byte)(HorizontalButton.RightButton | HorizontalButton.LeftButton));
                    else
                        _pressedButtons = AddInput((byte)HorizontalButton.RightButton);
                    _horizontalButtonAction?.Invoke(horizontal);
                }
                else if((horizontal == HorizontalButton.LeftButton))
                {
                    if(InputHasValue((byte)VerticalButton.BottomButton))
                        _pressedButtons = SwitchInput((byte)(HorizontalButton.RightButton | HorizontalButton.LeftButton));
                    else
                        _pressedButtons = AddInput((byte)HorizontalButton.LeftButton);
                    _horizontalButtonAction?.Invoke(horizontal);
                }
            }
            if (!InputHasValue(verticalNum))
            {
                // If previously input had vertical value 
                if((vertical == VerticalButton.Nothing) && InputHasValue((byte)VerticalButton.TopButton))
                {
                    _pressedButtons = SwitchInput((byte)VerticalButton.TopButton);
                    _verticalButtonAction?.Invoke(vertical);
                }
                else if((vertical == VerticalButton.Nothing) && InputHasValue((byte)VerticalButton.BottomButton))
                {
                    _pressedButtons = SwitchInput((byte)VerticalButton.BottomButton);
                    _verticalButtonAction?.Invoke(vertical);
                }
                else if((vertical == VerticalButton.TopButton))
                {
                    if(InputHasValue((byte)VerticalButton.BottomButton))
                        _pressedButtons = SwitchInput((byte)(VerticalButton.TopButton | VerticalButton.BottomButton));
                    else
                        _pressedButtons = AddInput((byte)VerticalButton.TopButton);
                    _verticalButtonAction?.Invoke(vertical);
                }
                else if((vertical == VerticalButton.BottomButton))
                {
                    if(InputHasValue((byte)VerticalButton.BottomButton))
                        _pressedButtons = SwitchInput((byte)(VerticalButton.TopButton | VerticalButton.BottomButton));
                    else
                        _pressedButtons = AddInput((byte)VerticalButton.BottomButton);
                    _verticalButtonAction?.Invoke(vertical);
                }
            }
            
            // Special buttons
            var interaction = Input.OrdinalInputHandler.Instance.E;
            var space = Input.OrdinalInputHandler.Instance.Space;
            var shift = Input.OrdinalInputHandler.Instance.Shift;
            var back = Input.OrdinalInputHandler.Instance.Esc;

            if (interaction != InputHasValue((byte)PressedButton.Interact))
            {
                _pressedButtons = SwitchInput((byte)PressedButton.Interact);
                _interactButtonAction?.Invoke(interaction);
            }
            if (space != InputHasValue((byte)PressedButton.Space))
            {
                _pressedButtons = SwitchInput((byte)PressedButton.Space);
                _spaceButtonAction?.Invoke(interaction);
            }
            if (back != InputHasValue((byte)PressedButton.Back))
            {
                _pressedButtons = SwitchInput((byte)PressedButton.Back);
                _backButtonAction?.Invoke(interaction);
            }
            if (shift != InputHasValue((byte)PressedButton.Shift))
            {
                _pressedButtons = SwitchInput((byte)PressedButton.Shift);
                _shiftButtonAction?.Invoke(interaction);
            }
        }
        
        
        private bool InputHasValue(byte valueToCompare)
        {
            return  ((byte)_pressedButtons & valueToCompare) != 0;
        }

        private Input.Data.PressedButton SwitchInput(byte valueToUpdate)
        {
            return (PressedButton)((byte)_pressedButtons ^ valueToUpdate);
        }
        
        private Input.Data.PressedButton AddInput(byte valueToUpdate)
        {
            return (PressedButton)((byte)_pressedButtons | valueToUpdate);
        }
    }
}