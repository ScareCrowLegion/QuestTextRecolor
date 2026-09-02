using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace QuestTextRecolor.Windows;

public class ConfigWindow : Window
{
    private readonly Plugin plugin;
    private readonly Configuration configuration;

    public ConfigWindow(Plugin plugin)
        : base("Quest Text Recolor")
    {
        this.plugin = plugin;
        configuration = plugin.Configuration;

        Flags = ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoCollapse;

        Size = new Vector2(500, 520);
        SizeCondition = ImGuiCond.Always;
    }

    public override void Draw()
    {
        DrawPluginSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawColorSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawTextStyleSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawPresetSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawTextureSection();

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

        if (ImGui.Checkbox("Enable Quest Text Recolor", ref enabled))
        {
            configuration.EnableQuestTextRecolor = enabled;
            configuration.Save();
        }

        ImGui.TextDisabled(
            "Recolors the center-screen quest objective progression text."
        );
    }

    private void DrawColorSection()
    {
        ImGui.Text("Colors");

        var textColor = configuration.QuestTextColor;

        if (ImGui.ColorEdit4("Text Color", ref textColor))
        {
            configuration.QuestTextColor = textColor;
            configuration.Save();
        }

        ImGui.TextDisabled("Plugin Default: #F2E4C4FF");

        ImGui.Spacing();

        var edgeColor = configuration.QuestEdgeColor;

        if (ImGui.ColorEdit4("Edge Color", ref edgeColor))
        {
            configuration.QuestEdgeColor = edgeColor;
            configuration.Save();
        }

        ImGui.TextDisabled("Plugin Default: #5A4526FF");
    }

    private void DrawTextStyleSection()
    {
        ImGui.Text("Text Style");

        var fontSize = configuration.QuestFontSize;

        if (ImGui.SliderInt("Font Size", ref fontSize, 12, 28))
        {
            configuration.QuestFontSize = fontSize;
            configuration.Save();
        }

        ImGui.TextDisabled("Plugin Default: 18");
    }

    private void DrawPresetSection()
    {
        ImGui.Text("Presets");

        if (ImGui.Button("Plugin Default", new Vector2(140, 0)))
        {
            configuration.QuestTextColor = new Vector4(
                242f / 255f,
                228f / 255f,
                196f / 255f,
                1.0f
            );

            configuration.QuestEdgeColor = new Vector4(
                90f / 255f,
                69f / 255f,
                38f / 255f,
                1.0f
            );

            configuration.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button("FFXIV Original", new Vector2(140, 0)))
        {
            configuration.QuestTextColor = new Vector4(
                1.0f,
                1.0f,
                1.0f,
                1.0f
            );

            configuration.QuestEdgeColor = new Vector4(
                0f / 255f,
                153f / 255f,
                255f / 255f,
                1.0f
            );

            configuration.Save();
        }

        if (ImGui.Button("FFXIV Gold", new Vector2(140, 0)))
        {
            configuration.QuestTextColor = new Vector4(
                232f / 255f,
                196f / 255f,
                110f / 255f,
                1.0f
            );

            configuration.QuestEdgeColor = new Vector4(
                76f / 255f,
                49f / 255f,
                20f / 255f,
                1.0f
            );

            configuration.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button("High Contrast", new Vector2(140, 0)))
        {
            configuration.QuestTextColor = new Vector4(
                245f / 255f,
                245f / 255f,
                245f / 255f,
                1.0f
            );

            configuration.QuestEdgeColor = new Vector4(
                20f / 255f,
                20f / 255f,
                20f / 255f,
                1.0f
            );

            configuration.Save();
        }

        if (ImGui.Button("Cool Blue", new Vector2(140, 0)))
        {
            configuration.QuestTextColor = new Vector4(
                190f / 255f,
                220f / 255f,
                255f / 255f,
                1.0f
            );

            configuration.QuestEdgeColor = new Vector4(
                25f / 255f,
                55f / 255f,
                95f / 255f,
                1.0f
            );

            configuration.Save();
        }

        ImGui.TextDisabled(
            "Presets update both text and edge colors."
        );
    }

    private void DrawTextureSection()
    {
        ImGui.Text("Quest Popup Textures");

        ImGui.Spacing();

        var penumbraAvailable = plugin.Penumbra.IsAvailable();

        if (penumbraAvailable)
        {
            ImGui.Text("Penumbra: Available");
        }
        else
        {
            ImGui.TextDisabled("Penumbra: Not Available");
        }

        ImGui.Spacing();

        var enabled = configuration.EnableQuestPopupTextures;

        ImGui.BeginDisabled(!penumbraAvailable);

        if (ImGui.Checkbox(
            "Enable Custom Quest Popup Textures",
            ref enabled))
        {
            configuration.EnableQuestPopupTextures = enabled;
            configuration.Save();

            if (enabled)
            {
                plugin.Penumbra.ApplyQuestPopupTextures();
            }
            else
            {
                plugin.Penumbra.RemoveQuestPopupTextures();
            }
        }

        ImGui.EndDisabled();

        ImGui.TextDisabled("Requires Penumbra.");
        ImGui.TextDisabled("Restart FFXIV after enabling or disabling.");
    }



    private void DrawTestSection()
    {
        ImGui.Text("In-Game Preview");

        ImGui.TextDisabled(
            "Preview your current colors and font size on screen."
        );

        if (ImGui.Button("Show Test Popup"))
        {
            plugin.ShowTestPopup();
        }

        ImGui.SameLine();

        ImGui.TextDisabled("Displays for 4 seconds.");
    }

    private void DrawResetSection()
    {
        if (ImGui.Button("Reset Appearance to Plugin Default"))
        {
            configuration.ResetAppearance();
        }

        ImGui.SameLine();

        ImGui.TextDisabled(
            "Restores plugin default colors and font size."
        );
    }
}