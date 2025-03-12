using System;

namespace Context
{
    public interface ILifeCycleCallbacksService
    {
        void Dispose();
        void AddCallback(CallbackType callbackType, Action action);
        void RemoveCallback(CallbackType callbackType, Action action);
    }
}