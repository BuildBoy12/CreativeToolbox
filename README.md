# CreativeToolbox
An SCP:SL EXILED plugin that makes your servers creative with a variety of customizable options!

### Features
- Players can have Invisibility
- Players can have Regeneration
- Players can have Infinite Ammo
- Players can have SCP-207 Speed + Resistance
- Players can respawn as a random class
- Flash Grenades and Frag Grenades timer modification
- Fall Damage can be prevented
- Grenade Damage (and SCP-018 Damage) can be prevented
- Medical Items can give AHP
- Locations of Players can be obtained (Rooms and Coordinates)

### Notes
- These values can be changed in game / overridden for only one round if people want to do events, if these plugins are enabled that is
- You must put the included 0harmony.dll file inside the "dependencies" folder within (%appdata%/plugins/dependencies for Windows or ~/.config/plugins/dependencies) for this plugin to work.

### Configuration Settings
Configuration Option | Configuration Data Type | Default Value | Description
:---: | :---: | :---: | :------
creativetoolbox_enable | Bool | True | Whether the CreativeToolbox plugin will be enabled or not
ct_enable_custom_grenade_time | Bool | False | Whether the custom grenade timers will apply in-game or not
ct_enable_fall_damage_prevention | Bool | False | Whether the fall damage will be disabled by default or not
ct_enable_custom_healing | Bool | False | Whether the medical items can give AHP or not
ct_enable_grenade_damage_prevention | Bool | False | Whether explosives or SCP-018 will deal damage or not
ct_regeneration_time | Float | 5 | The amount (in seconds) it takes to regenerate health for a given player
ct_regeneration_value | Float | 5 | The amount of health regenerated per interval for a given player
ct_random_respawn_timer | Float | 0.05 | The amount (in seconds) it takes to automatically respawn a player
ct_frag_grenade_fuse_timer | Float | 5 | The amount (in seconds) it takes to blow up a frag grenade
ct_flash_grenade_fuse_timer | Float | 3 | The amount (in seconds) it takes to blow up a flash grenade
ct_painkillers_ahp_healing | Float | 0 | The amount of AHP given if a player uses Painkillers
ct_medkit_ahp_healing | Float | 0 | The amount of AHP given if a player uses Medkits
ct_adrenaline_ahp_healing | Float | 0 | The amount of AHP given if a player uses Adrenaline
ct_scp500_ahp_healing | Float | 0 | The amount of AHP given if a player uses SCP-500

### Permission Values
- ct.* (all permissions)
- ct.arspawn (random auto respawn)
- ct.fdamage (fall damage)
- ct.fspeed (fast speed (SCP-207 effect))
- ct.giveammo (give ammo)
- ct.gnade (grenade timers)
- ct.infammo (infinite ammo)
- ct.invis (invisibility (SCP-268 effect))
- ct.locate (locating users)
- ct.regen (regeneration)

### Remote Admin Commands
- arspawn (on/off/time) (Value (if value is selected))
  - on/off (Enables or disables randomly auto respawning)
  - time (value) (Changes the number (in seconds) it takes to respawn a player)
- fdamage (on/off)
  - on/off (Enables or disables fall damage)
- fspeed ((id/name)/(asterisk)/all/clear/list)
  - id/name (player name / player id) (Gives or removes Fast Speed from the specified player)
  - (asterisk)/all (Gives everyone Fast Speed)
  - clear (Clears all players of Fast Speed)
  - list (Lists all players with Fast Speed)
- giveammo ((id/name)/(asterisk)/all) (ammo type (5, 7, 9)) (amount)
  - id/name (ammo type) (amount) (Gives the player the specified amount of ammo for the specified ammo type)
  - (asterisk)/all (ammo type) (amount) (Gives everyone the specified amount of ammo for the specified ammo type)
- gnade (frag/flash) (value)
  - frag (value) (Sets the time (in seconds) until frag grenades blow up)
  - flash (value) (Sets the time (in seconds) until flash grenades blow up)
- infammo ((id/name)/(asterisk)/all/clear/list)
  - id/name (Gives or removes Infinite Ammo from the specified player)
  - (asterisk)/all (Gives everyone Infinite Ammo)
  - clear (Clears all players of Infinite Ammo)
  - list (Lists all players with Infinite Ammo)
- invis ((id/name)/(asterisk)/all/clear/list)
  - id/name (Gives or removes Invisibility from the specified player)
  - (asterisk)/all (Gives everyone Invisibility)
  - clear (Clears all players of Invisibility)
  - list (Lists all players with Invisibility)
- locate (xyz/room) (id/name)
  - xyz (id/name) (Gives the coordinates of a specified player)
  - room (id/name) (Gives the room name a specified player is in)
- regen ((id/name)/(asterisk)/all/clear/list/time/value) (value (if time or value is selected))
  - id/name (player name / player id) (Gives or removes Regeneration from the specified player)
  - (asterisk)/all (Gives everyone Regeneration)
  - clear (Clears all players of Regeneration)
  - list (Lists all players with Regeneration)
  - time (value) (Sets the time (in seconds) it takes to regenerate health
  - value (value) (Sets the amount of health you gain per interval)
