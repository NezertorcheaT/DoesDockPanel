using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Files
{
    public class Folder : FileObject
    {
        public readonly ObservableList<FileObject> Links;

        public Folder(Texture2D image, string file) : base(image, file)
        {
            Links = new ObservableList<FileObject>();
        }

        public async Task UpdateLinks()
        {
            await FileObjectUtility.Populate(Links, File);
        }

        public override void Dispose()
        {
            Links.Dispose();
        }
    }
}