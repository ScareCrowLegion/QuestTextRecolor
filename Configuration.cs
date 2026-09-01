using Dalamud.Configuration;
using System;
using System.Numerics;

namespace QuestTextRecolor;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool EnableQuestTextRecolor { get; set; } = true;

    public Vector4 QuestTextColor { get; set; } = new Vector4(
        242f / 255f,
        228f / 255f,
        196f / 255f,
        1.0f
    );

    public Vector4 QuestEdgeColor { get; set; } = new Vector4(
        90f / 255f,
        69f / 255f,
        38f / 255f,
        1.0f
    );

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }

    public void ResetColors()
    {
        QuestTextColor = new Vector4(
            242f / 255f,
            228f / 255f,
            196f / 255f,
            1.0f
        );

        QuestEdgeColor = new Vector4(
            90f / 255f,
            69f / 255f,
            38f / 255f,
            1.0f
        );

        Save();
    }
}
