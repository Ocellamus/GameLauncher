# Quick Start: Using Steam Achievements in Your PGMMV Game

## Step 1: Create steam_appid.txt

In your game's root directory (where `Player.exe` is located), create a file named `steam_appid.txt` with your Steam App ID:

```
480
```

For testing, use `480` (Spacewar - Valve's test application).
For production, use your actual Steam App ID from Steamworks Partner.

## Step 2: Deploy the Launcher

Copy the compiled launcher executable to a subdirectory:
```
YourGame/
  ├── Player.exe
  ├── steam_appid.txt
  └── Launcher/
      └── Launcher.exe  (your compiled launcher)
```

## Step 3: Test Steam Integration

1. Ensure Steam client is running
2. Run the launcher - it will automatically initialize Steam API
3. The launcher will launch your PGMMV game with Steam enabled

## Step 4: Unlock Achievements from PGMMV

Since PGMMV games run JavaScript, you have several options:

### Option A: Call External Command (Simplest)

Create a small console app using `AchievementHelper.cs`:

```csharp
// In a new console app project
class Program {
    static int Main(string[] args) {
        return GameLauncher.AchievementHelper.ProcessAchievementCommand(args);
    }
}
```

Then from PGMMV JavaScript, execute:
```javascript
// Using PGMMV's system command plugin
executeCommand("AchievementUnlocker.exe unlock ACHIEVEMENT_WIN_FIRST_GAME");
```

### Option B: File-Based Communication

Have PGMMV write achievement names to a file:
```javascript
// In PGMMV game
writeTextFile("achievements_pending.txt", "ACHIEVEMENT_WIN_FIRST_GAME\n");
```

Then create a watcher service that processes these files and calls `SteamManager.UnlockAchievement()`.

### Option C: Named Pipes (Advanced)

Implement IPC between the launcher/background service and PGMMV game.

## Step 5: Configure Achievements in Steamworks

1. Log into [Steamworks Partner](https://partner.steamgames.com/)
2. Go to your app → Community → Stats & Achievements
3. Add achievements with API names like:
   - `ACHIEVEMENT_START_GAME`
   - `ACHIEVEMENT_LEVEL_1_COMPLETE`
   - `ACHIEVEMENT_COLLECT_100_COINS`
4. Publish the configuration

## Example Achievement Names

Common patterns for PGMMV games:
```
ACHIEVEMENT_FIRST_LAUNCH      - Player started the game
ACHIEVEMENT_TUTORIAL_COMPLETE - Finished tutorial
ACHIEVEMENT_LEVEL_1          - Completed first level
ACHIEVEMENT_DEFEAT_BOSS_1    - Defeated first boss
ACHIEVEMENT_COLLECT_10_COINS - Collected 10 coins
ACHIEVEMENT_SPEEDRUN         - Completed game in under 1 hour
```

## Testing

To test without publishing to Steam:

1. Use App ID `480` in `steam_appid.txt`
2. Achievements will show in Spacewar
3. Use `SteamManager.ClearAchievement()` to reset for testing

## Troubleshooting

**Steam not initializing?**
- Verify Steam client is running
- Check `steam_appid.txt` exists and has valid App ID
- Ensure your account owns the game (for non-480 App IDs)

**Achievements not unlocking?**
- Verify achievement API names match Steamworks exactly
- Ensure achievements are published in Steamworks
- Check Steam overlay is enabled

## Complete Documentation

See [STEAM_ACHIEVEMENTS_GUIDE.md](STEAM_ACHIEVEMENTS_GUIDE.md) for comprehensive documentation including:
- Detailed API reference
- Multiple integration approaches
- Best practices
- Troubleshooting guide
- Security considerations

## Code Examples

```csharp
// Initialize Steam (done automatically by launcher)
SteamManager.Initialize(480);
SteamManager.RequestCurrentStats();

// Unlock an achievement
bool success = SteamManager.UnlockAchievement("ACHIEVEMENT_WIN_FIRST_GAME");

// Check if unlocked
bool unlocked = SteamManager.IsAchievementUnlocked("ACHIEVEMENT_WIN_FIRST_GAME");

// Clear for testing
SteamManager.ClearAchievement("ACHIEVEMENT_WIN_FIRST_GAME");

// Shutdown (done automatically by launcher)
SteamManager.Shutdown();
```

## Need Help?

- Check the comprehensive guide: `STEAM_ACHIEVEMENTS_GUIDE.md`
- Review example code in: `SteamManager.cs` and `AchievementHelper.cs`
- Steam API docs: https://partner.steamgames.com/doc/api
- Steamworks.NET docs: https://steamworks.github.io/
