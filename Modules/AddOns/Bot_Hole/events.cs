//events for holes
//onBotSpawn - self, Bot, Minigame
//onBotDeath - self, Bot, Player, Client, Minigame
//onBotReachBrick - self, Bot, Minigame

//setBot - Bot Datablock
//goToBrick - Brick list
//startLoop -
//stopLoop -
//setSearch - Enable/Disable, Radius, Sight, Strafe
//setWander - Enable/Disable, Return to Spawn, Distance
//setAppearance - Pop up gui? / Apply Avatar Button?
//setWeapon - Weapon, Melee Damage /--/
//dropItem - Item, Chance of drop /--/
//respawn

//setRandomEventEnabled ?

//--Input Events--//
//Called when bot dies
function fxDTSBrick::onBotDeath(%obj)
{
	if(isObject(%obj.hBot.hKiller))
	{
		%client = %obj.hBot.hKiller;
		$InputTarget_["Self"]   = %obj;
		$InputTarget_["Bot"] = %obj.hBot;
		$InputTarget_["Player"] = %client.player;
		$InputTarget_["Client"] = %client;
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
	}
	else
	{
		%client = %obj.getGroup().client;
		$InputTarget_["Self"]	= %obj;
		$InputTarget_["Bot"]	= %obj.hBot;
		$InputTarget_["Player"]	= 0;
		$InputTarget_["Client"]	= 0;
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);
	}
   //process the event
   %obj.processInputEvent("onBotDeath", %client);
}
registerInputEvent("fxDTSBrick", "onBotDeath", "Self fxDTSBrick" TAB "Bot Bot" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

//calls when bot is clicked by a user
function fxDTSBrick::onBotActivated(%obj,%client)
{
	$InputTarget_["Self"]   = %obj;
	$InputTarget_["Bot"] = %obj.hBot;
	$InputTarget_["Player"] = %client.player;
	$InputTarget_["Client"] = %client;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);

	%obj.processInputEvent("onBotActivated", %client);
}
registerInputEvent("fxDTSBrick", "onBotActivated", "Self fxDTSBrick" TAB "Bot Bot" TAB "Player Player" TAB "Client GameConnection" TAB "MiniGame MiniGame");

//called when bot is spawned
function fxDTSBrick::onBotSpawn(%obj)
{
	%client = %obj.getGroup().client;
	$InputTarget_["Self"]	= %obj;
	$InputTarget_["Bot"]	= %obj.hBot;
	// $InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);

	%obj.processInputEvent("onBotSpawn", %client);
}
registerInputEvent("fxDTSBrick", "onBotSpawn", "Self fxDTSBrick" TAB "Bot Bot");// TAB "MiniGame MiniGame" 

//called when the bot reaches his target brick
function fxDTSBrick::onBotReachBrick(%obj,%bot)
{
	%client = %obj.getGroup().client;
	$InputTarget_["Self"]	= %obj;
	$InputTarget_["Bot"]	= %bot;
	$InputTarget_["MiniGame"] = getMiniGameFromObject(%obj);

	%obj.processInputEvent("onBotReachBrick", %client);
}
registerInputEvent("fxDTSBrick", "onBotReachBrick", "Self fxDTSBrick" TAB "Bot Bot" TAB "MiniGame MiniGame");

//--Output Events--//
function AIPlayer::lookAtPlayer( %obj, %opt, %client )
{	
	if( %opt == 0 )
	{
		%aimPlayer = %obj.getAimObject();
	
		%obj.clearAim();
		
		if( isObject( %aimPlayer ) )
			%obj.setAimLocation( %aimPlayer.getEyePoint() );
		
		return;
	}
	else if( %opt == 1 )
		%player = %client.player;
	else if( %opt == 2 )
	{
		if( %obj.hEventLastLookTime+600 > getSimTime() )
			return;
			
		// write down last time this was called
		%obj.hEventLastLookTime = getSimTime();
			
		%type = $TypeMasks::PlayerObjectType | $TypeMasks::CorpseObjectType;
		%pos = %obj.getPosition();
		%scale = getWord(%obj.getScale(),0);
		%radius = 2*%scale;

		%firstBot = 0;
		%player = 0;
		
		initContainerRadiusSearch( %pos, %radius, %type );
		while( ( %target = containerSearchNext() ) != 0 )
		{
			%target = %target.getID();
			
			%isBot = %target.getClassName() $= "AIPLayer";
			
			if( !%firstBot && %isbot && %target != %obj )
				%firstBot = %target;
				
			if( !%isBot )
			{
				%player = %target;
				break;
			}
			
			// if( !%isbot )//%player != %obj )
				// break;
		}
		
		if( !%player )
			%player = %firstBot;
		
		// %isItUs = %player == %obj;
		
		if( !%player )
		{
			
			if( isObject( (%aimPlayer = %obj.getAimObject()) ) )
			{
				// %aimPlayer = %obj.getAimObject();
			
				%obj.clearAim();
				%obj.setAimLocation( %aimPlayer.getEyePoint() );
				
				return;
			}
			
			return;
		}
		
	}
	else if( %opt == 3 )
		%obj.setAimVector( %obj.spawnBrick.getForwardVector() );
	
	if( isObject( %player ) )
		%obj.setAimObject( %player );
		
}
registerOutputEvent( Bot,"LookAtPlayer","list Clear 0 Activator 1 Closest 2 Reset 3", 1 );//,"string 20 100");

function AIPlayer::lookAtBrick(%obj,%target)
{	
	// %obj.hClearMovement();
	%count = getWordCount(%target);
	
	%brick = getWord(%target,getrandom(0,%count-1));
   
   if(isobject(%obj.spawnBrick))
   {
      %tBrick = hReturnNamedBrick(%obj.spawnBrick.getGroup(),%brick);
      if(isObject(%tBrick))
      {
         %obj.setAimLocation( %tBrick.getPosition() );
      }
   }
}
registerOutputEvent(Bot,"LookAtBrick","string 20 100");

function AIPlayer::goToBrick(%obj,%target)
{	
	%obj.hClearMovement();
	%count = getWordCount(%target);
	
	%brick = getWord(%target,getrandom(0,%count-1));

	%tBrick = hReturnNamedBrick(%obj.spawnBrick.getGroup(),%brick);
	if(isObject(%tBrick))
	{
		%obj.hPathBrick = %tBrick;
		//%obj.setMoveDestination(%tBrick.getTransform());
		cancel( %obj.hSched );
      %obj.hSched = 0;
		%obj.hLoop();
		
		%obj.hSkipOnReachLoop = 1;
	}
}
registerOutputEvent(Bot,"GoToBrick","string 20 100");

