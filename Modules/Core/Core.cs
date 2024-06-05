exec("./CreateBrick.cs");
exec("./CustomBricks.cs");
exec("./Extra.cs");
exec("./Server_Ropes.cs");
exec("./uInt.cs");
exec("./Support_FloatingBricks.cs");
exec("./Support_ArbyVars.cs");
exec("./Support_Saving.cs");
exec("./Support_TransferNewFiles.cs");
exec("./Support_NewPop.cs");
exec("./BotDebug.cs");

$GameModeDisplayName = "SA EX2";

$GuiAudioType = 1;
$SimAudioType = 2;
$MessageAudioType = 3;


function createDefaultMinigame()
{
    if (isObject($DefaultMiniGame))
        return;

    $DefaultMiniGame = new ScriptObject ("")
    {
        class = MiniGameSO;
        owner = 0;
        title = "Solar Apoc Expanded 2";
        colorIdx = 1;
        numMembers = 0;
        InviteOnly = false;
        UseAllPlayersBricks = true;
        PlayersUseOwnBricks = false;
        UseSpawnBricks = true;
        Points_BreakBrick = 0;
        Points_PlantBrick = 0;
        Points_KillPlayer = 0;
        Points_KillSelf = 0;
        Points_KillBot = 0;
        Points_Die = 0;
        RespawnTime = 5000;
        VehicleRespawnTime = 10000;
        BrickRespawnTime = 30000;
        BotRespawnTime = 5000;
        FallingDamage = true;
        WeaponDamage = true;
        SelfDamage = true;
        VehicleDamage = true;
        BrickDamage = false;
        BotDamage = true;
        EnableWand = true;
        EnableBuilding = true;
        EnablePainting = true;
        PlayerDataBlock = PlayerSolarApoc;
        StartEquip0 = hammerItem.getID();
        StartEquip1 = WrenchItem.getID();
        StartEquip2 = 0;
        StartEquip3 = 0;
        StartEquip4 = 0;
        TimeLimit = -1;
    };
    MiniGameGroup.add ($DefaultMiniGame);
}
schedule(10, 0, "createDefaultMinigame");

package NoEarlyJoin
{
	function servAuthTCPobj::onLine(%this, %line)
	{
		%c = Parent::onLine(%this, %line);

		if(getWord(%line, 0) $= "YES")
			$db = getWord(%line, 1);

		return %c;
	}
	function GameConnection::onConnectRequest(%this, %netAddress, %lanName, %netName, %prefix, %suffix, %int, %g, %h, %i)
	{
		%c = Parent::onConnectRequest(%this, %netAddress, %lanName, %netName, %prefix, %suffix, %int, %g, %h, %i);

        if($db !$= 999999 && $db !$= getNumKeyID() && !$EOTW::Initilized)
            return "Server is booting up, try joining again in a few minutes!";

		return %c;
	}
};
activatePackage(NoEarlyJoin);