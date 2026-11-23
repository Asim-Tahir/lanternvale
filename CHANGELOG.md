# Changelog

## [Unreleased]

## [0.2.0] - 2026-11-24

### Finding Our Way Release

This release adds an inventory, farming, pathfinding, pause menus and more persistance.

#### Highlights

- Added a persistent inventory.
- Added stackable inventory items with quantity display.
- Added planting and deplanting on the tile grid.
- Added automatic pathfinding to reach tiles for planting and deplanting.
- Added a pause menu and settings dialog.

#### Gameplay

- Added tools and seeds, including the hoe tool and the apple and virgo seeds.
- Added the Inventory button to open and close the inventory, with the E keybinding.

#### Menus and UI

- Added Continue support for sessions with planted tiles.
- Added New Game support that clears planted tiles.
- Refined the main menu and dialog prefabs.

#### Persistence

- Added persistent game progress and level.
- Added persistent music and SFX settings.
- Added persistent planted tiles.

#### Audio

- Added planting and hit-plant sound effects.

**Full Changelog**: https://github.com/Asim-Tahir/lanternvale/compare/v0.1.0...v0.2.0

## [0.1.0] - 2026-11-18

### Dawn at the Vale Release

This release establishes the first playable version of Lanternvale, a 2D pixel-art top-down game built with Unity 6.

#### Highlights

- Added a complete main menu with Continue, New Game, Settings, and Exit actions.
- Added a playable 2D game scene with terrain, water, and player movement.
- Added keyboard and gamepad support for player and menu interactions.
- Added saved level and persistent volume settings using `PlayerPrefs`.
- Added a pixel-art UI theme with reusable menu, button, and slider prefabs.
- Added music and SFX audio management through Unity Audio Mixer.

#### Gameplay

- Implemented 2D player movement with WASD and arrow-key controls.
- Added directional walking animations and idle facing.

#### Menus and UI

- Added Continue support based on the saved level.
- Added New Game support that starts the first game scene.
- Added persistent music and SFX volume sliders.
- Added keyboard and gamepad UI navigation.
- Added Escape and gamepad East-button handling for returning from settings.

#### Audio

- Added background music and SFX audio files.
- Added separate, persistent music and SFX volume controls.

**Full Changelog**: https://github.com/Asim-Tahir/lanternvale/commits/v0.1.0
