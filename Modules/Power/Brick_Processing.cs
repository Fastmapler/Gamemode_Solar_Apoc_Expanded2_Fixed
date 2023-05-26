datablock AudioProfile(AlloyForgeLoopSound)
{
   filename    = "./Sounds/AlloyForgeLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWAlloyForgeData)
{
	brickFile = "./Shapes/AlloyForge.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Alloy Forge";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/AlloyForge";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Alloying";
	processSound = AlloyForgeLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWAlloyForgeData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 256 TAB "Quartz" TAB 128 TAB "Silver";
$EOTW::BrickDescription["brickEOTWAlloyForgeData"] = "Uses different metals and materials to create alloys.";

$EOTW::BrickUpgrade["brickEOTWAlloyForgeData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWAlloyForgeData", 0] = 256 TAB "Steel" TAB 256 TAB "Electrum" TAB 128 TAB "Red Gold";
$EOTW::BrickUpgrade["brickEOTWAlloyForgeData", 1] = 512 TAB "PlaSteel" TAB 512 TAB "Energium" TAB 512 TAB "Naturum";

function brickEOTWAlloyForgeData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWAlloyForgeData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(FurnaceLoopSound)
{
   filename    = "./Sounds/RefineryLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWFurnaceData)
{
	brickFile = "./Shapes/Refinery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Furnace";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Refinery";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Burning";
	processSound = FurnaceLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWFurnaceData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Steel" TAB 256 TAB "Lead" TAB 256 TAB "Red Gold";
$EOTW::BrickDescription["brickEOTWFurnaceData"] = "Cooks materials in a controlled environment into something else.";

$EOTW::BrickUpgrade["brickEOTWFurnaceData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWFurnaceData", 0] = 1024 TAB "Granite" TAB 512 TAB "Steel" TAB 128 TAB "Brimstone";
$EOTW::BrickUpgrade["brickEOTWFurnaceData", 1] = 2048 TAB "Granite" TAB 512 TAB "PlaSteel" TAB 128 TAB "Naturum";

function brickEOTWFurnaceData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWFurnaceData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(MatterReactorSound)
{
   filename    = "./Sounds/MatterReactorLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWMatterReactorData)
{
	brickFile = "./Shapes/MatterReactor.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Matter Reactor";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/MatterReactor";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 3;
	matterSlots["Output"] = 2;
	isProcessingMachine = true;
	processingType = "Chemistry";
	processSound = MatterReactorSound;
};
$EOTW::CustomBrickCost["brickEOTWMatterReactorData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Steel" TAB 256 TAB "Lead" TAB 256 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWMatterReactorData"] = "Takes in various materials to produce chemicals.";

$EOTW::BrickUpgrade["brickEOTWMatterReactorData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWMatterReactorData", 0] = 512 TAB "Silver" TAB 512 TAB "Plastic" TAB 128 TAB "Ethylene";
$EOTW::BrickUpgrade["brickEOTWMatterReactorData", 1] = 1024 TAB "Silver" TAB 512 TAB "Teflon" TAB 128 TAB "Epoxy";

function brickEOTWMatterReactorData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWMatterReactorData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(SeperatorSound)
{
   filename    = "./Sounds/SeperatorLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWSeperatorData)
{
	brickFile = "./Shapes/Seperator.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Seperator";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Seperator";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 3;

	isProcessingMachine = true;
	processingType = "Seperation";
	processSound = SeperatorSound;
};
$EOTW::CustomBrickCost["brickEOTWSeperatorData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Plastic" TAB 256 TAB "Electrum" TAB 256 TAB "Red Gold";
$EOTW::BrickDescription["brickEOTWSeperatorData"] = "Seperates specific materials into useful components.";

$EOTW::BrickUpgrade["brickEOTWSeperatorData", "MaxTier"] = 1;
$EOTW::BrickUpgrade["brickEOTWSeperatorData", 0] = 512 TAB "Lead" TAB 256 TAB "Rubber" TAB 256 TAB "Sturdium";

function brickEOTWSeperatorData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWSeperatorData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(BrewerySound)
{
   filename    = "./Sounds/BreweryLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWBreweryData)
{
	brickFile = "./Shapes/Brewery.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Brewery";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Brewery";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 4;
	matterSlots["Output"] = 2;

	isProcessingMachine = true;
	processingType = "Brewing";
	processSound = BrewerySound;
};
$EOTW::CustomBrickCost["brickEOTWBreweryData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 128 TAB "Red Gold" TAB 128 TAB "Electrum";
$EOTW::BrickDescription["brickEOTWBreweryData"] = "Brews potion fluid from the combination of various materials.";

$EOTW::BrickUpgrade["brickEOTWBreweryData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWBreweryData", 0] = 512 TAB "Biomass" TAB 512 TAB "Flesh" TAB 128 TAB "Ethylene";
$EOTW::BrickUpgrade["brickEOTWBreweryData", 1] = 2048 TAB "Lead" TAB 512 TAB "Boss Essence" TAB 128 TAB "Epoxy";

function brickEOTWBreweryData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWBreweryData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

datablock AudioProfile(VoidDrillSound)
{
   filename    = "./Sounds/VoidDrillLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWVoidDrillData)
{
	brickFile = "./Shapes/VoidDrill.blb";
	category = "Solar Apoc";
	subCategory = "Processors";
	uiName = "Void Drill";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/VoidDrill";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Drilling";
	processSound = VoidDrillSound;
};
$EOTW::CustomBrickCost["brickEOTWVoidDrillData"] = 1.00 TAB "7a7a7aff" TAB 666 TAB "Boss Essence" TAB 1920 TAB "Steel" TAB 12800 TAB "Granite";
$EOTW::BrickDescription["brickEOTWVoidDrillData"] = "Uses Boss Essence and tons of power to synthesize most raw materials.";

$EOTW::BrickUpgrade["brickEOTWVoidDrillData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWVoidDrillData", 1] = 6660 TAB "Boss Essence" TAB 1280 TAB "Fluorspar" TAB 1280 TAB "Uraninite";
$EOTW::BrickUpgrade["brickEOTWVoidDrillData", 2] = 22200 TAB "Boss Essence" TAB 1280 TAB "Diamond" TAB 1280 TAB "Sturdium";

function brickEOTWVoidDrillData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWVoidDrillData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}

function fxDtsBrick::runProcessingTick(%obj)
{
	if (isObject(%obj.processingRecipe))
	{
		%recipe = %obj.processingRecipe;
		%data = %obj.getDatablock();

		for (%i = 0; (%cost = %recipe.input[%i]) !$= ""; %i++)
		{
			if (%obj.getMatter(getField(%cost, 0), "Input") < getField(%cost, 1))
			{
				//Couldn't find all needed materials, reset the recipe progress for now.
				//talk("Not enough " @ getField(%cost, 0) @ ", need " @ getField(%cost, 1));
				%obj.recipeProgress = 0;
				return;
			}
		}

		if (%obj.recipeProgress < %recipe.powerCost && %obj.attemptPowerDraw(%recipe.powerDrain))
			%obj.recipeProgress += %recipe.powerDrain;

		if (%obj.recipeProgress >= %recipe.powerCost)
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
				return;

			%obj.recipeProgress = 0;
			
			for (%i = 0; %recipe.input[%i] !$= ""; %i++)
				%obj.changeMatter(getField(%recipe.input[%i], 0), getField(%recipe.input[%i], 1) * -1, "Input");
			
			for (%i = 0; %recipe.output[%i] !$= ""; %i++)
				%obj.changeMatter(getField(%recipe.output[%i], 0), getField(%recipe.output[%i], 1), "Output");
		}
	}
}

function ServerCmdSR(%client) { ServerCmdSetRecipe(%client); }
function ServerCmdSetRecipe(%client)
{
	if(!isObject(%player = %client.player) || !isObject(%hit = %player.whatBrickAmILookingAt()) || %hit.getDatablock().processingType $= "")
		return;

	if (getTrustLevel(%client, %hit) < 2 && getBrickgroupFromObject(%hit).bl_id != 888888)
	{
		%client.chatMessage(%hit.getGroup().name @ " does not trust you enough to do that!");
		return;
	}
	
	cancel(%player.MatterBlockInspectLoop);

	%data = %hit.getDatablock();
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

		if (%recipe.recipeType !$= %data.processingType || %recipe.minTier > %brick.upgradeTier)
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

	%client.overRideBottomPrint(getRecipeText(getField(%bsm.entry[%client.selId], 1)));

	%client.recipeCheckSchedule = %client.schedule(100, "ShowSelectedRecipe");
}