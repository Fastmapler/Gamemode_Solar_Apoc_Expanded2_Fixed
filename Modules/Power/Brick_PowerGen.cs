datablock fxDTSBrickData(brickEOTWManualCrankData)
{
	brickFile = "./Shapes/Generic.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Manual Crank";
	//iconName = "";

    isPowered = true;
	powerType = "Source";
	inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWManualCrankData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWManualCrankData"] = "A basic device that allows power generation when activated, at the cost of your time.";

function brickEOTWManualCrankData::onTick(%this, %obj) {
    //Nothing!
}

function brickEOTWManualCrankData::onInspect(%this, %obj, %client) {
    %obj.changeBrickPower($EOTW::PowerLevel[0]);
}

datablock fxDTSBrickData(brickEOTWBoilerData)
{
	brickFile = "./Shapes/Generic.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Fuel Boiler";
	//iconName = "";

    isPowered = true;
	powerType = "Source";
	inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWBoilerData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWBoilerData"] = "Allows the controled boiling of water into steam. Requires burnable fuel (i.e. coal) and water.";

function brickEOTWBoilerData::onTick(%this, %obj) {
    //Boil water
}
