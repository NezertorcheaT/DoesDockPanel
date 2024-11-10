using System;

namespace Saving.Links
{
    public class FolderConfigFileSaver : IFileSaver<string>
    {
        public void Save(IFileSaver<string>.ISavable savable)
        {
            if (savable is not FolderConfig linkConfig)
                throw new ArgumentException($"Provided savable '{savable}' is not a {nameof(FolderConfig)}");
            GlobalFileSaver.SaveToDrive(savable.Convert(), linkConfig.AssociatedFolder.ConfigFile);
        }

        public string Read(string path)
        {
            return GlobalFileSaver.ReadFromDrive(path);
        }
    }
}