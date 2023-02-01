datablock fxDTSBrickData(brickEOTWMatterPipe1x1fData)
{
	brickFile = "base/data/bricks/bricks/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Matter Piping";
	uiName = "Pipe 1xf";
	iconName = "base/client/ui/brickIcons/1x1f";

    isMatterPipe = true;
	pipeType = "pipe";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x1fData"] = 1.00 TAB "7a7a7aff" TAB 16 TAB "Rubber" TAB 8 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterPipe1x1fData"] = "Used to connect Inserters and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x1Data)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Solar Apoc";
	subCategory = "Matter Piping";
	uiName = "Pipe 1x";
	iconName = "base/client/ui/brickIcons/1x1";

    isMatterPipe = true;
	pipeType = "pipe";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x1Data"] = 1.00 TAB "7a7a7aff" TAB 16 TAB "Rubber" TAB 8 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterPipe1x1Data"] = "Used to connect Inserters and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x2Data)
{
	brickFile = "base/data/bricks/bricks/1x2.blb";
	category = "Solar Apoc";
	subCategory = "Matter Piping";
	uiName = "Pipe 2x";
	iconName = "base/client/ui/brickIcons/1x2";

    isMatterPipe = true;
	pipeType = "pipe";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x2Data"] = 1.00 TAB "7a7a7aff" TAB 32 TAB "Rubber" TAB 16 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterPipe1x2Data"] = "Used to connect Inserters and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipe1x4Data)
{
	brickFile = "base/data/bricks/bricks/1x4.blb";
	category = "Solar Apoc";
	subCategory = "Matter Piping";
	uiName = "Pipe 4x";
	iconName = "base/client/ui/brickIcons/1x4";

    isMatterPipe = true;
	pipeType = "pipe";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipe1x4Data"] = 1.00 TAB "7a7a7aff" TAB 64 TAB "Rubber" TAB 32 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterPipe1x4Data"] = "Used to connect Inserters and Extractors for matter piping.";

datablock fxDTSBrickData(brickEOTWMatterPipeExtractor1Data)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Solar Apoc";
	subCategory = "Matter Piping";
	uiName = "Matter Extractor";
	iconName = "base/client/ui/brickIcons/1x1";

    isMatterPipe = true;
	pipeType = "extractor";
	maxTransfer = 16;

	isPowered = true;
	powerType = "Logistic";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipeExtractor1Data"] = 1.00 TAB "7a7a7aff" TAB 64 TAB "Rubber" TAB 32 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterPipeExtractor1Data"] = "Extracts matter from an adjacent machine's output into other machines in a network.";

function brickEOTWMatterPipeExtractor1Data::onTick(%this, %obj) { %obj.runPipingTick(); }

datablock fxDTSBrickData(brickEOTWMatterPipeConnectorData)
{
	brickFile = "base/data/bricks/bricks/1x1.blb";
	category = "Solar Apoc";
	subCategory = "Matter Piping";
	uiName = "Matter Connector";
	iconName = "base/client/ui/brickIcons/1x1";

    isMatterPipe = true;
	pipeType = "connector";
};
$EOTW::CustomBrickCost["brickEOTWMatterPipeConnectorData"] = 1.00 TAB "7a7a7aff" TAB 64 TAB "Rubber" TAB 32 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterPipeConnectorData"] = "Allows extractors to insert matter into whatever machine this device is adjacent to.";

function fxDtsBrick::runPipingTick(%obj)
{
	%data = %obj.getDatablock();

	//Make sure we have atleast one connector before we go ham on calculations.
	if (!isObject(%connectorSet = %obj.pipeNet.set["connector"]) || %connectorSet.getCount() < 1)
		return;

	//Get our source brick
	%source = getField(%obj.adjacentMatterBricks, 0);
	%sourceData = %source.getDatablock();

	//Figure out what material to extract
	%typelist = "Buffer" TAB "Output";
	for (%j = 0; %j < getFieldCount(%typelist); %j++)
	{
		%type = getField(%typelist, %j);
		for (%i = 0; %i < %sourceData.matterSlots[%type]; %i++)
		{
			%matterData = %source.matter[%type, %i];
			if (getField(%matterData, 0) !$= "")
			{
				%transferAmount = getMin(%data.maxTransfer, getField(%matterData, 1));
				%transferMatter = getField(%matterData, 0);
				%transferLeft = %transferAmount;
				break;
			}
		}
	}
	
	if (%transferLeft <= 0)
		return 0;

	//Find the target(s) to transfer, and place stuff in each one.
	for (%i = 0; %i < %connectorSet.getCount(); %i++)
	{
		%conn = %connectorSet.getObject(%i);
		%conn_source = getField(%conn.adjacentMatterBricks, 0);
		%change = 0;
		%transferLeft -= %conn_source.ChangeMatter(%transferMatter, %transferLeft, "Input");
		%transferLeft -= %conn_source.ChangeMatter(%transferMatter, %transferLeft, "Buffer");

		if (%transferLeft < 1)
			break;
	}

	return %source.ChangeMatter(%transferMatter, (%transferAmount - %transferLeft) * -1, %type);
}

