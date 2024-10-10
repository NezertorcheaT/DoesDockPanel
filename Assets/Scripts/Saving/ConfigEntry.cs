using System;
using System.IO;
using UnityEngine;

namespace Saving
{
    public class ConfigEntry : MonoBehaviour, IEntriable
    {
        public static Config Instance;

        private void Start()
        {
        }

        void IEntriable.Begin()
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