using Runtime.Data;
using Runtime.Data.Original;
using Runtime.Data.Provider;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.InGameSystem
{
    public class DataProviderManager : MonoBehaviour
    {
        public static DataProviderManager Instance { get; private set; }
        public IProvider<PlayerData> PlayerDataProvider { get; private set; }
        public IProvider<SettingsData> SettingsDataProvider { get; private set; }
        public IProvider<ControlsData> ControlsDataProvider { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);

                Init();
                
                return;
            }

            Destroy(gameObject);
        }
        
        private void Init()
        {
            PlayerDataProvider = new PlayerDataProvider(Addressables.LoadAssetAsync<PlayerData>("PlayerData").WaitForCompletion());
            SettingsDataProvider = new SettingsDataProvider(Addressables.LoadAssetAsync<SettingsData>("SettingsData").WaitForCompletion());
            ControlsDataProvider = new ControlsDataProvider(new ControlsData());
        }
    }
}