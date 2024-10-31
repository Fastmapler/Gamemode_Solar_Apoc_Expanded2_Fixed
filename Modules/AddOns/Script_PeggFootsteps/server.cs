//	server.cs
//-----------------------------------------------------------------------------------------------------------
// Title:			PeggyFootsteps
// Author: 			Peggworth the Pirate
// Description:		Footsteps that change noise based on what you touch below you.
//
//				A set of footsteps ripped from an old diablo-esque game, from 'FreeSound.org', and other Footstep Add-Ons.
//				We shall call them peggsteps and they will be the best.
//-----------------------------------------------------------------------------------------------------------
//	Version:
//				3.1.3:	Fixed endless schedules (thanks Queuenard!)
//				3.1.2:	Revamped Preferences for BLG. Heavy/Medium Landings kill your ears less. Fixed minor issues. [6/5/2018]
//				3.1.1B:	Bug fix, Standing on a ramp no longer continuously plays SoundFX [4/18/2018]
//        		3.1.1A: RTB / Glass Prefs fixed (for real) [11/3/2017]
//				3.1.1:  RTB / Glass Prefs fixed [11/2/2017]
//				3.1.0:  New more efficient method of detecting movement. Landing sound effects. Speed detection now scales to the size of the player, so being small won't make you constantly play the walking sound. Several bug fixes. Running sound effects are now AudioClose3d instead of AudioClosest3d for walking (making them heard from greater distances). [3/17/2017]
//				3.0.0:	BrickFX now makes custom sounds via prefs, Customize specific brick soundFX with events, Enhanced Server Prefs, Customize non-brick soundFX, New 'basic' footsteps (Ripped from Hata's "Support_Footsteps") [5/26/2016]
//				2.3.4:	Fixed error where certain commands would tell you to use '/help' not '/pegghelp' [2/20/2016]
//        		2.3.3:	Replaced '/help' with '/pegghelp' [12/29/2015]
//        		2.3.2:	Fixed bug with '/clearcustomsound' where when a single sound was deleted, the entire list was no longer played. [12/27/2015]
//        		2.3.1:	Fixed 'sand' sound FX (these weren't playing when you added custom sounds). Added '/clearcustomsound' to remove a single SFX to the color you have selected. Edited command system again ('/get' is no longer a parametered command) [12/23/2015]
//        		2.3.0:	Revamped custom sound functions. Instead of being stored in one large string, custom souds are in an array now. Edited 'getHex' to 'rgbToHex' to fix the occasional hex error.
//        		2.2.1:	Fixed Small Bugs	[12/12/2015]
//				2.2.0:	Edited command system, Sand footstep soundFX, Removed use of 'eval', New Pref, New Command	[12/6/2015]
//				2.1.0:	Metal and Snow footstep soundFX, Custom soundFX, Bug fix	[10/03/2015]
//				2.0.0:	Swimming, RTB Prefs, Released to public		[09/01/2015]
//				1.1.0:	All sounds replaced with new ones, no new functionality
//				1.0.0:	Footsteps adapt to color of ground stepped on
//-----------------------------------------------------------------------------------------------------------
//  Special Thanks:
//  	Thanks to Greek2Me for the 'rgbToHex' function
//		Thanks to Hata and Port for their footstep mods because I stole sound effects from each of theirs, as well as for releasing footstep add-ons that I learned from
//		Thanks to those of you who downloaded my Add-On and actually liked it, because you've made me actually want to keep developing it
//-----------------------------------------------------------------------------------------------------------


//--------------------------------------------------------------------------------------
// 		Server Preference Configuration:
//--------------------------------------------------------------------------------------

//+++ Register preferences:

