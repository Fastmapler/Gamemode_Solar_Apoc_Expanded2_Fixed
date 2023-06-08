//support.cs includes utility functions, random datablocks, useful functions

//---------------------------------------------------------------------------------//
//utilities : most functions here are called multiple times and have different uses//
//---------------------------------------------------------------------------------//

function isHoleDebug()
{
	return $holeDebugMode;
}

function setHoleDebug( %enable )
{
	if( %enable )
		$holeDebugMode = 1;
	else
		$holeDebugMode = 0;
}

// debug helper function to find out why we're calling that function
// simply echos out the function we're calling and why we're calling it
function debugHoles( %reason )
{
	if( !isHoleDebug() )
		return;
		
	echo( %reason );
	backtrace();
}

// function AIPlayer::hSetMoveTolerance( %obj, %val )
// {
	// %obj.setMoveTolerance( %val );
	// %obj.hMoveTolerance = %val;
// }

// function AIPlayer::hGetMoveTolerance( %obj )
// {
	// return %obj.hMoveTolerance;
// }

function AIPlayer::hGetTickRate( %obj )
{
	%tickRate = %obj.getDataBlock().hTickRate;
	
	if(!%tickRate)
		%tickRate = 3000;
		
	return %tickrate;
}

function hGetZ( %pos )
{
	%z = getWord( %pos, 2 );
	
	return %z;
}

function fxDTSBricK::hGetPosZ( %obj )
{
	%pos = %obj.getPosition();
	
	%z = getWord( %pos, 2 );
	
	return %z;
}

function ShapeBase::hGetPosZ( %obj )
{
	%pos = %obj.getPosition();
	
	%z = getWord( %pos, 2 );
	
	return %z;
}

// called on vehicle, any vehicle
function ShapeBase::ejectRandomPlayer( %obj )
{
	if( !isObject( %obj ) )
		return 0;
		
	%mCount = %obj.getMountedObjectCount();
	
	if( !%mCount )
		return 0;
	
	%rand = getRandom( 0, %mCount-1 );
	
	%mObj = %obj.getMountedObject( %rand );
	
	%mObj.dismount();
	
	return %mObj;
}

// functions to make a screenshot of your bot
// paint bot all black? all gray?
// position above the zero point
// freeze in air by mounting to static shape
// empty static shape used by bot icon function
datablock StaticShapeData( emptyBotHolderShape )
{
	shapefile = "base/data/shapes/empty.dts";
};

function serverCmdDoBotIcon( %client, %data )
{
	if( !%client.isSuperAdmin )
		return;
		
	// make sure the dataBlock exists
	if( !isObject( %data ) )
	{
		error( "Couldn't find" SPC %data );
		return 0;
	}
	
	// clear old bot icon brick
	if( isObject( $botIconBrick ) )
	{
		$botIconBrick.botHolder.delete();
      $botIconBrick.botHolder = 0;
		$botIconBrick.hBot.delete();
      $botIconBrick.hBot = 0;
		$botIconBrick.delete();
      $botIconBrick = 0;
	}
	
	%pos = "0 10 -1005";
	
	// should probably take into consideration the rotate adjust thing
	%rot = "0 0 -1 90";
	
	// create the bot brick
	%brick = new fxDTSBrick()
	{
		position  = %pos;
		rotation  = %rot;
		dataBlock = %data;
		angleId   = 1;
		colorId   = 5;
		colorFxId = 0;
		shapeFxId = 0;
		isPlanted = 1;
		client    = %client;
	};
	
	// remember the brick we made
	$botIconBrick = %brick;
	
	%error = %brick.plant();
	%brick.setTrusted(1);
	%client.brickGroup.add( %brick );
	
	// make sure the brick is rendering
	%brick.scheduleNoQuota( 1000, setRendering, 1 );
	
	// set hBot type since we're doing this out of onplant
	%brick.hBotType = %data.holeBot;
	
	// create the static shape that will hold the bot in place, honestly I'm unsure why there are no collision meshes in icon mode
	%static = new staticShape()
	{
		dataBlock = emptyBotHolderShape;
		position = vectorAdd( %pos, "0 0 0.225" );
		rotation = %rot;
	};
	
	missionCleanup.add( %static );
	
	%brick.botHolder = %static;
	
	// spawn the bot
	%bot = %brick.spawnHoleBot();
	
	%bot.stopHoleLoop();
	
	// paint the bot
	%bot.setTempColor( getColorIDTable( 5 ) );
	
	%static.mountObject( %bot, 0 );
		
	return 1;
}

