# Quest Text Recolor — Configuration Safety Tests

**Automated Test Status:** 16 Passed, 0 Failed  
**Test Project:** QuestTextRecolor.Tests  
**Branch:** development/config-safety

## Existing Settings — In-Game Persistence
- [ ] Custom text and edge colors survive reload
- [ ] Custom font size survives reload
- [ ] Custom popup offsets and scale survive reload
- [ ] Penumbra texture preference survives reload
- [ ] Main enable/disable preference survives reload

## Validation — Automated Tests Complete
- [x] Font size below minimum is clamped to 12
- [x] Font size above maximum is clamped to 28
- [x] Popup scale outside 0.75–1.50 is clamped
- [x] Invalid floating-point values return to defaults
- [x] Invalid color channels are corrected without affecting valid channels
- [x] Repeated validation reports no additional changes after correction

## Reset Behavior
- [x] Reset Appearance preserves popup layout
- [x] Reset Layout preserves text color and font size
- [x] Reset Appearance triggers exactly one save call
- [x] Reset Layout triggers exactly one save call
- [ ] Verify both reset operations preserve all unrelated enable settings

## Migration
- [x] Version 0 configuration migrates to version 1
- [x] Version 0 migration preserves tested customized settings
- [x] Current configuration values survive a JSON serialization round-trip
- [ ] Existing version 1 configuration migration is verified as a no-op
- [ ] A future version upgrade preserves unrelated settings

## Automated Unit Testing
- [x] Test project builds independently of FFXIV
- [x] Default configuration values verified
- [x] Configuration validation verified
- [x] Invalid floating-point values handled
- [x] Appearance and layout reset behavior verified
- [x] JSON serialization and deserialization verified using System.Text.Json
- [x] Migration after JSON loading verified
- [x] Save calls verified using test-only dependencies
- [x] Repeated validation stability verified
- [x] All 16 automated test cases passed

## In-Game Verification — Deferred
- [ ] Settings survive /xlreload
- [ ] Settings survive a plugin update
- [ ] Settings remain correct after restarting FFXIV
- [ ] Confirm Dalamud's actual configuration serializer preserves all customized settings
- [ ] Confirm Penumbra texture preferences remain intact

## Notes
- No production C# logic was changed during the initial offline test phase.
- The main project file was updated to exclude test source files from production compilation.
- Automated tests use the original Configuration.cs linked into the test project.
- Save behavior is verified using a test-only interface, not actual disk writes.
- JSON round-trip tests use System.Text.Json with IncludeFields enabled.
- Offline tests do not replace Dalamud lifecycle or in-game verification.
