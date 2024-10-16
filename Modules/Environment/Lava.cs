datablock StaticShapeData(EOTWStaticLava)
{
	shapeFile = "./Shapes/Lava.dts";
};

function CreateLavaStatic()
{
	if (isObject($EOTW::LavaStatic))
		$EOTW::LavaStatic.delete();
	
	%shape = new StaticShape(LavaStatic)
	{
		dataBlock = EOTWStaticLava;
	};
	
	MissionCleanup.add(%shape);
	
	%shape.setNodeColor("ALL", "1 0.5 0 1");
	%shape.setTransform("0 0 0");
	%shape.setScale("64 64 0.1");
	
	//talk(%shape);
	
	$EOTW::LavaStatic = %shape;
}
schedule(100, 0, "CreateLavaStatic");

function setLavaHeight(%height)
{
	$EOTW::LavaHeight = %height;

	if (%height <= 0)
		setNewWater("NONE");
	else
		setNewWater("Add-Ons/Water_BrickLava/bricklava.water");
	
	if (!isObject($EOTW::LavaStatic))
		CreateLavaStatic();
	
	$EOTW::LavaStatic.setTransform("0 0 " @ %height);

	if (isObject(EnvMaster))
	{
		servercmdEnvGui_SetVar(EnvMaster, "WaterHeight", %height);
		servercmdEnvGui_SetVar(EnvMaster, "WaterColor", "1 0.5 0 0.05");
	}
}

function SetupVehicleLavaImmunity()
{
	if (isObject(StandupJetskiArmor))
		StandupJetskiArmor.lavaImmune = true;
	if (isObject(JetskiArmor))
		JetskiArmor.lavaImmune = true;
	if (isObject(LifeBoatArmor))
		LifeBoatArmor.lavaImmune = true;
	if (isObject(MiniSpeedBoatArmor))
		MiniSpeedBoatArmor.lavaImmune = true;
	if (isObject(Wave_RacerArmor))
		Wave_RacerArmor.lavaImmune = true;
}
schedule(100, 0, "SetupVehicleLavaImmunity");

function SetupVehicleSunImmunity()
{
	if (isObject(Trolley))
	{
		trolley.sunImmune = true;
		trolley.sunProofSlots = "0 1 2 3 4 5 6 7 8 9";
	}
	if (isObject(SingleTrain))
	{
		SingleTrain.sunImmune = true;
		SingleTrain.sunProofSlots = "0 1 2 3";
	}
}
schedule(100, 0, "SetupVehicleSunImmunity");