// brick conversion functions, just to make code more readable
function brickToMetric( %number )
{
	%number = %number/2;
	return %number;
}

function metricToBrick( %number )
{
	%number = %number*2;
	return %number;
}

function brickToRadius( %number )
{
	%number = ( %number-5 )/2;
	
	if( %number <= 0 )
		%number = 0.25;
		
	return %number;
}

function radiusToBrick( %number )
{
	%number = %number*2+5;
		
	return %number;
}
// 1 = 6 brick units
// 1.5 in radiusSearch is equal to 8 units
// 2 = 10
// 5.5 = 16 units wtf
// 13.5 = 32
// 29.5 = 64

// 1.5 + 4 = 5.5; 5.5 + 8 = 13.5; 13.5 + 16 = 29.5; 29.5 + 32 = 61.5;

// 4 units is equal to 8 brick units

// 0.1875 final radius unit
// brick unit * 0.1875 = radius
// brick unit / 2 = normal distance
// function detectDestDist( %dist )
// {
	// %type = $TypeMasks::PlayerObjectType;
	
	// %pos = findClientByName( rot ).player.getPosition();
	
	// initContainerRadiusSearch( %pos, %dist, %type );
	// while( ( %target = containerSearchNext() ) != 0 )
	// {
		// echo( "Found" SPC %target );
		// echo( vectorDist( findClientByName( rot ).player.getPosition(), %target.getPosition() ) );
	// }
// }

// function fovTest( %bot, %player )
// {
	// %botEye = %bot.getEyeVector();
	// %botForward = %bot.getForwardVector();
	
	// %posBot = %bot.getPosition();
	// %posPlayer = %player.getPosition();
	
	// %line = vectorNormalize( vectorSub( %posPlayer, %posBot ) );
	
	// %dot = vectorDot( %botEye, %line );
	
	// echo( "bot" );
	// echo( "eye" SPC %botEye );
	// echo( "line" SPC %line );
	// echo( "DOT" SPC %dot );
// }

function Armor::onStuck( %this, %obj )
{
	return; //die
	// if( %obj.hLastStuckTime > getSimTime() || %obj.isDisabled()	)
		// return;
		
	// %obj.hLastStuckTime = getSimTime()+150;
	// if( %obj.isDisabled() )
		// return;
		
	// echo( "I'm stuck" );
	%obj.hAvoidObstacle( 0, 0, 1 );
	// %obj.hJump( 100 );
	// %obj.setJumping( 1 );
	// %obj.setJumping( 0 );
}

// called when target enters line of sight
function Armor::onTargetEnterLOS( %this, %obj )
{
	// echo( "target enter LOS" );
	%obj.hIsInLOS = 1;
	
	if( !%obj.getMountedImage(0).melee && %obj.hFollowing )
		%obj.scheduleNoQuota(getRandom(300,500),hShoot, 0, 1 );
}

function Armor::onTargetExitLOS( %this, %obj )
{
	// echo( "target leaving LOS" );
	%obj.hIsInLOS = 0;
	
	if( !%obj.getMountedImage(0).melee && %obj.hFollowing )
		%obj.hShoot( 0, 0 );
}

// called when target enters engage distance, aka attacking distance
function Armor::onTargetEngaged( %this, %obj )
{
	%obj.hIsEngaged = 1;
	
	// if we have a melee weapon start whacking away
	if( %obj.getMountedImage(0).melee && %obj.hFollowing )
		%obj.setImageTrigger( 0, 1 );
}

function Armor::onTargetDisEngaged( %this, %obj )
{
	%obj.hIsEngaged = 0;
	
	// if we have a melee weapon stop whacking away
	if( %obj.getMountedImage(0).melee && %obj.hFollowing )
		%obj.setImageTrigger( 0, 0 );
}


// empty custom but functions to prevent console warnings
function Armor::onBotLoop( %this, %obj )
{
	// echo( "Empty onBotLoop" );
	//Useful for doing unique behaviors during normal loop
}

