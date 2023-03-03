datablock fxDTSBrickData(brickEOTWWaterPumpData)
{
	brickFile = "./Shapes/WaterPump.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Water Pump";
	//iconName = "";

    isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Output"] = 1;
    inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWWaterPumpData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 128 TAB "Copper" TAB 128 TAB "Lead";
$EOTW::BrickDescription["brickEOTWWaterPumpData"] = "A device that draws water deep within the ground. Can be operated manually.";

function brickEOTWWaterPumpData::onTick(%this, %obj) {
	if (getSimTime() - %obj.lastDrawSuccess >= 100 && %obj.GetMatter("Water", "Output") < 128 && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{
        %obj.ChangeMatter("Water", 4, "Output");
	}
}

function brickEOTWWaterPumpData::onInspect(%this, %obj, %client) {
    if (%obj.GetMatter("Water", "Output") < 128 && getSimTime() - %obj.lastDrawSuccess >= 100)
    {
        %obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
        %obj.ChangeMatter("Water", 4, "Output");
    }
}

datablock fxDTSBrickData(brickEOTWThumperData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Mining Thumper";

	isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWThumperData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Adamantine" TAB 256 TAB "Lead" TAB 128 TAB "Teflon";
$EOTW::BrickDescription["brickEOTWThumperData"] = "When active gives a 100% speed boost (128 stud radius) to gathering nearby resources. Stacks.";

function brickEOTWThumperData::onTick(%this, %obj)
{
	if (%obj.attemptPowerDraw($EOTW::PowerLevel[1] >> 1))
		%obj.lastThump = getSimTime();
}

datablock fxDTSBrickData(brickEOTWSupersonicSpeakerData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Super-sonic Speaker";

	isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWSupersonicSpeakerData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Rubber" TAB 256 TAB "Steel" TAB 256 TAB "Copper";
$EOTW::BrickDescription["brickEOTWSupersonicSpeakerData"] = "Prevents enemies from spawning in its 64 stud radius. Enemies can still wander in, however.";

function brickEOTWSupersonicSpeakerData::onTick(%this, %obj)
{
	if (%obj.attemptPowerDraw($EOTW::PowerLevel[1] >> 1))
	{
		//Stuff
	}
}

datablock fxDTSBrickData(brickEOTWChemDiffuserData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Chemical Deffuser";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWChemDiffuserData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Rubber" TAB 256 TAB "Steel" TAB 256 TAB "Copper";
$EOTW::BrickDescription["brickEOTWChemDiffuserData"] = "Disperses held potion matter to nearby players. Uses the same amount of matter regardless of player count.";

function brickEOTWChemDiffuserData::onTick(%this, %obj)
{
	if (%obj.attemptPowerDraw($EOTW::PowerLevel[1] >> 1))
	{
		//Stuff
	}
}

datablock fxDTSBrickData(brickEOTWTurretData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Turret";

	matterSize = 256;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWTurretData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Electrum" TAB 128 TAB "Diamond";
$EOTW::BrickDescription["brickEOTWTurretData"] = "Fires at enemies using whatever ammo it is loaded with. Will also gather flesh when possible.";

function brickEOTWTurretData::onTick(%this, %obj)
{
	if (%obj.attemptPowerDraw($EOTW::PowerLevel[1] >> 1))
	{
		//Stuff
	}
}

datablock fxDTSBrickData(brickEOTWBiodomeData)
{
	brickFile = "./Shapes/Bioreactor.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Turret";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Biodome";
};
$EOTW::CustomBrickCost["brickEOTWBiodomeData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Quartz" TAB 256 TAB "Plastic" TAB 256 TAB "Uranium-238";
$EOTW::BrickDescription["brickEOTWBiodomeData"] = "Slowly grows plant life of your choice. Needs water. Speed and production can be boosted with ethylene.";

function brickEOTWBiodomeData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWBiodomeData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}