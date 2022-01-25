using System;


namespace Tenacity.General.Events.Data
{
    [Serializable]
    public enum EventType { None, GameInit, GameEnd, RoomLoad, RoomPreInit, RoomInit, RoomEnd, RoomClear, RoomChange, RoomEnter, RoomExit, ConditionalTrigger }
    
    public interface IAction<T>
    {
        T Perform();
    }
}