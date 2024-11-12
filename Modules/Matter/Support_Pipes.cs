//Normal pipes
datablock fxDTSBrickData(brickEOTWMatterPipe1x1fData)
{
	brickFile = "base/data/bricks/flats/1x1F.blb";
	category = "Solar Apoc";
	subCategory = "Piping Tubes";
	uiName = "Pipe 1x1f";
	iconName = "base/client/ui/brickIcons/1x1f";

    isMatterPipe = true;
	pipeType = "pipe";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x1fData"] = 1.00 TAB "" TAB 1 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipe1x1fData"] = "Used to connect Connectors and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x1Data : brickEOTWMatterPipe1x1fData)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	uiName = "Pipe 1x1";
	iconName = "base/client/ui/brickIcons/1x1";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x1Data"] = 1.00 TAB "" TAB 1 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipe1x1Data"] = "Used to connect Connectors and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x2Data : brickEOTWMatterPipe1x1fData)
{
	brickFile = "base/data/bricks/bricks/1x2.blb";
	uiName = "Pipe 1x2";
	iconName = "base/client/ui/brickIcons/1x2";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x2Data"] = 1.00 TAB "" TAB 2 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipe1x2Data"] = "Used to connect Connectors and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x4Data : brickEOTWMatterPipe1x1fData)
{
	brickFile = "base/data/bricks/bricks/1x4.blb";
	uiName = "Pipe 1x4";
	iconName = "base/client/ui/brickIcons/1x4";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x4Data"] = 1.00 TAB "" TAB 4 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipe1x4Data"] = "Used to connect Connectors and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x8Data : brickEOTWMatterPipe1x1fData)
{
	brickFile = "base/data/bricks/bricks/1x8.blb";
	uiName = "Pipe 1x8";
	iconName = "base/client/ui/brickIcons/1x8";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x8Data"] = 1.00 TAB "" TAB 8 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipe1x8Data"] = "Used to connect Connectors and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x16Data : brickEOTWMatterPipe1x1fData)
{
	brickFile = "base/data/bricks/bricks/1x16.blb";
	uiName = "Pipe 1x16";
	iconName = "base/client/ui/brickIcons/1x16";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x16Data"] = 1.00 TAB "" TAB 16 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipe1x16Data"] = "Used to connect Connectors and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x1x5Data : brickEOTWMatterPipe1x1fData)
{
	brickFile = "base/data/bricks/bricks/1x1x5.blb";
	uiName = "Pipe 1x1x5";
	iconName = "base/client/ui/brickIcons/1x1x5";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x1x5Data"] = 1.00 TAB "" TAB 5 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipe1x1x5Data"] = "Used to connect Connectors and Extractors for matter piping.";

//Extractors
datablock fxDTSBrickData(brickEOTWMatterPipeExtractor1Data)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Solar Apoc";
	subCategory = "Pipe Connections";
	uiName = "Matter Extractor I";
	iconName = "base/client/ui/brickIcons/1x1";

    isMatterPipe = true;
	pipeType = "extractor";
	maxTransfer = 2;
	allowFiltering = true;

	isPowered = true;
	powerType = "Logistic";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipeExtractor1Data"] = 1.00 TAB "" TAB 128 TAB "Silver" TAB 4 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipeExtractor1Data"] = "Extracts matter from an adjacent machine's output into other machines in a network. (2 Units/tick)";

function brickEOTWMatterPipeExtractor1Data::onTick(%this, %obj) { %obj.runPipingTick(); }

datablock fxDTSBrickData(brickEOTWMatterPipeExtractor2Data : brickEOTWMatterPipeExtractor1Data)
{
	uiName = "Matter Extractor II";
	maxTransfer = 8;
};
$EOTW::CustomBrickCost["brickEOTWMatterPipeExtractor2Data"] = 1.00 TAB "" TAB 128 TAB "Red Gold" TAB 8 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipeExtractor2Data"] = "Improved Matter Extractor. (8 Units/tick)";

