datablock fxDTSBrickData(brickEOTWOilGeyserData)
{
	brickFile = "./Shapes/OilGeyser.blb";
	category = "";
	subCategory = "";
	uiName = "Oil Geyser";
	//iconName = "";
};
$EOTW::BrickBlacklist["brickEOTWOilGeyserData"] = true;

function brickEOTWOilGeyserData::onPlant(%this, %obj)
{
	if (!isObject(OilGeyserSet))
		new SimSet(OilGeyserSet);

	%obj.OilCapacity = getRandom(16, 32) * 16;
	BrickGroup_1337.add(%obj);
	OilGeyserSet.add(%obj);
	%obj.despawnLife = getRandom(300, 500);

	Parent::onPlant(%this, %obj);
}

function SpawnOilGeyser(%eye)
{
	if (!isObject(OilGeyserSet))
		new SimSet(OilGeyserSet);

	if (%eye $= "")
		%eye = (getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2))) SPC (getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3))) SPC 1000;
		
	%dir = "0 0 -1";
	%for = "0 1 0";
	%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
	%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
	%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
	%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
	if(isObject(%hit = firstWord(%ray)) && (getWord(%pos, 2) > $EOTW::LavaHeight + 2))
	{
		if (%hit.getClassName() !$= "FxPlane" && strPos(%hit.getDatablock().uiName,"Ramp") > -1)
			%pos = vectorAdd(%pos,"0 0 0.4");
		
		%pos = vectorAdd(%pos,"0 0 0.2");

        %output = CreateBrick(BrickGroup_1337, brickEOTWOilGeyserData, %pos, getColorFromHex(getMatterType("Crude Oil").color), getRandom(0, 3));
		%brick = getField(%output, 0);
		if(getField(%output, 1)) { %brick.delete(); return; }
	}
	else if (getWord(%eye, 2) == 1000)
		SpawnOilGeyser(getWord(%eye, 0) SPC getWord(%eye, 1) SPC 500);
}
