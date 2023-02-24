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
$EOTW::BrickDescription["brickEOTWWaterPumpData"] = "A device that draws water deep within the ground. Can be operated manually.";

function brickEOTWWaterPumpData::onTick(%this, %obj) {
	if (getSimTime() - %obj.lastDrawSuccess >= 100 && %obj.GetMatter("Water", "Output") < 128 && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{
        %obj.ChangeMatter("Water", 4, "Output");
	}
}

function brickEOTWWaterPumpData::onInspect(%this, %obj, %client) {
    if (%obj.GetMatter("Water", "Output") < 128 && getSimTime() - %obj.lastDrawSuccess >= 100)
    {
        %obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
        %obj.ChangeMatter("Water", 4, "Output");
    }
}

datablock fxDTSBrickData(brickEOTWThumperData)
{
	brickFile = "./Bricks/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Mining Thumper";

	isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWThumperData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Lead" TAB 256 TAB "Steel" TAB 128 TAB "Teflon";
$EOTW::BrickDescription["brickEOTWThumperData"] = "When active gives a 100% speed boost to gathering nearby resources.";

function brickEOTWThumperData::onTick(%this, %obj)
{
	if (%obj.attemptPowerDraw($EOTW::PowerLevel[1] >> 1))
		%obj.lastThump = getSimTime();
}