function Armor::onBotCollision( %this, %obj, %col, %normal, %speed )
{
	// echo( "Empty onBotCollision" );
	//Called once every second the object is colliding with something
}

function Armor::onBotFollow( %this, %obj, %targ )
{
	// echo( "Empty onBotFollow" );
	//Called when the target follows a player each tick, or is running away
}

function Armor::onBotDamage( %this, %obj, %source, %pos, %damage, %type )
{
	// echo( "Empty onBotDamage" );
	// Called when the bot is being damaged
}

// these functions point to the correct custom bot function datablock
function AIPlayer::onBotLoop( %obj )
{
	//called every loop
	%obj.getDataBlock().onBotLoop( %obj );
}

function AIPlayer::onBotCollision( %obj, %col, %a, %b )
{
	//Called once every second the object is colliding with something
	%obj.getDataBlock().onBotCollision( %obj, %col, %a, %b );
}

function AIPlayer::onBotFollow( %obj, %targ )
{
	//Called when the target follows a player each tick, or is running away
	%obj.getDataBlock().onBotFollow( %obj, %targ );
}

function AIPlayer::onBotDamage( %obj, %source, %pos, %damage, %type )
{
	//Called when the bot is being damaged
	%obj.getDataBlock().onBotDamage( %obj, %source, %pos, %damage, %type );
}

// math functions
// returns a random vector
function hGetRandomVector( %min, %max, %neg )
{
	%x = hGetRandomFloat(%min, %max, %neg);
	%y = hGetRandomFloat(%min, %max, %neg);
	%z = hGetRandomFloat(%min, %max, %neg);
	
	%vector = %x SPC %y SPC %z;
	
	return %vector;
}

function getRandomDongFloat()
{
	%n = getRandom( 0, 1000 );
	%n = %n/1000;
	
	return %n;
}

// take a vector and remove anything below zero
function hClampVector( %vector )
{
	%x = getWord( %vector, 0 );
	%y = getWord( %vector, 1 );
	%z = getWord( %vector, 2 );
	
	// zero check
	if( %x < 0 )
		%x = 0;
	if( %y < 0 )
		%y = 0;
	if( %z < 0 )
		%z = 0;
		
	// beyond 1 check
	if( %x > 1 )
		%x = 1;
	if( %y > 1 )
		%y = 1;
	if( %z > 1 )
		%z = 1;
		
	return %x SPC %y SPC %z;
}

//returns a random float %min 0 to 10, %max 0 to 10, %neg if true will randomly make the value negative
function hGetRandomFloat(%min, %max, %neg)
{
	// for now take the min max and scale up by 100, need to change the function to only accept 0.0 to 1.0
	%min = %min*100;
	%max = %max*100;
	
	%n = getRandom( %min, %max );
	%n = %n/1000;
	
	// 50/50 for negative
	if( %neg && getRandom( 0, 1 ) )
		%n = %n-%n*2;
		
	return %n;
}

//Reset bots function called by many scripts
function hResetBots(%client)
{
	if( !isObject( %client ) )
		return;

	// check if we're being fed a client or a brickgroup
	if( %client.getClassName() $= "GameConnection" )
		%group = %client.brickGroup.hBotBrickSet;
	else if( %client.getClassName() $= "SimGroup" )
		%group = %client.hBotBrickSet;
	
	if(isObject(%group))
	{
		for(%a = %group.getCount()-1; %a >= 0; %a--)
		{
			%obj = %group.getObject(%a);

			cancel(%obj.hModS);
         %obj.hModS = 0;
			if(isObject(%obj.hBot))
			{
				%obj.hBot.delete();
            %obj.hBot = 0;
			}
         if(!%obj.hBotType.hManualSpawn)
   			%obj.spawnHoleBot();
		}
	}
}

//returns the id of the named brick, will randomly select one if multiple
function hReturnNamedBrick(%brickGroup,%name)
{
	%name = "_" @ %name;
	for(%a = 0; %a < %brickGroup.NTNameCount; %a++)
	{
		if(%brickGroup.NTName[%a] !$= %name || %brickGroup.NTObjectCount[%name] < 0)
			continue;

		%n = %brickGroup.NTObjectCount[%name];

		return %brickGroup.NTObject[%name,getRandom(0,%n-1)];
	}
	return 0;
	//echo("Couldn't find" SPC %name);
}