function AIPlayer::startHoleLoop(%obj, %limit)
{
	if( !isObject( %obj ) )
		return;
		
	if(%limit && isObject(%obj.spawnBrick) && getSimTime() < %obj.spawnBrick.hLastStartHoleTime+1000)
	{
		return;
	}
	if(!%obj.hLoopActive)
	{
		cancel(%obj.hSched);
      %obj.hSched = 0;
		%obj.hLoopActive = 1;
		%obj.hLoop();
		%obj.spawnBrick.hLastStartHoleTime = getSimTime();
	}
}
//registerOutputEvent(Bot,"StartHoleLoop");

function AIPlayer::stopHoleLoop(%obj, %limit)
{
	if(!%obj.hLoopActive || !isObject( %obj ) )
		return;

	cancel(%obj.hSched);
   %obj.hSched = 0;
	cancel(%obj.hShotSched);
   %obj.hShotSched = 0;
	%obj.hLoopActive = 0;
	
	%obj.hClearMovement();
	
	// if the bot just spawned to reset move destination, this is to fix a bug where larger bots spaz out
	if( %obj.hLastSpawnTime+200 < getSimTime() )
		%obj.setMoveDestination( %obj.getPosition() );
		
	return;
	// old code
	%obj.clearMoveY();
	%obj.clearMoveX();
	%obj.setMoveObject("");
	// %obj.setMoveDestination(%obj.getPosition());
	
	//Make sure they're looking forward, have to do this with horses and sharks to keep them from freaking out and looking in their model
	//Need some command that totally clears movement from setMoveDestination to prevent this
	%scale = getWord(%obj.getScale(),0);
	%pos = vectorAdd(%obj.getPosition(),"0 0" SPC 2*%scale);
	%vec = vectorScale(%obj.getForwardVector(),5);
	// %obj.setAimLocation(vectorAdd(%pos,%vec));
}
//registerOutputEvent(Bot,"StopHoleLoop");

function AIPlayer::resetHoleLoop(%obj, %limit)
{
	%obj.stopHoleLoop();
	%obj.startHoleLoop(%limit);
}

function AIPlayer::setBotPowered(%obj, %list)
{
	if(%list == 0)
		%obj.stopHoleLoop();
	else if(%list == 1)
		%obj.startHoleLoop(1);
	else if(%list == 2)
		%obj.resetHoleLoop(1);
}

registerOutputEvent(Bot, "SetBotPowered", "list Off 0 On 1 Reset 2", 0);

function AIPlayer::setRunSpeed(%obj, %speed)
{
	%obj.setMoveSpeed( %speed );
	
	// make sure we know our max speed for alt wander
	%obj.hMaxMoveSpeed = %speed;
}
// float MINIMUM MAXIMUM STEPSIZE DEFAULT
registerOutputEvent(Bot, "SetRunSpeed", "float 0.2 1.0 0.05 1.0", 0);


// $holeBotGestures[$hBotGest = 0] = "Ani_activate2 2";
// $holeBotGestures[$hBotGest++] = "Ani_rotccw 3";
// $holeBotGestures[$hBotGest++] = "Ani_undo 4";
// $holeBotGestures[$hBotGest++] = "Ani_wrench 5";
// $holeBotGestures[$hBotGest++] = "Ani_talk 6";
// $holeBotGestures[$hBotGest++] = "Ani_head 7";
// $holeBotGestures[$hBotGest++] = "Ani_headReset 8";

// $holeBotGestures[$hBotGest++] = "Mov_activate 1";
// $holeBotGestures[$hBotGest++] = "Mov_attack 9";
// $holeBotGestures[$hBotGest++] = "Mov_sit 10";
// $holeBotGestures[$hBotGest++] = "Mov_crouch 11";
// $holeBotGestures[$hBotGest++] = "Mov_jump 12";
// $holeBotGestures[$hBotGest++] = "Mov_jet 13";

// $holeBotGestures[$hBotGest++] = "Emo_alarm 14";
// $holeBotGestures[$hBotGest++] = "Emo_love 15";
// $holeBotGestures[$hBotGest++] = "Emo_hate 16";
// $holeBotGestures[$hBotGest++] = "Emo_confusion 17";
// $holeBotGestures[$hBotGest++] = "Emo_bricks 18";

// $holeBotGestures[$hBotGest++] = "Arm_ArmUp 19";
// $holeBotGestures[$hBotGest++] = "Arm_BothArmsUp 20";
// $holeBotGestures[$hBotGest++] = "Arm_ArmsDown 21";

