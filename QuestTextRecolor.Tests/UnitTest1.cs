
using System.Numerics;
using QuestTextRecolor;
using Xunit;

namespace QuestTextRecolor.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void DefaultConfigurationHasExpectedValues()
        {
            var config = new Configuration();

            Assert.Equal(Configuration.CurrentConfigVersion, config.Version);
            Assert.True(config.EnableQuestTextRecolor);
            Assert.False(config.EnableQuestPopupTextures);

            Assert.Equal(Configuration.DefaultQuestFontSize, config.QuestFontSize);
            Assert.Equal(Configuration.DefaultQuestPopupScale, config.QuestPopupScale);
            Assert.Equal(Configuration.DefaultQuestPopupOffsetX, config.QuestPopupOffsetX);
            Assert.Equal(Configuration.DefaultQuestPopupOffsetY, config.QuestPopupOffsetY);

            Assert.Equal(Configuration.DefaultQuestTextColor, config.QuestTextColor);
            Assert.Equal(Configuration.DefaultQuestEdgeColor, config.QuestEdgeColor);
        }


        [Fact]
        public void LoadedOldConfigurationMigratesWithoutLosingSettings()
        {
            var original = new Configuration
            {
                Version = 0,
                EnableQuestTextRecolor = false,
                EnableQuestPopupTextures = true,
                QuestFontSize = 23,
                QuestPopupScale = 1.2f,
                QuestPopupOffsetX = 90f,
                QuestPopupOffsetY = -45f,
                QuestTextColor = new Vector4(0.2f, 0.5f, 0.8f, 1f),
                QuestEdgeColor = new Vector4(0.1f, 0.2f, 0.3f, 1f)
            };

            var options = new System.Text.Json.JsonSerializerOptions
            {
                IncludeFields = true
            };

            string json = System.Text.Json.JsonSerializer.Serialize(original, options);

            var loaded = System.Text.Json.JsonSerializer.Deserialize<Configuration>(
                json, options);

            Assert.NotNull(loaded);

            bool migrated = loaded.Migrate();

            Assert.True(migrated);
            Assert.Equal(Configuration.CurrentConfigVersion, loaded.Version);

            Assert.False(loaded.EnableQuestTextRecolor);
            Assert.True(loaded.EnableQuestPopupTextures);
            Assert.Equal(23, loaded.QuestFontSize);
            Assert.Equal(1.2f, loaded.QuestPopupScale);
            Assert.Equal(90f, loaded.QuestPopupOffsetX);
            Assert.Equal(-45f, loaded.QuestPopupOffsetY);
            Assert.Equal(original.QuestTextColor, loaded.QuestTextColor);
            Assert.Equal(original.QuestEdgeColor, loaded.QuestEdgeColor);
        }


        [Fact]
        public void ResetAppearanceSavesExactlyOnce()
        {
            var config = new Configuration
            {
                QuestFontSize = 25,
                QuestTextColor = new Vector4(1f, 0f, 0f, 1f)
            };

            Plugin.PluginInterface.ResetSaveCount();

            config.ResetAppearance();

            Assert.Equal(1, Plugin.PluginInterface.SaveCount);
        }

        [Fact]
        public void ResetLayoutSavesExactlyOnce()
        {
            var config = new Configuration
            {
                QuestPopupScale = 1.4f,
                QuestPopupOffsetX = 100f,
                QuestPopupOffsetY = -50f
            };

            Plugin.PluginInterface.ResetSaveCount();

            config.ResetLayout();

            Assert.Equal(1, Plugin.PluginInterface.SaveCount);
        }


        [Fact]
        public void MigrationPreservesCustomSettings()
        {
            var customColor = new Vector4(0.2f, 0.4f, 0.6f, 1f);

            var config = new Configuration
            {
                Version = 0,
                QuestFontSize = 24,
                QuestPopupScale = 1.25f,
                QuestTextColor = customColor,
                EnableQuestTextRecolor = false
            };

            bool changed = config.Migrate();

            Assert.True(changed);
            Assert.Equal(Configuration.CurrentConfigVersion, config.Version);
            Assert.Equal(24, config.QuestFontSize);
            Assert.Equal(1.25f, config.QuestPopupScale);
            Assert.Equal(customColor, config.QuestTextColor);
            Assert.False(config.EnableQuestTextRecolor);
        }

        [Theory]
        [InlineData(5, 12)]
        [InlineData(20, 20)]
        [InlineData(50, 28)]
        public void FontSizeValidationClampsCorrectly(int input, int expected)
        {
            var config = new Configuration
            {
                QuestFontSize = input
            };

            config.Validate();

            Assert.Equal(expected, config.QuestFontSize);
        }

        [Theory]
        [InlineData(0.25f, 0.75f)]
        [InlineData(1.25f, 1.25f)]
        [InlineData(3.0f, 1.50f)]
        public void PopupScaleValidationClampsCorrectly(float input, float expected)
        {
            var config = new Configuration
            {
                QuestPopupScale = input
            };

            config.Validate();

            Assert.Equal(expected, config.QuestPopupScale);
        }

        [Fact]
        public void InvalidFloatingPointValuesReturnToDefaults()
        {
            var config = new Configuration
            {
                QuestPopupScale = float.NaN,
                QuestPopupOffsetX = float.PositiveInfinity,
                QuestPopupOffsetY = float.NegativeInfinity,
                QuestTextColor = new Vector4(float.NaN, 0.5f, 2f, 1f)
            };

            bool changed = config.Validate();

            Assert.True(changed);

            Assert.Equal(Configuration.DefaultQuestPopupScale, config.QuestPopupScale);
            Assert.Equal(Configuration.DefaultQuestPopupOffsetX, config.QuestPopupOffsetX);
            Assert.Equal(Configuration.DefaultQuestPopupOffsetY, config.QuestPopupOffsetY);

            Assert.Equal(Configuration.DefaultQuestTextColor.X, config.QuestTextColor.X);
            Assert.Equal(0.5f, config.QuestTextColor.Y);
            Assert.Equal(1f, config.QuestTextColor.Z);
            Assert.Equal(1f, config.QuestTextColor.W);
        }

        [Fact]
        public void ResetAppearancePreservesLayout()
        {
            var config = new Configuration
            {
                QuestFontSize = 25,
                QuestTextColor = new Vector4(1f, 0f, 0f, 1f),
                QuestPopupScale = 1.3f,
                QuestPopupOffsetX = 150f,
                QuestPopupOffsetY = -75f
            };

            config.ResetAppearance();

            Assert.Equal(Configuration.DefaultQuestFontSize, config.QuestFontSize);
            Assert.Equal(Configuration.DefaultQuestTextColor, config.QuestTextColor);

            Assert.Equal(1.3f, config.QuestPopupScale);
            Assert.Equal(150f, config.QuestPopupOffsetX);
            Assert.Equal(-75f, config.QuestPopupOffsetY);
        }

        [Fact]
        public void ResetLayoutPreservesAppearance()
        {
            var customColor = new Vector4(0.1f, 0.2f, 0.3f, 1f);

            var config = new Configuration
            {
                QuestFontSize = 22,
                QuestTextColor = customColor,
                QuestPopupScale = 1.4f,
                QuestPopupOffsetX = 200f,
                QuestPopupOffsetY = -100f
            };

            config.ResetLayout();

            Assert.Equal(Configuration.DefaultQuestPopupScale, config.QuestPopupScale);
            Assert.Equal(Configuration.DefaultQuestPopupOffsetX, config.QuestPopupOffsetX);
            Assert.Equal(Configuration.DefaultQuestPopupOffsetY, config.QuestPopupOffsetY);

            Assert.Equal(22, config.QuestFontSize);
            Assert.Equal(customColor, config.QuestTextColor);
        }


        [Fact]
        public void JsonSerializationPreservesCustomSettings()
        {
            var original = new Configuration
            {
                Version = Configuration.CurrentConfigVersion,
                EnableQuestTextRecolor = false,
                EnableQuestPopupTextures = true,
                QuestFontSize = 24,
                QuestPopupScale = 1.25f,
                QuestPopupOffsetX = 125f,
                QuestPopupOffsetY = -75f,
                QuestTextColor = new Vector4(0.2f, 0.4f, 0.8f, 1f),
                QuestEdgeColor = new Vector4(0.1f, 0.3f, 0.5f, 1f)
            };

            var json = System.Text.Json.JsonSerializer.Serialize(
                original,
                new System.Text.Json.JsonSerializerOptions
                {
                    IncludeFields = true
                });

            var restored = System.Text.Json.JsonSerializer.Deserialize<Configuration>(
                json,
                new System.Text.Json.JsonSerializerOptions
                {
                    IncludeFields = true
                });

            Assert.NotNull(restored);

            Assert.Equal(original.Version, restored.Version);
            Assert.Equal(original.EnableQuestTextRecolor, restored.EnableQuestTextRecolor);
            Assert.Equal(original.EnableQuestPopupTextures, restored.EnableQuestPopupTextures);
            Assert.Equal(original.QuestFontSize, restored.QuestFontSize);
            Assert.Equal(original.QuestPopupScale, restored.QuestPopupScale);
            Assert.Equal(original.QuestPopupOffsetX, restored.QuestPopupOffsetX);
            Assert.Equal(original.QuestPopupOffsetY, restored.QuestPopupOffsetY);
            Assert.Equal(original.QuestTextColor, restored.QuestTextColor);
            Assert.Equal(original.QuestEdgeColor, restored.QuestEdgeColor);
        }


        [Fact]
        public void RepeatedValidationDoesNotReportUnnecessaryChanges()
        {
            var config = new Configuration
            {
                QuestFontSize = 50,
                QuestPopupScale = 3.0f,
                QuestPopupOffsetX = 900f,
                QuestPopupOffsetY = -900f,
                QuestTextColor = new Vector4(2f, -1f, 0.5f, 1f)
            };

            // First validation should correct invalid settings.
            bool firstValidationChanged = config.Validate();

            Assert.True(firstValidationChanged);

            Assert.Equal(Configuration.MaxQuestFontSize, config.QuestFontSize);
            Assert.Equal(Configuration.MaxQuestPopupScale, config.QuestPopupScale);
            Assert.Equal(Configuration.MaxQuestPopupOffsetX, config.QuestPopupOffsetX);
            Assert.Equal(Configuration.MinQuestPopupOffsetY, config.QuestPopupOffsetY);

            // Second validation should find nothing else to correct.
            bool secondValidationChanged = config.Validate();

            Assert.False(secondValidationChanged);
        }
    }
}
