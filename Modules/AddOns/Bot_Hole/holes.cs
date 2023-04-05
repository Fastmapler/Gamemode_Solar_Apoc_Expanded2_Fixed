//-----------------------------Hole Mod--------------------------------//
//About:---------------------------------------------------------------//																
//Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis aliquam//
//eros doesn't this look fancy molestie neque consecetur. In ultricies //
//magna ac ipsum tristique elementum. Charlie Sheen Suspendisse Winning//
//ultricies elementum ligula, vel viverra enim semper eu.				  //
//---------------------------------------------------------------------//
//
//By:------------------------------------------------------------------//
//--Rodrigo------------------------------------------------------------//
//----"I always wanted to have a weird group of text-------------------//
//----like this prefixing code."---------------------------------------//
//----------"So fancy..."----------------------------------------------//
//---------------------------------------------------------------------//
//
//Bot Hole Mod Goals
//------------------
//Self-Contained
//Easy plugin support?
//Simple efficient ai
//Avoiding confclits if possible
//
//Was supposed to be a general clear movement command, but usually I needed to cancel only certain things //Should rework this
function AIPlayer::hClearMovement(%obj)
{
	// %obj.mountImage(hJumpImage,2);
	// %obj.mountImage(hCrouchImage,3);
	%obj.clearMoveY();
	%obj.clearMoveX();
	%obj.setImageTrigger(0,0);
	%obj.setImageTrigger(1,0);
	%obj.hResetMoveTriggers();
	%obj.setMoveObject("");
	%obj.clearAim();
	%obj.maxYawSpeed = 10;
	%obj.maxPitchSpeed = 10;
	
	%obj.hResetHeadTurn();
}

// clear the movement from triggers, jump/jet/crouch
function AIPlayer::hResetMoveTriggers( %obj )
{
	%obj.setJumping(0);
	%obj.setCrouching(0);
	
	// confusing, but hasJetted is called when doing long jets
	if( !%obj.hHasJetted )
		%obj.setJetting(0);
		
	%obj.setMoveSpeed( %obj.hMaxMoveSpeed );
	// if( %obj.getDataBlock().canJet )
		// %obj.hStopJetting();
}

function AIPlayer::hSetMoveY( %obj, %val )
{
	%val = %val*%obj.hMaxMoveSpeed;
	
	%obj.setMoveY( %val );
}

function AIPlayer::hSetMoveX( %obj, %val )
{
	%val = %val*%obj.hMaxMoveSpeed;
	
	if( %obj.isJetting() )
		return;
	
	%obj.setMoveX( %val );
}

function AIPlayer::hJump( %obj, %time )
{
	if( %obj.hLastSpawnTime+200 > getSimTime() )
		return;
		
	if( !strlen( %time ) )
		%time = 1000;

	%obj.setJumping(1);
	%obj.scheduleNoQuota(%time,setJumping,0);
}

function AIPlayer::hCrouch( %obj, %time )
{
	if( %obj.isJetting() )
		return;
	
	if( !strlen( %time ) )
		%time = 1000;

	%obj.setCrouching(1);
	%obj.scheduleNoQuota(%time,setCrouching,0);	
}

function AIPlayer::hLongJet( %obj )
{
	%obj.hHasJetted = 1;
	%obj.setMoveSpeed( 0.2 );
	%obj.hJump( 100 );
	%obj.setJetting( 1 );
	%obj.clearMoveX();
}

function AIPlayer::hJet( %obj, %time, %slow )
{
	cancel( %obj.hJetSchedule );
			
	if( %slow )
		%obj.setMoveSpeed( 0.2 );
		
	if( !strlen( %time ) )
		%time = 1000;

	%obj.setMoveSpeed(0.2);
	%obj.setJetting(1);
	%obj.hJetSchedule = %obj.scheduleNoQuota( %time, setJetting, 0 );	
	%obj.scheduleNoQuota( %time, setMoveSpeed, %obj.hMaxMoveSpeed );
}

// checks if we're falling too fast and jets if so
function AIPlayer::hIsFalling( %obj )
{
	%zVel = getWord( %obj.getVelocity(), 2 );
	
	if( %zVel <= -4 )
		return true;
	else
		return false;
}

// called whenever we stop jetting
function AIPlayer::hJetCheck( %obj, %override )
{
	%jet = %obj.getDataBlock().canJet;

	if( !%jet )
		return;

	if( !%obj.hIsFalling() && !%override )
	{
		%obj.setJetting( 0 );
		return;
	}
		
	%obj.hHasJetted = 0;
		
	// %obj.setJetting(1);
	// %obj.setMoveSpeed(0.3);
	// %tickRate = %obj.getDataBlock().hTickRate;
	
	// if(!%tickRate)
		// %tickRate = 3000;
	%tickrate = %obj.hGetTickRate();
		
	// if( %override )
		// %tickSub = 100;
	%obj.clearMoveX();
	
	if( %override )
		%obj.hJet( %tickRate-1000 );
	else
		%obj.hJet( %tickRate-500 );
		
	
}

function AIPlayer::hStopJetting( %obj )
{
	// stop the jet
	%obj.setJetting(0);
	
	// after a second do hJetCheck, which checks if we're falling to our death
	%obj.scheduleNoQuota( 1000, hJetCheck );
}

// randomly activate one of the triggers
function AIPlayer::hRandomMoveTrigger( %obj )
{
	%jet = %obj.getDataBlock().canJet;
	
	if( %jet )
		%ran = getRandom( 0, 2 );
	else
		%ran = getRandom( 0, 1 );
		
	if( %ran == 0 )
		%obj.setCrouching(1);
	else if( %ran == 1 )
		%obj.setJumping(1);
	else
		%obj.setJetting(1);
}

function hGetAnglesFromVector( %vec )
{
	%yawAng = mAtan( getWord(%vec,0), getWord(%vec,1) );

	if( %yawAng < 0.0 )
	{
		%yawAng += $pi*2;
	}
	
	return %yawAng;
}

function AIPlayer::hResetHeadTurn( %obj )
{
	%obj.setHeadAngleSpeed( 0.5 );
	%obj.setHeadAngle( 0 );
	
	%obj.hHeadTurn = 0;
}

function AIPlayer::hRandomHeadTurn( %obj )
{
	// 3.14159
	%rand = getRandom( 0, 3141 );
	%rand = %rand/1000;
	
	if( getRandom( 0, 1 ) )
		%rand = -%rand;
		
	%speed = hGetRandomFloat( 02, 10 );
	%obj.setHeadAngleSpeed( %speed );
		
	%obj.setHeadAngle( %rand );
	
	%obj.hHeadTurn = 1;
}

//Follow player command, can be called in and out of the hole loop
function AIPlayer::hFollowPlayer( %obj, %targ, %inHoleLoop, %skipAlert )
{
	//If the target doesn't exist or we're dead stop
	if(!isObject(%targ) || %obj.getstate() $= "Dead" )
		return;
	
	// if we can no longer damage the thing we're following let's stop following them
	if( !miniGameCanDamage( %obj, %targ ) || %targ.isProtected() )
	{
		%obj.hFollowing = 0;
		
		if( %obj.hEmote )
			%obj.emote(wtfImage);
				
		%obj.hClearMovement();
		
		return;
	}
	
	%objPos = %obj.getPosition();
	%targPos = %targ.getPosition();
	
	%targDist = vectorDist( %objPos, %targPos );
	
	%mount = %obj.getObjectMount();
	
	if( %mount )
		%driver = %mount.getControllingObject();//%mount.getMountNodeObject( 0 );
	else
		%driver = 0;
	
	%obj.playthread(3,root);
	cancel(%obj.hSched);
   %obj.hSched = 0;
	%obj.setMoveObject(%targ);
	// %obj.setMoveY(1);
	%obj.setMoveSlowdown( %obj.hMoveSlowdown );
	%obj.clearAim();
	// %obj.mountImage(hJumpImage,2);
	// %obj.mountImage(hCrouchImage,3);
	%obj.setImageTrigger(0,0);
	%obj.hResetMoveTriggers();
	%obj.maxYawSpeed = 10;
	%obj.maxPitchSpeed = 10;
	
	
	%scale = getWord(%obj.getScale(),0);

	// if(%obj.getDataBlock().hCustomFollow)
	%obj.getDataBlock().onBotFollow(%obj,%targ);

	//Remember who we're following
	%obj.hFollowing = %targ;
	//If we don't have a gun, and we can't melee PANIC
	%obj.hIsRunning = 0;
	
	if( ( !%obj.getMountedImage(0) || %obj.getMountedImage(0).nonLethal ) && !%obj.hMelee)
	{
		%obj.setMoveObject(0);
		%obj.clearMoveY();
		%obj.clearAim();
		%obj.hIsRunning = 1;
		%obj.hRunAwayFromPlayer( %targ );
		
		if( !getRandom( 0, 3 ) )
			%obj.hSpazzClick( 0, 1 );
	}
	//Good for when you have the bots really far away and you don't want them jumping like retards
	if( %obj.hSpazJump && %targDist <= 100*%scale )//vectorDist(%obj.getPosition(),%targ.getPosition()) <= 100*%scale)
	{
		%objZ = getword(%obj.getPosition(),2);
		%targZ = getword(%targ.getPosition(),2);
		if(%objz+1 < %targZ )//|| getrandom(0,2) == 0)
		{
			%obj.setJumping(1);
		}
	}
	//Play the Alarm Emote only when first finding a target
	if( %obj.hState !$= "Following" && %obj.hEmote && !%skipAlert && getSimTime() > %obj.hLastSpawnTime+3000)
	{
		%obj.emote(alarmProjectile);
	}
	//Tell the other bots you've been shot
	if(%obj.hAlertOtherBots && !%skipAlert)
	{
		%obj.hAlertTeammates( %targ );
	}

	%obj.hState = "Following";
	//Should I randomly strafe? Distance check is needed to avoid them circling corpses
	if(%obj.hStrafe && %targDist > 6*%scale )//vectorDist(%obj.getPosition(),%targ.getPosition()) > 6*%scale)
	{
		%obj.hSetMoveX(getrandom(-1,1));
	}
	//If there's no maxShootRange set, set the default one. Whenever a player gives a bot to a weapon it should work.
	if(!%obj.hMaxShootRange)
		%obj.hMaxShootRange = 42;
	
	// set the engage distance to hMaxShootRange, if we have a melee weapon set it to melee distance
	%obj.setEngageDistance( brickToMetric( %obj.hMaxShootRange )*%scale );
	
	if( %obj.getMountedImage(0).melee )
		%obj.setEngageDistance( ( 10 + getRandom(-2,2 ) )*%scale );
	
	//Giant shooting chunk, if we have weapon, can shoot etc
	if(!%obj.hIsRunning && %obj.hShoot && isObject(%obj.getMountedImage(0)) && brickToMetric( %obj.hMaxShootRange )*%scale >= %targDist && hLOSCheck(%obj,%targ,1))//vectorDist(%obj.getPosition(),%targ.getPosition())
	{
		cancel(%obj.hShotSched);
      %obj.hShotSched = 0;
		%obj.hShootTimes = %obj.getDataBlock().hShootTimes;
		%tickRate = %obj.getDataBlock().hTickRate;
		
		//Again if these values don't exist, create them
		if(!%obj.hShootTimes)
			%obj.hShootTimes = 4;
		if(!%tickRate)
			%tickRate = 3000;
		
		//if trying to shoot too many times within tick fix it
		if(%tickRate/%obj.hShootTimes < 275)
		{
			%obj.hShootTimes = mFloatLength(%tickRate/275,0);
		}
		%imageName = %obj.getMountedImage(0).getName();
		//Special case for spear
		if( %obj.getMountedImage(0).isChargeWeapon || %obj.getDataBlock().isChargeWeapon || %imageName $= "spearImage" || %imageName $= "hTurretImage")
		{
			%obj.hShootTimes = 1;
		}

		//Shoot the set amount of times within the tick
		%nIterate = %tickRate/%obj.hshootTimes;
		for(%a = 0; %a < %obj.hshootTimes; %a++)
		{
			%obj.hShotSched = %obj.scheduleNoQuota( %a*%nIterate, hShootPrediction, %targ, %tickRate, %obj.hShootTimes );
		}
		
		//Avoid close range fights if you don't have melee
		if(%obj.hAvoidCloseRange)
		{
			//convert to brick units
			%range = brickToMetric( %obj.hTooCloseRange )*%scale;
			
			//Make the bot backpedal if he's too close and the weapon he's using isn't melee based.
			if( %targDist <= %range && !%obj.getMountedImage(0).Melee)//vectordist(%obj.getPosition(),%targ.getPosition())
			{
				// %obj.setMoveY(-0.5);
				%obj.setMoveY( -%obj.hMaxMoveSpeed/2 );
			}

			//even if it's melee, still don't want to be point blank most of the time
			if(%obj.getMountedImage(0).melee)
			{
				%range = 2;
				if( %targDist <= %range)// vectordist(%obj.getPosition(),%targ.getPosition())
				{
					%obj.setMoveY(-0.1);
				}
			}	
		}
	}
	
	// if(%obj.hAvoidObstacles)
	// {
	%obj.hAvoidObstacle();
	// }

	//Record the player's last position, most likely not needed but it sometimes might make it function better
	%obj.hLastPosition = %obj.getPosition();
	//Record if we saw the player this tick
	%obj.hSawPlayerLastTick = hLOSCheck(%obj,%targ,1);
	
	//Check if we're in or out of the hole loop, if out that means we have to restart the schedule
	if(!%inHoleLoop)
	{
		%obj.inHoleLoop = 0;
		
		%tickrate = %obj.getDatablock().hTickRate;
		if(!%tickrate)
		{
			%tickrate = 3000;
		}
		%obj.hSched = %obj.scheduleNoQuota(%tickrate*1.75,hLoop);
	}
	
	//Swimming checks, uses empty images, not sure if this is the best way to do this
	if(%obj.getWaterCoverage())
	{
		%objZ = getWord(%obj.getPosition(),2);
		%targZ = getWord(%targ.getPosition(),2);

		if(%objZ < %targZ && %targZ-%objZ >= 2)
		{
			%obj.setJumping(1);
			%obj.setCrouching(0);
		}
		else if(%objZ-%targZ >= 2)
		{
			%obj.setJumping(0);
			%obj.setCrouching(1);
		}
	}
	
	// jet check, this is going to be bad most likely
	if( %obj.getDataBlock().canJet )
	{
		%objZ = getWord(%obj.getPosition(),2);
		%targZ = getWord(%targ.getPosition(),2);
		
		if( %targZ > %objZ+5 || %obj.hIsFalling() )//getWord( %obj.getVelocity(), 2 ) < 2 )
		{
			// %obj.setJetting(1);
			
			%obj.hJump();
			// %obj.hJetCheck(1);
			%obj.hHasJetted = 1;
			%obj.setJetting(1);
			%obj.setMoveSpeed( 0.2 );
			%obj.clearMoveX();
		}
		else if( %obj.hHasJetted )
			%obj.hJetCheck(1);
		else
			%obj.hHasJetted = 0;
		// else
			// %obj.hJetCheck();
			// %obj.hStopJetting();
			// %obj.setJetting(0);
	}
	
	if( %mount )
	{
		if( %driver != %obj && %targDist < brickToMetric( 32 ) || %targ.getObjectMount() == %mount )// !%driver || checkHoleBotTeams( %obj, %driver ) )
			%obj.dismount();
	}
	
	// if we're in a vehicle randomly "break" when close to target
	if( %mount && !getRandom( 0, 1 ) && %targDist < 32 && !( %mount.getType() & $TypeMasks::PlayerObjectType ) )
		%obj.hJump( 150+getRandom(0,150) );
}