//Create list from gesture array
function createGestureList()
{
	%count = -1;
	%list[%count++] = "Ani_activate2 2";
	%list[%count++] = "Ani_rotccw 3";
	%list[%count++] = "Ani_undo 4";
	%list[%count++] = "Ani_wrench 5";
	%list[%count++] = "Ani_talk 6";
	%list[%count++] = "Ani_head 7";
	%list[%count++] = "Ani_headReset 8";

	%list[%count++] = "Mov_activate 1";
	%list[%count++] = "Mov_attack 9";
	%list[%count++] = "Mov_sit 10";
	%list[%count++] = "Mov_crouch 11";
	%list[%count++] = "Mov_jump 12";
	%list[%count++] = "Mov_jet 13";

	%list[%count++] = "Emo_alarm 14";
	%list[%count++] = "Emo_love 15";
	%list[%count++] = "Emo_hate 16";
	%list[%count++] = "Emo_confusion 17";
	%list[%count++] = "Emo_bricks 18";

	%list[%count++] = "Arm_ArmUp 19";
	%list[%count++] = "Arm_BothArmsUp 20";
	%list[%count++] = "Arm_ArmsDown 21";

	%total = 0;
	// %count = $hBotGest;
	%catList = "";
	for(%a = 0; %a <= %count; %a++)
	{
		// %gesture = $holeBotGestures[%a];
		%gesture = %list[ %a ];
		
		%nGest = getWord( %gesture, 1 );
		
		%preGest = strreplace(%gesture,"_"," ");
		%preGest = getWord(%preGest,1);
	
		// add to our working list
		$hBotGest++;
		$holeBotGestures[ %nGest ] = %preGest;
		
		%catList = %catList SPC %gesture;// SPC %total++;
	}

	%fList = "Root 0" @ %catList;
	//registerOutputEvent(Bot, "SetBotDataBlock", list SPC %fList, 0);
	
	if( $pref::server::isDebugCrap )
		echo(%fList);
		
	registerOutputEvent(Bot, "PlayGesture", list SPC %fList, 0);
}
// hRandomHeadTurn
function AIPlayer::playGesture(%obj,%list)
{
	%gest = $holeBotGestures[%list];//-1
	// %gest = strreplace(%gest,"_"," ");
	// %gest = getWord(%gest,1);

	if(%list == 0)
	{
		%obj.playThread(3,root);
		// %obj.setImageTrigger(2,0);
		// %obj.setImageTrigger(3,0);
		%obj.hResetMoveTriggers();
		return;
	}

	//Arm thread
	if(%gest $= "ArmUp")
	{
		%obj.hDefaultThread = "armReadyRight";
		%obj.playThread(1, "armReadyRight");
		return;
	}
	if(%gest $= "BothArmsUp")
	{
		%obj.hDefaultThread = "ArmReadyBoth";
		%obj.playThread(1, "armReadyBoth");
		return;
	}
	if(%gest $= "ArmsDown")
	{
		%obj.hDefaultThread = 0;
		%obj.playThread(1,"root");
		return;
	}
	if(%gest $= "head" )
	{
		%obj.hRandomHeadTurn();
		return;
	}
	if(%gest $= "headReset" )
	{
		%obj.hResetHeadTurn();
		return;
	}

	//Actions
	if(%gest $= "sit")
	{
		serverCmdSit(%obj);
		return;
	}
	if(%gest $= "crouch")
	{
		%obj.setCrouching(1);
		return;
	}
	if(%gest $= "jump")
	{
		%obj.hJump(200);
		return;
	}
	if(%gest $= "jet")
	{
		%obj.hJet(2000);
	}
	if(%gest $= "attack")
	{
		%obj.setImageTrigger(0,1);
		%image = %obj.getMountedImage( 0 );
		
		cancel( %obj.hAttackChargeSchedule );
		
		if( %image.isChargeWeapon )	
			%obj.hAttackChargeSchedule = %obj.scheduleNoQuota( 1000, setImageTrigger, 0, 0 );
		else if( %image.melee )
			%obj.hAttackChargeSchedule = %obj.scheduleNoQuota( 32, setImageTrigger, 0, 0 );
		else
			%obj.setImageTrigger( 0, 0 );
			
		return;
	}

	//Emotes
	if(%gest $= "love")
	{
		%obj.emote("loveImage");
		return;
	}
	if(%gest $= "alarm")
	{
		%obj.emote("alarmProjectile");
		return;
	}
	if(%gest $= "confusion")
	{
		%obj.emote("wtfImage");
		return;
	}
	if(%gest $= "hate")
	{
		%obj.emote("hateImage");
		return;
	}
	if(%gest $= "bricks")
	{
		%obj.emote("BSDProjectile");
		return;
	}
	if(%gest $= "activate")
	{
		%obj.activateStuff();
		return;
	}

	%obj.playThread(3,%gest);
}

function AIPlayer::setBotDataBlock(%obj,%list)
{
	if(!%list)
	{
		//Set Bot to nothing
		return;
	}
	else
	{
		//%obj.hBotType = %list;
		//%obj.hBot.delete();
      //%obj.hBot = 0;
		//spawnHoleBot(%obj);
		%obj.changeDataBlock(%list);
	}
}

// this sets the direction the bot has to be facing relative of you to be activatable, useful for npc talking or npc backstabbing
function AIPlayer::setActivateDirection( %obj, %dir )
{
	%obj.hActivateDirection = %dir;
}

registerOutputEvent(Bot,"SetActivateDirection","list Both 0 Front 1 Back 2");

function fxDTSBrick::setBotPowered( %obj, %on )
{
	if( !isObject( %obj.hBot ) )
		return;

	if( %on )
		%obj.hBot.StartHoleLoop();
	else
		%obj.hBot.StopHoleLoop();
}
registerOutputEvent( fxDTSBrick, "setBotPowered", "bool" );

function fxDTSBrick::respawnBot( %obj )
{
	// %obj.hBot.respawnHoleBot();
	if( getSimTime() > %obj.hLastSpawntime+200 )
		%obj.spawnHoleBot();
}
registerOutputEvent(fxDTSBrick, "respawnBot");

function fxDTSBrick::setBotType(%obj,%list)
{
	if(getSimTime() >= %obj.hLastSpawnTime+500)
	{
		if( !%list )
		{
			cancel(%obj.hModS);
         %obj.hModS = 0;
			%obj.hBotType = 0;
			if(isObject(%obj.hBot) && %obj.hBot.getState() !$= "Dead")
			{
				%obj.hBot.delete();
            %obj.hBot = 0;
				%obj.hLastSpawnTime = getSimTime();
			}
		}
		else
		{
			// don't respawn bot if we set it to itself
			if( %obj.hBotType == %list || ( %list == -1 && %obj.hBotType == %obj.getDataBlock().holeBot.getID() ) )
				return;
			
			if(isObject(%obj.hBot) && %obj.hBot.getState() !$= "Dead")
			{
				%obj.hBot.delete();
            %obj.hBot = 0;
			}
			cancel(%obj.hModS);
         %obj.hModS = 0;
			
			// if -1 then set the bot type to default
			if( %list == -1 )
				%obj.hBotType = %obj.getDataBlock().holeBot.getID();
			else
				%obj.hBotType = %list;
			
         if(!%obj.hBotType.hManualSpawn)
   			%obj.spawnHoleBot();

			//%obj.hLastSpawnTime = getSimTime(); //egh: removed this because it is done in spawnholebot
		}
	}
}

//Create the list of the bots on mission load for the event fxDTSBrick::setHoleBot
function createHoleBotList()
{
	%total = 0;
	%count = getDataBlockGroupSize();
	%list = "";
	for(%a = 0; %a < %count; %a++)
	{
		%dataBlock = getDataBlock(%a);
		if(%dataBlock.getClassname() $= "PlayerData" && %dataBlock.isHoleBot && strlen(%dataBlock.hName))
		{
			%name = strreplace(%dataBlock.hName," ","");
			%list = %list SPC %name SPC %dataBlock;
			%total++;
		}
	}

	%fList = "Default -1 None 0" @ %list;
	
	if( $pref::server::isDebugCrap )
		echo( %fList );
		
	//registerOutputEvent(Bot, "SetBotDataBlock", list SPC %fList, 0);
	registerOutputEvent(fxDTSBrick, "setBotType", list SPC %fList, 0);
}

// createHoleBotList();

$HBSE = -1;
$hBotSearchEvent[$HBSE++] = "Off";
$hBotSearchEvent[$HBSE++] = "Only_React";
$hBotSearchEvent[$HBSE++] = "AlwaysFindPlayer";
$hBotSearchEvent[$HBSE++] = "FOV------>";
$hBotSearchEvent[$HBSE++] = "Radius--->";
// $hBotSearchEvent[$HBSE++] = "FOV_8";
// $hBotSearchEvent[$HBSE++] = "FOV_16";
// $hBotSearchEvent[$HBSE++] = "FOV_32";
// $hBotSearchEvent[$HBSE++] = "FOV_64";
// $hBotSearchEvent[$HBSE++] = "FOV_128";
// $hBotSearchEvent[$HBSE++] = "Radius_8";
// $hBotSearchEvent[$HBSE++] = "Radius_16";
// $hBotSearchEvent[$HBSE++] = "Radius_32";
// $hBotSearchEvent[$HBSE++] = "Radius_64";
// $hBotSearchEvent[$HBSE++] = "Radius_128";
// $hBotSearchEvent[$hBSE++] = "Radius_256";

