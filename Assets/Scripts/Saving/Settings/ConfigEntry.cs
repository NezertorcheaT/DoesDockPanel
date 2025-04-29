using System.IO;
using JetBrains.Annotations;
using VContainer.Unity;

namespace Saving.Settings
{
    [UsedImplicitly]
    public class ConfigEntry : IStartable
    {
        public static Config Instance;

        void IStartable.Start()
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
        }
    }
}