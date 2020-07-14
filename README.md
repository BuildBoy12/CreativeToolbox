# CreativeToolbox
An SCP:SL EXILED plugin that makes your servers creative with a variety of customizable options!

### Features
- Scopophobia Support
- Players can have regeneration
- Players can have infinite ammo
- Players can pry gates open
- Players can explode if they drink too much SCP-207
- Players can pry gates open if they drink enough SCP-207
- Players can be exploded via command
- Players can be located via command
- Players can be automatically scaled (it can also be kept if people leave)
- Players can be automatically respawned as a random class (or the same class)
- Players can have AHP that does not go down naturally, only by damage or up with more medical items
- AHP can have a limit to prevent abuse
- Grenades spawn on players after they die (support for custom timers)
- Grenades can have custom timers (works independently from player grenade spawns)
- Grenade damage can be prevented (and SCP-018 damage)
- Fall damage can be prevented (also can be toggled via RA command)
- Warhead can be set off with a custom timer or immediately (only goes up to 1 minute and 43 seconds max)
- Warhead destroys every door in the facility after detonation (WIP)
- Decontamination can be force started (cannot be stopped if this happens)
- Messages appear when interacting with certain doors, and with bypass, also with prying gates
- SCP-018 can make the warhead set off every time it bounces
- SCP-096 can gain a custom amount of Hume Shield when each target looks at them or shoots at them
- Custom announcement messages for Mobile Task Force / Chaos Insurgency respawn waves

### Notes
- These values can be changed in game / overridden for only one round if people want to do events, if these plugins are enabled that is
- You must put the included 0harmony.dll file inside the "dependencies" folder within (%appdata%/plugins/dependencies for Windows or ~/.config/plugins/dependencies) for this plugin to work.