function brickEOTWMatterPipeExtractor2Data::onTick(%this, %obj) { %obj.runPipingTick(); }

datablock fxDTSBrickData(brickEOTWMatterPipeExtractor3Data : brickEOTWMatterPipeExtractor1Data)
{
	uiName = "Matter Extractor III";
	maxTransfer = 32;
};
$EOTW::CustomBrickCost["brickEOTWMatterPipeExtractor3Data"] = 1.00 TAB "" TAB 128 TAB "Naturum" TAB 16 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipeExtractor3Data"] = "Superior Matter Extractor. (32 Units/tick)";

function brickEOTWMatterPipeExtractor3Data::onTick(%this, %obj) { %obj.runPipingTick(); }

datablock fxDTSBrickData(brickEOTWMatterSteamExtractorData : brickEOTWMatterPipeExtractor1Data)
{
	uiName = "Steam Extractor";
	pipeWhitelist = "Steam\tSuper-Heated Steam\tWater";
	maxTransfer = 512;
};
$EOTW::CustomBrickCost["brickEOTWMatterSteamExtractorData"] = 1.00 TAB "" TAB 128 TAB "Lead" TAB 6 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterSteamExtractorData"] = "Specialized matter extractor for moving specifically water/steam. (512 Units/tick)";

function brickEOTWMatterSteamExtractorData::onTick(%this, %obj) { %obj.runPipingTick(); }

//Connector
datablock fxDTSBrickData(brickEOTWMatterPipeConnectorData)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Solar Apoc";
	subCategory = "Pipe Connections";
	uiName = "Matter Connector";
	iconName = "base/client/ui/brickIcons/1x1";

    isMatterPipe = true;
	pipeType = "connector";
	allowFiltering = true;
};
$EOTW::CustomBrickCost["brickEOTWMatterPipeConnectorData"] = 1.00 TAB "" TAB 64 TAB "Rubber" TAB 1 TAB "Piping";
$EOTW::BrickDescription["brickEOTWMatterPipeConnectorData"] = "Allows extractors to insert matter into whatever machine this device is adjacent to.";

//This is probably the most intensive machine function.
function fxDtsBrick::runPipingTick(%obj)
{
	%data = %obj.getDatablock();

	//Make sure we have atleast one connector before we go ham on calculations.
	if (!isObject(%connectorSet = %obj.pipeNet.set["connector"]) || %connectorSet.getCount() < 1 || getFieldCount(%obj.adjacentMatterBricks) < 1)
		return;

	//Get our source brick
	%source = getField(%obj.adjacentMatterBricks, getRandom(getFieldCount(%obj.adjacentMatterBricks) - 1));
	if (!isObject(%source))
	{
		%obj.findAdjacentMatterBricks();
		return;
	}

	%sourceData = %source.getDatablock();

	%sourceSlots = "Output\tBuffer";
	for (%i = 0; %i < getFieldCount(%sourceSlots); %i++)
	{
		%sourceSlot = getField(%sourceSlots, %i);
		for (%j = 0; %j < %sourceData.matterSlots[%sourceSlot]; %j++)
		{
			%sourceMatter = getField(%source.matter[%sourceSlot, %j], 0);
			%sourceAmount = getMin(getField(%source.matter[%sourceSlot, %j], 1), mCeil(%data.maxTransfer / getFieldCount(%obj.adjacentMatterBricks)));
			if (%sourceMatter $= "" || (getFieldCount(%obj.machineFilter) > 0 && !hasField(%obj.machineFilter, %sourceMatter)))
				continue;

			if (%data.pipeWhitelist !$= "" && !hasField(%data.pipeWhitelist, %sourceMatter))
				continue;
				
			%obj.attemptPowerDraw(0);
			%transferLeft = %sourceAmount;

			for (%k = 0; %k < %connectorSet.getCount(); %k++)
			{
				//Get a matter connector
				%conn = %connectorSet.getObject(%k);

				//Enabled check
				if (%conn.machineDisabled)
					continue;

				//Filter check
				if (getFieldCount(%conn.machineFilter) > 0 && !hasField(%conn.machineFilter, %sourceMatter))
					continue;

				//Get a random adjacent brick, and try to add stuff to it
				%conn_source = getField(%conn.adjacentMatterBricks, getRandom(getFieldCount(%conn.adjacentMatterBricks) - 1));
				
				if (isObject(%conn_source))
				{
					%transferLeft -= %conn_source.ChangeMatter(%sourceMatter, %transferLeft, "Input");
					%transferLeft -= %conn_source.ChangeMatter(%sourceMatter, %transferLeft, "Buffer");
				}
				else
					%conn.findAdjacentMatterBricks();
				

				if (%transferLeft < 1)
					break;
			}
			//Round robin
			%connectorSet.pushFrontToBack();
			%source.changeMatter(%sourceMatter, (%sourceAmount - %transferLeft) * -1, %sourceSlot);
		}
	}
}

