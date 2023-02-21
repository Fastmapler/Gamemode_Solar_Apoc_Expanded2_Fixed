datablock fxDTSBrickData(brickEOTWManualCrankData)
{
	brickFile = "./Shapes/HandCrank.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Manual Crank";
	//iconName = "";

    isPowered = true;
	powerType = "Source";
	inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWManualCrankData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 128 TAB "Copper" TAB 128 TAB "Lead";
$EOTW::BrickDescription["brickEOTWManualCrankData"] = "A basic device that allows power generation while activated, at the cost of your personal time.";

function brickEOTWManualCrankData::onTick(%this, %obj) {
    //Nothing!
}

function brickEOTWManualCrankData::onInspect(%this, %obj, %client) {
    %obj.changeBrickPower($EOTW::PowerLevel[0]);
}

datablock fxDTSBrickData(brickEOTWFueledBoilerData)
{
	brickFile = "./Shapes/Boiler.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Fueled Boiler";
	//iconName = "";

    isPowered = true;
	powerType = "Source";
	isProcessingMachine = true;

	hasInventory = true;
    matterSize = 256;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWFueledBoilerData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 128 TAB "Silver" TAB 128 TAB "Gold";
$EOTW::BrickDescription["brickEOTWFueledBoilerData"] = "Allows the controled boiling of water into steam. Requires burnable fuel (i.e. coal) and water.";

$EOTW::FueledBoilerThreshold = 256;
function brickEOTWFueledBoilerData::onTick(%this, %obj) {
	if (%obj.machineHeat < $EOTW::FueledBoilerThreshold)
	{
		for (%i = 0; %i < %this.matterSlots["Input"]; %i++)
		{
			%matter = getMatterType(getField(%obj.matter["Input", %i], 0));
			if (%matter.fuelPower > 0)
			{
				%amount = mCeil(($EOTW::FueledBoilerThreshold - %obj.machineHeat) / %matter.fuelPower);
				%burned = %obj.ChangeMatter(%matter.name, %amount * -1, "Input");
				%obj.machineHeat -= %burned;
			}
		}
	}

	if (%obj.machineHeat > 0)
	{
		%convertCount = getMin(%obj.GetMatter("Water", "Input"), getMin(%obj.machineHeat, $EOTW::PowerLevel[0]));
		%obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
		%obj.ChangeMatter("Water", %convertCount * -1, "Input");
		%obj.ChangeMatter("Steam", %convertCount, "Output");
	}
    
}

function brickEOTWSolarBoilerData::getProcessingText(%this, %obj) {
    return "Fuel Level: " @ mRound(0 + %obj.machineHeat);
}

datablock fxDTSBrickData(brickEOTWSolarBoilerData)
{
	brickFile = "./Shapes/SolarPanel.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Solar Boiler";
	//iconName = "";

    isPowered = true;
	powerType = "Source";
	isProcessingMachine = true;

	hasInventory = true;
    matterSize = 256;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWSolarBoilerData"] = 0.75 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Quartz" TAB 128 TAB "Teflon";
$EOTW::BrickDescription["brickEOTWSolarBoilerData"] = "Allows the controled boiling of water into steam. Works during the day. Degrades overtime and must be replaced.";

function brickEOTWSolarBoilerData::onTick(%this, %obj) {

	if ($EOTW::Time > 12)
		return;

	%waterCount = %obj.GetMatter("Water", "Input");
    if (%waterCount > 0)
	{
		%obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
		%amount = getMin(%waterCount, ($EOTW::PowerLevel[0] >> 4) * (1 - %obj.machineHeat));
		if (%amount - mFloor(%amount) > getRandom())
			%amount++;
		%obj.machineHeat = getMin(0.99, getMax(0.01, %obj.machineHeat * 1.00001));
		%obj.ChangeMatter("Water", %amount * -1, "Input");
		%obj.ChangeMatter("Steam", %amount, "Output");
	}
}

function brickEOTWSolarBoilerData::getProcessingText(%this, %obj) {
    return "Efficiency: " @ mRound(100 * (1 - %obj.machineHeat)) @ "\%";
}

datablock fxDTSBrickData(brickEOTWSteamTurbineData)
{
	brickFile = "./Shapes/SteamTurbine.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Steam Turbine";
	//iconName = "";

    isPowered = true;
	powerType = "Source";

	hasInventory = true;
    matterSize = 256;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWSteamTurbineData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWSteamTurbineData"] = "Generates power when inputted with steam.";

function brickEOTWSteamTurbineData::onTick(%this, %obj) {
	%steamCount = %obj.GetMatter("Steam", "Input");
    if (%steamCount > 0)
	{
		%obj.changeBrickPower(%steamCount);
		%obj.ChangeMatter("Steam", %steamCount * -1, "Input");
		%obj.ChangeMatter("Water", %steamCount / 2, "Output");
	}
}
