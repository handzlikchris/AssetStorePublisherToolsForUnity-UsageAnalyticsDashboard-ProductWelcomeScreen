# Unity Asset Publisher Tools - Product Welcome Screen

I bet you've used plenty of Unity Store Assets with quality ranging from total waste of time to something that feels like it should be shipped with Unity installation.

The ones that make great impression have few things in common:
- Usable from first launch (no matter your setup) without headache of adjusting code
- Easy to configure for your needs
- When everything goes wrong support is solid and easy to find, be it documentation or contacting developer

This tool will help you with getting there:
- Make great first impression on your asset user
    - Automatically configure asset to be used with their setup 
        - eg. adjust shaders based on current project setup
    - Allow users to easily configure specific asset options for their needs
        - eg. allow to turn specific 3rd integrations on/off by automatically applying build symbols, this means you can ship all those integrations in main asset pack without user needing to import specific files 
    - Give them all details needed
        - from resources like documentation, website page
        - to easy direct support
- Save lots of time: little coding on your part, **just provide your content** and some basic handler code
- Easily communicate with end-users
    - send messages directly to users
        - could be sending new release info
        - **asking to leave a review on asset store** (perhaps while also offering a discount on your next product)
- Track your unity asset usage
    - understand if people are actually working with your product (as opposed to just knowing someone bought it)
    - helps to identify returning users - they could provide you with insight to make your product better
        - perhaps you could even nudge them to chat via quick message
    
## Easy Setup
(add image)

1) Copy and include `WelcomeScreen` folder into your asset
> You can also include compiled DLL as this contains base classes / common functionality, since this is asset that you distribute - I found it best to include as source code.
2) Copy `ExampleWelcomeScreen.cs` file to your project (this includes screen customisable screen initialization code)
3) Customisable parts, eg name, section, etc are in region `CustomisablePerProduct` in all 3 classes. Please adjust as needed (more options in later sections)
> Code in `RequiredSetupCode` regions is adding window / preferences to Unity Editor - it's best to left unchanged.
4) Done - you'll now have functional and professionally looking welcome screen that'll add analytics and communication ability to your asset/

### Create your welcome screen

## `Weclome Screen` class
Basic project information as well as content definitions can be found here (region `CustomisablePerProduct`).

### General
- `IsUsageAnalyticsAndCommunicationsEnabled` - this will enable usage analytics and one-way messaging for asset [more info in section `Set up Analytics`]
- `ProjectId` - unique ID (used for analytics and PerfSettings prefix) 
*Use only letters(a-z) and dashes(-)*
- `VersionId` - current product version (will help determine which users updated your asset to newest version)
- `ProductName` - shows as a secion name in Preferences, as well as window title
- `StartWindowMenuItemPath` - access to welcome screen is available via top-bar in Unity, this specifies path
- `ProductKeywords` - keywords used by Quick Search
- `ProjectIconName` - 64x64 product icon that'll be visible in bottom right corner

### Layout
- `_WindowSizePx` - width and height of your window
- `LeftColumnWidth` - window is in 2 column layout - this value controls widht of 'menu' (left side)

### Section Definitions
(TODO: add image of sections and describe)
Screen GUI is composed of 4 sections as on the image.

#### Navigation Menu (left column)
You can put various 'pages' to the menu, user will be able to click on it and that'd in turn show some content in `MainSection` and/or execute some code. 

Sample Usage:
- provide documentation pages
- segment settings to make setup simpler
- launch a demo scene

Example definition:
```
private static readonly List<GuiSection> LeftSections = new List<GuiSection>() {
    new GuiSection("Options", new List<ClickableElement>() {
        new ChangeMainViewButton("Shaders", (screen) =>
        {
            //This code will be executed when user clicked on 'Shaders' menu options - it's standard UnityEditorUI code

            //Create label with text using TextStyle (small, standard text)
            GUILayout.Label(
                @"By default package uses HDRP shaders, you can change those to standard surface shaders from dropdown below",
                screen.TextStyle
            );

            //200 label width to create additional space between label and dropdown box
            using (LayoutHelper.LabelWidth(200))
            {
                //Use RenderGuiAndPersistInput for ShaderModePreferenceDefinition, more on that later, editor definition knows what type of input to render, dropdown, text, etc...
                ProductPreferenceBase.RenderGuiAndPersistInput(WelcomeScreenPreferences.ShaderModePreferenceDefinition);
            }
        })
    })
};
```

