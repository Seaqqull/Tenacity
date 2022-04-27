using Tenacity.Input.Data;
using Tenacity.Utility;
using System;


namespace Tenacity.Input
{
    public static class InputQueue
    {
        private static PriorityQueue<Action<PressedButton>> _inputQueue = 
            new PriorityQueue<Action<PressedButton>>();


        public static void UnsubscribeInput(Action<PressedButton> onPress)
        {
            _inputQueue.Dequeue(onPress);
        }
        
        public static void SubscribeInput(int priority, Action<PressedButton> onPress)
        {
            if(!_inputQueue.Exist(onPress))
                _inputQueue.Enqueue(priority, onPress);
        }
    }
}