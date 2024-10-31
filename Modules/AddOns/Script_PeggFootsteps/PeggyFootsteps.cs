//	PeggyFootsteps.cs
//--------------------------------------------------------------------------------------
// Title:			PeggyFootsteps
// Author: 			Peggworth the Pirate
// Description:		Footsteps that change noise based on what you touch below you.
//--------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------
//		Misc. Functions:
//--------------------------------------------------------------------------------------

function rgbToHex(%rgb) //! Ripped from Greek2Me's SLAYER
{
	// use % to find remainder
	%r = getWord(%rgb,0);
	%g = getWord(%rgb,1);
	%b = getWord(%rgb,2);
	// in-case the rgb value isn't on the right scale
	%r = ( %r <= 1 ) ? %r * 255 : %r;
	%g = ( %g <= 1 ) ? %g * 255 : %g;
	%b = ( %b <= 1 ) ? %b * 255 : %b;
	// the hexidecimal numbers
	%a = "0123456789ABCDEF";

	%r = getSubStr(%a,(%r-(%r % 16))/16,1) @ getSubStr(%a,(%r % 16),1);
	%g = getSubStr(%a,(%g-(%g % 16))/16,1) @ getSubStr(%a,(%g % 16),1);
	%b = getSubStr(%a,(%b-(%b % 16))/16,1) @ getSubStr(%a,(%b % 16),1);

	return %r @ %g @ %b;
}


//--------------------------------------------------------------------------------------
//		Footstep Deciders:
//--------------------------------------------------------------------------------------

//+++	Return the sound datablock based off the surface.
function PeggFootsteps_getSound(%surface, %speed)
{
		//echo(%surface);
		switch$ ( %surface )
		{
			// swimmingfx only has one speed. There is no walking speed for swimmingfx.
			case "under water":
				return $StepSwimming[getRandom(1,6)];

			case "metal":
				if ( %speed $= "walking" )
					return $StepMetalW[getRandom(1,3)];
				else
					return $StepMetalR[getRandom(1,3)];

			case "dirt":
				if ( %speed $= "walking" )
					return $StepDirtW[getRandom(1,4)];
				else
					return $StepDirtR[getRandom(1,6)];

			case "grass":
				if ( %speed $= "walking" )
					return $StepGrassW[getRandom(1,4)];
				else
					return $StepGrassR[getRandom(1,6)];

			case "stone":
				if ( %speed $= "walking" )
					return $StepStoneW[getRandom(1,4)];
				else
					return $StepStoneR[getRandom(1,6)];

			case "water":
				if ( %speed $= "walking" )
					return $StepWaterW[getRandom(1,4)];
				else
					return $StepWaterR[getRandom(1,6)];

			case "wood":
				if ( %speed $= "walking" )
					return $StepWoodW[getRandom(1,4)];
				else
					return $StepWoodR[getRandom(1,6)];

			case "sand":
				if ( %speed $= "walking" )
					return $StepSandW[getRandom(1,4)];
				else
					return $StepSandR[getRandom(1,6)];

			case "basic":
				if ( %speed $= "walking" )
					return $StepBasicW[getRandom(1,4)];
				else
					return $StepBasicR[getRandom(1,4)];

			// snowsteps only have one speed. There is no walking sound effect for snowsteps
			case "snow":
				return $StepSnowR[getRandom(1,3)];

			case "default":
				return PeggFootsteps_getSound(parseSoundFromNumber($Pref::Server::PF::defaultStep, 0), %speed);

			default:
				if ( %speed $= "walking" )
					return $StepBasicW[getRandom(1,4)];
				else
					return $StepBasicR[getRandom(1,4)];
		}
}

//+++ 	Calculate the noise based off what the player is stepping on.
function checkPlayback(%obj)
{
	%surface = ( %obj.touchcolor $= "" ) ? %obj.surface : %obj.touchColor;
	%speed = ( %obj.isSlow == 0 ) ? "running" : "walking";

	if ( %obj.touchColor $= "" )
	{
		return PeggFootsteps_getSound(%surface, %speed);
	}
	else
	{
		%r = getWord(%surface, 0) * 255;
		%g = getWord(%surface, 1) * 255;
		%b = getWord(%surface, 2) * 255;

		for ( %i = 0; %i < $Pref::Server::PF::customsounds; %i++ )
		{
			%hit = $Pref::Server::PF::colorPlaysFX[%i];
			%hitcolor = getWords(%hit, 0, 3);
			%hr = getWord(%hitcolor, 0) * 255;
			%hg = getWord(%hitcolor, 1) * 255;
			%hb = getWord(%hitcolor, 2) * 255;
			if ( %hr == %r && %hg == %g && %hb == %b)
			{
				%hitsound = getWord(%hit, 4);
				return PeggFootsteps_getSound(%hitsound, %speed);
			}
		}

		if (%r >= %b && %b >= %g || (%r > 180 && %g > 180))
		{
			%surface = "dirt";
		}
		else if (%r >= %g && %g > %b)
		{
			%surface = "wood";
		}
		else if (%g >= %b && %g > %r)
		{
			%surface = "grass";
		}
		else
		{
			%surface = "stone";
		}
		if (%r == %b && %r == %g && %b == %g)
		{
			%surface = "stone";
			if(%r > 230 && %g > 230 && %b > 230)
			{
				%surface = "snow";
			}
		}
	}
	if ( %obj.isSwimming )		// splashing in water, not under the water
	{
		%surface = "water";
	}
	return PeggFootsteps_getSound(%surface, %speed);
}