package EOTW_Pipes {
	function fxDtsBrick::onPlant(%obj)
	{
		parent::onPlant(%obj);
		
		if (%obj.getDatablock().isMatterPipe)
			%obj.schedule(100, "LoadPipeData");

		if (%obj.getDatablock().matterSize > 0 && %obj.isPlanted)
			RefreshAdjacentExtractors(%obj.getWorldBox());
	}
	function fxDtsBrick::onLoadPlant(%obj, %b)
	{
		parent::onLoadPlant(%obj, %b); //LoadPlant is handled in Support_PowerNet
	}
	function fxDTSBrickData::onDeath(%data, %this)
	{
		Parent::onDeath(%data, %this);

		if (%this.pipeNet != 0)
		{
			if (%data.isMatterPipe)
				RefreshAdjacentPipes(%this.getWorldBox());
			if (%data.matterSize > 0)
				RefreshAdjacentExtractors(%this.getWorldBox());
		}
	}
	function fxDTSBrickData::onRemove(%data, %this)
	{
		Parent::onRemove(%data, %this);

		if (%this.pipeNet != 0)
		{
			if (%data.isMatterPipe)
				RefreshAdjacentPipes(%this.getWorldBox());
			if (%data.matterSize > 0)
				RefreshAdjacentExtractors(%this.getWorldBox());
		}
		
	}
	function fxDtsBrick::setColor(%brick, %color)
	{
		Parent::setColor(%brick, %color);

		if (%brick.isPlanted && %brick.getDatablock().isMatterPipe)
			RefreshAdjacentPipes(%brick.getWorldBox());
	}
};
activatePackage("EOTW_Pipes");

function fxDtsBrick::LoadPipeData(%obj)
{
	%data = %obj.getDatablock();
	if (!%data.isMatterPipe || !%obj.isPlanted)
		return;
		
	%obj.findAdjacentMatterBricks();

	%adj = findAdjacentPipes(%obj, "all", "pipe\tconnector\textractor", 0);
	if (%adj.count > 0)
	{
		//We found another pipe. Lets connect our new pipe to the others.
		//We can inherit the biggest pipenet found and overtake any others that are connected.
		//The findAdjacentPipes function should give us pipes that already have a pipenet on them.

		//TODO: Make pipenet to keep the one with the most items.
		for (%i = 0; %i < %adj.count; %i++)
		{
			%target = %adj.array[%i];
			if (isObject(%target.pipeNet))
			{
				%hitPipeNet = true;
				%target.SpreadPipeNet();
			}
			
		}

		if (%hitPipeNet)
			return;
	}

	//No pipes found. Lets just make our own pipenet.
	%pipeGroup = new ScriptObject(pipeGroup);
	%pipeGroup.AddPipe(%obj);
	%obj.SpreadPipeNet();
}

