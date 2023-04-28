datablock fxDTSBrickData(brickEOTWMatterBin1Data)
{
	brickFile = "./Shapes/BarrelThin.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "Matter Bin I";
	collisionShapeName = "./Shapes/BarrelThin.dts";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Matter/Icons/BarrelThin";

	hasPrint = 1;
	printAspectRatio = "1x1";

    hasInventory = true;
    matterSize = 1024;
	matterSlots["Buffer"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWMatterBin1Data"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterBin1Data"] = "A matter bin used for binning matter. Has 1 slot, 1024u of space.";

datablock fxDTSBrickData(brickEOTWMatterBin2Data)
{
	brickFile = "./Shapes/barrelThick.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "Matter Bin II";
	collisionShapeName = "./Shapes/barrelThick.dts";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Matter/Icons/BarrelThick";

	hasPrint = 1;
	printAspectRatio = "1x1";

    hasInventory = true;
    matterSize = 16384;
	matterSlots["Buffer"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWMatterBin2Data"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterBin2Data"] = "A larger matter bin used for binning more matter. Has 1 slot, 16384u of space.";

function fxDtsBrick::ChangeMatter(%obj, %matterName, %amount, %type)
{
	if(!isObject(GetMatterType(%matterName)))
        return 0;

    %data = %obj.getDatablock();
	%amount = mRound(%amount);
	
	//Try to add to existing matter.
	//We will only allow a unique matter to occupy 1 slot in its slot type.
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matter = %obj.matter[%type, %i];
		
		if (getField(%matter, 0) $= %matterName)
		{
			%testAmount = getField(%matter, 1) + %amount;
			%change = %amount;
			
			if (%testAmount > %data.matterSize)
				%change = %data.matterSize - getField(%matter, 1);
			else if (%testAmount < 0)
				%change = getField(%matter, 1) * -1;
			
			%obj.matter[%type, %i] = getField(%matter, 0) TAB (getField(%matter, 1) + %change);
			
			if ((getField(%matter, 1) + %change) <= 0)
				%obj.matter[%type, %i] = "";
			
			return %change;
		}
	}
	
	//So we don't remove from an empty slot, somehow
	if (%amount <= 0)
		return 0;
	
	//Double pass to add to an empty slot.
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matter = %obj.matter[%type, %i];
		
		if (getField(%matter, 0) $= "")
		{
			%change = %amount > %data.matterSize ? %data.matterSize : %amount;
			%obj.matter[%type, %i] = %matterName TAB %change;
			
			return %change;
		}
	}
	
	return 0;
}

function fxDtsBrick::GetMatter(%obj, %matter, %type)
{
	%data = %obj.getDatablock();
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matterData = %obj.matter[%type, %i];
		
		if (getField(%matterData, 0) $= %matter)
			return getField(%matterData, 1);
	}
	
	return 0;
}

function fxDtsBrick::getEmptySlotCount(%obj, %slot)
{
	%data = %obj.getDatablock();
	%count = 0;

	for (%i = 0; %i < %data.matterSlots[%slot]; %i++)
		if (%obj.matter[%slot, %i] $= "")
			%count++;

	return %count;
}

