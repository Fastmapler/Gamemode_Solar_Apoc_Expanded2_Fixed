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
$EOTW::CustomBrickCost["brickEOTWPowerSourceTestData"] = 1.00 TAB "7a7a7aff" TAB 1 TAB "dog";
$EOTW::BrickDescription["brickEOTWPowerSourceTestData"] = "A device from the bygone era of SAEX1. Produces power passively for no cost.";

function brickEOTWPowerSourceTestData::onTick(%this, %obj) {
    %obj.changeBrickPower($EOTW::PowerLevel[0] >> 2);
}