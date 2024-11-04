using CustomHelper;
using Files;
using R3;

namespace UI.Files
{
    public class LinkUI : FileUI
    {
        public void Initialize(Link link)
        {
            Initialize(link as FileObject);
            LeftClick.Subscribe(l => Helper.OpenWithDefaultProgram(l.File));
        }
    }
}