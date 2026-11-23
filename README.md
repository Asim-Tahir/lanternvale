# Lanternvale

Lanternvale is a 2D pixel-art game project built with Unity. The repository currently includes a main menu, settings interface, game scene, player movement and attack animations, persistent volume settings, and saved level selection.

## Screen Recording 

> [!TIP]
> For the best experience, turn on the audio.

https://github.com/user-attachments/assets/f930642a-82ea-48fa-94bd-9d7161bf4463

## Screenshots

<table>
  <tr>
    <td>
      <figure>
        <img width="1600" alt="Main menu" src="https://github.com/user-attachments/assets/339d80eb-1672-45df-b6a6-dd14c5d4b68d" />
        <figcaption align="center">Main menu</figcaption>
      </figure>
    </td>
    <td>
      <figure>
        <img width="1600" alt="Settings menu for music and SFX volume levels" src="https://github.com/user-attachments/assets/49e3b1f1-b96f-4ef8-8130-679046fad4be" />
        <figcaption align="center">Settings menu for adjusting music and SFX volume levels</figcaption>
      </figure>
    </td>
  </tr>
  <tr>
    <td>
      <figure>
        <img width="1600" alt="First frame after starting a new game" src="https://github.com/user-attachments/assets/817c3799-22bd-40d4-a052-5b54825a189e" />
        <figcaption align="center">First frame shown after selecting New Game</figcaption>
      </figure>
    </td>
    <td>
      <figure>
        <img width="1600" alt="Auto path finding to plant or deplant" src="https://github.com/user-attachments/assets/873ad220-c15b-4c57-a2fc-e0c037b49c63" />
        <figcaption align="center">Auto path finding to plant or deplant</figcaption>
      </figure>
    </td>
  </tr>
  <tr>
    <td>
      <figure>
        <img width="1600" alt="Empty player inventory with bottom hotbar" src="https://github.com/user-attachments/assets/33e89683-a9a4-420c-8260-057ad5bef6b2" />
        <figcaption align="center">Empty player inventory with bottom hotbar</figcaption>
      </figure>
    </td>
    <td>
      <figure>
        <img width="1600" alt="Main menu with the Continue button enabled" src="https://github.com/user-attachments/assets/8d92ea89-494f-495e-905c-5e70d74d5209" />
        <figcaption align="center">Main menu with the Continue (Devam et) button enabled after starting a game</figcaption>
      </figure>
    </td>
  </tr>
</table>

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

## Credit

- Sprout Lands Asset Pack by Cup Nooble
  - Link: https://cupnooble.itch.io/sprout-lands-asset-pack
- Sprout Lands UI Pack by Cup Nooble
  - Link: https://cupnooble.itch.io/sprout-lands-ui-pack
- Gaming Game Video Game Music by Viacheslav Starostin
  - Link: https://pixabay.com/music/video-games-gaming-game-video-game-music-474517/
- plant by Prmodrai
  - Link: https://pixabay.com/sound-effects/film-special-effects-plant-408205/
- Hit Plant 01 by u_xjrmmgxfru
  - Link: https://pixabay.com/sound-effects/film-special-effects-hit-plant-01-266293/
- Pixelify Sans by Stefie Justprince
  - Link: https://fonts.google.com/specimen/Pixelify+Sans
