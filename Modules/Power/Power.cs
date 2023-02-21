exec("./Support_PowerNet.cs");
exec("./Brick_PowerGen.cs");
exec("./Brick_PowerUnits.cs");
exec("./Brick_Processing.cs");
exec("./Brick_PowerMachines.cs");
exec("./Brick_Hatches.cs");
//exec("./Brick_Debug.cs");

function fxDtsBrick::isOnPublicBrick(%obj)
{
    for (%i = 0; isObject(%obj.getDownBrick(%i)); %i++)
        if (isObject(%group = %obj.getDownBrick(%i).getGroup()) && %group.bl_id == 888888)
            return true;

    return false;
}

$EOTW::CustomBrickCost["brickToolStorageData"] = 1.00 TAB "c1a872ff" TAB 512 TAB "Wood" TAB 128 TAB "Granite";
$EOTW::BrickDescription["brickToolStorageData"] = "Holds up to 5 held tools.";
datablock fxDTSBrickData(brickToolStorageData)
{
	brickFile = "Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Power/Bricks/Box.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "Tool Crate";
	//iconName = "./Bricks/Icon_Generator";

    maxStoredTools = 5;
};

function brickToolStorageData::onPlant(%this,%brick)
{
	Parent::onPlant(%this,%brick);
	if (isObject(%brick) && %brick.getDatablock().maxStoredTools > 0)
	{
		%brick.ignoreEventRestriction = true;
		%brick.addEvent(true, 0, "onActivate", "Self", "openStorage", 5, "", "", "");
	}
}