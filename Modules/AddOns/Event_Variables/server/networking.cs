//---
//	@package VCE
//	@title Networking
//	@author Zack0Wack0/www.zack0wack0.com
//	@time 4:09 PM 15/03/2011
//---
function serverCmdVCE_onLink(%client,%brick,%index)
{
	if(%client.varLink[%brick] == true && isObject(%brick) && %brick.varLink[%client] !$= "" && getFieldCount(%brick.varLink[%client])-1 >= %index)
	{
		%data = strReplace(getField(%brick.varLink[%client],%index),"=","\t");
		%brick.getGroup().vargroup.setVariable("Client",getField(%data,0),getField(%data,1),%client);
		%brick.varLink[%client] = "";
		%client.varLink[%brick] = "";
		$inputTarget_Self = %brick;
		$inputTarget_Player = %client.player;
		$inputTarget_Bot = %brick.vehicle;
		$inputTarget_Client = %client;
		$inputTarget_Minigame = getMinigameFromObject(%client);
		%brick.processInputEvent("onVariableUpdate",%client);
	}
}
function serverCmdVCE_Handshake(%client)
{
	%client.hasVCE = 1;
}