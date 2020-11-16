using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveVRTools.PublisherTools.WelcomeScreen;
using ImmersiveVRTools.PublisherTools.WelcomeScreen.GuiElements;
using ImmersiveVRTools.PublisherTools.WelcomeScreen.PreferenceDefinition;
using ImmersiveVRTools.PublisherTools.WelcomeScreen.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class WelcomeScreen : ProductWelcomeScreenBase
{
    #region CustomisablePerProduct

    //General
    public static bool IsUsageAnalyticsAndCommunicationsEnabled = true;
    public static readonly string ProjectId = "immersive-vr-mechanic-tools";
    public static string VersionId = "1.0";
    
    public static string ProductName = "Immersive VR Mechanic Tools";
    private const string StartWindowMenuItemPath = "Window/Immersive VR Mechanic Tools/Start Screen";
    public static string[] ProductKeywords = new[] { "start", "vr", "tools" };
    private static readonly string ProjectIconName = "ProductIcon64";

    //Window Layout
    private static Vector2 _WindowSizePx = new Vector2(650, 500);
    private static readonly int LeftColumnWidth = 175;
    
    //Section Definitions
    private static readonly List<GuiSection> LeftSections = new List<GuiSection>() {
        new GuiSection("", new List<ClickableElement>
        {
            //Standard communication screen which is used when user has any message that you passed on to them
            new LastUpdateButton("New Update!", (screen) => LastUpdateSection.RenderMainScrollViewSection(screen)),
            //Standard welcome screen, that's visible unless there's new update
            new ChangeMainViewButton("Welcome", (screen) => MainContentSection.RenderMainScrollViewSection(screen)),
        }),
        new GuiSection("Options", new List<ClickableElement>
        {
            new ChangeMainViewButton("VR Integrations", (screen) =>
            {
                GUILayout.Label(
                    @"XR Toolkit will require some dependencies to run, please have a look in documentation, it should only take a few moments to set up.",
                    screen.TextStyle
                );

                using (LayoutHelper.LabelWidth(200))
                {
                    ProductPreferenceBase.RenderGuiAndPersistInput(WelcomeScreenPreferences.EnableXrToolkitIntegrationPreferenceDefinition);
                }

                const int sectionBreakHeight = 15;
                GUILayout.Space(sectionBreakHeight);

                GUILayout.Label(
                    @"VRTK require some dependencies to run, please have a look in documentation, it should only take a few moments to set up.",
                    screen.TextStyle
                );

                using (LayoutHelper.LabelWidth(200))
                {
                    ProductPreferenceBase.RenderGuiAndPersistInput(WelcomeScreenPreferences.EnableVrtkIntegrationPreferenceDefinition);
                }
                GUILayout.Space(sectionBreakHeight);

            }),
            new ChangeMainViewButton("Shaders", (screen) =>
            {
                GUILayout.Label(
                    @"By default package uses HDRP shaders, you can change those to standard surface shaders from dropdown below",
                    screen.TextStyle
                );

                using (LayoutHelper.LabelWidth(200))
                {
                    ProductPreferenceBase.RenderGuiAndPersistInput(WelcomeScreenPreferences.ShaderModePreferenceDefinition);
                }
            })
        }),
        new GuiSection("Launch Demo", new List<ClickableElement>
        {
            new LaunchSceneButton("XR Toolkit", (s) => GetXrToolkingDemoScenePath())
        })
    };

    private static readonly GuiSection TopSection = new GuiSection("Support", new List<ClickableElement>
        {
            new OpenUrlButton("Documentation", $"{RedirectBaseUrl}/documentation"),
            new OpenUrlButton("Unity Forum", $"{RedirectBaseUrl}/unity-forum"),
            new OpenUrlButton("Contact", $"{RedirectBaseUrl}/contact")
        }
    );

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

    private static readonly GuiSection BottomSection = new GuiSection(
        "Can I ask for a quick favour?",
        $"I'd be great help if you could spend few minutes to leave a review on:",
        new List<ClickableElement>
        {
            new OpenUrlButton("  Unity Asset Store", $"{RedirectBaseUrl}/unity-asset-store"),
        }
    );

    private static readonly ScrollViewGuiSection LastUpdateSection = new ScrollViewGuiSection(
        "New Update", (screen) =>
        {
            GUILayout.Label(screen.LastUpdateText, screen.BoldTextStyle, GUILayout.ExpandHeight(true));
        }
    );

    #endregion
    private static string GetXrToolkingDemoScenePath()
    {
        var demoScene = AssetDatabase.FindAssets("t:Scene XRToolkitDemoScene").FirstOrDefault();
        return demoScene != null ? AssetDatabase.GUIDToAssetPath(demoScene) : null;
    }

    //Following code is required, please do not remove or amend
    #region RequiredSetupCode

    private static string WebSafeProjectId { get; } = Uri.EscapeDataString(ProjectId);
    public static string BaseUrl = "https://immersivevrtools.com";
    private static readonly string RedirectBaseUrl = $"{BaseUrl}/redirect/{WebSafeProjectId}";
    public static string GenerateGetUpdatesUrl(string userId, string versionId)
    {
        if (!IsUsageAnalyticsAndCommunicationsEnabled) return string.Empty;
        return $"{BaseUrl}/updates/{WebSafeProjectId}/{userId}?CurrentVersion={versionId}";
    }
    private static int RightColumnWidth => (int)_WindowSizePx.x - LeftSections.First().WidthPx - 15;
    public override string WindowTitle { get; } = ProductName;
    public override Vector2 WindowSizePx { get; } = _WindowSizePx;

    static WelcomeScreen()
    {
        foreach (var section in LeftSections)
            section.InitializeWidthPx(LeftColumnWidth);

        TopSection.InitializeWidthPx(RightColumnWidth);
        BottomSection.InitializeWidthPx(RightColumnWidth);
    }

    [MenuItem(StartWindowMenuItemPath, false, 1999)]
    public static void Init()
    {
        OpenWindow<WelcomeScreen>(ProductName, _WindowSizePx);
    }

    public void OnEnable()
    {
        OnEnableCommon(ProjectIconName);
    }

    public void OnGUI()
    {
        RenderGUI(LeftSections, TopSection, BottomSection, MainContentSection);
    }

    #endregion
}

