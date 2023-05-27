//---
//	@package VCE
//	@title Misc
//	@author Zack0wack0/www.zack0wack0.com
// @auther Monoblaster/46426
//	@time 9:20 PM 16/03/2011
//---
//Moddable special values
function getYawB(%this)
{
	%fv = %this.getForwardVector();
	%pi = 3.1415926535897932384626433;
	%x = mASin(getWord(%fv,0)) * 180 / %pi;
	return 180 - mFloatLength(%x / mAbs(%x),0) * (180 - (mACos(getWord(%fv,1)) * 180 / %pi));
}
function getPitchB(%this)
{
	%fv = %this.getEyeVector();
	%pi = 3.1415926535897932384626433;
	return mASin(getWord(%fv,2)) * 180 / %pi;
}

function VCE_Client_setFaceName(%client,%name)
{
	if(isObject(%client.player))
		%client.player.setFaceName(%name);
}
function VCE_Client_setDecalName(%client,%name)
{
	if(isObject(%client.player))
		%client.player.setDecalName(%name);
}
function VCE_Client_setClanPrefix(%client,%tag)
{
	%len = strLen(%tag);
	if(%len > 4)
		%tag = getSubStr(%tag,0,4);
	%client.clanPrefix = %tag;
}
function VCE_Client_setClanSuffix(%client,%tag)
{
	%len = strLen(%tag);
	if(%len > 4)
		%tag = getSubStr(%tag,0,4);
	%client.clanSuffix = %tag;
}
function VCE_Client_setScore(%client,%amount)
{
	%client.setScore(%amount);
}
function VCE_Player_setItem(%player,%name,%slot)
{	
	%item = $uiNameTable_Items[%name];
	if(%slot < 0 || %slot > %player.getDatablock().maxTools || !isObject(%item))
		return;
	%tool = %player.tool[%slot];
	%player.tool[%slot] = %item;
	messageClient(%player.client,'MsgItemPickup','',%slot,%item);
	if(%tool <= 0)
		%player.weaponCount++;
}
function VCE_Player_setCurrentItem(%player,%name)
{
	%item = $uiNameTable_Items[%name];
	if(!isObject(%item))
		return;
	%player.unMountImage(0);
	%player.mountImage(%item.image,0);
}
function VCE_Player_setEnergy(%player,%amount)
{
	%player.setEnergyLevel(%amount);
}
function VCE_Player_setVelocity(%player,%velocity)
{
	%player.setVelocity(%velocity);
}
function VCE_Player_setPosition(%player,%position)
{
	%player.setTransform(%position SPC getWords(%player.getTransform(),3,7));
}
function VCE_Brick_setColor(%brick,%color)
{
	%brick.setColor(%color);
}
function VCE_Brick_setShapeFX(%brick,%shape)
{
	%brick.setShapeFX(%shape);
}
function VCE_Brick_setColorFX(%brick,%colorfx)
{
	%brick.setColorFX(%colorfx);
}
function VCE_Brick_setPrintCount(%brick,%count)
{
	%brick.setPrintCount(%count);
}
function VCE_Brick_setPrint(%brick,%id)
{
	%brick.setPrint(%id);
}
function VCE_Brick_setBrickName(%brick,%name)
{
	%brick.setNTObjectName(%name);
}
function VCE_Brick_setPrintName(%brick,%name)
{
	if($printNameTable[%name] $= "")
		return;
	%brick.setPrint($printNameTable[%name]);
}
function VCE_Vehicle_setPosition(%vehicle,%position)
{
	%vehicle.setTransform(%position SPC getWords(%player.getTransform(),3,7));
}
function VCE_Vehicle_setVelocity(%vehicle,%velocity)
{
	%vehicle.setVelocity(%velocity);
}
//Shitfuck
function fxDtsBrick::getBrickName(%brick)
{
	%objname = %brick.getName();
	if(strLen(%objname) > 1)
	{
		return getSubStr(%objname,1,strLen(%objname)-1);
	}
	return;
}
function Vehicle::getDriverName(%vehicle)
{
	%driver = %vehicle.getMountObject(0);
	if(isObject(%driver.client))
		return %driver.client.getPlayerName();
	return;
}
function Vehicle::getDriverBL_ID(%vehicle)
{
	%driver = %vehicle.getMountObject(0);
	if(isObject(%driver.client))
		return %driver.client.bl_id;
	return;
}
function fxDtsBrick::getPrintName(%this)
{
	if(%this.getDataBlock().subCategory $= "Prints")
	{
		%texture = getPrintTexture(%this.getPrintID());
		%path = filePath(%texture);
		%underPos = strPos(%path,"_");
		%name = getSubStr(%path,%underPos + 1,strPos(%path,"_",14) - 14) @ "/" @ fileBase(%texture);
		if($printNameTable[%name] !$= "")
			return %name;
	}
}
function mPercent(%num,%total)
{
	return (%num / %total) * 100;
}
function isInt(%string)
{
	return %string $= mFloatLength(%string,0);
}
function getDate()
{
        return getWord(getDateTime(),0);
}
function getTime()
{
        return getWord(getDateTime(),1);
}
function removeWords(%string, %start, %end){
	for(%i = %end; %i >= %start; %i--){
		%string = removeWord(%string, %i);
	}
	return %string;
}
function countChar(%string, %char){
	%c = 0;
	for(%i = 0; %i < strLen(%string); %i++){
		if(%char $= getSubStr(%string ,%i, 1))
			%c++;
	}
	return %c;
}
function VCE_player_setHealth(%obj,%amount)
{
	%obj.VCE_setDamage(%obj.getDataBlock().maxDamage - %amount);
}
function VCE_Player_setDamage(%obj,%amount)
{
	%obj.VCE_setDamage(%amount);
}
function VCE_Vehicle_setHealth(%obj,%amount)
{
	%obj.VCE_setDamage(%obj.getDataBlock().maxDamage - %amount);
}
function VCE_Vehicle_setDamage(%obj,%amount)
{
	%obj.VCE_setDamage(%amount);
}
function VCE_Bot_setHealth(%obj,%amount)
{
	%obj.VCE_setDamage(%obj.getDataBlock().maxDamage - %amount);
}
function VCE_Bot_setDamage(%obj,%amount)
{
	%obj.VCE_setDamage(%amount);
}
function ShapeBase::VCE_setDamage(%obj, %amount){
	if(!isObject(%obj))
		return;
	if (%obj.getDamagePercent () >= 1)
		return;
	if (%ammount <= 0)
		%obj.Damage (%obj, %obj.getPosition(), %obj.getDataBlock().maxDamage, $DamageType::Default);
	else 
	{
		%damageLevel = %obj.getDataBlock().maxDamage - %health;
		if (%damageLevel < 0)
			%damageLevel = 0;
		%obj.setDamageLevel (%damageLevel);
	}
}