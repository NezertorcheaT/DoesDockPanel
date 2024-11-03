using UnityEngine;

namespace Files
{
    public class Link : FileObject
    {
        public Link(Texture2D image, string file) : base(image, file)
        {
        }

        public override void Dispose()
        {
        }
    }
}