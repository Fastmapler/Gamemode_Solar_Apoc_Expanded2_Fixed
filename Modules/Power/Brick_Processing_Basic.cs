datablock AudioProfile(OreFurnaceSound)
{
   filename    = "./Sounds/Furnace.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWBrickedRefineryData)
{
	brickFile = "./Shapes/Crucible.blb";
	category = "Solar Apoc";
	subCategory = "Primitive";
	uiName = "Bricked Refinery";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Crucible";

	isPowered = true;
	powerType = "Machine";
	useHeatForPower = true;

	matterSize = 512;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	automaticRecipe = true;
	processingType = "Refining";
	processSound = OreFurnaceSound;
};
$EOTW::CustomBrickCost["brickEOTWBrickedRefineryData"] = 1.00 TAB "c1a872ff" TAB 512 TAB "Granite" TAB 256 TAB "Quartz" TAB 256 TAB "Wood";
$EOTW::BrickDescription["brickEOTWBrickedRefineryData"] = "A very simple ore refinery that uses raw fuel instead of electric power.";

function brickEOTWBrickedRefineryData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWBrickedRefineryData::getProcessingText(%this, %obj) {
	%heatText = %obj.machineHeat > 0 ? "\c2Heated (" @ %obj.machineHeat @ " HU)" : "\c7Not Fueled";

    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe) SPC "\c6|" SPC %heatText;
	else
		return "\c0No Recipe (Automatic) \c6|" SPC %heatText;
}

datablock AudioProfile(CokeOvenSound)
{
   filename    = "./Sounds/CokeOven.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWBrickedCokeOvenData)
{
	brickFile = "./Shapes/CokeOven.blb";
	category = "Solar Apoc";
	subCategory = "Primitive";
	uiName = "Bricked Coke Oven";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/CokeOven";

	isPowered = true;
	powerType = "Machine";
	passivePower = true;
	powerEfficiency = 0.5;

	matterSize = 512;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	automaticRecipe = true;
	processingType = "Pyrolysis";
	processSound = CokeOvenSound;
};
$EOTW::CustomBrickCost["brickEOTWBrickedCokeOvenData"] = 1.00 TAB "c1a872ff" TAB 512 TAB "Granite" TAB 256 TAB "Quartz" TAB 128 TAB "Iron";
$EOTW::BrickDescription["brickEOTWBrickedCokeOvenData"] = "Turn wood and coal into more useful charcoal and coke.";

function brickEOTWBrickedCokeOvenData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWBrickedCokeOvenData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (Automatic)";
}

datablock AudioProfile(BlastFurnaceSound)
{
   filename    = "./Sounds/BlastFurnace.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWBrickedBlastFurnaceData)
{
	brickFile = "./Shapes/BlastFurnace.blb";
	category = "Solar Apoc";
	subCategory = "Primitive";
	uiName = "Bricked Blast Furnace";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/BlastFurnace";

	isPowered = true;
	powerType = "Machine";
	passivePower = true;
	powerEfficiency = 0.5;

	matterSize = 512;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	automaticRecipe = true;
	processingType = "Blasting";
	processSound = BlastFurnaceSound;
};
$EOTW::CustomBrickCost["brickEOTWBrickedBlastFurnaceData"] = 1.00 TAB "c1a872ff" TAB 1024 TAB "Granite" TAB 512 TAB "Quartz" TAB 128 TAB "Coke";
$EOTW::BrickDescription["brickEOTWBrickedBlastFurnaceData"] = "Turn iron into steel with extra coke. No electricity, just time! Lots of time.";

function brickEOTWBrickedBlastFurnaceData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWBrickedBlastFurnaceData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (Automatic)";
}