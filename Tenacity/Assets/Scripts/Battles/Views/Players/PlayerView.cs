using Tenacity.Battles.Data;
using UnityEngine;


namespace Tenacity.Battles.Views.Players
{
    public abstract class PlayerView : MonoBehaviour
    {
        public PlayerDataView Data { get; private set; }

        
        public virtual void UpdateData(PlayerDataView data)
        {
            Data = data;
        }
    }
}