//+++ Return the surface's name when given a number from a list.
function parseSoundFromNumber(%val, %obj) // brick is an optional parameter
{
	if ( !$Pref::Server::PF::brickFXSounds::enabled ) return "default";
	//Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9
	switch ( %val )
	{
		case 0:
			if ( %obj.touchColor !$= "" ) return "color";
			else return "default";
		case 1:
			return "basic";
		case 2:
			return "dirt";
		case 3:
			return "grass";
		case 4:
			return "metal";
		case 5:
			return "sand";
		case 6:
			return "snow";
		case 7:
			return "stone";
		case 8:
			return "water";
		case 9:
			return "wood";
	}
}

//+++ When any lifeform spawns, create a footstep loop.
deactivatepackage(peggsteps);
package peggsteps
{
	function Armor::onNewDatablock(%this, %obj)
	{
		cancel(%obj.peggstep);
		// creatures like horses won't make footsteps, but other AI will
		if ( %this.rideable )	return parent::onNewDatablock(%this, %obj);
		%obj.touchcolor = "";
		%obj.surface = parseSoundFromNumber($Pref::Server::PF::defaultStep, %obj);
		%obj.isSlow = 0;
		%obj.peggstep = schedule(50,0,PeggFootsteps,%obj);
		return parent::onNewDatablock(%this, %obj);
	}

	// I don't actually know how this built-in function works, but I know this will work.
	function onMissionEnded(%this, %a, %b, %c, %d)
	{
		$PFGlassInit = false;
		$PFRTBInit = false;
		return parent::onMissionEnded(%this, %a, %b, %c, %d);
	}
};
activatepackage(peggsteps);


//--------------------------------------------------------------------------------------
//		Footstep Playback:
//--------------------------------------------------------------------------------------

//+++ Landing from a fall
function Armor::onLand(%data, %obj, %horiz)
{
	if ( !$Pref::Server::PF::landingFX ) return;
	// The default speed at which to play a landing sound is 8.
	// The current pref for speed + the default * 2 is when we play the heavy landing sound
	// and current + default * 1 is when we play the medium landing sound, regardless of
	// whatever the user's pref for landing speed is.

	if ( %horiz > $Pref::Server::PF::minLandSpeed + 16 ) // heavy landing sound
	{
		serverplay3d(LandHeavy_Sound, %obj.getHackPosition());
	}
	else if ( %horiz > $Pref::Server::PF::minLandSpeed + 8 ) // medium landing sound
	{
		serverplay3d($LandMedium[getRandom(1,3)], %obj.getHackPosition());
	}
	else if ( %horiz >= $Pref::Server::PF::minLandSpeed ) // lite landing sound
	{
		serverplay3d($LandLite[getRandom(1,3)], %obj.getHackPosition());
	}
}