//++  If BLG is available:
if ( isFile("Add-Ons/System_BlocklandGlass/server.cs") )
{
	if ( ! isObject(Glass) && ! $PFGlassInit )
	{
		exec ("Add-Ons/System_BlocklandGlass/server.cs");
	}
	if ( ! $PFGlassInit )
	{
		exec("./BLGPrefs.cs");
	}
}
//++ If RTB all that's left:
else if( isFile("Add-Ons/System_ReturnToBlockland/server.cs") )
{
	if( !$RTB::RTBR_ServerControl_Hook && isFile("Add-Ons/System_ReturnToBlockland/server.cs") )
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	if ( ! $PFRTBInit)
	{
		RTB_RegisterPref("Enable Footstep SoundFX", "Peggy Footsteps", "$Pref::Server::PF::footstepsEnabled", "bool", "Script_PeggFootsteps", 1, 0, 0);
		RTB_RegisterPref("BrickFX custom SoundFX", "Peggy Footsteps", "$Pref::Server::PF::brickFXSounds::enabled", "bool", "Script_PeggFootsteps", 1, 0, 0);
		RTB_RegisterPref("Enable Landing SoundFX", "Peggy Footsteps", "$Pref::Server::PF::landingFX", "bool", "Script_PeggFootsteps", 1, 0, 0);
	  	RTB_RegisterPref("Enable Swimming SoundFX", "Peggy Footsteps", "$Pref::Server::PF::waterSFX", "bool", "Script_PeggFootsteps", 1, 0, 0);
		RTB_RegisterPref("Landing Threshold", "Peggy Footsteps", "$Pref::Server::PF::minLandSpeed", "int 0.0 20.0", "Script_PeggFootsteps", 10.0, 0, 0);
		RTB_RegisterPref("Running Threshold", "Peggy Footsteps", "$Pref::Server::PF::runningMinSpeed", "int 0.0 20.0", "Script_PeggFootsteps", 2.8, 0, 0);
		RTB_RegisterPref("Default Step SoundFX", "Peggy Footsteps", "$Pref::Server::PF::defaultStep", "List	Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Script_PeggFootsteps", 1, 0, 0);
		RTB_RegisterPref("Steps on Terrain SoundFX", "Peggy Footsteps", "$Pref::Server::PF::terrainStep", "List	Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Script_PeggFootsteps", 0, 0, 0);
		RTB_RegisterPref("Steps on Vehicles SoundFX", "Peggy Footsteps", "$Pref::Server::PF::vehicleStep", "List	Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9", "Script_PeggFootsteps", 0, 0, 0);

		$PFRTBInit = true;
	}

	if ( $Pref::Server::PF::brickFXsounds::pearlStep $= "" ) $Pref::Server::PF::pearlStep = 4;
	if ( $Pref::Server::PF::brickFXsounds::chromeStep $= "" ) $Pref::Server::PF::chromeStep = 4;
	if ( $Pref::Server::PF::brickFXsounds::glowStep $= "" ) $Pref::Server::PF::brickFXsounds::glowStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::blinkStep $= "" ) $Pref::Server::PF::brickFXsounds::blinkStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::swirlStep $= "" ) $Pref::Server::PF::brickFXsounds::swirlStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::rainbowStep $= "" ) $Pref::Server::PF::brickFXsounds::rainbowStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::unduloStep $= "" ) $Pref::Server::PF::unduloStep = 8;
}
//++ If neither is available:
else
{
	if ( $Pref::Server::PF::footstepsEnabled $= "" ) $Pref::Server::PF::footstepsEnabled = 1;
	if ( $Pref::Server::PF::brickFXSounds::enabled $= "" ) $Pref::Server::PF::brickFXSounds::enabled = 1;
	if ( $Pref::Server::PF::brickFXSounds::enabled $= "" ) $Pref::Server::PF::landingFX = 1;
	if ( $Pref::Server::PF::minLandSpeed $= "" ) $Pref::Server::PF::minLandSpeed = 8.0;
	if ( $Pref::Server::PF::runningMinSpeed $= "" ) $Pref::Server::PF::runningMinSpeed = 2.8;
	if ( $Pref::Server::PF::waterSFX $= "" ) $Pref::Server::PF::waterSFX = 1;
	if ( $Pref::Server::PF::defaultStep $= "" ) $Pref::Server::PF::defaultStep = 1;
	if ( $Pref::Server::PF::terrainStep $= "" ) $Pref::Server::PF::terrainStep = 0;
	if ( $Pref::Server::PF::vehicleStep $= "" ) $Pref::Server::PF::vehicleStep = 0;

	if ( $Pref::Server::PF::brickFXsounds::pearlStep $= "" ) $Pref::Server::PF::pearlStep = 4;
	if ( $Pref::Server::PF::brickFXsounds::chromeStep $= "" ) $Pref::Server::PF::chromeStep = 4;
	if ( $Pref::Server::PF::brickFXsounds::glowStep $= "" ) $Pref::Server::PF::brickFXsounds::glowStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::blinkStep $= "" ) $Pref::Server::PF::brickFXsounds::blinkStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::swirlStep $= "" ) $Pref::Server::PF::brickFXsounds::swirlStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::rainbowStep $= "" ) $Pref::Server::PF::brickFXsounds::rainbowStep = 0;
	if ( $Pref::Server::PF::brickFXsounds::unduloStep $= "" ) $Pref::Server::PF::unduloStep = 8;
}

//+++ Execute everything
exec("./Sounds/sounds.cs");
exec("./PeggyFootsteps.cs");
exec("./commands.cs");