function hCreateSearchList()
{
	%count = $hBSE;
	%list = $hBotSearchEvent[0] SPC 0;
	for(%a = 1; %a <= %count; %a++)
	{
		%gesture = $hBotSearchEvent[%a];

		%list = %list SPC %gesture SPC %a;
	}
	
	if( $pref::server::isDebugCrap )
		echo(%list);
		
	registerOutputEvent(Bot, "SetSearchRadius", list SPC %list TAB "int 0 1024 32" , 0);
}
function AIPlayer::SetSearchRadius( %obj, %list, %radius )
{
	%search = $hBotSearchEvent[%list];
	//%obj.hSearchFOV = %list;
	//%obj.hFOVRadius = %fov;
	%dataSight = %obj.getDataBlock().hSight;
	
	// reset sight setting
	%obj.hSight = %dataSight;
	
	if(%search $= "AlwaysFindPlayer")
	{
		%obj.hSearch = 1;
		%obj.hSearchRadius = -2;
		%obj.hSight = 0;
		%obj.hSearchFOV = 0;
	}
	if(%search $= "Off")
	{
		%obj.hSearch = 0;
		%obj.hSearchRadius = 0;

		%obj.hSearchFOV = 0;
		%obj.hFOVRadius = 0;
		%obj.hClearMovement();
	}
	if(%search $= "Only_React")
	{
		%obj.hSearch = 1;
		%obj.hSearchRadius = 0;

		%obj.hSearchFOV = 0;
		%obj.hFOVRadius = 0;
	}

	// %search = strreplace(%search,"_"," ");
	%search = strreplace(%search,"-"," ");
	%pre = getWord(%search,0);
	// %radius = getWord(%search,1);

	// echo( %pre );
	
	if(%pre $= "Radius")
	{
		%obj.hSearchFOV = 0;
		%obj.hSearch = 1;
		%obj.hSearchRadius = %radius;
	}
	if(%pre $= "FOV")
	{
		// for now force sight on
		%obj.hSight = 1;
		
		%obj.hSearchFOV = 1;
		%obj.hHearing = 1;
		// %obj.hFOVRadius = %radius;
		%obj.hSearchRadius = %radius;
		
		// check if we have a hFOVRange set
		if( !strlen( %obj.hFOVRange ) )
			%obj.hFOVRange = 0.7;
	}
}
//registerOutputEvent(Bot,"SetSearchRadius", "int -2 1024 32", 0);
$hBIE = -1;
$hBotIdleEvent[$hBIE++] = "Off";
$hBotIdleEvent[$hBIE++] = "On";
$hBotIdleEvent[$hBIE++] = "Anim_Off";
$hBotIdleEvent[$hBIE++] = "Anim_On";
$hBotIdleEvent[$hBIE++] = "Emote_Off";
$hBotIdleEvent[$hBIE++] = "Emote_On";
$hBotIdleEvent[$hBIE++] = "Look_Off";
$hBotIdleEvent[$hBIE++] = "Look_On";
$hBotIdleEvent[$hBIE++] = "Spam_Off";
$hBotIdleEvent[$hBIE++] = "Spam_On";
// $hBotIdleEvent[$hBIE++] = "SpasLook_Off";
// $hBotIdleEvent[$hBIE++] = "SpasLook_On";

function hCreateIdleList()
{
	%count = $hBIE;
	%list = $hBotIdleEvent[0] SPC 0;
	for(%a = 1; %a <= %count; %a++)
	{
		%gesture = $hBotIdleEvent[%a];

		%list = %list SPC %gesture SPC %a;
	}
	
	if( $pref::server::isDebugCrap )
		echo(%list);
		
	registerOutputEvent(Bot, "SetIdleBehavior", list SPC %list, 0);
}

function AIPlayer::SetIdleBehavior(%obj,%list)
{
	%search = $hBotIdleEvent[%list];
	
	if(%search $= "Off")
		%obj.hIdle = 0;
	else if(%search $= "On")
		%obj.hIdle = 1;
	else
		%obj.hIdle = 1;

	if(%search $= "Anim_Off")
		%obj.hIdleAnimation = 0;
	if(%search $= "Anim_On")
		%obj.hIdleAnimation = 1;
	
	if(%search $= "Emote_Off")
		%obj.hEmote = 0;
	if(%search $= "Emote_On")
		%obj.hEmote = 1;

	if(%search $= "Look_Off")
	{
		%obj.hIdleLookAtOthers = 0;
		%obj.hSpasticLook = 0;
	}
	if(%search $= "Look_On")
	{
		%obj.hIdleLookAtOthers = 1;
		%obj.hSpasticLook = 1;
	}

	if(%search $= "Spam_Off")
		%obj.hIdleSpam = 0;
	if(%search $= "Spam_On")
		%obj.hIdleSpam = 1;

	// if(%search $= "SpasLook_Off")
		// %obj.hSpasticLook = 0;
	// if(%search $= "SpasLook_On")
		// %obj.hSpasticLook = 1;
}

$hBWE = -1;
// $hBotWanderEvent[$hBWE++] = "Off";
// $hBotWanderEvent[$hBWE++] = "StayAtSpawn";
// $hBotWanderEvent[$hBWE++] = "Grid_32";
// $hBotWanderEvent[$hBWE++] = "Grid_64";
// $hBotWanderEvent[$hBWE++] = "Grid_128";
// $hBotWanderEvent[$hBWE++] = "Grid_Infinite";
// $hBotWanderEvent[$hBWE++] = "Distance_8";
// $hBotWanderEvent[$hBWE++] = "Distance_16";
// $hBotWanderEvent[$hBWE++] = "Distance_32";
// $hBotWanderEvent[$hBWE++] = "Distance_64";
// $hBotWanderEvent[$hBWE++] = "Distance_128";
// $hBotWanderEvent[$hBWE++] = "Distance_256";
// $hBotWanderEvent[$hBWE++] = "Distance_Infinite";

$hBotWanderEvent[$hBWE++] = "Off";
$hBotWanderEvent[$hBWE++] = "StayAtSpawn";
$hBotWanderEvent[$hBWE++] = "Grid------>";
$hBotWanderEvent[$hBWE++] = "Grid_Infinite";
$hBotWanderEvent[$hBWE++] = "Distance-->";
$hBotWanderEvent[$hBWE++] = "Distance_Infinite";

