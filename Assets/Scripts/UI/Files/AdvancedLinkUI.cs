using CustomHelper;
using Files;
using R3;

namespace UI.Files
{
    public class AdvancedLinkUI : FileUI
    {
        public AdvancedLink CurrentLink => CurrentFile as AdvancedLink;

        public void Initialize(AdvancedLink link)
        {
            Initialize(link as FileObject);
            Click.Subscribe(_ => Helper.OpenWithDefaultProgram(CurrentLink.Config.ClickAction));
            DoubleClick.Subscribe(_ => Helper.OpenWithDefaultProgram(CurrentLink.Config.DoubleClickAction));
            //RightClick.Subscribe(_ => Helper.OpenWithDefaultProgram(CurrentLink.Config.RightClickAction));
        }
    }
}