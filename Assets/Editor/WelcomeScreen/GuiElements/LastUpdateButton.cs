using System;

namespace ImmersiveVRTools.PublisherTools.WelcomeScreen.GuiElements
{
    public class LastUpdateButton: ChangeMainViewButton
    {
        public Action<ProductWelcomeScreenBase> RenderMainScrollViewFunction => _renderMainScrollViewFn;

        public LastUpdateButton(string text, Action<ProductWelcomeScreenBase> renderMainScrollViewFn) 
            : base(text, renderMainScrollViewFn)
        {
        }
    }
}