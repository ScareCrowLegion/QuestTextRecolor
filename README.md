# Quest Text Recolor

Quest Text Recolor is a Dalamud plugin for Final Fantasy XIV that lets you customize the center-screen quest objective popup.

## Features

Quest Text Recolor provides customization for the quest progression popup shown in the center of the screen.

### Text Customization

- Custom quest objective text color
- Custom outline / edge color
- Adjustable font size
- Built-in appearance presets
- In-game preview popup
- Reset appearance to plugin defaults

These features work without Penumbra.

### Popup Layout

Quest Text Recolor also allows you to adjust the position and size of the quest popup.

Available layout controls:

- Horizontal Offset
  - Range: `-500 px` to `+500 px`
- Vertical Offset
  - Range: `-300 px` to `+300 px`
- Popup Scale
  - Range: `75%` to `150%`
- Reset Layout
  - Restores horizontal offset to `0`
  - Restores vertical offset to `0`
  - Restores popup scale to `100%`

Offsets are relative to the default FFXIV popup position.

A scale of `100%` uses the default FFXIV popup size.

### Optional Quest Popup Textures

Quest Text Recolor can optionally replace selected quest popup textures through Penumbra.

Currently supported texture replacements:

- `ui/uld/ScreenInfo_hr1.tex`
- `ui/icon/060000/060081_hr1.tex`

The plugin automatically uses your Default Penumbra collection when applying these replacements.

Penumbra is only required for the optional texture replacement feature.

Text recoloring, font size, presets, popup position, and popup scale continue to work without Penumbra.

A full FFXIV restart is recommended after enabling or disabling custom quest popup textures.

## Requirements

- Dalamud API 15
- Penumbra only if using the optional Quest Popup Textures feature

## Configuration

Open the plugin configuration with:

`/questtext`

The configuration window uses a sidebar to separate the available settings into pages:

- General
- Colors
- Text Style
- Popup Layout
- Presets
- Textures
- About

### General

Contains the main plugin enable toggle, in-game preview, and appearance reset controls.

### Colors

Adjust:

- Text Color
- Edge Color

### Text Style

Adjust the quest objective font size.

### Popup Layout

Adjust:

- Horizontal Offset
- Vertical Offset
- Popup Scale

Use **Reset Layout** to restore the default popup position and size.

### Presets

Quest Text Recolor includes several built-in appearance presets:

- Plugin Default
- FFXIV Original
- FFXIV Gold
- High Contrast
- Cool Blue

Presets change the text and edge colors only.

### Textures

Controls the optional Penumbra-based quest popup texture replacements.

If Penumbra is not available, the texture controls are disabled while the rest of the plugin continues to function normally.

### About

Displays basic plugin information and the `/questtext` command.

## Plugin Default Appearance

Text Color:

`#F2E4C4FF`

Edge Color:

`#5A4526FF`

Font Size:

`18`

Popup Layout:

- Horizontal Offset: `0 px`
- Vertical Offset: `0 px`
- Popup Scale: `100%`

## Quest Popup Textures

Supported textures:

- `ui/uld/ScreenInfo_hr1.tex`
- `ui/icon/060000/060081_hr1.tex`

### Texture Requirements

- Penumbra must be installed and available.
- Enable **Custom Quest Popup Textures** in the plugin configuration.
- A full FFXIV restart is recommended after enabling or disabling texture replacement.

The standard quest text and popup layout customization features do not require Penumbra.

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

Webhook test
