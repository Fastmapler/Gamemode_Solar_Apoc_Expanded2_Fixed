function testRealTime(%start, %from)
{
	%timeTaken = getRealTime() - %start;
	if(%timeTaken > 100)
	{
		echo("LARGE LAG (" @ %timeTaken @ "ms) FROM: "@ %from);
		backtrace();
	}

	$TRT_numCalls[firstWord(%from)]++;
}

function Player::safeInstantRespawn(%player)
{
	if (!isObject(%client = %player.client))
		return;
	
	for (%i = 0; %i < %client.GetMaxInvSlots(); %i++)
		if (isObject(%tool = %player.tool[%i]))
			%client.respawnInvent = trim(%client.respawnInvent TAB %tool);

	%player.instantRespawn();
}

package testTime
{
	function GameConnection::createPlayer(%client, %trans)
	{		
		Parent::createPlayer(%client, %trans);

		if (isObject(%player = %client.player) && %client.respawnInvent !$= "")
		{
			for (%i = 0; %i < getFieldCount(%client.respawnInvent); %i++)
			{
				%item = new Item()
				{
					datablock = getField(%client.respawnInvent, %i);
					static    = "0";
					position  = vectorAdd(%player.getPosition(),"0 0 1");
					craftedItem = true;
				};
			}

			%client.chatMessage("Your previously held items have been dropped onto the ground.");
			%client.respawnInvent = "";
		}
	}
	function Armor::onStuck(%this, %obj, %a, %b)
	{
		%start = getRealTime();
		%p = parent::onStuck(%this, %obj, %a, %b);
		//testRealTime(%start, "onStuck" SPC %obj.getShapeName());

		return %p;
	}
	function fxDTSBrick::onPlayerTouch(%this, %player)
	{
		if (!isObject(%player))
			return;

		%start = getRealTime();
		%p = parent::onPlayerTouch(%this, %player);
		//testRealTime(%start, "onPlayerTouch");

		if(stripos(firstWord(%player.getPosition()), "nan") != -1)
		{
			echo("REMOVED BAD BOT? " @ %player.getShapeName() SPC %player SPC %player.numCollisions);
			echo("POSITION: " @ %player.getPosition() SPC "spawned at: " @ %player.spawnPosition);

			if (isObject(%player.client))
				%player.safeInstantRespawn();
			else
				%player.delete();

			
			return %p;
		}

		if(%player.numCollisions++ > 30)
		{
			echo("HUH? " @ %player.getShapeName() SPC %player SPC %player.numCollisions);
			echo("DELETING:" SPC "SCALE: "@ %player.getScale() @" WORLDBOX SCALE: "@ vectorDist(getWords(%player.getWorldBox(), 0, 2), getWords(%player.getWorldBox(), 3, 6)));
			echo("POSITION: " @ %player.getPosition() SPC "spawned at: " @ %player.spawnPosition);

			if (isObject(%player.client))
				%player.safeInstantRespawn();
			else
				%player.delete();

			return %p;
		}

		%player.numCollisions = 0;
		%player.lastGoodPosition = %player.getTransform();

		return %p;
	}
};
activatePackage(testTime);