exec("./Support_CenterprintMenuSystem.cs");
if (isFunction(RTB_registerPref))
{
	RTB_registerPref("Require trust to access", "Storage Events", "$Pref::Server::StorageEvents::RequireTrustAccess", "bool", "Event_Storage", 0, 0, 0);
	RTB_registerPref("Require trust to deposit", "Storage Events", "$Pref::Server::StorageEvents::RequireTrustDeposit", "bool", "Event_Storage", 0, 0, 0);
}
else
{
	if ($Pref::Server::StorageEvents::RequireTrustAccess $= "") $Pref::Server::StorageEvents::RequireTrustAccess = 0;
	if ($Pref::Server::StorageEvents::RequireTrustAccess $= "") $Pref::Server::StorageEvents::RequireTrustDeposit = 0;
}



package StorageEvents
{
	function serverCmdClearEvents (%client)
	{
		%brick = %client.wrenchbrick;
		if(!isObject(%brick))
			return parent::serverCmdClearEvents(%client);
		
		%data = %brick.getDataBlock();
		if(%data.maxStoredTools > 0 && !%client.isAdmin)
		{
			%client.centerprint("\c6You're not allowed to modify events on a storage brick!", 3);
			return;
		}

		return parent::serverCmdClearEvents(%client);
	}

	function serverCmdAddEvent (%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4)
	{
		%brick = %client.wrenchbrick;
		if(!isObject(%brick))
			return parent::serverCmdAddEvent (%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
		
		%data = %brick.getDataBlock();
		if(%data.maxStoredTools > 0 && !%client.isAdmin && !%brick.ignoreEventRestriction)
		{
			%client.centerprint("\c6You're not allowed to modify events on a storage brick!", 3);
			return;
		}
		%brick.ignoreEventRestriction = false;

		return parent::serverCmdAddEvent (%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
	}


	function fxDTSBrick::onRemove(%brick)
	{
		%data = %brick.getDatablock();
		if (%data.maxStoredTools > 0)
		{
			for (%i = 0; %i < %brick.numEvents; %i++)
			{
				%enabled = %brick.eventEnabled[%i];
				if (!%enabled)
				{
					continue;
				}
				%outputEventName = $OutputEvent_Name[%brick.getClassName(), %brick.eventOutputIdx[%i]];
				if (%outputEventName $= "openStorage")
				{
					%str1 = %brick.eventOutputParameter[%i, 2];
					%str2 = %brick.eventOutputParameter[%i, 3];
					%str3 = %brick.eventOutputParameter[%i, 4];
					%str = " " @ trim(%str1 SPC %str2 SPC %str3) @ " ";
					break;
				}
			}

			if (%str !$= "")
			{
				for (%i = 0; %i < getWordCount(%str); %i++)
				{
					%item = getWord(%str, %i);

					if (!isObject(%item))
						continue;
					%error = %brick.removeStoredItem(%item);
					if (%error == -1)
						continue;

					%brick.updateStorageMenu();
					
					%itemDrop = new Item()
					{
						dataBlock = %item;
					};
					MissionCleanup.add(%itemDrop);
					%itemDrop.setTransform(%brick.getTransform());
					%itemDrop.schedule(60000, schedulePop); // add 60 seconds of extra lifetime to ensure they can pick up the item
				}
			}
			
		}

		if (isObject(%brick.centerprintMenu))
		{
			%brick.centerprintMenu.delete();
		}

		return parent::onRemove(%brick);
	}

	function serverCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%item = %pl.tool[%slot];
			%start = %pl.getEyePoint();
			%end = vectorAdd(vectorScale(%pl.getEyeVector(), 6), %start);
			%hit = getWord(containerRaycast(%start, %end, $Typemasks::fxBrickObjectType), 0);
			if (isObject(%hit) && %hit.getStoredItemSpace() > 0 && %hit.getDataBlock().maxStoredTools > 0)
			{
				if (getTrustLevel(%cl, %hit) < $Pref::Server::StorageEvents::RequireTrustDeposit)
				{
					if (getBrickgroupFromObject(%hit).bl_id != 888888)
					{
						%cl.centerprint(%hit.getGroup().name @ " does not trust you enough to do that!", 1);
					}
					return parent::serverCmdDropTool(%cl, %slot);
				}

				if (%pl.toolMag[%slot] !$= "")
					%toolData = trim(%toolData TAB "mag" TAB %pl.toolMag[%slot]); //Note the data goes into the dropped item when spawned, so we need to use the respective vars.

				%error = %hit.storeItem(%item, %toolData);
				if (!%error)
				{
					%pl.tool[%slot] = 0;
					$hl2weaponMag = "";
					%pl.toolMag[%slot] = "";
					messageClient(%cl, 'MsgItemPickup', "", %slot, 0);
					if (%pl.currTool == %slot)
					{
						serverCmdUnuseTool(%cl, %slot);
					}
					return;
				}
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}
	function WrenchImage::onHitObject(%this, %obj, %slot, %col, %pos, %normal)
	{
		if(%col.getClassName() !$= "fxDTSBrick")
			return Parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
		
		%data = %col.getDatablock();
		if(%data.maxStoredTools <= 0)
			return Parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);

		%adminOnly = $Server::WrenchEventsAdminOnly;
		$Server::WrenchEventsAdminOnly = 1;
		%ret = Parent::onHitObject(%this, %obj, %slot, %col, %pos, %normal);
		$Server::WrenchEventsAdminOnly = %adminOnly;
		return %ret;
	}
	function fxDTSBrick::onPlant(%this,%brick)
	{
		Parent::onPlant(%this,%brick);
	}
};
schedule(1000, 0, activatePackage, StorageEvents);






// attempts to store %item (datablock) into %brick, looking for an available event slot to occupy
// returns error code
// 0 if no error, 1 if no space, 2 if space but failed to insert due to string lengths
function fxDTSBrick::storeItem(%brick, %item, %toolData)
{
	if (%brick.getDatablock().maxStoredTools < 1)
		return 1;

	%item = %item.getName();
	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%enabled = %brick.eventEnabled[%i];
		if (!%enabled)
		{
			continue;
		}
		%outputEventName = $OutputEvent_Name[%brick.getClassName(), %brick.eventOutputIdx[%i]];
		if (%outputEventName $= "openStorage")
		{
			%max = %brick.eventOutputParameter[%i, 1];
			%str1 = %brick.eventOutputParameter[%i, 2];
			%str2 = %brick.eventOutputParameter[%i, 3];
			%str3 = %brick.eventOutputParameter[%i, 4];
			%count = getStringItemCount(%str1, %str2, %str3);
			if (%count < %max)
			{
				%validEventSlots = %validEventSlots SPC %i;
				%validEventSlotCount++;
			}
		}
	}
	%validEventSlots = trim(%validEventSlots);

	if (%validEventSlots !$= "")
	{
		// attempt storing in every valid event found
		for (%i = 0; %i < %validEventSlotCount; %i++)
		{
			%slot = getWord(%validEventSlots, %i);
			%error = %brick.insertIntoEventStorage(%item, %slot, %toolData);
			if (%error == 0)
			{
				%brick.updateStorageMenu();
				return 0;
			}
		}
		return 2;
	}
	else
	{
		return 1;
	}
}

