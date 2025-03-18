using Context.Interfaces;

namespace Context
{
    public class ContextManager : IDisposableObject
    {
        public DataStore DataStore => _dataStore;
        public IFileService FileService => _fileService;

        private readonly DataStore _dataStore = new();
        private readonly FileService _fileService = new();

        public void Dispose()
        {
            _fileService.Dispose();
        }
    }
}