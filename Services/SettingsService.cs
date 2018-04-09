using System;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Stratus.Models;
using Stratus.Util;

namespace Stratus.Services
{
    class SettingsService
    {
        private const string SettingsFileName = "StatusSettings.json";

        public async void Save(Settings settings)
        {
            var folder = ApplicationData.Current.RoamingFolder;
            var settingsString = JsonConvert.SerializeObject(settings);
            var file = await folder.CreateFileAsync(SettingsFileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, settingsString);
        }

        public async Task Load()
        {
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.TryGetItemAsync(SettingsFileName) as StorageFile;
            if (file == null)
            {
                CurrentSettings = new Settings();
                return;
            }
            var settingsString = await FileIO.ReadTextAsync(file);
            CurrentSettings = JsonConvert.DeserializeObject<Settings>(settingsString, new SafeDictionaryCustomCreationConverter<string,bool>(true));
        }

        public Settings CurrentSettings { get; private set; }


    }
}
