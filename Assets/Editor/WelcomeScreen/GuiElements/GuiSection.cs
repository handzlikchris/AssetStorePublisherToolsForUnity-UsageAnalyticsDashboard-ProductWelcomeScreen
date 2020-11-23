using System.Collections.Generic;

namespace ImmersiveVRTools.PublisherTools.WelcomeScreen.GuiElements
{
    public class GuiSection : GuiSectionBase
    {
        public List<ClickableElement> ClickableElements { get; }

        public GuiSection(string labelText, List<ClickableElement> clickableElements)
            : base(labelText)
        {
            ClickableElements = clickableElements;
        }

        public GuiSection(string labelText, string labelSubText, List<ClickableElement> clickableElements)
            : base(labelText, labelSubText)
        {
            ClickableElements = clickableElements;
        }
    }
}