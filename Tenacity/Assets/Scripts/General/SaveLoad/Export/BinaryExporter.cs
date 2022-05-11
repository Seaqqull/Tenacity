using System.Runtime.Serialization.Formatters.Binary;
using Tenacity.General.SaveLoad.Data;
using System.Runtime.Serialization;
using System.IO;


namespace Tenacity.General.SaveLoad
{
    public class BinaryExporter : Exporter<SnapshotDatabase>
    {
        public BinaryExporter(string folder, string file) : base(folder, file) { }

        
        public override void Export(FileStream stream, SnapshotDatabase data)
        {
            base.Export(stream, data);
            

            IFormatter formatter = new BinaryFormatter();
            stream.Position = 0;
            //FileStream stream = new FileStream(_folder + "/" + _file, FileMode.Create);
            
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }
}
