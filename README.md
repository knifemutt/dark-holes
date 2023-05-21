# dark-holes
Desktop application to add Buttplug.io-integrated haptic feedback to Dark Souls games

# Instructions
TODO: More detailed instructions for how to run/operate etc.
It is necessary to run the application with admin privileges, this includes your IDE while developing.
You must to be running a Buttplug.io server for the application to connect to.
  - Currently the best option for this is https://intiface.com/central/.
Currently the game only has integration for Dark Souls III.
  - Adding support for other games requires adding logic to read through the specific memory portions where the relevant game attributes for haptic feedback (e.g. max health & current health for calculating health loss) are stored.
  - I've done this for Dark Souls III by reverse-engineering a Cheat Engine cheat-table for it, which is a do-able (albeit tricky) approach.

In its current state the application requires you to start up Dark Souls III first, have your Buttplug.io server running for the application to connect to, and to preferably connect your haptic feedback device to Buttplug.io first, and then you can run the application.

Look at https://buttplug.io/ for more info on Buttplug.io's capabilities, how to setup Intiface, which devices support the Buttplug.io protocol, etc.