// deprecated as of r1808
//ghetto setAimVector function, does not do z correctly
// function AIPlayer::hSetAimVector(%obj, %vec)
// {
	// %pos = %obj.getPosition();
	// %vec = vectorScale(%vec,5000);
	// %vec = vectorAdd(%vec,"0 0 2.2");
	// %aimLoc = vectorAdd(%pos,%vec);

	// %obj.setAimLocation(%aimLoc);
// }

//confusing function named, gets a point in front of or behind a player scaled by %a, hReturnForwardVectorScaled
function AIPlayer::hReturnForwardBackPos(%obj, %a)
{
	%vec = %obj.getForwardVector();
	%sVec = vectorScale(%vec,%a);
	
	%pos = vectorAdd(%obj.getPosition(), %sVec);
	return %pos;
}

//used in armor on mount, when a bot is infected or stampeding it boots the player, needs to be it's own function due to delay
function AIPlayer::hKickRider( %obj )//, %col )
{
	%col = %obj.getMountedObject(0);
	if( !isObject( %col ) || %obj.isDisabled() || %col.isDisabled() )
		return;

	// if(isObject(%obj) && isObject(%col) && %col.getState() !$= "Dead" && %obj.getState() !$= "Dead")
	// {
		%obj.addVelocity(vectorAdd(vectorScale(%col.getForwardVector(),8),"0 0 5"));
		%col.dismount();
		%col.addVelocity(vectorScale(%col.getForwardVector(),-10) );
	// }
}

// called when a hole spawn is planted. Used in packages onLoadPlant and trustCheckFinished so bots spawn correctly with ownership
function fxDTSBrick::onHoleSpawnPlanted(%obj)
{
	// set bot type, and set the isBotHole flag
	%obj.hBotType = %obj.getDatablock().holeBot;
	%obj.isBotHole = 1;
	
	//Check for personal bot brick set
	if(!isObject(%obj.getGroup().hBotBrickSet))
	{
		%obj.getGroup().hBotBrickSet = new simset();
	}
	%obj.getGroup().hBotBrickSet.add(%obj);
	
	//Check for global bot brick set
	if(!isObject(mainHoleBotBrickSet))
	{
		new simset(mainHoleBotBrickSet);
	}
	mainHoleBotBrickSet.add(%obj);
	
	%count = %obj.getNumUpBricks();
	for(%a = 0; %a < %count; %a++)
	{
		%oBrick = %obj.getUpBrick(%a);
		if(%oBrick.isBotHole)
		{
			%client = %obj.getGroup().client;
			
			if( isObject( %client ) )
				%client.centerPrint("Can't stack bot bricks",3);
			// %obj.delete();
			return;
		}
	}
	%count = %obj.getNumDownBricks();
	for(%a = 0; %a < %count; %a++)
	{
		%oBrick = %obj.getDownBrick(%a);
		if(%oBrick.isBotHole)
		{
			%client = %obj.getGroup().client;
			
			if( isObject( %client ) )
			%client.centerPrint("Can't stack bot bricks",3);
			// %obj.delete();
			return;
		}
	}

	// //Check for how many bots you have
	// if(isObject(%obj.getGroup().hBotBrickSet) && %obj.getGroup().hBotBrickSet.getCount() >= $Server::Quota::Player)
	// {
		// %client = %obj.getGroup().client;
		// %client.centerPrint("You already have" SPC $Server::Quota::Player SPC "Bots",3);
		// // %obj.delete();
		// return;
	// }
	
	// //Checks if server is at it's limit for bots, uses the same quota as the player vehicles
	// if(isObject(mainHoleBotBrickSet) && mainHoleBotBrickSet.getCount() >= $Server::MaxPlayerVehicles_Total)
	// {
		// %client = %obj.getGroup().client;
		// %client.centerPrint("Server is limited to" SPC $Server::MaxPlayerVehicles_Total SPC "Bots",3);
		// // %obj.delete();
		// return;
	// }

	// %obj.hBotType = %obj.getDatablock().holeBot;
	// %obj.isBotHole = 1;
	
	//And finally spawn the bot through the main spawn function
   if(!%obj.hBotType.hManualSpawn)
   	%obj.spawnHoleBot();
}