function fxDtsBrick::findAdjacentMatterBricks(%obj)
{
	%boxes = findAdjacentMatterBricks(%obj, "all", "");
	%obj.adjacentMatterBricks = "";
	for (%i = 0; %i < %boxes.count; %i++)
		if (%obj.stackBL_ID == %boxes.array[%i].stackBL_ID)
			%obj.adjacentMatterBricks = trim(%obj.adjacentMatterBricks TAB %boxes.array[%i]);
		
}

function RefreshAdjacentExtractors(%boundbox)
{
	%adj = findAdjacentPipes("","all","pipe\tconnector\textractor", %boundbox);
	if (%adj.count > 0)
	{
		for (%i = 0; %i < %adj.count; %i++)
		{
			%pipe = %adj.array[%i];
			%pipe.findAdjacentMatterBricks();
		}
	}
}

function RefreshAdjacentPipes(%boundbox)
{
	%adj = findAdjacentPipes("","all","pipe\tconnector\textractor", %boundbox);
	if (%adj.count > 0)
	{
		for (%i = 0; %i < %adj.count; %i++)
		{
			%pipe = %adj.array[%i];
			%pipeGroup = new ScriptObject(pipeGroup);
			%pipeGroup.AddPipe(%pipe);
			%pipe.SpreadPipeNet();
		}
	}
}

function fxDtsBrick::SpreadPipeNet(%obj, %scanCount)
{
	%adj = findAdjacentPipes(%obj, "all", "pipe\tconnector\textractor", 0);
	if (%adj.count > 0)
	{
		for (%i = 0; %i < %adj.count; %i++)
		{
			%pipe = %adj.array[%i];
			if (%pipe.pipeNet != %obj.pipeNet && %pipe.getColorID() == %obj.getColorID())
			{
				//%obj.pipeNet.overTakePipeNet(%pipe.pipeNet);
				%obj.pipeNet.AddPipe(%pipe);
				if (%scanCount % 5 == 4)
					%pipe.schedule(33, "SpreadPipeNet", %scanCount + 1);
				else
					%pipe.SpreadPipeNet(%scanCount + 1);
			}

			if (%scanCount > 1000) {
				warn("Way too many scans for pipe spread!" SPC %scanCount SPC %obj);
			}
			
		}
	}
}

function fxDtsBrick::RemovePipe(%obj)
{
	%obj.pipeNet.RemovePipe(%obj);
}

function ScriptObject::RemovePipe(%obj, %pipe)
{
	for (%i = 0; %i < getFieldCount(%obj.pipeTypes); %i++)
	{
		%set = %obj.set[getField(%obj.pipeTypes, %i)];
		if (%set.isMember(%pipe))
		{
			%set.remove(%pipe);

			if (%set.getCount() == 0)
			{
				%obj.pipeTypes = removeField(%obj.pipeTypes, getFieldIndex(%obj.pipeTypes, %pipe.getDatablock().pipeType));
				%set.delete();
				if (getFieldCount(%obj.pipeTypes) == 0)
					%obj.delete();
			}
			break;
		}
	}
}

function ScriptObject::AddPipe(%obj, %pipe)
{
	%data = %pipe.getDatablock();
	if (!%data.isMatterPipe || %pipe.pipeNet == %obj)
		return;

	if (isObject(%pipe.pipeNet))
		%pipe.RemovePipe();

	//Get what type of pipe this thing is
	%pipeType = "pipe";
	if (%data.pipeType !$= "")
		%pipeType = %data.pipeType;

	//Add the pipe type to our list of possible types
	if (!hasField(%obj.pipeTypes, %pipeType))
		%obj.pipeTypes = trim(%obj.pipeTypes TAB %pipeType);

	//Add the pipe to the pipenet, making a new simset for the pipetype if needed
	if (!isObject(%obj.set[%pipeType]))
		%obj.set[%pipeType] = new SimSet();

	//Add the pipe to its specified category, and make a reference from the pipe itself to the pipenet scriptobject
	%obj.set[%pipeType].add(%pipe);
	%pipe.pipeNet = %obj;
}

