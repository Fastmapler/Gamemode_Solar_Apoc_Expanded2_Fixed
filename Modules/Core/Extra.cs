function dumpTest(%amt, %reset)
{
	if (%reset)
	{
		deleteVariables("$EOTW::RandomTest*");
	
		for (%i = 0; %i < %amt; %i++)
			spawnGatherable();
	}
		
	for (%i = 0; %i < getFieldCount($EOTW::MatSpawnList); %i++)
	{
		%matter = GetMatterType(getField($EOTW::MatSpawnList, %i));
		echo(%matter.name SPC ($EOTW::RandomTest[%matter.name] + 0));
	}
	
}

function doTipLoop(%num)
{
	cancel($EOTW::TipLoop);
	%num++;
	switch (%num)
	{
		case 1: %text = "\c5Tip\c6: Crouch while scrolling through your materials to scroll backwards.";
		case 2: %text = "\c5Tip\c6: Machines can only interact within the same BL_ID. Use the Chown tool to change brick ownership if needed.";
		case 3: %text = "\c5Tip\c6: Ramps can be used to boost your mobility.";
		case 4: %text = "\c5Tip\c6: Some placable drink bricks (under the \"Special\" tab) can heal you! just don't poison yourself.";
		case 5: %text = "\c5Tip\c6: A checkpoint is asexual (essential) for a base, otherwise you will be severely displaced when you respawn.";
		case 6: %text = "\c5Tip\c6: Use the /insert and /extract commands to add or remove materials from a machine.";
		case 7: %text = "\c5Tip\c6: Railroad tracks very cheap to place, and all vehicles are free to spawn. Or you can just use teledoors instead.";
		case 8: %text = "\c5Tip\c6: You can obtain most items by simply spawning it on a brick. Many tools do require crafting material though.";
		case 9: %text = "\c5Tip\c6: Most enemies are vulnerable to lava and/or sun damage. You can lure enemies into these hazards for easy kills.";
		case 10: %text = "\c5Tip\c6: The \"/m [material]\" command will give basic information on a material, including its sources and uses.";
		case 11: %text = "\c5Tip\c6: Some monsters have extremely dangerous ranged attacks. Using firearms, running away, or safespotting is a must.";
		case 12: %text = "\c5Tip\c6: Your hammer can deal minor damage to enemies. This ability cannot be spamfired, unfortunately.";
		case 13: %text = "\c5Tip\c6: The \"/Help\" command will tell you what special commands exist.";
		case 14: %text = "\c5Tip\c6: Complex logistics can be done with the use of VCE.";
		case 15: %text = "\c5Tip\c6: Upgrading a machine will enable it to process lower tier recipes in parallel.";/
		case 16: %text = "\c5Tip\c6: While mininum or higher shaders might look pretty, having shaders OFF will provide the best visual clarity, especially at night.";
		default: %text = "\c5Tip\c6: Dying is bad, don't do it. You will drop all held tools on death. Held materials (ie Iron) will be kept."; %num = 0;
	}
	
	messageAll('',%text);
	
	$EOTW::TipLoop = schedule(60000 * 3, 0, "doTipLoop",%num);
}
schedule(60000 * 3, 0, "doTipLoop",%num);

function fallbackMaterialType()
{
	//Attempts to turn bricks with no material back to iron or custom
	//We could analyze the brick's color to see what material it might of been but whatever

	%blacklist = "888888 999999 1337";

    for (%j = 0; %j < MainBrickGroup.getCount(); %j++)
    {
        %group = MainBrickGroup.getObject(%j);
        %blid = %group.bl_id;

        if (hasWord(%blacklist, %blid))
            continue;

        for (%i = 0; %i < %group.getCount(); %i++)
        {
            %brick = %group.getObject(%i);
			if (%brick.material $= "")
			{
				if ($EOTW::CustomBrickCost[%brick.getDatablock().getName()] !$= "")
					%brick.material = "Custom";
				else
					%brick.material = "Iron";
			}
        }
    }
}

