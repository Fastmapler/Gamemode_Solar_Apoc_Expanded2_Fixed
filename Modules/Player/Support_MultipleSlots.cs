//Hacky way for custom max inventory
function ItemData::onPickup (%this, %obj, %user, %amount)
{
	if (%obj.canPickup == 0)
	{
		return;
	}
	%player = %user;
	%client = %player.client;
	%data = %player.getDataBlock ();
	if (!isObject (%client))
	{
		return;
	}
	%mg = %client.miniGame;
	%canUse = 1;
	if (miniGameCanUse (%player, %obj) == 1)
	{
		%canUse = 1;
	}
	if (miniGameCanUse (%player, %obj) == 0)
	{
		%canUse = 0;
	}
	if (!%canUse)
	{
		if (isObject (%obj.spawnBrick))
		{
			%ownerName = %obj.spawnBrick.getGroup ().name;
		}
		%msg = %ownerName @ " does not trust you enough to use this item.";
		if ($lastError == $LastError::Trust)
		{
			%msg = %ownerName @ " does not trust you enough to use this item.";
		}
		else if ($lastError == $LastError::MiniGameDifferent)
		{
			if (isObject (%client.miniGame))
			{
				%msg = "This item is not part of the mini-game.";
			}
			else 
			{
				%msg = "This item is part of a mini-game.";
			}
		}
		else if ($lastError == $LastError::MiniGameNotYours)
		{
			%msg = "You do not own this item.";
		}
		else if ($lastError == $LastError::NotInMiniGame)
		{
			%msg = "This item is not part of the mini-game.";
		}
		commandToClient (%client, 'CenterPrint', %msg, 1);
		return;
	}

	if (%this.pickupFunc !$= "")
	{
		if (call(%this.pickupFunc, %this, %player))
		{
			if (%obj.isStatic())
				%obj.Respawn();
			else 
				%obj.delete();
		}
		
		return;
	}
		
	%freeslot = -1;
	%i = 0;
	while (%i < %client.GetMaxInvSlots())
	{
		if (%player.tool[%i] == 0)
		{
			%freeslot = %i;
			break;
		}
		%i += 1;
	}
	if (%freeslot != -1)
	{
		if (%obj.isStatic ())
		{
			%obj.Respawn ();
		}
		else 
		{
			%obj.delete ();
		}
		%player.tool[%freeslot] = %this;
		if (%user.client)
		{
			messageClient (%user.client, 'MsgItemPickup', '', %freeslot, %this.getId ());
		}
		return 1;
	}
}

function Armor::onCollision (%this, %obj, %col, %vec, %speed)
{
	if (%obj.getState () $= "Dead")
	{
		return;
	}
	if (%col.getDamagePercent () >= 1)
	{
		return;
	}
	%colClassName = %col.getClassName ();
	if (%colClassName $= "Item")
	{
		%client = %obj.client;
		%colData = %col.getDataBlock ();
		%i = 0;
		while (%i < %client.GetMaxInvSlots())
		{
			if (%obj.tool[%i] == %colData)
			{
				return;
			}
			%i += 1;
		}
		%obj.pickup (%col);
	}
	else if (%colClassName $= "Player" || %colClassName $= "AIPlayer")
	{
		if (%col.getDataBlock ().canRide && %this.rideAble && %this.nummountpoints > 0)
		{
			if (getSimTime () - %col.lastMountTime <= $Game::MinMountTime)
			{
				return;
			}
			%colZpos = getWord (%col.getPosition (), 2);
			%objZpos = getWord (%obj.getPosition (), 2);
			if (%colZpos <= %objZpos + 0.2)
			{
				return;
			}
			%canUse = 0;
			if (isObject (%obj.spawnBrick))
			{
				%vehicleOwner = findClientByBL_ID (%obj.spawnBrick.getGroup ().bl_id);
			}
			if (isObject (%vehicleOwner))
			{
				if (getTrustLevel (%col, %obj) >= $TrustLevel::RideVehicle)
				{
					%canUse = 1;
				}
			}
			else 
			{
				%canUse = 1;
			}
			if (miniGameCanUse (%col, %obj) == 1)
			{
				%canUse = 1;
			}
			if (miniGameCanUse (%col, %obj) == 0)
			{
				%canUse = 0;
			}
			if (!%canUse)
			{
				if (!isObject (%obj.spawnBrick))
				{
					return;
				}
				%ownerName = %obj.spawnBrick.getGroup ().name;
				%msg = %ownerName @ " does not trust you enough to do that";
				if ($lastError == $LastError::Trust)
				{
					%msg = %ownerName @ " does not trust you enough to ride.";
				}
				else if ($lastError == $LastError::MiniGameDifferent)
				{
					if (isObject (%col.client.miniGame))
					{
						%msg = "This vehicle is not part of the mini-game.";
					}
					else 
					{
						%msg = "This vehicle is part of a mini-game.";
					}
				}
				else if ($lastError == $LastError::MiniGameNotYours)
				{
					%msg = "You do not own this vehicle.";
				}
				else if ($lastError == $LastError::NotInMiniGame)
				{
					%msg = "This vehicle is not part of the mini-game.";
				}
				commandToClient (%col.client, 'CenterPrint', %msg, 1);
				return;
			}
			for (%i = 0; %i < %this.nummountpoints; %i += 1)
			{
				if (%this.mountNode[%i] $= "")
				{
					%mountNode = %i;
				}
				else 
				{
					%mountNode = %this.mountNode[%i];
				}
				%blockingObj = %obj.getMountNodeObject (%mountNode);
				if (isObject (%blockingObj))
				{
					if (!%blockingObj.getDataBlock ().rideAble)
					{
						continue;
					}
					if (%blockingObj.getMountedObject (0))
					{
						continue;
					}
					%blockingObj.mountObject (%col, 0);
					if (%blockingObj.getControllingClient () == 0)
					{
						%col.setControlObject (%blockingObj);
					}
					%col.setTransform ("0 0 0 0 0 1 0");
					%col.setActionThread (root, 0);
					continue;
				}
				%obj.mountObject (%col, %mountNode);
				%col.setActionThread (root, 0);
				if (%i == 0)
				{
					if (%obj.isHoleBot)
					{
						if (%obj.controlOnMount)
						{
							%col.setControlObject (%obj);
						}
					}
					else if (%obj.getControllingClient () == 0)
					{
						%col.setControlObject (%obj);
					}
					if (isObject (%obj.spawnBrick))
					{
						%obj.lastControllingClient = %col;
					}
				}
				break;
			}
		}
	}
}

