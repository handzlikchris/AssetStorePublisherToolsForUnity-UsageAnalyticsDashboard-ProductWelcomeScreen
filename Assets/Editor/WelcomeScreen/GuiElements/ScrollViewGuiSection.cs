using System;

namespace ImmersiveVRTools.PublisherTools.WelcomeScreen.GuiElements
{
    public class ScrollViewGuiSection : GuiSectionBase
    {
        public Action<ProductWelcomeScreenBase> RenderMainScrollViewSection { get; }

        public ScrollViewGuiSection(string labelText, Action<ProductWelcomeScreenBase> renderMainScrollViewSection)
            : base(labelText)
        {
            RenderMainScrollViewSection = renderMainScrollViewSection;
        }

    }
}