using System.Reflection;

namespace Dalamud.Configuration
{
    public interface IPluginConfiguration
    {
        int Version { get; set; }
    }
}

namespace QuestTextRecolor
{
    public static class Plugin
    {
        public static TestPluginInterface PluginInterface { get; } = new();
    }

    public sealed class TestPluginInterface
    {
        public int SaveCount { get; private set; }

        public void SavePluginConfig(Configuration configuration)
        {
            SaveCount++;
        }

        public void ResetSaveCount()
        {
            SaveCount = 0;
        }
    }
}