function fxDtsBrick::rayCastPlayer(%obj)
{
	//TODO: Account for vertical brick size
	%eye = %obj.GetPosition();
	%dir = "0 0 1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::PlayerObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 2)), %mask, %obj);
	return firstWord(%ray);
}

datablock fxDTSBrickData(brickEOTWChargerHatchData)
{
	brickFile = "./Shapes/ChargePad.blb";
	category = "Solar Apoc";
	subCategory = "Hatches";
	uiName = "Charger Hatch";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Hatch";

    isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWChargerHatchData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Copper" TAB 256 TAB "Electrum" TAB 256 TAB "Lead";
$EOTW::BrickDescription["brickEOTWChargerHatchData"] = "Charges your personal battery. This can be used for tools like the Mining Scanner or Oil Pump.";

function brickEOTWChargerHatchData::onTick(%this, %obj) {
	%client = %obj.getGroup().client;
    if (isObject(%player = %client.player) && %obj.rayCastPlayer() == %player)
	{
		%change = %obj.drainPowerNet(getMin(%player.GetMaxBatteryEnergy() - %player.GetBatteryEnergy(), 500));
		%player.ChangeBatteryEnergy(%change);
		%player.lastBatteryRequest = getSimTime();
	}
}

datablock fxDTSBrickData(brickEOTWInputHatchData)
{
	brickFile = "./Shapes/ChargePad.blb";
	category = "Solar Apoc";
	subCategory = "Hatches";
	uiName = "Input Hatch";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Hatch";

    isPowered = true;
	powerType = "Machine";
	allowFiltering = true;

    matterSize = 128;
	matterSlots["Output"] = 1;
    inspectMode = 1;
	hasFiltering = true;
};
$EOTW::CustomBrickCost["brickEOTWInputHatchData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Silver" TAB 256 TAB "Red Gold" TAB 256 TAB "Lead";
$EOTW::BrickDescription["brickEOTWInputHatchData"] = "Deposits materials on a player into its buffer. Must be filtered (/setfilter) before use.";

function brickEOTWInputHatchData::onTick(%this, %obj) {
	%client = %obj.getGroup().client;
    if (isObject(%player = %client.player) && %obj.rayCastPlayer() == %player && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{
		for (%i = 0; %i < getFieldCount(%hit.machineFilter); %i++)
		{
			%matter = getMatterType(getField(%hit.machineFilter, %i));
			%amount = $EOTW::Material[%client.bl_id, %matter.name];
			%change = %obj.changeMatter(%matter.name, %amount, "Input");
			$EOTW::Material[%client.bl_id, %matter.name] -= %change;
		}
	}
}

datablock fxDTSBrickData(brickEOTWOutputHatchData)
{
	brickFile = "./Shapes/ChargePad.blb";
	category = "Solar Apoc";
	subCategory = "Hatches";
	uiName = "Output Hatch";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Hatch";

    isPowered = true;
	powerType = "Machine";

    matterSize = 128;
	matterSlots["Input"] = 1;
    inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWOutputHatchData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Iron" TAB 256 TAB "Steel" TAB 256 TAB "Lead";
$EOTW::BrickDescription["brickEOTWOutputHatchData"] = "Withdraws materials from itself into a player.";

function brickEOTWOutputHatchData::onTick(%this, %obj) {
    %client = %obj.getGroup().client;
    if (isObject(%player = %client.player) && %obj.rayCastPlayer() == %player && isObject(%matter = getMatterType(getField(%obj.matter["Input", 0], 0))) && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{
		%amount = getField(%obj.matter["Input", 0], 1);
		%change = %obj.changeMatter(%matter.name, %amount * -1, "Input");
		$EOTW::Material[%client.bl_id, %matter.name] -= %change;
	}
}

function ServerCmdF(%client, %mode, %mat1, %mat2, %mat3, %mat4) { ServerCmdFilter(%client, %mode, %mat1, %mat2, %mat3, %mat4); }
function ServerCmdFilter(%client, %mode, %mat1, %mat2, %mat3, %mat4)
{
    if (!isObject(%player = %client.player))
        return;

    %material = trim(%mat1 SPC %mat2 SPC %mat3 SPC %mat4);

    

	if(!isObject(%hit = %player.whatBrickAmILookingAt()))
	{
		%client.chatMessage("Usage: /Filter <add (a) / remove (r)> <material>. You must be looking at a valid brick. Leave out the arguments to just see the filter list.");
		return;
	}

	if (!%hit.getDatablock().allowFiltering)
	{
		%client.chatMessage("This command can only be applied to valid bricks.");
		return;
	}

	if (%material $= "")
	{
		%client.chatMessage("\c6Current Whitelist: " @ strReplace(%hit.machineFilter, "\t", ", "));
		return;
	}

	if (getTrustLevel(%player, %hit) < $TrustLevel::Hammer)
	{
		if (%hit.stackBL_ID $= "" || %hit.stackBL_ID != %client.getBLID())
		{
			%client.chatMessage("The owner of that object does not trust you enough.");
			return;
		}
	}
	if (!isObject(%matter = getMatterType(%material)))
	{
		%client.chatMessage("No material type by the name of \"" @ %material @ "\" exists.");
		return;
	}

	switch$ (%mode)
	{
		case "append" or "add" or "a":
			if (hasField(%hit.machineFilter, %matter.name))
			{
				%client.chatMessage(%matter.name @ " is already on the filter list.");
				return;
			}
			if (getFieldCount(%hit.machineFilter) > 9)
			{
				%client.chatMessage("The brick already has the maxinum amount of filters.");
				return;
			}
			%hit.machineFilter = trim(%hit.machineFilter TAB %matter.name);
			%client.chatMessage(%matter.name @ " has been added to the filter list.");
		case "remove" or "rem" or "r":
			if (!hasField(%hit.machineFilter, %matter.name))
			{
				%client.chatMessage(%matter.name @ " is not on the filter list.");
				return;
			}
			%hit.machineFilter = removeField(%hit.machineFilter, getFieldIndex(%hit.machineFilter, %matter.name));
			%client.chatMessage(%matter.name @ " has been removed from the filter list.");
	}
}