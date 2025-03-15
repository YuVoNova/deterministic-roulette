using System.Collections;
using Context.Interfaces;
using UnityEngine;

namespace Context
{
    public class CoroutineService : ICoroutineService, IDisposableObject
    {
        private readonly GameObject _serviceObject;
        private readonly CoroutineExecutor _coroutineExecutor;

        public CoroutineService()
        {
            _serviceObject = new GameObject("CoroutineService");
            _coroutineExecutor = _serviceObject.AddComponent<CoroutineExecutor>();
        }

        public void Dispose()
        {
            if (_coroutineExecutor != null)
                _coroutineExecutor.Dispose();

            if (_serviceObject != null)
                GameObject.Destroy(_serviceObject);
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return _coroutineExecutor.ExecuteCoroutine(routine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            if (_coroutineExecutor != null)
                _coroutineExecutor.StopRunningCoroutine(coroutine);
        }

        public class CoroutineExecutor : MonoBehaviour
        {
            public void Dispose()
            {
                StopAllCoroutines();
            }

            public Coroutine ExecuteCoroutine(IEnumerator routine)
            {
                if (routine != null)
                    StopCoroutine(routine);

                return StartCoroutine(routine);
            }

            public void StopRunningCoroutine(Coroutine routine)
            {
                if (routine != null)
                    StopCoroutine(routine);
            }
        }
    }
}