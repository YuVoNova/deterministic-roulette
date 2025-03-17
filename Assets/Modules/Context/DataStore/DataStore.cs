using Player.Data;

namespace Context
{
    public class DataStore
    {
        public ObservableVariable<PlayerData> playerData = new ObservableVariable<PlayerData>();
    }
}