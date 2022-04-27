


namespace Tenacity.General.Events.Actions
{
    public abstract class ActionBoolSO :
        ActionSO<bool>, Data.IAction<bool>
    {
        public override bool Perform()
        {
            if (CheckPossibility())
                return false;
            return PerformAction();
        }
    }
}