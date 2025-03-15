using Context;
using Context.Interfaces;
using Player.Data;

namespace Player
{
    public class PlayerModule : IDisposableObject
    {
        private readonly PlayerDataHandler _playerDataHandler;

        private PlayerData _playerData;

        public PlayerModule(IFileService fileService)
        {
            _playerDataHandler = new PlayerDataHandler(fileService);
            _playerData = _playerDataHandler.LoadData();
        }

        public void Dispose()
        {
            Save();
            
            _playerDataHandler.Dispose();
        }

        public PlayerData GetPlayerData()
        {
            return _playerData;
        }

        public void Save()
        {
            _playerDataHandler.SaveData(_playerData);
        }
    }
}