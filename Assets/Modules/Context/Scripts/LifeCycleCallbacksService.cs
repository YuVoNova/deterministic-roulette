using System;
using System.Collections.Generic;
using UnityEngine;

namespace Context
{
    public class LifeCycleCallbacksService : ILifeCycleCallbacksService
    {
        private readonly GameObject _serviceObject;
        private readonly MonoBehaviourCallbacks _monoBehaviourCallbacks;

        private Dictionary<Action, CallbackType> _actionsDictionary = new Dictionary<Action, CallbackType>();

        public LifeCycleCallbacksService()
        {
            _serviceObject = new GameObject("LifeCycleCallbacksService");
            _monoBehaviourCallbacks = _serviceObject.AddComponent<MonoBehaviourCallbacks>();
        }

        public void Dispose()
        {
            GameObject.Destroy(_serviceObject);
        }

        public void AddCallback(CallbackType callbackType, Action action)
        {
            if (_actionsDictionary.ContainsKey(action))
                return;

            _actionsDictionary[action] = callbackType;
            _monoBehaviourCallbacks.Add(callbackType, action);
        }

        public void RemoveCallback(CallbackType callbackType, Action action)
        {
            _actionsDictionary.Remove(action);
            _monoBehaviourCallbacks.Remove(callbackType, action);
        }

        public class MonoBehaviourCallbacks : MonoBehaviour
        {
            private HashSet<Action> _updateHashSet = new HashSet<Action>();
            private HashSet<Action> _lateUpdateHashSet = new HashSet<Action>();
            private HashSet<Action> _fixedUpdateHashSet = new HashSet<Action>();

            public void Add(CallbackType callbackType, Action action)
            {
                HashSet<Action> hashSetToUse = GetHashSet(callbackType);

                if (hashSetToUse.Contains(action))
                    return;

                hashSetToUse.Add(action);
            }

            public void Remove(CallbackType callbackType, Action action)
            {
                HashSet<Action> hashSetToUse = GetHashSet(callbackType);

                if (!hashSetToUse.Contains(action))
                    return;

                hashSetToUse.Remove(action);
            }

            public bool Contains(CallbackType callbackType, Action action)
            {
                HashSet<Action> hashSetToUse = GetHashSet(callbackType);
                return hashSetToUse.Contains(action);
            }

            private void Update()
            {
                Dispatch(CallbackType.Update);
            }

            private void LateUpdate()
            {
                Dispatch(CallbackType.LateUpdate);
            }

            private void FixedUpdate()
            {
                Dispatch(CallbackType.FixedUpdate);
            }

            private void Dispatch(CallbackType eventType)
            {
                HashSet<Action> hashSetToDispatch = GetHashSet(eventType);
                
                foreach (Action action in hashSetToDispatch)
                {
                    action.Invoke();
                }
            }

            private HashSet<Action> GetHashSet(CallbackType callbackType)
            {
                return callbackType switch
                {
                    CallbackType.Update => _updateHashSet,
                    CallbackType.LateUpdate => _lateUpdateHashSet,
                    CallbackType.FixedUpdate => _fixedUpdateHashSet,
                    _ => null
                };
            }
        }
    }
}