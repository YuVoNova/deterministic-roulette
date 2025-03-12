using System;

namespace GameManager
{
    public interface ISceneLoader
    {
        event Action OnSceneLoaded;

        void Dispose();
        void LoadScene(Scenes sceneToLoad);
        void LoadSceneAsync(Scenes sceneToLoad);
    }
}