function Player::RemoveTool(%obj, %currSlot)
{
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	if (isObject(%client = %obj.client))
	{
		messageClient(%client, 'MsgItemPickup', '', %currSlot, 0);
		serverCmdUnUseTool(%client);
	}
	else
		%obj.unMountImage(0);
	
}

function GameConnection::GetMaxInvSlots(%client)
{
	if (%client.MaxInvSlots $= "")
		return 5;

	return %client.MaxInvSlots;
}

function GameConnection::SetMaxInvSlots(%client, %amt)
{
	%client.MaxInvSlots = %amt;

	commandToClient(%client, 'PlayGui_CreateToolHud', %client.GetMaxInvSlots());
}

//Setting the support so we can see all Slots
package InventorySlotAdjustment
{
	function Armor::onNewDatablock(%data,%this)
	{
		Parent::onNewDatablock(%data,%this);
		if(isObject(%this.client) && %data.maxTools != %this.client.lastMaxTools)
		{
			%this.client.lastMaxTools = %data.maxTools;
			commandToClient(%this.client,'PlayGui_CreateToolHud',%data.maxTools);
			for(%i=0;%i<%data.maxTools;%i++)
			{
				if(isObject(%this.tool[%i]))
					messageClient(%this.client,'MsgItemPickup',"",%i,%this.tool[%i].getID(),1);
				else
					messageClient(%this.client,'MsgItemPickup',"",%i,0,1);
			}
		}
	}
	function GameConnection::setControlObject(%this,%obj)
	{
		Parent::setControlObject(%this,%obj);
		if(%obj == %this.player && %obj.getDatablock().maxTools != %this.lastMaxTools)
		{
			%this.lastMaxTools = %obj.getDatablock().maxTools;
			commandToClient(%this,'PlayGui_CreateToolHud',%obj.getDatablock().maxTools);
		}
	}
	function Player::changeDatablock(%this,%data,%client)
	{
		if(%data != %this.getDatablock() && %data.maxTools != %this.client.lastMaxTools)
		{
			%this.client.lastMaxTools = %data.maxTools;
			commandToClient(%this.client,'PlayGui_CreateToolHud',%data.maxTools);
		}
		Parent::changeDatablock(%this,%data,%client);
	}
	function GameConnection::createPlayer(%cl, %trans)
	{
		parent::createPlayer(%cl, %trans);
		if(isObject(%player = %cl.player))
		{
			%data = %player.getDatablock();
			
			%cl.lastMaxTools = %data.maxTools;
			%count = %data.maxTools;
			if (%cl.MaxInvSlots !$= "")
				%count = %cl.GetMaxInvSlots();
			commandToClient(%cl,'PlayGui_CreateToolHud',%count);
			for(%i=0;%i<%count;%i++)
			{
				if(isObject(%player.tool[%i]))
					messageClient(%cl,'MsgItemPickup',"",%i,%player.tool[%i].getID(),1);
				else
					messageClient(%cl,'MsgItemPickup',"",%i,0,1);
			}
		}
	}
};
activatePackage(InventorySlotAdjustment);