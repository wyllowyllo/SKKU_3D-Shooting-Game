# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a 3D shooting game developed in Unity (version 6000.0.60f1) for the SKKU x Com2us collaboration. The game features third-person and top-down gameplay modes with monster AI, weapon systems, and resource management.

## Build and Development Commands

### Opening the Project
- Open Unity Hub and select this project directory
- Unity Editor version: 6000.0.60f1
- Requires Universal Render Pipeline (URP) version 17.0.4

### Build Settings
- Build scenes are configured in: `ProjectSettings/EditorBuildSettings.asset`
- Scene order: LobbyScene → LoadingScene → GameScene
- To build: File → Build Settings in Unity Editor

### Testing
- Play mode: Press Play button in Unity Editor or Ctrl+P
- Test individual scenes by opening them from `Assets/0. Scenes/`
- Use Unity Test Framework for automated tests (package installed)

## Project Structure

### Asset Organization

The project uses a numbered folder structure under `Assets/`:

- `0. Scenes/` - Unity scene files (LobbyScene, LoadingScene, GameScene)
- `1. GameAssets/` - Game art and visual assets
- `2. Scripts/` - All C# scripts organized by functionality
- `3. Prefabs/` - Reusable GameObject prefabs
- `4. Sprites/` - 2D sprite assets
- `5. Materials/` - Materials and shaders
- `6. Animations/` - Animation clips and controllers
- `7. Fonts/` - Font assets
- `8. Models/` - 3D model files
- `9. Sounds/` - Audio files
- `10. Data/` - ScriptableObjects and data files
- `Resources/` - Runtime-loadable assets
- `Settings/` - Project settings and configurations

### Script Architecture

Scripts are located in `Assets/2. Scripts/` with the following organization:

