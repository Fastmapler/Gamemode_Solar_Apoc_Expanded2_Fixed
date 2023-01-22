// server.cs
// support for floating bricks

// check if RTB as an Add-On exists
if(isFile("Add-Ons/System_ReturnToBlockland/server.cs"))
{
	// execute the pref handler file if it was disabled for some reason (?)
	if(!$RTB::Hooks::ServerControl)
		exec("Add-Ons/System_ReturnToBlockland/hooks/serverControl.cs");

	// register the prefs for the RTB menu
	RTB_registerPref("Enable Floating Bricks?", "Floating Bricks", "Pref::FloatingBricks::Enabled", "bool", "Server_Floating_Bricks", 0, 0, 0);
	RTB_registerPref("Admin Only?", "Floating Bricks", "Pref::FloatingBricks::AdminOnly", "bool", "Server_Floating_Bricks", 0, 0, 0);
	RTB_registerPref("Floating Brick Plant Timeout", "Floating Bricks", "Pref::FloatingBricks::Timeout", "int 0 60", "Server_Floating_Bricks", 1, 0, 0);
}
else
{
	// RTB doesn't exist, assign default values
	// if you don't have RTB, change these in console and they will save automatically upon exit
	$Pref::FloatingBricks::Enabled = false;
	$Pref::FloatingBricks::AdminOnly = false;
	$Pref::FloatingBricks::Timeout = 1;
}

// audio files
datablock AudioProfile(FloatingPlantSound)
{
	filename = "./sounds/floatPlant.wav";
	description = AudioClose3d;
	preload = true;
};

datablock AudioProfile(FloatingPlantErrorSound)
{
	filename = "./sounds/floatError.wav";
	description = AudioClose3d;
	preload = true;
};

// (modified) checkPlantingError from jes00 (thanks)
// @param fxDTSBrick %this   :   the brick to check
function fxDTSBrick::checkPlantingError(%this)
{
	// make sure brick isn't actually a real brick
	if(%this.isPlanted())
	{
		error("You must use a temp brick. Not a brick!");
		return -1;
	}

	// make a new "brick" to check the error that it returns on plant
	%brick = new fxDTSBrick()
	{
		datablock = %this.getDatablock();
		position = %this.getPosition();
		rotation = %this.rotation;
		angleID = %this.angleID;
	};

	// plant and delete the brick to check the error
	%error = %brick.plant();
	%brick.delete();
	
	// set the error
	switch$(%error)
	{
		case 0: 
			%type = 0;
			
		case 1: 
			%type = "overlap";
			
		case 2: 
			%type = "float";
			
		case 3: 
			%type = "stuck";
			
		case 4: 
			%type = "unstable";
			
		case 5: 
			%type = "buried";
			
		default: 
			%type = "forbidden";
	}
	
	return %type;
}

