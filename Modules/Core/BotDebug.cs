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

function Player::resetTestVal(%this)
{
	%this.numCollisions = 0;
}

package testTime
{
	function Armor::onStuck(%this, %obj, %a, %b)
	{
		%start = getRealTime();
		%p = parent::onStuck(%this, %obj, %a, %b);
		testRealTime(%start, "onStuck" SPC %obj.getShapeName());

		return %p;
	}
	function Player::inLocation(%this, %loc, %a, %b)
	{
		%start = getRealTime();
		%p = parent::inLocation(%this, %loc, %a, %b);
		testRealTime(%start, "inLocation" SPC %this);

		return %p;
	}
	function GameConnection::setMusic(%this, %music, %vol, %a, %b)
	{
		%start = getRealTime();
		%p = parent::setMusic(%this, %music, %vol, %a, %b);
		testRealTime(%start, "setMusic" SPC %this);

		return %p;
	}
	function fxDTSBrick::onPlayerTouch(%this, %player)
	{
		%start = getRealTime();
		%p = parent::onPlayerTouch(%this, %player);
		testRealTime(%start, "onPlayerTouch");

		if(stripos(firstWord(%player.getPosition()), "nan") != -1)
		{
			echo("REMOVED BAD BOT?" SPC %player SPC %player.getShapeName());
			echo("POSITION: " @ %player.getPosition() SPC "spawnned at: " @ %player.spawnPosition);
			%player.delete();
			return;
		}
		%player.numCollisions++;
		if(%player.numCollisions > 30)
		{
			echo("HUH? " @ %player.getShapeName() SPC %player SPC %player.numCollisions);
			echo("DELETING:" SPC "SCALE: "@ %player.getScale() @" WORLDBOX SCALE: "@ vectorDist(getWords(%player.getWorldBox(), 0, 2), getWords(%player.getWorldBox(), 3, 6)));

			echo("POSITION: " @ %player.getPosition() SPC "spawnned at: " @ %player.spawnPosition);
			%player.delete();
			return;
		}

		if(!isEventPending(%player.testvalSchedule))
			%player.testvalSchedule = %player.schedule(1000, resetTestVal);

		return %p;
	}

	function WeaponImage::onFire (%this, %obj, %slot)
	{
		%start = getRealTime();
		%p = parent::onFire (%this, %obj, %slot);
		testRealTime(%start, "onFire" SPC %obj.getShapeName());

		return %p;
	}

	function Armor::onReachDestination (%this, %obj)
	{
		%start = getRealTime();
		%p = parent::onReachDestination (%this, %obj);
		testRealTime(%start, "onReachDestination" SPC %obj.getShapeName());

		return %p;
	}
};
activatePackage(testTime);