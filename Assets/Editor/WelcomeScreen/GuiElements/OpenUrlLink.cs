using UnityEngine;

namespace ImmersiveVRTools.PublisherTools.WelcomeScreen.GuiElements
{
    public class OpenUrlLink : ClickableElement
    {
        public string Url { get; set; }

        public OpenUrlLink(string text, string url)
            : base(text, string.Empty)
        {
            Url = url;
        }

        public override void OnClick(ProductWelcomeScreenBase welcomeScreen)
        {
            Application.OpenURL(Url);
        }
    }
}