// core package
package FloatingBricks
{
	// @param GameConnection %this   :   the client using the command 
	function serverCmdPlantBrick(%this)
	{
		%r = Parent::serverCmdPlantBrick(%this);
		
		// check if floating bricks are enabled
		// check if player exists
		// check if player's ghost brick exists
		// check if the brick error is float
		if(!isObject(%player = %this.player) || !isObject(%ghost = %player.tempBrick) || %ghost.checkPlantingError() !$= "float" || (!$Pref::FloatingBricks::Enabled && !%ghost.getDatablock().isGridless))
			return %r;

		%admin = %this.isAdmin;
		
		// check for admin pref & admin status
		if($Pref::FloatingBricks::AdminOnly && !%admin)
		{
			%ghost.playSound("FloatingPlantErrorSound");
			messageClient(%this, '', "\c0You are not an \c3Admin\c0.");
			
			return %r;
		}
		
		// check for timeout (admins are exempt from timeout restriction)
		if(!%admin && $Pref::FloatingBricks::Timeout > 0)
		{
			// convert seconds to milliseconds
			%milliseconds = $Pref::FloatingBricks::Timeout * 1000;
			
			if(getSimTime() - %player.floatingBrickTime < %milliseconds)
			{
				messageClient(%this, 'MsgPlantError_Flood');
				
				// convert time remaining to seconds
				%timeout = mCeil((%milliseconds - (getSimTime() - %player.floatingBrickTime)) / 1000);
				%timeout = %timeout @ "\c3" @ (%timeout > 1 ? " seconds" : " second");	
				
				%ghost.playSound("FloatingPlantErrorSound");
				commandtoClient(%this, 'CenterPrint', "\c0You must wait \c3" @ %timeout @ "\c0 before planting a floating brick again.", 3);
				
				return %r;
			}
		}

		%brick = new fxDTSBrick()
		{
			client = %this;
			position = %ghost.getPosition();
			dataBlock = %ghost.getDatablock().getName();
			angleID = %ghost.angleID;
			rotation = %ghost.rotation;
			printID = %ghost.printID;
			colorID = %ghost.colorID;
			colorFX = 0;
			shapeFX = 0;
			isPlanted = true;
			isFloatingBrick = true;
		};
		
		// add the brick to the player's brickgroup
		%group = nameToID("BrickGroup_" @ %this.bl_id);
		%group.add(%brick);
		
		%brick.plant();
		%brick.playSound("FloatingPlantSound");
		
		// not sure if this is required
		%brick.setTrusted(true);
		
		// add the brick to their undo stack and set their timeout
		%this.undoStack.push(%brick TAB "PLANT");
		%player.floatingBrickTime = getSimTime();

		return %brick;
	}
	
	// @param fxDTSBrickData %this   :   the brick datablock that was planted
	// @param fxDTSBrick %brick   :   the brick object that was planted
	function fxDTSBrickData::onPlant(%this, %brick)
	{
		Parent::onPlant(%this, %brick);
		
		if(%brick.isFloatingBrick)
			%brick.isBaseplate = true;
	}
	
	// brick loading fix by zapk
	// @param fxDTSBrick %this   :   the brick to plant (called before onLoadPlant)
	function fxDTSBrick::plant(%this)
	{
		%this.forceBaseplate = %this.isBaseplate;
		
		return Parent::plant(%this);
	}

	// brick loading fix by zapk
	// @param fxDTSBrick %this   :   the brick that was loaded
	function fxDTSBrick::onLoadPlant(%this)
	{
		if(%this.forceBaseplate)
			%this.isBaseplate = true;

		Parent::onLoadPlant(%this);
	}
	
	// brick loading fix by zapk
	function serverLoadSaveFile_End()
	{	
		// loop through all of the brick groups
		for(%i = 0; %i < MainBrickGroup.getCount(); %i++)
		{
			%group = MainBrickGroup.getObject(%i);

			// loop through all of the bricks in that group
			for(%j = 0; %j < %group.getCount(); %j++)
			{
				%brick = %group.getObject(%j);

				// check if brick is floating
				if(!%brick.hasPathToGround() && %brick.getNumDownBricks() == 0)
				{
					%brick.isFloatingBrick = true;
					%brick.isBaseplate = true;

					%brick.playSound("FloatingPlantSound");
					
					// fixes a strange bug
					if(%brick.getNumUpBricks() != 0)
					{
						%brick.onToolBreak(); // wtf
					}
				}
			}
		}

		//public brick group
		%brickgroup = brickGroup_888888.getid();
		%count = %brickgroup.getCount();
		for(%i = 0; %i < %count; %i++)
		{
			%brick = %brickgroup.getobject(%i);
			if(%brick.isFloatingBrick)
			{
				floatingBrickFixPlant(%brick);
			}
		}
		
		return Parent::serverLoadSaveFile_End();
	}
	function SimGroup::getTrustFailureMessage(%group)
	{
		%parent = %group.getGroup ();
		if (!isObject (%parent))
		{
			%msg = "ERROR: SimGroup::getTrustFailureMessage(" @ %group.getName () @ " [" @ %group.getId () @ "]) - brickgroup is not in a parent group";
			error (%msg);
			return %msg;
		}
		if (%parent != mainBrickGroup.getId ())
		{
			%msg = "ERROR: SimGroup::getTrustFailureMessage(" @ %group.getName () @ " [" @ %group.getId () @ "]) - brickgroup is not in the main brick group";
			error (%msg);
			return %msg;
		}
		if (%group.bl_id $= "")
		{
			%msg = "ERROR: SimGroup::getTrustFailureMessage(" @ %group.getName () @ " [" @ %group.getId () @ "]) - brickgroup has no bl_id";
			error (%msg);
			return %msg;
		}
		if (%group.bl_id == 888888)
		{
			//return "You cannot modify public bricks";
			return "";
		}
		else 
		{
			if (%group.name $= "")
			{
				%group.name = "\c1BL_ID: " @ %group.bl_id @ "\c1\c0";
			}
			%msg = %group.name @ " does not trust you enough to do that.";
			return %msg;
		}
	}
};
activatePackage("FloatingBricks");

function floatingBrickFixPlant(%brick)
{
	%datablock = %brick.getDatablock();
	%position = %brick.getPosition();
	%angleID = %brick.angleID;
	%rotation = %brick.rotation;
	%printID = %brick.printID;
	%colorID = %brick.colorID;
	
	%brick.delete();

	%brick = new fxDTSBrick()
	{
		position = %position;
		dataBlock = %datablock.getName();
		angleID = %angleID;
		rotation = %rotation;
		printID = %printID;
		colorID = %colorID;
		colorFX = 0;
		shapeFX = 0;
		isPlanted = true;
		isFloatingBrick = true;
		isBaseplate = true;
	};
	
	// add the brick to the player's brickgroup
	%group = nameToID("BrickGroup_888888");
	%group.add(%brick);
	
	%brick.plant();
	%brick.playSound("FloatingPlantSound");
	
	// not sure if this is required
	%brick.setTrusted(true);
}