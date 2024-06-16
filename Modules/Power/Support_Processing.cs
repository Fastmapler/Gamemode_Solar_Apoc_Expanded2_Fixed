function fxDtsBrick::getAutoRecipe(%obj)
{
	//%obj.processingRecipe = "";
	//%obj.recipeProgress = 0;
	%data = %obj.getDataBlock();

	for (%i = 0; %i < RecipeData.getCount(); %i++)
	{
		%recipe = RecipeData.getObject(%i);

		if (%recipe.recipeType !$= %data.processingType || %recipe.minTier > %brick.upgradeTier)
			continue;

		%fail = false;
		for (%j = 0; %recipe.input[%j] !$= ""; %j++)
		{
			%input = %recipe.input[%j];
			%matterName = getField(%input, 0);
			%matterCost = getField(%input, 1);
			if (%obj.GetMatter(%matterName, "Input") < %matterCost)
			{
				%fail = true;
				break;
			}
		}

		if (!%fail && %obj.processingRecipe.getId() != %recipe)
		{
			%obj.processingRecipe = %recipe.getName();
			%obj.recipeProgress = 0;
			break;
		}
	}
}

function fxDtsBrick::runProcessingTick(%obj)
{
	if (isObject(%recipe = %obj.processingRecipe))
	{
		%data = %obj.getDatablock();
		for (%i = 0; (%cost = %recipe.input[%i]) !$= ""; %i++)
		{
			if (%obj.getMatter(getField(%cost, 0), "Input") < getField(%cost, 1))
			{
				//Couldn't find all needed materials, reset the recipe progress for now.
				//talk("Not enough " @ getField(%cost, 0) @ ", need " @ getField(%cost, 1));
				%obj.recipeProgress = 0;

				if (%data.automaticRecipe)
					%obj.getAutoRecipe();

				return;
			}
		}

		%powerCost = getRecipePowerCost(%recipe);

		if (%obj.recipeProgress < %powerCost && %obj.attemptPowerDraw(%recipe.powerDrain))
		{
			%efficency = %data.powerEfficiency > 0 ? %data.powerEfficiency : 1;
			%obj.recipeProgress += %recipe.powerDrain * %efficency * ((1 + %obj.upgradeTier) - %recipe.minTier);
		}

		if (%obj.recipeProgress >= %powerCost)
		{
			for (%parallel = 0; %parallel < 1 + %obj.upgradeTier - %recipe.minTier; %parallel++)
			{
				for (%k = 0; %recipe.output[%k] !$= ""; %k++)
				{
					%matter = getField(%recipe.output[%k], 0);
					%amount = getField(%recipe.output[%k], 1);
					if (%obj.getMatter(%matter, "Output") + %amount > %data.matterSize || (%obj.getMatter(%matter, "Output") == 0 && %obj.getEmptySlotCount("Output") == 0))
					{
						%craftFail = true;
						return;
					}
				}
			
				if (%craftFail)
					break;

				%obj.recipeProgress = 0;
				
				for (%i = 0; %recipe.input[%i] !$= ""; %i++)
					%obj.changeMatter(getField(%recipe.input[%i], 0), getField(%recipe.input[%i], 1) * -1, "Input");
				
				for (%i = 0; %recipe.output[%i] !$= ""; %i++)
					%obj.changeMatter(getField(%recipe.output[%i], 0), getField(%recipe.output[%i], 1), "Output");
			}
		}
	}
}

