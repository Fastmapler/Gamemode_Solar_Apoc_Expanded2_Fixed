//Increase max object counts so we can support more players
$Server::MaxPhysVehicles_Total = 100;
$EOTW::PlayerLoopRate = 10;

function PlayerLoop()
{
	cancel($EOTW::PlayerLoop);

	if (getSimTime() - $EOTW::LastSPTick > $EOTW::PowerTickRate)
	{
		$EOTW::LastSPTick = getSimTime();
		%tickServers = true;
	}
	
	for (%i = 0; %i < ClientGroup.GetCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);
		%client.PrintEOTWInfo();

		if (isObject(%player = %client.player))
		{
			//Natural and potion healing
			if (%player.getDamageLevel() > 0)
			{
				%healAmount = %player.getDatablock().repairRate / $EOTW::PlayerLoopRate;
				if (%player.hasEffect("Healing"))
					%healAmount *= 100;

				if (%player.HasImplant("Mending"))
					%healAmount *= 5;

				%player.setDamageLevel(%player.getDamageLevel() - %healAmount);
			}

			if (%player.HasImplant("Mending") && %player.alcoholDrinkCount >= 1 && getRandom() < (0.05 / $EOTW::PlayerLoopRate))
				%player.alcoholDrinkCount -= 1;

			//Player buff duration
			for (%j = 0; %j < getFieldCount(%player.effectList); %j++)
			{
				%effect = getField(%player.effectList, %j);
				if (%player.appliedEffect[%effect] > 0)
					%player.appliedEffect[%effect]--;
			}

			//Buff from Adrenline Implant
			if(%player.HasImplant("Adrenline"))
			{
				if (!%player.isSpeedImplantBoosted)
				{
					%player.isSpeedImplantBoosted = true;
					%player.ChangeSpeedMulti(0.1);
				}
				
				%recoverAmount = 1 / $EOTW::PlayerLoopRate;
				%player.setEnergyLevel(%player.getEnergyLevel() + %recoverAmount);
			}

			//Speed buff effect
			if(%player.hasEffect("Speed"))
			{
				if (!%player.isSpeedPotionBoosted)
				{
					%player.isSpeedPotionBoosted = true;
					%player.ChangeSpeedMulti(1);
				}
					
			}
			else if (%player.isSpeedPotionBoosted)
			{
				%player.isSpeedPotionBoosted = false;
				%player.ChangeSpeedMulti(-1);
			}

			//Asphalt boost check
			%pos = %player.getPosition();
			initContainerBoxSearch(%pos, "1.25 1.25 0.1", $TypeMasks::fxBrickObjectType);
			%col = containerSearchNext();
			if(	isObject(%col) && %col.getClassName() $= "fxDTSbrick" && %col.isRendering() && %col.material $= "Asphalt")
			{
				if (!%player.isAsphaltBoosted)
				{
					%player.isAsphaltBoosted = true;
					%player.ChangeSpeedMulti(0.5);
				}
					
			}
			else if (%player.isAsphaltBoosted)
			{
				%player.isAsphaltBoosted = false;
				%player.ChangeSpeedMulti(-0.5);
			}
		}
				
				
		if (%tickServers && %client.ServerPoints > 0)
		{
			%client.ServerPoints -= mPow(%client.ServerPoints / 4, 1.25) / 10;
			%client.ServerPoints = mClamp(%client.ServerPoints, 0, 99);
		} 
	}
	
	$EOTW::PlayerLoop = schedule(100,ClientGroup,"PlayerLoop");
}
schedule(1000 / $EOTW::PlayerLoopRate, 0, "PlayerLoop");

function EOTW_applyBooze(%obj, %amount)
{
    if (!isActivePackage("ItemDrinksPackage") || !isObject(%obj))
        return;

	%obj.alcoholDrinkCount += %amount;
	%obj.setWhiteOut(%obj.alcoholDrinkCount / 100);

	if (%obj.alcoholDrinkCount > 32 && %obj.alcoholDrinkCount < 100)
	{
		%obj.client.centerprint("<color:ffff00>You're feeling funny... ", 3);
	}
	else if (%obj.alcoholDrinkCount >= 100)
	{
		%obj.setDamageFlash(1);
		messageClient(%obj.client, '', "You died of poisoning...");
		%obj.kill();
		return;
	}

	schedule(33, %obj, alcoholTickLoop, %obj);
}