### Configuration Settings
Configuration Option | Configuration Data Type | Default Value | Description
:---: | :---: | :---: | :------
is_enabled | Boolean | True | Whether the CreativeToolbox plugin will be enabled or not
enable_custom_grenade_time | Boolean | False | Whether the custom grenade timers will apply in-game or not
enable_custom_healing | Boolean | False | Whether the medical items can give AHP or not
enable_fall_damage_prevention | Boolean | False | Whether the fall damage will be disabled by default or not
enable_grenade_damage_prevention | Boolean | False | Whether explosives or SCP-018 will deal damage or not
enable_auto_scaling | Boolean | false | Whether auto-scaling at the start of the round is enabled or not
enable_keep_scale | Boolean | false | Whether the auto-scaling re-applies when the user re-joins or not
enable_grenade_on_death | Boolean | false | Whether players will drop live grenades on their death or not
enable_door_messages | Boolean | false | Whether door messages display on gates and doors or not
enable_exploding_after_drinking_scp207 | Boolean | false | Whether users can pry gates and explode after drinking SCP-207 or not
enable_scp018_warhead_bounce | Boolean | false | Whether the warhead will get set off every time SCP-018 bounces or not
enable_ahp_shield | Boolean | false | Whether the AHP Shield will remain constant or not (WARNING: It WILL affect SCP-096's AHP)
enable_scp106_advanced_god | Boolean | false | Whether SCP-106 can be contained if he has godmode or not
enable_custom_announcements | Boolean | false | Whether CASSIE will say custom announcements when waves respawn or not
enable_custom_scp096_shield | Boolean | false | Whether SCP-096 will get custom AHP Shield added for every target or not
enable_reverse_role_respawn_waves | Boolean | false | Whether respawn waves will be reversed or not (MTF in Van, CI in Heli)
enable_doors_destroyed_with_warhead | Boolean | false | Whether doors are destroyed after the warhead explodes or not (WARNING: It WILL lag players, potentially crash!)
disable_autoscale_messages | Boolean | false | Whether to disable auto-scale related messages or not
disable_fall_modification | Boolean | false | Whether people with RA access and permissions can modify fall damage
locked_door_message | String | you need a better keycard to open this door! | The message displayed when the user tries to open a locked door with a lower keycard
unlocked_door_message | String | you held the keycard next to the reader| The message displayed when the user tries to open a locked door with a keycard
need_keycard_message | String | this door requires a keycard | The message displayed when the user tries to open a locked door without a keycard
bypass_keycard_message | String | you bypassed the reader | The message displayed when the user bypasses a door
bypass_with_keycard_message | String | you bypassed the reader, but you did not need a keycard | The message displayed when the user bypasses a door with a keycard in their hand
pry_gate_message | String | you pried the gate open | The message displayed when the user pries open a gate
pry_gate_bypass_message | String | you pried the gate open, but you could bypass it | The message displayed when the user pries open a gate with a keycard in their hand
chaos_insurgency_announcement | String | The ChaosInsurgency have entered the facility | The message that is broadcaster by CASSIE when Chaos Insurgency respawn
nine_tailed_fox_announcement | String | '' | The message that is broadcaster by CASSIE when Nine Tailed Fox respawn
grenade_timer_on_death | Float | 5 | The amount (in seconds) it takes to regenerate health for a given player
regeneration_time | Float | 1 | The amount (in seconds) it takes to regenerate health for a given player
regeneration_value | Float | 5 | The amount of health regenerated per interval for a given player
random_respawn_timer | Float | 0.05 | The amount (in seconds) it takes to automatically respawn a player
frag_grenade_fuse_timer | Float | 5 | The amount (in seconds) it takes to blow up a frag grenade
flash_grenade_fuse_timer | Float | 3 | The amount (in seconds) it takes to blow up a flash grenade
painkiller_ahp_health_value | Float | 0 | The amount of AHP given if a player uses Painkillers
medkit_ahp_health_value | Float | 0 | The amount of AHP given if a player uses Medkits
adrenaline_ahp_health_value | Float | 0 | The amount of AHP given if a player uses Adrenaline
scp500_ahp_health_value | Float | 0 | The amount of AHP given if a player uses SCP-500
scp207_ahp_health_value | Float | 0 | The amount of AHP given if a player uses SCP-207
scp207_drink_limit| Float | 5 | The number of drinks it takes for a user to explode
scp207_pry_gate_limit | Float | 3 | The number of drinks it takes for a user to be able to pry gates
auto_scale_value | Float | 1 | The scale factor players are set to with auto-scaling

### Permission Values
- ct.* (all permissions)
- ct.arspawn (random auto respawn)
- ct.autoscale (forcing auto-scaling)
- ct.explode (exploding users)
- ct.fdamage (fall damage)
- ct.giveammo (give ammo)
- ct.gnade (grenade timers)
- ct.infammo (infinite ammo)
- ct.locate (locating users)
- ct.nuke (custom nuke)
- ct.prygates (prying gates)
- ct.regen (regeneration)
- ct.sdecon (force start decontamination)

### Remote Admin Commands
- arspawn (on/off/time) (value (if "time" is selected))
  - on/off (Enables or disables randomly auto respawning)
  - time (value) (Changes the number (in seconds) it takes to respawn a player)
- autoscale (on/off)
  - on/off (Enables or disables auto-scaling in the game)
- explode ((id/name)/(asterisk)/all)
  - id/name (Explodes the specified user)
  - (asterisk)/all (Explodes everyone)
- fdamage (on/off)
  - on/off (Enables or disables fall damage)
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
- locate (xyz/room) (id/name)
  - xyz (id/name) (Gives the coordinates of a specified player)
  - room (id/name) (Gives the room name a specified player is in)
- nuke (instant/start) (time (if "start" is selected))
  - instant (Sets off the warhead instantly, no 10 second wait like "detonate")
  - start (time) (Starts the warhead at the specified time, high is 142, low is 0.05)
- prygates ((id/name)/(asterisk)/all/clear/list)
  - id/name (Gives or removes the ability to pry gates from the specified player)
  - (asterisk)/all (Gives everyone the ability to pry gates)
  - clear (Clears all players of prying gates)
  - list (Lists all players with the ability to pry gates)
- regen ((id/name)/(asterisk)/all/clear/list/time/value) (value (if time or value is selected))
  - id/name (player name / player id) (Gives or removes Regeneration from the specified player)
  - (asterisk)/all (Gives everyone Regeneration)
  - clear (Clears all players of Regeneration)
  - list (Lists all players with Regeneration)
  - time (value) (Sets the time (in seconds) it takes to regenerate health
  - value (value) (Sets the amount of health you gain per interval)
- sdecon (Turns on Light Containment Zone decontamination (NOTE:  be reversible!))