function ServerCmdSR(%client,%a1,%a2,%a3,%a4,%a5) { ServerCmdSetRecipe(%client,%a1,%a2,%a3,%a4,%a5); }
function ServerCmdSetRecipe(%client,%a1,%a2,%a3,%a4,%a5)
{
	if(!isObject(%player = %client.player) || !isObject(%hit = %player.whatBrickAmILookingAt()) || %hit.getDatablock().processingType $= "")
		return;

	if (getTrustLevel(%client, %hit) < 2 && getBrickgroupFromObject(%hit).bl_id != 888888)
	{
		%client.chatMessage(%hit.getGroup().name @ " does not trust you enough to do that!");
		return;
	}

	%data = %hit.getDatablock();
	%search = getSafeVariableName(trim("Recipe" SPC %a1 SPC %a2 SPC %a3 SPC %a4 SPC %a5));
	if(%search !$= "Recipe")
	{
		%group = RecipeData;
		%count = %group.getCount();
		for (%i = 0; %i < %count; %i++)
		{
			%recipe = %group.getObject(%i);
			if (%recipe.recipeType !$= %data.processingType)
			{
				continue;
			}

			if(stripos(%recipe.getName(),%search) == 0)
			{
				%hit.processingRecipe = %recipe.getName();
				%client.chatMessage("\c6You set the machine's recipe to \c3" @ cleanRecipeName(%recipe.getName()) @ "\c6.");
				return;
			}
		}
	}
	
	cancel(%player.MatterBlockInspectLoop);

	%bsm = getTempBSM("MM_bsmSetRecipe");
	%bsm.targetBrick = %hit;

	%client.brickShiftMenuEnd();
	%client.brickShiftMenuStart(%bsm);
    %client.SetRecipeUpdateInterface();
}

function GameConnection::SetRecipeUpdateInterface(%client)
{
	if (!isObject(%bsm = %client.brickShiftMenu) || %bsm.class !$= "MM_bsmSetRecipe" || !isObject(%brick = %bsm.targetBrick))
        return;

	%data = %brick.getDataBlock();

	for (%i = 0; %i < %bsm.entryCount; %i++)
        %bsm.entry[%i] = "";

    %bsm.entryCount = 0;

	%bsm.title = "<font:tahoma:16>\c3Set Machine Recipe...";
	%bsm.entry[%bsm.entryCount] = "[Clear]" TAB "CLEAR";
	%bsm.entryCount++;
	
	for (%i = 0; %i < RecipeData.getCount(); %i++)
	{
		%recipe = RecipeData.getObject(%i);

		if (%recipe.recipeType !$= %data.processingType)
			continue;

		if (%recipe.minTier > %brick.upgradeTier)
		{
			%bsm.title = "<font:tahoma:16>\c3Set Machine Recipe... \c7Unlock more recipes with the Upgrade Tool";
			continue;
		}

		%bsm.entry[%bsm.entryCount] = cleanRecipeName(%recipe.getName()) TAB %recipe.getName();
		%bsm.entryCount++;
	}
	
	%client.ShowSelectedRecipe();
}

function MM_bsmSetRecipe::onUserMove(%obj, %client, %id, %move, %val)
{
	if (isObject(%player = %client.player))
	{
		if(%move == $BSM::PLT)
		{
			if (%id $= "CLEAR")
			{
				%obj.targetBrick.processingRecipe = "";
				%client.chatMessage("\c6You clear the machine's recipe.");
			}
			else
			{
				%obj.targetBrick.processingRecipe = %id;
				%client.chatMessage("\c6You set the machine's recipe to \c3" @ cleanRecipeName(%id) @ "\c6.");
			}
			%obj.recipeProgress = 0;
			%client.brickShiftMenuEnd();
			return;
		}
		if (%move == $BSM::CLR)
		{
			%client.brickShiftMenuEnd();
			return;
		}
	}
	
	Parent::onUserMove(%obj, %client, %id, %move, %val);
}

function GameConnection::ShowSelectedRecipe(%client)
{
	cancel(%client.recipeCheckSchedule);

	if (!isObject(%bsm = %client.brickShiftMenu) || %bsm.class !$= "MM_bsmSetRecipe" || !isObject(%brick = %bsm.targetBrick))
        return;

	%passiveText = "";
	if (%brick.getDatablock().passivePower)
		%passiveText = "(EU passively supplied)";

	%client.overRideBottomPrint(getRecipeText(getField(%bsm.entry[%client.selId], 1)) SPC %passiveText);

	%client.recipeCheckSchedule = %client.schedule(100, "ShowSelectedRecipe");
}
