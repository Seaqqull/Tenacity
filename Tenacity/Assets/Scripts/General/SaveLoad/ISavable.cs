


namespace Tenacity.General.SaveLoad
{
    public interface ISavable
    {
        public string Id { get; }


        public Data.SaveSnap MakeSnap();
        public void FromSnap(Data.SaveSnap data); // TODO: bool, so it will be possible to determine whether snap is processed
    }
}
