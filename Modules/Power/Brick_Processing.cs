datablock fxDTSBrickData(brickEOTWAlloyForgeData)
{
	brickFile = "./Shapes/AlloyForge.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Alloy Forge";
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
};
$EOTW::CustomBrickCost["brickEOTWSeperatorData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Plastic" TAB 256 TAB "Electrum" TAB 256 TAB "Red Gold";
$EOTW::BrickDescription["brickEOTWSeperatorData"] = "Electrically seperates specific materials into core elements.";

function brickEOTWSeperatorData::onTick(%this, %obj) { %obj.runProcessingTick(); }

datablock fxDTSBrickData(brickEOTWBreweryData)
{
	brickFile = "./Shapes/Brewery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Brewery";
};
$EOTW::CustomBrickCost["brickEOTWBreweryData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 128 TAB "Red Gold" TAB 128 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWBreweryData"] = "Brews potion fluid from the combination of various materials.";

function brickEOTWBreweryData::onTick(%this, %obj) { %obj.runProcessingTick(); }