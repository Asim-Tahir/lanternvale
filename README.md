# Lanternvale

Lanternvale is a 2D pixel-art game project built with Unity. The repository currently includes a main menu, settings interface, game scene, player movement and attack animations, persistent volume settings, and saved level selection.

## Features

- Main menu with Continue, New Game, Settings, and Exit actions
- Continue support through Unity `PlayerPrefs`
- Persistent music and SFX volume settings
- Keyboard and gamepad UI navigation
- 2D character movement with directional walking animations
- Space-key attack animation
- URP 2D rendering and tilemap-based content

## Requirements

- Unity **6000.6.7f1** (Unity 6)
- A desktop build target supported by Unity

The Unity project is configured for 2D URP and uses the Unity Input System package.

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/Asim-Tahir/lanternvale.git
   cd lanternvale
   ```

2. Open the repository folder in Unity Hub.
3. Select **Unity 6000.6.7f1**.
4. Open `Assets/Scenes/MainMenuScene.unity`.
5. Press **Play** in the Unity Editor.

## Controls

| Action        | Control                          |
| ------------- | -------------------------------- |
| Move          | `W`, `A`, `S`, `D` or arrow keys |
| Attack        | `Space`                          |
| Back / Cancel | `Escape` or gamepad East button  |

The main menu and settings panels support keyboard and gamepad selection through the Unity Event System.

## Project Structure

```text
Assets/
├── Animations/       Character and animation assets
├── Images/           Image assets
├── Palettes/         Tilemap palettes
├── Prefabs/          Reusable UI and game objects
├── Scenes/           Main menu and game scenes
├── Scripts/          C# gameplay and UI scripts
├── Settings/         URP and input settings
├── Sounds/           Background music and SFX
├── Sprites/          Characters, GUI, objects, and tilesets
└── Tiles/            Tile assets
```

## Key Scripts

- `Assets/Scripts/PlayerController.cs` - Handles movement, animation, and attack input.
- `Assets/Scripts/MainMenuController.cs` - Handles menu navigation, scene loading, continuation, and volume preferences.
- `Assets/Scripts/AudioManager.cs` - Persists and applies music/SFX mixer volumes and provides shared SFX playback.

## Scenes

The build scenes are configured in `ProjectSettings/EditorBuildSettings.asset`:

1. `Assets/Scenes/MainMenuScene.unity`
2. `Assets/Scenes/GameScene.unity`

## Development Notes

- Player and menu settings are stored with `PlayerPrefs`.
- The saved level key selects the scene loaded by the Continue button.
- The mixer exposes `MusicVolume` and `SFXVolume` parameters.
- Generated Unity directories such as `Library`, `Temp`, `Obj`, and `Build` are excluded through `.gitignore`.
