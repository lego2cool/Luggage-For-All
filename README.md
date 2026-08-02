# Luggage For All

Luggage For All makes luggage reusable during a run by automatically refilling it after a configurable cooldown.

## Features

- Every luggage object receives a refill timer automatically.
- Opening luggage starts a shared, synchronized countdown for all players.
- The luggage refills when the countdown reaches zero and can be opened again.
- The countdown is displayed above the luggage while it is open.

## Configuration

Configuration is available in the BepInEx config file after the mod has been launched once:

`BepInEx/config/legocool.LuggageForAll.cfg`

| Setting | Default | Description |
| --- | ---: | --- |
| `Refill Duration` | `60` | Number of seconds before opened luggage refills. |
| `Timer Font Size` | `5` | Size of the countdown text displayed above luggage. |
| `Show Timer` | `true` | Whether to display the countdown timer. |
| `Max Reopens` | `-1` | Maximum number of times luggage can refill. Set to `-1` for unlimited refills. |

### Luggage Types

These settings are in the `Luggages` section of the config file. Disable a type to prevent that luggage from refilling.

| Setting | Default | Description |
| --- | ---: | --- |
| `Luggage Small` | `true` | Whether small luggage should refill. |
| `Luggage Big` | `true` | Whether big luggage should refill. |
| `Luggage Epic` | `true` | Whether explorer luggage should refill. |
| `Luggage Ancient` | `true` | Whether ancient luggage should refill. |

## Installation

1. Install [BepInExPack PEAK](https://thunderstore.io/c/peak/p/BepInEx/BepInExPack_PEAK/).
2. Install this mod with a compatible Thunderstore mod manager, or extract the mod's files into the PEAK installation directory.
3. Launch PEAK once to generate the BepInEx configuration file.

## Multiplayer

The countdown is synchronized through Photon, and only the master client triggers the refill. To see the timer above the luggage you must have the mod installed

## Support

Report bugs and issues on the mod's github repository or ping me in the [PEAK Modding discord server](https://discord.gg/SAw86z24rB).

## Credits

- **Author** distinctdonut
