function fxDtsBrick::searchForConnections(%obj, %type)
{
	%bl_id = %obj.getGroup().bl_id;

	%maxConnect = %obj.getMaxConnect();
	%maxRange = %obj.getMaxRange();

	%obj.connections[%type] = "";
	%sourceSet = getPowerSet(%type, %bl_id);
	for (%i = 0; %i < %sourceSet.getCount(); %i++)
	{
		%target = %sourceSet.getObject(%i);
		%dist = vectorDist(%target.getPosition(), %obj.getPosition());

		if (%target.getID() != %obj.getID() && %dist <= getMin(%maxRange, %target.getMaxRange()))
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
	if (%max < 1)
		%max = 128;

	return %max;
}

function fxDtsBrick::getMaxRange(%obj)
{
	%data = %obj.getDatablock();

	%max = %data.maxRange;
	if (%max < 1)
		%max = 16;

	return %max;
}

function fxDtsBrick::getMaxConnect(%obj)
{
	%data = %obj.getDatablock();

	%max = %data.maxConnect;
	if (%max < 1)
		%max = 8;

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
	%obj.changeBrickPower(-1 * %sourceDifference); //Refund leftover power

	return %target.getPower() - %initTargetBuffer;
}

function fxDtsBrick::attemptPowerDraw(%obj, %amount)
{
    %drawLeft = %amount;
    %set = %obj.connections["Battery"];
    for (%i = 0; %i < getFieldCount(%set); %i++)
    {
        %source = getField(%set, %i);

        if (!isObject(%source))
        {
            %obj.searchForConnections("Battery");
            continue;
        }

        %drawLeft += %source.changeBrickPower(-1 * %drawLeft);

        if (%drawLeft < 1)
            return true;
    }

    return false;
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
	
	%obj.updateConnections();

	initContainerRadiusSearch(%obj.getPosition(), %obj.getMaxRange(), $TypeMasks::fxBrickAlwaysObjectType);
    while(isObject(%hit = containerSearchNext()))
		if (%hit.getDatablock().isPowered && %bl_id == %hit.getGroup().bl_id)
			%hit.updateConnections();
}

function fxDtsBrick::updateConnections(%obj)
{
	%obj.searchForConnections("Source");
	%obj.searchForConnections("Battery");
	%obj.searchForConnections("Machine");
}

function SimSet::TickMembers(%obj)
{
	if (%obj.getCount() == 0)
		return;
		
	%obj.pushFrontToBack();
	for (%i = 0; %i < %obj.getCount(); %i++)
		%obj.getObject(%i).onTick();
}

function GameConnection::TickPowerGroups(%client) {
	%bl_id = %client.bl_id;

	getPowerSet("Source", %bl_id).TickMembers();
	getPowerSet("Battery", %bl_id).TickMembers();
	getPowerSet("Machine", %bl_id).TickMembers();
	getPowerSet("Logistic", %bl_id).TickMembers(); //TODO: Tick this less often since we dont move matter as much.
}

function TickAllPowerGroups()
{
	cancel($EOTW::PowerTickSchedule);

	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);

		if (!%client.hasSpawnedOnce)
			continue;

		%client.schedule(33, "TickPowerGroups");
	}

	$EOTW::PowerTickSchedule = schedule(500, 0, "TickAllPowerGroups");
}
$EOTW::PowerTickSchedule = schedule(500, 0, "TickAllPowerGroups");

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
};
activatePackage("EOTW_Power");