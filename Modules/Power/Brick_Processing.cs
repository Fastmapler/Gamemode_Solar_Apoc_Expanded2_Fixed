datablock fxDTSBrickData(brickEOTWAlloyForgeData)
{
	brickFile = "./Shapes/AlloyForge.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Alloy Forge";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;
    inspectMode = 1;

	processingType = "Alloying";
};
$EOTW::CustomBrickCost["brickEOTWAlloyForgeData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 256 TAB "Quartz" TAB 128 TAB "Copper";
$EOTW::BrickDescription["brickEOTWAlloyForgeData"] = "Uses different metals and materials to create alloys.";

function brickEOTWAlloyForgeData::onTick(%this, %obj) { %obj.runProcessingTick(); }

datablock fxDTSBrickData(brickEOTWFurnaceData)
{
	brickFile = "./Shapes/Refinery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Furnace";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
    inspectMode = 1;

	processingType = "Heating";
};
$EOTW::CustomBrickCost["brickEOTWFurnaceData"] = 1.00 TAB "7a7a7aff" TAB 384 TAB "Steel" TAB 256 TAB "Lead" TAB 128 TAB "Red Gold";
$EOTW::BrickDescription["brickEOTWFurnaceData"] = "Cooks materials in a controlled environment into something else.";

function brickEOTWFurnaceData::onTick(%this, %obj) { %obj.runProcessingTick(); }

datablock fxDTSBrickData(brickEOTWMatterReactorData)
{
	brickFile = "./Shapes/MatterReactor.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Matter Reactor";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 3;
	matterSlots["Output"] = 2;
    inspectMode = 1;

	processingType = "Chemistry";
};
$EOTW::CustomBrickCost["brickEOTWMatterReactorData"] = 1.00 TAB "7a7a7aff" TAB 384 TAB "Steel" TAB 256 TAB "Lead" TAB 128 TAB "Red Gold";
$EOTW::BrickDescription["brickEOTWMatterReactorData"] = "Takes in various materials to produce chemicals.";

function brickEOTWMatterReactorData::onTick(%this, %obj) { %obj.runProcessingTick(); }

datablock fxDTSBrickData(brickEOTWSeperatorData)
{
	brickFile = "./Shapes/Seperator.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Seperator";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 3;
    inspectMode = 1;

	processingType = "Seperation";
};
$EOTW::CustomBrickCost["brickEOTWSeperatorData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Plastic" TAB 256 TAB "Electrum" TAB 256 TAB "Red Gold";
$EOTW::BrickDescription["brickEOTWSeperatorData"] = "Seperates specific materials into useful components.";

function brickEOTWSeperatorData::onTick(%this, %obj) { %obj.runProcessingTick(); }

datablock fxDTSBrickData(brickEOTWBreweryData)
{
	brickFile = "./Shapes/Brewery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Brewery";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 4;
	matterSlots["Output"] = 1;
    inspectMode = 1;

	processingType = "Brewing";
};
$EOTW::CustomBrickCost["brickEOTWBreweryData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 128 TAB "Red Gold" TAB 128 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWBreweryData"] = "Brews potion fluid from the combination of various materials.";

function brickEOTWBreweryData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function fxDtsData::runProcessingTick(%obj)
{
	if (isObject(%obj.processingRecipe))
	{
		%recipe = %obj.processingRecipe;

		for (%i = 0; (%cost = %recipe.input{%i}) !$= ""; %i++)
		{
			if (%obj.getMatter(getField(%cost, 0)) < getField(%cost, 1))
			{
				//Couldn't find all needed materials, reset the recipe progress for now.
				%obj.recipeProgress = 0;
				return;
			}
		}

		if (%obj.attemptPowerDraw(%recipe.powerDrain))
			%obj.recipeProgress += %recipe.powerDrain;

		if (%obj.recipeProgress >= %recipe.powerCost)
		{
			%obj.processingRecipe = "";
			%obj.recipeProgress = 0;
			
			for (%i = 0; %recipe.input[%i] !$= ""; %i++)
				%obj.changeMatter(getField(%recipe.input[%i], 0), getField(%recipe.input[%i], 1) * -1, "Input");
			
			for (%i = 0; %recipe.output[%i] !$= ""; %i++)
				%obj.changeMatter(getField(%recipe.output[%i], 0), getField(%recipe.output[%i], 1), "Output");
		}
	}
}
