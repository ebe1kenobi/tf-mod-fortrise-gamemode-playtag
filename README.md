# PlayTag
<img width="640" height="400" alt="14_100217_round_00" src="https://github.com/user-attachments/assets/e3b28ac1-84cf-4c2d-9d7e-4c6a4f3b9593" />

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

<img width="757" height="441" alt="image" src="https://github.com/user-attachments/assets/9e82f723-5779-4eb9-8581-5d11dd85f417" />

Pick the **PlayTag** mode on the versus screen (left/right on the mode button), then
start the match as usual.

> **Opening the popup**: on the versus screen, with the relevant mode selected,
> press **Y** (the "arrows" button on the controller) on the mode button. A hint is
> shown under the button. The popup locks the menu while it is open (no going back,
> no starting the match); **A** or **B** closes it.

<img width="420" height="240" alt="14_100217_round_00" src="https://github.com/user-attachments/assets/b8d69957-9c20-4d08-b02c-a12bb50c4b4e" />
<img width="503" height="603" alt="image" src="https://github.com/user-attachments/assets/5845f4dd-c5a8-47f1-81d8-0b7ea95d2aa0" />

The PlayTag popup sets the **explosion delay**:

| Input | Effect |
|-------|--------|
| Left / Right | -1 s / +1 s |
| Up / Down | +5 s / -5 s |
| A or B | close |

It writes to the `Delay Game Mode` setting, so the value is kept between sessions.
Every change is saved to disk immediately: FortRise only saves settings when leaving
the game's Options menu, so a value changed here — or right before quitting — used
to be lost.

The two delays apply to different situations, and each is used where it belongs:
`Delay Game Mode` during a PlayTag match, `Delay Pickup` for a bomb picked up in a
normal versus.

### On-screen display

The bomb countdown is drawn above the archer holding it. During a PlayTag match the
**arrow counter is hidden**, since it occupies the same spot and the two overlapped.

## Settings

| Setting | Purpose |
|---------|---------|
| Pickup activated even when variant is not selected | spawn the pickup even when the variant is unticked |
| Treasure Rate 1 chance on N | pickup spawn odds: 1 chance in N |
| Delay Pickup | bomb delay when it comes from the pickup |
| Delay Game Mode | bomb delay in PlayTag mode (also set from the popup) |
| Periodicity | `Normal` (random roll) or `Test` (every level) |

## Game mode icon

The mode has its own icon, at the size of the game's four (184x82) and in their
style - a silhouette in three shades of one colour, no black: two archers, one of them marked.

It used to be borrowed from WARLORD's head, cropped. Two modes sharing
one picture cannot be told apart in the list.

The file is `ModFile/Content/Atlas/gamemode.png`.

## API for other mods

The mod exposes `IPlayTagApi`, so another mod can tell a tag match from a deathmatch
and know who is carrying the tag:

| Member | Answers |
|--------|---------|
| `IsPlayTagMatch()` | is the **running** match a tag match |
| `IsTagged(playerIndex)` | is this player carrying the tag |
| `TaggedPlayer()` | which player is, or `-1` |

`IsPlayTagMatch` reads the *level's* session and not the menu settings: the latter
describe what will be launched next time, not what is being played. The typical
caller - an AI, mid-match - needs the second answer.

It is used by **AIJimmy**, which flees the tagged player instead of chasing the
nearest one. Without it the AI played a deathmatch in the middle of a tag match,
charging at the player who was chasing it.

Members will be added in **separate interfaces**, never on this one: mod interop
builds its proxy from the *shape* of the members, so a caller declaring a member the
installed version does not have gets nothing at all.

## Where the rule lives

The countdown above the tagged player is the mode's indicator: it says **who** is it and
how long they have. When it reaches zero the bomb goes off and the round ends.

That explosion used to live in the countdown's `Render`, with the admission written next
to it - *"Yes I know, it's so bad to put that here"*. It worked as long as the countdown
was drawn. It was drawn by a patch on `Player.HUDRender`, a three-line method - exactly
the kind the JIT copies into its caller, in which case the patch never fires. The day it
did, the countdown vanished **and the round stopped ending**, because the rule went with
the drawing.

Two fixes, not one:

- the rule moved into `Update`, where it belongs. It now applies whether or not anyone
  is looking;
- the HUD component is **visible**, so Monocle draws it with the archer, instead of a
  patch calling its `Render` by hand. No more bet on inlining.

The arrow counter is hidden at the level of `ArrowHUD.Render` - the method below, big
enough not to be copied - and not by skipping `Player.HUDRender` entirely. The Soccer mod
does the same thing on the same method; the two coexist, each returning false only in
its own mode.

## Build / deployment

| Script | Purpose |
|--------|---------|
| `script/release.bat` | build, then assemble into `release/` |
| `script/deploy.bat` | copy `release/` into the TowerFall `Mods` folder |
| `script/release_deploy.bat` | both, one after the other |

Paths (game folder, module name) are set in `script/config.bat`.
