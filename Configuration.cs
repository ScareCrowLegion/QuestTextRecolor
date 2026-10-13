using Dalamud.Configuration;
using System;
using System.Numerics;

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

    public const int CurrentConfigVersion = 1;

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

    public int Version { get; set; } = CurrentConfigVersion;

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

    public bool Migrate()
    {
        var changed = false;

        if (Version == 0)
        {
            Version = 1;
            changed = true;
        }

        return changed;
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

        var popupScale = ValidateFloat(
            QuestPopupScale,
            DefaultQuestPopupScale,
            MinQuestPopupScale,
            MaxQuestPopupScale
        );


        if (popupScale != QuestPopupScale)
        {
            QuestPopupScale = popupScale;
            changed = true;
        }

        var offsetX = ValidateFloat(
            QuestPopupOffsetX,
            DefaultQuestPopupOffsetX,
            MinQuestPopupOffsetX,
            MaxQuestPopupOffsetX
        );

        if (offsetX != QuestPopupOffsetX)
        {
            QuestPopupOffsetX = offsetX;
            changed = true;
        }

        var offsetY = ValidateFloat(
            QuestPopupOffsetY,
            DefaultQuestPopupOffsetY,
            MinQuestPopupOffsetY,
            MaxQuestPopupOffsetY
         );

        if (offsetY != QuestPopupOffsetY)
        {
            QuestPopupOffsetY = offsetY;
            changed = true;
        }

        var textColor = ValidateColor(
            QuestTextColor,
            DefaultQuestTextColor
        );

        if (textColor != QuestTextColor)
        {
            QuestTextColor = textColor;
            changed = true;
        }

        var edgeColor = ValidateColor(
            QuestEdgeColor,
            DefaultQuestEdgeColor
        );

        if (edgeColor != QuestEdgeColor)
        {
            QuestEdgeColor = edgeColor;
            changed = true;
        }

        return changed;
    }

    private static float ValidateFloat(
        float value,
        float defaultValue,
        float minValue,
        float maxValue)
    {
        if (!float.IsFinite(value))
        {
            return defaultValue;
        }

        return Math.Clamp(
            value,
            minValue,
            maxValue
            );
    }

    private static Vector4 ValidateColor(
        Vector4 value,
        Vector4 defaultValue)
    {
        return new Vector4(
            ValidateFloat(value.X, defaultValue.X, 0f, 1f),
            ValidateFloat(value.Y, defaultValue.Y, 0f, 1f),
            ValidateFloat(value.Z, defaultValue.Z, 0f, 1f),
            ValidateFloat(value.W, defaultValue.W, 0f, 1f)
        );
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
