//Support_AmmoGuns.cs
//Items with the field "maxAmmo" set to a positive value will store an ammo count when thrown away

//If a player is using an item with an ammo count, it is able to reload and it is at less than max ammo,
//it will set image ammo to 'off' to trigger reload states (WeaponImage.item must be set correctly)

//The weapon firing sequence should decrement ammo in the firing method (WeaponImage::onFire or otherwise) and set image ammo to trigger reloads
//The state system is individual for each weapon so this is not performed automatically
//
//function WeaponImage::onFire(%this,%obj,%slot)
//{
//	... other weapon firing code or Parent::onFire(%this,%obj,%slot) ...
//
//	%obj.toolAmmo[%obj.currTool]]--;
//}


if($SpaceMods::Server::AmmoGunsVersion > 2)
   return;

$SpaceMods::Server::AmmoGunsVersion = 2;

package AmmoGuns2
{
	function Player::pickup(%this,%item)
	{
		%data = %item.dataBlock;
		%ammo = %item.weaponAmmoLoaded;
		%val = Parent::pickup(%this,%item);
		if(%val == 1 && %data.maxAmmo > 0 && isObject(%this.client))
		{
			%slot = -1;
			for(%i=0;%i<%this.dataBlock.maxTools;%i++)
			{
				if(isObject(%this.tool[%i]) && %this.tool[%i].getID() == %data.getID() && %this.toolAmmo[%i] $= "")
				{
					%slot = %i;
					break;
				}
			}
			
			if(%slot == -1)
				return %val;
			
			if(%ammo $= "")
			{
				%this.toolAmmo[%slot] = %data.maxAmmo;
			}
			else
			{
				%this.toolAmmo[%slot] = %ammo;
			}
		}
		return %val;
	}
	
	function ItemData::onAdd(%this,%obj)
	{
		if($weaponAmmoLoaded !$= "")
		{
			%obj.weaponAmmoLoaded = $weaponAmmoLoaded;
			$weaponAmmoLoaded = "";
		}
		Parent::onAdd(%this,%obj);
	}
	
	function WeaponImage::onMount(%this,%obj,%slot)
	{	
		Parent::onMount(%this,%obj,%slot);
		
		//If using one of the "force mount item" events, set the ammo to maximum
		if(%this.item.maxAmmo >= 0 && (%obj.currTool == -1 || %obj.toolAmmo[%obj.currTool] $= ""))
		{
			%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
		}
	}
	
	//Check if the gun needs to reload. Use this to trigger state changes.
	function WeaponImage::onLoadCheck(%this,%obj,%slot)
	{
		if(%obj.toolAmmo[%obj.currTool] <= 0 && %this.item.maxAmmo > 0 && %obj.getState() !$= "Dead")
			%obj.setImageAmmo(%slot,0);
		else
			%obj.setImageAmmo(%slot,1);
	}
	
	//Use this state in single-ammo reload weapons e.g. Shotgun, Scattergun
	function WeaponImage::onReloadCheck(%this,%obj,%slot)
	{
		if(%obj.toolAmmo[%obj.currTool] < %this.item.maxAmmo && %this.item.maxAmmo > 0 && %obj.getState() !$= "Dead")
			%obj.setImageAmmo(%slot,0);
		else
			%obj.setImageAmmo(%slot,1);
	}
	
	//Example, you may wish to have weapons load all at once
	function WeaponImage::onReloaded(%this,%obj,%slot)
	{
		%obj.toolAmmo[%obj.currTool]++;
	}
	
	function servercmdDropTool(%client,%slot)
	{
		if(!isObject(%client.player))
			return Parent::servercmdDropTool(%client,%slot);
		
		if(!isObject(%client.player.tool[%slot]) || %client.player.tool[%slot].maxAmmo <= 0)
			return Parent::servercmdDropTool(%client,%slot);
		
		$weaponAmmoLoaded = %client.player.toolAmmo[%client.player.currTool];
		%client.player.toolAmmo[%client.player.currTool] = "";
		return Parent::servercmdDropTool(%client,%slot);
	}
	
	function servercmdLight(%client)
	{
		if(isObject(%client.player) && isObject(%client.player.getMountedImage(0)))
		{
			%p = %client.player;
			%im = %p.getMountedImage(0);
			if(%im.item.maxAmmo > 0 && %im.item.canReload == 1 && %p.toolAmmo[%p.currTool] < %im.item.maxAmmo)
			{
				if(%p.getImageState(0) $= "Ready")
					%p.setImageAmmo(0,0);
				return;
			}
		}
		
		Parent::servercmdLight(%client);
	}
};

function runAmmoGuns()
{
   if(isPackage(AmmoGuns))
      deactivatePackage(AmmoGuns);
   activatePackage(AmmoGuns2);
}
schedule(1,0,runAmmoGuns);