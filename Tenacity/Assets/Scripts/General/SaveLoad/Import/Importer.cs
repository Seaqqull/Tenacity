using System.IO;
using System;


namespace Tenacity.General.SaveLoad
{
    public class Importer<TData> : IImporter<TData>
        where TData : class 
    {
        protected string _folder;
        protected string _file;
        
        
        public Importer(string folder, string file)
        {
            _folder = folder;
            _file = file;
        }

        
        public virtual TData Import(FileStream stream)
        {
            if (!Directory.Exists(_folder))
                throw new ArgumentException($"[System] Wrong folder name: Folder with the given [{_folder}] name doesn't exist");
            if (!File.Exists(_folder + "/" + _file))
                throw new ArgumentException($"[System] Wrong file name: File with the given [{_folder + "/" + _file}] name doesn't exist");
            return null;
        }
    }
}