//used to shorten color values so they fit on events, can be used in general to shorten color values if needed
function hGetFourDigits(%num, %mod,%mode)
{
	if(!%mode)
	{
		%a = 0;
		%b = 3;
		for(%n = 0; %n < %mod; %n++)
		{
			%a+=3; %b+=3;
		}
		%final = getwords(%num,%a,%b);
		return %final SPC 1;
	}
	if(%mode)
	{
		%a = 0;
		%b = 3;
		for(%n = 0; %n < %mod; %n++)
		{
			%a+=4; %b+=4;
		}
		%r = mFloatLength(getword(%num,%a),1);
		%g = mFloatLength(getword(%num,%a+1),1);
		%b = mFloatLength(getword(%num,%a+2),1);
		%a = mFloatLength(getword(%num,%a+3),1);
		// %a = 1;
		
		// for the accent piece we need transparency
		if( %a != 1 )
			%final = %r SPC %g SPC %b SPC %a;
		else
			%final = %r SPC %g SPC %b;
			
		// echo( %final );
		return %final;
	}
}

//returns brickGroup from bl id, used for when the user isn't here in client form
function hGetBrickGroupFromBLID(%id)
{
	%count = mainBrickGroup.getCount();

	for(%a = 0; %a < %count; %a++)
	{
		%userGroup = mainBrickGroup.getObject(%a);

		if(%userGroup.bl_id == %id)
		{
			return %userGroup;
		}
	}

	return 0;
}

//prevents spam from using the apply appearance commands, probably a better way to do this
function AIPlayer::validateAvatarPrefs(%obj)
{
	return;
}
function AIPlayer::applyBodyParts(%obj)
{
	// echo( "AIPlayer::applyBodyParts called" );
	
	// GameConnection::ApplyBodyParts(%obj);
	return;
}
function AIPlayer::applyBodyColors(%obj)
{
	// echo( "AIPlayer::applyBodyColors called");
	
	// GameConnection::ApplyBodyColors(%obj);
	return;
}
//this will randomly get called when the bots are alive, not sure what it's doing, need to check with badspot on this
function AIPlayer::eventFloodCheck(%a,%b,%c,%d,%e)
{
	//echo(%a SPC %b SPC %c SPC %d SPC %e);
	// not sure why this is getting called ##
	return;
}

//Useful for getting your player's avatar to use in onadd bot functions, check console when used
function dumpAvatarSettings(%client,%name)
{
	%client = findclientbyname(%client);
	echo("\c2//Appearance " @ %name);
	%prefRoot = "\c4%obj.";
	echo(%prefroot @ "llegColor =  \"" @ %client.llegColor @ "\";");
	echo(%prefroot @ "secondPackColor =  \"" @ %client.secondPackColor @ "\";");
	echo(%prefroot @ "lhand =  \"" @ %client.lhand @ "\";");
	echo(%prefroot @ "hip =  \"" @ %client.hip @ "\";");
	echo(%prefroot @ "faceName =  \"" @ %client.faceName @ "\";");
	echo(%prefroot @ "rarmColor =  \"" @ %client.rarmColor @ "\";");
	echo(%prefroot @ "hatColor =  \"" @ %client.hatColor @ "\";");
	echo(%prefroot @ "hipColor =  \"" @ %client.hipColor @ "\";");
	echo(%prefroot @ "chest =  \"" @ %client.chest @ "\";");
	echo(%prefroot @ "rarm =  \"" @ %client.rarm @ "\";");
	echo(%prefroot @ "packColor =  \"" @ %client.packColor @ "\";");
	echo(%prefroot @ "pack =  \"" @ %client.pack @ "\";");
	echo(%prefroot @ "decalName =  \"" @ %client.decalName @ "\";");
	echo(%prefroot @ "larmColor =  \"" @ %client.larmColor @ "\";");
	echo(%prefroot @ "secondPack =  \"" @ %client.secondPack @ "\";");
	echo(%prefroot @ "larm =  \"" @ %client.larm @ "\";");
	echo(%prefroot @ "chestColor =  \"" @ %client.chestColor @ "\";");
	echo(%prefroot @ "accentColor =  \"" @ %client.accentColor @ "\";");
	echo(%prefroot @ "rhandColor =  \"" @ %client.rhandColor @ "\";");
	echo(%prefroot @ "rleg =  \"" @ %client.rleg @ "\";");
	echo(%prefroot @ "rlegColor =  \"" @ %client.rlegColor @ "\";");
	echo(%prefroot @ "accent =  \"" @ %client.accent @ "\";");
	echo(%prefroot @ "headColor =  \"" @ %client.headColor @ "\";");
	echo(%prefroot @ "rhand =  \"" @ %client.rhand @ "\";");
	echo(%prefroot @ "lleg =  \"" @ %client.lleg @ "\";");
	echo(%prefroot @ "lhandColor =  \"" @ %client.lhandColor @ "\";");
	echo(%prefroot @ "hat =  \"" @ %client.hat @ "\";");
}