function getGatherableDensity()
{
	deleteVariables("$EOTW::MatTest*");
	
	for (%i = 0; %i < Brickgroup_1337.getCount(); %i++)
		$EOTW::MatTest[Brickgroup_1337.getObject(%i).material]++;
		
	for (%i = 0; %i < getFieldCount($EOTW::MatSpawnList); %i++)
	{
		%matter = GetMatterType(getField($EOTW::MatSpawnList, %i));
		echo(%matter.name SPC ($EOTW::MatTest[%matter.name] + 0));
		%sum += $EOTW::MatTest[%matter.name];
	}
	echo("Total: " @ %sum);
}

function mRound(%val)
{
	if (%val - mFloor(%val) < 0.5)
		return mFloor(%val);
	else
		return mCeil(%val);
}

registerOutputEvent("Player", "AddSafeVelocity", "vector 20");
registerOutputEvent("Bot", "AddSafeVelocity", "vector 20");
function Player::AddSafeVelocity(%player, %vector)
{
	%vel = %player.getVelocity();
	%vel = VectorAdd(%vel, %vector);

	//So players can't get multi boosted into a wall and die epically
	%vel = mClamp(getWord(%vel, 0), -20, 20) SPC mClamp(getWord(%vel, 1), -20, 20) SPC mClamp(getWord(%vel, 2), -20, 20);
	%player.setVelocity(%vel);
}

function getFieldFromValue(%list, %value)
{
	for (%i = 0; %i < getFieldCount(%list); %i++)
		if (getField(%list, %i) $= %value)
			return %i;
			
	return -1;
}

function decimalFromHex(%hex)
{
	%seq = "0123456789ABCDEF";
	
	%val = 0;
	for (%i = 0; %i < strLen(%hex); %i++)
	{
		%char = getSubStr(%hex, %i, 1);
		%d = striPos(%seq, %char);
		%val = 16*%val + %d;
	}
	
	return %val;
}
//
function hexFromFloat(%float)
{
	%hex = "0123456789abcdef";
	
	%val = %float * 255;
	
	%first = getSubStr(%hex, mFloor(%val / 16), 1);
	%second = getSubStr(%hex, mFloor(%val % 16), 1);
	
	return %first @ %second;
}

function getColorDecimalFromHex(%hex)
{
	for (%i = 0; %i < strLen(%hex); %i += 2)
		%color = %color SPC (decimalFromHex(getSubStr(%hex, %i, 2)) / 255);
		
	if (strLen(%hex) == 6)
		%color = %color SPC "1.0";
	
	return trim(%color);
}

function getColorFromHex(%hex)
{
	return getClosestColor(getColorDecimalFromHex(%hex));
}

function ServerCmdHexFromPaintColor(%client)
{
	if (!%client.isSuperAdmin || !isObject(%player = %client.player))
		return;
	
	%color = getColorIDTable(%player.currSprayCan);
	
	%hex = hexFromFloat(getWord(%color, 0)) @ hexFromFloat(getWord(%color, 1)) @ hexFromFloat(getWord(%color, 2)) @ hexFromFloat(getWord(%color, 3));
	SetClipboard(%hex);
	talk(%hex);
}

function getFieldIndex(%text, %field)
{
	for (%i = 0; %i < getFieldCount(%text); %i++)
		if (%field $= getField(%text, %i))
			return %i;
	return -1;
}

function HexToRGB(%hex) {
	if(strLen(%hex) != 6) {
		return;
	}

	%chars = "0123456789abcdef";

	for(%i=0;%i<3;%i++) {
		%value = getSubStr(%hex, 2*%i, 2);

		%first = getSubStr(%value, 0, 1);
		%last = getSubStr(%value, 1, 1);
		
		%first = stripos(%chars, %first)*16;
		%last = stripos(%chars, %last);

		%sum = %first + %last;
		%str = trim(%str SPC %sum);
	}

	return %str;
}

function RGBToHex(%rgb) {
	%rgb = getWords(%rgb,0,2);
	for(%i=0;%i<getWordCount(%rgb);%i++) {
		%dec = mFloor(getWord(%rgb,%i)*255);
		%str = "0123456789ABCDEF";
		%hex = "";

		while(%dec != 0) {
			%hexn = %dec % 16;
			%dec = mFloor(%dec / 16);
			%hex = getSubStr(%str,%hexn,1) @ %hex;    
		}

		if(strLen(%hex) == 1)
			%hex = "0" @ %hex;
		if(!strLen(%hex))
			%hex = "00";

		%hexstr = %hexstr @ %hex;
	}

	if(%hexstr $= "") {
		%hexstr = "FF00FF";
	}
	return %hexstr;
}

