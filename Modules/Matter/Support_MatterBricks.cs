datablock fxDTSBrickData(brickEOTWMatterBin1Data)
{
	brickFile = "./Shapes/BarrelThin.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "Matter Bin I";
	collisionShapeName = "./Shapes/BarrelThin.dts";

	hasPrint = 1;
	printAspectRatio = "1x1";

	//iconName = "";

    hasInventory = true;
    matterSize = 512;
	matterSlots["Buffer"] = 2;
};
$EOTW::CustomBrickCost["brickEOTWMatterBin1Data"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterBin1Data"] = "A matter bin used for binning matter. Has 1 slot, 512u of space.";

datablock fxDTSBrickData(brickEOTWMatterBin2Data)
{
	brickFile = "./Shapes/barrelThick.blb";
	category = "Solar Apoc";
	subCategory = "Storage";
	uiName = "Matter Bin II";
	collisionShapeName = "./Shapes/barrelThick.dts";

	hasPrint = 1;
	printAspectRatio = "1x1";

	//iconName = "";

    hasInventory = true;
    matterSize = 2048;
	matterSlots["Buffer"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWMatterBin2Data"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 64 TAB "Copper" TAB 96 TAB "Lead";
$EOTW::BrickDescription["brickEOTWMatterBin2Data"] = "A larger matter bin used for binning more matter. Has 1 slot, 2048u of space.";

function fxDtsBrick::ChangeMatter(%obj, %matterName, %amount, %type)
{
	if(!isObject(GetMatterType(%matterName)))
        return 0;

    %data = %obj.getDatablock();
	%amount = mRound(%amount);
	
	//Try to add to existing matter.
	//We will only allow a unique matter to occupy 1 slot in its slot type.
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matter = %obj.matter[%type, %i];
		
		if (getField(%matter, 0) $= %matterName)
		{
			%testAmount = getField(%matter, 1) + %amount;
			%change = %amount;
			
			if (%testAmount > %data.matterSize)
				%change = %data.matterSize - getField(%matter, 1);
			else if (%testAmount < 0)
				%change = getField(%matter, 1) * -1;
			
			%obj.matter[%type, %i] = getField(%matter, 0) TAB (getField(%matter, 1) + %change);
			
			if ((getField(%matter, 1) + %change) <= 0)
				%obj.matter[%type, %i] = "";
			
			return %change;
		}
	}
	
	//So we don't remove from an empty slot, somehow
	if (%amount <= 0)
		return 0;
	
	//Double pass to add to an empty slot.
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matter = %obj.matter[%type, %i];
		
		if (getField(%matter, 0) $= "")
		{
			%change = %amount > %data.matterSize ? %data.matterSize : %amount;
			%obj.matter[%type, %i] = %matterName TAB %change;
			
			return %change;
		}
	}
	
	return 0;
}

function fxDtsBrick::GetMatter(%obj, %matter, %type)
{
	%data = %obj.getDatablock();
	for (%i = 0; %i < %data.matterSlots[%type]; %i++)
	{
		%matterData = %obj.matter[%type, %i];
		
		if (getField(%matterData, 0) $= %matter)
			return getField(%matterData, 1);
	}
	
	return 0;
}