function GetTimeStamp()
{
	%hours = mFloor($EOTW::Time);
    %minutes = 60*($EOTW::Time - %hours);
    %minutes = mFloor(%minutes * 0.1) * 10;

    %hours = (%hours + 6) % 24; //corrects for dawn being considered hour 0
	
	if (%minutes < 10)
		%minutes = "0" @ %minutes;
		
    return %hours @ ":" @ %minutes;
}

function Player::GetEffectText(%player)
{
	%text = "";
	for (%i = 0; %i < getFieldCount(%player.effectList); %i++)
	{
		%effect = getField(%player.effectList, %i);
		if (%player.appliedEffect[%effect] > 0)
		{
			%text = %text @ getSubStr(%effect, 0, 2) @ ": " @ mCeil(%player.appliedEffect[%effect] / $EOTW::PlayerLoopRate) @ "|";
		}
	}

	if (%text !$= "")
		return trim(getSubStr(%text, 0, strLen(%text) - 1));
	else
		return "";
}

function GameConnection::PrintEOTWInfo(%client)
{
	if (!isObject(%player = %client.player))
	{
		%client.bottomPrint("<just:center>\c7You died!");
		return;
	}

	%blacklist = "CardsOutImage ChipImage DeckOutImage";
	if (isObject(%image = %player.getMountedImage(0)) && hasWord(%blacklist, %image.getName()))
		return;

	if (getSimTime() - %client.lastBottomPrintReqest > $EOTW::PowerTickRate)
	{
		%health = mCeil(%player.getDatablock().maxDamage - %player.getDamageLevel());
	
		if (%player.getDamagePercent() > 0.75)
			%health = "<color:ff0000>" @ %health;
		else if (%player.getDamagePercent() > 0.25)
			%health = "<color:ffff00>" @ %health;
		else
			%health = "<color:00ff00>" @ %health;
		
		%health = %health @ "<color:ffffff>/" @ %player.getDatablock().maxDamage;
		if (%player.isProtected())
			%health = %health SPC "\c5[PROTECT | " @ mCeil(%client.GetProtectionTime()) @ " min(s)]";
		
		if (isObject(%image = %player.getMountedImage(0)))
		{
			%db = %client.inventory[%client.currInvSlot];

			if (!isObject(%db) && isObject(%player.tempBrick))
				%db = %player.tempBrick.getDatablock();
				
			if (%image.getName() $= "BrickImage" && isObject(%db))
			{
				if (%client.buildMaterial $= "")
					%client.buildMaterial = GetMatterType("Granite").name;

				if ($EOTW::BrickDescription[%db.getName()] !$= "")
				{
					%centerText = "<br><br><br><br>\c6" @ $EOTW::BrickDescription[%db.getName()];
				}

				%db.AutoUpdateCost();
				
				if ($EOTW::CustomBrickCost[%db.getName()] !$= "")
				{
					%cost = $EOTW::CustomBrickCost[%db.getName()];
					
					%brickText = "<br>";
					
					if (getField(%cost, 0) < 1.0)
						%brickText = %brickText @ "(" @ ((1.0 - getField(%cost, 0)) * 100) @ "% Refund Fee!)";
						
					for (%i = 2; %i < getFieldCount(%cost); %i += 2)
					{
						%volume = getField(%cost, %i);
						%matter = getMatterType(getField(%cost, %i + 1));
						%name = %matter.name;
						%color = "<color:" @ %matter.color @ ">";
						%inv = $EOTW::Material[%client.bl_id, %name] + 0;
						if (%inv < %volume) %inv = "\c0" @ %inv;
					
						%brickText = %brickText SPC %inv @ "\c6/" @ %volume SPC %color @ %name @ "\c6,";
						%hasCost = true;
					}
					
					if (%hasCost)
						%brickText = getSubStr(%brickText, 0, strLen(%brickText) - 1);
				}
				else
				{
					%matter = getMatterType(%client.buildMaterial);
					%name = %matter.name;
					%color = "<color:" @ %matter.color @ ">";
					%inv = $EOTW::Material[%client.bl_id, %name] + 0;
				
					%volume = %db.brickSizeX * %db.brickSizeY * %db.brickSizeZ;
					if (%inv < %volume) %inv = "\c0" @ %inv;
					
					%brickText = "<br>" @ %color @ %name @ "\c6: " @ %inv @ "\c6/" @ %volume SPC "[" @ %db.getName() @ "]";
				}
			}
			else if (%image.printPlayerBattery)
			{
				%brickText = "<br>" @ %player.GetBatteryText();
			}
			else if (%image.ammoType !$= "")
			{
				%brickText = "<br>" @ %image.ammoType @ ": " @ $EOTW::Material[%client.bl_id, %image.ammoType];
			}
			else if (%image.showMatterBetting)
			{
				if (%player.matterBet !$= "")
					%brickText = "<br>Current Bet: " @ getField(%player.matterBet, 0) SPC getField(%player.matterBet, 1);
				else
					%brickText = "<br>Current Bet: None (/betMatter <Amount> <Material Name>)";
			}
				
		}
		else if (getSimTime() - %player.lastBatteryRequest < 1000)
			%brickText = "<br>" @ %player.GetBatteryText();
		
		%effectText = %player.GetEffectText();
		if (%effectText !$= "")
			%effectText = "<br>" @ %effectText;
		
		if (%centerText !$= "")
			%client.centerPrint(%centerText, 1);

		%dayText = $EOTW::Time >= 12 ? "Night\c6:" SPC GetDayCycleText() : "Day\c6:" SPC GetDayCycleText();
		%client.bottomPrint("<just:center>\c3" @ %dayText @ " | \c3Time\c6:" SPC GetTimeStamp() SPC "| \c3Health\c6:" SPC %health @ %brickText @ %effectText,3);
	}
	else
	{
		%client.bottomPrint(%client.bottomPrintText, 2);
	}
	
}

