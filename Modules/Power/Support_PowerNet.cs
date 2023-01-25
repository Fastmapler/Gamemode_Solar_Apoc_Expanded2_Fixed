function fxDtsBrick::searchForConnections(%obj)
{
	%data = %obj.getDatablock();
	%bl_id = %obj.getGroup().bl_id;

	%types = "Source\tMachine";

	for (%typeCount = 0; %typeCount < getFieldCount(%types); %typeCount++)
	{
		%type = getField(%types, %typeCount);

		%obj.connections[%type] = "";
		%sourceSet = getPowerSet(%type, %bl_id);
		for (%i = 0; %i < %sourceSet.getCount(); %i++)
		{
			%target = %sourceSet.getObject(%i);

			if (vectorLen(%target.getPosition(), %obj.getPosition()) < 16)
			{
				%obj.connections[%type] = trim(%obj.connections[%type] TAB %target);

				if (getFieldCount(%obj.connections[%type]) > 9)
					break;
			}
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

function fxDTSBrick::changeBrickEnergy(%obj, %amount)
{
	%data = %obj.getDatablock();

	if (!%data.isPowered)
		return;
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