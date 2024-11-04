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
            LeftClick.Subscribe(_ => Helper.OpenWithDefaultProgram(CurrentLink.Config.LeftClickAction));
            MiddleClick.Subscribe(_ => Helper.OpenWithDefaultProgram(CurrentLink.Config.MiddleClickAction));
            RightClick.Subscribe(_ => Helper.OpenWithDefaultProgram(CurrentLink.Config.RightClickAction));
        }
    }
}