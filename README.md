# Quest Text Recolor

Quest Text Recolor is a Dalamud plugin for Final Fantasy XIV that allows you to customize center-screen quest objective text.

## Quest Popup Textures

Quest Text Recolor can optionally replace the quest popup UI textures through Penumbra.

Supported textures:

- `ui/uld/ScreenInfo_hr1.tex`
- `ui/icon/060000/060081_hr1.tex`

### Requirements

- Penumbra must be installed and available.
- Enable **Custom Quest Popup Textures** in the plugin configuration.
- Restart FFXIV after enabling or disabling the texture replacement.

The standard quest text recoloring features do not require Penumbra.

## Features

Quest Text Recolor lets you customize the center-screen quest progression popup in FFXIV.

### Text Customization

* Custom quest objective text color
* Custom outline / edge color
* Adjustable font size
* Built-in appearance presets
* In-game preview popup
* Reset appearance to plugin defaults

These features work without Penumbra.

### Optional Quest Popup Textures

Quest Text Recolor can also replace selected quest popup textures through Penumbra.

Currently supported texture replacements:

* `ui/uld/ScreenInfo_hr1.tex`
* `ui/icon/060000/060081_hr1.tex`

The plugin automatically uses your Default Penumbra collection when applying these replacements.

Penumbra is only required for this optional texture replacement feature. Normal text recoloring and font customization continue to work without Penumbra.

A full FFXIV restart is recommended after enabling or disabling quest popup texture replacement.

## Requirements

* Dalamud API 15
* Penumbra only if using the optional Quest Popup Textures feature

## Configuration

Open the plugin configuration with:

`/questtext`

From the configuration window you can adjust text colors, edge colors, font size, presets, popup textures, and use the built-in preview.

## Presets

Quest Text Recolor includes several built-in presets:

- Plugin Default
- FFXIV Original
- FFXIV Gold
- High Contrast
- Cool Blue

## Plugin Default Appearance

Text:

`#F2E4C4FF`

Edge:

`#5A4526FF`

Font Size:

`18`

## Command

`/questtext`

Opens the Quest Text Recolor configuration window.

## Installation

Quest Text Recolor is currently available through a custom Dalamud plugin repository.

Add the following URL to your Dalamud custom plugin repositories:

`https://raw.githubusercontent.com/ScareCrowLegion/QuestTextRecolor/main/repo.json`

Then search for:

`Quest Text Recolor`

in the Dalamud Plugin Installer.

## Repository

https://github.com/ScareCrowLegion/QuestTextRecolor

## Author

ScareCrowLegion