function GetRandomSpawnLocation(%initPos, %failCount)
{
	
	if (%initPos !$= "")
	{
		%xOffset = (getRandom() < 0.5 ? getRandom(16, 32) : getRandom(-32, -16));
		%yOffset = (getRandom() < 0.5 ? getRandom(16, 32) : getRandom(-32, -16));
		%eye = (getWord(%initPos, 0) + %xOffset) SPC (getWord(%initPos, 1) + %yOffset) SPC 495; //getRandom(0, 1664)
	}
	else
		%eye = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2)) / 1) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3)) / 1) SPC 495;
	%dir = "0 0 -1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
	
	if (getWord(%ray,3) < $EOTW::LavaHeight && %failCount < 500)
		return GetRandomSpawnLocation(%initPos, %failCount + 1); //Try again lol
		
	%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
	if(isObject(%hit = firstWord(%ray)))
	{
		%pos = vectorAdd(%pos,"0 0 5");
		return %pos;
	}
}

function Player::LookingAtBrick(%obj, %brick)
{
	%eye = %obj.getEyePoint();
	%dir = %obj.getEyeVector();
	%for = %obj.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	if(isObject(%hit = firstWord(%ray)) && %hit.getID() $= %brick.getID())
		return true;

	return false;
}

function Player::GetMatterCount(%obj, %matter)
{
	if (!isObject(%client = %obj.client))
		return;

	return $EOTW::Material[%client.bl_id, %matter] + 0;
}

function Player::ChangeMatterCount(%obj, %matter, %count)
{
	if (!isObject(%client = %obj.client))
		return;

	$EOTW::Material[%client.bl_id, %matter] += %count;

	if ($EOTW::Material[%client.bl_id, %matter] < 0)
		$EOTW::Material[%client.bl_id, %matter] = 0;

	return $EOTW::Material[%client.bl_id, %matter];
}

registerOutputEvent("fxDTSBrick", "WarpPlayer", "", 1);
function fxDTSBrick::WarpPlayer(%brick, %client)
{
	if (!isObject(%group = %brick.getGroup()) || %group.bl_id != 888888 || !isObject(%player = %client.player))
		return;

	%client.WarpingPlayer = true;

	if ($EOTW::Time < 12 && $EOTW::SunSize >= 1.0)
		commandToClient(%client,'messageBoxYesNo',"Warning", "Warning!\n\nThe sun is currently out and lethally hot! Teleport anyways?", 'WarpPlayerAccept','WarpPlayerCancel');
	else
		ServerCmdWarpPlayerAccept(%client);
}

function ServerCmdWarpPlayerAccept(%client)
{
	if (%client.WarpingPlayer && isObject(%player = %client.player))
		%player.setTransform(GetRandomSpawnLocation());

	ServerCmdWarpPlayerCancel(%client);
}

function ServerCmdWarpPlayerCancel(%client)
{
	%client.WarpingPlayer = false;
}