//Main bot loop, called every tick, tick is usually every 3 seconds, every bot uses this function in some way.
function AIPlayer::hLoop(%obj)
{
	//if we somehow got here and you or your spawnbrick doesn't exist anymore, then poof
	if( !isObject(%obj.spawnBrick) ) // isObject(%obj) && 
	{
		%obj.delete();
		return;
	}	
	//Check if we exist, our spawnbrick exists, we're currently active and not dead, we need to exist
	if( !isObject(%obj.spawnbrick) || %obj.isDisabled() || !%obj.hLoopActive ) // !isObject(%obj) || .getState() $= "Dead" 
		return;
		
	%mount = %obj.getObjectMount();
	
	// if we're in water set our move tolerance to be a bit higher
	if( %mount && !( %mount.getType() & $TypeMasks::PlayerObjectType ) )
		%obj.setMoveTolerance( 12 );
	else if( %obj.getWaterCoverage() != 0 || %mount )
		%obj.setMoveTolerance( 3 );
	else
		%obj.setMoveTolerance( 0.25 );
	
	//Lay out all the options we have so we can alter them as the function progresses
	%obj.inHoleLoop = 1;
	%wander = %obj.hWander;
	%gridWander = %obj.hGridWander;
	%search = %obj.hSearch;
	%strafe = %obj.hStrafe;
	%tickrate = %obj.getDatablock().hTickRate;
	%spastic =  %obj.hSpasticLook;
	%idleAnim = %obj.hIdleAnimation;
	%AFKScale = %obj.hAFKOmeter;
	%idle = %obj.hIdle;
	%scale = getWord(%obj.getScale(),0);

	if(!%AFKScale)
		%AFKScale = 1;
	
	%data = %obj.getDatablock();
	%canJet = %data.canJet;
	
	%spawnBrick = %obj.spawnBrick;
	%minigame = getMiniGameFromObject( %obj );// %obj.spawnBrick.getGroup().client.minigame;
	%minigameHost = %minigame.owner;// %obj.spawnBrick.getGroup().client.minigame.owner;
	%isHost = %obj.spawnBrick.getGroup().client == %minigameHost;
	%isIncluded = %minigame.useAllPlayersBricks;
	%respawnTime = %miniGame.botRespawnTime;
	%brickGroup = %obj.spawnBrick.getGroup();

	// let's figure out when to get out of the vehicle
	if( %mount )
	{
		// %driver = %mount.getMountNodeObject( 0 );
		%driver = %mount.getControllingObject();
		
		if( !%driver && !getRandom( 0, 2 ) )
			%mount.mountObject( %obj, 0 );
		else if( !%driver || checkHoleBotTeams( %obj, %driver ) )
			%obj.dismount();
		else if( %idle && %driver != %obj && !getRandom( 0, 4 ) )
		{
			if( getRandom( 0, 1 ) )
				serverCmdNextSeat( %obj );
			else
				serverCmdPrevSeat( %obj );
		}
	}
	
	%obj.playThread(3,root);
	if(%obj.getDataBlock().getName() $= "CannonTurret" || %obj.getDataBlock().getName() $= "TankTurretPlayer" || %obj.getDataBlock().isTurret)
	{
		%isTurret = 1;
		%obj.hShoot = 1;
		%obj.mountImage(hTurretImage,0);
	}
	else
		%isTurret = 0;
		
	//If we have certain idle actions activate this function gives you back your weapon/fixes your hands
	if(%obj.hIsSpazzing)
	{
		if( isObject(%obj.hLastWeapon) )
		{
			%obj.setWeapon( %obj.hLastWeapon );
			fixArmReady(%obj);
			// %obj.mountImage(%obj.hLastWeapon,0);
			// if( isObject(%obj.hLastWeaponL) )
			// {
				// %obj.mountImage(%obj.hLastWeaponL,1);
			// }
		}
		else
		{
			%obj.unMountImage(0);
			fixArmReady(%obj);
		}
		%obj.hIsSpazzing = 0;
		// fixArmReady(%obj);
		
		if( strlen( %obj.hDefaultThread ) )
			%obj.playThread(1,%obj.hDefaultThread);
	}
	else if( isObject( %obj.getMountedImage(0) ) )
	{
		%obj.hLastWeapon = %obj.getMountedImage(0);
			
		// if( isObject(%obj.getMountedImage(1)) )
		// {
			// %obj.hLastWeaponL = %obj.getMountedImage(1);
		// }
	}

	//If no tickrate create one, useful for when bots are changed into other datablocks ie horse
	if(!%tickrate)
	{
		%tickrate = 3000;
	}
	// %customLoop = %obj.getDatablock().hCustomLoop;
	
	//if our user isn't in the server, teleport back to spawn and stop doing things
	// if(!isObject(%obj.spawnBrick.getGroup().client))
	if( %spawnBrick.itemPosition != 1 && !hBrickClientCheck( %brickGroup ) )
	{
		//%pos = %obj.spawnBrick.getPosition();
		//%pos = vectoradd(%pos,"0 0 0.15");
		//%rot = getwords(%obj.spawnBrick.getTransform(),3,6);
		// other options delete the bot until user comes back? ##
		//%obj.setTransform(%pos SPC %rot);

		//%obj.stopHoleLoop();
		%obj.delete();
		//%obj.hSched = %obj.scheduleNoQuota(%tickrate+getrandom(0,750),hLoop);
		return;
	}
	
	//If spawnClose is set, check if we've seen a player, if we don't see one for 3 ticks then poof ourselves
	if(%obj.getDatablock().hSpawnClose)
	{
		%sCheck = doHoleSpawnDetect(%obj);
		if(!%sCheck)
			%obj.hCSpawnD++;
		else
			%obj.hCSpawnD = 0;

		if(%obj.hCSpawnD >= 1) //egh: changing this to 1 so they unspawn faster
		{
			%spawnBrick = %obj.spawnBrick;
         %spawnBrick.unSpawnHoleBot();
			cancel(%spawnBrick.hSpawnDetectSchedule);
			%spawnBrick.hSpawnDetectSchedule = %spawnBrick.scheduleNoQuota( 5000, spawnHoleBot );
			return;
		}
	}
	
	//Again clear movement and reattach jump/crouch images, there is probably a better way to do this
	// %obj.mountImage(hJumpImage,2);
	// %obj.mountImage(hCrouchImage,3);
	%obj.clearMoveY();
	%obj.clearMoveX();
	//%obj.clearAim();
	%obj.hResetMoveTriggers();
	%obj.maxYawSpeed = 10;
	%obj.maxPitchSpeed = 10;
	
	// check if we're a jet datablock and we're falling to our death
	%obj.hJetCheck();
	
	if(%search)
	{	
		//Radius Check
		if(isObject(%minigame) && %obj.hSearchFOV != 1)
		{
			if(%isHost || %isIncluded)
			{
				%target = %obj.hFindClosestPlayer();

				if( isObject( %target ) && hLOSCheck( %obj, %target ) && !%target.isProtected() )// && vectorDist(%target.getPosition(),%obj.getPosition()) <= %obj.hFinalRadius  && hLOSCheck(%obj,%target))
				{
					//if we've found someone set wander to 0 so we don't do any idle actions or wander around
					%wander = 0;
					%idle = 0;
					%obj.hFollowPlayer( %target, 1 );
				}
				else if( %obj.hIgnore != %obj.hFollowing && %obj.hSawPlayerLastTick && isObject(%obj.hFollowing) && !%obj.hFollowing.isDisabled() && checkHoleBotTeams(%obj,%obj.hFollowing, 1))
				{
					if(%obj.hIsRunning)
						%chaseDist = 64;
					else
						%chaseDist = 128;

					if(vectorDist(%obj.getPosition(),%obj.hFollowing.getPosition()) <= %chaseDist*%scale)
					{
						//We saw someone last tick so we should go to where we last saw him/towards him. Might redo the way this works
						%wander = 0;
						%idle = 0;
						%obj.hFollowPlayer( %obj.hFollowing, 1 );
					}
				}
			}
		}
		//Fov Check
		if(isObject(%minigame) && %obj.hSearchFOV)
		{
			if(%isHost || %isIncluded)
			{
				if( %obj.hIgnore != %obj.hFollowing && %obj.hSawPlayerLastTick && isObject(%obj.hFollowing) && !%obj.hFollowing.isDisabled() && checkHoleBotTeams(%obj,%obj.hFollowing, 1))
				{
					if(%obj.hIsRunning)
						%chaseDist = 64;
					else
						%chaseDist = 128;

					if(vectorDist(%obj.getPosition(),%obj.hFollowing.getPosition()) <= %chaseDist*%scale)
					{
						//We saw someone last tick so we should go to where we last saw him/towards him. Might redo the way this works
						%wander = 0;
						%idle = 0;
						%obj.hFollowPlayer( %obj.hFollowing, 1 );
					}
				}

				if(%obj.hDoHoleFOVCheck(%obj.hFOVRadius,1,1))
				{
					%wander = 0;
					%idle = 0;
				}
				else
				{
					cancel(%obj.hFOVSchedule);
					%obj.hFOVSchedule = %obj.scheduleNoQuota(%tickRate/2,hDoHoleFOVCheck,%obj.hFOVRadius,0,1);
				}
			}
		}
	}

	//If there's a custom script tag on the datablock, then call the function
	// if(%customLoop)
	%obj.getDataBlock().onBotLoop(%obj);

	//If we have a brick target that takes precedence over wandering
	if(isObject(%obj.hPathBrick) && %obj.hState !$= "Following")
	{	
		//Again ghetto clear movement things
		%obj.setMoveObject("");
		%obj.clearMoveY();
		%obj.clearMoveX();
		%obj.clearAim();
		%obj.hResetHeadTurn();
		
		// %obj.hResetMoveTriggers();

		%obj.hState = "Pathing";
		
		%tPos = getWords(%obj.hPathBrick.getPosition(),0,1) SPC 0;
		%pos = getWords(%obj.getPosition(),0,1) SPC 0;
		
		//Ok we've landed at the brick, er.. well close enough
		// if(vectorDist(%pos,%tPos) <= 3*%scale)
		// {
			// %lastBrick = %obj.hPathBrick;
			// %obj.hPathBrick = 0;
			// %lastBrick.onBotReachBrick(%obj);
			// %obj.lastBrickReachTime = getSimTime()+500;//No idea why this is here, I must of had something planned for it
			// //%obj.hSched = %obj.scheduleNoQuota(%tickrate/2+getrandom(0,750),hLoop);
			// %obj.scheduleNoQuota(100,hLoop);
			// return;
		// }
		
		
		
		if( %canJet )
		{
			%brickZ = %obj.hPathBrick.hGetPosZ();
			%objZ = %obj.hGetPosZ();
			
			// echo( %brickZ SPC %objZ+3 );
			
			if( %brickZ > %objZ+18 )
				%obj.hLongJet();
			else if( %obj.hHasJetted || %brickZ > %objZ+2 )
				%obj.hJetCheck(1);
				
			%obj.maxYawSpeed = 20;
			// else if( !getRandom( 0, 2 ) && !%obj.hIsFalling() )
			// {
				// %obj.hJump( 100 );
				// %obj.hJet( 500+getRandom(0,300) );
			// }
			
			if( %obj.isJetting() && vectorDist(%pos,%tPos) <= 6*%scale)
			{
				// echo( "close" );
				%obj.setMoveSpeed( 0.1 );
			}
		}
			
		// setMoveSlowdown should govern this as well
		%obj.setMoveDestination( getwords(%obj.hPathBrick.getPosition(),0,1) SPC getword(%obj.getPosition(),2) );

		// %obj.setAimLocation( %obj.hPathBrick.getPosition() );
		
		//If people disable %wander then we assume they don't want the bot to be doing much, so don't avoid obstacles
		if(%obj.hAvoidObstacles && %wander)
		{
			%obj.hAvoidObstacle();
		}
		
		%obj.hSched = %obj.scheduleNoQuota(%tickrate+getrandom(0,750),hLoop);
		return;
	}
	if(%wander)
	{
		%obj.hIsRunning = 0;
		//Er again, but still...
		%obj.setMoveObject("");
		%obj.clearMoveY();
		%obj.clearMoveX();
		//%obj.clearAim();
		%obj.setImageTrigger(0,0);
		%obj.hResetHeadTurn();
		
		// %obj.hResetMoveTriggers();
		//%obj.stopThread(1);
		
		%pos = %obj.getPosition();
		
		//Again converting to brick units, easier for players
		%returnDist = brickToMetric( %obj.hSpawnDist );//2-0.25;

		%avoid = 0;
		
		
		
		//if 0 we assume they want to always return to brick
		// if returnDistance is zero then our bot will always try to get back to spawn since the distance between bot and spawn is never truly zero
		if(%returnDist <= 0)
			%returnDist = 1.6;//1.6;
			
		%returnDist = %returnDist*%scale;
		
		if( %returnDist < 1 )
				%returnDist = 1;

		// perhaps we should make this trigger onReachBrick when we get back?
		//We're too far let's go back.. it's safe there
		if( %obj.hReturnToSpawn && vectordist( %pos, %obj.spawnBrick.getPosition() ) > %returnDist )
		{	
			// if %obj.hSpawnDist is zero that means we will never random wander, so let's set a flag that will reset our rotation to our spawnbrick when we arrive
			if( %obj.hSpawnDist == 0 )
				%obj.hIsGuard = 1;
			
			%brickPos = %obj.spawnBrick.getPosition();
			%brickZ = hGetZ( %brickPos );
			%objZ = %obj.hGetPosZ();
			
			if( vectorDist(%pos,%brickPos) <= 6*%scale )
				%obj.setMoveSpeed( 0.2 );
			
			if( %canJet )
			{
				if( %brickZ > %objZ+18 )
					%obj.hLongJet();
				else if( %obj.hHasJetted || %brickZ > %objZ+2 )
					%obj.hJetCheck(1);
					
				if( %obj.isJetting() && vectorDist(%pos,%brickPos) <= 6*%scale )
					%obj.setMoveSpeed( 0.1 );
			}
			// else if( !getRandom( 0, 2 ) && !%obj.hIsFalling() )
			// {
				// %obj.hJump( 100 );
				// %obj.hJet( 500+getRandom(0,300) );
			// }
			
			
			
			%obj.clearAim();
			%obj.hFollowing = "";
			%obj.setMoveDestination( %brickPos );
			%obj.hState = "Returning";
			%obj.hAvoidObstacle();

			%obj.hSched = %obj.scheduleNoQuota(%tickrate+getrandom(0,1000),hLoop);
			return;
		}
		else if( %obj.hReturnToSpawn && %obj.hIsGuard )
		{
			%posSpawn = %spawnBrick.getPosition();
			
			// check that we've fully returned to spawn
			if( vectordist( %pos, %posSpawn ) <= %returnDist )
			{
				// ok we've returned so let's look forward and turn off hIsGuard	
				%obj.maxYawSpeed = getRandom( 5, 10 );
				%obj.maxPitchSpeed = getRandom( 5, 10 );
				
				// %obj.setAimLocation( %loc );
				%obj.setAimVector( %spawnBrick.getForwardVector() );
				%obj.hIsGuard = 0;
				
				// call onBotReachBrick event
				%spawnBrick.onBotReachBrick(%obj);
			}
		}
		
		// randomly jet
		if( %obj.hSpawnDist != 0 && !getRandom( 0, 4 ) && !%obj.hIsFalling() )
		{
			%obj.hJump( 100 );
			
			// randomly do a long safe jump, or a short jump
			if( !getRandom( 0, 2 ) )
				%obj.hJet( %tickrate-500 );
			else
				%obj.hJet( 1000+getRandom(0,500) );
		}
		
		
		//If we just saw a target, better act fast and irrational
		if(%obj.hState $= "Following" && %obj.hSpawnDist != 0)
		{
			if(isObject(%obj.hFollowing) && %obj.hFollowing.getState() $= "Dead")
			{
				//do some sort of victory dance maybe?
				if(%obj.hEmote)
				{
					%obj.emote(loveImage);
				}
				%obj.hFollowing = 0;
			}
			else
			{
				%obj.clearAim();
				%xRand = getrandom(-15,15);
				%yRand = getrandom(-15,15);
				
				%obj.setMoveDestination(vectoradd(%pos,%xRand SPC %yRand SPC 0));
				%obj.hAvoidObstacle();
			
				%obj.hState = "Wandering";
				if(%obj.hEmote)
				{
					%obj.emote(wtfImage);
				}
				//echo("From going to wander from following");
				%obj.hFollowing = "";
				%obj.hSched = %obj.scheduleNoQuota(%tickrate/2,hLoop);
				return;
			}
		}

		//Already did return check and lost target check, set to wander
		%obj.hState = "Wandering";
		
		//If spawn distance is set to 0 we don't randomly wander. We assume the user wants them to act as guard bots
		//Grid walking
		if(%obj.hGridWander && getRandom(-1,mFloatLength(1*%AFKScale,0)) <= 0 && %obj.hSpawnDist != 0)
		{
			%avoid = 1;
			%noStrafe = 1;
			%onlyJump = 1;
			%idle = 0;

			%obj.clearAim();
			%obj.hFollowing = "";
			
			//%sPos = %obj.getPosition();
			//%sPos = %obj.hGridPosition;
			%sPos = lockToGrid(%obj, %obj.getPosition());

			if(getRandom(0,1))
			{
				%rand = getRandom(-4,4)*2*%scale;
				%tPos = vectorAdd(%sPos, %rand SPC "0 0");
			}
			else
			{
				%rand = getRandom(-4,4)*2*%scale;
				%tPos = vectorAdd(%sPos, "0" SPC %rand SPC "0");
			}
			
			%pos = %tPos;
			//%pos = lockToGrid(%obj, %tPos);
			%obj.setMoveDestination(%pos);
			//%obj.hGridPosition = %pos;

			if(getRandom(0,4) > 0)
				%obj.scheduleNoQuota( %tickrate/2, hDetectWall );
		}
		//Smooth wandering
		else if(%obj.hSmoothWander && !%obj.hGridWander && !getRandom(0,mFloatLength(2*%AFKScale,0)) && %obj.hSpawnDist != 0)
		{
			%avoid = 1;
			%idle = 0;
			
			// randomly crouch
			if( !getRandom(0,3) ) 
				%obj.setCrouching(1);
			else if( !getRandom(0,1) ) 
				%obj.setCrouching(0);

			%obj.smoothWander();

			// randomly go up or down in water
			if(%obj.getWaterCoverage())
			{
				if(getRandom(1,4) == 1)
				{
					%obj.hRandomMoveTrigger();
				}
			}

			//3/4 chance that the bot will face away from a wall
			if( getRandom(0,3) > 0 )
				%obj.scheduleNoQuota( %tickrate/2, hDetectWall );
		}
		//More conventional bot movement, mixing between the two creates a good variety of movement
		else if(!%obj.hGridWander && !getrandom(0,mFloatLength(2*%AFKScale,0)) && %obj.hSpawnDist != 0)
		{
			%idle = 0;
			%avoid = 1;
			//Strafing is disbaled from the avoidance function to prevent the bot from walking around a point
			%noStrafe = 1;
			
			//Random crouching, works surprisingly well
			if(!getRandom(0,3)) 
				%obj.setCrouching(1);
			else if(!getRandom(0,1)) 
				%obj.setCrouching(0);

			%obj.maxYawSpeed = getRandom(3,10);
			%obj.maxPitchSpeed = getRandom(3,10);
			%obj.clearAim();
			%xRand = getrandom(-7,7);
			%yRand = getrandom(-7,7);
			%obj.hFollowing = "";

			%rFinal = vectorAdd(%pos,%xRand SPC %yRand SPC 0);
			%obj.setMoveDestination(%rFinal);
				
			%dif = vectorScale(vectorSub(%rFinal,%pos),8);
			%aim = vectorAdd(%pos, getWords(%dif,0,1) SPC getRandom(-8,8));
			//%sub = vectorSub(%pos,%rFinal);

			//%aim = vectorAdd(%rFinal,%sub);
			// Have to look into this, appears to be a function to make the bot randomly look in a direction while moving
			// but would only be called under weird circumstance ##
			if(%xRand && %yRand)
			{
				%obj.setAimLocation(%aim);
			}

			if(getRandom(0,4) > 0)
				%obj.scheduleNoQuota( %tickrate/2, hDetectWall );
			
			//Randomly go up or down in water
			if(%obj.getWaterCoverage())
			{
				if(getRandom(1,4) == 1)
				{
					%obj.hRandomMoveTrigger();
				}
			}
		}

	}
	
	// head turn is out of all
	// random head turn, only if hIdleLookAtOthers is enabled && !%obj.hHeadTurn 
	if( %idle && !%isTurret && %obj.hIdleLookAtOthers && !getRandom( 0, mFloatLength(4*%AFKScale,0) ) )
		%obj.hRandomHeadTurn();
	else //if( !getRandom( 0, 1 ) )
		%obj.hResetHeadTurn();
	
	// randomly reset the being sprayed flag so the bot doesn't just spray all the time
	if( !getRandom( 0, 2 ) )
		%obj.hIsBeingSprayed = 0;
	
	if(%idle || ( %obj.hIsBeingSprayed && %obj.hIdle && %obj.hIdleSpam ))
	{
		%obj.hIsRunning = 0;
		//Ok if we're here then we're going to stand still and screw around for a bit
		%avoid = 1;
		%noStrafe = 1;
			
		//If we're screwing around on the grid we don't want to have the chance of being in "stuck mode", we should always stay on the grid based from the spawnbrick
		if(%gridWander)
			%onlyJump = 1;

		// %obj.clearAim();
		%obj.hFollowing = "";
		%obj.maxYawSpeed = 10;
		%obj.maxPitchSPeed = 10;
		
		%isLookingAtPlayer = 0;
		
		//Randomly look at other bots
		if( !%isTurret && %obj.hIdleLookAtOthers && ( !getRandom( 0, mFloatLength(2*%AFKScale,0) ) || %obj.hIsBeingSprayed ) && isObject( (%targ = %obj.hReturnCloseBlockhead()) ) )
		{
			if( %obj.hIsBeingSprayed )
				%targ = %obj.hIsBeingSprayed;
				
			%isLookingAtPlayer = 1;	
			
			%obj.setAimObject(%targ);
				
			%alreadyLooked = 1;
			//Randomly follow the other bot/player
			if(!getRandom(0,mFloatLength(1*%AFKScale,0)))
			{
				if(%obj.hSpawnDist != 0 && %wander && !%obj.hGridWander )
					%obj.setMoveY( %obj.hMaxMoveSpeed*0.75 );
					// %obj.setMoveY(0.5);
					
				// //If this idle behavior is enabled, randomly spray or hammer the bot/player
				// if(%obj.hIdleSpam && ( !getRandom(0,mFloatLength(1*%AFKScale,0)) || %obj.hIsBeingSprayed ) )
				// {
					// %spamChoice = getRandom(0,4);
					
					// if( %obj.hIsBeingSprayed )
					// {
						// %spamChoice = 0;
						// %obj.hIsBeingSprayed = 0;
					// }
					
					// if( isObject( %obj.getMountedImage( 0 ) ) )
					// {
						// %obj.unMountImage(0);
						// fixArmReady( %obj );
					// }
					// %obj.unMountImage(1);
					// //%obj.playThread(1,root);

					// if(%spamChoice == 0) 
						// serverCmdUseSprayCan(%obj,getRandom(0,27));

					// else if(%spamChoice == 1) 
						// %obj.mountImage(hammerImage,0);

					// else if(%spamChoice == 2) 
					// {
						// %obj.mountImage(printGunImage,0);
						// %multiShoot = 1;
					// }
					// else if(%spamChoice == 3) 
					// {
						// %obj.mountImage(brickImage,0);
						// %multiShoot = 1;
					// }
					// else if(%spamChoice == 4) 
					// {
						// %obj.mountImage(wrenchImage,0);
						// %multiShoot = 1;
					// }
					// if(%multiShoot)
					// {
						// %shootTimes = getRandom(1,4);
						// %nIterate = %tickRate/%shootTimes;
						// for(%a = 0; %a < %shootTimes; %a++)
						// {
							// %obj.hShotSched = %obj.scheduleNoQuota( %a*%nIterate, hShootPrediction, "none", %tickRate, 4, 1 );
						// }
					// }
					// else
						// %obj.setImageTrigger(0,1);

					// fixArmReady(%obj);
					// //Set is spazzing to 1 so that we know not to spam click later
					// %obj.hIsSpazzing = 1;
				// }
			}
		}
		//Ok we're not going to randomly look at another bot, let's randomly look around anyway... maybe
		else
		{
			if(getRandom(-3,mFloatLength(2*%AFKScale,0)) <= 0 && %obj.hSpasticLook)
				%obj.hLookSpastically();

			if(getRandom(-3,mFloatLength(2*%AFKScale,0)) <= 0 && %obj.hSpasticLook) 
				%spazLookSchedule = %obj.scheduleNoQuota( %tickrate/4+getRandom( 0, 500 ), hLookSpastically );
		}
			
		if( %isLookingAtPlayer == 0 )
			%obj.hIsbeingSprayed = 0;
			
		//If idle spam is enabled but idle look is not, do this
		if(!%isTurret && %obj.hIdleSpam && ( !getRandom(0,mFloatLength(2*%AFKScale,0)) || %obj.hIsBeingSprayed ) ) //  !%obj.hIdleLookAtOthers &&
		{
			%spamChoice = getRandom(0,4);
					
			if( %obj.hIsBeingSprayed )
			{
				%spamChoice = 0;
				%obj.hIsBeingSprayed = 0;
			}
		
			if( isObject( %obj.getMountedImage( 0 ) ) )
			{
				%obj.unMountImage(0);
				fixArmReady( %obj );
			}
			%obj.unMountImage(1);

			if(%spamChoice == 0) 
				serverCmdUseSprayCan(%obj,getRandom(0,27));

			else if(%spamChoice == 1) 
				%obj.mountImage(hammerImage,0);

			else if(%spamChoice == 2) 
			{
				%obj.mountImage(printGunImage,0);
				%multiShoot = 1;
			}
			else if(%spamChoice == 3) 
			{
				%obj.mountImage(brickImage,0);
				%multiShoot = 1;
			}
			else if(%spamChoice == 4) 
			{
				%obj.mountImage(wrenchImage,0);
				%multiShoot = 1;
			}
			if(%multiShoot)
			{
				%shootTimes = getRandom(1,4);
				%nIterate = %tickRate/%shootTimes;
				for(%a = 0; %a < %shootTimes; %a++)
				{
					%obj.hShotSched = %obj.scheduleNoQuota( %a*%nIterate, hShootPrediction, "none", %tickRate, 4, 1 );
				}
			}
			else
				%obj.setImageTrigger(0,1);
				
			fixArmReady(%obj);
			//Set is spazzing to 1 so that we know not to spam click later
			%obj.hIsSpazzing = 1;
		}

		%obj.hIsBeingSprayed = 0;
		
		//Since we're already screwing around maybe we should sit or spam an emoete
		if(!%isTurret && %obj.hIdleAnimation && !getRandom(0, mFloatLength(1*%AFKScale,0) ) )
		{
			//%obj.clearAim();$holeBotIdleGestures[$hBotGest
			%anim = $holeBotIdleGestures[getRandom(0,$hBotIdGest)];
			if(!getRandom(0,5))
			{
				serverCmdSit(%obj);
			}
			if(%anim $= "crouch")
			{
				%obj.setCrouching(1);
			}
			else if(%anim $= "sit")
			{
				serverCmdSit(%obj);
			}
			else
			{
				%obj.playthread(3,%anim);
				if(%anim $= "love")
					%obj.emote("loveImage");

				if(%anim $= "wtf")
					%obj.emote("wtfImage");

				if(%anim $= "alarm")
					%obj.emote("alarmProjectile");

				//If we get the bricks emote, it's only right that we look at the ground like all 90% of blocklanders do when they pick blocks
				if(%anim $= "Bricks")
				{
					if( !getRandom(0,2) )
					{
						cancel(%spazLookSchedule);
						%pos = %obj.getPosition();
						%vec = %obj.getForwardVector()*2;
						%loc =  vectorAdd(%vec,"0 0 -1");

						%obj.setAimLocation( vectorAdd(%pos, %loc));
					}
					%obj.scheduleNoQuota(300,emote,"BSDProjectile");
				}
			}
		}
		%obj.hState = "Wandering";
	}
	
	%obj.hIsBeingSprayed = 0;
	
	//Pop quiz time, did anyone notice I misspelt emote a few lines back.. er not that it's really a word
	if(%obj.hSpawnDist != 0 && !%isTurret && %avoid && %wander) 
		%obj.hAvoidObstacle( %noStrafe, %onlyJump );
	
	//Spazz click check, blocklanders do it why not bots?
	if(!%isTurret && %wander && %obj.hIdle && %obj.hIdleSpam && !getRandom(0,mFloatLength(5*%AFKScale,0)) ) 
	{
		if( isObject(%obj.getMountedImage(0)) )
		{
			%obj.unMountImage(0);

			if( isObject(%obj.getMountedImage(1)) )
			{
				%obj.unMountImage(1);
			}
			//%obj.playthread(1,root);
		}
		fixArmReady(%obj);
		%obj.hIsSpazzing = 1;
		%obj.hSpazzClick();
	}
	
	//hmm well apparently emote is an official word, or at least far as I can tell, well it's in the dictionary anyway
	%obj.hSched = %obj.scheduleNoQuota(%tickrate+getrandom(0,1000),hLoop);
	return;
}

