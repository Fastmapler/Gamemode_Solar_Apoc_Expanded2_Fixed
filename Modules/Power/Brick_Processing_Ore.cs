datablock fxDTSBrickData(brickEOTWOreRefineryData)
{
	brickFile = "./Shapes/OreRefinery.blb";
	category = "Solar Apoc";
	subCategory = "Ore Processing";
	uiName = "Ore Refinery";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ineedanimage";

	isPowered = true;
	powerType = "Machine";

	matterSize = 64;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	automaticRecipe = true;
	processingType = "Refining";
	processSound = OreFurnaceSound;
};
$EOTW::CustomBrickCost["brickEOTWOreRefineryData"] = 1.00 TAB "c1a872ff" TAB 512 TAB "Steel" TAB 512 TAB "Granite" TAB 256 TAB "Quartz";
$EOTW::BrickDescription["brickEOTWOreRefineryData"] = "Turn those processed/unprocessed ores into shiny metal!";

function brickEOTWOreRefineryData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWOreRefineryData::getProcessingText(%this, %obj) {
	%heatText = %obj.machineHeat > 0 ? "\c2Machine Heated" : "\c7Not Fueled";

    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe) SPC "\c6|" SPC %heatText;
	else
		return "\c0No Recipe (Automatic) \c6|" SPC %heatText;
}

datablock AudioProfile(CrusherLoopSound)
{
   filename    = "./Sounds/Crusher.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWCrusherData)
{
	brickFile = "./Shapes/Crusher.blb";
	category = "Solar Apoc";
	subCategory = "Ore Processing";
	uiName = "Crusher";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ineedanimage";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 2;

	isProcessingMachine = true;
	automaticRecipe = true;
	processingType = "Crushing";
	processSound = CrusherLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWCrusherData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "PlaSteel" TAB 256 TAB "Steel" TAB 128 TAB "Copper";
$EOTW::BrickDescription["brickEOTWCrusherData"] = "Crushes raw ores into a more usable state.";

function brickEOTWCrusherData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWCrusherData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (Automatic)";
}

datablock AudioProfile(WasherLoopSound)
{
   filename    = "./Sounds/Washer.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWWasherData)
{
	brickFile = "./Shapes/Orewasher.blb";
	category = "Solar Apoc";
	subCategory = "Ore Processing";
	uiName = "Washing Machine";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ineedanimage";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 2;

	isProcessingMachine = true;
	automaticRecipe = true;
	processingType = "Washing";
	processSound = WasherLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWWasherData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Adamantine" TAB 256 TAB "Electrum" TAB 128 TAB "Quartz";
$EOTW::BrickDescription["brickEOTWWasherData"] = "For washing ores, not clothing.";

function brickEOTWWasherData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWWasherData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(FrotherSound)
{
   filename    = "./Sounds/Froth.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWFrotherData)
{
	brickFile = "./Shapes/OreFrother.blb";
	category = "Solar Apoc";
	subCategory = "Ore Processing";
	uiName = "Ore Frother";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ineedanimage";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 2;

	isProcessingMachine = true;
	processingType = "Frothing";
	processSound = FrotherSound;
};
$EOTW::CustomBrickCost["brickEOTWFrotherData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Rare Earths" TAB 256 TAB "Quartz" TAB 128 TAB "Silver";
$EOTW::BrickDescription["brickEOTWFrotherData"] = "Specialized machine for getting the most out of your ores, using chemicals.";

function brickEOTWFrotherData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWFrotherData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}