# Quest Text Recolor — Development Roadmap

This document tracks major development milestones, resolved bugs, active testing, and remaining work for Quest Text Recolor.

> **Current focus:** v1.3.0 final commit and release publication. In-game regression testing and Release package verification have passed.

## v1.3.0 Progress

### Completed / Working

- Quest objective text color customization
- Quest objective outline / edge color customization
- Adjustable quest text font size
- Quest popup position controls
- Quest popup scale controls
- Built-in appearance presets
- In-game test popup / preview
- Reset-to-default appearance controls
- Optional Penumbra temporary texture integration
- Custom `ui/uld/ScreenInfo_hr1.tex` replacement support
- Custom `ui/icon/060000/060081_hr1.tex` replacement support
- Expanded configuration interface for text, layout, presets, textures, and plugin information

## Resolved Issues & Bugs

### Quest Popup Black Seam / Dark Line

**Status:** Fixed and verified in-game across the completed v1.3.0 regression checklist.

A thin dark horizontal seam could appear beneath the custom quest objective popup background.

Investigation isolated the background to `_ScreenText` NineGrid **Node 10**, using **PartId 3**.

Diagnostic values:

- Logical texture size: `328 x 228`
- `_hr1` texture size: `656 x 456`
- Part coordinates: `U=108`, `V=128`
- Part size: `108 x 36`
- NineGrid offsets: Top `16`, Bottom `16`, Left `40`, Right `40`
- Approximate `_hr1` physical region: `X 216–431`, `Y 256–327`

The visible seam was traced to dark RGB values in transparent / semi-transparent pixels along the lower edge of the sampled region. Cleaning the affected edge pixels with **transparent white** instead of transparent black removed the artifact while preserving the soft popup edge.

Confirmed working cleanup included the lower-edge area around:

- Row `327`
- Row `328`
- Approximate X range `222–429`

This matters because filtered UI textures may sample neighboring transparent texels, and hidden black RGB values can bleed into otherwise transparent edges.

### Incorrect NineGrid Height Experiments

**Status:** Resolved / abandoned diagnostic path

Reducing Node 10 or Part 3 from height `36` to `35` caused the popup background to disappear or render incorrectly rather than trimming the artifact. The node and part should remain at their normal dimensions.

### Persistent ULD Diagnostic State After Plugin Reload

**Status:** Understood / documented

Directly mutating ULD part data during diagnostics can persist beyond a normal Dalamud plugin reload. A full FFXIV restart is required when a clean UI resource state is needed after low-level ULD experiments.

### Texture Preview / Output Confusion

**Status:** Resolved during development

The in-plugin preview PNG and the in-game Penumbra replacement are separate paths. The preview image is only used by the configuration UI, while the in-game popup replacement uses the packaged `.tex` files.

Build output can also retain older texture files when using copy-if-newer behavior. Force rebuilds and, when necessary, clearing `bin` / `obj` help ensure the intended texture is packaged.

### Penumbra Texture Replacement Verification

**Status:** Working

The plugin successfully registers temporary Penumbra texture replacements for the quest popup assets in the Default collection. Texture changes may require a plugin reload, texture toggle, or full game restart depending on cached UI resources.

## Current Testing

The full v1.3.0 in-game regression checklist is complete and passed; see [TESTING_CHECKLIST_v1.3.0.md](TESTING_CHECKLIST_v1.3.0.md). Verified coverage includes:

- Short quest objective popups
- Long quest objective popups
- Completion / checkmark popup variants
- Popup scaling at multiple values
- Popup X/Y positioning
- Font-size changes
- Text and edge color changes
- Preset switching
- Penumbra texture enable / disable behavior
- Fresh FFXIV restart behavior
- Clean plugin build with no diagnostic logging

The forced Release rebuild and package verification passed with 0 warnings and 0 errors. The DLL and generated manifest report version `1.3.0.0`. Both `.tex` files and both preview PNGs were packaged at their expected paths, with no unwanted debug or source files in `latest.zip`.

## Remaining v1.3.0 Tasks

- Create the final v1.3.0 commit
- Push the development branch
- Merge/reconcile with `main` if needed
- Create the v1.3.0 tag and release with final release notes
- Attach the verified release ZIP
- Update `repo.json` to v1.3.0
- Verify the Dalamud custom repository update

## Release Readiness

v1.3.0 has passed **in-game regression testing and Release rebuild/package verification**. The popup black-seam fix is verified in-game, including a fresh FFXIV restart with textures enabled. The remaining work is the commit and release publication sequence listed above.

## Debugging Notes for Future FFXIV Updates

If the quest popup rendering changes after a future FFXIV patch, the following diagnostic path was effective:

1. Inspect `_ScreenText` component nodes.
2. Identify NineGrid nodes associated with the popup background.
3. Temporarily hide individual nodes to isolate the rendered layer.
4. Read the active `AtkNineGridNode` PartId and offsets.
5. Read the associated `AtkUldPart` U/V/Width/Height values.
6. Account for `_hr1` texture scaling when mapping logical ULD coordinates to physical texture pixels.
7. Inspect transparent edge pixels for hidden dark RGB values before changing node dimensions.
8. Fully restart FFXIV after any direct ULD-memory mutation tests.

This roadmap will be updated as v1.3.0 publication proceeds and future issues are resolved.