//Radius check for bots/players, does team checks returns target
function AIPlayer::hFindClosestPlayer(%bot)
{
	%type = $TypeMasks::PlayerObjectType;
	%pos = %bot.getPosition();
	%scale = getWord(%bot.getScale(),0);
	//%radius = %bot.getDatablock().hSearchRadius/2+0.25;
	// Need to re-organize this definitely ##
	if(%bot.hSearchRadius == 0)	
		%bot.hFinalRadius = 0;
	else	
		%bot.hFinalRadius = brickToRadius( %bot.hSearchRadius ) * %scale;

	if(%bot.hSearchRadius == -2)	
		%bot.hFinalRadius = 2000000000;
	
	initContainerRadiusSearch(%pos,%bot.hFinalRadius,%type);
	while((%target = containerSearchNext()) != 0)
	{
		%target = %target.getID();
		
		if(%bot != %target && !%target.isCloaked && %target.getState() !$= "Dead" && %bot.hIgnore != %target && checkHoleBotTeams(%bot,%target) && miniGameCanDamage( %target, %bot ) == 1)
		{
			return %target;
		}
	}
}

//FOV check, structured oddly
function AIPlayer::hDoHoleFOVCheck(%obj,%range,%a,%hear)
{
	%target = %obj.hFindClosestPlayerFOV( %range, %hear );
	
	if(isObject(%target))
	{
		%obj.hFollowPlayer( %target, %a );
		return 1;
	}

	// if we hear target
	if(%target == -1)
	{
		return 1;
	}
	
	return 0;
}