//+++ Drop some rad peggstep noise in here!
function PeggFootsteps(%obj, %lastVert)
{
	cancel(%obj.peggstep);
	if($Pref::Server::PF::footstepsEnabled == 1 && isObject(%obj))
	{
		if ( %obj.isMounted() )
		{
			%obj.peggstep = schedule(50,0,PeggFootsteps,%obj);
			return;
		}
		//! Ripped from Hata's support_footstep.cs
		%vel = %obj.getVelocity();
		%vert = getWord(%vel, 2);
		%horiz = vectorLen(setWord(%vel, 2, 0));

		%pos = %obj.getPosition();
		initContainerBoxSearch(%pos, "1.25 1.25 0.1", $TypeMasks::fxBrickObjectType | $Typemasks::TerrainObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::FxPlaneObjectType);
		%col = containerSearchNext();
		//	echo(%type);
		if( isObject(%col) )
		{
			%type = %col.getClassName();
			if ( %type $= "fxDTSbrick" && %col.isRendering() )
			{
					%obj.lastBrick =  %col;
					// by default, the surface isn't decided yet, and will be decided by the color
					%obj.touchColor = getColorIDTable(%col.getColorId());
					%obj.surface = "";
					// check to see if there is a custom sound based on the brick's special FX
					if ( $Pref::Server::PF::brickFXSounds::enabled )
					{
						// if there's a color fx
						switch ( %col.getColorFxID() )
						{
							case 1:
								%obj.surface = parseSoundFromNumber($Pref::Server::PF::brickFXsounds::pearlStep, %obj);
								%obj.touchColor = "";
							case 2:
								%obj.surface = parseSoundFromNumber($Pref::Server::PF::brickFXsounds::chromeStep, %obj);
								%obj.touchColor = "";
							case 3:
								%obj.surface = parseSoundFromNumber($Pref::Server::PF::brickFXsounds::glowStep, %obj);
								%obj.touchColor = "";
							case 4:
								%obj.surface = parseSoundFromNumber($Pref::Server::PF::brickFXsounds::blinkStep, %obj);
								%obj.touchColor = "";
							case 5:
								%obj.surface = parseSoundFromNumber($Pref::Server::PF::brickFXsounds::swirlStep, %obj);
								%obj.touchColor = "";
							case 6:
								%obj.surface = parseSoundFromNumber($Pref::Server::PF::brickFXsounds::rainbowStep, %obj);
								%obj.touchColor = "";
						}
						// if there's a shape fx, which takes priority over color fx
						if ( %col.getShapeFxID() )
						{
							%obj.surface = parseSoundFromNumber($Pref::Server::PF::brickFXsounds::unduloStep, %obj);
							%obj.touchColor = "";
						}
						// if the preference for the shape or color fx is default, then just play the regular sound that would be made based on color
						if ( %obj.surface $= "color" )
						{
							%obj.touchColor = getColorIDTable(%col.getColorId());
						}
					}
					// check to see if the brick has an event based custom sound
					if ( %col.customStep !$= "" )
					{
						%obj.touchColor = "";
						%obj.surface = %col.customStep;
					}
			}
			else if ( %type $= "fxPlane" )
			{
				%obj.touchColor = "";
				%obj.surface = parseSoundFromNumber($Pref::Server::PF::terrainStep, %obj);
			}
			else if ( %type $= "WheeledVehicle" || %type $= "FlyingVehicle" )
			{
				%obj.touchColor = "";
				%obj.surface = parseSoundFromNumber($Pref::Server::PF::vehicleStep, %obj);
			}
			else
			{
				%obj.touchColor = "";
				%obj.surface = parseSoundFromNumber($Pref::Server::PF::defaultStep, %obj);
			}
			if ( %obj.getWaterCoverage() > 0 )
			{
				%obj.surface = "water";
				%obj.touchColor = "";
			}
			if ( !%isGround && mAbs(%lastVert) > $Pref::Server::PF::minLandSpeed * getWord(%obj.getScale(), 1) && $Pref::Server::PF::landingFX )
			{
				%obj.getDatablock().onLand(%obj, mAbs(%lastVert));
			}
			%isGround = true;
		}
		else
		{
			%isGround = false;
		}

		%obj.isSlow = ( mAbs(%horiz) < $Pref::Server::PF::runningMinSpeed * getWord(%obj.getScale(), 0) || %obj.isCrouched() );

		if( %obj.getWaterCoverage() > 0 && $Pref::Server::PF::waterSFX == 1 && mAbs(%horiz) > 0.1 && !%isGround )
		{
			%obj.touchColor = "";
			%obj.surface = "under water";
			serverplay3d(checkPlayback(%obj), %obj.getHackPosition());
			%obj.peggstep = schedule(500 * getWord(%obj.getScale(), 0), 0, PeggFootsteps, %obj);
		}
		else if( mFloor(%horiz) == 0 || !%isGround )
		{
			%obj.peggstep = schedule(50, 0, PeggFootsteps, %obj, %vert);
		}
		else if ( %isGround && mAbs(%horiz) > 0 )
		{
			%obj.peggstep = schedule(320 * getWord(%obj.getScale(), 0), 0, PeggFootsteps, %obj, %vert);
			serverplay3d(checkPlayback(%obj), %obj.getHackPosition());
		}

		// Thanks Queuenard!
		%obj.peggstep = schedule(1000, 0, PeggFootsteps, %obj);
		return;
	}
}


//--------------------------------------------------------------------------------------
//		 Assigning custom sounds to specific bricks with events.
//--------------------------------------------------------------------------------------

registerOutputEvent("fxDTSBrick","setFootstep","List Clear -1 Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9");

function fxDTSBrick::setFootstep(%brick, %val, %client)
{
	if ( %val == -1 )
	{
		%brick.customStep = "";
	}
	else
	{
		%brick.customStep = parseSoundFromNumber(%val, %client.player);
		//echo(%brick.customStep);
	}
}
