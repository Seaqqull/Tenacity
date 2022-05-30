using System.IO;


namespace Tenacity.General.SaveLoad
{
    public class Exporter<TData> : IExporter<TData>
        where TData : class
    {
        protected string _folder;
        protected string _file;
        
        
        public Exporter(string folder, string file)
        {
            _folder = folder;
            _file = file;
        }
        
        
        public virtual void Export(FileStream stream, TData data)
        {
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);
        }
    }
}