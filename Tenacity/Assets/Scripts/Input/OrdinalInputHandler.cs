using EngineInput = UnityEngine.Input;
using Tenacity.Utility.Base;
using UnityEngine;


namespace Tenacity.Input
{
    public class OrdinalInputHandler : SingleBehaviour<OrdinalInputHandler>
    {
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
            
            // Special
            Shift = EngineInput.GetKey(KeyCode.LeftShift) || EngineInput.GetKey(KeyCode.RightShift);
            Space = EngineInput.GetKey(KeyCode.Space);
            Esc = EngineInput.GetKey(KeyCode.Escape);
            E = EngineInput.GetKey(KeyCode.E);
        }
    }
}