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

function setVCEVariables()
{
    unregisterSpecialVar(GameConnection,"kdratio");
    
    unregisterSpecialVar(GameConnection,"clanPrefix");
	registerSpecialVar(GameConnection,"clanPrefix","%this.clanPrefix");

    unregisterSpecialVar(GameConnection,"clanSuffix");
	registerSpecialVar(GameConnection,"clanSuffix","%this.clanSuffix");

    unregisterSpecialVar(GameConnection,"score");
    registerSpecialVar(GameConnection,"score","%this.score");

    unregisterSpecialVar(Player,"energy");
    registerSpecialVar(Player,"energy","%this.getEnergyLevel()");

    unregisterSpecialVar(Player,"damage");
	registerSpecialVar(Player,"damage","%this.getDamageLevel()");

    unregisterSpecialVar(Player,"health");
	registerSpecialVar(Player,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()");

    unregisterSpecialVar(Player,"vel");
    registerSpecialVar(Player,"vel","getWords(%this.getVelocity(),0,2)");

    unregisterSpecialVar(Player,"currentitem","%this.tool[%this.currTool].uiName","setCurrentItem");
    registerSpecialVar(Player,"currentitem","%this.tool[%this.currTool].uiName"); 
    
	unregisterSpecialVar(Player,"item1");
    registerSpecialVar(Player,"item1","%this.tool[0].uiName");

	unregisterSpecialVar(Player,"item2");
    registerSpecialVar(Player,"item2","%this.tool[1].uiName");

	unregisterSpecialVar(Player,"item3");
    registerSpecialVar(Player,"item3","%this.tool[2].uiName");

	unregisterSpecialVar(Player,"item4");
    registerSpecialVar(Player,"item4","%this.tool[3].uiName");

	unregisterSpecialVar(Player,"item5");
    registerSpecialVar(Player,"item5","%this.tool[4].uiName");

	unregisterSpecialVar(Player,"item6");
    registerSpecialVar(Player,"item6","%this.tool[5].uiName");

	unregisterSpecialVar(Player,"item7");
    registerSpecialVar(Player,"item7","%this.tool[6].uiName");

	unregisterSpecialVar(Player,"item8");
    registerSpecialVar(Player,"item8","%this.tool[7].uiName");

	unregisterSpecialVar(Player,"item9");
    registerSpecialVar(Player,"item9","%this.tool[8].uiName");

	unregisterSpecialVar(Player,"item10");
    registerSpecialVar(Player,"item10","%this.tool[9].uiName");

    unregisterSpecialVar(Vehicle,"damage");
    registerSpecialVar(Vehicle,"damage","%this.getDamageLevel()");

    unregisterSpecialVar(Vehicle,"health");
	registerSpecialVar(Vehicle,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()");

    unregisterSpecialVar(Vehicle,"vel");
    registerSpecialVar(Vehicle,"vel","%this.getVelocity()");

    unregisterSpecialVar(Vehicle,"pos");
    registerSpecialVar(Vehicle,"pos","getWords(%this.getTransform(),0,2)");

    //Remove bot variables
    unregisterSpecialVar(Bot,"yaw");
    unregisterSpecialVar(Bot,"pitch");
    unregisterSpecialVar(Bot,"hat");
    unregisterSpecialVar(Bot,"accent");
    unregisterSpecialVar(Bot,"pack");
    unregisterSpecialVar(Bot,"secondPack");
    unregisterSpecialVar(Bot,"hip");
    unregisterSpecialVar(Bot,"rleg");
    unregisterSpecialVar(Bot,"rarm");
    unregisterSpecialVar(Bot,"lleg");
    unregisterSpecialVar(Bot,"larm");
    unregisterSpecialVar(Bot,"chest");
    unregisterSpecialVar(Bot,"decal");
    unregisterSpecialVar(Bot,"face");
    unregisterSpecialVar(Bot,"energy");
    unregisterSpecialVar(Bot,"damage");
    unregisterSpecialVar(Bot,"health");
    unregisterSpecialVar(Bot,"velx");
    unregisterSpecialVar(Bot,"vely");
    unregisterSpecialVar(Bot,"velz");
    unregisterSpecialVar(Bot,"vel");
    unregisterSpecialVar(Bot,"posx");
    unregisterSpecialVar(Bot,"posy");
    unregisterSpecialVar(Bot,"posz");
    unregisterSpecialVar(Bot,"pos");
    unregisterSpecialVar(Bot,"speed");
    unregisterSpecialVar(Bot,"crouching");
    unregisterSpecialVar(Bot,"jumping");
    unregisterSpecialVar(Bot,"jetting");
    unregisterSpecialVar(Bot,"firing");
    unregisterSpecialVar(Bot,"sitting");
    unregisterSpecialVar(Bot,"datablock");
    unregisterSpecialVar(Bot,"weapon");
    unregisterSpecialVar(Bot,"type");
    unregisterSpecialVar(Bot,"state");
    unregisterSpecialVar(Bot,"enabled");

    //Solar Apoc special stuff
    registerSpecialVar(fxDTSbrick,"brickPower","%this.getPower()");
    registerSpecialVar(fxDTSbrick,"netPower","%this.cablenet.powerBuffer");
    registerSpecialVar(fxDTSbrick,"machineDisabled","%this.machineDisabled");
    registerSpecialVar(fxDTSbrick,"processingRecipe","cleanRecipeName(%this.processingRecipe)");
    registerSpecialVar(fxDTSbrick,"recipeProgress","%this.recipeProgress");
    
    registerSpecialVar(fxDTSbrick,"input1name","getField(%this.matter[Input, 0], 0)");
    registerSpecialVar(fxDTSbrick,"input1amount","getField(%this.matter[Input, 0], 1)");
    registerSpecialVar(fxDTSbrick,"input2name","getField(%this.matter[Input, 1], 0)");
    registerSpecialVar(fxDTSbrick,"input2amount","getField(%this.matter[Input, 1], 1)");
    registerSpecialVar(fxDTSbrick,"input3name","getField(%this.matter[Input, 2], 0)");
    registerSpecialVar(fxDTSbrick,"input3amount","getField(%this.matter[Input, 2], 1)");
    registerSpecialVar(fxDTSbrick,"input4name","getField(%this.matter[Input, 3], 0)");
    registerSpecialVar(fxDTSbrick,"input4amount","getField(%this.matter[Input, 3], 1)");

    registerSpecialVar(fxDTSbrick,"buffer1name","getField(%this.matter[buffer, 0], 0)");
    registerSpecialVar(fxDTSbrick,"buffer1amount","getField(%this.matter[buffer, 0], 1)");
    registerSpecialVar(fxDTSbrick,"buffer2name","getField(%this.matter[buffer, 1], 0)");
    registerSpecialVar(fxDTSbrick,"buffer2amount","getField(%this.matter[buffer, 1], 1)");
    registerSpecialVar(fxDTSbrick,"buffer3name","getField(%this.matter[buffer, 2], 0)");
    registerSpecialVar(fxDTSbrick,"buffer3amount","getField(%this.matter[buffer, 2], 1)");
    registerSpecialVar(fxDTSbrick,"buffer4name","getField(%this.matter[buffer, 3], 0)");
    registerSpecialVar(fxDTSbrick,"buffer4amount","getField(%this.matter[buffer, 3], 1)");

    registerSpecialVar(fxDTSbrick,"output1name","getField(%this.matter[output, 0], 0)");
    registerSpecialVar(fxDTSbrick,"output1amount","getField(%this.matter[output, 0], 1)");
    registerSpecialVar(fxDTSbrick,"output2name","getField(%this.matter[output, 1], 0)");
    registerSpecialVar(fxDTSbrick,"output2amount","getField(%this.matter[output, 1], 1)");
    registerSpecialVar(fxDTSbrick,"output3name","getField(%this.matter[output, 2], 0)");
    registerSpecialVar(fxDTSbrick,"output3amount","getField(%this.matter[output, 2], 1)");
    registerSpecialVar(fxDTSbrick,"output4name","getField(%this.matter[output, 3], 0)");
    registerSpecialVar(fxDTSbrick,"output4amount","getField(%this.matter[output, 3], 1)");

    registerSpecialVar(fxDTSbrick,"ugVeinSize","%this.getUGVeinComp()");
    registerSpecialVar(fxDTSbrick,"ugVeinType","%this.getUGVeinType()");

    registerSpecialVar(GameConnection,"scanMaterialName","%this.scanMaterialName");
    registerSpecialVar(GameConnection,"scanMaterialAmount","$EOTW::Material[%this.bl_id, %this.scanMaterialName]");

    registerSpecialVar("GLOBAL","SA_Day","$EOTW::Day");
    registerSpecialVar("GLOBAL","SA_Time","$EOTW::Time");
}
setVCEVariables();

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