function SimObject::doCall(%this, %method, %arg0, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8, %arg9)
{
	return call(%method, %this, %arg0, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8, %arg9);
}

// //why is this not defined in dedicated servers
// function SimObject::Call(%this, %method, %arg0, %arg1, %arg2, %arg3, %arg4, %arg5, %arg6, %arg7, %arg8, %arg9)
// {
// 	for (%i = 0; %arg[%i] !$= ""; %i++)
// 		%args = %args @ %arg[%i] @ ",";
	
// 	%args = getSubStr(%args, 0, getMax(strLen(%args) - 1, 0));

// 	%string = %this @ "." @ %method @ "(" @ %args @ ");";
// 	eval(%string);
// }

// function SimObject::call(%this, %method, %v0, %v1, %v2, %v3, %v4, %v5, %v6, %v7, %v8, %v9, %v10, %v11, %v12, %v13, %v14, %v15, %v16, %v17)
// {
// 	%lastNull = -1;
// 	for(%i = 0; %i < 18; %i ++)
// 	{
// 		%a = %v[%i];
// 		if(%a $= "")
// 		{
// 			if(%lastNull < 0)
// 				%lastNull = %i;
// 			continue;
// 		}
// 		else
// 		{
// 			if(%lastNull >= 0)
// 			{
// 				for(%e = %lastNull; %e < %i; %e ++)
// 				{
// 					if(%args !$= "")
// 						%args = %args @ ",";
// 					%args = %args @ "\"\"";
// 				}
// 				%lastNull = -1;
// 			}
// 			if(%args !$= "")
// 				%args = %args @ ",";
// 			%args = %args @ "\"" @ %a @ "\"";
// 		}
// 	}

// 	eval(%this @ "." @ %method @ "(" @ %args @ ");");
// }

function hasWord(%str, %word)
{
	for (%i = 0; %i < getWordCount(%str); %i++)
		if (getWord(%str, %i) $= %word)
			return true;
			
	return false;
}

function hasField(%fields, %field)
{
	%count = getFieldCount(%fields);

	for (%i = 0; %i < %count; %i++)
		if (strStr(%field, getField(%fields, %i)) == 0)
			return 1;

	return 0;
}

function getFieldIndex(%fields, %field)
{
	%count = getFieldCount(%fields);

	for (%i = 0; %i < %count; %i++)
		if (strStr(%field, getField(%fields, %i)) == 0)
			return %i;

	return -1;
}

function randomRound(%val)
{
	return %val - mFloor(%val) > getRandom() ? mCeil(%val) : mFloor(%val);
}

function removeFieldText(%fields, %field)
{
	return removeField(%fields, getFieldIndex(%fields, %field));
}

function SimSet::pushFrontToBack(%set)
{
	if (%set.getCount() > 0)
		%set.pushToBack(%set.getObject(0));
}

function SimSet::Shuffle(%set)
{
	%count = %set.getCount();
	
	while (%count > 0)
	{
		%count--;
		%obj = %set.getObject(getRandom(0, %count));
		%set.pushToBack(%obj);
	}
}

function ServerCmdServerStats(%client)
{
	if (%client.viewingServerStats)
	{
		cancel(%client.ssLoop);
		%client.chatMessage("Server Stats view OFF.");
	}
	else
	{
		%client.ssLoop = %client.schedule(100, "viewServerStats");
		%client.chatMessage("Server Stats view ON.");
	}
	%client.viewingServerStats = !%client.viewingServerStats;
}

function GameConnection::viewServerStats(%client)
{
	cancel(%client.ssLoop);
	%client.centerPrint("<just:left>Server FPS: " @ $FPS::Real @ "<br>Schedules: " @ getNumSchedules() @ "<br>Sim Time: " @ getSimTime(), 2);
	%client.ssLoop = %client.schedule(1000, "viewServerStats");
}

