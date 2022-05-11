using System.Runtime.Serialization.Formatters.Binary;
using Tenacity.General.SaveLoad.Data;
using System.Runtime.Serialization;
using System.IO;
using System;


namespace Tenacity.General.SaveLoad
{
    public class BinaryImporter : Importer<SnapshotDatabase>
    {
        public BinaryImporter(string folder, string file) : base(folder, file) { }
        
        
        public override SnapshotDatabase Import(FileStream stream)
        {
            base.Import(stream);


            IFormatter formatter = new BinaryFormatter();
            stream.Position = 0;
            //FileStream stream = new FileStream(_folder + "/" + _file, FileMode.Open);
            if (stream.Length <= 0)
                throw new Exception("Cannot import saves.");
            var data = (SnapshotDatabase)formatter.Deserialize(stream);
            return data;
        }
    }
}
