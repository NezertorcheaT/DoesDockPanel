using UnityEngine;

namespace Files
{
    public class Link : FileObject
    {
        public Link(Texture2D image, string file)
        {
            Image = image;
            File = file;
        }

        public override void Dispose()
        {
        }
    }
}