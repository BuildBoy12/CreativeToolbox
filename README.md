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
- Players can be automatically scaled (It can also be kept if people leave)
- Players can be automatically respawned as a random class (Or the same class)
- Players can have AHP that does not go down naturally, only by damage or up with more medical items
- AHP can have a limit to prevent abuse
- Grenades spawn on players after they die (support for custom timers)
- Grenades can have custom timers (Works independently from player grenade spawns)
- Grenade damage can be prevented (And SCP-018 damage)
- Fall damage can be prevented (Also can be toggled via RA command)
- Warhead can be set off with a custom timer or immediately (Only goes up to 1 minute and 42 seconds max)
- Warhead destroys every door in the facility after detonation
- Warhead can automatically be set off instantly by chance if it is cancelled
- Decontamination can be force started (Cannot be stopped if this happens)
- Messages appear when interacting with certain doors, and with bypass, also with prying gates
- SCP-018 can make the warhead set off every time it bounces
- SCP-096 can gain a custom amount of Hume Shield when each target looks at them or shoots at them
- Custom announcement messages for Mobile Task Force / Chaos Insurgency respawn waves (With support for unit names and numbers, also scp's left (unit names & numbers for NTF only, scp's left for both NTF and CI)
- Custom SCP-207 messages after drinking them
- Custom jamming and glitch chances for custom announcement messages
- Random chance for Chaos Insurgency Announcment
- Option to disable all CreativeToolbox broadcasts

### Notes
- A lot of these values can be changed in game / overridden for only one round if people want to do events, if these plugins are enabled that is
- You must put the included 0harmony.dll file inside the "dependencies" folder within (%appdata%/exiled/plugins/dependencies for Windows or ~/.config/exiled/plugins/dependencies) for this plugin to work.

### Note for Custom Announcements
- You can use %unitname for the NTF Unit Name, %unitnumber for the NTF Unit Number, and %scpnumber for the amount of SCPs left
- %scpnumber is the only variable that works with both Chaos Insurgency and NTF Announcements
- %scpnumber has AwaitingRecontainment in it, you do not need to re-add it in your announcement lines

### Note for Custom SCP-207 messages
- You can use %counter to show how many drinks the user consumed

### Configuration Settings
```yaml
creative_toolbox:
  is_enabled: true
  enable_custom_grenade_time: false
  enable_custom_healing: false
  enable_fall_damage_prevention: false
  enable_grenade_damage_prevention: false
  enable_auto_scaling: false
  enable_keep_scale: false
  enable_grenade_on_death: false
  enable_door_messages: false
  enable_kill_messages: false
  enable_custom_effects_after_drinking_scp207: false
  enable_scp018_warhead_bounce: false
  enable_ahp_shield: false
  enable_scp106_advanced_god: false
  enable_custom_announcements: false
  enable_custom_scp096_shield: false
  enable_reverse_role_respawn_waves: false
  enable_doors_destroyed_with_warhead: false
  enable_random_chaos_insurgency_announcement_chance: false
  enable_warhead_detonation_when_canceled_chance: false
  disable_auto_scale_messages: false
  use_xmas_scp_in_announcement: false
  prevent_ct_broadcasts: false
  locked_door_message: you need a better keycard to open this door!
  unlocked_door_message: you held the keycard next to the reader
  need_keycard_message: this door requires a keycard
  bypass_keycard_message: you bypassed the reader
  bypass_with_keycard_message: you bypassed the reader, but you did not need a keycard
  pry_gate_message: you pried the gate open
  pry_gate_bypass_message: you pried the gate open, but you could bypass it
  drinking_scp207_message: 'Number of drinks consumed: %counter'
  pry_gates_with_scp207_message: You can now pry gates open
  explode_after_scp207_message: You drank too much and your body could not handle it
  chaos_insurgency_announcement: The ChaosInsurgency have entered the facility %scpnumber
  nine_tailed_fox_announcement: MtfUnit Epsilon 11 Designated %unitname %unitnumber HasEntered AllRemaining %scpnumber
  scp096_ahp: 250
  grenade_timer_on_death: 5
  regeneration_time: 1
  regeneration_value: 5
  random_respawn_timer: 0.0500000007
  frag_grenade_fuse_timer: 5
  flash_grenade_fuse_timer: 3
  painkiller_ahp_health_value: 0
  medkit_ahp_health_value: 0
  adrenaline_ahp_health_value: 0
  scp500_ahp_health_value: 0
  scp207_ahp_health_value: 0
  scp207_drink_limit: 5
  scp207_pry_gate_limit: 3
  auto_scale_value: 1
  ahp_value_limit: 75
  chaos_insurgency_announcement_glitch_chance: 0
  chaos_insurgency_announcement_jam_chance: 0
  nine_tailed_fox_announcement_glitch_chance: 0
  nine_tailed_fox_announcement_jam_chance: 0
  chaos_insurgency_announcement_chance: 50
  instant_warhead_detonation_chance: 10
```

### Permission Values
- ct.* (all permissions)
- ct.autorespawn (random auto respawn)
- ct.autoscale (forcing auto-scaling)
- ct.explode (exploding users)
- ct.falldamage (fall damage)
- ct.giveammo (give ammo)
- ct.customnade (grenade timers)
- ct.infammo (infinite ammo)
- ct.locate (locating users)
- ct.nuke (custom nuke)
- ct.prygates (prying gates)
- ct.regen (regeneration)
- ct.startdecon (force start decontamination)

### Remote Admin Commands
- autorespawn (on/off/time) (value (if "time" is selected))
  - on/off (Enables or disables randomly auto respawning)
  - time (value) (Changes the number (in seconds) it takes to respawn a player)
- autoscale (on/off)
  - on (value) (Enables auto-scaling and sets everyone to the value that is set)
  - off (Turns off auto-scaling)
- explode ((id/name)/(asterisk)/all)
  - id/name (Explodes the specified user)
  - (asterisk)/all (Explodes everyone)
- falldamage (on/off)
  - on/off (Enables or disables fall damage)
- giveammo ((id/name)/(asterisk)/all) (ammo type (5, 7, 9)) (amount)
  - id/name (ammo type) (amount) (Gives the player the specified amount of ammo for the specified ammo type)
  - (asterisk)/all (ammo type) (amount) (Gives everyone the specified amount of ammo for the specified ammo type)
- customnade (frag/flash) (value)
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
- startdecon (Turns on Light Containment Zone decontamination (NOTE:  be reversible!))
