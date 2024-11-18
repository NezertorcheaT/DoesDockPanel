using System;
using UI.Files;
using UnityEngine;

namespace Saving.Links
{
    public class LinkConfigFileSaver : IFileSaver<string>
    {
        public void Save(IFileSaver<string>.ISavable savable)
        {
            if (savable is not LinkConfig linkConfig)
                throw new ArgumentException($"Provided savable '{savable}' is not a {nameof(LinkConfig)}");
            GlobalFileSaver.SaveToDrive(savable.Convert(), linkConfig.AssociatedLink.ConfigFile);
        }

        public string Read(string path)
        {
            if (!LinkUI.IsPathToConfig(path))
                Debug.LogWarning($"File '{path}' probably not a {nameof(LinkConfig)}, be careful");
            return GlobalFileSaver.ReadFromDrive(path);
        }
    }
}