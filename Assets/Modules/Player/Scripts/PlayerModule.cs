using Context;
using Context.Interfaces;
using Player.Data;

namespace Player
{
    public class PlayerModule : IDisposableObject
    {
        private readonly PlayerDataHandler _playerDataHandler;
        private readonly PlayerData _playerData;

        public PlayerModule(IFileService fileService, DataStore dataStore)
        {
            _playerDataHandler = new PlayerDataHandler(fileService);
            _playerData = _playerDataHandler.LoadData();
            
            dataStore.playerData.Set(_playerData);
        }

        public void Dispose()
        {
            Save();
        }

        private void Save()
        {
            _playerDataHandler.SaveData(_playerData);
        }
    }
}