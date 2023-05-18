datablock fxDTSBrickData(brickEOTWEnergyCable1x1fData)
{
	brickFile = "base/data/bricks/flats/1x1F.blb";
	category = "Solar Apoc";
	subCategory = "Power Cables";
	uiName = "Cable 1x1f";
	iconName = "base/client/ui/brickIcons/1x1f";

    isPowerCable = true;
};
$EOTW::CustomBrickCost["brickEOTWEnergyCable1x1fData"] = 1.00 TAB "" TAB 16 TAB "Lead" TAB 32 TAB "Copper";
$EOTW::BrickDescription["brickEOTWEnergyCable1x1fData"] = "Used to connect machines to create a power network.";

datablock fxDTSBrickData(brickEOTWEnergyCable1x2fData : brickEOTWEnergyCable1x1fData)
{
	brickFile = "base/data/bricks/flats/1x2F.blb";
	uiName = "Cable 1x2f";
	iconName = "base/client/ui/brickIcons/1x2f";
};
$EOTW::CustomBrickCost["brickEOTWEnergyCable1x2fData"] = 1.00 TAB "" TAB 32 TAB "Lead" TAB 64 TAB "Copper";
$EOTW::BrickDescription["brickEOTWEnergyCable1x2fData"] = "Used to connect machines to create a power network.";

datablock fxDTSBrickData(brickEOTWEnergyCable1x4fData : brickEOTWEnergyCable1x1fData)
{
	brickFile = "base/data/bricks/flats/1x4F.blb";
	uiName = "Cable 1x4f";
	iconName = "base/client/ui/brickIcons/1x4f";
};
$EOTW::CustomBrickCost["brickEOTWEnergyCable1x4fData"] = 1.00 TAB "" TAB 64 TAB "Lead" TAB 128 TAB "Copper";
$EOTW::BrickDescription["brickEOTWEnergyCable1x4fData"] = "Used to connect machines to create a power network.";

datablock fxDTSBrickData(brickEOTWEnergyCable1x8fData : brickEOTWEnergyCable1x1fData)
{
	brickFile = "base/data/bricks/flats/1x8F.blb";
	uiName = "Cable 1x8f";
	iconName = "base/client/ui/brickIcons/1x8f";
};
$EOTW::CustomBrickCost["brickEOTWEnergyCable1x8fData"] = 1.00 TAB "" TAB 64 TAB "Rubber" TAB 128 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWEnergyCable1x8fData"] = "Used to connect machines to create a power network.";

datablock fxDTSBrickData(brickEOTWEnergyCable1x16fData : brickEOTWEnergyCable1x1fData)
{
	brickFile = "base/data/bricks/flats/1x16F.blb";
	uiName = "Cable 1x16f";
	iconName = "base/client/ui/brickIcons/1x16f";
};
$EOTW::CustomBrickCost["brickEOTWEnergyCable1x16fData"] = 1.00 TAB "" TAB 128 TAB "Rubber" TAB 256 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWEnergyCable1x16fData"] = "Used to connect machines to create a power network.";

$EOTW::PowerTickRate = 600;

function GetCablesInBox(%boxcenter,%boxsize,%filterbrick)//returns an array object,filter brick gets passed up..
{
	%arrayobj = new ScriptObject(brickarray);
	%arrayobj.array[0] = 0;
	%arrayobj.count = 0;

	//DEBUG
	//createBoxMarker(%boxcenter, '1 1 0 0.5', %boxsize).schedule(2000, "delete");
	
	InitContainerBoxSearch(%boxcenter,%boxsize,$TypeMasks::fxBrickObjectType);
	while(isObject(%obj = containerSearchNext()))
	{
		if (!%obj.isPlanted)
			continue;
		
		if(!isObject(%filterbrick) || %obj != %filterbrick)
		{
			%data = %obj.getDatablock();

			if (%data.isPowered || %data.isPowerCable)
			{
				%arrayobj.array[%arrayobj.count] = %obj;
				%arrayobj.count++;
			}
		}
	}

	return %arrayobj;
}


