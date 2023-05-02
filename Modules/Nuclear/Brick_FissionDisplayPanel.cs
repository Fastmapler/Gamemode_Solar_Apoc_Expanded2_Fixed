$EOTW::CustomBrickCost["brickMFRDisplayPanelData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Steel" TAB 256 TAB "Copper" TAB 128 TAB "Silver";
$EOTW::BrickDescription["brickMFRDisplayPanelData"] = "Displays vital information about the fission reactor.";
datablock fxDTSBrickData(brickMFRDisplayPanelData)
{
	brickFile = "./Bricks/MFRDisplay.blb";
	category = "Nuclear";
	subCategory = "Base Parts";
	uiName = "MFR Display Panel";

	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
	inspectFunc = "EOTW_DisplayPanelInspectLoop";
};

function Player::EOTW_DisplayPanelInspectLoop(%player, %brick)
{
	cancel(%player.PoweredBlockInspectLoop);
	
	if (!isObject(%client = %player.client))
		return;

	if (!isObject(%brick) || !%player.LookingAtBrick(%brick))
	{
		%client.centerPrint("", 1);
		return;
	}

	//%client.centerPrint(%printText, 1);
	
	%data = %brick.getDatablock();
	
	%player.PoweredBlockInspectLoop = %player.schedule(1000 / $EOTW::PowerTickRate, "EOTW_DisplayPanelInspectLoop", %brick);
}
