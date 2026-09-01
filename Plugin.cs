using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;
using QuestTextRecolor.Windows;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using System.Threading.Tasks;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace QuestTextRecolor;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService]
    internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

    [PluginService]
    internal static ICommandManager CommandManager { get; private set; } = null!;

    [PluginService]
    internal static IGameGui GameGui { get; private set; } = null!;

    [PluginService]
    internal static IAddonLifecycle AddonLifecycle { get; private set; } = null!;

    private const string CommandName = "/questtext";

    public Configuration Configuration { get; init; }
    
    private TestPopupWindow TestPopupWindow { get; init; }


    public readonly WindowSystem WindowSystem = new("Quest Text Recolor");

    private ConfigWindow ConfigWindow { get; init; }



    public Plugin()
    {
        Configuration =
            PluginInterface.GetPluginConfig() as Configuration
            ?? new Configuration();

        ConfigWindow = new ConfigWindow(this);
            WindowSystem.AddWindow(ConfigWindow);

        TestPopupWindow = new TestPopupWindow(Configuration);
            WindowSystem.AddWindow(TestPopupWindow);

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
    }

    public void Dispose()
    {
        AddonLifecycle.UnregisterListener(OnScreenTextPostUpdate);

        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;
        PluginInterface.UiBuilder.OpenMainUi += ToggleConfigUi;

        WindowSystem.RemoveAllWindows();

        TestPopupWindow.Dispose();

        ConfigWindow.Dispose();

        TestPopupWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
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
        if (!Configuration.EnableQuestTextRecolor)
        {
            return;
        }

        var addon = (AtkUnitBase*)args.Addon.Address;

        if (addon == null)
        {
            return;
        }

        int foundCount = 0;

        var textColor = Configuration.QuestTextColor;
        var edgeColor = Configuration.QuestEdgeColor;

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

            var node = componentNode->Component->UldManager.SearchNodeById(3);

            if (node == null)
                continue;

            if (node->Type != NodeType.Text)
                continue;

            var textNode = (AtkTextNode*)node;

            foundCount++;

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
}
