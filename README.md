# Lanternvale

Lanternvale is a 2D pixel-art game project built with Unity. The repository currently includes a main menu, settings interface, game scene, player movement and attack animations, persistent volume settings, and saved level selection.

## Screen Recording 

> [!TIP]
> For the best experience, turn on the audio.

https://github.com/user-attachments/assets/15fb5004-8c78-4ebd-8b28-6471d6d1d169

## Screenshots

<table>
  <tr>
    <td>
      <figure>
        <img width="1600" alt="Main menu" src="https://github.com/user-attachments/assets/4219aaf0-ac4d-424f-a3a6-1f659a573c5e" />
        <figcaption align="center">Main menu</figcaption>
      </figure>
    </td>
    <td>
      <figure>
        <img width="1600" alt="Settings menu for music and SFX volume levels" src="https://github.com/user-attachments/assets/234b64dc-651a-46cd-9e78-dece82636abe" />
        <figcaption align="center">Settings menu for adjusting music and SFX volume levels</figcaption>
      </figure>
    </td>
  </tr>
  <tr>
    <td>
      <figure>
        <img width="1600" alt="First frame after starting a new game" src="https://github.com/user-attachments/assets/faaf84d1-7592-4bba-acc0-cb4c3ccdd959" />
        <figcaption align="center">First frame shown after selecting New Game</figcaption>
      </figure>
    </td>
    <td>
      <figure>
        <img width="1600" alt="Southeast view of the terrain" src="https://github.com/user-attachments/assets/e63bd43e-90db-4fa2-bc8a-dde55257de1b" />
        <figcaption align="center">Southeast view of the terrain</figcaption>
      </figure>
    </td>
  </tr>
  <tr>
    <td>
      <figure>
        <img width="1600" alt="South view of the terrain, with the character over the water beside a lily pad" src="https://github.com/user-attachments/assets/608c2b6a-fe54-4225-a69e-de8c928c09d4" />
        <figcaption align="center">South view of the terrain, with the character over the water beside a lily pad</figcaption>
      </figure>
    </td>
    <td>
      <figure>
        <img width="1600" alt="Northeast view of the terrain" src="https://github.com/user-attachments/assets/cd3c31ab-0c9e-4842-b607-66e3bf620c62" />
        <figcaption align="center">Northeast view of the terrain</figcaption>
      </figure>
    </td>
  </tr>
  <tr>
    <td>
      <figure>
        <img width="1600" alt="Northwest view of the terrain" src="https://github.com/user-attachments/assets/b6286a82-7d69-4a5d-9dd0-57e9340a6104" />
        <figcaption align="center">Northwest view of the terrain</figcaption>
      </figure>
    </td>
    <td>
      <figure>
        <img width="1600" alt="Southwest view of the terrain" src="https://github.com/user-attachments/assets/b32aadbd-688b-4cf5-8057-dae4c5d26893" />
        <figcaption align="center">Southwest view of the terrain</figcaption>
      </figure>
    </td>
  </tr>
  <tr>
    <td>
      <figure>
        <img width="1600" alt="Main menu with the Continue button enabled" src="https://github.com/user-attachments/assets/a8799914-9550-4850-9be5-cc92b523d87c" />
        <figcaption align="center">Main menu with the Continue (Devam et) button enabled after starting a game</figcaption>
      </figure>
    </td>
    <td></td>
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
- Pixelify Sans by Stefie Justprince
  - Link: https://fonts.google.com/specimen/Pixelify+Sans
