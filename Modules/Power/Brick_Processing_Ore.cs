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

datablock AudioProfile(SieveSound)
{
   filename    = "./Sounds/Sieve.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWSifterData)
{
	brickFile = "./Shapes/ineedamodel.blb";
	category = "Solar Apoc";
	subCategory = "Ore Processing";
	uiName = "Siever";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/ineedamodel";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 6;
	processSound = SieveSound;
};
$EOTW::CustomBrickCost["brickEOTWSifterData"] = 1.00 TAB "7a7a7aff" TAB 1024 TAB "Coal" TAB 512 TAB "Plastic" TAB 256 TAB "Steel";
$EOTW::BrickDescription["brickEOTWSifterData"] = "Filter specific matter for various stuff! Will not process if all output slots have content. Will void material overflow.";

$EOTW::SieveOutput["Sludge"] = "Magnetite\t60" TAB "Malachite\t15" TAB "Acanthite\t15" TAB "Anglesite\t10" TAB "Native Gold\t10" TAB "Sturdite\t1";
$EOTW::SieveOutput["Water"] = "\t120" TAB "Granite\t7" TAB "Salt\t1";
$EOTW::SieveOutput["Uranic Dust"] = "Uranium-238\t127" TAB "Uranium-235\t1";
$EOTW::SieveOutput["Fluoric Dust"] = "Calcium\t2" TAB "Fluorine\t1";
function brickEOTWSifterData::onTick(%this, %obj) {

	%sieveMatter = getField(%obj.matter["Input", 0], 0);

	if ($EOTW::SieveOutput[%sieveMatter] !$= "" && %obj.GetMatter(%sieveMatter, "Input") > 0 && %obj.getEmptySlotCount("Output") > 0 && %obj.attemptPowerDraw($EOTW::PowerLevel[0]))
	{
		if ($EOTW::SieveOutputWeight[%sieveMatter] $= "")
		{
			//Calculate max weight for rng stuff
			%weight = 0;
			for (%i = 1; %i < getFieldCount($EOTW::SieveOutput[%sieveMatter]); %i += 2)
				%weight += getField($EOTW::SieveOutput[%sieveMatter], %i);
			$EOTW::SieveOutputWeight[%sieveMatter] = %weight;
		}

		%roll = getRandom() * $EOTW::SieveOutputWeight[%sieveMatter];

		for (%i = 0; %i < getFieldCount($EOTW::SieveOutput[%sieveMatter]); %i += 2)
		{
			%outputRoll = getField($EOTW::SieveOutput[%sieveMatter], %i);

			if (%roll < getField($EOTW::SieveOutput[%sieveMatter], %i + 1))
				break;

			%roll -= getField($EOTW::SieveOutput[%sieveMatter], %i + 1);
		}

		%obj.changeMatter(%sieveMatter, -1, "Input");

		if (isObject(%matterOutput = getMatterType(%outputRoll)))
			%obj.changeMatter(%outputRoll, 1, "Output");
	}
}