// check if the target is in our field of view
function AIPlayer::hFOVCheck( %bot, %player, %returnDot, %forwardVec )
{
	// get all required information
	if( %forwardVec )
		%botEye = %bot.getForwardVector();
	else
		%botEye = %bot.getEyeVector();
	
	%posBot = %bot.getPosition();
	%posPlayer = %player.getPosition();
	
	// draw a line between us and the player
	%line = vectorNormalize( vectorSub( %posPlayer, %posBot ) );
	
	// compare our eye to the line
	%dot = vectorDot( %botEye, %line );
	
	// return the dot product if they want it, otherwise return true or false
	if( %returnDot )
		return %dot;
	else
	{
		// this will return 0 or 1
		%fovCheck = %dot >= %bot.hFOVRange;
		
		// take into consideration if they want us to look backwards only?
		if( %bot.hFOVRange < 0 )
			%fovCheck = !%fovCheck;
			
		return %fovCheck;
	}
}

// return the closest player within our field of view
function AIPlayer::hFindClosestPlayerFOV( %bot, %range, %hear )
{
	%type = $TypeMasks::PlayerObjectType;
	
	//Need to scale it to the player
	%scale = getWord(%bot.getScale(),0);
	//FOV Search
	// %a = 1.2*%range*%scale;
	// %b = 1*%range*%scale;
	// %pos = vectorAdd(%bot.getPosition(),vectorScale(%bot.getEyeVector(),3.5*%scale+%a));//+1.5// default is 5 : 1//Switched to eye vector to see how it reacts RoTemp
	// %radius = %b;//+1
	
	%pos = %bot.getPosition();
	// take our bots current search radius into account
	// convert it to blockland studs
	// unsure if I should just return here if set to zero?
	if(%bot.hSearchRadius == 0)	
		%bot.hFinalRadius = 0;
	else
		%bot.hFinalRadius = brickToRadius( %bot.hSearchRadius )*%scale;

	// if hSearchRadius is set to -2 set search radius to huge, unsure if this is needed for fov
	if(%bot.hSearchRadius == -2)
		%bot.hFinalRadius = 2000000000;
	
	%n = 0;
	initContainerRadiusSearch( %pos, %bot.hFinalRadius, %type );
	while( ( %target = containerSearchNext() ) != 0 )
	{
		%target = %target.getID();
	
		// if we see us continue
		if( %bot == %target )
			continue;
		
		// check if we can see the target in our field of view
		if( !%bot.hFOVCheck( %target ) ) // <= 0.7 )
			continue;
		
		if( %target.getState() !$= "Dead" && !%target.isCloaked && %bot.hIgnore != %target && checkHoleBotTeams(%bot,%target) && hLOSCheck(%bot,%target) && miniGameCanDamage( %target, %bot ) == 1)
		{
			return %target;
		}
	}

	//Hearing
	if(%bot.hHearing && %hear)
	{
		%pos = %bot.getPosition();
		//%radius = 8*%scale;
		
		initContainerRadiusSearch( %pos, %bot.hFinalRadius/2, %type );
		while((%target = containerSearchNext()) != 0)
		{
			%target = %target.getID();
			%speed = vectorLen( %target.getVelocity() );
			
			if(%bot != %target && %speed > 3 && %target.getState() !$= "Dead" && !%target.isCloaked && checkHoleBotTeams(%bot,%target, 1) && miniGameCanDamage( %target, %bot ) == 1)
			{
				%bot.clearMoveY();
				%bot.clearMoveX();
				%bot.maxYawSpeed = 10;
				%bot.maxPitchSPeed = 10;
				%bot.setAimLocation( vectorAdd(%target.getPosition(), "0 0 2") );

				if(%bot.hEmote)
					%bot.emote(wtfImage);
				cancel(%bot.hFOVSchedule);
				%bot.hFOVSchedule = %bot.scheduleNoQuota( 500, hDoHoleFOVCheck, 0, 0, 0 );//%bot.hFOVRadius,0,0);

				return -1;
			}
		}
	}
	return 0;
}