#### Core Systems
- **Manager/** - Central game management
  - `GameManager.cs` - Singleton managing game state (Ready, Playing, Auto, GameOver), pause/resume, and cursor control
  - `GoldDropManager.cs` - Handles gold coin spawning and management

#### Player Systems
- **Player/** - Player character components
  - `PlayerInput.cs` - Centralizes all input handling (movement, shooting, zoom, view modes)
  - `PlayerMove.cs` - Character movement and physics
  - `PlayerRotate.cs` - Camera and character rotation
  - `PlayerGunFire.cs` - Gun firing mechanics
  - `PlayerBombFire.cs` - Bomb throwing mechanics
  - `PlayerStat.cs` - Player health and statistics
  - `IDamagable.cs` - Interface for damage-receiving entities

#### Enemy/AI Systems
- **Monster/** - Enemy AI implementation using Finite State Machine (FSM)
  - `MonsterStateController.cs` - FSM implementation with states: Idle, Patrol, Trace, Comeback, Jump, Attack, Hit, Death
  - `MonsterMove.cs` - Monster movement and navigation
  - `MonsterCombat.cs` - Attack logic
  - `MonsterHealth.cs` - Health management
  - `BossStateController.cs` / `BossCombat.cs` - Boss-specific variants
  - State enums: `EMonsterState.cs`, `EBossState.cs`

#### Weapon Systems
- **Weapon/** - Weapon implementation
  - `Gun.cs` - Raycast-based shooting with recoil, reload, and muzzle effects
  - `GunStat.cs` - Weapon statistics (damage, fire rate, ammo)
  - `Bomb.cs` - Explosive weapon implementation
  - `AttackInfo.cs` - Data structure for damage events (damage, direction, hit point, normal)

#### Camera Systems
- **Camera/** - Camera control
  - `CameraController.cs` - Main camera behavior
  - `CameraFollow.cs` - Smooth camera following
  - `CameraRotate.cs` - Camera rotation based on input
  - `CameraRecoil.cs` - Weapon recoil effects
  - `TopviewCamera.cs` - Top-down view implementation
  - `EZoomMode.cs` - Zoom mode enumeration

#### Supporting Systems
- **Factory/** - Object pooling for performance
  - `GoldFactory.cs` - Pools gold coins (uses Redcode.Pools)
  - `BombFactory.cs` - Pools bomb objects

- **Stats/** - Stat management system
  - `ValueStat.cs` - Simple value with increase/decrease
  - `ComsumableStat.cs` - Resource with max value and regeneration (health, stamina)

- **UI/** - User interface
  - `UI_PlayerStats.cs` - Player HUD display
  - `UI_Crosshair.cs` - Crosshair rendering
  - `BloodScreenEffect.cs` - Damage screen effect
  - `UI_OptionPopup.cs` - Pause menu

- **Scene/** - Scene management
  - `LoadingScene.cs` - Async scene loading with progress bar
  - `LoginScene.cs` - Login/registration using PlayerPrefs

- **Items/** - Collectible items
  - `GoldCoin.cs` - Currency pickup

## Key Architecture Patterns

### Finite State Machine (FSM) for AI
Monster AI uses a state-based system in `MonsterStateController.cs`:
- Each state has its own method (Idle(), Patrol(), Trace(), Attack(), etc.)
- State transitions occur based on distance to player, attack range, and damage events
- Timer-based cooldowns for attacks and patrol delays

### Singleton Pattern
Several managers use singleton pattern:
- `GameManager` - Global game state
- `GoldFactory` - Object pooling
- All factory classes follow this pattern

### Component-Based Design
Player and monsters are composed of multiple specialized components:
- Movement, combat, health, and input are separate MonoBehaviours
- Components communicate through direct references (GetComponent)
- `IDamagable` interface allows any object to receive damage

### Object Pooling
Uses Redcode.Pools library for frequently spawned objects (gold coins, bombs) to improve performance.

### Data Structures
- `AttackInfo` - Encapsulates all damage-related data
- `ValueStat` / `ComsumableStat` - Serializable stat classes for health, stamina, etc.
- ScriptableObjects for weapon stats (`GunStat`)

### Input Handling
- Hybrid approach: Unity's new Input System (`InputSystem_Actions.inputactions`) is configured but current code uses legacy `Input` class
- All input is centralized in `PlayerInput.cs` for easy rebinding
- Game state awareness: inputs only active when `GameManager.State == EGameState.Playing`

### Scene Flow
1. **LobbyScene** - Login/registration (stores credentials in PlayerPrefs)
2. **LoadingScene** - Async loading with progress indicator
3. **GameScene** - Main gameplay with countdown (Ready → Go → Playing)

## Important Dependencies

- Unity Input System (1.14.2) - Configured but not fully utilized
- Universal Render Pipeline (17.0.4)
- AI Navigation (2.0.9) - For monster pathfinding
- Redcode.Pools - For object pooling
- TextMeshPro - For UI text

## Coding Conventions

- Private fields use `_camelCase` prefix
- Properties use `PascalCase`
- Enums prefixed with `E` (EGameState, EMonsterState, EBossState)
- Serialized fields marked with `[SerializeField]` even if private
- Korean comments are present in some files (this is a Korean university project)
- State machine pattern: one method per state
- Initialization in `Awake()`, setup in `Start()`

## Common Gotchas

1. **Cursor Management**: GameManager controls cursor state based on game mode (Playing vs Auto/TopView)
2. **Input Gating**: Many input checks verify `GameManager.Instance.State == EGameState.Playing`
3. **Pause System**: Uses `Time.timeScale = 0` which affects all time-based operations
4. **Object Pooling**: Always return pooled objects to factory, never Destroy()
5. **Damage System**: Uses `IDamagable` interface - implement `TryTakeDamage(AttackInfo)` for damageable objects
6. **Monster AI**: State changes must go through `ChangeState()` method to ensure proper cleanup
7. **Scene Loading**: Always use LoadingScene as intermediate step, never direct scene transition

## Testing Patterns

- Game can be tested in Play mode from any scene
- GameManager initializes even if not starting from LobbyScene
- Monster AI can be observed with Gizmos in Scene view
- Test scripts exist in `Assets/2. Scripts/Test/` for experimentation
