$EOTW::CustomBrickCost["brickMFRFuelPortBrick"] = 1.00 TAB "56643bff" TAB 1024 TAB "Energium" TAB 512 TAB "Teflon";
$EOTW::BrickDescription["brickMFRFuelPortBrick"] = "Takes in nuclear fissile fuels, outputs nuclear waste.";
datablock fxDTSBrickData(brickMFRFuelPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Fuel/Waste I/O";
	notRecolorable = true;

	matterMaxBuffer = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
	inspectFunc = "EOTW_DefaultInspectLoop";

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};

$EOTW::CustomBrickCost["brickMFRCoolantPortBrick"] = 1.00 TAB "3d5472ff" TAB 1024 TAB "Naturum" TAB 512 TAB "Teflon";
$EOTW::BrickDescription["brickMFRCoolantPortBrick"] = "Takes in coolants, outputs heated coolants.";
datablock fxDTSBrickData(brickMFRCoolantPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Coolant/Hot Coolant I/O";
	notRecolorable = true;

	matterMaxBuffer = 512;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;
	inspectFunc = "EOTW_DefaultInspectLoop";

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};

$EOTW::CustomBrickCost["brickMFRBreederPortBrick"] = 1.00 TAB "9c593dff" TAB 512 TAB "Sturdium" TAB 512 TAB "Epoxy";
$EOTW::BrickDescription["brickMFRBreederPortBrick"] = "Takes in specific materials, outputs neutron activated materials. Powered by placing fuel rods adjacent to reflectors.";
datablock fxDTSBrickData(brickMFRBreederPortBrick)
{
	brickFile = "./Bricks/MFRPort.blb";
	category = "Nuclear";
	subCategory = "Material Ports";
	uiName = "MFR Neutron Activator";
	notRecolorable = true;

	matterMaxBuffer = 64;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	energyGroup = "Source";
	energyMaxBuffer = 80;
	loopFunc = "EOTW_MatterReactorLoop";
	matterUpdateFunc = "EOTW_MatterReactorMatterUpdate";
	energyWattage = 40;
	inspectFunc = "EOTW_MatterReactorInspectLoop";

	ComponentType = "Port";
	reqFissionPart = brickMFRHullData;
	blacklistFromAdjacentScan = true;
};