function ServerCmdI(%client, %slot, %amount, %material, %matB, %matC, %matD) { ServerCmdInsert(%client, %slot, %amount, %material, %matB, %matC, %matD); }
function ServerCmdInput(%client, %slot, %amount, %material, %matB, %matC, %matD) { ServerCmdInsert(%client, %slot, %amount, %material, %matB, %matC, %matD); }
function ServerCmdInsert(%client, %slot, %amount, %material, %matB, %matC, %matD)
{
	if (!isObject(%player = %client.player))
		return;

	%material = trim(%material SPC %matB SPC %matC SPC %matD);

	if (%amount <= 0 || %amount > 999999 || %material $= "" || %slot $= "")
	{
		%client.chatMessage("Usage: /Insert <input (i)/output (o)/buffer (b)> <amount> <material>");
		return;
	}

	switch$ (%slot)
	{
		case "i": %slot = "Input";
		case "b": %slot = "Buffer";
		case "o": %slot = "Output";
	}

	%amount = Round(%amount);
	if(isObject(%hit = %player.whatBrickAmILookingAt()) && %hit.getClassName() $= "fxDtsBrick")
	{
		if (getTrustLevel(%player, %hit) < $TrustLevel::Hammer)
		{
			if (%hit.stackBL_ID $= "" || %hit.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}
		%data = %hit.getDatablock();
		if (%data.matterSlots[%slot] > 0)
		{
			if (isObject(%matter = GetMatterType(%material)))
			{
				%change = getMin(%amount, $EOTW::Material[%client.bl_id, %matter.name]);

				%finalChange = %hit.changeMatter(%matter.name, %change, %slot);

				%client.chatMessage("You input " @ %finalChange @ " units of " @ %matter.name @ " into the " @ %slot @ ".");
				$EOTW::Material[%client.bl_id, %matter.name] -= %finalChange;
			}
			else
				%client.chatMessage("Material type " @ %material @ " not found.");
		}
		else
			%client.chatMessage("This block has no compatible \"" @ %slot @ "\" slot.");
	}
}

function ServerCmdE(%client, %slot, %amount, %material, %matB, %matC, %matD) { ServerCmdExtract(%client, %slot, %amount, %material, %matB, %matC, %matD); }
function ServerCmdOutput(%client, %slot, %amount, %material, %matB, %matC, %matD) { ServerCmdExtract(%client, %slot, %amount, %material, %matB, %matC, %matD); }
function ServerCmdExtract(%client, %slot, %amount, %material, %matB, %matC, %matD)
{
	if (!isObject(%player = %client.player))
		return;

	%material = trim(%material SPC %matB SPC %matC SPC %matD);

	if (%amount <= 0 || %amount > 999999 || %material $= "" || %slot $= "")
	{
		%client.chatMessage("Usage: /Extract <input (i)/output (o)/buffer (b)> <amount> <material>");
		return;
	}

	switch$ (%slot)
	{
		case "i": %slot = "Input";
		case "b": %slot = "Buffer";
		case "o": %slot = "Output";
	}

	%amount = Round(%amount);
	if(isObject(%hit = %player.whatBrickAmILookingAt()) && %hit.getClassName() $= "fxDtsBrick")
	{
		if (getTrustLevel(%player, %hit) < $TrustLevel::Hammer)
		{
			if (%hit.stackBL_ID $= "" || %hit.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}
		%data = %hit.getDatablock();
		if (%data.matterSlots[%slot] > 0)
		{
			if (isObject(%matter = GetMatterType(%material)))
			{
				%change = getMin(%amount, %hit.GetMatter(%matter.name, %slot)) * -1;

				%finalChange = %hit.changeMatter(%matter.name, %change, %slot) * -1;

				%client.chatMessage("Extracted " @ %finalChange @ " units of " @ %matter.name @ ".");
				$EOTW::Material[%client.bl_id, %matter.name] += %finalChange;
			}
			else
				%client.chatMessage("Material type " @ %material @ " not found.");
		}
		else
			%client.chatMessage("This brick has no " @ %slot @ " slot.");
	}
}

function ServerCmdAR(%client) { ServerCmdAddRecipe(%client); }
function ServerCmdAddRecipe(%client)
{
	if (!isObject(%player = %client.player))
		return;

	if(isObject(%hit = %player.whatBrickAmILookingAt()) && %hit.getClassName() $= "fxDtsBrick")
	{
		if (isObject(%craftingData = %hit.processingRecipe))
		{
			%client.chatMessage("Attempting to add recipe....");
			for (%j = 0; %craftingData.input[%j] !$= ""; %j++)
			{
				%input = %craftingData.input[%j];
				%type = getField(%input, 0);
				%cost = getField(%input, 1) * 128;
				ServerCmdInsert(%client, "Input", %cost, getWord(%type, 0), getWord(%type, 1), getWord(%type, 2), getWord(%type, 3));
			}
			return;
		}
		else
		{
			%client.chatMessage("This brick has no recipe set.");
		}
	}

}

function ServerCmdEA(%client, %slot) { ServerCmdExtractAll(%client, %slot); }
function ServerCmdExtractAll(%client, %slot)
{
	if (!isObject(%player = %client.player))
		return;

	%material = trim(%material SPC %matB SPC %matC SPC %matD);

	if (%slot $= "")
	{
		%client.chatMessage("Usage: /ExtractAll <input (i)/output (o)/buffer (b)/all (a)>");
		return;
	}

	switch$ (%slot)
	{
		case "i": %slot = "Input";
		case "b": %slot = "Buffer";
		case "o": %slot = "Output";
		case "a" or "all":
			ServerCmdExtractAll(%client, "Input");
			ServerCmdExtractAll(%client, "Output");
			ServerCmdExtractAll(%client, "Buffer");
			return;
	}

	%amount = Round(%amount);
	if(isObject(%hit = %player.whatBrickAmILookingAt()) && %hit.getClassName() $= "fxDtsBrick")
	{
		if (getTrustLevel(%player, %hit) < $TrustLevel::Hammer)
		{
			if (%hit.stackBL_ID $= "" || %hit.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}
		%data = %hit.getDatablock();
		if (%data.matterSlots[%slot] > 0)
		{
			for (%i = 0; %i < %data.matterSlots[%slot]; %i++)
			{
				%extract = %hit.matter[%slot, %i];
				%type = getField(%extract, 0);
				%cost = getField(%extract, 1);
				ServerCmdExtract(%client, %slot, %cost, getWord(%type, 0), getWord(%type, 1), getWord(%type, 2), getWord(%type, 3));
			}
		}
		else
			%client.chatMessage("This brick has no " @ %slot @ " slot.");
	}
}