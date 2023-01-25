datablock fxDTSBrickData(brickEOTWPowerSourceTestData)
{
	brickFile = "./Shapes/Generic.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Generator";
	//iconName = "";

    isPowered = true;
	powerType = "Source";
};
$EOTW::CustomBrickCost["brickEOTWPowerSourceTestData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerSourceTestData"] = "A device from the bygone era of SAEX1. Produces energy passively for no cost.";

function brickEOTWPowerSourceTestData::onTick(%this, %obj) {
    %obj.changeBrickEnergy(8);
}

datablock fxDTSBrickData(brickEOTWPowerUnitTestData)
{
	brickFile = "./Shapes/Generic.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Power Unit";
	//iconName = "";

    isPowered = true;
	powerType = "Battery";
};
$EOTW::CustomBrickCost["brickEOTWPowerUnitTestData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerUnitTestData"] = "Takes in power from nearby power sources, and allows machines to use it.";

datablock fxDTSBrickData(brickEOTWMachineTestData)
{
	brickFile = "./Shapes/Generic.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Machine";
	//iconName = "";

    isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWMachineTestData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMachineTestData"] = "A device that takes in power and spams the chat. Why?";

function brickEOTWMachineTestData::onTick(%this, %obj) {

}

function brickEOTWMachineTestData::onTaskProcessed(%this, %obj) {
    if (getRandom() < 0.001)
        talk("https://www.youtube.com/watch?v=sZW5jySoFTM");
    else
        talk("trol @ " @ %obj.getID());
}