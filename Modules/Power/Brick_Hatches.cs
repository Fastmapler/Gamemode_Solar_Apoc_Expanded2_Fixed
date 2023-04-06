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
    if (isObject(%player = %client.player) && %obj.rayCastPlayer() == %player && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 1))
	{
		%player.ChangeBatteryEnergy($EOTW::PowerLevel[0] >> 1);
		%client.centerPrint(%player.GetBatteryText(), 1);
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

    matterSize = 128;
	matterSlots["Buffer"] = 1;
    inspectMode = 1;
	hasFiltering = true;
};
$EOTW::CustomBrickCost["brickEOTWInputHatchData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Silver" TAB 256 TAB "Red Gold" TAB 256 TAB "Lead";
$EOTW::BrickDescription["brickEOTWInputHatchData"] = "Deposits materials on a player into its buffer. Must be filtered (/setfilter) before use.";

function brickEOTWInputHatchData::onTick(%this, %obj) {
	%client = %obj.getGroup().client;
    if (isObject(%player = %client.player) && %obj.rayCastPlayer() == %player && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{
		//Take materials, based on filter
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
	matterSlots["Buffer"] = 1;
    inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWOutputHatchData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Iron" TAB 256 TAB "Steel" TAB 256 TAB "Lead";
$EOTW::BrickDescription["brickEOTWOutputHatchData"] = "Withdraws materials from itself into a player.";

function brickEOTWOutputHatchData::onTick(%this, %obj) {
    %client = %obj.getGroup().client;
    if (isObject(%player = %client.player) && %obj.rayCastPlayer() == %player && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{
		//Give materials
	}
}