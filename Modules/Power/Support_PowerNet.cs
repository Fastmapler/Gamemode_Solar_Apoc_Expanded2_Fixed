//TODO: Move bricks to their own unique files.

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

//Base Code

function getPowerSet(%type, %bl_id)
{
	%data = (%type @ "Group_" @ %bl_id);

	if (!isObject(%data))
		%data = new SimSet(%data);

	return %data.getID();
}

function fxDTSBrick::changeBrickEnergy(%obj, %amount)
{
	%data = %obj.getDatablock();

	if (!%data.isPowered)
		return;
}

function fxDtsBrick::onTick(%obj)
{
	%obj.getDatablock().onTick(%obj);
}

function fxDtsBrick::LoadPowerData(%obj)
{
	%data = %obj.getDatablock();
	%bl_id = %obj.getGroup().bl_id;

	if (!%data.isPowered || %bl_id < 1)
		return;

	%set = getPowerSet(%data.powerType, %bl_id);
	talk(%set);
}

package EOTW_Power {
	function fxDtsBrick::onPlant(%obj, %b)
	{
		parent::onPlant(%obj, %b);
		
		%obj.LoadPowerData();
	}

	function fxDtsBrick::onLoadPlant(%obj, %b)
	{
		parent::onLoadPlant(%obj, %b);
		
		%obj.LoadPowerData(%obj);
	}

	function fxDTSBrick::onAdd(%obj)
	{
		Parent::onAdd(%obj);
	}
};
activatePackage("EOTW_Power");