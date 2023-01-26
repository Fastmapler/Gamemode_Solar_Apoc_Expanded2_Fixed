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

        %obj.inputLeft -= %source.transferBrickPower(%obj.inputLeft, %obj);

        if (%obj.inputLeft < 1)
            break;
    }
}

function fxDtsBrick::attemptPowerDraw(%obj, %amount)
{
    %drawLeft = %amount;
    %set = %obj.connections["Battery"];
    for (%i = 0; %i < getFieldCount(%set); %i++)
    {
        %source = getField(%set, %i);

        if (!isObject(%source))
        {
            %obj.searchForConnections("Battery");
            continue;
        }

        %drawLeft += %source.changeBrickPower(-1 * %drawLeft);

        if (%drawLeft < 1)
            return true;
    }

    return false;
}

datablock fxDTSBrickData(brickEOTWPowerUnitTestData)
{
	brickFile = "./Shapes/Generic.blb";
	category = "Solar Apoc";
	subCategory = "Power Unit";
	uiName = "Power Unit";
	//iconName = "";

    isPowered = true;
	powerType = "Battery";
    maxBuffer = 2048;
    maxInput  = 96;
    maxOutput = 128;
    maxRange  = 16;
    maxConnect= 4;
};
$EOTW::CustomBrickCost["brickEOTWPowerUnitTestData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWPowerUnitTestData"] = "Takes in power from nearby power sources, and allows machines to use it.";

function brickEOTWPowerUnitTestData::onTick(%this, %obj) { onTickPowerUnit(%this, %obj); }