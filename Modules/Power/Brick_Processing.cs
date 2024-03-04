
datablock fxDTSBrickData(brickEOTWElectricBlastFurnaceData)
{
	brickFile = "./Shapes/ElectricBlastFurnace.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Electric Blast Furnace";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ineedanimage";

	isPowered = true;
	powerType = "Machine";
	powerEfficiency = 2;

	matterSize = 128;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Blasting";
	processSound = BlastFurnaceSound;
};
$EOTW::CustomBrickCost["brickEOTWElectricBlastFurnaceData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "PlaSteel" TAB 512 TAB "Steel" TAB 512 TAB "Copper";
$EOTW::BrickDescription["brickEOTWElectricBlastFurnaceData"] = "A blast furnace, but uses electricity to speed stuff up!";

function brickEOTWElectricBlastFurnaceData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWElectricBlastFurnaceData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock fxDTSBrickData(brickEOTWPyrolysisOvenData)
{
	brickFile = "./Shapes/PyroOven.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Pyrolysis Oven";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ineedanimage";

	isPowered = true;
	powerType = "Machine";
	powerEfficiency = 2;

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Pyrolysis";
	processSound = CokeOvenSound;
};
$EOTW::CustomBrickCost["brickEOTWPyrolysisOvenData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "PlaSteel" TAB 512 TAB "Steel" TAB 1024 TAB "Granite";
$EOTW::BrickDescription["brickEOTWPyrolysisOvenData"] = "Get those coke oven recipes done MUCH faster!";

function brickEOTWPyrolysisOvenData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWPyrolysisOvenData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(AlloyForgeLoopSound)
{
   filename    = "./Sounds/AlloyForgeLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWAlloyForgeData)
{
	brickFile = "./Shapes/AlloyForge.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Alloy Forge";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/AlloyForge";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Alloying";
	processSound = AlloyForgeLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWAlloyForgeData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Quartz" TAB 128 TAB "Silver";
$EOTW::BrickDescription["brickEOTWAlloyForgeData"] = "Uses different metals and materials to create alloys.";

$EOTW::BrickUpgrade["brickEOTWAlloyForgeData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWAlloyForgeData", 0] = 256 TAB "Plastic" TAB 512 TAB "Granite" TAB 256 TAB "Quartz";
$EOTW::BrickUpgrade["brickEOTWAlloyForgeData", 1] = 512 TAB "PlaSteel" TAB 512 TAB "Energium" TAB 512 TAB "Naturum";

function brickEOTWAlloyForgeData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWAlloyForgeData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(ChemHeaterLoopSound)
{
   filename    = "./Sounds/RefineryLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWChemHeaterData)
{
	brickFile = "./Shapes/Refinery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Matter Heater";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Refinery";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	automaticRecipe = true;
	processingType = "Burning";
	processSound = FurnaceLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWChemHeaterData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Steel" TAB 256 TAB "Lead" TAB 256 TAB "Red Gold";
$EOTW::BrickDescription["brickEOTWChemHeaterData"] = "Cooks materials in a controlled environment into something else.";

$EOTW::BrickUpgrade["brickEOTWChemHeaterData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWChemHeaterData", 0] = 1024 TAB "Granite" TAB 512 TAB "Steel" TAB 128 TAB "Brimstone";
$EOTW::BrickUpgrade["brickEOTWChemHeaterData", 1] = 2048 TAB "Granite" TAB 512 TAB "PlaSteel" TAB 128 TAB "Naturum";

function brickEOTWChemHeaterData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWChemHeaterData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (Automatic)";
}

datablock AudioProfile(MatterReactorSound)
{
   filename    = "./Sounds/MatterReactorLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWMatterReactorData)
{
	brickFile = "./Shapes/MatterReactor.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Matter Reactor";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/MatterReactor";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 3;
	matterSlots["Output"] = 2;
	isProcessingMachine = true;
	processingType = "Chemistry";
	processSound = MatterReactorSound;
};
$EOTW::CustomBrickCost["brickEOTWMatterReactorData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Steel" TAB 256 TAB "Lead" TAB 256 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWMatterReactorData"] = "Takes in various materials to produce chemicals.";

$EOTW::BrickUpgrade["brickEOTWMatterReactorData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWMatterReactorData", 0] = 512 TAB "Silver" TAB 512 TAB "Plastic" TAB 128 TAB "Ethylene";
$EOTW::BrickUpgrade["brickEOTWMatterReactorData", 1] = 1024 TAB "Silver" TAB 512 TAB "Teflon" TAB 128 TAB "Epoxy";

function brickEOTWMatterReactorData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWMatterReactorData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(SeperatorSound)
{
   filename    = "./Sounds/SeperatorLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWSeperatorData)
{
	brickFile = "./Shapes/Seperator.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Seperator";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Seperator";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 3;

	isProcessingMachine = true;
	processingType = "Seperation";
	processSound = SeperatorSound;
};
$EOTW::CustomBrickCost["brickEOTWSeperatorData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Plastic" TAB 256 TAB "Electrum" TAB 256 TAB "Red Gold";
$EOTW::BrickDescription["brickEOTWSeperatorData"] = "Seperates specific materials into useful components.";

$EOTW::BrickUpgrade["brickEOTWSeperatorData", "MaxTier"] = 1;
$EOTW::BrickUpgrade["brickEOTWSeperatorData", 0] = 512 TAB "Lead" TAB 256 TAB "Rubber" TAB 256 TAB "Sturdium";

function brickEOTWSeperatorData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWSeperatorData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(BrewerySound)
{
   filename    = "./Sounds/BreweryLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWBreweryData)
{
	brickFile = "./Shapes/Brewery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Brewery";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Brewery";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 4;
	matterSlots["Output"] = 2;

	isProcessingMachine = true;
	processingType = "Brewing";
	processSound = BrewerySound;
};
$EOTW::CustomBrickCost["brickEOTWBreweryData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 128 TAB "Red Gold" TAB 128 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWBreweryData"] = "Brews potion fluid from the combination of various materials.";

$EOTW::BrickUpgrade["brickEOTWBreweryData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWBreweryData", 0] = 512 TAB "Biomass" TAB 512 TAB "Flesh" TAB 128 TAB "Ethylene";
$EOTW::BrickUpgrade["brickEOTWBreweryData", 1] = 2048 TAB "Lead" TAB 512 TAB "Boss Essence" TAB 128 TAB "Epoxy";

function brickEOTWBreweryData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWBreweryData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}