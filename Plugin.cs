using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using QuestTextRecolor.Windows;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using QuestTextRecolor.Services;
using System;
using System.Collections.Generic;


namespace QuestTextRecolor;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService]
    internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

    [PluginService]
    internal static ICommandManager CommandManager { get; private set; } = null!;

    [PluginService]
    internal static IAddonLifecycle AddonLifecycle { get; private set; } = null!;

    [PluginService]
    internal static ITextureProvider TextureProvider { get; private set; } = null!;

    [PluginService]
    internal static IGameGui GameGui { get; private set; } = null!;

    private const string CommandName = "/questtext";

    private float lastAppliedOffsetX = 0f;
    private float lastAppliedOffsetY = 0f;

    private nint lastScreenTextAddress = 0;

    private readonly Dictionary<nint, ScreenTextAppearanceState>
        originalScreenTextAppearance = new();

    private sealed class ScreenTextAppearanceState
    {
        public float ScaleX;
        public float ScaleY;

        public bool ScaleModified;

        public byte FontSize;

        public byte TextColorR;
        public byte TextColorG;
        public byte TextColorB;
        public byte TextColorA;

        public byte EdgeColorR;
        public byte EdgeColorG;
        public byte EdgeColorB;
        public byte EdgeColorA;

    }

    public Configuration Configuration { get; init; }

    private TestPopupWindow TestPopupWindow { get; init; }

    internal PenumbraService Penumbra { get; }

    public readonly WindowSystem WindowSystem = new("Quest Text Recolor");

    private ConfigWindow ConfigWindow { get; init; }

 
    public Plugin()
    {

        Configuration =
            PluginInterface.GetPluginConfig() as Configuration
            ?? new Configuration();

        var configChanged = Configuration.Migrate();
        var validationChanged = Configuration.Validate();

        if (configChanged || validationChanged)
        {
            Configuration.Save();
        }

        ConfigWindow = new ConfigWindow(this);
        WindowSystem.AddWindow(ConfigWindow);

        TestPopupWindow = new TestPopupWindow(Configuration);
        WindowSystem.AddWindow(TestPopupWindow);

        Penumbra = new PenumbraService(PluginInterface, Configuration);

        Penumbra.RefreshQuestPopupTextures();

        CommandManager.AddHandler(
            CommandName,
            new CommandInfo(OnCommand)
            {
                HelpMessage = "Open the Quest Text Recolor settings."
            }
        );


        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;
        PluginInterface.UiBuilder.OpenMainUi += ToggleConfigUi;


        AddonLifecycle.RegisterListener(
            AddonEvent.PostUpdate,
            "_ScreenText",
            OnScreenTextPostUpdate
            );

        AddonLifecycle.RegisterListener(
            AddonEvent.PreFinalize,
            "_ScreenText",
            OnScreenTextPreFinalize
            );
    }

    public unsafe void Dispose()
    {
        var screenTextAddon =
            GameGui.GetAddonByName<AtkUnitBase>("_ScreenText");

        RestoreScreenTextAppearance(screenTextAddon);
        RestoreScreenTextPosition(screenTextAddon);

        if (Penumbra.IsAvailable())
        {
            Penumbra.RemoveQuestPopupTextures();
        }

        AddonLifecycle.UnregisterListener(OnScreenTextPostUpdate);

        AddonLifecycle.UnregisterListener(OnScreenTextPreFinalize);

        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfigUi;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleConfigUi;

        WindowSystem.RemoveAllWindows();

        CommandManager.RemoveHandler(CommandName);

        Penumbra.Dispose();
    }

    private void OnCommand(string command, string args)
    {
        ConfigWindow.Toggle();
    }

    public void ToggleConfigUi()
    {
        ConfigWindow.Toggle();
    }

    public void ShowTestPopup()
    {
        TestPopupWindow.Show();
    }

    private unsafe void OnScreenTextPostUpdate(AddonEvent type, AddonArgs args)
    {

        var addon = (AtkUnitBase*)args.Addon.Address;

        if (addon == null)
        {
            return;
        }

        var currentScreenTextAddress = (nint)addon;

        if (!Configuration.EnableQuestTextRecolor)
        {
            RestoreScreenTextAppearance(addon);
            RestoreScreenTextPosition(addon);
            return;
        }

        if (currentScreenTextAddress != lastScreenTextAddress)
        {
            ResetScreenTextState();
            lastScreenTextAddress = currentScreenTextAddress;
        }

        var offsetX = MathF.Round(Configuration.QuestPopupOffsetX);
        var offsetY = MathF.Round(Configuration.QuestPopupOffsetY);

        var deltaX = offsetX - lastAppliedOffsetX;
        var deltaY = offsetY - lastAppliedOffsetY;

        if (deltaX != 0f || deltaY != 0f)
        {
            var newX = (short)(addon->X + deltaX);
            var newY = (short)(addon->Y + deltaY);

            addon->SetPosition(newX, newY);

            lastAppliedOffsetX = offsetX;
            lastAppliedOffsetY = offsetY;
        }

        var textColor = Configuration.QuestTextColor;
        var edgeColor = Configuration.QuestEdgeColor;

        if (addon->UldManager.NodeList == null)
        {
            return;
        }

        for (int i = 0; i < addon->UldManager.NodeListCount; i++)
        {
            var topNode = addon->UldManager.NodeList[i];

            if (topNode == null)
                continue;

            if ((ushort)topNode->Type < 1000)
                continue;

            var componentNode = (AtkComponentNode*)topNode;

            if (componentNode->Component == null)
                continue;

            if (componentNode->Component->UldManager.NodeList == null)
                continue;

            var node = componentNode->Component->UldManager.SearchNodeById(3);

            if (node == null)
                continue;

            if (node->Type != NodeType.Text)
                continue;

            var textNode = (AtkTextNode*)node;

            var textNodeAddress = (nint)textNode;

            if (!originalScreenTextAppearance.TryGetValue(
                textNodeAddress,
                out var originalAppearance))
            {
                originalAppearance = new ScreenTextAppearanceState
                {
                    ScaleX = topNode->ScaleX,
                    ScaleY = topNode->ScaleY,

                    ScaleModified = false,

                    FontSize = textNode->FontSize,

                    TextColorR = textNode->TextColor.R,
                    TextColorG = textNode->TextColor.G,
                    TextColorB = textNode->TextColor.B,
                    TextColorA = textNode->TextColor.A,

                    EdgeColorR = textNode->EdgeColor.R,
                    EdgeColorG = textNode->EdgeColor.G,
                    EdgeColorB = textNode->EdgeColor.B,
                    EdgeColorA = textNode->EdgeColor.A
                };

                originalScreenTextAppearance[
                    textNodeAddress
                ] = originalAppearance;
            }

            if (topNode->X != 0f || topNode->Y != 0f)
            {
                originalAppearance.ScaleModified = true;

                var popupScale = Configuration.QuestPopupScale;

                topNode->SetScale(popupScale, popupScale);
            }

            textNode->FontSize = (byte)Configuration.QuestFontSize;

            textNode->TextColor.R = (byte)(textColor.X * 255f);
            textNode->TextColor.G = (byte)(textColor.Y * 255f);
            textNode->TextColor.B = (byte)(textColor.Z * 255f);
            textNode->TextColor.A = (byte)(textColor.W * 255f);

            textNode->EdgeColor.R = (byte)(edgeColor.X * 255f);
            textNode->EdgeColor.G = (byte)(edgeColor.Y * 255f);
            textNode->EdgeColor.B = (byte)(edgeColor.Z * 255f);
            textNode->EdgeColor.A = (byte)(edgeColor.W * 255f);

            ((AtkResNode*)textNode)->IsDirty = true;
        }
    }

     private void OnScreenTextPreFinalize(AddonEvent type, AddonArgs args)
    {
        ResetScreenTextState();
    }

    private unsafe void RestoreScreenTextPosition(AtkUnitBase* addon)
    {
        if (addon == null ||
            lastScreenTextAddress == 0 ||
            (nint)addon != lastScreenTextAddress)
        {
            ResetScreenTextState();
            return;
        }

        if (lastAppliedOffsetX != 0f ||
            lastAppliedOffsetY != 0f)
        {
            var restoredX = (short)(addon->X - lastAppliedOffsetX);
            var restoredY = (short)(addon->Y - lastAppliedOffsetY);

            addon->SetPosition(restoredX, restoredY);
        }

        ResetScreenTextState();
    }

    private unsafe void RestoreScreenTextAppearance(AtkUnitBase* addon)
    {
        if (addon == null || lastScreenTextAddress == 0 ||
                (nint)addon != lastScreenTextAddress)
        {
            ResetScreenTextState();
            return;
        }

        if (addon->UldManager.NodeList == null)
        {
            originalScreenTextAppearance.Clear();
            return;
        }

        for (int i = 0; i < addon->UldManager.NodeListCount; i++)
        {
            var topNode = addon->UldManager.NodeList[i];

            if (topNode == null)
                continue;

            if ((ushort)topNode->Type < 1000)
                continue;

            var componentNode = (AtkComponentNode*)topNode;

            if (componentNode->Component == null)
                continue;

            if (componentNode->Component->UldManager.NodeList == null)
                continue;

            var node =
                componentNode->Component->UldManager.SearchNodeById(3);

            if (node == null)
                continue;

            if (node->Type != NodeType.Text)
                continue;

            var textNode = (AtkTextNode*)node;

            if (!originalScreenTextAppearance.TryGetValue(
                (nint)textNode,
                out var originalAppearance))
            {
                continue;
            }

            if (originalAppearance.ScaleModified)
            {
                topNode->SetScale(
                    originalAppearance.ScaleX,
                    originalAppearance.ScaleY
                );
            }

            textNode->FontSize =
                originalAppearance.FontSize;

            textNode->TextColor.R =
                originalAppearance.TextColorR;

            textNode->TextColor.G =
                originalAppearance.TextColorG;

            textNode->TextColor.B =
                originalAppearance.TextColorB;

            textNode->TextColor.A =
                originalAppearance.TextColorA;

            textNode->EdgeColor.R =
                originalAppearance.EdgeColorR;

            textNode->EdgeColor.G =
                originalAppearance.EdgeColorG;

            textNode->EdgeColor.B =
                originalAppearance.EdgeColorB;

            textNode->EdgeColor.A =
                originalAppearance.EdgeColorA;

            ((AtkResNode*)textNode)->IsDirty = true;
        }

        originalScreenTextAppearance.Clear();
    }
    private void ResetScreenTextState()
    {
        lastScreenTextAddress = 0;
        lastAppliedOffsetX = 0f;
        lastAppliedOffsetY = 0f;

        originalScreenTextAppearance.Clear();

    }
}
