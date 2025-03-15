using Context;
using Context.Interfaces;
using Player.Data;

namespace Player
{
    public class PlayerDataHandler : IDisposableObject
    {
        private const string FILE_NAME = "PlayerData.json";
        
        private readonly IFileService _fileService;
        
        public PlayerDataHandler(IFileService fileService)
        {
            _fileService = fileService;
        }
        
        public void Dispose()
        {
            
        }
        
        public PlayerData LoadData()
        {
            string fileData = _fileService.Load(FILE_NAME);

            if (string.IsNullOrEmpty(fileData) || fileData.Trim() == "{}")
            {
                return new PlayerData();
            }

            try
            {
                return PlayerData.FromJson(fileData);
            }
            catch
            {
                return new PlayerData();
            }
        }
        
        public void SaveData(PlayerData playerData)
        {
            string data = playerData.ToString();
            _fileService.Save(FILE_NAME, data);
        }
    }
}