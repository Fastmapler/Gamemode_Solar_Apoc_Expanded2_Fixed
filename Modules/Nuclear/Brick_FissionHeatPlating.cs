$EOTW::CustomBrickCost["brickMFRHeatPlatingData"] = 1.00 TAB "2f2d2fff" TAB 256 TAB "Copper" TAB 80 TAB "Sturdium";
$EOTW::BrickDescription["brickMFRHeatPlatingData"] = "Increases the reactor's max heat by 5000 HU.";
datablock fxDTSBrickData(brickMFRHeatPlatingData)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "MFR Heat Plating";
	notRecolorable = true;

	reqFissionPart = brickMFRHullData;
	maxFissionHeatBonus = 5000;
	blacklistFromAdjacentScan = true;
};