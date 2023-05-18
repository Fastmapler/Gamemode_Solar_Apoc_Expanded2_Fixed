datablock AudioProfile(PowerUnitLoopSound)
{
   filename    = "./Sounds/PowerCell.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

function onTickPowerUnit(%this, %obj)
{
    //We handle power transfer in the cable network object.
}

datablock fxDTSBrickData(brickEOTWPowerUnit1Data)
{
	brickFile = "./Shapes/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Unit";
	uiName = "Power Cell";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Capacitor1";

    isPowered = true;
	powerType = "Battery";
    isProcessingMachine = true;
    maxBuffer = $EOTW::PowerLevel[0] << 8;
    maxInput  = $EOTW::PowerLevel[0] << 3;
    maxOutput = $EOTW::PowerLevel[0] << 4;
    maxRange  = 16;
    maxConnect= 4;

    processSound = PowerUnitLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWPowerUnit1Data"] = 1.00 TAB "d36b04ff" TAB 128 TAB "Steel" TAB 128 TAB "Electrum" TAB 128 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerUnit1Data"] = "Takes in power from power sources, and allows machines to use it.";
$EOTW::BrickDescription["brickEOTWPowerUnit1Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit1Data"] @ "<br>(" @ brickEOTWPowerUnit1Data.maxBuffer SPC " BUFFER";
$EOTW::BrickDescription["brickEOTWPowerUnit1Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit1Data"] @ "|" @ brickEOTWPowerUnit1Data.maxInput SPC " IN";
$EOTW::BrickDescription["brickEOTWPowerUnit1Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit1Data"] @ "|" @ brickEOTWPowerUnit1Data.maxOutput SPC " OUT)";

function brickEOTWPowerUnit1Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerUnit1Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }

datablock fxDTSBrickData(brickEOTWPowerUnit2Data : brickEOTWPowerUnit1Data)
{
	uiName = "Power Cell II";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Capacitor2";

    maxBuffer = $EOTW::PowerLevel[1] << 8;
    maxInput  = $EOTW::PowerLevel[1] << 3;
    maxOutput = $EOTW::PowerLevel[1] << 4;
};
$EOTW::CustomBrickCost["brickEOTWPowerUnit2Data"] = 1.00 TAB "dfc47cff" TAB 128 TAB "Adamantine" TAB 128 TAB "Energium" TAB 256 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerUnit2Data"] = "Better battery for better storage.";
$EOTW::BrickDescription["brickEOTWPowerUnit2Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit2Data"] @ "<br>(" @ brickEOTWPowerUnit2Data.maxBuffer SPC " BUFFER";
$EOTW::BrickDescription["brickEOTWPowerUnit2Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit2Data"] @ "|" @ brickEOTWPowerUnit2Data.maxInput SPC " IN";
$EOTW::BrickDescription["brickEOTWPowerUnit2Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit2Data"] @ "|" @ brickEOTWPowerUnit2Data.maxOutput SPC " OUT)";

function brickEOTWPowerUnit2Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerUnit2Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }

datablock fxDTSBrickData(brickEOTWPowerUnit3Data : brickEOTWPowerUnit1Data)
{
	uiName = "Power Cell III";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Capacitor3";

    maxBuffer = $EOTW::PowerLevel[2] << 8;
    maxInput  = $EOTW::PowerLevel[2] << 3;
    maxOutput = $EOTW::PowerLevel[2] << 4;
};
$EOTW::CustomBrickCost["brickEOTWPowerUnit3Data"] = 1.00 TAB "d69c6bff" TAB 128 TAB "Rare Earths" TAB 128 TAB "Plutonium" TAB 512 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerUnit3Data"] = "Superior storage for super electrical uses.";
$EOTW::BrickDescription["brickEOTWPowerUnit3Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit3Data"] @ "<br>(" @ brickEOTWPowerUnit3Data.maxBuffer SPC " BUFFER";
$EOTW::BrickDescription["brickEOTWPowerUnit3Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit3Data"] @ "|" @ brickEOTWPowerUnit3Data.maxInput SPC " IN";
$EOTW::BrickDescription["brickEOTWPowerUnit3Data"] = $EOTW::BrickDescription["brickEOTWPowerUnit3Data"] @ "|" @ brickEOTWPowerUnit3Data.maxOutput SPC " OUT)";

function brickEOTWPowerUnit3Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerUnit3Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }

