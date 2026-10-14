using System;
using System.IO;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using Lumina.Excel.Sheets;


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

        Size = new Vector2(820, 580);
        SizeCondition = ImGuiCond.Always;
    }

    public override void Draw()
    {
        DrawHeader();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        ImGui.PushStyleVar(
            ImGuiStyleVar.ChildRounding,
            8f
            );

        ImGui.PushStyleColor(
            ImGuiCol.ChildBg,
            new Vector4(
                0.08f,
                0.08f,
                0.10f,
                0.72f
                )
            );

        ImGui.BeginChild(
            "Navigation",
            new Vector2(180f, 0f),
            true
           );

        DrawNavigation();

        ImGui.EndChild();

        ImGui.SameLine();

        ImGui.BeginChild(
            "Content",
            new Vector2(0f, 0f),
            true
            );

        DrawCurrentPage();

        ImGui.EndChild();

        ImGui.PopStyleColor();
        ImGui.PopStyleVar();

    }

    private void DrawHeader()
    {
        ImGui.TextColored(
            new Vector4(
                0.95f,
                0.89f,
                0.72f,
                1.0f
                ),
            "QUEST TEXT RECOLOR"
            );

        ImGui.TextDisabled(
            "Customize your quest objective popup."
            );

        ImGui.SameLine();

        var availableWidth =
            ImGui.GetContentRegionAvail().X;

        if (availableWidth > 190f)
        {
            ImGui.SetCursorPosX(
                ImGui.GetCursorPosX() +
                availableWidth -
                185f
                );

            if (configuration.EnableQuestTextRecolor)
            {
                ImGui.TextColored(
                    new Vector4(
                        0.40f,
                        0.85f,
                        0.55f,
                        1.0f
                        ),
                    "● ACTIVE"
                    );
            }
            else
            {
                ImGui.TextDisabled(
                    "● DISABLED"
                    );
            }
        }
    }

    private void DrawNavigation()
    {
        ImGui.TextDisabled("CUSTOMIZATION");

        ImGui.Spacing();

        DrawNavigationItem(
            "General",
            ConfigPage.General
        );

        DrawNavigationItem(
            "Colors",
            ConfigPage.Colors
        );

        DrawNavigationItem(
            "Text Style",
            ConfigPage.TextStyle
        );

        DrawNavigationItem(
            "Popup Layout",
            ConfigPage.PopupLayout
        );

        DrawNavigationItem(
            "Presets",
            ConfigPage.Presets
        );

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        ImGui.TextDisabled("INTEGRATION");

        ImGui.Spacing();

        DrawNavigationItem(
            "Textures",
            ConfigPage.Textures
        );

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        ImGui.TextDisabled("PLUGIN");

        ImGui.Spacing();

        DrawNavigationItem(
            "About",
            ConfigPage.About
        );
    }

    private void DrawNavigationItem(string label, ConfigPage page)
    {
        var selected =
            currentPage == page;

        if (selected)
        {
            ImGui.PushStyleColor(
                ImGuiCol.Header,
                new Vector4(
                    0.26f,
                    0.35f,
                    0.48f,
                    0.90f
                )
            );

            ImGui.PushStyleColor(
                ImGuiCol.HeaderHovered,
                new Vector4(
                    0.30f,
                    0.40f,
                    0.55f,
                    1.0f
                )
            );
        }

        if (ImGui.Selectable(
            label,
            selected,
            ImGuiSelectableFlags.None,
            new Vector2(0f, 32f)))
        {
            currentPage = page;
        }

        if (selected)
        {
            ImGui.PopStyleColor(2);
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

    private void DrawPageHeader(string title, string description)
    {
        ImGui.TextColored(
            new Vector4(
                0.95f,
                0.89f,
                0.72f,
                1.0f
            ),
            title
        );

        ImGui.TextDisabled(description);

        ImGui.Spacing();
        ImGui.Spacing();
    }

    private void BeginSettingsCard(string id, string title, string description, float height)
    {
        ImGui.PushStyleVar(
            ImGuiStyleVar.ChildRounding,
            6f
         );

        ImGui.PushStyleColor(
            ImGuiCol.ChildBg,
            new Vector4(
                0.12f,
                0.12f,
                0.15f,
                0.88f
            )
        );

        ImGui.PushStyleColor(
            ImGuiCol.Border,
            new Vector4(
                0.26f,
                0.28f,
                0.34f,
                0.70f
            )
        );

        ImGui.BeginChild(
            id,
            new Vector2(0f, height),
            true
        );

        ImGui.Text(title);

        ImGui.TextDisabled(description);

        ImGui.Spacing();
    }

    private void EndSettingsCard()
    {
        ImGui.EndChild();

        ImGui.PopStyleColor(2);
        ImGui.PopStyleVar();
    }

    private void DrawGeneralPage()
    {
        DrawPageHeader(
            "General",
            "Plugin controls, preview tools, and appearance reset."
            );

        DrawPluginSection();

        ImGui.Spacing();

        DrawTestSection();

        ImGui.Spacing();

        DrawResetSection();
    }

    private void DrawPluginSection()
    {
        BeginSettingsCard(
            "GeneralPluginCard",
            "PLUGIN",
            "Control whether Quest Text Recolor is currently active.",
            100f
        );

        var enabled =
            configuration.EnableQuestTextRecolor;

        if (ImGui.Checkbox(
            "Enable Quest Text Recolor",
            ref enabled))
        {
            configuration.EnableQuestTextRecolor =
                enabled;

            configuration.Save();
        }

        EndSettingsCard();
    }

    private void DrawColorSection()
    {
        DrawPageHeader(
            "Colors",
            "Customize the quest text and outline colors."
        );

        BeginSettingsCard(
            "TextColorCard",
            "TEXT COLOR",
            "Choose the primary color used for quest objective text.",
            125f
        );

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

        EndSettingsCard();

        ImGui.Spacing();

        BeginSettingsCard(
            "EdgeColorCard",
            "EDGE COLOR",
            "Choose the outline color used around quest objective text.",
            125f
        );

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

        EndSettingsCard();
    }

    private void DrawTextStyleSection()
    {
        DrawPageHeader(
            "Text Style",
            "Adjust the size of the quest objective text."
         );

        BeginSettingsCard(
            "FontSizeCard",
            "FONT SIZE",
            "Adjust the size of the quest objective text.",
            105f
        );

        var fontSize = configuration.QuestFontSize;

        if (ImGui.SliderInt(
            "Font Size",
            ref fontSize,
            Configuration.MinQuestFontSize,
            Configuration.MaxQuestFontSize))

        {
            configuration.QuestFontSize = fontSize;
            configuration.Save();
        }

        ImGui.TextDisabled(
            "Plugin Default: 18"
        );

        EndSettingsCard();
    }

    private void DrawPositionSection()
    {
        DrawPageHeader(
            "Popup Layout",
            "Adjust the position and size of the quest popup."
        );

        BeginSettingsCard(
            "PopupPositionCard",
            "POSITION",
            "Move the quest popup relative to its default FFXIV position.",
            165f
         );

        var offsetX = configuration.QuestPopupOffsetX;

        if (ImGui.SliderFloat(
            "Horizontal Offset",
            ref offsetX,
            Configuration.MinQuestPopupOffsetX,
            Configuration.MaxQuestPopupOffsetX,
            "%.0f px"))
        {
            configuration.QuestPopupOffsetX = offsetX;
            configuration.Save();
        }

        var offsetY = configuration.QuestPopupOffsetY;

        if (ImGui.SliderFloat(
            "Vertical Offset",
            ref offsetY,
            Configuration.MinQuestPopupOffsetY,
            Configuration.MaxQuestPopupOffsetY,
            "%.0f px"))
        {
            configuration.QuestPopupOffsetY = offsetY;
            configuration.Save();
        }

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Offsets are relative to the default FFXIV popup position."
         );

        EndSettingsCard();

        ImGui.Spacing();

        BeginSettingsCard(
            "PopupScaleCard",
            "SCALE",
            "Adjust the overall size of the quest popup.",
            120f
         );

        var scalePercent =
            configuration.QuestPopupScale * 100f;

        if (ImGui.SliderFloat(
            "Popup Scale",
            ref scalePercent,
            Configuration.MinQuestPopupScale * 100f,
            Configuration.MaxQuestPopupScale * 100f,
            "%.0f%"))
        {
            configuration.QuestPopupScale =
                scalePercent / 100f;

            configuration.Save();
        }

        ImGui.Spacing();

        ImGui.TextDisabled(
            "100% restores the default FFXIV popup size."
        );

        EndSettingsCard();

        ImGui.Spacing();

        BeginSettingsCard(
            "PopupResetCard",
            "RESET LAYOUT",
            "Restore popup position and scale to their plugin defaults.",
            105f
        );

        if (ImGui.Button(
            "Reset Layout",
            new Vector2(140f, 0f)))
        {
            configuration.ResetLayout();
        }

        EndSettingsCard();
    }

    private void DrawPresetSection()
    {
        DrawPageHeader(
            "Presets",
            "Apply preset text and edge color combinations."
        );

        BeginSettingsCard(
            "ColorPresetsCard",
            "COLOR PRESETS",
            "Quickly apply a matching text and outline color combination.",
            205f
        );

        if (ImGui.Button(
            "Plugin Default",
            new Vector2(170f, 0f)))
        {
            configuration.QuestTextColor = Configuration.DefaultQuestTextColor;

            configuration.QuestEdgeColor = Configuration.DefaultQuestEdgeColor;

            configuration.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button(
            "FFXIV Original",
            new Vector2(170f, 0f)))
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
            new Vector2(170f, 0f)))
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
            new Vector2(170f, 0f)))
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
            new Vector2(170f, 0f)))
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

        EndSettingsCard();
    }

    private void DrawTextureSection()
    {
        DrawPageHeader(
            "Textures",
            "Preview and enable the optional custom quest popup textures."
         );

        DrawTexturePreview(
            "Quest Popup Frame",
            "ScreenInfo_Preview.png",
            360f,
            105f
          );

        ImGui.Separator();

        DrawTexturePreview(
            "Stage Progression Complete Icon",
            "060081_hr1_preview.png",
            120f,
            65f
         );

        ImGui.Separator();

        var penumbraAvailable =
            plugin.Penumbra.IsAvailable();

        BeginSettingsCard(
            "PenumbraIntergrationCard",
            "PENUMBRA INTERGRATION",
            "Control whether the custom quest popup textures are applied.",
            180f
        );


        if (penumbraAvailable)
        {
            ImGui.TextColored(
                new Vector4(
                    0.40f,
                    0.85f,
                    0.55f,
                    1.0f
                ),
                "● PENUMBRA AVAILABLE"
            );

            ImGui.TextDisabled(
                "Custom quest popup textures can be applied."
            );
        }
        else
        {
            ImGui.TextDisabled(
                "● PENUMBRA NOT AVAILABLE"
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
         var previousEnabled =
          configuration.EnableQuestPopupTextures;

          configuration.EnableQuestPopupTextures = enabled;

          if (plugin.Penumbra.RefreshQuestPopupTextures())
        {
            configuration.Save();
        }
          else
        {
            configuration.EnableQuestPopupTextures = previousEnabled;
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

        EndSettingsCard();
    }

    private void DrawTestSection()
    {
        BeginSettingsCard(
            "GeneralPreviewCard",
            "IN-GAME PREVIEW",
            "Preview your current colors, font size, scale, and position.",
            135f
        );

        if (ImGui.Button(
            "Short",
            new Vector2(110f, 0f)))
        {
            plugin.ShowTestPopup(
                "Objective complete."
            );
        }

        ImGui.SameLine();

        if (ImGui.Button(
            "Standard",
            new Vector2(110f, 0f)))
        {
            plugin.ShowTestPopup(
                "Wait at the designated location."
            );
        }

        ImGui.SameLine();

        if (ImGui.Button(
            "Long",
            new Vector2(110f, 0f)))
        {
            plugin.ShowTestPopup(
                "Investigate the disturbance and report your findings."
            );
        }

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Choose a sample length. Preview displays for 4 seconds."
        );

        EndSettingsCard();
    }

    private void DrawResetSection()
    {
        BeginSettingsCard(
            "GeneralResetCard",
            "APPEARANCE RESET",
            "Restore the plugin's default text appearance.",
            115f
        );

        if (ImGui.Button(
            "Reset Appearance",
            new Vector2(180f, 0f)))
        {
            configuration.ResetAppearance();
        }

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Resets text color, edge color, and font size."
        );

        EndSettingsCard();
    }

    private void DrawTexturePreview(string label, string fileName, float maxWidth, float maxHeight)
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

        var widthScale = maxWidth / size.X;

        var heightScale = maxHeight / size.Y;

        var scale = MathF.Min(1f, MathF.Min(widthScale, heightScale));

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
        DrawPageHeader(
            "About",
            "Plugin information, version details, and intergration notes."
        );

        BeginSettingsCard(
            "AboutPluginCard",
            "QUEST TEXT RECOLOR",
            "Customize FFXIV's center-screen quest objective progression text and popup layout.",
            115f
        );

        ImGui.TextDisabled(
            "Features: colors, font size, popup position, scale, presets, and optional textures."
        );

        EndSettingsCard();

        ImGui.Spacing();

        BeginSettingsCard(
            "AboutBuildCard",
            "BUILD INFORMATION",
            "Current plugin version and command.",
            150f
        );

        var version = typeof(Plugin).Assembly.GetName().Version;

        var versionText = version == null
            ? "Unknown"
            : $"{version.Major}.{version.Minor}.{version.Build}";

        ImGui.Text("Version");
        ImGui.TextDisabled(versionText);

        ImGui.Spacing();

        ImGui.Text("Command");
        ImGui.TextDisabled("/questtext");

        EndSettingsCard();

        ImGui.Spacing();

        BeginSettingsCard(
            "AboutPenumbraCard",
            "PENUMBRA",
            "Optional intergration used for custom quest popup textures.",
            105f
         );

        ImGui.TextDisabled("Penumbra is not required for text, color, position, scale or preset customization.");

        EndSettingsCard();
    }

}
