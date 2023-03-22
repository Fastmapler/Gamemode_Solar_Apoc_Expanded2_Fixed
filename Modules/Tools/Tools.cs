exec("./Tool_SurvivalKnife.cs");

exec("./Tool_Scanner.cs");
exec("./Tool_OilPump.cs");
//exec("./Tool_Multitool.cs");

exec("./Tool_Sickle.cs");
exec("./Tool_Pickaxes.cs");
exec("./Tool_Dynamite.cs");

exec("./Tool_Potions.cs");
exec("./Tool_BossKey.cs");
exec("./Tool_Chipper.cs");

exec("./Tool_InvExpander.cs");
exec("./Tool_UpgradeKit.cs");

exec("./Support_DropInventoryOnDeath.cs");

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

    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "DeagonFireBreathItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "DragonFireBallItem";
    $EOTW::BacklistedItem[-1 + $EOTW::BacklistedItems++] = "DragonFireBarrageItem";

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
}
schedule(0, 0, "updateItemNames");

$Game::Item::PopTime = 1000 * 60 * 0.5;

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