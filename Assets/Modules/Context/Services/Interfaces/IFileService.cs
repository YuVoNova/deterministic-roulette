namespace Context
{
    public interface IFileService
    {
        string Load(string fileName);
        void Save(string fileName, string data);
    }
}