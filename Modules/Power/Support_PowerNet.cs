datablock fxDTSBrickData(brickEOTWGroupPowerUnitData)
{
	brickFile = "./Shapes/MicroCapacitor2x.blb";
	category = "Solar Apoc";
	subCategory = "Power Unit";
	//uiName = "Central Power Unit";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/MicroCapacitor";

    isPowered = true;
	powerType = "Battery";
    processSound = PowerUnitLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWGroupPowerUnitData"] = 1.00 TAB "d36b04ff" TAB 1 TAB "dog";
$EOTW::BrickDescription["brickEOTWGroupPowerUnitData"] = "Not implemented!";

//$EOTW::CustomBrickCost["brickEOTWGroupPowerUnitData"] = 1.00 TAB "d36b04ff" TAB 128 TAB "Copper" TAB 128 TAB "Silver" TAB 128 TAB "Gold" TAB 128 TAB "Lead";
//$EOTW::BrickDescription["brickEOTWGroupPowerUnitData"] = "Main central unit for power networks. Connect machines to this using the Cable Layer to power them!";

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

		if (%target.getID() != %obj.getID()) // && %dist <= getMin(%maxRange, %target.getMaxRange())
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
		%max = 4096;

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

	%change = %obj.getPower() - %initBuffer;
	if (%change > 0)
	{
		%obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
		%obj.PlayMachineSound();
	}

	return %change;
}

function fxDTSBrick::PlayMachineSound(%obj)
{
	%data = %obj.getDatablock();
	if (isObject(%data.processSound) && !%obj.machineDisabled)
	{
		if (!isObject(%obj.audioEmitter))
			%obj.playSoundLooping(%data.processSound);

		cancel(%obj.EndSoundsLoopSchedule);
		%obj.EndSoundsLoopSchedule = %obj.schedule($EOTW::PowerTickRate * 1.1, "playSoundLooping");
	}
}

function fxDTSBrick::transferBrickPower(%obj, %amount, %target)
{
	%initTargetBuffer = %target.getPower();

	if (%obj.getPower() <= 0)
		return 0;

	%sourceDifference = %obj.changeBrickPower(-1 * %amount);
	%sourceDifference += %target.changeBrickPower(-1 * %sourceDifference);
	%obj.changeBrickPower(-1 * %sourceDifference); //Refund leftover power

	return %target.getPower() - %initTargetBuffer;
}

function fxDtsBrick::attemptPowerDraw(%obj, %amount)
{
	%amount = mRound(%amount);
    %drawLeft = %amount;
	%obj.lastDrawTime = getSimTime();

	%extractFrom = "Source\tBattery";
	for (%j = 0; %j < getFieldCount(%extractFrom); %j++)
	{
		%type = getField(%extractFrom, %j);
		%set = %obj.connections[%type];
		for (%i = 0; %i < getFieldCount(%set) && %drawLeft > 0; %i++)
		{
			%source = getField(%set, %i);

			if (!isObject(%source))
			{
				%obj.searchForConnections(%type);
				continue;
			}

			%drawLeft += %source.changeBrickPower(-1 * %drawLeft);
		}
	}

	if (%drawLeft <= 0)
		%obj.PlayMachineSound();

	if (%drawLeft <= 0)
	{
		%obj.lastDrawSuccess = getSimTime();
		return true;
	}
	
	return false;
}

function fxDTSbrick::SetMachinePowered(%brick,%mode)
{
	switch (%mode)
	{
		case 0: %brick.machineDisabled = !%brick.machineDisabled;
		case 1: %brick.machineDisabled = true;
		case 2: %brick.machineDisabled = false;
	}
}
registerOutputEvent(fxDTSbrick, "SetMachinePowered", "list Toggle 0 On 1 Off 2", 0);

$EOTW::PowerTickRate = 500;
function fxDtsBrick::getStatusText(%obj) {
	%powerStatus = "---";
	%data = %obj.getDatablock();
	if (%data.isPowered)
	{
		%powerStatus = "\c0Not Running";
		if (%obj.machineDisabled)
			%powerStatus = "\c1Disabled";
		else if (getSimTime() - %obj.lastDrawTime <= $EOTW::PowerTickRate)
		{
			if (getSimTime() - %obj.lastDrawSuccess <= $EOTW::PowerTickRate)
				%powerStatus = "\c2Running";
			else
				%powerStatus = "\c3Not Enough Power";
		}
	}

	%machineStatus = "---";
	if (%data.isProcessingMachine)
	{
		%machineStatus = %data.getProcessingText(%obj);
	}
	else if (%data.maxHeatCapacity > 0)
	{
		%machineStatus = "HEAT: " @ (%obj.fissionHeat + 0) @ "/" @ %data.maxHeatCapacity;
	}
	
	return "<just:center>\c6[" @ %machineStatus @ "\c6] | [" @ %powerStatus @ "\c6]";
}

function fxDtsBrick::onTick(%obj)
{
	for (%i = 0; %i <= (%obj.upgradeTier + 0); %i++)
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

function echoGroup(%obj, %pre)
{
	echo(%pre @ ": " @ %obj.getGroup());
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
		if (!%obj.getObject(%i).machineDisabled)
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

	$EOTW::PowerTickSchedule = schedule($EOTW::PowerTickRate, 0, "TickAllPowerGroups");
}
$EOTW::PowerTickSchedule = schedule($EOTW::PowerTickRate, 0, "TickAllPowerGroups");

package EOTW_Power {
	function fxDtsBrick::onPlant(%obj, %b)
	{
		parent::onPlant(%obj, %b);
		
		%obj.LoadPowerData();
	}

	function fxDtsBrick::onLoadPlant(%obj, %b)
	{
		parent::onLoadPlant(%obj, %b);
		
		%data = %obj.getDatablock();
		if (%data.isPowered || %data.isMatterPipe)
			$EOTW::PostLoad.add(%obj);
	}
};
activatePackage("EOTW_Power");

$EOTW::PostLoad = new SimSet();
function BrickPostLoad()
{
	for (%i = 0; %i < $EOTW::PostLoad.getCount(); %i++)
	{
		%obj = $EOTW::PostLoad.getObject(%i);

		%obj.LoadPowerData();
		%obj.LoadPipeData();

		if (%obj.getDatablock().matterSize > 0)
			RefreshAdjacentExtractors(%obj.getWorldBox());
	}
}