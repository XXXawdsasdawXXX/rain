using System.Collections.Generic;
using Code.UI.Components.Window;

namespace Code.UI.Hud
{
    public class HudNavigator : AUIWindowNavigator
    {
        public HudNavigator(IEnumerable<AUIWindowView> windows) : base(windows)
        {
        }
    }
}