function ServerCmdGetAllMats(%client)
{
	if (%client.isSuperAdmin || $Pref::Server::SAEX2::DevMode)
	{
		for (%i = 0; %i < MatterData.getCount(); %i++)
			$EOTW::Material[%client.bl_id, MatterData.getObject(%i).name] += 500000;
			
		%client.chatMessage("Inventory updated.");
	}
}

function ServerCmdClearAllMats(%client)
{
	if (%client.isSuperAdmin || $Pref::Server::SAEX2::DevMode)
	{
		for (%i = 0; %i < MatterData.getCount(); %i++)
			$EOTW::Material[%client.bl_id, MatterData.getObject(%i).name] = 0;
			
		%client.chatMessage("Inventory updated.");
	}
}

function ServerCmdReloadCode(%client, %section)
{
	if(%client.isSuperAdmin)
	{
		if(%section $= "Power")
		{
			exec("add-ons/gamemode_solar_apoc_expanded2_fixed/Modules/Power/Power.cs");
		}

		else
		{
			exec("add-ons/gamemode_solar_apoc_expanded2_fixed/server.cs");
		}

		createuinametable();
		transmitdatablocks();
		commandtoall('missionstartphase3');
		
		%client.chatMessage("Code reloaded.");
	}
}

function ServerCmdDumpMatSpawnRates(%client)
{
	if (%client.isSuperAdmin)
	{
		for (%i = 0; %i < MatterData.getCount(); %i++)
		{
			%matter = MatterData.getObject(%i);
			if (%matter.spawnWeight > 0)
			{
				talk(%matter.name @ ": " @ ((%matter.spawnWeight / $EOTW::MatSpawnWeight) * 100) @ "%");
			}
		}
	}
}

function traceSecond()
{
	cancel($traceSecondSched);
	echo("----traceSecond started");
	$traceSecondStart = getSimTime();
	$traceSecondSched = schedule(33, 0, traceSecondLoop);
	trace(1);
}

function traceSecondLoop()
{
	if(getSimTime() - $traceSecondStart > 1000)
	{
		trace(0);
		echo("----traceSecond ended");
		return;
	}

	cancel($traceSecondSched);
	$traceSecondSched = schedule(33, 0, traceSecondLoop);
}

function serverCmdDonate(%client,%receiver,%amt,%mat1,%mat2,%mat3,%mat4)
{
	%mat = trim(%mat1 SPC %mat2 SPC %mat3 SPC %mat4);
	%amt = mFloor(%amt);
	if (%receiver $= "" || %amt $= "" || %mat $= "")
	{
		messageClient(%client, '', "Usage: /donate <receiver> <amount> <material>");
		return;
	}
	%rc = findClientByName(%receiver);
	if (%rc == 0)
	{
		messageClient(%client, '', "Could not find user:" SPC %receiver @ ".");
		return;
	}
	
	if (%rc == %client)
	{
		messageClient(%client, '', "You can't donate to yourself.");
		return;
	}

	if (%rc.tutorialStep < 10)
	{
		messageClient(%client, '', "Let them finish the tutorial first before donating.");
		return;
	}
	
	if (%amt < 1)
	{
		messageClient(%client, '', "You need to donate atleast one material.");
		return;
	}
	
	if (!isObject(%matter = getMatterType(%mat)))
	{
		messageClient(%client, '', %mat SPC "is not a material.");
		return;
	}
	
	if ($EOTW::Material[%client.bl_id, %mat] < %amt)
	{
		messageClient(%client, '', "You can't donate than more of what you have. (You have " @ $EOTW::Material[%client.bl_id, %mat] @ " of the material you selected.)");
		return;
	}
	
	$EOTW::Material[%client.bl_id, %matter.name] -= %amt;
	$EOTW::Material[%rc.bl_id, %matter.name] += %amt;

	messageClient(%client, '', "\c6You donated\c4 " @ %amt @ " \c2" @ %matter.name @ " \c6to\c5 " @ %rc.netName @ "\c6.");
	messageClient(%rc, '', "\c5" @ %client.netName @ "\c6 gave you \c4" @ %amt @ " \c2" @ %matter.name @ "\c6.");
}


