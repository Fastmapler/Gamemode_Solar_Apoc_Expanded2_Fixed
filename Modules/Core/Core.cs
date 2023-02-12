exec("./CreateBrick.cs");
exec("./CustomBricks.cs");
exec("./Extra.cs");
exec("./Server_Ropes.cs");
exec("./uInt.cs");
exec("./Support_FloatingBricks.cs");
exec("./Support_ArbyVars.cs");
exec("./Support_Saving.cs");
exec("./Support_TransferNewFiles.cs");

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
        VehicleRespawnTime = 10;
        BrickRespawnTime = 30;
        BotRespawnTime = 5;
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
        StartEquip2 = SurvivalKnifeItem.getID();
        StartEquip3 = RecurveBowItem.getID();
        StartEquip4 = 0;
        TimeLimit = -1;
    };
    MiniGameGroup.add ($DefaultMiniGame);
}
schedule(10, 0, "createDefaultMinigame");