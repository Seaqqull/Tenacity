using Tenacity.Battles.Data;
using Tenacity.Battles.Data.Field;


namespace Tenacity.Battles.Players
{
    public sealed class ManualPlayer : Player
    {
        public ManualPlayer(IPlayerData data, int id,  TeamType team) : base(data, id, team) { }
        
        
        public override void SetupMatchForTurn(IBoard board)
        {
            board.SetUI(true);
        }

        public override void StartTurn()
        {
        }
    }
}