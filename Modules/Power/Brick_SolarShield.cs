datablock fxDTSBrickData(brickEOTWSolarShieldProjectorData)
{
	brickFile = "./Shapes/ShieldProjector.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Solar Shield Projector";
	iconName = "./Icons/ShieldProjector";

    isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWSolarShieldProjectorData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Energium" TAB 256 TAB "Teflon";
$EOTW::BrickDescription["brickEOTWSolarShieldProjectorData"] = "Creates a sun proof bubble shield. Turns off automatically at night.";

function brickEOTWSolarShieldProjectorData::onTick(%this, %obj)
{
	if ($EOTW::Time < 12 && %obj.attemptPowerDraw($EOTW::PowerLevel[2] >> 1))
	{
		if (!isObject(%obj.shieldShape))
		{
			%obj.shieldShape = new StaticShape()
			{
				datablock = SolarShieldProjectorShieldShape;
				position = %obj.getPosition();
				scale = "1 1 1";
			};
			%obj.shieldShape.setTransform(%obj.getPosition());
			%obj.shieldShape.EOTW_SetShieldLevel(18);
			
			//serverplay3D(shieldPowerUpSound, %obj.getPosition());
		}

		//Using schedules to make sure we stop the projector if we get shutoff via events
		//Or if the bricks get hammered.
		cancel(%obj.shieldShape.shieldSchedule);
		%obj.shieldShape.shieldSchedule = %obj.shieldShape.schedule(2000, "EOTW_SolarShieldProjectorEnd");
	}
}

function brickEOTWSolarShieldProjectorData::onPlant(%this,%brick)
{
	Parent::onPlant(%this,%brick);

	if (!isObject(SolarShieldGroup))
		new SimSet(SolarShieldGroup);
		
	SolarShieldGroup.add(%brick);
}

function brickEOTWSolarShieldProjectorData::onLoadPlant(%this,%brick)
{
	Parent::onLoadPlant(%this,%brick);
	
	if (!isObject(SolarShieldGroup))
		new SimSet(SolarShieldGroup);

	SolarShieldGroup.add(%brick);
}

datablock StaticShapeData(SolarShieldProjectorShieldShape)
{
	category = "Shapes";
	shapeFile = "./Shapes/shield.dts";
};

function StaticShape::EOTW_SolarShieldProjectorEnd(%obj)
{
	%obj.delete();
}

function StaticShape::EOTW_SetShieldLevel(%obj, %Level)
{
	%obj.level = %level;
	%obj.setScale(vectorScale("1 1 1", 1.5 + %obj.level / 5));
}

function StaticShape::EOTW_GetShieldRadius(%obj)
{
	%obj.level = %obj.level + 0;

	return (4 * (2.4 + %obj.level / 4));
}