public class WelcomeScreenPreferences : ProductPreferenceBase
{
    #region CustomisablePerProduct

    public static string BuildSymbol_EnableXrToolkit = "INTEGRATIONS_XRTOOLKIT";
    public static string BuildSymbol_EnableVRTK = "INTEGRATIONS_VRTK";


    public static readonly ToggleProjectEditorPreferenceDefinition EnableXrToolkitIntegrationPreferenceDefinition = new ToggleProjectEditorPreferenceDefinition(
        "Enable Unity XR Toolkit integration", "XRToolkitIntegrationEnabled", true,
        (newValue, oldValue) =>
        {
            //BuildDefineSymbolManager.SetBuildDefineSymbolState(BuildSymbol_EnableXrToolkit, (bool)newValue);
        });

    public static readonly ToggleProjectEditorPreferenceDefinition EnableVrtkIntegrationPreferenceDefinition = new ToggleProjectEditorPreferenceDefinition(
        "Enable VRTK integration", "VRTKIntegrationEnabled", false,
        (newValue, oldValue) =>
        {
            //BuildDefineSymbolManager.SetBuildDefineSymbolState(BuildSymbol_EnableVRTK, (bool)newValue);
        });

    public static readonly EnumProjectEditorPreferenceDefinition ShaderModePreferenceDefinition = new EnumProjectEditorPreferenceDefinition("Shaders",
        "ShadersMode",
        ShadersMode.HDRP,
        typeof(ShadersMode),
        (newValue, oldValue) =>
        {
            if (oldValue == null) oldValue = default(ShadersMode);

            var newShaderModeValue = (ShadersMode)newValue;
            var oldShaderModeValue = (ShadersMode)oldValue;

            if (newShaderModeValue != oldShaderModeValue)
            {
                SetCommonMaterialsShader(newShaderModeValue);
            }
        }
    );