function hCreateWanderList()
{
	%count = $hBWE;
	%list = $hBotWanderEvent[0] SPC 0;
	for(%a = 1; %a <= %count; %a++)
	{
		%gesture = $hBotWanderEvent[%a];

		%list = %list SPC %gesture SPC %a;
	}
	
	if( $pref::server::isDebugCrap )
		echo(%list);
		
	registerOutputEvent(Bot, "SetWanderDistance", list SPC %list TAB "int 8 1024 64", 0);
}

function AIPlayer::SetWanderDistance( %obj, %list, %radius)
{
	%search = $hBotWanderEvent[%list];

	if(%search $= "Off")
	{
		%obj.hGridWander = 0;
		%obj.hWander = 0;
		%obj.hReturnToSpawn = 0;
		%obj.hSpawnDist = 0;
		%obj.hClearMovement();
		return;
	}
	if(%search $= "StayAtSpawn")
	{
		%obj.hGridWander = 0;
		%obj.hWander = 1;
		%obj.hReturnToSpawn = 1;
		%obj.hSpawnDist = 0;
		return;
	}
	if(%search $= "Distance_Infinite")
	{
		%obj.hGridWander = 0;
		%obj.hWander = 1;
		%obj.hReturnToSpawn = 0;
		%obj.hSpawnDist = 1;
		return;
	}
	if(%search $= "Grid_Infinite")
	{
		%obj.hGridWander = 1;
		%obj.hWander = 1;
		%obj.hReturnToSpawn = 0;
		%obj.hSpawnDist = 1;
		return;
	}
	%search = strreplace(%search,"-"," ");
	%pre = getWord(%search,0);
	// %radius = getWord(%search,1);
	
	if(%pre $= "Distance")
	{
		%obj.hGridWander = 0;
		%obj.hWander = 1;
		%obj.hReturnToSpawn = 1;
		%obj.hSpawnDist = %radius;
	}
	if(%pre $= "Grid")
	{
		%obj.hGridWander = 1;
		%obj.hWander = 1;
		%obj.hReturnToSpawn = 1;
		%obj.hSpawnDist = %radius;
	}
}
//registerOutputEvent(Bot,"SetWanderDist","int -2 1024 64",0);



function AIPlayer::respawnHoleBot(%obj,%a)
{
	if(getSimTime() > %obj.spawnBrick.hLastSpawntime+500 && isObject(%obj))
	{
		//%obj.spawnBrick.hLastSpawntime = getSimTime();
		%brick = %obj.spawnBrick;
		%brick.spawnHoleBot();	
		cancel(%brick.hModS);
      %brick.hModS = 0;
		if(isObject(%obj))
		{
			%obj.delete();
		}
	}
}
//registerOutputEvent(Bot,"RespawnHoleBot");

function AIPlayer::dropItem( %obj, %dataBlock, %per)
{
	if(!isObject(%dataBlock))
		return;
	
	%random = getRandom(1,100);
	if(%random <= %per)
	{
		%item = new item()
		{
			datablock = %dataBlock;
		};
		if( isObject(%item) )
		{
			%item.schedulePop();
			%pos = %obj.getPosition();
			%rot = getWords(%obj.getTransform(),3,6);
			%pos = vectorAdd(%pos,"0 0 1.7");
			%fVec = %obj.getForwardVector();

			%item.setTransform(vectorAdd(vectorScale(%fVec,0.5), %pos)  SPC %rot);
			%item.setVelocity(vectorAdd( vectorScale(%fVec,7), %obj.getVelocity()));
			%item.miniGame = getMiniGameFromObject( %obj );// %obj.spawnBrick.getGroup().client.miniGame;//Rewrite so this actually functions ##
		}
	}
}
registerOutputEvent(Bot,"DropItem", "dataBlock itemData" TAB "float 0 100 5 100", 0);

function AIPlayer::setWeapon( %obj, %item )
{
	if(%item == -1)
	{
		%obj.unMountImage(0);
		%obj.unMountImage(1);

		%obj.playThread(1,root);
		%obj.playThread(1,%obj.hDefaultThread);
		%obj.hAvoidCloseRange = 0;
		%obj.hShoot = 0;
		%obj.hLastWeapon = 0;
		return;
	}
	else
	{
		// if it's an item, switch it to it's image
		if( %item.getClassName() $= "ItemData" )
			%item = %item.image;
		
		%obj.hLastWeapon = %item;
		
		if( %item.armReady )
			%obj.playThread( 1, armReadyRight );
		
		%obj.mountImage(%item,0);
		// fixArmReady(%obj);
		%obj.playThread(1,%obj.hDefaultThread);
		%obj.hAvoidCloseRange = %obj.getDataBlock().hAvoidCloseRange;
		%obj.hTooCloseRange = %obj.getDataBlock().hTooCloseRange;
		%obj.hShoot = 1;
	}
}
registerOutputEvent(Bot,"SetWeapon", "dataBlock itemData",0);

function AIPlayer::setBotName(%obj,%name)
{
	%obj.Name = %name;
}
registerOutputEvent(Bot,"SetBotName","string 20 100");


function AIPlayer::setTeam(%obj,%list,%custom)
{
	%aList[%a = 0] = "enemy";
	%aList[%a++] = "neutral";
	%aList[%a++] = "friendly";
	%aList[%a++] = "mercenary";
	%aList[%a++] = "owner";
	%aList[%a++] = "custom";

	if(%aList[%list] $= "custom")
	{
		%final = %custom;
	}
	else %final = %aList[%list];

	%obj.hType = %final;
}
registerOutputEvent(Bot,"SetTeam","list Enemy 0 Neutral 1 Friendly 2 Mercenary 3 Owner 4 Custom 5" TAB "string 20 100");

function AIPlayer::setMeleeDamage(%obj,%damage)
{
	if(%damage >= 1)
	{
		%obj.hAttackDamage = %damage;
		%obj.hMelee = 1;
	}
	else
	{
		%obj.hAttackDamage = 0;
		%obj.hMelee = 0;
	}
}
registerOutputEvent(Bot,"SetMeleeDamage", "int 0 1024 15", 0);

//function AIPlayer::setFOV(%obj,%list,%fov)
//{
//	%obj.hSearchFOV = %list;
//	%obj.hFOVRadius = %fov;
//}
//registerOutputEvent(Bot,"SetFOV","list Disable 0 Enable 1" TAB "float 1 10 1 5",0);

// 7 hats
// 3 hat accessories for certain hats
// 6 packs
// 6 epaulets
// 2 shoulders
// 2 hands
// 2 feet
// 2 crotches

