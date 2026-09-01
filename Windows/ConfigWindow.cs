using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace QuestTextRecolor.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    // We give this window a constant ID using ###.
    // This allows for labels to be dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("Quest Text Recolor###QuestTextRecolorConfig")
    {
        Flags = ImGuiWindowFlags.NoResize |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(360, 185);
        SizeCondition = ImGuiCond.Always;

        configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var enabled = configuration.EnableQuestTextRecolor;
        if (ImGui.Checkbox("Enable Quest Text Recolor", ref enabled))
        {
            configuration.EnableQuestTextRecolor = enabled;
            configuration.Save();
        }

        ImGui.Separator();

        var textColor = configuration.QuestTextColor;
        if (ImGui.ColorEdit4("Quest Text Color", ref textColor))
        {
            configuration.QuestTextColor = textColor;
            configuration.Save();
        }

        ImGui.TextDisabled("Default: #F2E4C4FF");

        var edgeColor = configuration.QuestEdgeColor;
        if (ImGui.ColorEdit4("Quest Edge Color", ref edgeColor))
        {
            configuration.QuestEdgeColor = edgeColor;
            configuration.Save();
        }

        ImGui.TextDisabled("Default: #5A4526FF");

        ImGui.Separator();

        ImGui.Text("Preview:");

        var previewColor = ImGui.ColorConvertFloat4ToU32(configuration.QuestTextColor);

        ImGui.TextColored(
            configuration.QuestTextColor,
            "Quest objective preview text"
        );

        if (ImGui.Button("Reset to Defaults"))
        {
            configuration.ResetColors();
        }
    }
}
