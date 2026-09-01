using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace QuestTextRecolor.Windows;

class TestPopupWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    private DateTime hideAt;

    public TestPopupWindow(Configuration configuration)
        : base("Quest Text Recolor Test Popup###QuestTextRecolorTestPopup")
    {
        this.configuration = configuration;

        Flags =
            ImGuiWindowFlags.NoTitleBar |
            ImGuiWindowFlags.NoResize |
            ImGuiWindowFlags.NoMove |
            ImGuiWindowFlags.NoScrollbar |
            ImGuiWindowFlags.NoScrollWithMouse |
            ImGuiWindowFlags.NoBackground |
            ImGuiWindowFlags.NoInputs;

        IsOpen = false;
    }

    public void Show()
    {
        hideAt = DateTime.UtcNow.AddSeconds(4);
        IsOpen = true;
    }

    public override void Draw()
    {
        if (DateTime.UtcNow >= hideAt)
        {
            IsOpen = false;
            return;
        }

        var viewport = ImGui.GetMainViewport();

        const string testText = "Wait at the designated location.";

        var font = ImGui.GetFont();
        var fontSize = (float)configuration.QuestFontSize;

        // Give the transparent popup plenty of drawing room.
        var popupWidth = 800f;
        var popupHeight = 120f;

        ImGui.SetWindowSize(
            "Quest Text Recolor Test Popup###QuestTextRecolorTestPopup",
            new Vector2(popupWidth, popupHeight),
            ImGuiCond.Always
        );

        // Center the popup horizontally.
        ImGui.SetWindowPos(
            "Quest Text Recolor Test Popup###QuestTextRecolorTestPopup",
            new Vector2(
                viewport.Pos.X + (viewport.Size.X / 2f) - (popupWidth / 2f),
                viewport.Pos.Y + (viewport.Size.Y * 0.35f)
            ),
            ImGuiCond.Always
        );

        var drawList = ImGui.GetWindowDrawList();

        // Add some padding inside the transparent window.
        var baseTextSize = ImGui.CalcTextSize(testText);
        var scale = fontSize / ImGui.GetFontSize();

        var textWidth = baseTextSize.X * scale;

        var position = new Vector2(
           viewport.Pos.X + (viewport.Size.X / 2f) - (textWidth / 2f),
           viewport.Pos.Y + (viewport.Size.Y * 0.35f) + 20f
);


        var textColor =
            ImGui.ColorConvertFloat4ToU32(configuration.QuestTextColor);

        var edgeColor =
            ImGui.ColorConvertFloat4ToU32(configuration.QuestEdgeColor);

        // Outline
        drawList.AddText(
            font,
            fontSize,
            position + new Vector2(-1, -1),
            edgeColor,
            testText
        );

        drawList.AddText(
            font,
            fontSize,
            position + new Vector2(0, -1),
            edgeColor,
            testText
        );

        drawList.AddText(
            font,
            fontSize,
            position + new Vector2(1, -1),
            edgeColor,
            testText
        );

        drawList.AddText(
            font,
            fontSize,
            position + new Vector2(-1, 0),
            edgeColor,
            testText
        );

        drawList.AddText(
            font,
            fontSize,
            position + new Vector2(1, 0),
            edgeColor,
            testText
        );

        drawList.AddText(
            font,
            fontSize,
            position + new Vector2(-1, 1),
            edgeColor,
            testText
        );

        drawList.AddText(
            font,
            fontSize,
            position + new Vector2(0, 1),
            edgeColor,
            testText
        );

        drawList.AddText(
            font,
            fontSize,
            position + new Vector2(1, 1),
            edgeColor,
            testText
        );

        // Main text
        drawList.AddText(
            font,
            fontSize,
            position,
            textColor,
            testText
        );

        ImGui.Dummy(
            new Vector2(
                popupWidth - 40f,
                popupHeight - 40f
            )
        );
    }

    public void Dispose()
    {
    }
}