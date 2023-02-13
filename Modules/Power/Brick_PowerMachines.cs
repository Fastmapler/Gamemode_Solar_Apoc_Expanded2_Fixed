datablock fxDTSBrickData(brickEOTWWaterPumpData)
{
	brickFile = "./Shapes/WaterPump.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Water Pump";
	//iconName = "";

    isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Output"] = 1;
    inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWWaterPumpData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWWaterPumpData"] = "A device that draws water deep within the ground. Must be connected to the terrain. Can be operated manually.";

function brickEOTWWaterPumpData::onTick(%this, %obj) {
    if (!%obj.checkTerrain)
    {
        %obj.checkTerrain = true;
        %obj.onTerrain = %obj.isOnPublicBrick();
    }
	if (%obj.onTerrain && getSimTime() - %obj.lastDrawSuccess >= 100 && %obj.GetMatter("Water", "Output") < 128 && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{
        %obj.ChangeMatter("Water", 1, "Output");
	}
}

function brickEOTWWaterPumpData::onInspect(%this, %obj, %client) {
    if (%obj.onTerrain && %obj.GetMatter("Water", "Output") < 128 && getSimTime() - %obj.lastDrawSuccess >= 100)
    {
        %obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
        %obj.ChangeMatter("Water", 4, "Output");
    }
}

datablock fxDTSBrickData(brickEOTWChargerHatchData)
{
	brickFile = "./Shapes/ChargePad.blb";
	category = "Solar Apoc";
	subCategory = "Hatches";
	uiName = "Charger Hatch";
	//iconName = "";

    isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWChargerHatchData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWChargerHatchData"] = "Charges a character's personal battery. This can be used for tools like the Mining Scanner or Oil Pump.";

function brickEOTWChargerHatchData::onTick(%this, %obj) {
    //Check if player is nearby via clientgroup

    //do box raycast to see if someone(s) is actually using it

    //give powah!
}

datablock fxDTSBrickData(brickEOTWInputHatchData)
{
	brickFile = "./Shapes/ChargePad.blb";
	category = "Solar Apoc";
	subCategory = "Hatches";
	uiName = "Input Hatch";
	//iconName = "";

    isPowered = true;
	powerType = "Machine";

    matterSize = 128;
	matterSlots["Buffer"] = 1;
    inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWInputHatchData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWInputHatchData"] = "Deposits materials on a player into its buffer. Must be filtered (/setfilter) before use.";

function brickEOTWInputHatchData::onTick(%this, %obj) {
    //Check if player is nearby via clientgroup

    //do box raycast to see if someone(s) is actually using it

    //attempt to move matter based on filter
}

datablock fxDTSBrickData(brickEOTWOutputHatchData)
{
	brickFile = "./Shapes/ChargePad.blb";
	category = "Solar Apoc";
	subCategory = "Hatches";
	uiName = "Output Hatch";
	//iconName = "";

    isPowered = true;
	powerType = "Machine";

    matterSize = 128;
	matterSlots["Buffer"] = 1;
    inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWOutputHatchData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWOutputHatchData"] = "Withdraws materials from itself into a player.";

function brickEOTWOutputHatchData::onTick(%this, %obj) {
    //Check if player is nearby via clientgroup

    //do box raycast to see if someone(s) is actually using it

    //attempt to move matter based on filter
}