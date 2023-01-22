function SetupMatterData()
{
	if (isObject(MatterData))
	{
		MatterData.deleteAll();
		MatterData.delete();
	}

	new SimSet(MatterData)
	{
		//Buildable Material
		new ScriptObject(MatterType) { name="Wood";			color="75502eff";	spawnWeight=30;	spawnVeinSize=8;	spawnValue=256;	collectTime=2000;	placable=true;	health=1.0;	heatCapacity=25;	meteorImmune=false;	gatherableDB="brickEOTWGatherableBasicData";									helpText="A primitive, basic building material. High quanity but exceptionally low quality building material. Will burn on hotter days."; };
		new ScriptObject(MatterType) { name="Granite";		color="c1a872ff";	spawnWeight=60;	spawnVeinSize=4;	spawnValue=128;	collectTime=4000;	placable=true;	health=2.0;	heatCapacity=50;	meteorImmune=false;	gatherableDB="brickEOTWGatherableBasicData"; 									helpText="One of the few building materials. Will not melt in the sunlight but is still vulnerable to meteor strikes.";	};
		new ScriptObject(MatterType) { name="Iron";			color="7a7a7aff";	spawnWeight=40;	spawnVeinSize=6;	spawnValue=128;	collectTime=12000;	placable=true;	health=4.0;	heatCapacity=60;	meteorImmune=true;	gatherableDB="brickEOTWGatherableMetalData";									helpText="The bread and butter of civilization and technology. Can be used to build. Immune to meteors and sunlight.";	};
		new ScriptObject(MatterType) { name="Quartz";		color="181c26a8";	spawnWeight=20;	spawnVeinSize=3;	spawnValue=128;	collectTime=8000;	placable=true;	health=3.0;	heatCapacity=50;	meteorImmune=true;	gatherableDB="brickEOTWGatherableCrystalData"; 									helpText="Transparent building material, allowing transparent paint colors. Immune to both sunlight and meteors.";	};
		new ScriptObject(MatterType) { name="Sturdium";		color="646defff";	spawnWeight=02;	spawnVeinSize=2;	spawnValue=128;	collectTime=24000;	placable=true;	health=999;	heatCapacity=90;	meteorImmune=true;	gatherableDB="brickEOTWGatherableMetalData";									helpText="An ultra rare strange blue shaded metal with almost magic-like properties. Immune to meteors and sunlight and is used for high tier crafting.";	};
		//Growable Organics
		new ScriptObject(MatterType) { name="Moss";			color="446942ff";	spawnWeight=10;	spawnVeinSize=2;	spawnValue=128;	collectTime=1000;	gatherableDB="brickEOTWGatherableBasicData";																										helpText="Strange tasting moss. Grows horizontally on wood and itself when placed."; };
		new ScriptObject(MatterType) { name="Vines";		color="264b38ff";	spawnWeight=10;	spawnVeinSize=2;	spawnValue=128;	collectTime=1500;	gatherableDB="brickEOTWGatherableBasicData";																										helpText="Thick sturdy vines full of biomatter. Grows downward on wood and itself when placed.";	 };
		new ScriptObject(MatterType) { name="Cacti";		color="56643bff";	spawnWeight=10;	spawnVeinSize=2;	spawnValue=128;	collectTime=2000;	gatherableDB="brickEOTWGatherableBasicData";																										helpText="An exotic type of cacti. Grows upward on wood or itself.";	 };
		//Basic Gatherable Materials
		new ScriptObject(MatterType) { name="Copper";		color="d36b04ff";	spawnWeight=30;	spawnVeinSize=2;	spawnValue=128;	collectTime=13000;	gatherableDB="brickEOTWGatherableMetalData";																										helpText="One of the variant metals, primarily used for electrical applications."; };
		new ScriptObject(MatterType) { name="Silver";		color="e0e0e0ff";	spawnWeight=15;	spawnVeinSize=4;	spawnValue=128;	collectTime=14000;	gatherableDB="brickEOTWGatherableMetalData";	 																									helpText="A variant of metal with uses in machine construction.";	};
		new ScriptObject(MatterType) { name="Lead";			color="533d60ff";	spawnWeight=20;	spawnVeinSize=4;	spawnValue=128;	collectTime=15000;	gatherableDB="brickEOTWGatherableMetalData";					 																					helpText="Tastes sweet, must be edible. Used for matter transfer and machine construction.";	};
		new ScriptObject(MatterType) { name="Gold";			color="e2af14ff";	spawnWeight=10;	spawnVeinSize=2;	spawnValue=128;	collectTime=20000;	gatherableDB="brickEOTWGatherableMetalData";																										helpText="We're rich! This rather rare metal has important use in making higher tier metal alloys."; };
		new ScriptObject(MatterType) { name="Diamond";		color="00d0ffa8";	spawnWeight=04;	spawnVeinSize=1;	spawnValue=128;	collectTime=22000;	gatherableDB="brickEOTWGatherableCrystalData";	 																									helpText="Who knew carbon could be so rare and expensive. Has niche but useful uses in tools and Adamantine production.";	};
		//Complex Gatherable Materials
		new ScriptObject(MatterType) { name="Coal";			color="000000ff";	spawnWeight=30;	spawnVeinSize=4;	spawnValue=128;	collectTime=10000;	gatherableDB="brickEOTWGatherableBasicData";																										helpText="Burnable carbon useful in both fuel and steel production.";	};
		new ScriptObject(MatterType) { name="Crude Oil";	color="1c1108ff";																																																									helpText="Unrefined fossil fuels ready to be refined into valuable oil products.";	};
		new ScriptObject(MatterType) { name="Fluorspar";	color="1f568cff";	spawnWeight=05;	spawnVeinSize=4;	spawnValue=128;	collectTime=10000;	gatherableDB="brickEOTWGatherableCrystalData";																										helpText="A special material with some useful applications in lategame materials.";	};
		new ScriptObject(MatterType) { name="Uraninite";	color="007c3fff";	spawnWeight=10;	spawnVeinSize=2;	spawnValue=128;	collectTime=18000;	gatherableDB="brickEOTWGatherableCrystalData";																										helpText="Spicy rocks which can be further refined into uranium.";	};
	};
	
	$EOTW::PlacableList = "";
	for (%i = 0; %i < MatterData.getCount(); %i++)
		if (MatterData.getObject(%i).placable)
			$EOTW::PlacableList = $EOTW::PlacableList TAB MatterData.getObject(%i).name;
	$EOTW::PlacableList = trim($EOTW::PlacableList);
	
	$EOTW::MatSpawnWeight = 0;
	$EOTW::MatSpawnList = "";
	for (%i = 0; %i < MatterData.getCount(); %i++)
	{
		if (MatterData.getObject(%i).spawnWeight > 0)
		{
			$EOTW::MatSpawnList = $EOTW::MatSpawnList TAB MatterData.getObject(%i).name;
			$EOTW::MatSpawnWeight += MatterData.getObject(%i).spawnWeight;
		}
	}
	$EOTW::MatSpawnList = trim($EOTW::MatSpawnList);

	schedule(10, 0, "EOTWbsm_PopulateRecipesMenu");
}
SetupMatterData();