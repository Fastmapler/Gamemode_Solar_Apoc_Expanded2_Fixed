exec("./Tool_SurvivalKnife.cs");

exec("./Tool_Scanner.cs");
exec("./Tool_OilPump.cs");
//exec("./Tool_Multitool.cs");

exec("./Tool_Sickle.cs");
exec("./Tool_Pickaxes.cs");
exec("./Tool_Dynamite.cs");
exec("./Tool_AutoDrill.cs");

exec("./Tool_Potions.cs");
exec("./Tool_BossKey.cs");
exec("./Tool_Chipper.cs");
exec("./Tool_SoftHammer.cs");

exec("./Tool_InvExpander.cs");
exec("./Tool_UpgradeKit.cs");

exec("./Support_DropInventoryOnDeath.cs");

$Game::Item::PopTime = 60 * 2 * 1000;

//Blacklist specific items
function updateItemNames()
{
    if ($Pref::Server::SAEX2::DevMode)
        return;

	$EOTW::BacklistedItems = 0;
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "BowItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "GunItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "AkimboGunItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "horseRayItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "pushBroomItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "rocketLauncherItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "spearItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "swordItem";
    
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "acidItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "BioRifleItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "InfernalRangerStoneItem";

    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "DragonFireBreathItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "DragonFireBallItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "DragonFireBarrageItem";

    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "nukeLauncherItem";

	for(%i = 0; $EOTW::BacklistedItem[%i] !$= ""; %i++)
	{
		%data = ($EOTW::BacklistedItem[%i]).getID();

        if (isObject(%data))
            %data.uiName = "";
	}
	
	createuinametable();
	transmitdatablocks();
	commandtoall('missionstartphase3');

    //Can be obtained, but not craftable/spawned by a brick
    $EOTW::UniqueItem["crystalHalberdItem"] = true;
    $EOTW::UniqueItem["crystalBowItem"] = true;
    $EOTW::UniqueItem["crystalStaveItem"] = true;
    $EOTW::UniqueItem["bulwarkItem"] = true;

    $EOTW::UniqueVehicle["AirDragonArmor"] = true;
    $EOTW::UniqueVehicle["LandDragonArmor"] = true;
    $EOTW::UniqueVehicle["FlyingWheeledJeepVehicle"] = true;
    $EOTW::UniqueVehicle["MagicCarpetVehicle"] = true;
    $EOTW::UniqueVehicle["CannonTurret"] = true;
    $EOTW::UniqueVehicle["TankVehicle"] = true;
    $EOTW::UniqueVehicle["TankTurretPlayer"] = true;  
}
schedule(0, 0, "updateItemNames");

function ServerCmdGrantItem(%client, %data, %target)
{
    if (!%client.isSuperAdmin || !isObject(%targetPlayer = findClientByName(%target).player) || !isObject(%data))
        return;

    %item = new Item()
    {
        dataBlock = %data;
        position = %targetPlayer.getPosition();
    };

    %client.chatMessage("Granted");
}

$EOTW::CustomBrickCost["brickToolStorageData"] = 1.00 TAB "c1a872ff" TAB 512 TAB "Wood" TAB 128 TAB "Granite";
$EOTW::BrickDescription["brickToolStorageData"] = "Holds up to 5 held tools.";
datablock fxDTSBrickData(brickToolStorageData)
{
	brickFile = "./Shapes/Box.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "Tool Crate";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Tools/Icons/StorageCrate";

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