//Use this to alert other bots around one bot or target
function AIPlayer::hAlertTeammates(%obj,%player)
{
	if(%obj.hType $= "Mercenary")
		return;
	
	%scale = getWord(%obj.getScale(),0);

	%type = $TypeMasks::PlayerObjectType;
	%pos = %obj.getPosition();

	%radius = 10*%scale;

	initContainerRadiusSearch(%pos,%radius,%type);
	while((%target = containerSearchNext()) != 0)
	{
		%target = %target.getID();
			
		if(%target.isHoleBot && %target.hSearch && %obj != %target && %obj.hType $= %target.hType && %target.inHoleLoop == 1 && %target.hLoopActive && %target.hState $= "Wandering")
		{
			if(%obj.getState() !$= "Dead" && %target.getState() !$= "Dead" && miniGameCanDamage(%obj,%target) == 1)
			{
				cancel(%target.hSched);
            %target.hSched = 0;
				%target.hFollowPlayer( %player,0, 1 );
			}
		}
	}
}

//This functioned is used in most bot logic functions to determine how we should react to that bot team
function checkHoleBotTeams(%obj, %target, %neutralAttack, %melee)
{
	// special case for bot vehicles
	%data1 = %obj.getDataBlock();
	%data2 = %target.getDataBlock();

	if( %data2.rideable && !strlen( %target.hType ) )
		return 0;

	//Mercenaries attack everything
	if(%obj.hType $= "mercenary" || %target.hType $= "mercenary")
	{
		return 1;
	}
	//Neutrals don't attack other neutrals
	if(%obj.hType $= "neutral" && %target.hType $= "neutral")
	{
		return 0;
	}
	//Neutrals don't attack others, but some bots may attack them zombies/sharks causing them to retaliate
	if(%target.hType $= "neutral")
	{
		if(getRandom(0,100) < %obj.hNeutralAttackChance)
		{
			return 1;
		}
		else
			return 0;
	}
	//Neutrals only attack when attacked themselves
	if(%obj.hType $= "neutral" && %obj.hType !$= %target.hType)
	{
		if(%neutralAttack)
			return 1;
		else
			return 0;
	}
	if(%obj.hType $= "friendly" && %target.getClassName() $= "Player")
	{
		return 0;
	}
	if(%obj.hType $= "owner" && %target == %obj.spawnBrick.getGroup().client.player)
	{
		return 0;
	}
	if(%obj.hType $= "owner" && %target.hType $= "owner" && %obj.spawnBrick.getGroup().client != %target.getGroup().spawnBrick.client)
	{
		return 1;
	}
	// if(%target.getClassName() $= "AIPlayer" && !strlen(%target.hType) && strlen(%target.getDataBlock().uiName))
	// {
		// if(getRandom(0,100) < %obj.hNeutralAttackChance)
		// {
			// return 1;
		// }
		// else
			// return 0;
	// }
	if(%obj.hType !$= %target.hType)
	{
		return 1;
	}
	if(%obj.hType $= %target.hType)
	{
		return 0;
	}
	return 1;
}

//Used to detect a wall and face the player away from the wall, you don't want bots always looking at walls, if keepMove is 1 then it won't reset his position
function AIPlayer::hDetectWall(%obj,%keepMove)
{
	if(%obj.hState $= "Wandering" || %keepMove)
	{
		%scale = getWord(%obj.getScale(),0);
		%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType;
		
		%pos = %obj.getEyePoint();
		%vector = vectorScale(%obj.getForwardVector(),4*%scale);
		%endPoint = vectorAdd(%pos,%vector);

		%target = ContainerRayCast(%pos, %endPoint, %mask,%obj);

		if(%target)
		{
			%normal = getWords( %target, 4, 6 );
			
			// add in a random element to the wall look away
			%normal = vectorAdd( hGetRandomVector( 0, 3, 1 ), %normal );
			%normal = vectorNormalize( %normal );
			
			%brick = %target.getID();
			if(!%keepMove)
				%obj.setMoveDestination(%obj.getPosition());

			// %obj.hLookAwayFromBrick( %brick, %normal );
			// %obj.hSetAimVector( %normal );
			%obj.setAimVector( %normal );
			return;
		}
	}
}
function AIPlayer::hLookAwayFromBrick( %obj, %brick )
{
	%pos = %obj.getPosition();
	%bPos = %brick.getPosition();
	
	%dif = vectorScale( vectorNormalize( vectorSub(%pos,%bPos) ),100);
	%final = vectorAdd(%pos, getWords(%dif,0,1) SPC "0");

	%final = vectorAdd(%final,"0 0 2");
	%final = vectorAdd(%final, vectorScale(getRandom(-10,10) SPC getRandom(-10,10) SPC 0, 6) );

	%obj.setAimLocation(%final);
	//obj.setaimlocation(vectorAdd(te.getposition(),vectorScale(vectorSub(te.getposition(),ta.getposition()),2)));
}

//Called when the bot has no weapon, you would run too if you were made of plastic
function AIPlayer::hRunAwayFromPlayer(%obj,%target)
{
	%obj.clearAim();
	%pos = %obj.getPosition();
	%bPos = %target.getPosition();
	
	%dif = vectorScale( vectorNormalize( vectorSub(%pos,%bPos) ),100);//was 50
	//%vec =  vectorNormalize( vectorSub(%pos,%bPos) );
	
	%final = vectorAdd(%pos, getWords(%dif,0,1) SPC "0");

	%final = vectorAdd(%final,"0 0 2");

	%final = vectorAdd(%final, vectorScale(getRandom(-10,10) SPC getRandom(-10,10) SPC 0, 6) );
	//setAimVector(%obj,%vec);
	%obj.setAimLocation(%final);
	%obj.setMoveY(%obj.hMaxMoveSpeed);
	%obj.hDetectWall(1);
	//hSpamHandsLoop(%obj);
}

function hSpamHandsLoop(%obj)
{
	cancel(%obj.hSpamHandsLoop);
   %obj.hSpamHandsLoop = 0;
	if(isObject(%obj) && %obj.hIsRunning)
	{
		%obj.playThread(3,activate2);
		%obj.hSpamHandsLoop = scheduleNoQuota(500,%obj,hSpamHandsLoop,%obj);
	}
}

