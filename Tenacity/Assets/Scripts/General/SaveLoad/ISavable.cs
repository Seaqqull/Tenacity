


namespace Tenacity.General.SaveLoad
{
    public interface ISavable
    {
        public int Id { get; }


        public Data.SaveSnap MakeSnap();
        public void FromSnap(Data.SaveSnap data);
    }
}
