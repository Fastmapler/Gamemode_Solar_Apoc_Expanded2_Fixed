datablock AudioProfile(ManualCrankLoopSound)
{
   filename    = "./Sounds/Crank.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWManualCrankData)
{
	brickFile = "./Shapes/HandCrank.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Manual Crank";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ManualCrank";

    isPowered = true;
	powerType = "Source";
	inspectMode = 1;

	isProcessingMachine = true;
	processSound = ManualCrankLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWManualCrankData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Electrum" TAB 128 TAB "Copper" TAB 128 TAB "Lead";
$EOTW::BrickDescription["brickEOTWManualCrankData"] = "A basic device that allows power generation while activated, at the cost of your personal time. Multiple users can use the same crank.";

function brickEOTWManualCrankData::onTick(%this, %obj) {
    //Nothing!
}

function brickEOTWManualCrankData::onInspect(%this, %obj, %client) {
	if (getSimTime() - %obj.lastCrankTime[%client] >= $EOTW::PowerTickRate)
	{
		%obj.lastCrankTime[%client] = getSimTime();
		%obj.changeBrickPower(38);
	}
}

function brickEOTWManualCrankData::getProcessingText(%this, %obj) {
    return "Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower();
}

datablock AudioProfile(FueledBoilerLoopSound)
{
   filename    = "./Sounds/Boiler.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWFueledBoilerData)
{
	brickFile = "./Shapes/Boiler.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Fueled Boiler";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/FueledBoiler";

    isPowered = true;
	powerType = "Source";
	isProcessingMachine = true;

	hasInventory = true;
    matterSize = 512;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	processSound = FueledBoilerLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWFueledBoilerData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Silver" TAB 256 TAB "Gold";
$EOTW::BrickDescription["brickEOTWFueledBoilerData"] = "Allows the controled boiling of water into steam. Requires burnable fuel (i.e. coal) and water.";

$EOTW::RawFuelThreshold = 256;

function fxDtsBrick::addRawFuel(%obj) {
	%this = %obj.getDatablock();
	if (%obj.machineHeat < $EOTW::RawFuelThreshold)
	{
		for (%i = 0; %i < %this.matterSlots["Input"]; %i++)
		{
			%matter = getMatterType(getField(%obj.matter["Input", %i], 0));
			if (%matter.fuelPower > 0 && (!%this.requireCombustionFuel || %matter.combustable))
			{
				%amount = $EOTW::RawFuelThreshold - %obj.machineHeat;
				%burned = %obj.ChangeMatter(%matter.name, %amount * -1, "Input");
				
				%obj.machineHeat += mAbs(%burned * %matter.fuelPower * $EOTW::GlobalPowerCostMultiplier);

				%obj.machineBonus = getMax(1.0, %matter.fuelMultiplier);
			}
		}
	}
}

function brickEOTWFueledBoilerData::onTick(%this, %obj) {
	
	%obj.addRawFuel();

	if (%obj.machineHeat > 0)
	{
		%obj.machineBonus = getMax(1.0, %obj.machineBonus);
		%convertCount = getMin(%obj.GetMatter("Water", "Input"), getMin(%obj.machineHeat, $EOTW::PowerLevel[0] * %obj.machineBonus));
		%convertCount = getMin(%convertCount, %this.matterSize - %obj.GetMatter("Steam", "Output"));
		if (%convertCount > 0)
		{
			%obj.lastDrawTime = getSimTime();
			%obj.lastDrawSuccess = getSimTime();
			%obj.ChangeMatter("Water", %convertCount * -1, "Input");
			%obj.ChangeMatter("Steam", %convertCount, "Output");
			%obj.machineHeat -= %convertCount;

			if (isObject(%this.processSound))
			{
				if (!isObject(%obj.audioEmitter))
					%obj.playSoundLooping(%this.processSound);

				cancel(%obj.EndSoundsLoopSchedule);
				%obj.EndSoundsLoopSchedule = %obj.schedule($EOTW::PowerTickRate * 1.1, "playSoundLooping");
			}
		}
		
	}
    
}

function brickEOTWFueledBoilerData::getProcessingText(%this, %obj) {
    return %obj.machineHeat > 0 ? "\c2Machine Heated (Speed: " @ %obj.machineBonus @ "x)" : "\c7Not Fueled";
}

datablock fxDTSBrickData(brickEOTWSolarBoilerData)
{
	brickFile = "./Shapes/SolarPanel.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Solar Boiler";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/SolarPanel";

    isPowered = true;
	powerType = "Source";
	isProcessingMachine = true;

	hasInventory = true;
    matterSize = 512;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWSolarBoilerData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Quartz" TAB 256 TAB "Teflon";
$EOTW::BrickDescription["brickEOTWSolarBoilerData"] = "Allows the controled boiling of water into steam. Works during the day. Degrades overtime and must be replaced.";

function brickEOTWSolarBoilerData::onTick(%this, %obj) {

	if ($EOTW::Time > 12)
		return;

	%waterCount = %obj.GetMatter("Water", "Input");
    if (%waterCount > 0)
	{
		%obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
		%amount = getMin(%waterCount, ($EOTW::PowerLevel[0] >> 2) * (1 - %obj.machineHeat));
		%amount = getMin(%amount, %this.matterSize - %obj.GetMatter("Steam", "Output"));
		if (%amount - mFloor(%amount) > getRandom())
			%amount++;
		if (getRandom() < $EOTW::PowerTickRate / 1000)
			%obj.machineHeat = getMin(0.99, getMax(0.01, %obj.machineHeat + 0.00001));
		%obj.ChangeMatter("Water", %amount * -1, "Input");
		%obj.ChangeMatter("Steam", %amount, "Output");
	}
}

function brickEOTWSolarBoilerData::getProcessingText(%this, %obj) {
    return "Efficiency: " @ mRound(100 * (1 - %obj.machineHeat)) @ "\%";
}

datablock AudioProfile(SteamTurbineLoopSound)
{
   filename    = "./Sounds/Turbine.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWSteamTurbineData)
{
	brickFile = "./Shapes/SteamTurbine.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Steam Turbine";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/SteamTurbine";

    isPowered = true;
	powerType = "Source";
	isProcessingMachine = true;

	hasInventory = true;
    matterSize = 512;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	maxBuffer = 4096;

	processSound = SteamTurbineLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWSteamTurbineData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Steel" TAB 256 TAB "Copper" TAB 128 TAB "Lead";
$EOTW::BrickDescription["brickEOTWSteamTurbineData"] = "Generates power when inputted with steam. Continuous use will give a power bonus.";

function brickEOTWSteamTurbineData::getProcessingText(%this, %obj) {
    return "Bonus: " @ (1 + %obj.machineHeat) @ "x | Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower();
}

function brickEOTWSteamTurbineData::onTick(%this, %obj) {
	%matter = getMatterType(getField(%obj.matter["Input", 0], 0));
	%matterCount = getField(%obj.matter["Input", 0], 1);
	%bonusChange = 0.01;
    if (%matter.turbinePower > 0 && %matterCount > 0)
	{
		%obj.machineHeat = getMin(%obj.machineHeat + %bonusChange, 3);
		%obj.changeBrickPower(%matter.turbinePower * %matterCount * (1 + %obj.machineHeat));
		%obj.ChangeMatter(%matter.name, %matterCount * -1, "Input");
		%obj.ChangeMatter(%matter.coolMatter, mFloor(%matterCount / 1.1), "Output");
	}
	else
	{
		%obj.machineHeat = getMax(%obj.machineHeat - %bonusChange, 0);
	}
}

datablock AudioProfile(CombustionEngineLoopSound)
{
   filename    = "./Sounds/Combustion.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWCombustionEngineData)
{
	brickFile = "./Shapes/Boiler.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Combustion Engine";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/CombustionEngine";

    isPowered = true;
	powerType = "Source";
	isProcessingMachine = true;
	requireCombustionFuel = true;

	hasInventory = true;
    matterSize = 512;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	processSound = CombustionEngineLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWCombustionEngineData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Silver" TAB 256 TAB "Gold";
$EOTW::BrickDescription["brickEOTWCombustionEngineData"] = "Directly burns petroleum based fuels for compact power production. Also needs oxygen and lubricant.";

function brickEOTWCombustionEngineData::onTick(%this, %obj) {

	%obj.addRawFuel();

	if (%obj.machineHeat > 0)
	{

	}
    
}

function brickEOTWCombustionEngineData::getProcessingText(%this, %obj) {
    return %obj.machineHeat > 0 ? "\c2Machine Heated (Speed: " @ %obj.machineBonus @ "x)" : "\c7Not Fueled";
}

datablock AudioProfile(PlutoniumRTGLoopSound)
{
   filename    = "./Sounds/RTG.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWPlutoniumRTGData)
{
	brickFile = "./Shapes/RTG.blb";
	category = "Solar Apoc";
	subCategory = "Power Gen";
	uiName = "Plutonium RTG";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ineedanimage";

    isPowered = true;
	powerType = "Source";
	inspectMode = 1;

	hasInventory = true;
    matterSize = 256;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processSound = PlutoniumRTGLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWPlutoniumRTGData"] = 0.00 TAB "7a7a7aff" TAB 128 TAB "Plutonium" TAB 512 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPlutoniumRTGData"] = "An **UNSALVAGABLE** device which slowly decays into Rare Earths, and produces a small amount of power.";

function brickEOTWPlutoniumRTGData::onTick(%this, %obj) {
	%obj.lastDrawTime = getSimTime();
	%obj.lastDrawSuccess = getSimTime();

	if (getRandom() < (1 / $EOTW::GlobalPowerCostMultiplier))
		%obj.machineHeat++;

	%percentLeft = mPow(0.5, %obj.machineHeat / 3600);
	%obj.recipeProgress += %percentLeft;

	//TODO: Get the right number for production rate
	if (%obj.recipeProgress >= 100 * $EOTW::GlobalPowerCostMultiplier)
	{
		%obj.recipeProgress -= 100;
		%obj.ChangeMatter("Rare Earths", 1, "Output");
	}

	%amount = ($EOTW::PowerLevel[0] >> 2) * %percentLeft;
	if (%amount - mFloor(%amount) > getRandom())
		%amount++;

	%obj.changeBrickPower(%amount);
	%obj.machineHeat = getMin(%obj.machineHeat, 999999);
}

function brickEOTWPlutoniumRTGData::getProcessingText(%this, %obj) {
	%percentLeft = mPow(0.5, %obj.machineHeat / 3600);
    return "Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower() @ " | Efficiency: " @ mRound(100 * %percentLeft) @ "\%";
}