// attempts to insert %item (datablock) into storage event string on brick
// %slot corresponds to event line #
// returns error code
// 0 if no error, 1 if failed to insert due to string lengths
function fxDTSBrick::insertIntoEventStorage(%brick, %item, %slot, %toolData)
{
	%str1 = %brick.eventOutputParameter[%slot, 2];
	%str2 = %brick.eventOutputParameter[%slot, 3];
	%str3 = %brick.eventOutputParameter[%slot, 4];
	%result = parseItemList(%str1 SPC %str2 SPC %str3 SPC %item, 200);
	if (%result == -1)
	{
		return 1;
	}
	else
	{
		%count = getWord(%result, 0);

		if (%toolData !$= "")
			%brick.storedToolData[%count - 1] = %toolData;

		for (%i = 0; %i < 3; %i++)
		{
			if (%i < %count)
			{
				%brick.eventOutputParameter[%slot, %i + 2] = getField(%result, %i + 1);
			}
			else
			{
				%brick.eventOutputParameter[%slot, %i + 2] = "";
			}
		}
		return 0;
	}
}

// attempts to removed stored %item (datablock) from %brick, looking in any available storage event
// returns error code
// 0 if no error, 1 if item not found
function fxDTSBrick::removeStoredItem(%brick, %item)
{
	%item = strLwr(%item.getName());
	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%enabled = %brick.eventEnabled[%i];
		if (!%enabled)
		{
			continue;
		}
		%outputEventName = $OutputEvent_Name[%brick.getClassName(), %brick.eventOutputIdx[%i]];
		if (%outputEventName $= "openStorage")
		{
			%str1 = %brick.eventOutputParameter[%i, 2];
			%str2 = %brick.eventOutputParameter[%i, 3];
			%str3 = %brick.eventOutputParameter[%i, 4];
			%str = " " @ trim(%str1 SPC %str2 SPC %str3) @ " ";
			if (strPos(strLwr(%str), " " @ %item @ " ") >= 0)
			{
				%index = getWordIndex(%str, %item);
				if (%brick.storedToolData[%index - 1] !$= "")
				{
					%toolData = %brick.storedToolData[%index - 1];
					%brick.storedToolData[%index - 1] = "";
				}
				//item found, remove and return
				//%items = trim(strReplace(strLwr(%str), " " @ %item @ " ", " "));
				%items = trim(removeWord(strLwr(%str), %index));
				%items = parseItemList(%items, 200);
				%brick.eventOutputParameter[%i, 2] = getField(%items, 1);
				%brick.eventOutputParameter[%i, 3] = getField(%items, 2);
				%brick.eventOutputParameter[%i, 4] = getField(%items, 3);
				
				%brick.updateStorageMenu();
				return %toolData;
			}
		}
	}
	return -1;
}

