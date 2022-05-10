using System.Collections.Generic;
using UnityEngine;
using System;


namespace Tenacity.General.SaveLoad.Data
{
    [System.Serializable]
    public abstract class SaveSnap : IEqualityComparer<SaveSnap>
    {
        private Func<MonoBehaviour> _monoGetter;
        private Func<bool> _isMonoAccessible;
        public int Id; // Redundant parameter
        
        public MonoBehaviour GetMono
        {
            get { return _monoGetter?.Invoke(); }
        }
        public bool MonoAccessible
        {
            get { return _isMonoAccessible?.Invoke() ?? true; }
        }

        
        public SaveSnap(MonoBehaviour behaviour)
        {
            Id = behaviour.GetInstanceID();
        }


        public void AssignAccessor(Func<bool> isMonoAccessible)
        {
            _isMonoAccessible = isMonoAccessible;
        }

        public void AssignGetter<T>(Func<T> monoGetter) where T : MonoBehaviour, ISavable
        {
            _monoGetter = monoGetter;
        }


        #region Comparison
        public int GetHashCode(SaveSnap obj)
        {
            return obj.Id.GetHashCode();
        }
        
        public bool Equals(SaveSnap x, SaveSnap y)
        {
            if ((x == null) && (y == null))
                return true;
            if ((x == null) || (y == null))
                return false;
            return (x.Id == y.Id);
        }
        #endregion
    }

    [System.Serializable]
    public class SaveSnapshot
    {
        public string Title;
        public HashSet<SaveSnap> Data = new();
    }
}
