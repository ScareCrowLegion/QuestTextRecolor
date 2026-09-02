using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Plugin;
using Penumbra.Api.Enums;
using Penumbra.Api.IpcSubscribers;

namespace QuestTextRecolor.Services;

internal sealed class PenumbraService
{
    private const string TemporaryModName = "QuestTextRecolor_PopupTexture";

    private readonly IDalamudPluginInterface pluginInterface;

    private bool questPopupTexturesApplied;

    private readonly ApiVersion apiVersion;
    private readonly GetCollections getCollections;
    private readonly AddTemporaryMod addTemporaryMod;
    private readonly RemoveTemporaryMod removeTemporaryMod;

    public PenumbraService(IDalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;

        apiVersion = new ApiVersion(pluginInterface);
        getCollections = new GetCollections(pluginInterface);
        addTemporaryMod = new AddTemporaryMod(pluginInterface);
        removeTemporaryMod = new RemoveTemporaryMod(pluginInterface);
    }

    public bool IsAvailable()
    {
        try
        {
            var version = apiVersion.Invoke();
            return version.Breaking > 0;
        }
        catch
        {
            return false;
        }
    }

    public Guid? GetDefaultCollectionId()
    {
        try
        {
            var collections = getCollections.Invoke();

            foreach (var collection in collections)
            {
                if (string.Equals(
                    collection.Value,
                    "Default",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return collection.Key;
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public bool ApplyQuestPopupTextures()
    {
        if (questPopupTexturesApplied)
        {
            return true;
        }
        try
        {
            var collectionId = GetDefaultCollectionId();

            if (!collectionId.HasValue)
                return false;

            var screenInfoPath = Path.Combine(
    pluginInterface.AssemblyLocation.DirectoryName!,
    "Resources",
    "Textures",
    "ScreenInfo_hr1.tex"
);

            var iconPath = Path.Combine(
                pluginInterface.AssemblyLocation.DirectoryName!,
                "Resources",
                "Textures",
                "060081_hr1.tex"
            );

            if (!File.Exists(screenInfoPath) ||
                !File.Exists(iconPath))
            {
                return false;
            }

            var files = new Dictionary<string, string>
{
    {
        "ui/uld/ScreenInfo_hr1.tex",
        screenInfoPath
    },
    {
        "ui/icon/060000/060081_hr1.tex",
        iconPath
    }
};
            var result = addTemporaryMod.Invoke(
                TemporaryModName,
                collectionId.Value,
                files,
                string.Empty,
                0
            );

            if (result == PenumbraApiEc.Success)
            {
                questPopupTexturesApplied = true;
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveQuestPopupTextures()
    {
        try
        {
            var collectionId = GetDefaultCollectionId();

            if (!collectionId.HasValue)
                return false;

            var result = removeTemporaryMod.Invoke(
                TemporaryModName,
                collectionId.Value,
                0
            );

            if (result == PenumbraApiEc.Success)
            {
                questPopupTexturesApplied = false;
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}