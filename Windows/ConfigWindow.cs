using System.IO;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;


namespace QuestTextRecolor.Windows;

public class ConfigWindow : Window
{
    private enum ConfigPage
    {
        General,
        Colors,
        TextStyle,
        PopupLayout,
        Presets,
        Textures,
        About
    }

    private readonly Plugin plugin;
    private readonly Configuration configuration;

    private ConfigPage currentPage = ConfigPage.General;

    public ConfigWindow(Plugin plugin)
        : base("Quest Text Recolor")
    {
        this.plugin = plugin;
        configuration = plugin.Configuration;

        Flags = ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoCollapse;

        Size = new Vector2(680, 520);
        SizeCondition = ImGuiCond.Always;
    }

    public override void Draw()
    {
        ImGui.BeginChild(
            "Navigation",
            new Vector2(150f, 0f),
            true
        );

        DrawNavigation();

        ImGui.EndChild();

        ImGui.SameLine();

        ImGui.BeginChild(
            "Content",
            new Vector2(0f, 0f),
            false
        );

        DrawCurrentPage();

        ImGui.EndChild();
    }

    private void DrawNavigation()
    {
        ImGui.Text("Settings");
        ImGui.Separator();
        ImGui.Spacing();

        if (ImGui.Selectable(
            "General",
            currentPage == ConfigPage.General))
        {
            currentPage = ConfigPage.General;
        }

        ImGui.Spacing();

        if (ImGui.Selectable(
            "Colors",
            currentPage == ConfigPage.Colors))
        {
            currentPage = ConfigPage.Colors;
        }

        ImGui.Spacing();

        if (ImGui.Selectable(
            "Text Style",
            currentPage == ConfigPage.TextStyle))
        {
            currentPage = ConfigPage.TextStyle;
        }

        ImGui.Spacing();

        if (ImGui.Selectable(
            "Popup Layout",
            currentPage == ConfigPage.PopupLayout))
        {
            currentPage = ConfigPage.PopupLayout;
        }

        ImGui.Spacing();

        if (ImGui.Selectable(
            "Presets",
            currentPage == ConfigPage.Presets))
        {
            currentPage = ConfigPage.Presets;
        }

        ImGui.Spacing();

        if (ImGui.Selectable(
            "Textures",
            currentPage == ConfigPage.Textures))
        {
            currentPage = ConfigPage.Textures;
        }

        ImGui.Spacing();

        if (ImGui.Selectable(
            "About",
            currentPage == ConfigPage.About))
        {
            currentPage = ConfigPage.About;
        }
    }

    private void DrawCurrentPage()
    {
        switch (currentPage)
        {
            case ConfigPage.General:
                DrawGeneralPage();
                break;

            case ConfigPage.Colors:
                DrawColorSection();
                break;

            case ConfigPage.TextStyle:
                DrawTextStyleSection();
                break;

            case ConfigPage.PopupLayout:
                DrawPositionSection();
                break;

            case ConfigPage.Presets:
                DrawPresetSection();
                break;

            case ConfigPage.Textures:
                DrawTextureSection();
                break;

            case ConfigPage.About:
                DrawAboutSection();
                break;
        }
    }

    private void DrawGeneralPage()
    {
        DrawPluginSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawTestSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawResetSection();
    }

    private void DrawPluginSection()
    {
        ImGui.Text("Plugin");

        var enabled = configuration.EnableQuestTextRecolor;

        if (ImGui.Checkbox(
            "Enable Quest Text Recolor",
            ref enabled))
        {
            configuration.EnableQuestTextRecolor = enabled;
            configuration.Save();
        }

        ImGui.TextDisabled(
            "Enable or disable quest popup text customization."
        );
    }

    private void DrawColorSection()
    {
        ImGui.Text("Colors");

        ImGui.TextDisabled(
            "Customize the quest text and outline colors."
        );

        ImGui.Spacing();

        var textColor = configuration.QuestTextColor;

        if (ImGui.ColorEdit4(
            "Text Color",
            ref textColor))
        {
            configuration.QuestTextColor = textColor;
            configuration.Save();
        }

        ImGui.TextDisabled(
            "Plugin Default: #F2E4C4FF"
        );

        ImGui.Spacing();

        var edgeColor = configuration.QuestEdgeColor;

        if (ImGui.ColorEdit4(
            "Edge Color",
            ref edgeColor))
        {
            configuration.QuestEdgeColor = edgeColor;
            configuration.Save();
        }

        ImGui.TextDisabled(
            "Plugin Default: #5A4526FF"
        );
    }

