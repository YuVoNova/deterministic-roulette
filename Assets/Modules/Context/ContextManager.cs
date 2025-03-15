using Context.Interfaces;

namespace Context
{
    public class ContextManager : IDisposableObject
    {
        public DataStore DataStore => _dataStore;
        public ILifeCycleCallbacksService LifeCycleCallbacksService => _lifeCycleCallbacksService;
        public ICoroutineService CoroutineService => _coroutineService;
        public IFileService FileService => _fileService;

        private readonly DataStore _dataStore;
        private readonly LifeCycleCallbacksService _lifeCycleCallbacksService;
        private readonly CoroutineService _coroutineService;
        private readonly FileService _fileService;

        public ContextManager()
        {
            _dataStore = new DataStore();
            _lifeCycleCallbacksService = new LifeCycleCallbacksService();
            _coroutineService = new CoroutineService();
            _fileService = new FileService();
        }

        public void Dispose()
        {
            _lifeCycleCallbacksService?.Dispose();
            _coroutineService?.Dispose();
            _fileService?.Dispose();
        }
    }
}