function getRandomBotTransColor()
{
	%color0 = "1 1 1 0.25098";
	%color1 = "0.667 0.000 0.000 0.700";
	%color2 = "1.000 0.500 0.000 0.700";
	%color3 = "0.990 0.960 0.000 0.700";
	%color4 = "0.000 0.471 0.196 0.700";
	%color5 = "0.000 0.200 0.640 0.700";
	%color6 = "0.596078 0.160784 0.392157 0.698039";
	%color7 = "0.550 0.700 1.000 0.700";
	%color8 = "0.850 0.850 0.850 0.700";
	%color9 = "0.100 0.100 0.100 0.700";
	%nColors = 9;
	
	return %color[ getRandom( 0, 9 ) ];
}

function getRandomBotColor()
{
	%color0 = "0.9 0 0 1";
	%color1 = "0.900 0.900 0.000 1.000";
	%color2 = "0.74902 0.180392 0.482353 1";
	%color3 = "0.388235 0 0.117647 1";
	%color4 = "0.133333 0.270588 0.270588 1";
	%color5 = "0 0.141176 0.333333 1";
	%color6 = "0.105882 0.458824 0.768627 1";
	%color7 = "1 1 1 1";
	%color8 = "0.0784314 0.0784314 0.0784314 1";
	%color9 = "0.92549 0.513726 0.678431 1";
	%color10 = "1 0.603922 0.423529 1";
	%color11 = "0.000 0.500 0.250 1.000";
	%color12 = "1 0.878431 0.611765 1";
	%color13 = "0.956863 0.878431 0.784314 1";
	%color14 = "0.784314 0.921569 0.490196 1";
	%color15 = "0.541176 0.698039 0.552941 1";
	%color16 = "0.560784 0.929412 0.960784 1";
	%color17 = "0.698039 0.662745 0.905882 1";
	%color18 = "0.878431 0.560784 0.956863 1";
	%color19 = "0.2 0 0.8 1";
	%color20 = "0.616822 0.71028 0.457944 1";
	%color21 = "0.892523 0.61215 0.0794393 1";
	%color22 = "0 0 0 1";
	%color23 = "0.900 0.900 0.900 1.000";
	%color24 = "0.750 0.750 0.750 1.000";
	%color25 = "0.500 0.500 0.500 1.000";
	%color26 = "0.200 0.200 0.200 1.000";
	%color27 = "0.392157 0.196078 0 1";
	%color28 = "0.901961 0.341176 0.0784314 1";
	%numColors = 28;
	
	return %color[ getRandom( 0, %numColors ) ];
}

function getRandomBotOffsetColor( %color )
{
	%color = vectorSub( %color, hGetRandomVector( 0, 5 ) );
	
	%color = hClampVector( %color );
	
	return %color SPC 1;
}

function getRandomBotPantsColor( %shirtColor )
{
	%color = hGetRandomFloat( 1, 10 );
	
	%color = %color SPC %color SPC %color;
	
	// randomly switch to additive color from shirt
	if( !getRandom( 0, 3 ) )
		%color = vectorSub( %shirtColor, hGetRandomVector( 0, 5 ) );
		
	// clamp vector
	%color = hClampVector( %color );
		
	return %color SPC 1;
}

function getRandomBotSkinColor()
{
	%color0 = "0.900 0.900 0.000 1.000";
	%color1 = "0.392157 0.196078 0 1";
	%color2 = "0.901961 0.341176 0.0784314 1";
	%color3 = "1 0.603922 0.423529 1";
	%color4 = "1 0.878431 0.611765 1";
	%color5 = "0.956863 0.878431 0.784314 1";
	%color6 = "0.672897 0.327103 0 1";
	
	return %color[ getRandom( 0, 6 ) ];
}
function getRandomBotFace()
{
	%face[ %nFace = 0 ] = "smiley";
	%face[ %nFace++ ] = "asciiTerror";
	// %face[ %nFace++ ] = "memeBlockMongler";
	// %face[ %nFace++ ] = "memeCats";
	// %face[ %nFace++ ] = "memeDesu";
	// %face[ %nFace++ ] = "memeGrinMan";
	// %face[ %nFace++ ] = "memeHappy";
	// %face[ %nFace++ ] = "memePBear";
	// %face[ %nFace++ ] = "memeYaranika";
	%face[ %nFace++ ] = "smileyBlonde";
	%face[ %nFace++ ] = "smileyCreepy";
	%face[ %nFace++ ] = "smileyEvil1";
	%face[ %nFace++ ] = "smileyEvil2";
	%face[ %nFace++ ] = "smileyFemale1";
	%face[ %nFace++ ] = "smileyOld";
	%face[ %nFace++ ] = "smileyPirate1";
	%face[ %nFace++ ] = "smileyPirate2";
	%face[ %nFace++ ] = "smileyPirate3";
	%face[ %nFace++ ] = "smileyRedBeard";
	%face[ %nFace++ ] = "smileyRedBeard2";
	
	return %face[ getRandom( 0, %nFace ) ];
}

// returns a random color for the bots
function getRandomBotRGBColor()
{
	%r = getRandomDongFloat();
	%g = getRandomDongFloat();
	%b = getRandomDongFloat();
	
	// for now keep alpha at 1
	%a = 1;
	
	%color = %r SPC %g SPC %b SPC %a;
	
	return %color;
}