// returns space-delimited word list of all items stored
function fxDTSBrick::getAllStoredItems(%brick)
{
	%list = "";
	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%enabled = %brick.eventEnabled[%i];
		if (!%enabled)
		{
			continue;
		}
		%outputEventName = $OutputEvent_Name[%brick.getClassName(), %brick.eventOutputIdx[%i]];
		if (%outputEventName $= "openStorage")
		{
			%max = %brick.eventOutputParameter[%i, 1];
			%str1 = %brick.eventOutputParameter[%i, 2];
			%str2 = %brick.eventOutputParameter[%i, 3];
			%str3 = %brick.eventOutputParameter[%i, 4];
			%append = trim(%str1 SPC %str2 SPC %str3);
			%list = %list SPC %append;
		}
	}
	return trim(%list);
}

// returns number of empty spaces available, max space, and total current count
function fxDTSBrick::getStoredItemSpace(%brick)
{
	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%enabled = %brick.eventEnabled[%i];
		if (!%enabled)
		{
			continue;
		}
		%outputEventName = $OutputEvent_Name[%brick.getClassName(), %brick.eventOutputIdx[%i]];
		if (%outputEventName $= "openStorage")
		{
			%max = %brick.eventOutputParameter[%i, 1];
			%str1 = %brick.eventOutputParameter[%i, 2];
			%str2 = %brick.eventOutputParameter[%i, 3];
			%str3 = %brick.eventOutputParameter[%i, 4];
			%count += getStringItemCount(%str1, %str2, %str3);
			%totalMax += %max;
		}
	}
	return %totalMax - %count SPC %totalMax SPC %count;
}

// returns total number of extant slots occupied
// does not check if item datablock exists, in case of stored but missing datablocks
function getStringItemCount(%str1, %str2, %str3)
{
	for (%i = 1; %i < 4; %i++)
	{
		%count += getWordCount(%str[%i]);
	}
	return %count;
}