function hBrickClientCheck( %brickGroup )
{
	// check if public
	if( %brickGroup.bl_id == 888888 || %brickGroup.bl_id == 999999 )
		return 1;
		
	// check for client
	if( isObject( %brickGroup.client ) )
		return 1;
		
	// well we failed, so return 0
	return 0;

	// if( !isObject( %brickGroup.client ) && !( %brickGroup.bl_id == 888888 || %brickGroup.bl_id == 999999 ) )
		// return 0;
	// else
		// return 1;
}

//Function that controls the spawning of hole bots, all bots spawn through this
function fxDTSBrick::spawnHoleBot( %obj )
{
	%brickGroup = %obj.getGroup();
	%client = %brickGroup.client;
	
	if( %obj.isDead() )
		return 0;
	
	if(isObject(%obj.hBot))
	{
		if( %obj.hBot.isDisabled() )
			%obj.unSpawnHoleBot();
		else
		{
			%obj.hBot.delete();
			%obj.hBot = 0;

			cancel(%obj.hModS);
			%obj.hModS = 0;
		}
		
		// if(%obj.hBot.getState() !$= "Dead")
      // {
			// %obj.hBot.delete();
         // %obj.hBot = 0;
      // }

		// cancel(%obj.hModS);
      // %obj.hModS = 0;
	}

	// admin only check
	if( $Pref::Server::BotsAdminOnly && isObject( %client ) && !%client.isAdmin && !%client.isSuperAdmin )
	{
		%client.centerPrint( "Sorry bots are set to admin only." , 3 );
		return 0;
	}
	
	//Checks if server is at it's limit for bots, uses the same quota as the player vehicles
   if(isObject(mainHoleBotSet) && mainHoleBotSet.getCount() >= $Server::MaxPlayerVehicles_Total)
   {
      %client = %brickGroup.client;
      if( %client )
         %client.centerPrint("Server is limited to" SPC $Server::MaxPlayerVehicles_Total SPC "Bots",3);

      return 0;
   }

	//check if brick exists, check if has client, check if it's a hole brick, and check that it has a defined datablock attached
	if( !isObject(%obj) || !%obj.getDataBlock().isBotHole ||  !isObject(%obj.hBotType) )
		return 0;
		
	// if we don't have a client and we're not public we shouldn't be spawning
	// if( !isObject( %brickGroup.client ) && !( %brickGroup.bl_id == 888888 || %brickGroup.bl_id == 999999 ) )
	if( %obj.itemPosition != 1 && !hBrickClientCheck( %brickGroup ) )
		return 0;
		
	//if(isObject(%obj) && isObject(%obj.getGroup().client) && %obj.getDataBlock().isBotHole && isObject(%obj.hBotType))
	//{
	%tooCloseCheck = %obj.getDataBlock().holeBot.hSpawnTooClose;
	%closeCheck = %obj.getDataBlock().holeBot.hSpawnClose;

	%spawnDetect = 0;
	
	if( ( !isObject(%obj.hBot) || %obj.hBot.isDisabled() ) && (%tooCloseCheck || %closeCheck) )
	{
		cancel(%obj.hSpawnDetectSchedule);
      %obj.hSpawnDetectSchedule = 0;
		%SpawnDetect = doHoleSpawnDetect(%obj);

		if(!%spawnDetect)
		{
			%obj.hSpawnDetectSchedule = %obj.scheduleNoQuota( 5000, spawnHoleBot );
			return 0;
		}
	}

	//Determine if spawn is blocked, or if we should spawn up or down
	%upBlocked = isObject( %obj.getUpBrick(0) ) && %obj.getUpBrick(0).isColliding();
	%downBlocked = isObject( %obj.getDownBrick(0) ) && %obj.getDownBrick(0).isColliding();

	%mask = $TypeMasks::TerrainObjectType;// no more interiors :( // $TypeMasks::InteriorObjectType;
	%target = ContainerRayCast(%obj.getPosition(), vectorAdd(%obj.getPosition(),"0 0 -2.8"), %mask,%obj);
	if(%target)
	{
		%downBlocked = 1;
	}
	
	//both blocked don't spawn.
	if(%upBlocked && %downBlocked)
	{
		if(!%obj.hIsBlocked)
		{
			%obj.hIsBlocked = 1;
			%obj.hOColor = %obj.getColorID();
		}
		%obj.setColorFX(4);
		%obj.setColor(0);
		cancel(%obj.hSpawnDetectSchedule);
		%obj.hSpawnDetectSchedule = %obj.scheduleNoQuota( 5000, spawnHoleBot );
		return 0;
	}
	else if(%obj.hIsBlocked)
	{
		%obj.setColor(%obj.hOColor);
		%obj.setColorFX(0);
		%obj.hIsBlocked = 0;
	}

	%spawnUp = 1;

	//Go Downward
	if(%upBlocked && !%downBlocked)
	{
		%spawnDown = 1;
	}

	// isObject %obj.hBot was here, moved up
	//Set quota object
	// %quotaObject = getQuotaObjectFromClient(%client);
	%quotaObject = getQuotaObjectFromBrickGroup( %brickGroup );
	
	if(!isObject(%quotaObject))
		error("Error: serverCmdVehicleSpawn_Respawn() - new quota object creation failed!");
		
	setCurrentQuotaObject(%quotaObject);

	%player = new AIPlayer()
	{
		dataBlock = %obj.hBotType;
		path = "";
		spawnBrick = %obj;
				
		//Apply attributes to Bot
		Name = %obj.hBotType.hName;
		hType = %obj.hBotType.hType;
		hSearchRadius = %obj.hBotType.hSearchRadius;
		hSearch = %obj.hBotType.hSearch;
		hSight = %obj.hBotType.hSight;
		hWander = %obj.hBotType.hWander;
		hGridWander = %obj.hBotType.hGridWander;
		hReturnToSpawn = %obj.hBotType.hReturnToSpawn;
		hSpawnDist = %obj.hBotType.hSpawnDist;
		hMelee = %obj.hBotType.hMelee;
		hAttackDamage = %obj.hBotType.hAttackDamage;
		hSpazJump = %obj.hBotType.hSpazJump;
		hSearchFOV = %obj.hBotType.hSearchFOV;
		hFOVRadius = %obj.hBotType.hFOVRadius;
		hTooCloseRange = %obj.hBotType.hTooCloseRange;
		hAvoidCloseRange = %obj.hBotType.hAvoidCloseRange;
		hShoot = %obj.hBotType.hShoot;
		hMaxShootRange = %obj.hBotType.hMaxShootRange;
		hStrafe = %obj.hBotType.hStrafe;
		hAlertOtherBots = %obj.hBotType.hAlertOtherBots;
		hIdleAnimation = %obj.hBotType.hIdleAnimation;
		hSpasticLook = %obj.hBotType.hSpasticLook;
		hAvoidObstacles = %obj.hBotType.hAvoidObstacles;
		hIdleLookAtOthers = %obj.hBotType.hIdleLookAtOthers;
		hIdleSpam = %obj.hBotType.hIdleSpam;
		hAFKOmeter = %obj.hBotType.hAFKOmeter + getRandom( 0, 2 );
		hHearing = %obj.hBotType.hHearing;
		hIdle = %obj.hBotType.hIdle;
		hSmoothWander = %obj.hBotType.hSmoothWander;
		hEmote = %obj.hBotType.hEmote;
		hSuperStacker = %obj.hBotType.hSuperStacker;
		hNeutralAttackChance = %obj.hBotType.hNeutralAttackChance;
		hFOVRange = %obj.hBotType.hFOVRange;
		hMoveSlowdown = %obj.hBotType.hMoveSlowdown;
		hMaxMoveSpeed = 1.0;
		hActivateDirection = %obj.hBotType.hActivateDirection;
		
		isHoleBot = 1;
	};
	
	
	clearCurrentQuotaObject();
	// Need something better than this setScale stuff for spawn effect
	// seems to go out of sync with some clients ##
	if(isObject(%player))
	{
		missionCleanup.add(%player);
		
		%player.setMoveSlowdown( %obj.hMoveSlowdown );
		
		// set the move tolerance to default with our wrapped function
		%player.setMoveTolerance( 0.25 );
	
		//%player.setscale("0.3 0.3 0.3");
		%damageType = %player.getDataBlock().hMeleeCI;

		if(strlen(%damageType))
			%player.hDamageType = $DamageType["::" @ %damageType];
		else
			%player.hDamageType = $DamageType::HoleMelee;

		%obj.hBot = %player;
		
		// don't spawn effect if we're being spawned via the director
		if( !%spawnDetect )
			%player.scheduleNoQuota(10,spawnProjectile,"audio2d","spawnProjectile","0 0 0", 1);
		//%obj.scheduleNoQuota(50,onBotSpawn);

		if(%spawnUp)
		{
			%trans = vectoradd(%obj.getposition(), "0 0 0.15");//-1.3"); "0 0 0.15"
		}

		if(%spawnDown)
		{
			%trans = vectoradd(%obj.getposition(), "0 0 -3");//-1.3");
		}	
		%rot = getwords(%obj.gettransform(),3,6);
		
		%player.setTransform(%trans SPC %rot);
		%obj.hLastSpawnTime = getSimTime();
		%player.hGridPosition = %obj.getPosition();
		//%player.scheduleNoQuota(60,setscale,"0.5 0.5 0.5");
		//%player.scheduleNoQuota(120,setscale,"1 1 1");
		%obj.onBotSpawn();
	}
	//}
	
	return %player;
}

//egh: remove the bot immediately with a little poof effect
function fxDTSBrick::unSpawnHoleBot( %brick )
{
   //echo("unspawning hole bot");

   //cancel scheduled respawn of bot.  "hModS" is a pretty vague name...
   cancel(%brick.hModS);
   %brick.hModS = 0;

   if(isObject(%brick.hBot))
   {
      %brick.hBot.spawnProjectile("audio2d","deathProjectile","0 0 0", 1);
      %brick.hBot.delete();
      %brick.hBot = 0;
   }   
}

