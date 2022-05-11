using System.IO;


namespace Tenacity.General.SaveLoad
{
    interface IImporter<TData>
        where TData : class
    {
        TData Import(FileStream stream);
    }
}
