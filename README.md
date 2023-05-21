# Dark-Holes
Dark-Holes is a desktop application that adds Buttplug.io-integrated haptic feedback to Dark Souls games.

This allows one to connect a device supporting the Buttplug.io protocol (presumably a sex toy) and have that device's vibrations/stroking/motions/etc. dynamically respond to events in the game, such as taking damage or making attacks.

See here for more information on Buttplug.io and what devices support it: https://buttplug.io/

---

# Important note!
## ***__Use this application at your own risk!__***
Dark-Holes works by hooking into and reading (but not altering!) the memory data of your running game.

### __*This opens the possibility of the game's anti-cheat banning/soft-banning you for using this application.*__

The anti-cheat for the Dark Souls series *supposedly* will not be triggered if no game data is changed and only memory values are read (which is what Dark-Holes does), which has been [detailed here.](https://github.com/igromanru/Dark-Souls-III-Cheat-Engine-Guide#what-does-and-does-not-get-you-banned)
 I have no affiliation with the Dark-Souls-III-Cheat-Engine project and cannot confirm this information.
 
 

### * The best precaution against being banned while using this application is to run Steam in offline mode, and to also run Dark Souls III in Offline Mode (make sure to fully restart the game after changing this setting!).
  - While developing I just run Dark Souls III in Offline Mode, and I have not experienced any issues so far.
  - You should also be safe to re-enable Online Mode again whenever you are playing the game without running Dark-Holes.

---

__I do not take any responsibility for anything that may occur to your save files, ban status, account, etc., that occurs as a result of using this application.__

---

# Instructions
TODO: More detailed instructions for how to run/operate etc.

It is necessary to run the application with admin privileges, this includes your IDE while developing.

You must to be running a Buttplug.io server for the application to connect to.
  - Currently the best option for this is https://intiface.com/central/.
  
Currently the game only has integration for Dark Souls III.
  - Adding support for other games requires adding logic to read through the specific memory portions where the relevant game attributes for haptic feedback (e.g. max health & current health for calculating health loss) are stored.
  - I've done this for Dark Souls III by reverse-engineering a Cheat Engine cheat-table for it, which is a do-able (albeit tricky) approach.

In its current state the application requires you to start up Dark Souls III first, have your Buttplug.io server running for the application to connect to, and to preferably (required?) connect your haptic feedback device to Buttplug.io first, and then you can run the application.

Look at https://buttplug.io/ for more info on Buttplug.io's capabilities, how to setup Intiface, which devices support the Buttplug.io protocol, etc.

# Planned features:
- Having connected devices respond to specific events in the game such as:
    - Loss of health (currently implemeted)
    - Character death (also implemented)
    - Attacks made by the player
        - (This seems harder to actually implement but will probably be a more enjoyable experience for the user. One problem with the health loss feature is that skilled players end up experiencing very little physical feedback since they don't get hit very often.)
    - Responsive to sound levels in the game?
        - (This somewhat avoids the above issue with the health loss feature, but this might also be somewhat lacking since things like game music, less notable actions like running/using items, etc., also cause the intensity to increase. The amount of noise added somewhat lessens the feeling of the game events and the haptic feedback being linked.)
    - Others?

- UI features for:
    - Selecting between/multiple connected Buttplug.io devices
    - Configuring server connection details
    - Selecting which game the application will hook into
    - Selecting which event source (from above) the intensity should be based on
    - Fine-tuning the relationship between the game events and how exactly the device responds, such as:
        - Changing the max intensity level
        - Adding a baseline floor to make the device never stop running
        - Setting how long and intense the device will run based on the intensity of an event
        - Adding random variation and noise if desired
        - Having fade-ins and fade-outs for the intensity
        - Inverting the event-feedback linkage (i.e. making the device always run at high until you get hit)
        - ...
    - Showing the user a disclaimer about the potential risks of using this application (see above).

- Automatic device re-connect