//Used for checking if a player is close to spawn or player to determine whether to poof or spawn
function doHoleSpawnDetect(%obj)
{
	//check if it's a brick or a player being used in the function
	if(%obj.getClassName() $= "fxDTSBrick")
	{
		%tooCloseCheck = %obj.getDataBlock().holeBot.hSpawnTooClose;
		%tooCloseRange = %obj.getDataBlock().holeBot.hSpawnTCRange;
		%rangeCheck = %obj.getDataBlock().holeBot.hSpawnClose;
		%maxRange = %obj.getDataBlock().holeBot.hSpawnCRange;

		%minigame = getMiniGameFromObject( %obj );// %obj.getGroup().client.minigame;
		%minigameHost = %minigame.owner;
		%isHost = %obj.getGroup().client == %minigameHost;
		%isIncluded = %minigame.useAllPlayersBricks;
	}
	else
	{
		%tooCloseCheck = 0;
		%rangeCheck = %obj.getDataBlock().hSpawnClose;
		%maxRange = %obj.getDataBlock().hSpawnCRange;

		%minigame = getMiniGameFromObject( %obj );//%obj.spawnBrick.getGroup().client.minigame;
		%minigameHost = %minigame.owner;//%obj.spawnBrick.getGroup().client.minigame.owner;
		%isHost = %obj.spawnBrick.getGroup().client == %minigameHost;
		%isIncluded = %minigame.useAllPlayersBricks;
	}
	
	//If In minigame
	if(isObject(%minigame) && (%isHost || %isIncluded) && (%tooCloseCheck || %rangeCheck) )
	{
		%count = %minigame.numMembers;
		
		//Loop through the miniGame player list instead of doing radius searches
		for(%a = 0; %a < %count; %a++)
		{
			%player = %minigame.member[%a].player;
			if(isObject(%player) && isObject(%obj))
			{
				//get the distance from the player in brick units
				%dist = mFloatLength( VectorDist(%obj.getPosition(), %player.getPosition())-1,1)*2;
				%dist = mFloatLength( %dist ,0);
				if(%rangeCheck && %tooCloseCheck)
				{
					if( %dist <= %maxRange )
					{
						if( %dist >= %tooCloseRange && hSpawnLOSCheck(%obj,%player) )
						{
							return 0;
						}
						else if( %dist <= %tooCloseRange )
							return 0;
						// else
							// return 0;
						// if( %dist < %tooCloseRange && !hSpawnLOSCheck(%obj,%player) )
						// {
							// return 1;
						// }
					}
					else
						continue;
						
					// return 0;
				}
				else if(%rangeCheck)
				{
					if( %dist <= %maxRange )
					{
						return 1;
					}
					else
						continue;
				}
				else if(%tooCloseCheck)
				{
						// if( %dist >= %tooCloseRange && hSpawnLOSCheck(%obj,%player) )
							// return 0;
					if( %dist <= %tooCloseRange )
							return 0;
					else
						continue;
					// if( %dist >= %tooCloseRange && !hSpawnLOSCheck(%obj,%player) )
					// {
						// return 1;
					// }
					// if(%dist < %tooCloseRange && !hSpawnLOSCheck(%obj,%player))
					// {
						// return 1;
					// }
				}
			}
		}
		
		// if too close is enabled then we have to return 1 
		if( %tooCloseCheck )
			return 1;
		else
			return 0;
	}
	else//Not in minigame
	{
		return 1;
	}
	return 0;
}

//Used to check if spawn can see player, alters position to eye level
function hSpawnLOSCheck(%obj,%targ)
{
	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType;
	%target = ContainerRayCast(vectorAdd(%obj.getPosition(),"0 0 1.9"), %targ.getEyePoint(), %mask,%obj);
	if(!%target)
	{
		return 1;
	}
	else return 0;
}

//Used for bots to check if they can see us/each other
function hLOSCheck(%obj,%targ,%ignore)
{
	if( %targ.getClassName() $= "fxDTSBrick" )
		return 0;

	if(%obj.hSight == 0 && !%ignore)
	{
		return 1;
	}
	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType;
	%target = ContainerRayCast(%obj.getEyePoint(), %targ.getEyePoint(), %mask,%obj);

	if(!%target)
	{
		return 1;
	}
	else return 0;
}

//separated due to calling multiple times, and being delayed
function AIPlayer::hShootAim(%obj,%targ)
{
	//geteworldboxcenter
	if(isObject(%obj) && isObject(%targ))
		%obj.setAimLocation( vectoradd( %targ.getEyePoint(), vectorscale( %targ.getVelocity(), getRandom( 2,6 )/10 ) ) ); // 0.2
}

// shoot function, this is governed by engage distance as well as LOS
function AIPlayer::hShoot( %obj, %slot, %trigger )
{
	// if we're out of sight or out of range return, but only if we're trying to activate our image
	if( ( !%obj.hIsEngaged || !%obj.hIsInLOS || %obj.getDamagePercent() >= 1 ) && %trigger == 1 )
		return;
		
	// actually do the trigger call
	%obj.setImageTrigger( %slot, %trigger );
}

//Shooting prediction, very basic, good enough for most weapons
function AIPlayer::hShootPrediction( %obj, %targ, %tick, %shoot, %noAim )
{
	if(!%noAim)
		%obj.clearAim();
	
	if(%targ $= "none")
	{
		%obj.scheduleNoQuota(150,hShoot,0,1);
		%obj.scheduleNoQuota(%tick/%shoot/1.5,hShoot,0,0);
		return;
	}
	if(isObject(%obj) && isObject(%targ) && %obj.getState() !$= "Dead" && %targ.getState() !$= "Dead" && isObject(%obj.getMountedImage(0)))
	{
		// %scale = getWord(%obj.getScale(),0);
		// if(%obj.getMountedImage(0).melee && vectorDist(%obj.getPosition(),%targ.getPosition()) >= 10*%scale)
		// {
			// return;
		// }
		if(getRandom(0,3) > 2) 
			%obj.clearMoveX();

		//replace geteyepoint geteworldboxcenter
		//gethackposition(); 
			//%obj.hshootTimes
		//%tick = %obj.getDataBlock().hTickRate;
		//%shoot = %obj.hshootTimes;
		%imageName = %obj.getMountedImage(0).getName();
		//Special case for spear
		if(%obj.getMountedImage(0).isChargeWeapon || %obj.getDataBlock().isChargeWeapon )//|| %imageName $= "spearImage" || %obj.getDataBlock().getName() $= "CannonTurret")
		{
			%obj.scheduleNoQuota( %tick/%shoot/1.5-150, hShootAim, %targ );
		}
		else if(!%noAim)
		{
			%obj.hShootAim( %targ );
		}

		%obj.scheduleNoQuota(150,hShoot,0,1);
		if(%obj.getMountedImage(1))
			%obj.scheduleNoQuota(%tick/%shoot/4.5,hShoot,0,0);//1.5
		else
			%obj.scheduleNoQuota(%tick/%shoot/1.5,hShoot,0,0);

		if(!%noAim)
			%obj.scheduleNoQuota(%tick/%shoot/1.2,clearAim);//1.2
	}
}

//used to make the bot randomly aim around, to simulate human behavior
function AIPlayer::hLookSpastically(%obj)
{
	%obj.maxYawSpeed = getRandom(3,8);
	%obj.maxPitchSpeed = getRandom(3,8);
	
	//Randomly turn slightly instead of doing big movements
	if(getRandom(0,3) > 0)
	{
		%obj.hLookSlightly();
		return;
	}

	if(%obj.hState !$= "Following"  && isObject(%obj))
	{
		%xPos = getRandom(1,5);
		if(getRandom(0,1)) %xPos = -%xPos;

		%yPos = getRandom(1,5);
		if(getRandom(0,1)) %yPos = -%yPos;


		%obj.setaimlocation(vectorAdd(%obj.getEyePoint(),%xPos SPC %yPos SPC getrandom(1,-1)));
	}
}

//need to rewrite this and put it in the above function, using getRandomFloat
function AIPlayer::hLookSlightly(%obj)
{
	if(%obj.hState !$= "Following" && isObject(%obj))
	{
		%xPos = "0." @ getRandom(1,5);
		if(getRandom(0,1)) %xPos = -%xPos;

		%yPos = "0." @ getRandom(1,5);
		if(getRandom(0,1)) %yPos = -%yPos;
		
		%zPos = "0." @ getRandom(0,2);
		if(getRandom(0,1)) %zPos = -%zPos;

		%vector = %obj.getForwardVector();
		%fPos = vectorAdd(%obj.getEyePoint(),vectorScale(vectorAdd(%vector, %xPos SPC %yPos SPC %zPos),5));

		%obj.setAimLocation(%fPos);
	}
}

