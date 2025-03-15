using System.Collections;
using UnityEngine;

namespace Context
{
    public interface ICoroutineService
    {
        void Dispose();
        Coroutine StartCoroutine(IEnumerator routine);
        void StopCoroutine(Coroutine coroutine);
    }
}