Code will add a section called `Options` with button `Shaders` that upon clicking will open page `MainView` with text `By default package uses HDRP shaders, you can change t...` and will allow user to tweak `Shader` preference via dropdown

(TODO: add screen)

> More on available classes in `Available Section Definition Actions` section.

#### Links/Actions Section (top, right column)
This is short buttons/actions section that's placed in right column. Items will be placed on same line.

Generally used for:
- various easy to access links
    - support forum
    - contact

Example definition:
TODO: work on getting client ID incorporated in GUI, eg open TrackableOpenUrlButton
```
private static readonly GuiSection TopSection = new GuiSection("Support", new List<ClickableElement>
    {
        new OpenUrlButton("Documentation", $"{RedirectBaseUrl}/documentation"),
        new OpenUrlButton("Unity Forum", $"{RedirectBaseUrl}/unity-forum"),
        new OpenUrlButton("Contact", $"{RedirectBaseUrl}/contact")
    }
);
```

Definition will add `Support` section with 3 links `Documentation` / `Unity Forum` / `Contact` those will open a browser for user.

TODO: include that as a section of it's own
> You can also use `TrackableOpenUrlButton` if tracking is enabled, those URLs will then go via website where you can set up redirect URL. It'll ensure your links never go stale, not even for people that do not update your asset. As additional benefit it'll web-application will also provide you link usage data.

#### Main Content Section (center, right column)
This is what will show upon startup

Example definition:
```
    private static readonly ScrollViewGuiSection MainContentSection = new ScrollViewGuiSection(
        "", (screen) =>
        {
            GenerateCommonWelcomeText(ProductName, screen);

            GUILayout.Label("Quick adjustments:", screen.LabelStyle);
            using (LayoutHelper.LabelWidth(220))
            {
                ProductPreferenceBase.RenderGuiAndPersistInput(WelcomeScreenPreferences.EnableXrToolkitIntegrationPreferenceDefinition);
                ProductPreferenceBase.RenderGuiAndPersistInput(WelcomeScreenPreferences.EnableVrtkIntegrationPreferenceDefinition);
                ProductPreferenceBase.RenderGuiAndPersistInput(WelcomeScreenPreferences.ShaderModePreferenceDefinition);
            }
        }
    );
```
As simple as that a welcome text with some basic info will be displayed, additionally some `Quick Adjustments` menu will be shown that'll allow to easily configure often-used preferences

#### Bottom Section (bottom, right column)
Small bottom section for additional content that you'd like to be visible on every page. It can also feature small 64x64px icon of your product.

Example definition:
```
private static readonly GuiSection BottomSection = new GuiSection(
    "Can I ask for a quick favour?",
    $"I'd be great help if you could spend few minutes to leave a review on:",
    new List<ClickableElement>
    {
        //TODO: Replace with trackable
        new OpenUrlButton("  Unity Asset Store", $"<your asset store link>"),
    }
);
```
Hopefully this will get them to write that review...


#### Last Update Text Section (same as Main Content)
This section will replace `MainContent` when there's a message available for user. It'll only be displayed once after which is considered 'read'. By default it's just text but you can adjust to include anything that's needed. Access to last update text via `screen.LastUpdateText`

Exaple definition:
```
private static readonly ScrollViewGuiSection LastUpdateSection = new ScrollViewGuiSection(
    "New Update", (screen) =>
    {
        GUILayout.Label(screen.LastUpdateText, screen.BoldTextStyle, GUILayout.ExpandHeight(true));
    }
);
```

#### Available Section Definition Actions
TODO:

#### Extending Section Definition Actions
TODO: perhaps move further


## `Weclome Screen Preferences` class

## Set up Analytics


## Communication with your asset users

### Roadmap
- 2 way communication 
    - eg. for support purposes