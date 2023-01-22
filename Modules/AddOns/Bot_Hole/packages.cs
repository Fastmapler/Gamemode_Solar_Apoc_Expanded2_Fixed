// Everything the mod overwrites is in here. scripts overwritten, armor callbacks, minigame create/resets, brickPlanting, addevent commands

function AIPlayer::hDelayOnReachLoop( %obj )
{
	if( !%obj.hSkipOnReachLoop )
	{
		cancel( %obj.hSched );
      %obj.hSched = 0;
		%obj.hLoop();
	}
	else
		%obj.hSkipOnReachLoop = 0;
}

package BotHolePackage
{
	// temporary overrides
	function Player::emote( %obj, %emote )
	{
		if( !isObject( %emote ) )
			return;
			
		if( %obj.isHoleBot && %obj.hEmote == 0 )
			return;
			
		parent::emote( %obj, %emote );
	}

	function paintProjectile::onCollision( %this, %obj, %col, %bool, %pos, %normal )
	{
		if( %col.isHoleBot && $lolPainting )
			%col.hIsBeingSprayed = %obj.sourceObject;
			
		return parent::onCollision( %this, %obj, %col, %bool, %pos, %normal );
	}
	
	// make fixArmReady take into consideration dual guns
	function fixArmReady( %obj )
	{
		parent::fixArmReady( %obj );

		if( %obj.getMountedImage(0).armReadyBoth )
			%obj.playThread(1, armreadyboth);
	}
	
	// Armor overrides
	// called when the bot reaches the brick target, specificed by goToBrick event
	function Armor::onReachDestination( %this, %obj )
	{
		// should package this
		return parent::onReachDestination( %this, %obj );
		//Solar Apoc: We are returning this early as we don't really need the later code and this function seems to be the source(?)
		//Of the strange crashes.


		// if not a holebot return
		if( !%obj.isHoleBot || %obj.hState $= "Following" )
			return;
			
		%scale = getWord( %obj.getScale(), 0 );
		%pos = %obj.getPosition();
		
		// flag set when we're returning to spawn
		if( %obj.hIsGuard )
		{
			%returnDist = brickToMetric( %obj.hSpawnDist );
			
			%returnDist = %returnDist*%scale;
			
			if(%returnDist <= 0)
				%returnDist = 1.6;
				
			%returnDist = %returnDist*%scale;
			
			if( %returnDist < 1 )
				%returnDist = 1;
			
			%posSpawn = %obj.spawnBrick.getPosition();
			
			// check that we've fully returned to spawn
			if( vectordist( %pos, %posSpawn ) <= %returnDist )
			{
				// ok we've returned so let's look forward and turn off hIsGuard
				// %vec = vectorScale( %obj.spawnBrick.getForwardVector(), 15 );
				// %loc = vectorAdd( vectorAdd( %posSpawn, "0 0 1.8" ), %vec );
				
				%obj.maxYawSpeed = getRandom( 5, 10 );
				%obj.maxPitchSpeed = getRandom( 5, 10 );
				
				// %obj.setAimLocation( %loc );
				%obj.setAimVector( %obj.spawnBrick.getForwardVector() );
				%obj.hIsGuard = 0;
				
				// call onBotReachBrick event
				%obj.spawnBrick.onBotReachBrick(%obj);
			}
		}
		
		// called when we're pathing towards a brick and we're not following someone
		if( isObject( %obj.hPathBrick ) && vectorDist( %pos, %obj.hPathBrick.getPosition() ) <= %obj.getMoveTolerance()*12 )//3 )//*%scale ) // removed the scaling on this, doesn't matter much
		{
			%lastBrick = %obj.hPathBrick;
			%obj.hPathBrick = 0;
			%lastBrick.onBotReachBrick(%obj);
			%obj.lastBrickReachTime = getSimTime()+500;//No idea why this is here, I must of had something planned for it
			%obj.scheduleNoQuota( 100, hDelayOnReachLoop );
			// return;
		}
	}

	function Armor::onNewDataBlock( %this, %obj )
	{
		parent::onNewDataBlock( %this, %obj );
		
		if( %obj.isHoleBot )
		{
			GameConnection::ApplyBodyParts(%obj);
			GameConnection::ApplyBodyColors(%obj);
		}
	}
	
	//SWeps Melee: Monster melee attacks will be deflected (then stunned) by blocks
	function AIPlayer::hMeleeAttack( %obj, %col )
	{
		if(%col.getType() & $TypeMasks::VehicleObjectType || %col.getType() & $TypeMasks::PlayerObjectType)
		{
			if( %obj.hState $= "Following" && !%obj.isStunned )
			{
				if(getSimTime()-%col.lastMeleeBlock < 300 && isObject(%col.meleeHand))
				{
					%subTo = vectorNormalize(vectorSub(vectorAdd(%obj.getPosition(),"0 0 1"),vectorAdd(%col.getPosition(),"0 0 1")));

					%dot = vectorDot(%subTo,%col.getEyeVectorHack());
					if(%dot > 0.2)
					{
						%oA = %col.meleeHand.getMountedImage(0).item.smMaterial;
						serverPlay3D(swolMelee_getSFX(%oA @ "_" @ %oA @ "_Parry"), %obj.getPosition());
						swolMelee_doParry(%col);
						%obj.playThread(2,plant);
						swolMelee_stunPlayer(%obj,1,1700,1);
						%p = new projectile()
						{
							datablock = wrenchProjectile;
							initialPosition = vectorAdd(vectorAdd(%col.getPosition(),"0 0 1"),%subTo);
							scale = "1 1 1";
						};
						%p.explode();
						return;
					}
				}
				else
				{
					%client = %col.client;

					%name = %col.client.name;
					%obj.playthread(2,activate2);

					%col.damage(%obj.hFakeProjectile, %col.getposition(), %obj.hAttackDamage, %obj.hDamageType);
					%obj.lastattacked = getsimtime()+1000;
				}
			}
		}
	}
	
	function armor::onCollision(%this,%obj,%col,%a,%b,%c,%d)
	{
		//check if it's a hole bot, make sure it's an AIPlayer
		if(%obj.isHoleBot && %obj.getClassname() $= "AIPlayer" && getSimTime() > %obj.lastattacked)
		{	
			if(isObject(%obj.spawnBrick) && isObject( getMiniGameFromObject( %obj ) ) || %obj.getState() !$= "Dead")// %obj.spawnBrick.getGroup().client.miniGame
			{
				//Check if the target is dead then stop following it
				if(%obj.hFollowing == %col && %col.getState() $= "Dead")
				{
					%obj.hFollowing = "";
					%obj.hClearMovement();
					%obj.hState = "Wandering";
					
					if(%obj.getDatablock().hEmote)
					{
						%obj.emote(loveImage);
					}

					cancel(%obj.hSched);
               %obj.hSched = 0;
					%obj.hLoop();
					return;

				}
				
				if(%obj.hSuperStacker && %col.isHoleBot && !checkHoleBotTeams(%obj,%col) && isObject(%obj.hFollowing))
				{
					%scale = getWord(%obj.getScale(),0);

					%targetZ = getWord(%obj.hFollowing.getPosition(),2);

					%objPos = %obj.getPosition();
					%objZ = getWord(%objPos,2);

					if(%targetZ-%objZ > 2)
					{
						//if is close enough x y, and is above head
						%colPos = %col.getPosition();

						%xy = getWords(%objPos,0,1);
						%txy = getWords(%colPos,0,1);
						%dist = vectorDist(%xy, %txy);

						%colZ = getWord(%colPos,2);
						%zDif = %colZ-%objZ;
						
						// should make the lower bot jump later so the bots can get a super jump going
						if(%zDif >= 2.5*%scale && %dist <= 1.3*%scale)
						{
							%col.hStackMode = 1;
							%col.clearMoveX();

							%col.scheduleNoQuota(500,setJumping,1);

							%obj.hStackMode = 1;
							%obj.clearMoveX();

							%obj.scheduleNoQuota(500,setJumping,1);
						}
					}
					%obj.lastattacked = getsimtime()+1000;
				}

				if( ( %col.getType() & $TypeMasks::PlayerObjectType ) && %col.getState() $= "Dead")
					return;

					
				%checkTeams = checkHoleBotTeams( %obj, %col, 1 );
				%meleeVehicle = %col.getDataBlock().rideable && !strlen( %col.hType );
				
				%canDamage = isObject(%obj.spawnBrick) && isObject( getMiniGameFromObject( %obj ) ) && %obj.getState() !$= "Dead" && miniGameCanDamage(%obj,%col) == 1; // %obj.spawnBrick.getGroup().client.miniGame
				//Melee 
				//if(%col.getClassname() $= "Player" || %col.getClassname() $= "AIPlayer" || %col.getClassname() $= "WheeledVehicle" || %col.getClassname() $= "FlyingVehicle")
				if(%obj.hMelee && %canDamage && ( %checkTeams || %meleeVehicle ))
				{
					%obj.hMeleeAttack( %col );
					// if(%col.getType() & $TypeMasks::VehicleObjectType || %col.getType() & $TypeMasks::PlayerObjectType)
					// {
						// if( %obj.hState $= "Following" )
						// {
							// %client = %col.client;

							// %name = %col.client.name;
							// %obj.playthread(2,activate2);

							// %col.damage(%obj.hFakeProjectile, %col.getposition(), %obj.hAttackDamage, %obj.hDamageType);
							// %obj.lastattacked = getsimtime()+1000;
							// %didSomething = 1;
						// }
					// }
				}

				//If it's a player or bot and you hate him, attack him
				if( %col.getType() & $TypeMasks::PlayerObjectType )
				{
					if(%obj.hSearch && %obj.hFollowing != %col && %obj.hIgnore != %col && %canDamage && %checkTeams && %obj.inHoleLoop == 1 && %obj.hLoopActive && %col.getState() !$= "Dead" && %obj.hType !$= "neutral")
					{
						%obj.hFollowPlayer( %col, 0 );
						%obj.lastattacked = getsimtime()+1000;
						%didSomething = 1;
					}
				}
				
				if(%obj.hShoot)
				{
					if(%obj.hFollowing == %col && %obj.hAvoidCloseRange && !%obj.getMountedImage(0).Melee)
						%obj.setMoveY( %obj.hMaxMoveSpeed/2 );
						// %obj.setMoveY(-0.5);

					%obj.lastAttacked = getSimTime()+1000;
				}
			}
			//Custom collision called only once every second; placed at the end so we can detect if the col is dead
			// if(%this.hCustomCollision)
			%this.onBotCollision(%obj,%col,%a,%b,%c);
		}
		parent::onCollision(%this,%obj,%col,%a,%b,%c,%d);
	}
	
	function armor::onDisabled(%this,%obj,%a)
	{
		parent::onDisabled(%this,%obj,%a);
		holeBotDisabled(%obj);
	}
	
	//has to be packaged due to using custom calls
	function CannonTurret::onDisabled(%this,%obj,%state)
	{
		parent::onDisabled(%this,%obj,%state);
		holeBotDisabled(%obj);
	}
	
	//if you want your player to be compatible with bots, and you use custom calls put this at the end of your onDisabled funcitons
	function holeBotDisabled(%obj)
	{
		//Do usual holebot checks
		if(%obj.isHoleBot && %obj.getState() $= "Dead" && isObject(%obj.spawnBrick) && %obj.getClassName() $= "AIPlayer")
		{
			%miniGame = getMiniGameFromObject( %obj );// %obj.spawnBrick.getGroup().client.miniGame;
			
			%spawnTime = 0;
			%spawnTime = %miniGame.botRespawnTime;//Insert minigame bot respawn time here
			%brick = %obj.spawnBrick;
			%brickGroup = %brick.getGroup();
			%botSet = %brickGroup.hBotSet;
			
			// remove bot from bot sets
			%botSet.remove( %obj );
			mainHoleBotSet.remove( %obj );
			
			// if we have an override spawntime on the brick use that instead
			if( %brick.itemRespawnTime )
				%spawnTime = %brick.itemRespawnTime;
			
			// hBotSet
			
			//If spawntime is too low put it to 1 second
			if(%spawnTime < 1000)	
				%spawnTime = 1000;
			
			//If outside of minigame respawn should be 5 seconds
			if( !isObject(%miniGame) && %spawnTime < 5000 ) 
				%spawnTime = 5000;

			
			if(%brick.hBot == %obj)
			{
				%brick.hModS = %brick.scheduleNoQuota( %spawnTime, spawnHoleBot );
			}
		}
	}

	function armor::onMount(%this,%obj,%col,%slot)
	{
		Parent::onMount(%this,%obj,%col,%slot);
	
		// echo( "onMount" SPC %obj SPC %col );
	
		// if we're entering a vehicle set our move tolerance to be a bit higher
		if( %obj.isHoleBot ) 
		{
			if( %col.getType() & $TypeMasks::PlayerObjectType )
				%obj.setMoveTolerance( 3 );
			else
				%obj.setMoveTolerance( 12 );
		}
		
		if(!%col.isHoleBot || !%col.getDataBlock().rideable)
			return;
			
		//should switch this to a more generalized variable name
		if(%col.hIsInfected || %col.isStampeding || checkHoleBotTeams(%obj,%col) )
		{
			%col.emote("AlarmProjectile");
			%col.scheduleNoQuota( getRandom( 500, 1500 ), hKickRider );
		}
	}
	
	function armor::onUnMount( %this,%obj,%col,%slot )
	{
		Parent::onUnMount( %this,%obj,%col,%slot );
		
		if( %obj.isHoleBot )//&& !( %col.getType() & $TypeMasks::PlayerObjectType ) )
			%obj.setMoveTolerance( 0.25 );
	}
	
	function armor::onAdd(%this,%obj)
	{
		parent::onAdd(%this,%obj);

		if(!%obj.isHoleBot || !%obj.getClassname() $= "AIPlayer")
			return;

			
		%spawnBrick = %obj.spawnBrick;
		
		if(!isObject(%obj.spawnBrick.getGroup().hBotSet))
		{
			%obj.spawnBrick.getGroup().hBotSet = new simset();
		}
		%obj.spawnBrick.getGroup().hBotSet.add(%obj);
		
		//Same for global set
		if(!isObject(mainHoleBotSet))
		{
			new simset(mainHoleBotSet);
		}
		mainHoleBotSet.add(%obj);

		// echo( %obj.spawnBrick );
		
		%item = %spawnBrick.item;
		
		// echo( %spawnBrick.item.getDataBlock().image );
		if( isObject( %item ) )
		{
			%image = %item.getDataBlock().image;
			
			%obj.setWeapon( %image );
			// %obj.mountImage( %image, 0 );
			
			// if( %image.armReady )
				// %obj.playThread( 1, armReadyRight );
		}
		// if(%this.hShoot && isObject(%this.hWep))
		// {
			// %obj.mountImage(%this.hWep,0);
			// if(%this.hWep.armReady)
			// {
				 // %obj.playThread(1, armReadyRight);
			// }
		// }

		//Fake projectile is so they can correctly damage using melee
		%obj.hFakeProjectile = new scriptObject(){};
		%obj.hFakeProjectile.sourceObject = %obj;
		%obj.hFakeProjectile.client = %obj;
		
		%obj.isBot = 1;
		//This is done so they can use certain functions meant to be called on a client
		%obj.player = %obj;
		// %obj.mountImage(hJumpImage,2);
		// %obj.mountImage(hCrouchImage,3);
		%obj.hLastSpawnTime = getSimTime();
		%obj.hSched = %obj.scheduleNoQuota(100,hLoop);
		%obj.hLoopActive = 1;
	}
	
	// package for bots to make sure we clear out some stuff
	function armor::onRemove( %this, %obj )
	{
		parent::onRemove( %this, %obj );
		
		if(!%obj.isHoleBot || !%obj.getClassname() $= "AIPlayer")
			return;
			
		// remove the fake projectile
		if( isObject( %obj.hFakeProjectile ) )
      {
			%obj.hFakeProjectile.delete();
         %obj.hFakeProjectile = 0;
      }
	}
	function TankTurretPlayer::Damage(%this,%obj,%source,%pos,%amm,%type)
	{
		parent::Damage(%this,%obj,%source,%pos,%amm,%type);
		holeBotDamage(%this, %obj, %source,%a,%b,%c);
	}
	function armor::Damage(%this,%obj,%source,%a,%b,%c)
	{
		parent::Damage(%this,%obj,%source,%a,%b,%c);
		holeBotDamage(%this, %obj, %source,%a,%b,%c);
	}
	
	function holeBotDamage( %this, %obj, %source, %a, %b, %c )
	{
		if(!isObject(%source) || !%obj.isHoleBot) return;
		
		%target = %source.sourceObject;
		if(%source.getClassName() $= "player" || %source.getClassName() $= "AIPlayer")
		{
			%target = %source;
		}

		// if(%this.hCustomDamage)
		%this.onBotDamage(%obj,%source,%a,%b,%c);
		
		if(%obj.getState() $= "Dead" && !%obj.hCalledBotDeath)
		{
			%obj.hKiller = %source.client;
			
			if( isObject(%obj.spawnBrick) )
			{
				%obj.spawnBrick.onBotDeath(%obj.hKiller);
				//%obj.spawnBrick.hBot = 0;
			}

			if(isObject(%obj.hKiller) && %obj.hKiller.getClassName() $= "GameConnection")
			{
				%obj.hKiller.incScore(%obj.hKiller.Minigame.Points_KillBot);
			}
			%obj.hCalledBotDeath = 1;
			return;
		}

		if( isObject(%target) && isObject(%obj) && isObject( getMiniGameFromObject( %obj ) ) )// %obj.spawnBrick.getGroup().client.minigame)
		{
			if(%obj.inHoleLoop == 1 && %obj.hLoopActive && checkHoleBotTeams(%obj,%target, 1) && %obj.hSearch  && %obj.getClassName() $= "AIPlayer")
			{
				%scale = getWord(%obj.getScale(),0);
				//check if he can see the player who shot him
				if(%obj.hIgnore != %target && hLOSCheck(%obj,%target) && vectorDist(%obj.getPosition(),%target.getPosition()) <= 128*%scale || getRandom(1,10) == 1)
				{
					%obj.hFollowPlayer( %target, 0 );
				}
				//he can't so play ??? emote and have a chance of alerting other bots around him
				else
				{
					%obj.emote(wtfImage);
				}
			}
		}
	}
	//Activate stuff parented so you can call onBotActivated, useful for rpgs and giving players items
	function Player::ActivateStuff(%obj)
	{
		if( %obj.isHoleBot && $pref::bot::disableActivate )
			return;
	
		parent::ActivateStuff(%obj);

		%mask = $TypeMasks::PlayerObjectType | $TypeMasks::FxBrickObjectType;
		
		%eye = %obj.getEyePoint();
		%eyeVec = vectorScale(%obj.getEyeVector(),5);

		%fPoint = vectorAdd(%eye,%eyeVec);

		%target = ContainerRayCast( %eye, %fPoint, %mask, %obj );
		if( %target )
		{
			%target = %target.getID();

			if(%target.getClassName() $= "AIPlayer" && isObject(%target.spawnBrick))
			{
				// check if we're facing the bot correctly
				%fov = %target.hFOVCheck( %obj, 1, 1 );

				if( %target.hActivateDirection == 0 || ( %fov > 0 && %target.hActivateDirection == 1 ) || ( %fov < 0 && %target.hActivateDirection == 2 ) )
					%target.spawnBrick.onBotActivated(%obj.client);
			}
		}
	}	

	function AIPlayer::InstantRespawn(%this,%obj,%a,%b,%c,%d,%e)
	{
		if(%this.isHoleBot  && %this.getClassName() $= "AIPlayer")
		{
			// %this.respawnHoleBot();
			if( getSimTime() > %this.spawnBrick.hLastSpawntime+200 )
				%this.spawnbrick.spawnHoleBot();
				
			return;
		}
		parent::InstantRespawn(%this,%obj,%a,%b,%c,%d,%e);
	}

	// Brick Overrides
	function fxDTSBrick::onLoadPlant(%obj)
	{
		parent::onLoadPlant(%obj);

		if(!isObject($Server_LoadFileObj) || !%obj.getDataBlock().isBotHole) 
			return;

		%obj.isBotHole = 1;
		%obj.scheduleNoQuota( 50, onHoleSpawnPlanted );
	}
	//Overwritten to spawn the hole bots, maybe not the best way to do this
	function fxDTSBrick::trustCheckFinished(%obj,%a,%b,%c)
	{
		parent::trustCheckFinished(%obj,%a,%b,%c);
		
		if(isObject($Server_LoadFileObj) || !%obj.getDataBlock().isBotHole) 
			return;

		// set the default values for the brick
		%obj.isBotHole = 1;
		%obj.setItem( %obj.getDatablock().holeBot.hWep );
		%obj.itemRespawnTime = 0;
		
		// do normal holebot stuff
		%obj.onHoleSpawnPlanted();
		
	}

	function fxDTSBrick::onRemove(%obj)
	{
		cancel(%obj.hSpawnDetectSchedule);
      %obj.hSpawnDetectSchedule = 0;
		parent::onRemove(%obj);
	}
	
	function fxDTSBrick::onDeath(%obj)
	{
		//do special effect and delete the bot if it's a hole brick
		if(%obj.getdatablock().isBotHole && isObject(%obj.hBot))
		{
			%obj.hBot.spawnProjectile("audio2d","deathProjectile","0 0 0", 1);
			%obj.hBot.delete();
         %obj.hBot = 0;
		}
		parent::onDeath(%obj);
	}

	// Brick Clearing overrides
	function serverCmdClearBricks(%client)
	{
		parent::serverCmdClearBricks(%client);

		%botSet = %client.brickGroup.hBotSet;
		if(isObject(%botSet))
		{
			if(%botSet.getCount() <= 0)
			{
				return;
			}
			for(%a = %botSet.getCount()-1; %a >= 0; %a--)
			{
				%botSet.getObject(%a).delete();
			}
		}

	}
	function serverCmdClearBrickGroup(%client,%id)
	{
		parent::serverCmdClearBrickGroup(%client,%id);

      if(!%client.isAdmin)
		   return;

		%group = hGetBrickGroupFromBLID(%id);
		%botSet = %group.hBotSet;
		if(isObject(%botSet))
		{
			if(%botSet.getCount() <= 0)
			{
				return;
			}
			for(%a = %botSet.getCount()-1; %a >= 0; %a--)
			{
				%botSet.getObject(%a).delete();
			}
		}
	}
	function serverCmdClearAllBricks(%client)
	{
		parent::serverCmdClearAllBricks(%client);

      if(!%client.isAdmin)
   		return;

		if(isObject(mainHoleBotSet))
		{
			%group = mainHoleBotSet;

			for(%a = %group.getCount()-1; %a >= 0; %a--)
			{
				%group.getObject(%a).delete();
			}
		}
	}
	
	//Packaged so the bots properly get reset
	function MiniGameSO::reset(%this,%client)
	{
		%lastReset = %this.lastResetTime;

		parent::reset(%this,%client);

		if( %lastReset+5000 > getSimTime() )
			return;
		
		%owner = %this.owner;
		%numMembers = %this.numMembers;
		%useAll = %this.useAllPlayersBricks;

		if(%useAll)
		{
			for(%a = 0; %a < %numMembers; %a++)
			{
				%member = %this.member[%a];

				hResetBots(%member);
			}
		}
		else
		{
			hResetBots(%owner);
		}
		
		// defaultMinigame check, reset the public bricks
		if( isObject( $defaultMiniGame ) )
			hResetBots( BrickGroup_888888 );
	}

	function serverCmdCreateMiniGame(%client,%name,%a,%b,%c,%d,%e)
	{
		parent::serverCmdCreateMiniGame(%client,%name,%a,%b,%c,%d,%e);

		hResetBots(%client);
	}

	// Game start related Packages
	function GameConnection::startMission(%this)
	{
		parent::startMission(%this);
		
		//Restart all bots since the user is back
		%group = %this.brickGroup.hBotBrickSet;
		
		if(!isObject(%group))
			return;

		for(%a = 0; %a < %group.getCount(); %a++)
		{
			%brick = %group.getObject(%a);

         if( !%brick.hBotType.hManualSpawn )//&& %brick.itemPosition != 1 )
   			%brick.spawnHoleBot();
		}
	}
	
	// client leaves game disconnect
	function GameConnection::onDrop( %this, %dunno )
	{
		parent::onDrop( %this, %dunno );
		
		// clear all the bots since the user is leaving
		%group = %this.brickGroup.hBotBrickSet;
		
		if(!isObject(%group))
			return;

		for(%a = 0; %a < %group.getCount(); %a++)
		{
			%brick = %group.getObject(%a);
			
			// reset the bots that are marked as keep on
			// this is done to match up with vehicle spawns, since those are reset upon leaving
			if( %brick.itemPosition == 1 )
				%brick.spawnHoleBot();
			else
				%brick.unSpawnHoleBot();
		}
		
	}
	
	// called when the game begins, by this time we have all the information there is
	function startGame(%a,%b,%c,%d)
	{
		parent::startGame(%a,%b,%c,%d);
		
		// Create the event lists
		createHoleBotList();
		createGestureList();
		hCreateWanderList();
		hCreateSearchList();
		hCreateIdleList();
	}
	
	// function fxDTSBrick::setLight( %obj, %light, %client )
	// {
		// echo( %light );
		// parent::setLight( %obj, %light, %client );
		
		
	// }
	
	function fxDTSBrick::setItemRespawntime( %obj, %time )
	{
		parent::setItemRespawntime( %obj, %time );
		
		if( %obj.isBotHole && %time <= 0 )
			%obj.itemRespawnTime = 0;
	}
	
	// packaged so we can keep bots when gone, admin only
	function fxDTSBrick::setItemPosition( %obj, %pos )
	{
		if( %obj.isBotHole )
		{
			%client = %obj.getGroup().client;
			
			if( %pos > 1 )
				%pos = 1;
				
			if( %pos < 0 )
				%pos = 0;
			
			if( isObject( %client ) && !%client.isAdmin && !%client.isSuperAdmin )
				%pos = 0;
		}
	
		parent::setItemPosition( %obj, %pos );
	}
	
	function fxDTSBrick::setItem( %obj, %item, %client )
	{
		parent::setItem( %obj, %item, %client );
		if( %obj.isBotHole )
		{
			if( isObject( %obj.item ) )
			{
				%obj.item.setHidden(1);
				%obj.item.hideNode("ALL");
			}
			
			if( isObject( %obj.hBot ) )
			{	
				if( %item == 0 )
					%item = -1;
					
				%obj.hBot.setWeapon( %item );
			}
		}
	}
	
	// intercepts the wrench data for setting the weapon on the bot from brick
	function serverCmdSetWrenchData( %client, %info )
	{
		%brick = %client.wrenchBrick;
		
		if( %brick.isBotHole )
		{
			%item = getField( %info, 4 );
			%item = getWord( %item, 1 );
			// return;
		}
		parent::serverCmdSetWrenchData( %client, %info );
	}
	
	function fxDTSBrick::sendWrenchData( %obj, %client )
	{
		if( %obj.isBotHole )
			commandToClient( %client, 'OpenWrenchBotHack' );
			
		parent::sendWrenchData( %obj, %client );
	}
	
	function fxDTSBrick::colorVehicle( %obj )
	{
		if( %obj.isBotHole && isObject( %obj.hBot ) )
			GameConnection::ApplyBodyColors(%obj.hBot);
		
		parent::colorVehicle( %obj );
	}
	
	//Overwrite for set bot appearance event
	function serverCmdAddEvent(%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r,%s,%t,%u,%v,%w,%x,%y,%z)
	{
		//SetBodyColorOnAddEvent(%a,%a.wrenchbrick);
		//SetBodyPartsOnAddEvent(%a,%a.wrenchbrick);
		Parent::serverCmdAddEvent(%a,%b,%c,%d,%e,%f,%g,%h,%i,%j,%k,%l,%m,%n,%o,%p,%q,%r,%s,%t,%u,%v,%w,%x,%y,%z);
		if (!isObject(%brick = %a.wrenchBrick))
			return;
		
		%client = %a;

		for(%c = 0; %c < %brick.numEvents; %c++)
		{
			//find the setBotAppearance event, and make sure it's the right event one that's not set already
			if(%brick.eventOutput[%c] $= "setAppearance" && %brick.eventOutputParameter[%c,1] == 6 && !%brick.eventOutputParameter[%c,2]) 
			{
				//echo("Bot Appearance event Found");
				%brick.EventOutputParameter[%c,2] = "1" SPC %client.hat SPC %client.accent SPC %client.pack SPC %client.secondpack SPC %client.chest SPC %client.hip SPC
					%client.lleg SPC %client.rleg SPC %client.larm SPC %client.rarm SPC %client.lhand SPC %client.rhand SPC %client.faceName SPC %client.decalName;

				%save = %client.headcolor SPC %client.hatcolor SPC %client.packcolor SPC %client.secondpackcolor SPC %client.chestcolor SPC %client.hipcolor SPC %client.accentcolor
					SPC %client.llegcolor SPC %client.rlegcolor SPC %client.larmcolor SPC %client.rarmcolor SPC %client.lhandcolor SPC %client.rhandcolor;

				%hn = -1;
				%brick.eventOutputParameter[%c,3] = hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1) SPC 
					hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1);

				%brick.eventOutputParameter[%c,4] = hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1) SPC
					hGetFourDigits(%save, %hn++,1) SPC hGetFourDigits(%save, %hn++,1);
			}
		}

		if(%brick.getDatablock().isBotHole && getSimTime() > %brick.hLastSpawnTime+100)
		{
			%bot = %brick.hBot;
			cancel(%brick.hModS);
         %brick.hModS = 0;
			if(isObject(%bot))
			{
				%bot.delete();
			}
         if(!%brick.hBotType.hManualSpawn)
         {
   			%brick.scheduleNoQuota( 100, spawnHoleBot );
	   		%brick.hLastSpawnTime = getSimTime();
         }
		}
	}
};
activatepackage(BotHolePackage);