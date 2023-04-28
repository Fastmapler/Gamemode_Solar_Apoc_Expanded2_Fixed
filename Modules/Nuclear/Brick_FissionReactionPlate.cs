$EOTW::CustomBrickCost["brickMFRReactionPlateData"] = 1.00 TAB "7a7a7aff" TAB 192 TAB "Steel" TAB 16 TAB "Diamond";
$EOTW::BrickDescription["brickMFRReactionPlateData"] = "Essential part which allows components to be placed.";
datablock fxDTSBrickData(brickMFRReactionPlateData)
{
	brickFile = "./Bricks/MFRReactionPlate.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "MFR Reaction Plate";

	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};