//Obstacle avoidance function, self explanatory, well if we're talking about [i]what[/i] it does
//How it does it: it shoots a ray forward, left, right, above, as well as one determined by jump height
//then at the end uses this knowledge to react to the world
function AIPlayer::hAvoidObstacle( %obj, %noStrafe, %onlyJump, %onStuck )
{
	if( !%obj.hAvoidObstacles ) 
		return;
		
	%mount = %obj.getobjectMount();
	%isWheeled = %mount && !( %mount.getType() & $TypeMasks::PlayerObjectType );
	
	// if we're immobile try to reverse and turn the wheel randomly
	if( %isWheeled && %onStuck )
	{
		// check if we're flipped
		// %vec = %mount.getUpVector();
		// %pos = %mount.getPosition();
		
		// %fPos = 
		
		// %target = ContainerRayCast(%pos, %fPos, %mask,%obj);
		
		%obj.stop();
		
		%obj.setMoveY(-0.5);
	
		%obj.setAimVector( hGetRandomFloat(0,10,1) SPC hGetRandomFloat(0,10,1) SPC 0 );
		return;
	}
	
	if( %isWheeled )
		return;
	
	%data = %obj.getDataBlock();
	
	%leftBlocked = 0;
	%rightBlocked = 0;
	%forwardBlocked = 0;
	%jumpBlocked = 0;
	%canJump = 1;
	%avoided = 0;
	%canJet = 0;
	
	if(%obj.hStackMode)
	{
		%obj.clearMoveX();
		%obj.setJumping(1);
		%obj.hStackMode = 0;
		return;
	}
	if(%obj.hLastDirAttempts >= 3)
		%obj.hLastDir = 0;
	
	//if we have a path brick and we're close to it, just try and go to it to prevent circling around target
	if(isobject( %obj.hPathBrick ) )
	{
		%pathBrickDist = vectorDist( %obj.getPosition(), %obj.hPathBrick.getPosition() )*2;// ## why am I multiplying by two here?

		if(%pathBrickDist < 8)
			%closePathBrick = 1;
	}
	//Keeping this for now, as i should probably put the spaz jump function in here
	if( isObject(%obj.hFollowing) )
	{
		%player = %obj.hFollowing;
		%playerPos = %player.getPosition();
	}
	%scale = getWord(%obj.getScale(),0);

	%pos = vectorAdd(%obj.getPosition(),"0 0" SPC 0.8*%scale);
	%vec = %obj.getForwardVector();
	
	%xVec = getWord(%vec, 0);
	%yVec = getWord(%vec, 1);

	%mask = $TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType;
	

	%dist = 5*%scale;
	%sDist = 3*%scale;
	//Forward Check
	%fPos = vectorAdd(%pos,vectorScale(%vec,%dist));//5

	%target = ContainerRayCast(%pos, %fPos, %mask,%obj);
	if(%target)
	{
		%blockingBrick = 0;
	
		if( %target.getType() & $TypeMasks::FxBrickObjectType )
		{
			// if we encounter a brick remember it
			%blockingBrick = %target;
			
			if( %blockingBrick.numEvents )
				%obj.activateStuff();
				
		}
		else if( %target.getClassName() $= "AIPlayer" )// && getWord(%playerPos,2) > getWord(%obj.getPosition(),2)+2)
		{
			%obj.clearMoveX();
			%obj.hJump();
		}
		else if( %target.getClassName() $= "Player" )
		{
			%forwardBlockPlayer = 1;

			if( %noStrafe && !getRandom(0,3) ) 
				%forwardBlockPlayer = 0;
		}
		
		

		%forwardBlocked = 1;
	}

	if( %onStuck )
		%forwardBlocked = 1;
		
	if( !%forwardBlocked )
	{
		return;
	}

	//Left Check
	%sVec = -%yVec SPC %xVec SPC 0;
	%fPos = vectorAdd(%pos,vectorScale(%svec,%sDist));

	%target = ContainerRayCast(%pos, %fPos, %mask,%obj);
	if(%target)
	{
		// echo("left Blocked");
		%leftBlocked = 1;
	}

	//Right Check
	%sVec = %yVec SPC -%xVec SPC 0;
	%fPos = vectorAdd(%pos,vectorScale(%svec,%sDist));

	%target = ContainerRayCast(%pos, %fPos, %mask,%obj);
	if(%target)
	{
		// echo("Right Blocked");
		%rightBlocked = 1;
	}

	//Up Jump Check
	%target = ContainerRayCast(%pos, vectorAdd(%pos,"0 0" SPC 5*%scale), %mask,%obj);
	if(%target)
	{
		//echo("Jump Blocked");
		//If there's an bot above me tell him to jump up so we can get that bastard
		if(isObject(%target) && %target.getClassName() $= "AIPlayer")
		{
			%obj.clearMoveX();
			%obj.hJump();
			%leftBlocked = 1;
			%rightBlocked = 1;
			
			%target.clearMoveX();
			%target.hJump();
			%target.hStackMode = 1;
		}
		
		%jumpBlocked = 1;
	}

	//Can I jump it?
	
	// temporary hack for the horse, since I don't exactly know howt he jump formula works in torque
	if( %data.jumpForce == 1530 )
		%unitPerForce = 0.004;
	else
		%unitPerForce = 0.0025;
		
		
	%jumpHeight = %unitPerForce * %data.jumpForce;
	
	%pos = vectorAdd(%pos,"0 0" SPC %jumpHeight*%scale);
	%fPos = vectorAdd(%pos,vectorScale(%vec,%dist));

	%target = ContainerRayCast(%pos, %fPos, %mask,%obj);
	if(%target)
	{
		// %target.setColor(0);
		// echo("Too High to jump");
		%canJump = 0;
		//if it's a player or bot try to jump if he's blocking our way
		if(%target.getClassName() $= "player") 
			%canJump = 1;

		if(%target.getClassName() $= "AIPlayer") 
			%canJump = 1;
		
		if( %data.canJet && !getRandom( 0, 1 ) )
		{
			%canJump = 1;
			%canJet = 1;
		}

		
		//Random jumping always good as well
		if(!getRandom(0,2)) 
			%canJump = 1;
	}
		
	if(%forwardBlocked && !%avoided && !%jumpBlocked && %canJump && !%forwardBlockPlayer)
	{
		//if in water we need to hold jump a bit longer so we can get out, this causes some problems with sea based bots, bot overall it works good
		if(%obj.getWaterCoverage() && %data.drag >= 0.05)
			%obj.hJump();
		else
			%obj.hJump(150);
		
		if( %canJet && !%obj.isJetting() )
		{
			// if( %obj.hHasJetted )
			
			// else if( !getRandom( 0, 2 ) && !%obj.hIsFalling() )
			// {
				// %obj.hJump( 100 );
			if( !getRandom( 0, 2 ) )
				%obj.hJetCheck(1);
			else
				%obj.hJet( 500+getRandom(0,300) );
			// }
		}
		else if( getRandom(0,1) && !%obj.getWaterCoverage())
		{
			%obj.hCrouch(1500);
		}
		//return;
		%hasJumped = 1;
		
		%obj.hAvoidJumpTries++;
	}
	else
		%obj.hAvoidJumpTries = 0;
	
	//if we are set to only jump, return
	if(%onlyJump)
		return;
		
	if(%forwardBlocked && ( !%hasJumped || %obj.hAvoidJumpTries > 2 ) && !%noStrafe && !%closePathBrick)
	{
		%obj.hAvoidJumpTries = 0;

		if(!%rightBlocked && !%leftBlocked)
		{
			//Check if we're already going one way, good for long walls
			if(%obj.hLastDir)
			{
				%obj.hSetMoveX(%obj.hLastDir);
				%avoided = 1;
				%obj.hLastDirAttempts++;
			}
			else
			{
				%chance = getRandom(1,2);
				if(!getRandom(0,1))
					%obj.hLastDir = %chance;

				switch (%chance)
				{
					case 1: %obj.hSetMoveX(1);		
					case 2: %obj.hSetMoveX(-1);
				}
				%avoided = 1;
			}
		}
		else if(!%leftBlocked)
		{
			%avoided = 1;
			%obj.hSetMoveX(-1);
		}
		else if(!%rightBlocked)
		{
			%avoided = 1;
			%obj.hSetMoveX(1);
		}
	}
	//Check if the bot is stuck
	if( !%onStuck &&  vectorDist(%obj.hLastPosition ,%obj.getPosition()) < 2*%scale && !%obj.hLastMoveRandom)
	{
		if (getSimTime() > %obj.lastattacked+2000)
		{
			%obj.hJump(); //Try jumping to make sure we can actually get somewhere
		}
		else if (getSimTime() > %obj.lastattacked+8000)
		{
			//echo("I'm stuck!");
			if(isObject(%obj.hFollowing) && vectorDist(%pos, %obj.hFollowing.getPosition()) <= 4)
			{
				%obj.hLastMoveRandom = 0;
				return;
			}
			%obj.hLastMoveRandom = 1;
			%obj.hClearMovement();
			%xRand = getrandom(-10,10);
			%yRand = getrandom(-10,10);
			
			%fPos = %obj.hReturnForwardBackPos(-15);

			%obj.setMoveDestination( vectoradd(%fPos,%xRand SPC %yRand SPC 0) );

			if(%obj.hEmote)
			{
				%obj.emote(hateImage);
			}
			return;
		}
		
	}
	%obj.hLastMoveRandom = 0;
	%obj.hLastPosition = %obj.getPosition();
}

// 1 3.2224

//0.999 0.0002 0

//Locks the position to a grid relative of the bots spawn, does not take into consideration bot scale // pos = mFloor(pos / 2) * 2
function lockToGrid(%obj, %pos)
{
	%scale = getWord(%obj.getScale(),0);
	// %scale = 1;

	%brick = %obj.spawnBrick;
	%bPos = %brick.getPosition();
   // %bPosX = getWord(%bPos, 0);
   // %bPosY = getWord(%bPos, 1);

   // %bDelta = vectorSub(%pos, %bPos);
   // %bDeltaX = getWord(%bDelta, 0);
   // %bDeltaY = getWord(%bDelta, 1);
   
   // //round to nearest 2 world units
   // %bDeltaX = mFloor(%bDeltaX / 2) * 2;
   // %bDeltaY = mFloor(%bDeltaY / 2) * 2;

   // %x = %bPosX + %bDeltaX;
   // %y = %bPosY + %bDeltaY;

   // return %x SPC %y SPC 0;

	// echo( %brick SPC %bPos );
	for(%a = 0; %a < 2; %a++)
	{
		%brick[%a] = getWord(%bPos,%a);

		//check if brick pos x/y is even 
		%even[%a] = mFloor(%brick[%a]) % 2 == 0;

		%len[%a] = strlen(%brick[%a]);
		
		//does brick x or y have .5 at the end?
		if( %len[%a] > 2 && getSubStr(%brick[%a], %len[%a]-2, 2) $= ".5" )
			%add[%a] = 1;
		
		%pos[%a] = mFloatLength( getWord(%pos, %a)/(2*%scale) ,0 )*(2*%scale);
		
		//If brick x/y is odd, make our number odd
		if(!%even[%a])
			%pos[%a] = %pos[%a]-1;
		
		//add back .5 if we need to
		if(%add[%a])
			%pos[%a] += 0.5;
	}
	
	%final = %pos0 SPC %pos1 SPC 0;
	
	return %pos0 SPC %pos1 SPC 0;
}

//A more human like wandering function, slowly turns most of the time
function AIPlayer::smoothWander(%obj)
{
	%obj.maxYawSpeed = getRandom(3,10);
	%obj.maxPitchSpeed = getRandom(3,10);

	%maxSpeed = %obj.hMaxMoveSpeed*10;
	%halfSpeed = %maxSpeed/2;
	
	// %speed = hGetRandomFloat(5,8);
	%speed = hGetRandomFloat( %halfSpeed , %maxSpeed );
	
	// small chance of us walking backwards
	if( !getRandom(0,5) ) 
		%speed = -%speed;
		
	%obj.setMoveY(%speed);

	%p[0] = "";
	%p[1] = "-";

	%x = hGetRandomFloat(0,10,1);

	%y = hGetRandomFloat(0,10,1);
	
	%z = hGetRandomFloat(0,3,1);

	%vec = %x SPC %y SPC %z;
	// %obj.hSetAimVector( %vec );
	%obj.setAimVector( %vec );
}

//My desk is really dirty at the moment.
//blah blah blah blah blah
//Wooohoohoohohohohohooho

//Used for idle behaviors doesn't take into consideration teams, not sure if it needs to
function AIPlayer::hReturnCloseBlockhead(%obj)
{
	%type = $TypeMasks::PlayerObjectType;
	%pos = %obj.getPosition();
	%scale = getWord(%obj.getScale(),0);
	%radius = brickToRadius( %obj.hSearchRadius )*%scale;

	initContainerRadiusSearch(%pos,%radius,%type);
	while((%target = containerSearchNext()) != 0)
	{
		%target = %target.getID();

		// take into consideration LOS
		if( %target != %obj && !%target.isCloaked && hLOSCheck( %obj, %target ) )
		{
			// remember to check FOV before continuing
			if( %obj.hSearchFOV )
			{
				if( %obj.hFOVCheck( %target ) )
					return %target;
			}
			else
				return %target;
		}
	}
	
	return 0;
}

//there was a really stupid comment here, I removed it for your safety
//function that imitates the spam clicking of users, need to simplify this
function AIPlayer::hSpazzClick(%obj, %amount, %panic)
{
	//Called if the bot is deactivated mid loop
	if(%obj.isHoleBot && !%obj.hLoopActive)
	{
		if( isObject(%obj.hLastWeapon) )
		{
			%obj.setWeapon( %obj.hLastWeapon );
			// %obj.mountImage(%obj.hLastWeapon,0);
			// if( isObject(%obj.hLastWeaponL) )
			// {
				// %obj.mountImage(%obj.hLastWeaponL,1);
			// }
		}
		else
		{
			%obj.unMountImage(0);
		}
		%obj.hIsSpazzing = 0;
		// fixArmReady(%obj);
		if( strlen( %obj.hDefaultThread ) )
			%obj.playThread(1,%obj.hDefaultThread);
		return;
	}

	//Rather than playing the animation I should probably switch to bot.activatestuff()
	if(isObject(%obj) && %obj.getState() !$= "Dead" && %amount <= 12 && ( %obj.hState $= "Wandering" || %panic) )
	{
		cancel(%obj.hSpazzClick);
      %obj.hSpazzClick = 0;
		%amount++;
		%obj.activateStuff();
		%obj.hSpazzClick = %obj.scheduleNoQuota( 200, hSpazzClick, %amount, %panic );
	}
	if(%amount > 12)
	{
		if( isObject(%obj.hLastWeapon) )
		{
			%obj.setWeapon( %obj.hLastWeapon );
			// %obj.mountImage(%obj.hLastWeapon,0);
			// if( isObject(%obj.hLastWeaponL) )
			// {
				// %obj.mountImage(%obj.hLastWeaponL,1);
			// }
		}
		else
		{
			%obj.unMountImage(0);
		}

		%obj.hIsSpazzing = 0;
		fixArmReady(%obj);
		
		if( strlen( %obj.hDefaultThread ) )
			%obj.playThread(1,%obj.hDefaultThread);
	}

}

//for more information check page 16 of the blockland manual