function ScriptObject::GetPipeNetSize(%obj)
{
	%sum = 0;
	for (%i = 0; %i < getFieldCount(%obj.pipeTypes); %i++)
		%sum += %obj.set[getField(%obk.pipeTypes, %i)].getCount();

	return %sum;
}

function ScriptObject::takeOverPipeNet(%obj, %target)
{
	for (%i = 0; %i < getFieldCount(%target.pipeTypes); %i++)
	{
		%targetSet = %target.set[getField(%target.pipeTypes, %i)];
		for (%j = 0; %j < %targetSet.getCount(); %j++)
			%obj.addPipe(%targetSet.getObject(%j));
	}

	%target.deletePipeNet();
}

function ScriptObject::deletePipeNet(%obj)
{
	for (%i = 0; %i < getFieldCount(%obj.pipeTypes); %i++)
		%obj.set[getField(%obk.pipeTypes, %i)].delete();

	%obj.delete();
}

function GetPipesInBox(%boxcenter,%boxsize,%type,%filterbrick)//returns an array object,filter brick gets passed up..
{
	%arrayobj = new ScriptObject(brickarray);
	%arrayobj.array[0] = 0;
	%arrayobj.count = 0;

	//DEBUG
	//createBoxMarker(%boxcenter, '1 0 0 0.5', %boxsize).schedule(2000, "delete");
	
	InitContainerBoxSearch(%boxcenter,%boxsize,$TypeMasks::FxBrickAlwaysObjectType);
	while(isObject(%obj = containerSearchNext()))
	{
		if(!isObject(%filterbrick) || (%obj != %filterbrick && %obj.getColorID() == %filterbrick.getColorID()))
		{
			%data = %obj.getDatablock();
			if(%data.isMatterPipe && (%type $= "" || hasField(%type, %data.pipeType)))
			{
				%arrayobj.array[%arrayobj.count] = %obj;
				%arrayobj.count++;
			}
		}
	}

	return %arrayobj;
}

