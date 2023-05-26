//------
//Variable/Conditional Events
//Creator: Zack0Wack0
//Helpers: Clockturn, Truce, Boom, Lilboarder32, Chrono, and Monoblaster
//------
//Major: 7
//Minor: 5
//Patch: 0
//Total: 7.50
$VCE::Server::Version = "7.50";
//---
// Support
//---
//eeee
//---
// Main
//---

if(isFile("add-ons/system_returntoblockland/server.cs"))
{
	RTB_registerPref("Can Non-Admins Edit Special Variables", "VCE", "$Pref::VCE::canEditSpecialVars", "bool", "Event_Variables", 1, 0, 0);
	RTB_registerPref("Show Vce Handshake", "VCE", "$Pref::Client::ShowVCEHandshake", "bool", "Event_Variables", 0, 0, 0);
	RTB_registerPref("Loop Delay", "VCE", "$Pref::VCE::LoopDelay", "int 0 30000", "Event_Variables", 33, 0, 0);
	RTB_registerPref("Event Functions Admin Only", "VCE", "$Pref::VCE::EventFunctionsAdminOnly", "bool", "Event_Variables", 1, 0, 0);
}

package VCE_Other
{
	function GameConnection::autoAdminCheck(%client)
	{
		%v = Parent::autoAdminCheck(%client);
 	
		schedule(500,0,"commandToClient",%client,'VCE_Handshake',$VCE::Server::Version);
		
		return %v;
	}
};
function VCE_initServer()
{
	deactivatePackage(VCE_Main);
	activatePackage(VCE_Main);
	deactivatePackage(VCE_FireRelayNumFix);
	activatePackage(VCE_FireRelayNumFix);
	deactivatePackage(VCE_Other);
	activatePackage(VCE_Other);
	//extends the targets of all listed items
	extendTargetList();
	//list of operators
	%functionParameter = "\tlist Set 0 Add 1 Subtract 2 Multiply 3 Divide 4 Modulos 16 Power 7 Radical 8 Percent 9 Random 10 Absolute 17 Floor 5 Ceil 6 Clamp 18 Sin 19 Cos 20 Tan 21 ASin 22 ACos 23 ATan 24 Length 15 StringPosition 25 Lowercase 12 Uppercase 13 Character 14 Replace 26 Trim 27 SubString 28 Words 11 CountWord 29 SubWord 30 RemoveWord 31 RemoveWords 32 SetWord 33 VectorDist 34 VectorAdd 35 VectorSub 36 VectorScale 37 VectorLen 38 VectorNormalize 39 VectorDot 40 VectorCross 41 VectorCenter 42 And 43 Or 44 BitwiseAnd 45 BitwiseOr 46 BitwiseShiftRight 47 BitwiseShiftLeft 48 BitwiseXOR 49 BitwiseComplement 50 BooleanInverse 51";
	//Register all events and special vars
	registerOutputEvent(fxDtsBrick,"VCE_modVariable","list Brick 0 Player 1 Client 2 Minigame 3 Vehicle 4 Bot 5 Local 6\tstring 180 100" @ %functionParameter @ "\tstring 180 1000",1);
	registerOutputEvent(fxDtsBrick,"VCE_ifValue","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_retroCheck","list ifPlayerName 0 ifPlayerID 1 ifAdmin 2 ifPlayerEnergy 3 ifPlayerDamage 4 ifPlayerScore 5 ifLastPlayerMsg 6 ifBrickName 7 ifRandomDice 8\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_stateFunction","string 32 100\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_startFunction","list Brick 0 Local 1\tstring 32 100\tstring 8 30",1);
	registerOutputEvent(fxDtsBrick,"VCE_callFunction","string 32 100\tstring 64 100\tstring 32 100",1);
	registerOutputEvent(fxDtsBrick,"VCE_relayCallFunction","list Up 0 Down 1 North 2 East 3 South 4 West 5\tstring 32 100\tstring 64 100\tstring 32 100",1);
	registerOutputEvent(fxDtsBrick,"VCE_cancelFunction","string 32 100",1);
	//registerOutputEvent(fxDtsBrick,"VCE_castRelay","list Up 0 Down 1 North 2 East 3 South 4 West 5\tint 1 96 2",1);
 	registerOutputEvent(fxDtsBrick,"VCE_saveVariable","list Client 2 Player 1 Brick 0 Local 6\tstring 200 255",1);
 	registerOutputEvent(fxDtsBrick,"VCE_loadVariable","list Client 2 Player 1 Brick 0 Local 6\tstring 200 255",1);
	registerInputEvent(fxDtsBrick,"onVariableTrue","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
	registerInputEvent(fxDtsBrick,"onVariableFalse","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
	registerInputEvent(fxDtsBrick,"onVariableFunction","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
	registerInputEvent(fxDtsBrick,"onVariableUpdate","Self fxDtsBrick\tPlayer Player\tClient GameConnection\tMinigame Minigame");
	registerOutputEvent(Player,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(Player,"VCE_modVariable","string 180 100" @ %functionParameter @ "\tstring 180 255",1);
	registerOutputEvent(GameConnection,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(GameConnection,"VCE_modVariable","string 180 100" @ %functionParameter @ "\tstring 180 255",1);
	registerOutputEvent(Minigame,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(Minigame,"VCE_modVariable","string 180 100" @ %functionParameter @ "\tstring 180 255",1);
	registerOutputEvent(Vehicle,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	registerOutputEvent(Vehicle,"VCE_modVariable","string 180 100" @ %functionParameter @ "\tstring 180 255",1);
	registerOutputEvent(Bot,"VCE_modVariable","string 180 100" @ %functionParameter @ "\tstring 180 255",1);
	registerOutputEvent(Bot,"VCE_ifVariable","string 100 100\tlist == 0 != 1 > 2 < 3 >= 4 <= 5 ~= 6\tstring 100 100\tstring 8 30",1);
	if(!$VCE::Server)
	{
		//event function hooks
		hookFunctionToVCEEventFunction("GameConnection","onConnectionDropped","%client, %msg","true","","onPlayerLeave");
		hookFunctionToVCEEventFunction("GameConnection","onDeath","%client, %sourceObject, %sourceClient, %damageType, %damLoc","true","","onPlayerDeath");
		activateVCEEventFunctionHooks();
		//special vars
		registerSpecialVar(GameConnection,"bl_id","%this.bl_id");
		registerSpecialVar(GameConnection,"name","%this.getPlayerName()");
		//registerSpecialVar(GameConnection,"kdratio","(isInt(%client.vceKills / %client.vceDeaths)) ? (%client.vceKills / %client.vceDeaths) : 0");
		//registerSpecialVar(GameConnection,"clanPrefix","%this.clanPrefix","setClanPrefix");
		//registerSpecialVar(GameConnection,"clanSuffix","%this.clanSuffix","setClanSuffix");
		registerSpecialVar(GameConnection,"score","%this.score"); //registerSpecialVar(GameConnection,"score","%this.score","setScore");
		registerSpecialVar(GameConnection,"hat","$hat[%this.hat]");
		registerSpecialVar(GameConnection,"accent","$accent[%this.accent]");
		registerSpecialVar(GameConnection,"pack","$pack[%this.pack]");
		registerSpecialVar(GameConnection,"secondPack","$secondpack[%this.secondpack]");
		registerSpecialVar(GameConnection,"hip","$hip[%this.hip]");
		registerSpecialVar(GameConnection,"rleg","$rleg[%this.rleg]");
		registerSpecialVar(GameConnection,"rarm","$rarm[%this.rarm]");
		registerSpecialVar(GameConnection,"lleg","$lleg[%this.lleg]");
		registerSpecialVar(GameConnection,"larm","$larm[%this.larm]");
		registerSpecialVar(GameConnection,"chest","$chest[%this.chest]");
		registerSpecialVar(GameConnection,"decal","%this.decalName","setDecalName");
		registerSpecialVar(GameConnection,"face","%this.faceName","setFaceName");
		registerSpecialVar(GameConnection,"lastmsg","%this.lastMessage");
		registerSpecialVar(GameConnection,"lastteammsg","%this.lastTeamMessage");
		registerSpecialVar(GameConnection,"isAdmin","%this.isAdmin");
		registerSpecialVar(GameConnection,"isSuperAdmin","%this.isSuperAdmin");
		registerSpecialVar(GameConnection,"isHost","(%this.bl_id == getNumKeyID() || %this.isLocalConnection())");
		registerSpecialVar(GameConnection,"isAlive","isObject(%this.player)");
		registerSpecialVar(Player,"energy","%this.getEnergyLevel()"); //registerSpecialVar(Player,"energy","%this.getEnergyLevel()","setEnergy");
		registerSpecialVar(Player,"damage","%this.getDamageLevel()"); //registerSpecialVar(Player,"damage","%this.getDamageLevel()","setDamage");
		registerSpecialVar(Player,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()"); //registerSpecialVar(Player,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()","setHealth");
		registerSpecialVar(Player,"maxhealth","%this.getDatablock().maxDamage");
		registerSpecialVar(Player,"velx","getWord(%this.getVelocity(),0)");
		registerSpecialVar(Player,"vely","getWord(%this.getVelocity(),1)");
		registerSpecialVar(Player,"velz","getWord(%this.getVelocity(),2)");
		registerSpecialVar(Player,"vel","getWords(%this.getVelocity(),0,2)"); //registerSpecialVar(Player,"vel","getWords(%this.getVelocity(),0,2)","setVelocity");
		registerSpecialVar(Player,"speed","vectorDist(%this.getVelocity(),\"0 0 0\")");
		registerSpecialVar(Player,"crouching","%this.VCETrigger3 $= \"\" ? 0 : %this.VCETrigger3");
		registerSpecialVar(Player,"jumping","%this.VCETrigger2 $= \"\" ? 0 : %this.VCETrigger2");
		registerSpecialVar(Player,"jetting","%this.VCETrigger4 $= \"\" ? 0 : %this.VCETrigger4");
		registerSpecialVar(Player,"firing","%this.VCETrigger0 $= \"\" ? 0 : %this.VCETrigger0");
		registerSpecialVar(Player,"sitting","%this.VCESitting $= \"\" ? 0 : %this.VCESitting");
		registerSpecialVar(Player,"datablock","%this.getDatablock().uiName");
		registerSpecialVar(Player,"currentitem","%this.tool[%this.currTool].uiName"); //registerSpecialVar(Player,"currentitem","%this.tool[%this.currTool].uiName","setCurrentItem");
		registerSpecialVar(Player,"item1","%this.tool[0].uiName"); //registerSpecialVar(Player,"item1","%this.tool[0].uiName","setItem",0);
		registerSpecialVar(Player,"item2","%this.tool[1].uiName"); //registerSpecialVar(Player,"item2","%this.tool[1].uiName","setItem",1);
		registerSpecialVar(Player,"item3","%this.tool[2].uiName"); //registerSpecialVar(Player,"item3","%this.tool[2].uiName","setItem",2);
		registerSpecialVar(Player,"item4","%this.tool[3].uiName"); //registerSpecialVar(Player,"item4","%this.tool[3].uiName","setItem",3);
		registerSpecialVar(Player,"item5","%this.tool[4].uiName"); //registerSpecialVar(Player,"item5","%this.tool[4].uiName","setItem",4);
		registerSpecialVar(Player,"item6","%this.tool[5].uiName"); //registerSpecialVar(Player,"item6","%this.tool[5].uiName","setItem",5);
		registerSpecialVar(Player,"item7","%this.tool[6].uiName"); //registerSpecialVar(Player,"item7","%this.tool[6].uiName","setItem",6);
		registerSpecialVar(Player,"item8","%this.tool[7].uiName"); //registerSpecialVar(Player,"item8","%this.tool[7].uiName","setItem",7);
		registerSpecialVar(Player,"item9","%this.tool[8].uiName"); //registerSpecialVar(Player,"item9","%this.tool[8].uiName","setItem",8);
		registerSpecialVar(Player,"item10","%this.tool[9].uiName"); //registerSpecialVar(Player,"item10","%this.tool[9].uiName","setItem",9);
		registerSpecialVar(Player,"yaw","getYawB(%this)");
		registerSpecialVar(Player,"pitch","getPitchB(%this)");
		registerSpecialVar(Player,"posx","getWord(%this.getPosition(),0)");
		registerSpecialVar(Player,"posy","getWord(%this.getPosition(),1)");
		registerSpecialVar(Player,"posz","getWord(%this.getPosition(),2)");
		registerSpecialVar(Player,"pos","%this.getPosition()");
		registerSpecialVar(Player,"light","isObject(%this.light)");
		registerSpecialVar(Player,"isPassenger","(%this.getObjectMount().dataBlock.rideable)");
		registerSpecialVar(Player,"isDriver","(%this.getObjectMount().dataBlock.rideable && %this.getObjectMount().getControllingObject() == %this)");
		registerSpecialVar(Player,"veDamage","%this.getObjectMount() ? %this.getObjectMount().getDamageLevel() : 0");
		registerSpecialVar(Player,"veHealth","%this.getObjectMount() ? %this.getObjectMount().getDatablock().maxDamage - %this.getObjectMount().getDamageLevel() : 0");
		registerSpecialVar(Player,"veMaxHealth","%this.getObjectMount() ? %this.getObjectMount().getDatablock().maxDamage : 0");
		registerSpecialVar(Player,"veDatablock","%this.getObjectMount() ? %this.getObjectMount().getDatablock().uiName : 0");
		registerSpecialVar(fxDTSbrick,"ownername","%this.getGroup().name");
		registerSpecialVar(fxDTSbrick,"ownerbl_id","%this.getGroup().bl_id");
		registerSpecialVar(fxDTSbrick,"datablock","%this.getDatablock().uiName");
		registerSpecialVar(fxDTSbrick,"colorid","%this.getColorID()","setColor");
		registerSpecialVar(fxDTSbrick,"printcount","%this.printcount","setPrintCount");
		registerSpecialVar(fxDTSbrick,"printname","%this.getPrintName()","setPrintName");
		registerSpecialVar(fxDTSbrick,"name","%this.getBrickName()","setBrickName");
		registerSpecialVar(fxDTSbrick,"colorfxid","%this.getColorFXID()","setColorFX");
		registerSpecialVar(fxDTSbrick,"printid","%this.printid","setPrint");
		registerSpecialVar(fxDTSbrick,"shapefxid","%this.getShapeFXID()","setShapeFX");
		registerSpecialVar(fxDTSbrick,"posx","getWord(%this.getPosition(),0)");
		registerSpecialVar(fxDTSbrick,"posy","getWord(%this.getPosition(),1)");
		registerSpecialVar(fxDTSbrick,"posz","getWord(%this.getPosition(),2)");
		registerSpecialVar(fxDTSbrick,"pos","%this.getPosition()");
		registerSpecialVar(fxDTSbrick,"collision","%this.isColliding()");
		registerSpecialVar(fxDTSbrick,"rendering","%this.isRendering()");
		registerSpecialVar(fxDTSbrick,"raycasting","%this.isRaycasting()");
		registerSpecialVar(fxDTSbrick,"type","%this.getDataBlock().getID().category");
		registerSpecialVar(Vehicle,"drivername","%this.getDriverName()");
		registerSpecialVar(Vehicle,"driverbl_id","%this.getDriverBL_ID()");
		registerSpecialVar(Vehicle,"damage","%this.getDamageLevel()"); //registerSpecialVar(Vehicle,"damage","%this.getDamageLevel()","setDamage");
		registerSpecialVar(Vehicle,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()");//registerSpecialVar(Vehicle,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()","setHealth");
		registerSpecialVar(Vehicle,"maxhealth","%this.getDatablock().maxDamage");
		registerSpecialVar(Vehicle,"datablock","%this.getDatablock().uiName");
		registerSpecialVar(Vehicle,"velx","getWord(%this.getVelocity(),0)");
		registerSpecialVar(Vehicle,"vely","getWord(%this.getVelocity(),1)");
		registerSpecialVar(Vehicle,"velz","getWord(%this.getVelocity(),2)");
		registerSpecialVar(Vehicle,"vel","%this.getVelocity()"); //registerSpecialVar(Vehicle,"vel","%this.getVelocity()","setVelocity");
		registerSpecialVar(Vehicle,"speed","vectorDist(%this.getVelocity(),\"0 0 0\")");
		registerSpecialVar(Vehicle,"posx","getWord(%this.getPosition(),0)");
		registerSpecialVar(Vehicle,"posy","getWord(%this.getPosition(),1)");
		registerSpecialVar(Vehicle,"posz","getWord(%this.getPosition(),2)");
		registerSpecialVar(vehicle,"pos","%this.getPosition()");
		//registerSpecialVar(Vehicle,"pos","getWords(%this.getTransform(),0,2)"); //registerSpecialVar(Vehicle,"pos","getWords(%this.getTransform(),0,2)","setPosition");
		registerSpecialVar(Vehicle,"yaw","getWord(axisToEuler(getWords(%this.getTransform(),3,6)),2)","setYaw");
		registerSpecialVar(Vehicle,"pitch","getWord(axisToEuler(getWords(%this.getTransform(),3,6)),0)");
		registerSpecialVar(Vehicle,"roll","getWord(axisToEuler(getWords(%this.getTransform(),3,6)),1)");
		//registerSpecialVar(Bot,"yaw","getYawB(%this)");
		//registerSpecialVar(Bot,"pitch","getPitchB(%this)");
		//registerSpecialVar(Bot,"hat","$hat[%this.hat]");
		//registerSpecialVar(Bot,"accent","$accent[%this.accent]");
		//registerSpecialVar(Bot,"pack","$pack[%this.pack]");
		//registerSpecialVar(Bot,"secondPack","$secondpack[%this.secondpack]");
		//registerSpecialVar(Bot,"hip","$hip[%this.hip]");
		//registerSpecialVar(Bot,"rleg","$rleg[%this.rleg]");
		//registerSpecialVar(Bot,"rarm","$rarm[%this.rarm]");
		//registerSpecialVar(Bot,"lleg","$lleg[%this.lleg]");
		//registerSpecialVar(Bot,"larm","$larm[%this.larm]");
		//registerSpecialVar(Bot,"chest","$chest[%this.chest]");
		//registerSpecialVar(Bot,"decal","%this.decalName","setDecalName");
		//registerSpecialVar(Bot,"face","%this.faceName","setFaceName");
		//registerSpecialVar(Bot,"energy","%this.getEnergyLevel()","setEnergy");
		//registerSpecialVar(Bot,"damage","%this.getDamageLevel()","setDamage");
		//registerSpecialVar(Bot,"health","%this.getDatablock().maxDamage - %this.getDamageLevel()","setHealth");
		//registerSpecialVar(Bot,"velx","getWord(%this.getVelocity(),0)");
		//registerSpecialVar(Bot,"vely","getWord(%this.getVelocity(),1)");
		//registerSpecialVar(Bot,"velz","getWord(%this.getVelocity(),2)");
		//registerSpecialVar(Bot,"vel","getWords(%this.getVelocity(),0,2)","setVelocity");
		//registerSpecialVar(Bot,"posx","getWord(%this.getPosition(),0)");
		//registerSpecialVar(Bot,"posy","getWord(%this.getPosition(),1)");
		//registerSpecialVar(Bot,"posz","getWord(%this.getPosition(),2)");
		//registerSpecialVar(Bot,"pos","%this.getPosition()");
		//registerSpecialVar(Bot,"speed","vectorDist(%this.getVelocity(),\"0 0 0\")");
		//registerSpecialVar(Bot,"crouching","%this.VCETrigger3 $= \"\" ? 0 : %this.VCETrigger3");
		//registerSpecialVar(Bot,"jumping","%this.VCETrigger2 $= \"\" ? 0 : %this.VCETrigger2");
		//registerSpecialVar(Bot,"jetting","%this.VCETrigger4 $= \"\" ? 0 : %this.VCETrigger4");
		//registerSpecialVar(Bot,"firing","%this.VCETrigger0 $= \"\" ? 0 : %this.VCETrigger0");
		//registerSpecialVar(Bot,"sitting","%this.VCESitting $= \"\" ? 0 : %this.VCESitting");
		//registerSpecialVar(Bot,"datablock","%this.getDatablock().uiName");
		//registerSpecialVar(Bot,"weapon","%this.hLastWeapon.getName()","");
		//registerSpecialVar(Bot,"type","%this.hType","");
		//registerSpecialVar(Bot,"state","%this.hState","");
		//registerSpecialVar(Bot,"enabled","%this.hIsEnabled","");
		registerSpecialVar(Minigame,"lastmsg","%this.lastMessage");
		registerSpecialVar(Minigame,"membercount","%this.numMembers");
		registerSpecialVar("GLOBAL","date","getDate()");
		registerSpecialVar("GLOBAL","lastmsg","$VCE::Other::LastMessage");
		registerSpecialVar("GLOBAL","brickcount","$Server::BrickCount");
		registerSpecialVar("GLOBAL","time","getTime()");
		registerSpecialVar("GLOBAL","hour","getField(strReplace(getTime(),\":\",\"\t\"),0)");
		registerSpecialVar("GLOBAL","minute","getField(strReplace(getTime(),\":\",\"\t\"),1)");
		registerSpecialVar("GLOBAL","second","getField(strReplace(getTime(),\":\",\"\t\"),2)");
		registerSpecialVar("GLOBAL","simtime","getSimTime()");
		registerSpecialVar("GLOBAL","macintosh","isMacintosh()");
		registerSpecialVar("GLOBAL","windows","isWindows()");
		registerSpecialVar("GLOBAL","servername","$Pref::Server::Name");
		registerSpecialVar("GLOBAL","port","$Pref::Server::Port");
		registerSpecialVar("GLOBAL","maxplayercount","$Pref::Server::MaxPlayers");
		registerSpecialVar("GLOBAL","playercount","$server::playercount");
		registerSpecialVar("GLOBAL","pi","3.14159265");
		registerSpecialVar("GLOBAL","e","2.718281828");

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

		registerSpecialVar(GameConnection,"scanMaterialName","%this.scanMaterialName");
		registerSpecialVar(GameConnection,"scanMaterialAmount","$EOTW::Material[%this.bl_id, %this.scanMaterialName]");

		registerSpecialVar("GLOBAL","SA_Day","$EOTW::Day");
		registerSpecialVar("GLOBAL","SA_Time","$EOTW::Time");
	}
	//Set up prefs and various stuff
	$VCE::Server = 1;
	$VCE::Server::SavePath = "config/server/VCE/saves.txt";
	VCE_updateSaveFile();
}
//---
// Package
//---
exec("./server/package.cs");
//---
//Misc
//---
exec("./server/misc.cs");
//---
// Networking
//---
exec("./server/networking.cs");
//---
// Replacers
//---
exec("./server/replacers.cs");
//---
// Groups
//---
// Introduced in v5, variables are now stored in brick groups.
// Basically if you are working with variables, all variables that you modify will only exist in your brickgroup, so now one else can modify them.
//---
exec("./server/groups.cs");
//---
// Events
//---
//exec("./server/circuits.cs"); discontinued
exec("./server/inputs.cs");
exec("./server/outputs.cs");
//-
$VCE::InitSchedule = schedule(2000,0,"VCE_initServer");