// returns field count + up to 3 fields that are <= %strLenMax chars
// -1 indicates items were lost/item list is too long
// assuming around 20-25 chars per item, approximately 20-30 items can be stored max
function parseItemList(%items, %strLenMax)
{
	// iterate over items in list, build strings of length %strLenMax or less
	%strCount = 0;
	%currStr = "";
	for (%i = 0; %i < getWordCount(%items); %i++)
	{
		// skip empty slots
		%nextItem = getWord(%items, %i);
		if (%nextItem $= "")
		{
			continue;
		}
		if (strLen(%nextItem) > %strLenMax) //word too long to fit
		{
			return - 1;
		}

		%nextStr = trim(%currStr SPC %nextItem);
		if (strLen(%nextStr) > %strLenMax)
		{
			%str[%strCount] = %currStr;
			%currStr = getWord(%items, %i);
			%strCount++;
		}
		else
		{
			%currStr = %nextStr;
		}

		if (%strCount > 2) //too many strings needed
		{
			return -1;
		}
	}
	// lock in current string
	if (%currStr !$= "")
	{
		%str[%strCount] = %currStr;
		%currStr = getWord(%items, %i);
		%strCount++;
	}

	if (%strCount == 0 || strLen(%currStr) > %strLenMax)
	{
		return -1;
	}
	else
	{
		return %strCount TAB trim(%str0 TAB %str1 TAB %str2);
	}
}














function fxDTSBrick::openStorage(%brick, %max, %str1, %str2, %str3, %cl)
{
	if (%brick.getDatablock().maxStoredTools < 1)
		return;

	if ($Pref::Server::StorageEvents::RequireTrustAccess && getTrustLevel(%cl, %brick) < 2 && getBrickgroupFromObject(%brick).bl_id != 888888)
	{
		%cl.centerprint(%brick.getGroup().name @ " does not trust you enough to do that!", 1);
		return;
	}

	%brick.updateStorageMenu();

	%cl.startCenterprintMenu(%brick.centerprintMenu);
	storageLoop(%cl, %brick);
}

function fxDTSBrick::setStorageName(%brick, %name, %cl)
{
	if (%brick.getDatablock().maxStoredTools < 1)
		return;
		
	%brick.updateStorageMenu();
	%brick.centerprintMenu.menuName = %name;
}

registerOutputEvent("fxDTSBrick", "openStorageData", "string 200 85" TAB "string 200 85" TAB "string 200 85", 1);
registerOutputEvent("fxDTSBrick", "openStorage", "int 1 20 1" TAB "string 200 85" TAB "string 200 85" TAB "string 200 85", 1);
registerOutputEvent("fxDTSBrick", "setStorageName", "string 200 200", 1);

function storageLoop(%cl, %brick)
{
	cancel(%cl.storageSchedule);

	if (!isObject(%brick) || !isObject(%cl.player) || !%cl.isInCenterprintMenu)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	if (vectorDist(%brick.getPosition(), %cl.player.getPosition()) > 6)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	%start = %cl.player.getEyePoint();
	%end = vectorAdd(vectorScale(%cl.player.getEyeVector(), 8), %start);
	if (containerRaycast(%start, %end, $Typemasks::fxBrickObjectType) != %brick)
	{
		%cl.exitCenterprintMenu();
		return;
	}

	%brick.updateStorageMenu();
	%cl.displayCenterprintMenu(0);

	%cl.storageSchedule = schedule(200, %cl, storageLoop, %cl, %brick);
}

