datablock fxDTSBrickData(brickEOTWUGPipeInputData)
{
	brickFile = "./Shapes/pump.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "UG Input Pipe";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Matter/Icons/PumpIcon";

    hasInventory = true;
    matterSize = 64;
	matterSlots["Input"] = 3;

	isPowered = true;
	powerType = "Logistic";
};
$EOTW::CustomBrickCost["brickEOTWUGPipeInputData"] = 1.00 TAB "" TAB 128 TAB "Sturdium" TAB 128 TAB "Piping";
$EOTW::BrickDescription["brickEOTWUGPipeInputData"] = "Pushes matter to output underground pipes. Define networks based on brick name.";

//Based on teledoor code
function brickEOTWUGPipeInputData::onTick(%this, %obj)
{
	%inputMatter = getField(%obj.matter["Input", 0], 0);
	%inputAmount = getField(%obj.matter["Input", 0], 1);

	//We dont have anything to transfer in the first place
	if (!isObject(getMatterType(%inputMatter)) || %inputAmount == 0)
		return;

	//get nt object array stuff
	%group = %obj.getGroup();
	%name  = %obj.getName(); 
	%count = %group.NTObjectCount[%name];

	//no other bricks of this name, early out
	if(%count <= 1)
		return;

	//find ourselves in ntobject array, go forward from there
	%ourIdx = -1;
	for(%i = 0; %i < %count; %i++)
	{
		%targetObj = %group.NTObject[%name, %i];
		if(%targetObj == %obj)
		{
			%ourIdx = %i;
			break;
		}
	}
	if(%ourIdx == -1)
	{
		error("ERROR: brickEOTWUGPipeInputData::onTick(" @ %data @ ", " @ %obj @ ") - could not find ourselves in named target list");
		return;
	}

	//get the next door   
	%targetIdx = %ourIdx + 1;
	%targetIdx = %targetIdx % %count;
	%targetBrick = %group.NTObject[%name, %targetIdx];

	if(%targetBrick.getDatablock().getId() != brickEOTWUGPipeOutputData.getId())
	{
		//user has named other bricks the same thing as their pipe
		//go forward until we find a pipe 
		for(%i = 0; %i < %count; %i++)
		{
			%testIdx = (%targetIdx + %i) % %count;
			%targetBrick = %group.NTObject[%name, %testIdx];
			if(%targetBrick.getDatablock().getId() == brickEOTWUGPipeOutputData.getId() && %targetBrick != %obj)
			break;
		}

		//did we find anything?
		if(%targetBrick.getDatablock().getId() != brickEOTWUGPipeOutputData.getId())
			return;
	}

	//teleport matter
	for (%i = 0; %i < %this.matterSlots["Input"]; %i++)
	{
		%inputMatter = getField(%obj.matter["Input", %i], 0);
		%inputAmount = getField(%obj.matter["Input", %i], 1);

		//We dont have anything to transfer in the first place
		if (!isObject(getMatterType(%inputMatter)) || %inputAmount == 0)
			return;

		%moveAmount = %targetBrick.changeMatter(%inputMatter, %inputAmount, "Output");

		if (%moveAmount > 0)
		{
			%obj.changeMatter(%inputMatter, %moveAmount * -1, "Input");
			%obj.attemptPowerDraw(0);
		}
	}
}

function brickEOTWUGPipeInputData::onPlant(%data, %obj)
{
   %client = %obj.client;

   //help the client plant a matching entrance/exit pair
   if(%client.currPipeName $= "")
   {
      %name = "Pipe_" @ getSubStr(sha1(%obj.getTransform()), 0, 5);
      %client.currPipeName = %name;
   }
   %obj.setNTObjectName(%client.currPipeName);
}

datablock fxDTSBrickData(brickEOTWUGPipeOutputData)
{
	brickFile = "./Shapes/pump.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "UG Output Pipe";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Matter/Icons/PumpIcon";

    hasInventory = true;
    matterSize = 64;
	matterSlots["Output"] = 3;
};
$EOTW::CustomBrickCost["brickEOTWUGPipeOutputData"] = 1.00 TAB "" TAB 128 TAB "Diamond" TAB 128 TAB "Piping";
$EOTW::BrickDescription["brickEOTWUGPipeOutputData"] = "Recieves matter from UG input pipes. Define networks based on brick name.";

function brickEOTWUGPipeOutputData::onPlant(%data, %obj)
{
   %client = %obj.client;

   //help the client plant a matching entrance/exit pair
   if(%client.currPipeName $= "")
   {
      %name = "Pipe_" @ getSubStr(sha1(%obj.getTransform()), 0, 5);
      %client.currPipeName = %name;
   }
   %obj.setNTObjectName(%client.currPipeName);
}