    public static void SetCommonMaterialsShader(ShadersMode newShaderModeValue)
    {
        var rootToolFolder = AssetPathResolver.GetAssetFolderPathRelativeToScript(ScriptableObject.CreateInstance(typeof(WelcomeScreen)), 1);
        var assets = AssetDatabase.FindAssets("t:Material", new[] { rootToolFolder });

        try
        {
            Shader shaderToUse = null;
            switch (newShaderModeValue)
            {
                case ShadersMode.HDRP: shaderToUse = Shader.Find("HDRP/Lit"); break;
                case ShadersMode.URP: shaderToUse = Shader.Find("Universal Render Pipeline/Lit"); break;
                case ShadersMode.Surface: shaderToUse = Shader.Find("Standard"); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var guid in assets)
            {
                var material = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));
                if (material.shader.name != shaderToUse.name)
                {
                    material.shader = shaderToUse;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Shader does not exist: {ex.Message}");
        }
    }

    public enum ShadersMode
    {
        HDRP,
        URP,
        Surface
    }

    //Add all preference options that you'd like to be persisted for project
    public static List<ProjectEditorPreferenceDefinitionBase> PreferenceDefinitions = new List<ProjectEditorPreferenceDefinitionBase>()
    {
        CreateDefaultShowOptionPreferenceDefinition(),
        EnableXrToolkitIntegrationPreferenceDefinition,
        EnableVrtkIntegrationPreferenceDefinition,
        ShaderModePreferenceDefinition
    };
    
    #endregion


    #region RequiredSetupCode
    private static bool PrefsLoaded = false;
    

#if UNITY_2019_1_OR_NEWER
    [SettingsProvider]
    public static SettingsProvider ImpostorsSettings()
    {
        return GenerateProvider(WelcomeScreen.ProductName, WelcomeScreen.ProductKeywords, PreferencesGUI);
    }

#else
	[PreferenceItem(ProductName)]
#endif
    public static void PreferencesGUI()
    {
        if (!PrefsLoaded)
        {
            LoadDefaults(PreferenceDefinitions);
            PrefsLoaded = true;
        }

        RenderGuiCommon(PreferenceDefinitions);
    }

    #endregion
}

[InitializeOnLoad]
public class WelcomeScreenInitializer : WelcomeScreenInitializerBase
{
    #region CustomisablePerProduct

    public static void RunOnWindowOpened(bool isFirstRun)
    {
        AutoDetectAndSetShaderMode();
    }

    private static void AutoDetectAndSetShaderMode()
    {
        var usedShaderMode = WelcomeScreenPreferences.ShadersMode.Surface;
        var renderPipelineAsset = GraphicsSettings.renderPipelineAsset;
        if (renderPipelineAsset == null)
        {
            usedShaderMode = WelcomeScreenPreferences.ShadersMode.Surface;
        }
        else if (renderPipelineAsset.GetType().Name.Contains("HDRenderPipelineAsset"))
        {
            usedShaderMode = WelcomeScreenPreferences.ShadersMode.HDRP;
        }
        else if (renderPipelineAsset.GetType().Name.Contains("UniversalRenderPipelineAsset"))
        {
            usedShaderMode = WelcomeScreenPreferences.ShadersMode.URP;
        }

        WelcomeScreenPreferences.ShaderModePreferenceDefinition.SetEditorPersistedValue(usedShaderMode);
        WelcomeScreenPreferences.SetCommonMaterialsShader(usedShaderMode);
    }

    #endregion


    #region RequiredSetupCode

    static WelcomeScreenInitializer()
    {
        var userId = ProductPreferenceBase.CreateDefaultUserIdDefinition(WelcomeScreen.ProjectId).GetEditorPersistedValueOrDefault().ToString();

        HandleUnityStartup(WelcomeScreen.Init, WelcomeScreen.GenerateGetUpdatesUrl(userId, WelcomeScreen.VersionId), RunOnWindowOpened);
    }

    #endregion 
}