function fxDTSBrick::updateStorageMenu(%brick)
{
	if (!isObject(%brick.centerprintMenu))
	{
		%brick.centerprintMenu = new ScriptObject(StorageCenterprintMenus)
		{
			isCenterprintMenu = 1;
			menuName = %brick.getDatablock().uiName @ " Contents";

			// menuOption[0] = "Empty";

			// menuFunction[0] = "removeItem";
			menuOptionCount = 0;
			brick = %brick;
		};
		MissionCleanup.add(%brick.centerprintMenu);
	}
	// populate menu
	%menu = %brick.centerprintMenu;
	%items = %brick.getAllStoredItems();
	%empty = %brick.getStoredItemSpace();

	%itemCount = getWordCount(%items);
	%total = %itemCount + %empty;
	%preTotal = %menu.menuOptionCount;
	%menu.menuOptionCount = %total;

	%curr = 0;
	// items
	for (%i = 0; %i < %itemCount; %i++)
	{
		%item = getWord(%items, %i);
		if (isObject(%item) && %item.getClassName() $= "ItemData")
		{
			%menu.menuOption[%curr] = (%curr + 1) @ ". " @ (%item.uiName $= "" ? %item : %item.uiName);
			%menu.menuOptionItem[%curr] = %item;
			%menu.menuFunction[%curr] = "removeItem";
		}
		else
		{
			%menu.menuOption[%curr] = (%curr + 1) @ ". \c0[INVALID ITEM ERROR]";
			%menu.menuOptionItem[%curr] = %item;
			%menu.menuFunction[%curr] = "removeItem";
		}
		%curr++;
	}
	// empty space
	for (%i = 0; %i < %empty; %i++)
	{
		%menu.menuOption[%curr] = (%curr + 1) @ ". None";
		%menu.menuOptionItem[%curr] = "";
		%menu.menuFunction[%curr] = "removeItem";
		%curr++;
	}
	// remove excess menu options
	for (%i = %curr; %i < %preTotal; %i++)
	{
		%menu.menuOption[%curr] = "";
		%menu.menuOptionItem[%curr] = "";
		%menu.menuFunction[%curr] = "";
		%curr++;
	}
}

function removeItem(%cl, %menu, %option)
{
	cancel(%cl.storageSchedule);
	if (!isObject(%cl.player))
	{
		%cl.centerprint("You are dead!", 3);
		return;
	}
	%brick = %menu.brick;

	if (%brick.getDatablock().maxStoredTools < 1)
		return 1;

	if (vectorDist(%brick.getPosition(), %cl.player.getHackPosition()) > 7)
	{
		%cl.centerprint("You are too far away from the container!", 3);
		return;
	}

	if (getWords(%menu.menuOption[%option], 1, 10) $= "None")
	{
		if (%cl.nextMessageEmpty < $Sim::Time)
			messageClient(%cl, '', "That slot is empty!");

		%cl.nextMessageEmpty = $Sim::Time + 2;

		%brick.updateStorageMenu();
		reopenCenterprintMenu(%cl, %menu, %option);
		storageLoop(%cl, %brick);
		return;
	}

	// handle cases of missing datablocks or removal failing
	%item = %menu.menuOptionItem[%option];
	if (!isObject(%item))
	{
		%cl.centerprint("INVALID ITEM \"" @ %item @ "\"! Please notify the host!", 3);
		messageClient(%cl, '', "INVALID ITEM \"" @ %item @ "\"! Please notify the host!");
		return;
	}
	%toolData = %brick.removeStoredItem(%item);
	if (%toolData == -1)
	{
		%cl.centerprint("Failed to remove item \"" @ %item @ "\"! Please notify the host!", 3);
		messageClient(%cl, '', "Failed to remove item \"" @ %item @ "\"! Please notify the host!");
		return;
	}

	%brick.updateStorageMenu();
	
	%i = new Item()
	{
		dataBlock = %item;
	};
	MissionCleanup.add(%i);

	for (%k = 0; %k < getFieldCount(%toolData); %k += 2)
		set_var_obj(%i, getField(%toolData, %k), getField(%toolData, %k + 1));

	%i.setTransform(%cl.player.getTransform());
	%i.schedule(60000, schedulePop); // add 60 seconds of extra lifetime to ensure they can pick up the item
	%cl.player.pickup(%i);

	%brick.updateStorageMenu();
	reopenCenterprintMenu(%cl, %menu, %option);
	storageLoop(%cl, %brick);
}