    private void DrawTextStyleSection()
    {
        ImGui.Text("Text Style");

        ImGui.TextDisabled(
            "Adjust the size of the quest objective text."
        );

        ImGui.Spacing();

        var fontSize = configuration.QuestFontSize;

        if (ImGui.SliderInt(
            "Font Size",
            ref fontSize,
            12,
            28))
        {
            configuration.QuestFontSize = fontSize;
            configuration.Save();
        }

        ImGui.TextDisabled(
            "Plugin Default: 18"
        );
    }

    private void DrawPositionSection()
    {
        ImGui.Text("Popup Layout");

        ImGui.TextDisabled(
            "Adjust the position and size of the quest popup."
        );

        ImGui.Spacing();

        var offsetX = configuration.QuestPopupOffsetX;

        if (ImGui.SliderFloat(
            "Horizontal Offset",
            ref offsetX,
            -500f,
            500f,
            "%.0f px"))
        {
            configuration.QuestPopupOffsetX = offsetX;
            configuration.Save();
        }

        var offsetY = configuration.QuestPopupOffsetY;

        if (ImGui.SliderFloat(
            "Vertical Offset",
            ref offsetY,
            -300f,
            300f,
            "%.0f px"))
        {
            configuration.QuestPopupOffsetY = offsetY;
            configuration.Save();
        }

        var scalePercent =
            configuration.QuestPopupScale * 100f;

        if (ImGui.SliderFloat(
            "Popup Scale",
            ref scalePercent,
            75f,
            150f,
            "%.0f%%"))
        {
            configuration.QuestPopupScale =
                scalePercent / 100f;

            configuration.Save();
        }

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Offsets are relative to the default FFXIV popup position."
        );

        ImGui.TextDisabled(
            "100% restores the default FFXIV popup size."
        );

        ImGui.Spacing();

