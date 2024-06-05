exec("./Tool_SurvivalKnife.cs");

exec("./Tool_Scanner.cs");
exec("./Tool_OilPump.cs");
exec("./Tool_Multitool.cs");

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
exec("./Tool_Implants.cs");

exec("./Tool_PSS.cs");

exec("./Support_DropInventoryOnDeath.cs");

$Game::Item::PopTime = 60 * 5 * 1000;

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

    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "TankTurretPlayer";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "TankVehicle";

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

//Jetpack integration. Does not much without Vehicle_Jetpack and the MoveHandler DLL.
$EOTW::ItemCrafting["JetpackItem"] = (256 TAB "Adamantine") TAB (128 TAB "Epoxy") TAB (32 TAB "Rare Earths");
$EOTW::ItemDescription["JetpackItem"] = "Take to the skies! Uses a ton of Jet Fuel to run.";

package EOTW_Jetpack
{
    function Jetpackvehicle::onadd(%this,%obj)
    { 
        
        parent::onadd(%this,%obj);

        JetpackContrailCheck(%obj);
            
    }
    function JetpackContrailCheck(%obj)
    {
        if(!isObject(%obj))
            return;

        %speed = vectorLen(%obj.getVelocity());
        if(%speed < %obj.dataBlock.minContrailSpeed)
        {
            if(%obj.getMountedImage(0) !$= "")
            {

                %obj.unMountImage(0);
                %obj.unMountImage(1);
            }
        }
        else
        {
            if(%obj.getMountedImage(0) $= 0)
            {

                %obj.mountImage(jetpackcontrailImage,0);
                %obj.mountImage(jetpackcontrailImage2,1);
            }
        }

        if ($MoveHandlerEnabled && isObject(%player = %obj.getMountedObject(0)) && isObject(%client = %player.client))
        {
            %ammoType = "Jet Fuel";
            if (mAbs(getWord(%player.moveHandler, 1)) > 0.1)
            {
                 //I am just going to leave this strange ranging potion interaction here for the fun of it.
                %shellcount = 1;
                %shellcount = getMin($EOTW::Material[%client.bl_id, %ammoType], %shellcount);
                if (%shellcount < 1)
                {
                    %player.unMount();
                    %client.chatMessage("Not enough fuel!");
                }
                if (!%player.hasEffect("Ranging") || getRandom() < 0.5)
                    $EOTW::Material[%client.bl_id, %ammoType] -= %shellcount;
            }

            %client.CenterPrint("<just:right>Jet Fuel: " @  $EOTW::Material[%client.bl_id, %ammoType], 1);
        }

        schedule(100,0,"JetpackContrailCheck",%obj);
    }

    function JetpackVehicle::onImpact(%this,%obj)
    {
        //echo("skivehicle impact");
        %trans = %obj.getTransform();
        %p = new Projectile()
        {
            dataBlock = skiImpactAProjectile;
            initialVelocity  = "0 0 0";
            initialPosition  = %trans;
            sourceObject     = %obj;
            sourceSlot       = 0;
            client           = %obj.client;
        };
        MissionCleanup.add(%p);

        //Blow up epically if we impact too hard
    }
};
activatePackage("EOTW_Jetpack");