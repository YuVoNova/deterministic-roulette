using System;
using UnityEngine;
using Context;
using Roulette;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        private DataStore _dataStore;
        private ISceneLoader _sceneLoader;
        private ICoroutineService _coroutineService;
        private ILifeCycleCallbacksService _lifeCycleCallbacksService;

        private RouletteModule _rouletteModule;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            _sceneLoader = new SceneLoader();
            _sceneLoader.OnSceneLoaded += SceneLoaded;
        }

        private void Start()
        {
            _sceneLoader.LoadSceneAsync(Scenes.Game);
        }

        private void OnDestroy()
        {
            _sceneLoader.Dispose();

            _coroutineService.Dispose();
        }

        private void SceneLoaded()
        {
            _sceneLoader.OnSceneLoaded -= SceneLoaded;
            
            _dataStore = new DataStore();
            _lifeCycleCallbacksService = new LifeCycleCallbacksService();
            _coroutineService = new CoroutineService();
            
            
        }
    }
}