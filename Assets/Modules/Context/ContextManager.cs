using Context.Interfaces;

namespace Context
{
    public class ContextManager : IDisposableObject
    {
        public DataStore DataStore => _dataStore;
        public ILifeCycleCallbacksService LifeCycleCallbacksService => _lifeCycleCallbacksService;
        public ICoroutineService CoroutineService => _coroutineService;
        public IFileService FileService => _fileService;

        private readonly DataStore _dataStore = new();
        private readonly LifeCycleCallbacksService _lifeCycleCallbacksService = new();
        private readonly CoroutineService _coroutineService = new();
        private readonly FileService _fileService = new();

        public void Dispose()
        {
            _lifeCycleCallbacksService?.Dispose();
            _coroutineService?.Dispose();
            _fileService?.Dispose();
        }
    }
}