datablock AudioProfile(PowerUnitLoopSound)
{
   filename    = "./Sounds/PowerCell.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

function onTickPowerUnit(%this, %obj)
{
    %obj.inputLeft =  %this.maxInput;
    %obj.outputLeft = %this.maxOutput;

    %set = %obj.connections["Source"];
    for (%i = 0; %i < getFieldCount(%set); %i++)
    {
        %source = getField(%set, %i);

        if (!isObject(%source))
        {
            %obj.searchForConnections("Source");
            continue;
        }
        %change = %source.transferBrickPower(%obj.inputLeft, %obj);
        %obj.inputLeft -= %change;

        if (%obj.inputLeft < 1)
            break;
    }
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
    maxInput  = $EOTW::PowerLevel[0] << 1;
    maxOutput = $EOTW::PowerLevel[0] << 2;
    maxRange  = 16;
    maxConnect= 4;

    processSound = PowerUnitLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWPowerUnit1Data"] = 1.00 TAB "d36b04ff" TAB 128 TAB "Steel" TAB 128 TAB "Electrum" TAB 128 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerUnit1Data"] = "Takes in power from power sources, and allows machines to use it.";

function brickEOTWPowerUnit1Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerUnit1Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }

datablock fxDTSBrickData(brickEOTWPowerUnit2Data)
{
	brickFile = "./Shapes/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Unit";
	uiName = "Power Cell II";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Capacitor2";

    isPowered = true;
	powerType = "Battery";
    isProcessingMachine = true;
    maxBuffer = $EOTW::PowerLevel[1] << 10;
    maxInput  = $EOTW::PowerLevel[1] << 1;
    maxOutput = $EOTW::PowerLevel[1] << 2;
    maxRange  = 16;
    maxConnect= 6;

    processSound = PowerUnitLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWPowerUnit2Data"] = 1.00 TAB "dfc47cff" TAB 128 TAB "Adamantine" TAB 128 TAB "Energium" TAB 256 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerUnit2Data"] = "Better battery for better storage.";

function brickEOTWPowerUnit2Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerUnit2Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }

datablock fxDTSBrickData(brickEOTWPowerUnit3Data)
{
	brickFile = "./Shapes/Capacitor.blb";
	category = "Solar Apoc";
	subCategory = "Power Unit";
	uiName = "Power Cell III";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Capacitor3";

    isPowered = true;
	powerType = "Battery";
    isProcessingMachine = true;
    maxBuffer = $EOTW::PowerLevel[2] << 12;
    maxInput  = $EOTW::PowerLevel[2] << 1;
    maxOutput = $EOTW::PowerLevel[2] << 2;
    maxRange  = 16;
    maxConnect= 8;

    processSound = PowerUnitLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWPowerUnit3Data"] = 1.00 TAB "d69c6bff" TAB 128 TAB "Rare Earths" TAB 128 TAB "Plutonium" TAB 512 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerUnit3Data"] = "Superior storage for super electrical uses.";

function brickEOTWPowerUnit3Data::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }
function brickEOTWPowerUnit3Data::getProcessingText(%this, %obj) { return "\c6Power: " @ %obj.getPower() @ "/" @ %obj.getMaxPower(); }