package EOTW_Pipes {
	function fxDtsBrick::onPlant(%obj, %b)
	{
		parent::onPlant(%obj, %b);
		
		%obj.LoadPipeData();

		if (%obj.getDatablock().matterSize > 0)
			RefreshAdjacentExtractors(%obj.getWorldBox());
	}
	function fxDtsBrick::onLoadPlant(%obj, %b)
	{
		parent::onLoadPlant(%obj, %b); //LoadPlant is handled in Support_PowerNet
	}
	function fxDTSBrickData::onDeath(%data, %this)
	{
		Parent::onDeath(%data, %this);

		if (%data.isMatterPipe)
			RefreshAdjacentPipes(%this.getWorldBox());
		if (%data.matterSize > 0)
			RefreshAdjacentExtractors(%this.getWorldBox());
	}
	function fxDTSBrickData::onRemove(%data, %this)
	{
		Parent::onRemove(%data, %this);

		if (%data.isMatterPipe)
			RefreshAdjacentPipes(%this.getWorldBox());
		if (%data.matterSize > 0)
			RefreshAdjacentExtractors(%this.getWorldBox());
	}
};
activatePackage("EOTW_Pipes");

function fxDtsBrick::LoadPipeData(%obj)
{
	%data = %obj.getDatablock();
	if (!%data.isMatterPipe)
		return;

	%obj.findAdjacentMatterBricks();

	%adj = findAdjacentPipes(%obj, "all", "", 0);
	if (%adj.count > 0)
	{
		//We found another pipe. Lets connect our new pipe to the others.
		//We can inherit the biggest pipenet found and overtake any others that are connected.
		//The findAdjacentPipes function should give us pipes that already have a pipenet on them.

		//TODO: Make pipenet to keep the one with the most items.
		%firstPipe = %adj.array[0];
		if (isObject(%firstPipe.pipeNet))
		{
			%firstPipe.pipeNet.addPipe(%obj);

			for (%i = 1; %i < %adj.count; %i++)
			{
				%pipe = %adj.array[%i];
				//%obj.pipeNet.overTakePipeNet(%pipe.pipeNet);
				%obj.pipeNet.AddPipe(%pipe);
				%pipe.SpreadPipeNet();
			}

			return;
		}
	}

	//No pipes found. Lets just make our own pipenet.
	%pipeGroup = new ScriptObject(pipeGroup);
	%pipeGroup.AddPipe(%obj);
}

function fxDtsBrick::findAdjacentMatterBricks(%obj)
{
	%boxes = findAdjacentMatterBricks(%obj, "all", "");
	%obj.adjacentMatterBricks = "";
	for (%i = 0; %i < %boxes.count; %i++)
		%obj.adjacentMatterBricks = trim(%obj.adjacentMatterBricks TAB %boxes.array[%i]);
}

function RefreshAdjacentExtractors(%boundbox)
{
	%adj = findAdjacentPipes("","all","extractor\tconnector", %boundbox);
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
	%adj = findAdjacentPipes("","all","", %boundbox);
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

function fxDtsBrick::SpreadPipeNet(%obj)
{
	%adj = findAdjacentPipes(%obj, "all", "", 0);
	if (%adj.count > 0)
	{
		for (%i = 0; %i < %adj.count; %i++)
		{
			%pipe = %adj.array[%i];
			if (%pipe.pipeNet != %obj.pipeNet)
			{
				//%obj.pipeNet.overTakePipeNet(%pipe.pipeNet);
				%obj.pipeNet.AddPipe(%pipe);
				%pipe.SpreadPipeNet();
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
	
	InitContainerBoxSearch(%boxcenter,%boxsize,$TypeMasks::fxBrickObjectType | $TypeMasks::StaticShapeObjectType);
	while(isObject(%obj = containerSearchNext()))
	{
		if(%obj != %filterbrick)
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
			%center = (%xcenter SPC %ycenter SPC (GetWord(%worldbox,5) + 0.10));
			%size = ((%xsize - %lateralcutoff) SPC (%ysize - %lateralcutoff) SPC %zsize - %verticalcutoff );
			%boxes = GetPipesInBox(%center,%size,%type,%Obj);
		case "zneg":
			%center = (%xcenter SPC %ycenter SPC (GetWord(%worldbox,2) - 0.10));
			%size = ((%xsize - %lateralcutoff) SPC (%ysize - %lateralcutoff) SPC %zsize - %verticalcutoff );
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
	
	InitContainerBoxSearch(%boxcenter,%boxsize,$TypeMasks::fxBrickObjectType | $TypeMasks::StaticShapeObjectType);
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