function clearIllegalEvents()
{
	unregisterOutputEvent("fxDtsBrick", "spawnExplosion");		//Assholes try to lag the server up.
	unregisterOutputEvent("fxDtsBrick", "spawnItem");			//Allows players to bypass the crafting process + Bypasses Blacklist
	unregisterOutputEvent("fxDtsBrick", "setItem");				//Allows players to bypass the Blacklist
	unregisterOutputEvent("fxDtsBrick", "spawnProjectile");		//People shoot projectiles at others.
	
	unregisterOutputEvent("Player", "addHealth");				//This is a survival-based gamemode. Drink some coffee.
	unregisterOutputEvent("Player", "changeDatablock");			//You can't be a tank turret, sorry.
	unregisterOutputEvent("Player", "clearTools");				//People *will* use this to clear others' tools. Ctrl-k if you want to actually clear tools for some reason.
	unregisterOutputEvent("Player", "instantRespawn");			//Kill but even worse because items won't drop.
	unregisterOutputEvent("Player", "kill");					//Death traps and the like. People are assholes.
	unregisterOutputEvent("Player", "setHealth");				//Just drink some coffee.
	unregisterOutputEvent("Player", "spawnExplosion");			//Assholes try to lag the server up + Landmines/Turrets.
	unregisterOutputEvent("Player", "spawnProjectile");			//People shoot projectiles at others + Turrets.
	unregisterOutputEvent("Player", "setPlayerScale");			//Trap players by increasing their size.
	unregisterOutputEvent("Player", "addVelocity");				//So players don't nuke other players into orbit.
	unregisterOutputEvent("Player", "setVelocity");				//So players don't nuke other players into orbit.
	
	unregisterOutputEvent("MiniGame", "CenterPrintAll");		//There are other ways to annoy people.
	unregisterOutputEvent("MiniGame", "BottomPrintAll");		//There are other ways to annoy people. Would get drowned out by the gamemode's bottom print anyways.
	unregisterOutputEvent("MiniGame", "ChatMsgAll");			//There are other ways to annoy people.
	unregisterOutputEvent("MiniGame", "Reset");					//instantRespawn but for the whole server!
	unregisterOutputEvent("MiniGame", "RespawnAll");			//instantRespawn but for the whole server!

	unregisterOutputEvent("Player", "SetHat");					//Get your own hats.
	unregisterOutputEvent("GameConnection", "GrantHat");		//Get your own hats.

	//Bot stuff. Players can't make bot holes but can still use horses to activate these events.
	unregisterOutputEvent("Bot", "addHealth");
	unregisterOutputEvent("Bot", "changeDatablock");
	unregisterOutputEvent("Bot", "clearTools");
	unregisterOutputEvent("Bot", "instantRespawn");
	unregisterOutputEvent("Bot", "kill");
	unregisterOutputEvent("Bot", "setHealth");
	unregisterOutputEvent("Bot", "spawnExplosion");
	unregisterOutputEvent("Bot", "spawnProjectile");
	unregisterOutputEvent("Bot", "setPlayerScale");
	unregisterOutputEvent("Bot", "addVelocity");
	unregisterOutputEvent("Bot", "setVelocity");

	unregisterOutputEvent("Bot", "DropItem");
	unregisterOutputEvent("Bot", "SetTeam");
	unregisterOutputEvent("Bot", "SetWeapon");
	unregisterOutputEvent("Bot", "SetBotPowered");
	unregisterOutputEvent("Bot", "VCE_modVariable");
}
schedule(10, 0, "clearIllegalEvents");

package EOTW_Player
{
	function Armor::onTrigger(%data, %obj, %trig, %tog)
	{
		if(isObject(%client = %obj.client) && %tog && !isObject(%obj.getMountedImage(0)) && isObject(%hit = %obj.whatBrickAmILookingAt()) && %hit.getClassName() $= "fxDtsBrick")
		{
			%data = %hit.getDatablock();
			if(%trig == 0 && (%data.matterSize > 0 || %data.isPowered))
			{
				if (%client.tutorialStep < 10)
				{
					messageClient(%client, '', "You can interact with machines after the tutorial.");
					return Parent::onTrigger(%data, %obj, %trig, %tog);
				}

				cancel(%obj.MatterBlockInspectLoop);
				%obj.MatterBlockInspectLoop = %obj.schedule(100, "InspectBlock", %hit);
			}
			else if (%trig == 4 && %data.processingType !$= "" && %obj.isCrouched())
				ServerCmdSetRecipe(%client);
		}
		Parent::onTrigger(%data, %obj, %trig, %tog);
	}
	function serverCmdClearCheckpoint(%client)
	{
		if(isObject(%client.checkPointBrick))
		{
			%client.checkPointBrick = "";
			%client.checkPointBrickPos = "";

			messageClient(%client, '', '\c3Checkpoint reset');
		}
	}
	function minigameCanDamage(%o1,%o2)
	{
		%mini1 = getMinigameFromObject(%o1);
		%mini2 = getMinigameFromObject(%o2);
		if(isObject(%mini1) && %mini1 == %mini2)
		{
			if((%o1.getType() & $TypeMasks::VehicleObjectType && %o2.getClassName() $= "Player") || (%o2.getType() & $TypeMasks::VehicleObjectType && %o1.getClassName() $= "Player"))
				return 0;

			if(!%o1.getType())
				%p1 = %o1.player;
			else if(%o1.getClassName() $= "Player")
				%p1 = %o1;
			else
				return Parent::minigameCanDamage(%o1,%o2);

			if(!%o2.getType())
				%p2 = %o2.player;
			else if(%o2.getClassName() $= "Player")
				%p2 = %o2;
			else
				return Parent::minigameCanDamage(%o1,%o2);

			if(%p1 == %p2)
				return Parent::minigameCanDamage(%o1,%o2);
			
			return 0;
		}
		return Parent::minigameCanDamage(%o1,%o2);
	}
};
activatePackage("EOTW_Player");

