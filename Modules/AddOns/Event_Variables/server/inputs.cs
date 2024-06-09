$VCE_extendTargetList[0] = "onActivate" TAB ", %player, %client, %pos, %vec";
$VCE_extendTargetList[1] = "onBlownUp" TAB ",%client, %player";
$VCE_extendTargetList[2] = "onBotActivated" TAB ",%client";
$VCE_extendTargetList[3] = "onBotDeath" TAB ",%client";
$VCE_extendTargetList[4] = "onBotTouch" TAB ",%client";
$VCE_extendTargetList[5] = "onDoorClose" TAB ",%client";
$VCE_extendTargetList[6] = "onDoorOpen" TAB ",%client";
$VCE_extendTargetList[7] = "onKeyMatch" TAB ",%client";
$VCE_extendTargetList[8] = "onKeyMismatch" TAB ",%client";
$VCE_extendTargetList[9] = "onMinigameReset" TAB ",%client";
$VCE_extendTargetList[10] = "onPlayerTouch" TAB ",%player";
$VCE_extendTargetList[11] = "onPrintCountOverFlow" TAB ",%client";
$VCE_extendTargetList[12] = "onPrintCountUnderFlow" TAB ",%client";
$VCE_extendTargetList[13] = "onProjectileHit"  TAB ", %projectile,%client";
$VCE_extendTargetList[14] = "onRelay" TAB ",%client";
$VCE_extendTargetList[15] = "onRespawn" TAB ",%client, %player";
$VCE_extendTargetList[16] = "onTouchDown" TAB ",%client";

$VCE_targets[0] = "Self fxDtsBrick";
$VCE_isDefaultTarget[getWord($VCE_targets[0],0)] = true;
$VCE_targets[1] = "Player Player";
$VCE_isDefaultTarget[getWord($VCE_targets[1],0)] = true;
$VCE_targets[2] = "Client GameConnection";
$VCE_isDefaultTarget[getWord($VCE_targets[2],0)] = true;
$VCE_targets[3] = "Bot Bot";
$VCE_isDefaultTarget[getWord($VCE_targets[3],0)] = true;
$VCE_targets[4] = "Vehicle Vehicle";
$VCE_isDefaultTarget[getWord($VCE_targets[4],0)] = true;
$VCE_targets[5] = "Minigame Minigame";
$VCE_isDefaultTarget[getWord($VCE_targets[5],0)] = true;

function extendTargetList(){
	%c = 0;
	deactivatePackage(VCE_NewInputs);
	while((%name = getField($VCE_extendTargetList[%c], 0)) !$= ""){
		//generate target list adding targets that aren't in default list
		//we don't need to do anything with the global variable target as that's done through parenting
		if(isFunction(fxDTSBrick, %name)){
			%eventIdx = inputEvent_GetInputEventIdx(%name);
			%targetString = "";
			%t = 0;
			while((%targetClass = inputEvent_GetTargetClass(fxDTSBrick, %eventIdx, %t)) !$= ""){
				%targetName = inputEvent_GetTargetName(fxDTSBrick, %eventIdx, %t);
				if(!$VCE_isDefaultTarget[%targetName]){
					%targetString = %targetString @ %targetName SPC %targetClass @ "\t";
				}
				%t++;
			}
			%t = 0;
			while((%target = $VCE_targets[%t]) !$= ""){
				%targetString = %targetString @ $VCE_targets[%t] @ "\t";
				%t++;
			}
			%args = getField($VCE_extendTargetList[%c], 1);
			registerInputEvent(fxDtsBrick,%name,%targetString);
			eval("package VCE_NewInputs{function fxDTSBrick::" @ %name @"(%brick" @ %args @ "){$inputTarget_Self = %brick;$inputTarget_Player = %client.player;$inputTarget_Bot = %brick.hBot;$inputTarget_Client = %client;$inputTarget_Vehicle = %brick.vehicle;$inputTarget_Minigame = getMinigameFromObject(%client);Parent::" @ %name @ "(%brick"@ %args @");}};");
		}
		%c++;
	}
	activatePackage(VCE_NewInputs);
}
function fxDtsBrick::onVariableUpdate(%brick,%client)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Vehicle = %brick.vehicle;
	$inputTarget_Minigame = getMinigameFromObject(%client);
}
function fxDtsBrick::onVariableTrue(%brick,%client,%start,%end)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Vehicle = %brick.vehicle;
	$inputTarget_Minigame = getMinigameFromObject(%client);
}
function fxDtsBrick::onVariableFalse(%brick,%client, %start, %end)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Vehicle = %brick.vehicle;
	$inputTarget_Minigame = getMinigameFromObject(%client);
}
function fxDtsBrick::onVariableFunction(%brick,%client, %start, %end)
{
	$inputTarget_Self = %brick;
	$inputTarget_Player = %client.player;
	$inputTarget_Bot = %brick.vehicle;
	$inputTarget_Client = %client;
	$inputTarget_Vehicle = %brick.vehicle;
	$inputTarget_Minigame = getMinigameFromObject(%client);
}