//--------------------------------------------------------------------------//
//----------Random datablocks used for jumping and crouching mainly---------//
//--------------------------------------------------------------------------//

datablock ShapeBaseImageData(hTurretImage)
{
    shapeFile = "base/data/shapes/empty.dts";
	emap = false;
	mountPoint = 0;
};
// datablock ShapeBaseImageData(hJumpImage)
// {
    // shapeFile = "base/data/shapes/empty.dts";
	// emap = false;
	// mountPoint = 2;
// };
// datablock ShapeBaseImageData(hCrouchImage)
// {
    // shapeFile = "base/data/shapes/empty.dts";
	// emap = false;
	// mountPoint = 3;
// };

//Default melee damage type for bots
AddDamageType("HoleMelee",   '<bitmap:Add-Ons/Bot_Hole/CI_Melee> %1',    '%2 <bitmap:Add-Ons/Bot_Hole/CI_Melee> %1',0.5,1);



//Arrays : makes going through large chunks of common things easier
//useFul for making functions shorter where you have to loop through each color/piece
$aCL = -1;
$avatarColorLoop[$aCL++] = "secondPackColor";
$avatarColorLoop[$aCL++] = "rarmColor";
$avatarColorLoop[$aCL++] = "hatColor";
$avatarColorLoop[$aCL++] = "hipColor";
$avatarColorLoop[$aCL++] = "packColor";
$avatarColorLoop[$aCL++] = "larmColor";
$avatarColorLoop[$aCL++] = "chestColor";
$avatarColorLoop[$aCL++] = "accentColor";
$avatarColorLoop[$aCL++] = "rhandColor";
$avatarColorLoop[$aCL++] = "rlegColor";
$avatarColorLoop[$aCL++] = "lhandColor";
$avatarColorLoop[$aCL++] = "llegColor";
$avatarColorLoop[$aCL++] = "headColor";
	
$aPL = -1;
$avatatPieceLoop[$aPL++] = "hat";
$avatatPieceLoop[$aPL++] = "rhand";
$avatatPieceLoop[$aPL++] = "lleg";
$avatatPieceLoop[$aPL++] = "accent";
$avatatPieceLoop[$aPL++] = "rleg";
$avatatPieceLoop[$aPL++] = "secondPack";
$avatatPieceLoop[$aPL++] = "larm";
$avatatPieceLoop[$aPL++] = "pack";
$avatatPieceLoop[$aPL++] = "decalName";
$avatatPieceLoop[$aPL++] = "chest";
$avatatPieceLoop[$aPL++] = "rarm";
$avatatPieceLoop[$aPL++] = "lhand";
$avatatPieceLoop[$aPL++] = "hip";
$avatatPieceLoop[$aPL++] = "faceName";


//Bot gestures, automatically does animations, special actions need to be scripted in. Might rewrite this to make it easier to add special cases to
$holeBotIdleGestures[$hBotIdGest = 0] = "sit";
$holeBotIdleGestures[$hBotIdGest++] = "activate";
$holeBotIdleGestures[$hBotIdGest++] = "activate2";
$holeBotIdleGestures[$hBotIdGest++] = "rotccw";
$holeBotIdleGestures[$hBotIdGest++] = "undo";
$holeBotIdleGestures[$hBotIdGest++] = "wrench";
$holeBotIdleGestures[$hBotIdGest++] = "talk";
$holeBotIdleGestures[$hBotIdGest++] = "crouch";
$holeBotIdleGestures[$hBotIdGest++] = "love";
$holeBotIdleGestures[$hBotIdGest++] = "Bricks";
$holeBotIdleGestures[$hBotIdGest++] = "Alarm";
$holeBotIdleGestures[$hBotIdGest++] = "wtf";

