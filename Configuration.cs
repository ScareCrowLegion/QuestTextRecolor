using Dalamud.Configuration;
using FFXIVClientStructs.FFXIV.Client.Game.InstanceContent;
using Penumbra.Api.IpcSubscribers;
using System;
using System.Numerics;
using System.Threading.Channels;

namespace QuestTextRecolor;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public const int DefaultQuestFontSize = 18;

    public const int MinQuestFontSize = 12;

    public const int MaxQuestFontSize = 28;

    public const float DefaultQuestPopupScale = 1.0f;

    public const float MinQuestPopupScale = 0.75f;

    public const float MaxQuestPopupScale = 1.50f;

    public const float DefaultQuestPopupOffsetX = 0f;

    public const float MinQuestPopupOffsetX = -500f;

    public const float MaxQuestPopupOffsetX = 500f;

    public const float DefaultQuestPopupOffsetY = 0f;

    public const float MinQuestPopupOffsetY = -500f;

    public const float MaxQuestPopupOffsetY = 500f;


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

    public bool Validate()
    {
        var changed = false;

        var fontSize = Math.Clamp(
           QuestFontSize,
           MinQuestFontSize,
           MaxQuestFontSize
        );

        if (fontSize != QuestFontSize)
        {
            QuestFontSize = fontSize;
            changed = true;
        }

        var popupScale = Math.Clamp(
            QuestPopupScale,
            MinQuestPopupScale,
            MaxQuestPopupScale
        );

        if (popupScale != QuestPopupScale)
        {
            QuestPopupScale = popupScale;
            changed = true;
        }

        var offsetX = Math.Clamp(
            QuestPopupOffsetX,
            MinQuestPopupOffsetX,
            MaxQuestPopupOffsetX
        );

        if (offsetX != QuestPopupOffsetX)
        {
            QuestPopupOffsetX = offsetX;
            changed = true;
        }

        var offsetY = Math.Clamp(
            QuestPopupOffsetY,
            MinQuestPopupOffsetY,
            MaxQuestPopupOffsetY
         );

        if (offsetY != QuestPopupOffsetY)
        {
            QuestPopupOffsetY = offsetY;
            changed = true;
        }

        return changed;
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