function Player::InspectBlock(%obj, %brick)
{
	cancel(%obj.MatterBlockInspectLoop);
	if (!isObject(%client = %obj.client) || !isObject(%brick))
		return;

	if(!isObject(%hit = %obj.whatBrickAmILookingAt()) || %hit != %brick)
	{
		%client.centerPrint("",1);
		return;
	}

	%data = %brick.getDatablock();

	if (%data.inspectMode > 0)
		%data.onInspect(%brick, %client);
	if (%data.inspectMode > 1) //Skip our default inspect completely
	{
		%obj.MatterBlockInspectLoop = %obj.schedule(100, "InspectBlock", %brick);
		return;
	}

	%text = "<color:ffffff>[\c3" @ %data.uiName @ "\c6]";

	if ($EOTW::BrickUpgrade[%data.getName(), "MaxTier"] > 0)
		%text = %text SPC "(Tier " @ (%brick.upgradeTier + 1) @ ")";

	%slotTypes = "Input\tBuffer\tOutput";
	for (%i = 0; %i < getFieldCount(%slotTypes); %i++)
	{
		%type = getField(%slotTypes, %i);
		for (%j = 0; %j < %data.matterSlots[%type]; %j++)
		{
			%slotData = %brick.matter[%type, %j];
			%text = %text NL %type @ " (" @ (%j + 1) @ "):" SPC getField(%slotData, 1) SPC getField(%slotData, 0);
		}
	}
	%client.centerPrint(%text,1);
	%client.overRideBottomPrint(%brick.getStatusText());

	%obj.MatterBlockInspectLoop = %obj.schedule(100, "InspectBlock", %brick);
}

function GameConnection::overRideBottomPrint(%client, %text)
{
	%client.bottomPrintText = %text;
	%client.lastBottomPrintReqest = getSimTime();
}

function Player::whatBrickAmILookingAt(%obj)
{
	%eye = %obj.getEyePoint();
	%dir = %obj.getEyeVector();
	%for = %obj.getForwardVector();
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
	return firstWord(%ray);
}

function GameConnection::GetProtectionTime(%client)
{
	if (%client.permaProtection)
		return "--";
	else
		return uint_sub(%client.protectionLimit, getSimTime()) / (60 * 1000);
}
function GameConnection::SetProtectionTime(%client, %timeMS, %notify)
{
	if (%timeMS == -1)
	{
		%client.permaProtection = true;
		return;
	}
	%client.permaProtection = false;
	%client.protectionLimit = uint_add(getSimTime(), %timeMS);
	if (%notify)
	{
		%client.chatMessage("\c5You now have " @ (%timeMS / (1000 * 60)) @ " minute(s) of Sun and Monster Agression protection.");
		%client.chatMessage("\c5Use this time wisely to collect materials to make a base.");
	}
}

function Player::isProtected(%player)
{
	return %client.permaProtection || (isObject(%client = %player.client) && %client.protectionLimit > getSimTime());
}

function GetDayCycleText()
{
	%day = ($EOTW::Day % 60);
	%cycle = mFloor($EOTW::Day / 60) + 1;
	return %day @ " (Cycle " @ %cycle @ ")";
}

exec("./Player_SolarApoc.cs");
exec("./Support_MultipleSlots.cs");
exec("./Support_PlayerBattery.cs");
exec("./Item_Armors.cs");
//exec("./Support_Achievements.cs");
exec("./Support_BrickShiftMenu.cs");
exec("./Script_HelpSystem.cs");
exec("./Support_MoveHandler.cs");
//exec("./Support_NoPvP.cs");


//SetMutualBrickGroupTrust