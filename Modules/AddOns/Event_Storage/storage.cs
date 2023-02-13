// attempts to store %item (datablock) into %brick, looking for an available event slot to occupy
// returns error code
// 0 if no error, 1 if no space, 2 if space but failed to insert due to string lengths
function fxDTSBrick::storeItem(%brick, %item)
{
	for (%i = 0; %i < %brick.numEvents; %i++)
	{
		%outputEventIdx = %brick.eventOutputIdx[%i];
		%outputEventName = $OutputEvent_Name[%brick.getClassName(), %outputEventIdx];
		if (%outputEventName $= "eventStorage")
		{
			%max = %brick.eventOutputParameter[%i, 1];
			%str1 = %brick.eventOutputParameter[%i, 2];
			%str2 = %brick.eventOutputParameter[%i, 3];
			%str3 = %brick.eventOutputParameter[%i, 4];
			%count = getStoredItemCount(%str1, %str2, %str3);
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
		for (%i = 0; %i < %validEventSlotCount; %i++)
		{
			%error = %brick.insertIntoEventStorage(%item, %slot);
			if (%error == 0)
			{
				//no error, return 0 for no error
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

// returns total number of extant slots occupied
// does not check if item datablock exists, in case of temporarily missing datablocks
function getStoredItemCount(%str1, %str2, %str3)
{
	for (%i = 1; %i < 4; %i++)
	{
		%currStr = %str[%i];
		while (%currStr !$= "")
		{
			%currStr = nextToken(%currStr, "nextItem", "|");
			if (%nextItem !$= "")
			{
				%count++;
			}
		}
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
	%currStr = getWord(%items, 0);
	for (%i = 1; %i < getWordCount(%items); %i++)
	{
		%nextStr = %currStr SPC getWord(%items, %i);
		if (%nextStr > %strLenMax)
		{
			%currStr = strReplace(trim(%currStr), " ", "|");
			%str[%strCount] = %currStr;
			%currStr = getWord(%items, %i);
			%strCount++;
		}
		if (%strCount > 2)
		{
			return -1;
		}
		%currStr = %nextStr;
	}
	// lock in current string
	if (%currStr !$= "")
	{
		%currStr = strReplace(trim(%currStr), " ", "|");
		%str[%strCount] = %currStr;
		%currStr = getWord(%items, %i);
		%strCount++;
	}

	if (%strCount == 0)
	{
		return -1;
	}
	else
	{
		return trim(%strCount TAB %str0 TAB %str1 TAB %str2);
	}
}

function fxDTSBrick::updateStorageMenu(%brick)
{
	if (!isObject(%brick.centerprintMenu))
	{
		%brick.centerprintMenu = new ScriptObject(StorageCenterprintMenus)
		{
			isCenterprintMenu = 1;
			menuName = %brick.getDatablock().uiName @ " Contents";

			//menuOption[0] = "Empty";

			//menuFunction[0] = "removeItem";
			menuOptionCount = 0;
			brick = %brick;
		};
		MissionCleanup.add(%brick.centerprintMenu);
	}
	//populate menu
}



registerOutputEvent("fxDTSBrick", "displayStorageContents", "int 1 20 1" TAB "string 200 65" TAB "string 200 65" TAB "string 200 65", 1);
