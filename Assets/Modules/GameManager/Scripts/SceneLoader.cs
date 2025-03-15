using System;
using UnityEngine.SceneManagement;

namespace GameManager
{
    public class SceneLoader : ISceneLoader
    {
        public event Action OnSceneLoaded;

        public SceneLoader()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }
        
        public void Dispose()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
        }

        public void LoadScene(Scenes sceneToLoad)
        {
            int sceneId = (int) sceneToLoad;
            SceneManager.LoadScene(sceneId, LoadSceneMode.Single);
        }

        public void LoadSceneAsync(Scenes sceneToLoad)
        {
            int sceneId = (int) sceneToLoad;
            SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Single);
        }

        private void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            OnSceneLoaded?.Invoke();
        }
    }
}
