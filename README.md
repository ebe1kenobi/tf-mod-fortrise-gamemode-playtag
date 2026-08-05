# PlayTag

**PlayTag** game mode: one player carries the bomb and has to pass it on by
touching someone else. Once the delay runs out, the bomb explodes and kills whoever
holds it. The mod also adds a pickup that hands the bomb over.

A mod for **FortRise 5** (>= 5.3.3). The FortRise 4 version (`tf-mod-fortrise-gamemode-playtag`) is no longer maintained: fixes and new features only land in this repository.

## Installation

1. Install FortRise 5 and start the game through `FortRise.exe`.
2. Copy `release/playtag` (or the shipped folder) into `<TowerFall>/FortRise/Mods/`.

Settings are under **Options > Mods > PlayTag**.
Data and log files live in `<TowerFall>/FortRise/Saves/PlayTag/` and `<TowerFall>/FortRise/Logs/`.

## Usage

Pick the **PlayTag** mode on the versus screen (left/right on the mode button), then
start the match as usual.

> **Opening the popup**: on the versus screen, with the relevant mode selected,
> press **Y** (the "arrows" button on the controller) on the mode button. A hint is
> shown under the button. The popup locks the menu while it is open (no going back,
> no starting the match); **A** or **B** closes it.

The PlayTag popup sets the **explosion delay**:

| Input | Effect |
|-------|--------|
| Left / Right | -1 s / +1 s |
| Up / Down | +5 s / -5 s |
| A or B | close |

It writes to the `Delay Game Mode` setting, so the value is kept between sessions.

## Settings

| Setting | Purpose |
|---------|---------|
| Pickup activated even when variant is not selected | spawn the pickup even when the variant is unticked |
| Treasure Rate 1 chance on N | pickup spawn odds: 1 chance in N |
| Delay Pickup | bomb delay when it comes from the pickup |
| Delay Game Mode | bomb delay in PlayTag mode (also set from the popup) |
| Periodicity | `Normal` (random roll) or `Test` (every level) |

## Build / deployment

| Script | Purpose |
|--------|---------|
| `script/release.bat` | build, then assemble into `release/` |
| `script/deploy.bat` | copy `release/` into the TowerFall `Mods` folder |
| `script/release_deploy.bat` | both, one after the other |

Paths (game folder, module name) are set in `script/config.bat`.
