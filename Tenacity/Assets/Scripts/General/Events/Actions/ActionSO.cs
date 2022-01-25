


namespace Tenacity.General.Events.Actions
{
    public abstract class ActionSO<Toutput> :
        UnityEngine.ScriptableObject
    {
        protected virtual bool CheckPossibility()
        {
            return true;
        }
        
        protected abstract Toutput PerformAction();
        

        public abstract Toutput Perform();
    }
}