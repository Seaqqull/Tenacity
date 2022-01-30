using EngineInput = UnityEngine.Input;
using Tenacity.Base;
using UnityEngine;


namespace Tenacity.Input
{
    public class OrdinalInputHandler : SingleBehaviour<OrdinalInputHandler>
    {
        private byte _mouseButtons;
        
        public bool MiddleMouseButton
        {
            get { return (_mouseButtons & 2) != 0; }
        }
        public bool RightMouseButton
        {
            get { return (_mouseButtons & 4) != 0; }
        }
        public bool LeftMouseButton
        {
            get { return (_mouseButtons & 1) != 0; }
        }
        public float Horizontal
        {
            get; private set;
        }
        public float Vertical
        {
            get; private set;
        }
        public bool Space
        {
            get; private set;
        }
        public bool Shift
        {
            get; private set;
        }
        public bool Esc
        {
            get; private set;
        }
        public bool E
        {
            get; private set;
        }
        
        
        private void Update()
        {
            // General
            Horizontal = EngineInput.GetAxisRaw("Horizontal");
            Vertical = EngineInput.GetAxisRaw("Vertical");
            
            // Mouse
            _mouseButtons = (byte) ((EngineInput.GetKey(KeyCode.Mouse0) ? 1 : 0) |
                                    (EngineInput.GetKey(KeyCode.Mouse1) ? 2 : 0) |
                                    (EngineInput.GetKey(KeyCode.Mouse2) ? 4 : 0));
            
            // Special
            Shift = EngineInput.GetKey(KeyCode.LeftShift) || EngineInput.GetKey(KeyCode.RightShift);
            Space = EngineInput.GetKey(KeyCode.Space);
            Esc = EngineInput.GetKey(KeyCode.Escape);
            E = EngineInput.GetKey(KeyCode.E);
        }
    }
}