function AIPlayer::setRandomAppearance( %obj, %style )
{
	// colors
	%skinColor = getRandomBotSkinColor();// getRandomBotColor();
	%handColor = %skinColor;
	
	%hatColor = getRandomBotColor();
	%packColor = getRandomBotColor();
	%shirtColor = getRandomBotColor();
	// %pantsColor = getRandomBotColor();
	// %shoeColor = getRandomBotColor();
	%accentColor = getRandomBotColor();
	
	%pantsColor = getRandomBotPantsColor( %shirtColor );
	%shoeColor = %pantsColor;
	
	// zero everything out
	%hat = 0;
	%accent = 0;
	%pack = 0;
	%pack2 = 0;
	%decal = "AAA-None";
	// %face = "smiley";
	%face = getRandomBotFace();
	
	// %style = getRandom( 0, 1 );

	// city style clothings
	if( %style == 0 )
	{
		// decal
		%decal[ %nDecal = 0 ] = "Mod-Army";
		%decal[ %nDecal++ ] = "Mod-Police";
		%decal[ %nDecal++ ] = "Mod-Suit";
		%decal[ %nDecal++ ] = "Meme-Mongler";
		%decal[ %nDecal++ ] = "Mod-DareDevil";
		%decal[ %nDecal++ ] = "Mod-Pilot";
		%decal[ %nDecal++ ] = "Mod-Prisoner";
		%decal[ %nDecal++ ] = "AAA-None";
		
		%decal = %decal[ getRandom( 0, %nDecal ) ];
		
		// hat
		%hat[ %nHat = 0 ] = 4;
		%hat[ %nHat++ ] = 6;
		%hat[ %nHat++ ] = 7;
		%hat[ %nHat++ ] = 0;
		
		%hat = %hat[ getRandom( 0, %nHat ) ];
	}
	// space things
	else if( %style == 1 )
	{
		%decal0 = "AAA-None";
		%decal1 = "Space-Nasa";
		%decal2 = "Space-New";
		%decal3 = "space-Old";
		
		%decal = %decal[ getRandom( 0, 3 ) ];
		
		// %pack0 = 0;
		// %pack1 = 6;
		
		// %pack = %pack[ getRandom( 0, 1 ) ];
		
		// %hat0 = 0;
		// %hat1 = 1;
		
		// %hat = %hat[ getRandom( 0, 1 ) ];
		
		%pack = 6;
		%hat = 1;
		
		%hatColor = getRandomBotOffsetColor( %shirtColor );
		%packColor = %hatColor;
		%handColor = %packColor;
		
		%pantsColor = getRandomBotOffsetColor( %shirtColor );
		%shoeColor = %pantsColor;
		
		%accent = 1;
		%accentColor = getRandomBotTransColor();
	}
	
	// accent
	%obj.accentColor = %accentColor;
	%obj.accent =  %accent;
	
	// hat
	%obj.hatColor = %hatColor;
	%obj.hat = %hat;
	
	// head
	%obj.headColor = %skinColor;
	%obj.faceName = %face;
	
	// chest
	%obj.chest =  "0";
	%obj.decalName = %decal;
	%obj.chestColor = %shirtColor;
		
	// packs
	%obj.pack =  %pack;
	%obj.packColor =  %packColor;

	%obj.secondPack =  %pack2;
	%obj.secondPackColor =  "0 0.435 0.831 1";
		
	// left arm
	%obj.larm =  "0";
	%obj.larmColor = %shirtColor;
	
	%obj.lhand =  "0";
	%obj.lhandColor = %handColor;
	
	// right arm
	%obj.rarm =  "0";
	%obj.rarmColor = %shirtColor;
	
	%obj.rhandColor = %handColor;
	%obj.rhand =  "0";
	
	// hip
	%obj.hip =  "0";
	%obj.hipColor = %pantsColor;
	
	// left leg
	%obj.lleg =  "0";
	%obj.llegColor = %shoeColor;
	
	// right leg
	%obj.rleg =  "0";
	%obj.rlegColor = %shoeColor;

	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

registerOutputEvent(Bot, "SetRandomAppearance", "List City 0 Space 1",0 );

function AIPlayer::setAppearance(%obj,%list,%body,%colorA,%colorB)
{
	if(%list == 0)
	{
		//Appearance Blockhead
		%obj.llegColor =  "0.2 0 0.8 1";
		%obj.secondPackColor =  "0 0.435 0.831 1";
		%obj.lhand =  "0";
		%obj.hip =  "0";
		%obj.faceName =  "smiley";
		%obj.rarmColor =  "0.9 0 0 1";
		%obj.hatColor =  "1 1 1 1";
		%obj.hipColor =  "0.2 0 0.8 1";
		%obj.chest =  "0";
		%obj.rarm =  "0";
		%obj.packColor =  "0.2 0 0.8 1";
		%obj.pack =  "0";
		%obj.decalName =  "AAA-None";
		%obj.larmColor =  "0.9 0 0 1";
		%obj.secondPack =  "0";
		%obj.larm =  "0";
		%obj.chestColor =  "1 1 1 1";
		%obj.accentColor =  "0.990 0.960 0 0.700";
		%obj.rhandColor =  "1 0.878 0.611 1";
		%obj.rleg =  "0";
		%obj.rlegColor =  "0.2 0 0.8 1";
		%obj.accent =  "1";
		%obj.headColor =  "1 0.878 0.611 1";
		%obj.rhand =  "0";
		%obj.lleg =  "0";
		%obj.lhandColor =  "1 0.878 0.611 1";
		%obj.hat =  "0";
	}
	if(%list == 1)
	{
		//Appearance Caveman
		%obj.llegColor =  "1 0.878 0.611 1";
		%obj.secondPackColor =  "0 0.435 0.831 1";
		%obj.lhand =  "0";
		%obj.hip =  "0";
		%obj.faceName =  "smileyRedBeard2";
		%obj.rarmColor =  "1 0.878 0.611 1";
		%obj.hatColor =  "1 1 1 1";
		%obj.hipColor =  "0.392 0.196 0 1";
		%obj.chest =  "0";
		%obj.rarm =  "0";
		%obj.packColor =  "0.2 0 0.8 1";
		%obj.pack =  "0";
		%obj.decalName =  "AAA-None";
		%obj.larmColor =  "1 0.878 0.611 1";
		%obj.secondPack =  "0";
		%obj.larm =  "0";
		%obj.chestColor =  "1 0.878 0.611 1";
		%obj.accentColor =  "0.990 0.960 0 0.700";
		%obj.rhandColor =  "1 0.878 0.611 1";
		%obj.rleg =  "0";
		%obj.rlegColor =  "1 0.878 0.611 1";
		%obj.accent =  "1";
		%obj.headColor =  "1 0.878 0.611 1";
		%obj.rhand =  "0";
		%obj.lleg =  "0";
		%obj.lhandColor =  "1 0.878 0.611 1";
		%obj.hat =  "0";
	}
	if(%list == 2)
	{
		//Appearance Cop
		%obj.llegColor =  "0.2 0.2 0.2 1";
		%obj.secondPackColor =  "0 0.435 0.831 1";
		%obj.lhand =  "0";
		%obj.hip =  "0";
		%obj.faceName =  "smileyRedBeard2";
		%obj.rarmColor =  "0 0.141 0.333 1";
		%obj.hatColor =  "0 0.141 0.333 1";
		%obj.hipColor =  "0.2 0.2 0.2 1";
		%obj.chest =  "0";
		%obj.rarm =  "0";
		%obj.packColor =  "0 1 1 1";
		%obj.pack =  "0";
		%obj.decalName =  "Mod-Police";
		%obj.larmColor =  "0 0.141 0.333 1";
		%obj.secondPack =  "0";
		%obj.larm =  "0";
		%obj.chestColor =  "0 0.141 0.333 1";
		%obj.accentColor =  "0.92549 0.513726 0.678431 1";
		%obj.rhandColor =  "1 0.878 0.611 1";
		%obj.rleg =  "0";
		%obj.rlegColor =  "0.2 0.2 0.2 1";
		%obj.accent =  "0";
		%obj.headColor =  "1 0.878 0.611 1";
		%obj.rhand =  "0";
		%obj.lleg =  "0";
		%obj.lhandColor =  "1 0.878 0.611 1";
		%obj.hat =  "6";
	}
	if(%list == 3)
	{
		//Appearance Criminal
		%obj.llegColor =  "0.2 0.2 0.2 1";
		%obj.secondPackColor =  "0 0.435 0.831 1";
		%obj.lhand =  "0";
		%obj.hip =  "0";
		%obj.faceName =  "memeBlockMongler";
		%obj.rarmColor =  "0.901 0.341 0.078 1";
		%obj.hatColor =  "0 0.141 0.333 1";
		%obj.hipColor =  "0.2 0.2 0.2 1";
		%obj.chest =  "0";
		%obj.rarm =  "0";
		%obj.packColor =  "0 1 1 1";
		%obj.pack =  "0";
		%obj.decalName =  "Mod-Prisoner";
		%obj.larmColor =  "0.901 0.341 0.078 1";
		%obj.secondPack =  "0";
		%obj.larm =  "0";
		%obj.chestColor =  "0.901 0.341 0.078 1";
		%obj.accentColor =  "0.92549 0.513726 0.678431 1";
		%obj.rhandColor =  "1 0.878 0.611 1";
		%obj.rleg =  "0";
		%obj.rlegColor =  "0.2 0.2 0.2 1";
		%obj.accent =  "0";
		%obj.headColor =  "1 0.878 0.611 1";
		%obj.rhand =  "0";
		%obj.lleg =  "0";
		%obj.lhandColor =  "1 0.878 0.611 1";
		%obj.hat =  "0";
	}
	if(%list == 4)
	{
		//Appearance Nazi
		%obj.llegColor =  "0.2 0.2 0.2 1";
		%obj.secondPackColor =  "0 0.435 0.831 1";
		%obj.lhand =  "0";
		%obj.hip =  "0";
		%obj.faceName =  "smileyBlonde";
		%obj.rarmColor =  "0.785 0.654 0.373 1";
		%obj.hatColor =  "0.785 0.654 0.373 1";
		%obj.hipColor =  "0.2 0.2 0.2 1";
		%obj.chest =  "0";
		%obj.rarm =  "0";
		%obj.packColor =  "0 1 1 1";
		%obj.pack =  "0";
		%obj.decalName =  "Mod-Suit";
		%obj.larmColor =  "0.9 0 0 1";
		%obj.secondPack =  "0";
		%obj.larm =  "0";
		%obj.chestColor =  "0.785 0.654 0.373 1";
		%obj.accentColor =  "0.92549 0.513726 0.678431 1";
		%obj.rhandColor =  "1 0.878 0.611 1";
		%obj.rleg =  "0";
		%obj.rlegColor =  "0.2 0.2 0.2 1";
		%obj.accent =  "0";
		%obj.headColor =  "1 0.878 0.611 1";
		%obj.rhand =  "0";
		%obj.lleg =  "0";
		%obj.lhandColor =  "1 0.878 0.611 1";
		%obj.hat =  "6";
	}
	if(%list == 5)
	{
		//Appearance zombie
		%obj.llegColor =  "0 0.141 0.333 1";
		%obj.secondPackColor =  "0 0.435 0.831 1";
		%obj.lhand =  "0";
		%obj.hip =  "0";
		%obj.faceName =  "asciiTerror";
		%obj.rarmColor =  "0.593 0 0 1";
		%obj.hatColor =  "1 1 1 1";
		%obj.hipColor =  "0 0.141 0.333 1";
		%obj.chest =  "0";
		%obj.rarm =  "0";
		%obj.packColor =  "0.2 0 0.8 1";
		%obj.pack =  "0";
		%obj.decalName =  "AAA-None";
		%obj.larmColor =  "0.593 0 0 1";
		%obj.secondPack =  "0";
		%obj.larm =  "0";
		%obj.chestColor =  "0.75 0.75 0.75 1";
		%obj.accentColor =  "0.990 0.960 0 0.700";
		%obj.rhandColor =  "0.626 0.71 0.453 1";
		%obj.rleg =  "0";
		%obj.rlegColor =  "0 0.141 0.333 1";
		%obj.accent =  "1";
		%obj.headColor =  "0.626 0.71 0.453 1";
		%obj.rhand =  "0";
		%obj.lleg =  "0";
		%obj.lhandColor =  "0.626 0.71 0.453 1";
		%obj.hat =  "0";
	}
	if(%list == 6)
	{
		//Custom Appearance
		%hn = 0;
		servercmdupdatebodyparts(%obj,getword(%body,%hn++),getword(%body,%hn++),getword(%body,%hn++),getword(%body,%hn++),getword(%body,%hn++),
			getword(%body,%hn++),getword(%body,%hn++),getword(%body,%hn++),getword(%body,%hn++),getword(%body,%hn++),getword(%body,%hn++),getword(%body,%hn++));

		%hn = -1;
		%bn = -1;// 3 is accentcolor, accent piece is at the end since it has transparency, kind of a hack
		servercmdupdatebodycolors(%obj,hGetFourDigits(%colorA, %hn++, 0),hGetFourDigits(%colorA, %hn++, 0),hGetFourDigits(%colorA, 6, 0),hGetFourDigits(%colorA, %hn++, 0),hGetFourDigits(%colorA, %hn++, 0),
			hGetFourDigits(%colorA, %hn++, 0),hGetFourDigits(%colorA, %hn++, 0),hGetFourDigits(%colorB, %bn++, 0),hGetFourDigits(%colorB, %bn++, 0),hGetFourDigits(%colorB, %bn++, 0),hGetFourDigits(%colorB, %bn++, 0),hGetFourDigits(%colorB, %bn++, 0),hGetFourDigits(%colorB, %bn++, 0));
		
		%obj.faceName = getWord(%body,13);
		%obj.decalName = getWord(%body,14);
		%obj.player = %obj;
	}
	GameConnection::ApplyBodyParts(%obj);
	GameConnection::ApplyBodyColors(%obj);
}

registerOutputEvent(Bot, "SetAppearance", "list Blockhead 0 Caveman 1 Cop 2 Criminal 3 Nazi 4 Zombie 5 Custom 6" TAB "string 200 19" TAB "string 200 19 0" TAB "string 200 19 0",0);// TAB "string 200 19" TAB "string 200 19 0" TAB "string 200 19 0", 0);


// register all the dynamic events
// createHoleBotList();
// createGestureList();
// hCreateWanderList();
// hCreateSearchList();
// hCreateIdleList();

//accent
//chest
//hat
//hip
//pack
//secondPack
//lArm
//lHand
//lLeg
//rArm
//rHand
//rLeg

//accentColor##
//chest
//hat
//hip
//pack
//secondPack
//lArm
//lHand
//lLeg
//rArm
//rHand
//rLegColor##
//headColor