datablock fxDTSBrickData(brickEOTWPowerBank1Data : brickEOTWPowerUnit1Data)
{
	uiName = "Power Bank I";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Capacitor1";

    maxBuffer = $EOTW::PowerLevel[0] << 12;
    maxInput  = $EOTW::PowerLevel[0] << 1;
    maxOutput = $EOTW::PowerLevel[0] << 2;
};
$EOTW::CustomBrickCost["brickEOTWPowerBank1Data"] = 1.00 TAB "d36b04ff" TAB 128 TAB "Steel" TAB 128 TAB "Electrum" TAB 128 TAB "Gold";
$EOTW::BrickDescription["brickEOTWPowerBank1Data"] = "Alternative better battery with more space but lower power I/O rates.";
$EOTW::BrickDescription["brickEOTWPowerBank1Data"] = $EOTW::BrickDescription["brickEOTWPowerBank1Data"] @ "<br>(" @ brickEOTWPowerBank1Data.maxBuffer SPC " BUFFER";
$EOTW::BrickDescription["brickEOTWPowerBank1Data"] = $EOTW::BrickDescription["brickEOTWPowerBank1Data"] @ "|" @ brickEOTWPowerBank1Data.maxInput SPC " IN";
$EOTW::BrickDescription["brickEOTWPowerBank1Data"] = $EOTW::BrickDescription["brickEOTWPowerBank1Data"] @ "|" @ brickEOTWPowerBank1Data.maxOutput SPC " OUT)";

function brickEOTWPowerBank1Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerBank1Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }

datablock fxDTSBrickData(brickEOTWPowerBank2Data : brickEOTWPowerUnit1Data)
{
	uiName = "Power Bank II";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Capacitor1";

    maxBuffer = $EOTW::PowerLevel[1] << 12;
    maxInput  = $EOTW::PowerLevel[1] << 1;
    maxOutput = $EOTW::PowerLevel[1] << 2;
};
$EOTW::CustomBrickCost["brickEOTWPowerBank2Data"] = 1.00 TAB "dfc47cff" TAB 128 TAB "Adamantine" TAB 128 TAB "Energium" TAB 256 TAB "Gold";
$EOTW::BrickDescription["brickEOTWPowerBank2Data"] = "Alternative battery with more space but lower power I/O rates.";
$EOTW::BrickDescription["brickEOTWPowerBank2Data"] = $EOTW::BrickDescription["brickEOTWPowerBank2Data"] @ "<br>(" @ brickEOTWPowerBank2Data.maxBuffer SPC " BUFFER";
$EOTW::BrickDescription["brickEOTWPowerBank2Data"] = $EOTW::BrickDescription["brickEOTWPowerBank2Data"] @ "|" @ brickEOTWPowerBank2Data.maxInput SPC " IN";
$EOTW::BrickDescription["brickEOTWPowerBank2Data"] = $EOTW::BrickDescription["brickEOTWPowerBank2Data"] @ "|" @ brickEOTWPowerBank2Data.maxOutput SPC " OUT)";

function brickEOTWPowerBank2Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerBank2Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }

datablock fxDTSBrickData(brickEOTWPowerBank3Data : brickEOTWPowerUnit1Data)
{
	uiName = "Power Bank III";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Capacitor1";

    maxBuffer = $EOTW::PowerLevel[2] << 12;
    maxInput  = $EOTW::PowerLevel[2] << 1;
    maxOutput = $EOTW::PowerLevel[2] << 2;
};
$EOTW::CustomBrickCost["brickEOTWPowerBank3Data"] = 1.00 TAB "d69c6bff" TAB 128 TAB "Rare Earths" TAB 128 TAB "Plutonium" TAB 512 TAB "Gold";
$EOTW::BrickDescription["brickEOTWPowerBank3Data"] = "Alternative superior battery with more space but lower power I/O rates.";
$EOTW::BrickDescription["brickEOTWPowerBank3Data"] = $EOTW::BrickDescription["brickEOTWPowerBank3Data"] @ "<br>(" @ brickEOTWPowerBank3Data.maxBuffer SPC " BUFFER";
$EOTW::BrickDescription["brickEOTWPowerBank3Data"] = $EOTW::BrickDescription["brickEOTWPowerBank3Data"] @ "|" @ brickEOTWPowerBank3Data.maxInput SPC " IN";
$EOTW::BrickDescription["brickEOTWPowerBank3Data"] = $EOTW::BrickDescription["brickEOTWPowerBank3Data"] @ "|" @ brickEOTWPowerBank3Data.maxOutput SPC " OUT)";

function brickEOTWPowerBank3Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerBank3Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }