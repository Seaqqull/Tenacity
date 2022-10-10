using UnityEngine;


namespace Tenacity.Managers.Additional
{
    [System.Serializable]
    public class SaveLoadSnap : General.SaveLoad.Data.SaveSnap
    {
        [System.Serializable]
        public struct Position
        {
            public float X;
            public float Y;
            public float Z;

            
            public Position(Vector3 position)
            {
                X = position.x;
                Y = position.y;
                Z = position.z;
            }
            
            public static implicit operator Vector3(Position position)
            {
                return new Vector3(position.X, position.Y, position.Z);
            }
        }
        
        // General
        // Image of the save 
        public long Date;
        
        // Environment/Storage managers
        public int TargetFramerate;
        public float GameTimeScale;
        public float GameTime;
        
        // In-game info
        public Position PlayerPosition;
        public int SceneIndex;


        public SaveLoadSnap(MonoBehaviour behaviour) : base(behaviour)
        {

        }
    }
}