//put replacementworldbox as 0 when you input a brick, use bricks, ie or pe.
//dir("xpos,xneg etc" or "all" for a useless array of all adj.,types specifies what type like pipes
function findAdjacentPipes(%Obj,%dir,%type,%replacementworldbox)
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
			
			%boxes = GetPipesInBox(%center,%size,%type,%Obj);
		case "xneg":
			%center = ((GetWord(%worldbox,0) - 0.25) SPC %ycenter SPC %zcenter);
			%size = ((0.5 - %lateralcutoff) SPC %ysize - %lateralcutoff SPC %zsize - %verticalcutoff );
			
			%boxes = GetPipesInBox(%center,%size,%type,%Obj);
		case "ypos":
			%center = (%xcenter SPC (GetWord(%worldbox,4) + 0.25) SPC %zcenter);
			%size = ((%xsize - %lateralcutoff) SPC (0.5 - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetPipesInBox(%center,%size,%type,%Obj);
		case "yneg":
			%center = (%xcenter SPC (GetWord(%worldbox,1) - 0.25) SPC %zcenter);
			%size = ((%xsize - %lateralcutoff) SPC (0.5 - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetPipesInBox(%center,%size,%type,%Obj);
		case "zpos":
			%center = (%xcenter SPC %ycenter SPC (GetWord(%worldbox,5) + 0.08));
			%size = ((%xsize - %lateralcutoff) SPC (%ysize - %lateralcutoff) SPC %verticalcutoff );
			%boxes = GetPipesInBox(%center,%size,%type,%Obj);
		case "zneg":
			%center = (%xcenter SPC %ycenter SPC (GetWord(%worldbox,2) - 0.08));
			%size = ((%xsize - %lateralcutoff) SPC (%ysize - %lateralcutoff) SPC %verticalcutoff );
			%boxes = GetPipesInBox(%center,%size,%type,%Obj);
		
		case "all":
			%xposbricks = findAdjacentPipes(%Obj,"xpos",%type,%replacementworldbox);
			%xnegbricks = findAdjacentPipes(%Obj,"xneg",%type,%replacementworldbox);
			%yposbricks = findAdjacentPipes(%Obj,"ypos",%type,%replacementworldbox);
			%ynegbricks = findAdjacentPipes(%Obj,"yneg",%type,%replacementworldbox);
			%zposbricks = findAdjacentPipes(%Obj,"zpos",%type,%replacementworldbox);
			%znegbricks = findAdjacentPipes(%Obj,"zneg",%type,%replacementworldbox);
			
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

//Matter Bricks
function GetMatterBricksInBox(%boxcenter,%boxsize,%filterbrick)//returns an array object,filter brick gets passed up..
{
	%arrayobj = new ScriptObject(brickarray);
	%arrayobj.array[0] = 0;
	%arrayobj.count = 0;
	
	InitContainerBoxSearch(%boxcenter,%boxsize,$TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::StaticShapeObjectType);
	while(isObject(%obj = containerSearchNext()))
	{
		if(%obj != %filterbrick)
		{
			%data = %obj.getDatablock();
			if(%data.matterSize > 0)
			{
				%arrayobj.array[%arrayobj.count] = %obj;
				%arrayobj.count++;
			}
		}
	}

	return %arrayobj;
}

function findAdjacentMatterBricks(%Obj,%dir,%replacementworldbox)
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
	%verticalcutoff = 0.06;
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
			
			%boxes = GetMatterBricksInBox(%center,%size,%Obj);
		case "xneg":
			%center = ((GetWord(%worldbox,0) - 0.25) SPC %ycenter SPC %zcenter);
			%size = ((0.5 - %lateralcutoff) SPC %ysize - %lateralcutoff SPC %zsize - %verticalcutoff );
			
			%boxes = GetMatterBricksInBox(%center,%size,%Obj);
		case "ypos":
			%center = (%xcenter SPC (GetWord(%worldbox,4) + 0.25) SPC %zcenter);
			%size = ((%xsize - %lateralcutoff) SPC (0.5 - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetMatterBricksInBox(%center,%size,%Obj);
		case "yneg":
			%center = (%xcenter SPC (GetWord(%worldbox,1) - 0.25) SPC %zcenter);
			%size = ((%xsize - %lateralcutoff) SPC (0.5 - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetMatterBricksInBox(%center,%size,%Obj);
		case "zpos":
			%center = (%xcenter SPC %ycenter SPC (GetWord(%worldbox,5) + 0.10));
			%size = ((%xsize - %lateralcutoff) SPC (%ysize - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetMatterBricksInBox(%center,%size,%Obj);
		case "zneg":
			%center = (%xcenter SPC %ycenter SPC (GetWord(%worldbox,2) - 0.10));
			%size = ((%xsize - %lateralcutoff) SPC (%ysize - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetMatterBricksInBox(%center,%size,%Obj);
		
		case "all":
			%xposbricks = findAdjacentMatterBricks(%Obj,"xpos",%replacementworldbox);
			%xnegbricks = findAdjacentMatterBricks(%Obj,"xneg",%replacementworldbox);
			%yposbricks = findAdjacentMatterBricks(%Obj,"ypos",%replacementworldbox);
			%ynegbricks = findAdjacentMatterBricks(%Obj,"yneg",%replacementworldbox);
			%zposbricks = findAdjacentMatterBricks(%Obj,"zpos",%replacementworldbox);
			%znegbricks = findAdjacentMatterBricks(%Obj,"zneg",%replacementworldbox);
			
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
