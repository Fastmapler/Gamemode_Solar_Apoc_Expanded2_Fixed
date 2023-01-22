//Holes, one for work and one for fun
exec("./support.cs");
exec("./holes.cs");
exec("./events.cs");
exec("./packages.cs");
//exec("./bot_base.cs");

// non lethal items, makes us run away when using them
WrenchImage.nonLethal = 1;
spearImage.isChargeWeapon = 1;
CannonTurret.isChargeWeapon = 1;

AkimboGunImage.armReadyBoth = 1;

//Remove these from release
// exec("./bugLogger.cs");

//servercmds the player can call

// callbacks from the wrench gui
function serverCmdCurrentHoleBotOff( %client )
{
	if( isObject( %client.wrenchBrick.hBot ) )
		%client.wrenchBrick.hBot.stopHoleLoop();
}

function serverCmdCurrentHoleBotOn( %client )
{
	if( isObject( %client.wrenchBrick.hBot ) )
		%client.wrenchBrick.hBot.startHoleLoop();
}

function serverCmdRespawnCurrentHoleBot( %client )
{
	%brick = %client.wrenchBrick;
	// respawn the bot from the current wrenchBrick
   if( isObject( %brick ) && getSimTime() > %brick.hLastSpawntime+200 && !%brick.hBotType.hManualSpawn)
   	%client.wrenchBrick.spawnHoleBot();
}

//returns bot count
function serverCmdBotCount(%client)
{
	if(isObject(mainHoleBotSet))
	{
		%count = mainHoleBotSet.getCount();
		messageClient(%client,'botCount',%count SPC "Bots");
	}
	else
		messageClient(%client,'botCount',%count SPC "Bots");
}

//returns user bot count
function serverCmdMyBotCount(%client)
{
	if(isObject(%client.brickgroup.hBotSet))
	{
		%count = %client.brickGroup.hBotSet.getCount();
		messageClient(%client,'botCount',%count SPC "Bots");
	}
	else
		messageClient(%client,'botCount',"0 Bots");
}

//resets all bots
function serverCmdResetAllBots(%client)
{
	if(%client.isAdmin || %client.isSuperAdmin)
	{	
		messageAll('resetbots',"<color:FFFF00>" @ %client.name SPC "<color:FF0000>reset all bots.");

		if(!isObject(mainHoleBotSet))
			return;

		for(%a = mainHoleBotBrickSet.getCount()-1; %a >= 0; %a--)
		{
			%obj = mainHoleBotBrickSet.getObject(%a);

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

//resets clients bots
function serverCmdResetBots(%client, %user)
{
	// check if user is admin if reseting other peoples bots
	if( ( %client.isAdmin || %client.isSuperAdmin ) && strlen(%user) )
	{
		%user = findClientByName(%user);
		if( isObject(%user) )// && (%client.isAdmin || %client.isSuperAdmin) )
		{
			messageAll('resetbots',"<color:FFFF00>" @ %client.name SPC "<color:FF0000>reset<color:FFFF00>" SPC %user.name @ "<color:FF0000>'s bots.");
			hResetBots(%user);
			return;
		}
		else
		{
			messageClient( %client, 'resetBots', "User not found" );
		}
	}
	else
	{
		if(getSimTime() > %client.hLastBotResetTime+2000 || %client.isAdmin || %client.isSuperAdmin)
		{
			messageClient(%client,'resetmybots',"Your bots have been reset.");
			%client.hLastBotResetTime = getSimTime();
			hResetBots(%client);
		}
		else
			messageClient(%client,'resetTooSoon',"Can't reset your bots that quickly.");
	}
}

//is kind of useless once gui edits are done, sets if bots can damage, respawn time and points they give
function serverCmdBotMiniGame(%client, %damage, %respawnTime, %points)
{
	%miniGame = %client.miniGame;
	if(%miniGame.owner == %client)
	{
		%miniGame.botDamage = %damage;
		%miniGame.botRespawnTime = %respawnTime*1000;
		%miniGame.Points_KillBot = %points;
	}
}

// debug crap
if( !$pref::server::isDebugCrap )
	return;
	
function execBots( %events )
{
	// exec( "Add-Ons/Bot_Hole/server.cs" );
	exec("./support.cs");
	exec("./holes.cs");
	exec("./packages.cs");

	if( %events )
	{
		exec("./events.cs");
		// exec("./defaultDamage.cs");
	}
}