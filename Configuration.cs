using Dalamud.Configuration;
using System;
using System.Numerics;

namespace QuestTextRecolor;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public const int DefaultQuestFontSize = 18;

    public const float DefaultQuestPopupScale = 1.0f;

    public const float DefaultQuestPopupOffsetX = 0f;

    public const float DefaultQuestPopupOffsetY = 0f;

    public static readonly Vector4 DefaultQuestTextColor = new Vector4(
        242f / 255f,
        228f / 255f,
        196f / 255f,
        1.0f
    );

    public static readonly Vector4 DefaultQuestEdgeColor = new Vector4(
        90f / 255f,
        69f / 255f,
        38f / 255f,
        1.0f
    );

    public int Version { get; set; } = 0;

    public bool EnableQuestTextRecolor { get; set; } = true;

    public bool EnableQuestPopupTextures { get; set; } = false;


    public int QuestFontSize { get; set; } = DefaultQuestFontSize;

    public float QuestPopupOffsetX { get; set; } = DefaultQuestPopupOffsetX;
    public float QuestPopupOffsetY { get; set; } = DefaultQuestPopupOffsetY;

    public float QuestPopupScale { get; set; } = DefaultQuestPopupScale;

    public Vector4 QuestTextColor { get; set; } = DefaultQuestTextColor;


    public Vector4 QuestEdgeColor { get; set; } = DefaultQuestEdgeColor;


    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }

    public void ResetAppearance()
    {
        QuestTextColor = DefaultQuestTextColor;

        QuestEdgeColor = DefaultQuestEdgeColor;

        QuestFontSize = DefaultQuestFontSize;

        Save();
    }

    public void ResetLayout()
    {
        QuestPopupOffsetX = DefaultQuestPopupOffsetX;
        QuestPopupOffsetY = DefaultQuestPopupOffsetY;
        QuestPopupScale = DefaultQuestPopupScale;

        Save();
    }
}