function setNewSkyBox(%dml)
{
	for (%i = 0; %i < $EnvGUIServer::SkyCount; %i++)
	{
		if ($EnvGUIServer::Sky[%i] $= %dml)
		{
			servercmdEnvGui_SetVar(EnvMaster, "SkyIdx", %i);
			break;
		}
	}
}

function setNewWater(%water)
{
	for (%i = 0; %i < $EnvGUIServer::WaterCount; %i++)
	{
		if ($EnvGUIServer::Water[%i] $= %water)
		{
			servercmdEnvGui_SetVar(EnvMaster, "WaterIdx", %i);
			break;
		}
	}
}

function purgeAllGatherables()
{
	%dataNames = "brickEOTWOilGeyserData";
	for (%i = 0; %i < MainBrickGroup.getCount(); %i++)
	{
		%group = MainBrickGroup.getObject(%i);
		for (%j = 0; %j < %group.getCount(); %j++)
		{
			%brick = %group.getObject(%j);
			if (hasWord(%dataNames, %brick.getDatablock().getName()))
				%brick.delete();
		}
	}
}

function periodicConsoleLogReset()
{
	cancel($EOTW::LogReset);
	setLogMode(0);
	setLogMode(2);

	$EOTW::LogReset = schedule(1000 * 120, 0, "periodicConsoleLogReset");
}

//TODO: getMin, getMax (lol)
function mMin(%a, %b)
{
	if (%a > %b)
		return %b;
	else
		return %a;
}

function mMax(%a, %b)
{
	if (%a < %b)
		return %b;
	else
		return %a;
}


function serverCmdGullible(%cl)
{
	if ($EOTW::ColorScroll[%cl.bl_id] $= "")
	{
		%cl.chatMessage("\c5Super secret rainbow player name activated!");
		%cl.player.ScrollNameColor();
	}
	else
	{
		%cl.chatMessage("\c5Super secret rainbow player name deactivated!");
		cancel($EOTW::ColorScroll[%cl.bl_id]);
		$EOTW::ColorScroll[%cl.bl_id] = "";
		%cl.player.setShapeNameColor("1 1 1");
	}
}

function Player::ScrollNameColor(%obj,%scroll)
{
	%scroll++;
	switch (%scroll)
	{
		case  1: %obj.setShapeNameColor("1.00 0.00 0.00");
		case  2: %obj.setShapeNameColor("1.00 0.42 0.00");
		case  3: %obj.setShapeNameColor("1.00 0.72 0.00");
		case  4: %obj.setShapeNameColor("0.71 1.00 0.00");
		case  5: %obj.setShapeNameColor("0.30 1.00 0.00");
		case  6: %obj.setShapeNameColor("0.00 1.00 0.12");
		case  7: %obj.setShapeNameColor("0.00 1.00 0.35");
		case  8: %obj.setShapeNameColor("0.00 1.00 1.00");
		case  9: %obj.setShapeNameColor("0.00 0.58 1.00");
		case 10: %obj.setShapeNameColor("0.00 0.15 1.00");
		case 11: %obj.setShapeNameColor("0.28 0.00 1.00");
		case 12: %obj.setShapeNameColor("0.70 0.00 1.00");
		case 13: %obj.setShapeNameColor("1.00 0.00 0.86");
		case 14: %obj.setShapeNameColor("1.00 0.00 0.43"); %scroll = 0;
	}
	
	$EOTW::ColorScroll[%obj.client.bl_id] = %obj.schedule(1000 / 14,"ScrollNameColor",%scroll);
}

//A slightly alternate version of /doicon that is easier to use
//Don't forget to do /iconinit and then set env to old brick icon sky
function serverCmdDoSelectedIcon (%client)
{
	if (!%client.isAdmin)
	{
		return;
	}
	%brickData = %client.inventory[%client.currInv];

	if (!isObject(%brickData))
		return;
		
	if (isObject ($iconBrick))
	{
		$iconBrick.delete ();
	}
	$iconBrick = new fxDTSBrick ("")
	{
		dataBlock = %brickData;
		isPlanted = 1;
	};
	MissionCleanup.add ($iconBrick);
	$iconBrick.setTransform ("0 10 -1005 0 0 1 " @ %brickData.orientationFix * $piOver2);
	schedule (3000, 0, doIconScreenshot);
}