//put replacementworldbox as 0 when you input a brick, use bricks, ie or pe.
//dir("xpos,xneg etc" or "all" for a useless array of all adj.
function findAdjacentCables(%Obj,%dir,%replacementworldbox)
{
	if(!IsObject(%Obj) && !%replacementworldbox)//if not enough Data is supplied, freak out.
	{
		%boxes = new ScriptObject(brickarray);
		%boxes.array[0] = 0;
		%boxes.count = 0;
		return %boxes;
	}
	
	if(%replacementworldbox)
		%worldbox = %replacementworldbox;

	if(IsObject(%Obj))
		%worldbox = %Obj.GetWorldBox();

	%lateralcutoff = 0.4;//cuttof factor for x and y directions. (makes search box slightly smaller)
	%verticalcutoff = 0.055566;
	%xsize = GetWord(%worldbox,3) - GetWord(%worldbox,0);
	%ysize = GetWord(%worldbox,4) - GetWord(%worldbox,1);
	%zsize = GetWord(%worldbox,5) - GetWord(%worldbox,2);
	
	%xcenter = GetWord(%worldbox,0) + %xsize/2;
	%ycenter = GetWord(%worldbox,1) + %ysize/2;
	%zcenter = GetWord(%worldbox,2) + %zsize/2;
	
	switch$(%dir)
	{
		case "xpos":
			%center = ((GetWord(%worldbox,3) + 0.25) SPC %ycenter SPC %zcenter);
			%size = ((0.5 - %lateralcutoff) SPC %ysize - %lateralcutoff SPC %zsize - %verticalcutoff );
			
			%boxes = GetCablesInBox(%center,%size,%Obj);
		case "xneg":
			%center = ((GetWord(%worldbox,0) - 0.25) SPC %ycenter SPC %zcenter);
			%size = ((0.5 - %lateralcutoff) SPC %ysize - %lateralcutoff SPC %zsize - %verticalcutoff );
			
			%boxes = GetCablesInBox(%center,%size,%Obj);
		case "ypos":
			%center = (%xcenter SPC (GetWord(%worldbox,4) + 0.25) SPC %zcenter);
			%size = ((%xsize - %lateralcutoff) SPC (0.5 - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetCablesInBox(%center,%size,%Obj);
		case "yneg":
			%center = (%xcenter SPC (GetWord(%worldbox,1) - 0.25) SPC %zcenter);
			%size = ((%xsize - %lateralcutoff) SPC (0.5 - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetCablesInBox(%center,%size,%Obj);
		case "zpos":
			%center = (%xcenter SPC %ycenter SPC (GetWord(%worldbox,5) + 0.10));
			%size = ((%xsize - %lateralcutoff) SPC (%ysize - %lateralcutoff) SPC %verticalcutoff );
			%boxes = GetCablesInBox(%center,%size,%Obj);
		case "zneg":
			%center = (%xcenter SPC %ycenter SPC (GetWord(%worldbox,2) - 0.10));
			%size = ((%xsize - %lateralcutoff) SPC (%ysize - %lateralcutoff) SPC %verticalcutoff );
			%boxes = GetCablesInBox(%center,%size,%Obj);
		
		case "all":
			%xposbricks = findAdjacentCables(%Obj,"xpos",%replacementworldbox);
			%xnegbricks = findAdjacentCables(%Obj,"xneg",%replacementworldbox);
			%yposbricks = findAdjacentCables(%Obj,"ypos",%replacementworldbox);
			%ynegbricks = findAdjacentCables(%Obj,"yneg",%replacementworldbox);
			%zposbricks = findAdjacentCables(%Obj,"zpos",%replacementworldbox);
			%znegbricks = findAdjacentCables(%Obj,"zneg",%replacementworldbox);
			
			%boxes = new ScriptObject(brickarray);
			%boxes.array[0] = 0;
			%boxes.count = 0;
			
			for(%a=0;%a<%xposbricks.count;%a++)
			{
				%boxes.array[%boxes.count] = %xposbricks.array[%a];
				%boxes.count++;
			}
			
			for(%b=0;%b<%xnegbricks.count;%b++)
			{
				%boxes.array[%boxes.count] = %xnegbricks.array[%b];
				%boxes.count++;
			}
			
			/////////////////////////////////////////////////////////
			for(%c=0;%c<%yposbricks.count;%c++)
			{
				%boxes.array[%boxes.count] = %yposbricks.array[%c];
				%boxes.count++;
			}
			
			for(%d=0;%d<%ynegbricks.count;%d++)
			{
				%boxes.array[%boxes.count] = %ynegbricks.array[%d];
				%boxes.count++;
			}
			
			/////////////////////////////////////////////////////////
			for(%e=0;%e<%zposbricks.count;%e++)
			{
				%boxes.array[%boxes.count] = %zposbricks.array[%e];
				%boxes.count++;
			}
			
			for(%f=0;%f<%znegbricks.count;%f++)
			{
				%boxes.array[%boxes.count] = %znegbricks.array[%f];
				%boxes.count++;
			}
			%xposbricks.delete();
			%xnegbricks.delete();
			%yposbricks.delete();
			%ynegbricks.delete();
			%zposbricks.delete();
			%znegbricks.delete();
		default:
	}
	return %boxes;
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

function fxDTSBrick::PlayMachineSound(%obj, %override)
{
	%data = %obj.getDatablock();
	if (isObject(%data.processSound) && !%obj.machineDisabled)
	{
		%sound = isObject(%override) ? %override : %data.processSound;
		if (!isObject(%obj.audioEmitter) || %obj.audioEmitter.profile.getID() != %sound.getID())
				%obj.playSoundLooping(%sound);

		cancel(%obj.EndSoundsLoopSchedule);
		%obj.EndSoundsLoopSchedule = %obj.schedule($EOTW::PowerTickRate * 1.1, "playSoundLooping");
	}
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
				%powerStatus = "\c3Not Enough EU/Tick!";
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
	
	if ((!%data.isPowered && !%data.isPowerCable) || %bl_id < 1)
		return;

	%set = getPowerSet(%data.powerType, %bl_id);
	%set.add(%obj);

	%adj = findAdjacentCables(%obj, "all", 0);

	for (%i = 0; %i < %adj.count; %i++)
	{
		%target = %adj.array[%i];
		if (isObject(%target.cableNet))
		{
			%hitCableNet = true;
			%target.SpreadCableNet();
		}
	}

	if (%hitCableNet)
		return;

	//No cables found. Lets just make our own cable net.
	%cableGroup = new ScriptObject(cableGroup);
	%cableGroup.AddCable(%obj);
	getPowerSet("Cable", %bl_id).add(%cableGroup);
	%obj.SpreadCableNet();
}

function fxDtsBrick::RemoveCable(%obj)
{
	%obj.cableNet.RemoveCable(%obj);
}

function ScriptObject::RemoveCable(%obj, %cable)
{
	for (%i = 0; %i < getFieldCount(%obj.cableTypes); %i++)
	{
		%set = %obj.set[getField(%obj.cableTypes, %i)];
		if (%set.isMember(%cable))
		{
			%set.remove(%cable);

			if (%set.getCount() == 0)
			{
				%cableType = "power";
				if (%cable.getDatablock().powerType !$= "")
					%cableType = %cable.getDatablock().powerType;

				%obj.cableTypes = removeField(%obj.cableTypes, getFieldIndex(%obj.cableTypes, %cableType));
				%set.delete();
				if (getFieldCount(%obj.cableTypes) == 0)
					%obj.delete();
			}
			break;
		}
	}
}

function ScriptObject::AddCable(%obj, %cable)
{
	%data = %cable.getDatablock();
	%obj.isCableNet = true;

	if (%cable.cableNet == %obj)
		return;

	if (isObject(%cable.cableNet))
		%cable.RemoveCable();

	//Get what type of cable this thing is
	%cableType = "power";
	if (%data.powerType !$= "")
		%cableType = %data.powerType;

	//Add the cable type to our list of possible types
	if (!hasField(%obj.cableTypes, %cableType))
		%obj.cableTypes = trim(%obj.cableTypes TAB %cableType);

	//Add the cable to the cablenet, making a new simset for the cable type if needed
	if (!isObject(%obj.set[%cableType]))
		%obj.set[%cableType] = new SimSet();

	//Add the cable to its specified category, and make a reference from the cable itself to the cable net scriptobject
	%obj.set[%cableType].add(%cable);
	%cable.cableNet = %obj;
}

function fxDtsBrick::SpreadCableNet(%obj, %scanCount)
{
	%adj = findAdjacentCables(%obj, "all", 0);
	for (%i = 0; %i < %adj.count; %i++)
	{
		%cable = %adj.array[%i];
		if (%cable.cableNet != %obj.cableNet)
		{
			%obj.cableNet.AddCable(%cable);
			if (%scanCount > 5)
				%cable.schedule(33, "SpreadCableNet", 0);
			else
				%cable.SpreadCableNet(%scanCount + 1);
		}
		
	}
}

function RefreshAdjacentCables(%boundbox)
{
	%adj = findAdjacentCables("","all", %boundbox);
	if (%adj.count > 0)
	{
		for (%i = 0; %i < %adj.count; %i++)
		{
			%cable = %adj.array[%i];
			%cableGroup = new ScriptObject(cableGroup);
			%cableGroup.AddCable(%cable);
			getPowerSet("Cable", %cable.getGroup().bl_id).add(%cableGroup);
			%cable.SpreadCableNet();
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

function SimSet::TickMembers(%obj)
{
	if (%obj.getCount() == 0)
		return;
		
	%obj.pushFrontToBack();
	for (%i = 0; %i < %obj.getCount(); %i++)
	{
		%target = %obj.getObject(%i);
		if (%target.isCableNet)
			%target.onPowerCableTick();
		else if (!%target.machineDisabled)
			%target.onTick();
	}
		
}

function ScriptObject::onPowerCableTick(%obj)
{
	//Move whatever extra power we have into batteries
	if (isObject(%set = %obj.set["Battery"]))
	{
		%set.pushFrontToBack(); //Round robin
		for (%i = 0; %i < %set.getCount(); %i++)
		{
			%battery = %set.getObject(%i);
			%obj.powerBuffer -= %battery.changeBrickPower(getMin(%battery.getDatablock().maxInput, %obj.powerBuffer));
			if (%obj.powerBuffer <= 0)
				break;
		}
	}

	//Move power from sources into itself
	if (%obj.powerBuffer == 0 && isObject(%set = %obj.set["Source"]))
	{
		%set.pushFrontToBack(); //Round robin
		for (%i = 0; %i < %set.getCount(); %i++)
		{
			%target = %set.getObject(%i);
			%obj.powerBuffer += %target.getPower();
			%target.powerBuffer = 0;
		}
	}
}

datablock AudioProfile(MachineBrownOutLoopSound)
{
   filename    = "base/data/sound/error.wav"; //TODO: Get a fancier noise.
   description = AudioCloseLooping3d;
   preload = true;
};

function fxDtsBrick::attemptPowerDraw(%obj, %drain)
{
	if (!isObject(%net = %obj.cableNet))
		return false;

	%initDrain = %drain;

	%obj.lastDrawTime = getSimTime();

	//Check to see if the cable network buffer already has enough
	%change = getMin(%net.powerBuffer, %drain);
	%net.powerBuffer -= %change;
	%drain -= %change;

	//Attempt to drain batteries and sources of thier juices
	%extractFrom = "Source\tBattery";
	for (%j = 0; %j < getFieldCount(%extractFrom) && %drain > 0; %j++)
	{
		%type = getField(%extractFrom, %j);
		%set = %net.set[%type];
		if (isObject(%set))
			for (%i = 0; %i < %set.getCount() && %drain > 0; %i++)
				%drain += %set.getObject(%i).changeBrickPower(-1 * %drain);
	}

	if (%drain <= 0)
	{
		%obj.PlayMachineSound();
		%obj.lastDrawSuccess = getSimTime();
		return true;
	}
	else
	{
		%obj.PlayMachineSound(MachineBrownOutLoopSound);
		return false;
	}
}

function fxDtsBrick::drainPowerNet(%obj, %drain)
{
	if (!isObject(%net = %obj.cableNet))
		return false;

	%initDrain = %drain;

	%obj.lastDrawTime = getSimTime();

	//Check to see if the cable network buffer already has enough
	%change = getMin(%net.powerBuffer, %drain);
	%net.powerBuffer -= %change;
	%drain -= %change;

	//Attempt to drain batteries and sources of thier juices
	%extractFrom = "Source\tBattery";
	for (%j = 0; %j < getFieldCount(%extractFrom) && %drain > 0; %j++)
	{
		%type = getField(%extractFrom, %j);
		%set = %net.set[%type];
		if (isObject(%set))
			for (%i = 0; %i < %set.getCount() && %drain > 0; %i++)
				%drain += %set.getObject(%i).changeBrickPower(-1 * %drain);
	}

	return %initDrain - %drain;
}

function GameConnection::TickPowerGroups(%client) {
	%bl_id = %client.bl_id;

	getPowerSet("Source", %bl_id).TickMembers();
	getPowerSet("Cable", %bl_id).TickMembers();
	getPowerSet("Battery", %bl_id).TickMembers();
	getPowerSet("Machine", %bl_id).TickMembers();
	getPowerSet("Logistic", %bl_id).TickMembers();
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
	function fxDTSBrickData::onDeath(%data, %this)
	{
		Parent::onDeath(%data, %this);

		if (%data.isPowered || %data.isPowerCable)
			RefreshAdjacentCables(%this.getWorldBox());
	}
	function fxDTSBrickData::onRemove(%data, %this)
	{
		Parent::onRemove(%data, %this);

		if (%data.isPowered || %data.isPowerCable)
			RefreshAdjacentCables(%this.getWorldBox());
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