        if (ImGui.Button("Reset Layout"))
        {
            configuration.ResetLayout();
        }
    }

    private void DrawPresetSection()
    {
        ImGui.Text("Presets");

        ImGui.TextDisabled(
            "Apply preset text and edge color combinations."
        );

        ImGui.Spacing();

        if (ImGui.Button(
            "Plugin Default",
            new Vector2(140, 0)))
        {
            configuration.QuestTextColor = Configuration.DefaultQuestTextColor;

            configuration.QuestEdgeColor = Configuration.DefaultQuestEdgeColor;

            configuration.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button(
            "FFXIV Original",
            new Vector2(140, 0)))
        {
            configuration.QuestTextColor =
                new Vector4(
                    1.0f,
                    1.0f,
                    1.0f,
                    1.0f
                );

            configuration.QuestEdgeColor =
                new Vector4(
                    0f / 255f,
                    153f / 255f,
                    255f / 255f,
                    1.0f
                );

            configuration.Save();
        }

        if (ImGui.Button(
            "FFXIV Gold",
            new Vector2(140, 0)))
        {
            configuration.QuestTextColor =
                new Vector4(
                    232f / 255f,
                    196f / 255f,
                    110f / 255f,
                    1.0f
                );

            configuration.QuestEdgeColor =
                new Vector4(
                    76f / 255f,
                    49f / 255f,
                    20f / 255f,
                    1.0f
                );

            configuration.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button(
            "High Contrast",
            new Vector2(140, 0)))
        {
            configuration.QuestTextColor =
                new Vector4(
                    245f / 255f,
                    245f / 255f,
                    245f / 255f,
                    1.0f
                );

            configuration.QuestEdgeColor =
                new Vector4(
                    20f / 255f,
                    20f / 255f,
                    20f / 255f,
                    1.0f
                );

            configuration.Save();
        }

        if (ImGui.Button(
            "Cool Blue",
            new Vector2(140, 0)))
        {
            configuration.QuestTextColor =
                new Vector4(
                    190f / 255f,
                    220f / 255f,
                    255f / 255f,
                    1.0f
                );

            configuration.QuestEdgeColor =
                new Vector4(
                    25f / 255f,
                    55f / 255f,
                    95f / 255f,
                    1.0f
                );

            configuration.Save();
        }

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Presets change text and edge colors only."
        );
    }

    private void DrawTextureSection()
    {
        ImGui.Text("Quest Popup Textures");

        ImGui.TextDisabled(
            "Optionally replace the quest popup textures through Penumbra."
        );

        ImGui.Spacing();

        DrawTexturePreview(
            "Quest Popup Frame",
            "ScreenInfo_Preview.png",
            420f
          );

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawTexturePreview(
            "Stage Progression Complete Icon",
            "060081_hr1_preview.png",
            120f
         );

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        var penumbraAvailable =
            plugin.Penumbra.IsAvailable();

        if (penumbraAvailable)
        {
            ImGui.Text(
                "Penumbra: Available"
            );

            ImGui.TextDisabled(
                "Custom quest popup textures can be applied."
            );
        }
        else
        {
            ImGui.TextDisabled(
                "Penumbra: Not Available"
            );

            ImGui.TextDisabled(
                "Install and enable Penumbra to use custom quest popup textures."
            );
        }

        ImGui.Spacing();

        var enabled =
            configuration.EnableQuestPopupTextures;

        ImGui.BeginDisabled(
            !penumbraAvailable
        );

        if (ImGui.Checkbox(
            "Enable Custom Quest Popup Textures",
            ref enabled))
        {
            configuration.EnableQuestPopupTextures =
                enabled;

            configuration.Save();

            if (enabled)
            {
                plugin.Penumbra
                    .ApplyQuestPopupTextures();
            }
            else
            {
                plugin.Penumbra
                    .RemoveQuestPopupTextures();
            }
        }

        ImGui.EndDisabled();

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Penumbra is only required for this optional texture feature."
        );

        ImGui.TextDisabled(
            "A full FFXIV restart is recommended after enabling or disabling textures."
        );
    }

    private void DrawTestSection()
    {
        ImGui.Text("In-Game Preview");

        ImGui.TextDisabled(
            "Preview your current text colors and font size in game."
        );

        ImGui.Spacing();

        if (ImGui.Button("Show Test Popup"))
        {
            plugin.ShowTestPopup();
        }

        ImGui.SameLine();

        ImGui.TextDisabled(
            "Displays for 4 seconds."
        );
    }

    private void DrawResetSection()
    {
        ImGui.Text("Appearance Reset");

        ImGui.TextDisabled(
            "Restore the plugin's default text appearance."
        );

        ImGui.Spacing();

        if (ImGui.Button(
            "Reset Appearance to Plugin Default"))
        {
            configuration.ResetAppearance();
        }

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Reset text color, edge color, and font size."
        );
    }

    private void DrawTexturePreview(
    string label,
    string fileName,
    float maxWidth)
    {
        ImGui.Text(label);
        ImGui.Spacing();

        var assemblyDirectory =
            Plugin.PluginInterface.AssemblyLocation.DirectoryName;

        if (assemblyDirectory == null)
        {
            ImGui.TextDisabled("Preview unavailable.");
            return;
        }

        var previewPath = Path.Combine(
            assemblyDirectory,
            "Resources",
            "Previews",
            fileName
        );

        var sharedTexture =
            Plugin.TextureProvider.GetFromFile(previewPath);

        if (sharedTexture == null)
        {
            ImGui.TextDisabled("Preview unavailable.");
            return;
        }

        var texture = sharedTexture.GetWrapOrEmpty();

        var size = texture.Size;

        var scale = 1f;

        if (size.X > maxWidth)
        {
            scale = maxWidth / size.X;
        }

        var displaySize = new Vector2(
            size.X * scale,
            size.Y * scale
        );

        ImGui.Image(
            texture.Handle,
            displaySize
        );
    }


    private void DrawAboutSection()
    {
        ImGui.Text("Quest Text Recolor");

        ImGui.Spacing();

        ImGui.PushTextWrapPos(0f);
        ImGui.TextDisabled(
            "Customize FFXIV's center-screen quest objective progression text and popup layout."
        );
        ImGui.PopTextWrapPos();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        var version = typeof(Plugin).Assembly.GetName().Version;

        var versionText = version == null
            ? "Unkown"
            : $"{version.Major}.{version.Minor}.{version.Build}";

        ImGui.Text("Version");
        ImGui.TextDisabled(versionText);

        ImGui.Spacing();

        ImGui.Text("Command");
        ImGui.TextDisabled("/questtext");

        ImGui.Spacing();

        ImGui.Text("Features");
        ImGui.TextDisabled(
            "Text colors, font size, popup position, scale, presets, and optional textures."
        );

        ImGui.Spacing();

        ImGui.Text("Penumbra");
        ImGui.TextDisabled(
            "Only required for the optional custom quest popup textures."
        );

    }

}
