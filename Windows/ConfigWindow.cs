using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace QuestTextRecolor.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;
    private readonly Plugin plugin;


    public ConfigWindow(Plugin plugin)
        : base("Quest Text Recolor###QuestTextRecolorConfig")
    {
        this.plugin = plugin;

        Flags = ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoCollapse;

        Size = new Vector2(500, 520);
        SizeCondition = ImGuiCond.Always;

        configuration = plugin.Configuration;
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        DrawHeader();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawEnableSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawColorSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawFontSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawPresetSection();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        DrawTestSection();
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();


        DrawResetSection();
    }

    private void DrawHeader()
    {
        ImGui.Text("Quest Text Recolor");

        ImGui.TextDisabled(
            "Customize center-screen quest objective text colors."
        );
    }

    private void DrawEnableSection()
    {
        ImGui.Text("Plugin");

        ImGui.Spacing();

        var enabled = configuration.EnableQuestTextRecolor;

        if (ImGui.Checkbox("Enable Quest Text Recolor", ref enabled))
        {
            configuration.EnableQuestTextRecolor = enabled;
            configuration.Save();
        }

        ImGui.TextDisabled(
            "Controls whether custom quest text colors are applied."
        );
    }

    private void DrawColorSection()
    {
        ImGui.Text("Colors");

        ImGui.Spacing();

        var textColor = configuration.QuestTextColor;

        ImGui.SetNextItemWidth(180);

        if (ImGui.ColorEdit4("Quest Text Color", ref textColor))
        {
            configuration.QuestTextColor = textColor;
            configuration.Save();
        }

        ImGui.TextDisabled("Plugin Default: #F2E4C4FF");

        ImGui.Spacing();

        var edgeColor = configuration.QuestEdgeColor;

        ImGui.SetNextItemWidth(180);

        if (ImGui.ColorEdit4("Quest Edge Color", ref edgeColor))
        {
            configuration.QuestEdgeColor = edgeColor;
            configuration.Save();
        }

        ImGui.TextDisabled("Plugin Default: #5A4526FF");
    }

    private void DrawTestSection()
    {
        ImGui.Text("In-Game Preview");

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Preview your current colors and font size on screen."
        );

        ImGui.Spacing();

        if (ImGui.Button("Show Test Popup"))
        {
            plugin.ShowTestPopup();
        }

        ImGui.SameLine();

        ImGui.TextDisabled("Displays for 4 seconds.");
    }
    private void DrawFontSection()
    {
        ImGui.Text("Text Style");

        ImGui.Spacing();

        var fontSize = configuration.QuestFontSize;

        ImGui.SetNextItemWidth(180);

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

        ImGui.Spacing();

        // Row 1
        if (ImGui.Button("Plugin Default", new Vector2(140, 0)))
        {
            ApplyPreset(
                new Vector4(242f / 255f, 228f / 255f, 196f / 255f, 1.0f),
                new Vector4(90f / 255f, 69f / 255f, 38f / 255f, 1.0f)
            );
        }

        ImGui.SameLine();

        if (ImGui.Button("FFXIV Original", new Vector2(140, 0)))
        {
            ApplyPreset(
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                new Vector4(0f / 255f, 153f / 255f, 255f / 255f, 1.0f)
            );
        }

        // Row 2
        if (ImGui.Button("FFXIV Gold", new Vector2(140, 0)))
        {
            ApplyPreset(
                new Vector4(232f / 255f, 196f / 255f, 110f / 255f, 1.0f),
                new Vector4(76f / 255f, 49f / 255f, 20f / 255f, 1.0f)
            );
        }

        ImGui.SameLine();

        if (ImGui.Button("High Contrast", new Vector2(140, 0)))
        {
            ApplyPreset(
                new Vector4(245f / 255f, 245f / 255f, 245f / 255f, 1.0f),
                new Vector4(20f / 255f, 20f / 255f, 20f / 255f, 1.0f)
            );
        }

        // Row 3
        if (ImGui.Button("Cool Blue", new Vector2(140, 0)))
        {
            ApplyPreset(
                new Vector4(190f / 255f, 220f / 255f, 255f / 255f, 1.0f),
                new Vector4(25f / 255f, 55f / 255f, 95f / 255f, 1.0f)
            );
        }

        ImGui.Spacing();

        ImGui.TextDisabled(
            "Presets update both text and edge colors."
        );
    }

    private void ApplyPreset(Vector4 textColor, Vector4 edgeColor)
    {
        configuration.QuestTextColor = textColor;
        configuration.QuestEdgeColor = edgeColor;
        configuration.Save();
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