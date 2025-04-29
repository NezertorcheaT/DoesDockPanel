using System.IO;
using JetBrains.Annotations;
using VContainer;

namespace Saving.Settings
{
    [UsedImplicitly]
    public class ConfigEntry
    {
        public static Config Instance { get; private set; }
        public Config Current;

        [Inject]
        private ConfigEntry()
        {
            var configSaver = new ConfigFileSaver();
            var config = new Config(configSaver);
            string configText;

            try
            {
                configText = configSaver.Read(null);
            }
            catch (FileNotFoundException)
            {
                configSaver.Save(config);
                configText = configSaver.Read(null);
            }

            config = config.Deconvert(configText, configSaver) as Config;
            Instance = config;
            Current = config;
        }
    }
}