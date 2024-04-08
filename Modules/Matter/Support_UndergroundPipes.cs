datablock fxDTSBrickData(brickEOTWUGPipeInputData)
{
	brickFile = "./Shapes/MicroCapacitor.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "UG Pipe Input";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Matter/Icons/MicroCapacitor";

    hasInventory = true;
    matterSize = 32;
	matterSlots["Input"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWUGPipeInputData"] = 1.00 TAB "7a7a7aff" TAB 128 TAB "Sturdium" TAB 128 TAB "Piping";
$EOTW::BrickDescription["brickEOTWUGPipeInputData"] = "Pushes matter to output underground pipes. Define networks based on brick color.";