using System.IO;


namespace Tenacity.General.SaveLoad
{
    public interface IExporter<TData>
        where TData : class
    {
        void Export(FileStream stream, TData data);
    }
}
