function fxDtsBrick::searchForConnections(%obj, %type)
{
	%data = %obj.getDatablock();
	%bl_id = %obj.getGroup().bl_id;

	%maxConnect = %data.maxConnect;
	if (%maxConnect < 1)
		%maxConnect = 9;

	%maxRange = %data.maxRange;
	if (%maxRange < 1)
		%maxRange = 16;

	%obj.connections[%type] = "";
	%sourceSet = getPowerSet(%type, %bl_id);
	for (%i = 0; %i < %sourceSet.getCount(); %i++)
	{
		%target = %sourceSet.getObject(%i);

		if (vectorDist(%target.getPosition(), %obj.getPosition()) <= %maxRange)
		{
			%obj.connections[%type] = trim(%obj.connections[%type] TAB %target);

			if (getFieldCount(%obj.connections[%type]) >= %maxConnect)
				break;
		}
	}
}

function getPowerSet(%type, %bl_id)
{
	%data = (%type @ "Group_" @ %bl_id);

	if (!isObject(%data))
		%data = new SimSet(%data);

	return %data.getID();
}

function fxDtsBrick::getPowerSet(%obj)
{
	%data = %obj.getDatablock();
	%bl_id = %obj.getGroup().bl_id;
	return getPowerSet(%data.powerType, %bl_id);
}

function fxDtsBrick::getPower(%obj)
{
	return 0 + %obj.powerBuffer;
}

function fxDtsBrick::getMaxPower(%obj)
{
	%data = %obj.getDatablock();

	%max = %data.maxBuffer;
	if (%data.maxBuffer < 1)
		%max = 128;

	return %max;
}

function fxDTSBrick::changeBrickPower(%obj, %amount)
{
	if (%amount == 0)
		return 0;

	%max = %obj.getMaxPower();

	%initBuffer = %obj.getPower();
	%obj.powerBuffer = mClamp(mRound(%initBuffer + %amount), 0, %max);

	return %obj.getPower() - %initBuffer;
}

function fxDTSBrick::transferBrickPower(%obj, %amount, %target)
{
	%initTargetBuffer = %target.getPower();

	%sourceDifference = %obj.changeBrickPower(-1 * %amount);
	%sourceDifference += %target.changeBrickPower(-1 * %sourceDifference);
	%obj.changeBrickPower(%sourceDifference); //Refund leftover power

	return %target.getPower() - %initTargetBuffer;
}

function fxDtsBrick::onTick(%obj)
{
	%obj.getDatablock().onTick(%obj);
}

function fxDtsBrick::LoadPowerData(%obj)
{
	%data = %obj.getDatablock();
	%bl_id = %obj.getGroup().bl_id;

	if (!%data.isPowered || %bl_id < 1)
		return;

	%set = getPowerSet(%data.powerType, %bl_id);
	%set.add(%obj);
	talk(%set);
}

function SimSet::TickMembers(%obj)
{
	for (%i = 0; %i < %obj.getCount(); %i++)
		%obj.getObject(%i).onTick();
}

package EOTW_Power {
	function fxDtsBrick::onPlant(%obj, %b)
	{
		parent::onPlant(%obj, %b);
		
		%obj.LoadPowerData();
	}

	function fxDtsBrick::onLoadPlant(%obj, %b)
	{
		parent::onLoadPlant(%obj, %b);
		
		%obj.LoadPowerData(%obj);
	}

	function fxDTSBrick::onAdd(%obj)
	{
		Parent::onAdd(%obj);
	}
};
activatePackage("EOTW_Power");