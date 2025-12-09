# Steam Achievements API Implementation Guide

## Overview

This launcher now includes Steam Achievements API integration via Steamworks.NET. This allows Pixel Game Maker MV (PGMMV) games to unlock and track Steam achievements.

## Prerequisites

1. **Steam Client**: The Steam client must be running on the user's computer
2. **Steam App ID**: Your game must be registered on Steam with a valid App ID
3. **Achievement Configuration**: Achievements must be configured in Steamworks Partner portal

## Setup Instructions

### 1. Configure Your Steam App ID

Create a file named `steam_appid.txt` in your game's root directory (the same directory as `Player.exe`). This file should contain only your Steam App ID:

```
480
```

Replace `480` with your actual Steam App ID.

**Note**: For development/testing, you can use App ID `480` (Spacewar) which is Valve's test application.

### 2. Configure Achievements in Steamworks

1. Log into the [Steamworks Partner portal](https://partner.steamgames.com/)
2. Navigate to your application
3. Go to **Community** → **Stats & Achievements**
4. Define your achievements with:
   - **API Name**: The identifier you'll use in code (e.g., `ACHIEVEMENT_WIN_FIRST_GAME`)
   - **Display Name**: The name shown to players
   - **Description**: What the player must do to unlock it
   - **Icon**: Images for locked and unlocked states

### 3. Publish Achievement Configuration

After configuring achievements, publish them to Steam:
1. Click **Publish** in the Steamworks Partner portal
2. Wait for the configuration to propagate (usually a few minutes)

## How It Works

### Launcher Integration

The launcher automatically:
1. Looks for `steam_appid.txt` in the launcher or game directory
2. Initializes the Steam API if found
3. Requests current user stats and achievements
4. Shuts down Steam API after launching the game

### Using Steam Achievements in PGMMV

The `SteamManager` class provides the following methods:

#### Initialize Steam
```csharp
// Called automatically by the launcher
bool success = SteamManager.Initialize(yourAppId);
```

#### Unlock an Achievement
```csharp
bool unlocked = SteamManager.UnlockAchievement("ACHIEVEMENT_WIN_FIRST_GAME");
```

#### Check if Achievement is Unlocked
```csharp
bool isUnlocked = SteamManager.IsAchievementUnlocked("ACHIEVEMENT_WIN_FIRST_GAME");
```

#### Clear an Achievement (for testing)
```csharp
bool cleared = SteamManager.ClearAchievement("ACHIEVEMENT_WIN_FIRST_GAME");
```

## Integration Methods for PGMMV Games

Since PGMMV games are JavaScript-based, you'll need to create a bridge between JavaScript and the C# Steam API. Here are recommended approaches:

### Option 1: External Achievement Tracker

Create a separate executable that the PGMMV game can call to unlock achievements:

1. Create a console application that accepts achievement names as command-line arguments
2. Call it from PGMMV using system commands
3. Example: `AchievementUnlocker.exe ACHIEVEMENT_WIN_FIRST_GAME`

### Option 2: Named Pipes/IPC

Implement inter-process communication:

1. The launcher creates a named pipe server
2. The PGMMV game (via JavaScript plugin) sends achievement unlock requests
3. The launcher processes these requests

### Option 3: File-Based Communication

Simple file-based approach:

1. PGMMV game writes achievement names to a file (e.g., `achievements.txt`)
2. A background service monitors this file and processes achievements
3. Delete or clear the file after processing

### Option 4: HTTP API Server

Run a local HTTP server in the launcher:

1. Launcher starts a simple HTTP server on localhost
2. PGMMV game makes HTTP requests to unlock achievements
3. Example: `POST http://localhost:8080/achievement/unlock?name=ACHIEVEMENT_WIN_FIRST_GAME`

## Example Achievement Names

Common achievement patterns:

```
ACHIEVEMENT_START_GAME        - Start the game for the first time
ACHIEVEMENT_COMPLETE_LEVEL_1  - Complete the first level
ACHIEVEMENT_COLLECT_100_COINS - Collect 100 coins
ACHIEVEMENT_DEFEAT_BOSS       - Defeat the final boss
ACHIEVEMENT_PERFECT_RUN       - Complete a level without taking damage
```

## Testing

### Testing with Spacewar (App ID 480)

For testing without a published game:

1. Create `steam_appid.txt` with content: `480`
2. Run the launcher with Steam client open
3. Any achievements you unlock will appear in Spacewar

### Resetting Achievements

To clear achievements for testing:

```csharp
SteamManager.ClearAchievement("ACHIEVEMENT_NAME");
```

Or use the Steam Client:
1. Right-click your game in Steam library
2. Properties → Local Files → Browse
3. Delete the `userdata` folder (backs up first!)

## Troubleshooting

### Steam API Not Initializing

**Symptoms**: Console shows "Steam API initialization failed"

**Solutions**:
- Ensure Steam client is running
- Verify `steam_appid.txt` exists and contains valid App ID
- Check that your Steam account owns the game (for non-480 App IDs)
- Run the launcher from the correct directory

### Achievements Not Unlocking

**Symptoms**: `UnlockAchievement` returns false

**Solutions**:
- Verify achievement API names match Steamworks configuration
- Ensure achievements are published in Steamworks Partner portal
- Call `RequestCurrentStats()` after Steam initialization
- Check Steam overlay is enabled for the game

### DLL Not Found Errors

**Symptoms**: Missing Steamworks DLL errors

**Solutions**:
- Ensure `steam_api64.dll` (or `steam_api.dll` for 32-bit) is in the same directory as the launcher
- Steamworks.NET should include these automatically, but verify they're copied to output directory
- Check that the correct platform DLL is being used (x86 vs x64)

## Best Practices

1. **Graceful Degradation**: Always check if Steam is initialized before calling Steam functions
2. **Error Handling**: Wrap Steam API calls in try-catch blocks
3. **Logging**: Log Steam operations for debugging
4. **Testing**: Test with Steam offline to ensure your game handles missing Steam gracefully
5. **Achievement Design**: Make achievements meaningful and achievable
6. **Localization**: Provide translations for achievement names and descriptions in Steamworks

## Security Considerations

1. **Never Trust Client**: Validate achievement unlocks server-side for competitive games
2. **Rate Limiting**: Prevent achievement spam by implementing cooldowns
3. **App ID Security**: Don't hardcode App IDs in public repositories

## Additional Resources

- [Steamworks.NET Documentation](https://steamworks.github.io/)
- [Steamworks API Reference](https://partner.steamgames.com/doc/api)
- [Steam Achievements Best Practices](https://partner.steamgames.com/doc/features/achievements)
- [PGMMV Plugin Development](https://pixelgamemakermv.com/documentation/)

## Example Code

See `SteamManager.cs` for the complete implementation of Steam API integration.

## Support

For issues with:
- **Steam API**: Check Steamworks Partner forums
- **PGMMV Integration**: Consult PGMMV documentation
- **This Launcher**: Check the GitHub repository issues

## License

This implementation uses Steamworks.NET, which is MIT licensed. Ensure compliance with Steam's terms of service when distributing your game.
