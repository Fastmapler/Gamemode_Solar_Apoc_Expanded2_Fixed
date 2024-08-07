function SetupMatterData()
{
	if (isObject(MatterData))
	{
		MatterData.deleteAll();
		MatterData.delete();
	}

	new SimSet(MatterData)
	{
		//Raw Ores
		new ScriptObject(MatterType) { name="Magnetite";	color="7a7a7aff";	spawnWeight=40;	spawnVeinSize=6;	spawnValue=128;	collectTime=8000;	gatherableDB="brickEOTWGatherableMetalData";	flotationChemical="Sulfuric Acid";	oreOutput="Iron";	};
		new ScriptObject(MatterType) { name="Sturdite";		color="646defff";	spawnWeight=02;	spawnVeinSize=2;	spawnValue=128;	collectTime=24000;	gatherableDB="brickEOTWGatherableMetalData";	flotationChemical="Hydrofluoric Acid";	oreOutput="Sturdium";	};
		new ScriptObject(MatterType) { name="Malachite";	color="d36b04ff";	spawnWeight=30;	spawnVeinSize=2;	spawnValue=128;	collectTime=12000;	gatherableDB="brickEOTWGatherableMetalData";	flotationChemical="Sulfuric Acid";	oreOutput="Copper";	};
		new ScriptObject(MatterType) { name="Acanthite";	color="e0e0e0ff";	spawnWeight=15;	spawnVeinSize=4;	spawnValue=128;	collectTime=12000;	gatherableDB="brickEOTWGatherableMetalData";	flotationChemical="Sulfuric Acid";	oreOutput="Silver";	};
		new ScriptObject(MatterType) { name="Anglesite";	color="533d60ff";	spawnWeight=20;	spawnVeinSize=2;	spawnValue=128;	collectTime=16000;	gatherableDB="brickEOTWGatherableMetalData";	flotationChemical="Ethanol";	oreOutput="Lead";	};
		new ScriptObject(MatterType) { name="Native Gold";	color="c1a872ff";	spawnWeight=10;	spawnVeinSize=4;	spawnValue=128;	collectTime=16000;	gatherableDB="brickEOTWGatherableMetalData";	flotationChemical="Ethanol";	oreOutput="Gold";	};
		new ScriptObject(MatterType) { name="Fluorspar";	color="507582ff";	spawnWeight=05;	spawnVeinSize=4;	spawnValue=128;	collectTime=20000;	gatherableDB="brickEOTWGatherableCrystalData";	flotationChemical="Hydrofluoric Acid";	oreOutput="Fluoric Dust";										helpText="Unrefined material with some useful applications in lategame materials.";	};
		new ScriptObject(MatterType) { name="Uraninite";	color="007c3fff";	spawnWeight=10;	spawnVeinSize=2;	spawnValue=128;	collectTime=20000;	gatherableDB="brickEOTWGatherableCrystalData";	flotationChemical="Hydrofluoric Acid";	oreOutput="Uranic Dust";											helpText="Spicy rocks which can be further refined into uranium.";	};
		//Raw Gatherables
		new ScriptObject(MatterType) { name="Wood";			color="75502eff";	spawnWeight=30;	spawnVeinSize=8;	spawnValue=256;	collectTime=2000;	placable=true;	health=1.0;	heatCapacity=10;	meteorImmune=false;	gatherableDB="brickEOTWGatherableBasicData";	fuelPower=4;					helpText="A primitive, weak basic building material. High quantity. Will burn on hot days."; };
		new ScriptObject(MatterType) { name="Granite";		color="c1a872ff";	spawnWeight=60;	spawnVeinSize=4;	spawnValue=128;	collectTime=4000;	placable=true;	health=2.0;	heatCapacity=999;	meteorImmune=false;	gatherableDB="brickEOTWGatherableBasicData"; 									helpText="A basic building material. Will not melt in the sunlight, but is still vulnerable to meteor strikes.";	};
		new ScriptObject(MatterType) { name="Quartz";		color="181c26a8";	spawnWeight=20;	spawnVeinSize=3;	spawnValue=128;	collectTime=8000;	placable=true;	health=3.0;	heatCapacity=999;	meteorImmune=true;	gatherableDB="brickEOTWGatherableCrystalData"; 									helpText="Transparent building material, allowing transparent paint colors. Immune to both sunlight and meteors.";	};
		new ScriptObject(MatterType) { name="Coal";			color="000000ff";	spawnWeight=30;	spawnVeinSize=4;	spawnValue=128;	collectTime=6000;	gatherableDB="brickEOTWGatherableBasicData";	fuelPower=16;													helpText="Burnable carbon that is usefuel in both fuel and steel production.";	};
		//Coke Oven Stuff
		new ScriptObject(MatterType) { name="Charcoal";		color="000000ff";	fuelPower=12;	fuelMultiplier=2.0; };
		new ScriptObject(MatterType) { name="Coke";			color="000000ff";	fuelPower=32;	fuelMultiplier=2.0; };
		//Growable Organics
		new ScriptObject(MatterType) { name="Moss";			color="446942ff";	gatherableDB="brickEOTWGatherableBasicData";	helpText="Strange tasting moss. Grows horizontally on wood and itself when placed.";	obtainText="Monster Drop\tGrowing placed seeds."; };
		new ScriptObject(MatterType) { name="Vines";		color="264b38ff";	gatherableDB="brickEOTWGatherableBasicData";	helpText="Thick sturdy vines full of biomatter. Grows downward on wood and itself when placed.";	obtainText="Monster Drop\tGrowing placed seeds.";	 };
		new ScriptObject(MatterType) { name="Cacti";		color="56643bff";	gatherableDB="brickEOTWGatherableBasicData";	helpText="An exotic type of cacti. Grows upward on wood or itself.";	obtainText="Monster Drop\tGrowing placed seeds.";	 };
		//Basic Pure Materials
		new ScriptObject(MatterType) { name="Iron";			color="7a7a7aff";	placable=true;	health=4.0;	heatCapacity=999;	meteorImmune=true;									helpText="The bread and butter of civilization and technology. Can be used to safely build. Immune to meteors and sunlight.";	};
		new ScriptObject(MatterType) { name="Copper";		color="d36b04ff";	helpText="One of the variant metals, primarily used for electrical applications."; };
		new ScriptObject(MatterType) { name="Silver";		color="e0e0e0ff";	helpText="A variant of metal with uses in machine construction.";	};
		new ScriptObject(MatterType) { name="Lead";			color="533d60ff";	helpText="Tastes sweet, must be edible. Used for matter transfer and machine construction.";	};
		new ScriptObject(MatterType) { name="Gold";			color="e2af14ff";	helpText="We're rich! Has important use in making higher tier metal alloys."; };
		new ScriptObject(MatterType) { name="Diamond";		color="00d0ffa8";	spawnWeight=04;	spawnVeinSize=1;	spawnValue=128;	collectTime=24000;	gatherableDB="brickEOTWGatherableCrystalData";	 																									helpText="Who knew carbon could be so rare and expensive. Has niche but useful uses in tools and Adamantine production.";	};
		new ScriptObject(MatterType) { name="Sturdium";		color="646defff";	helpText="An ultra rare strange blue shaded metal from the island of stability. Used for high tier crafting.";	};
		//Miscallenous Gatherable Materials
		new ScriptObject(MatterType) { name="Crude Oil";	color="1c1108ff";																																																								helpText="Unrefined fossil fuels ready to be refined into valuable oil products.";	obtainText="Oil Wells (Needs Oil Pump tool)\tDrilling Rig (Needs Lubricant)";	};
		new ScriptObject(MatterType) { name="Water";		color="bcc1c88e";					boilCapacity=1;	boilMatter="Steam";	superBoilMatter="Super-Heated Steam";	helpText="HOLY FUCK IM DROWNING IN 6.02214076x10^23 MOLECULES OF WATER!!!!!";	obtainText="Water Pump machine. Uncommon drop from some enemies."; };
		new ScriptObject(MatterType) { name="Flesh";		color="82281fff";					helpText="Fresh meat, useful for fermentation and other gross applications.";	obtainText="Butcher valid corpses with a Survival Knife\tTurret machine"; };
		new ScriptObject(MatterType) { name="Sludge";		color="844c75ff"; 					helpText="Lootboxes but in fluid form. Sieve via machine for random loot!"; };
		//Processed Gatherables
		new ScriptObject(MatterType) { name="Fluoric Dust";	color="507582ff"; 					helpText="Mostly processed fluorspar ore. Needs some final processing via a sieve."; };
		new ScriptObject(MatterType) { name="Uranic Dust";	color="007c3fff"; 					helpText="Mostly processed uraninite ore. Needs some final processing via a sieve."; };
		new ScriptObject(MatterType) { name="Steam";		color="bcc1c88e";					turbinePower=2;	coolMatter="Water";	helpText="A more thematically fitting water.";	obtainText="Fueled Boiler machine.";  };
		new ScriptObject(MatterType) { name="Brimstone";	color="93690eff";					helpText="Also known as (solid) sulfur.";	obtainText="Uncommon drop from some enemies."; };
		new ScriptObject(MatterType) { name="Fluorine";		color="1f568cff";					helpText="Good for your teeth. Used in higher tier chemical processing and Uranium boosting."; };
		new ScriptObject(MatterType) { name="Calcium";		color="503623ff"; 					helpText="Good for your bones. Used in higher tier chemical processing and Steel boosting."; };
		new ScriptObject(MatterType) { name="Uranium-238";	color="32F032ff"; 					helpText="Emits cancerous particles. Good thing we don't have to worry about that in the moment."; };
		new ScriptObject(MatterType) { name="Uranium-235";	color="46FA46ff"; 					helpText="Don't get too green."; };
		new ScriptObject(MatterType) { name="Salt";			color="F0C8C8ff"; 					helpText="Anti-water, but yet comes from raw water..."; };
		new ScriptObject(MatterType) { name="Rubber";		color="151515ff"; 					helpText="I hope you aren't allergic. Essential in piping and fluid-like applications."; };
		//Chemistry
		new ScriptObject(MatterType) { name="Oxygen";		color="90AAEEFF"; 					helpText="Not necessary for you to live, apparently. Used in higher level steelmaking and epoxy.";  };
		new ScriptObject(MatterType) { name="Hydrogen";		color="00FFAAFF"; 					turbinePower=1;	coolMatter="";	helpText="The most primitive element, yet has high level uses. Can also be used as inefficent turbine gas."; };
		new ScriptObject(MatterType) { name="Biomass";		color="C3690Fff";	fuelPower=64; 	helpText="You probably shouldn't drink this stuff. Has some use in plastics and drinks. Can also be used as fuel."; };
		new ScriptObject(MatterType) { name="Ethanol";		color="FF8000ff";  					helpText="200% proof booze, perfect for making drinks! And ethylene, if you fancy that."; };
		new ScriptObject(MatterType) { name="Sulfuric Acid";	color="FF8000ff"; 				helpText="Not the safest chemical to handle."; };
		new ScriptObject(MatterType) { name="Hydrofluoric Acid";	color="FF8000ff"; 			helpText="Funny chemical used in the highest tier ore processing."; };
		new ScriptObject(MatterType) { name="Ethylene";		color="11382189"; 					helpText="A simple hydrocarbon for boosting your Biodomes or making plastics."; };
		//Basic Petrochemistry
		new ScriptObject(MatterType) { name="Naphata";		color="4f494bff"; 	fuelPower=64;	combustable=true;	fuelMultiplier=2.0;	helpText="Quite flammable stuff that starts several production chains."; };
		new ScriptObject(MatterType) { name="Light Oil";	color="12037896"; 					helpText="I wouldn't put this on your salad. It is for petrochemistry, not food!"; };
		new ScriptObject(MatterType) { name="Heavy Oil";	color="16776960"; 					helpText="I wouldn't put this on your salad. It is for petrochemistry, not food!"; };
		new ScriptObject(MatterType) { name="Diesel";		color="C3690Fff";	fuelPower=256;	combustable=true;	fuelMultiplier=4.0; helpText="Industry at its finest! Powerful long lasting fuel."; };
		new ScriptObject(MatterType) { name="Paraffin";		color="75502eff"; 					helpText="Wax from oil. Explosive recipes use this stuff."; };
		new ScriptObject(MatterType) { name="Jet Fuel";		color="BDB78Cff";	fuelPower=128;	combustable=true;	fuelMultiplier=8.0; helpText="Industry at its finest! Exceptionally powerful short lasting fuel."; };
		new ScriptObject(MatterType) { name="Lubricant";	color="FFC400ff"; 					helpText="Allows particular machines (i.e. Thumper) to run."; };
		//Advanced Petrochemistry
		new ScriptObject(MatterType) { name="Propylene";	color="12890952"; 					helpText="Gaseous!"; };
		new ScriptObject(MatterType) { name="Toulene";		color="ccccccff"; 					helpText="Has somewhat volitile combution properties."; };
		new ScriptObject(MatterType) { name="Explosives";	color="FFFFAAff"; 					helpText="Matter that is... Explosive. Big suprise. Used in bombs and the highest efficency ammunition."; };
		new ScriptObject(MatterType) { name="Acetone";		color="93426060"; 					helpText="Smelly paint stripping chemical."; };
		new ScriptObject(MatterType) { name="Phenol";		color="66355590"; 					helpText="Carbolic acid, phenolic acid, or benzenol."; };
		new ScriptObject(MatterType) { name="Bisphenol";	color="10848014"; 					helpText="Definately not BPA-free."; };
		new ScriptObject(MatterType) { name="Epichlorohydrin";	color="C8C400ff"; 				helpText="Have fun spelling out this one."; };
		//Plastics
		new ScriptObject(MatterType) { name="Plastic";		color="797260ff"; 					helpText="Polyethyene. Marks a major milestone in your industrial carrer! Lots of applications."; };
		new ScriptObject(MatterType) { name="Teflon";		color="504b3fff"; 					helpText="Yeah the name is trademarked, but who cares? Lots of applications."; };
		new ScriptObject(MatterType) { name="Epoxy";		color="264b38ff"; 					helpText="Sticky resin. Marks the ultimate milestone in your industrial carrer! Lots of applications."; };
		new ScriptObject(MatterType) { name="Implanting Polymer";		color="36acb5ff";	 	helpText="The salvinorin helps lessen the pain of the implants."; };
		//Metal Alloys & Components
		//t1
		new ScriptObject(MatterType) { name="Electrum";		color="dfc47cff"; 					helpText="An alloy of Gold and Silver. Not actually electric, but is used in eletronics and more."; };
		new ScriptObject(MatterType) { name="Red Gold";		color="ca959eff"; 					helpText="An alloy of Copper and Gold, the latter the alloy's name suggests. Used in eletronics and more."; };
		new ScriptObject(MatterType) { name="Steel";		color="2f2d2fff"; 					helpText="If we couldn't make steel, then life as we know it could not exist."; };
		//t2
		new ScriptObject(MatterType) { name="Energium";		color="d69c6bff"; 					helpText="Upgraded Electrum through the application of Teflon."; };
		new ScriptObject(MatterType) { name="Naturum";		color="83bc8cff"; 					helpText="Upgraded Red Gold through the application of Teflon."; };
		new ScriptObject(MatterType) { name="Adamantine";	color="bf1f21ff"; 					helpText="Upgraded Steel through the application of GT Diamond."; };
		new ScriptObject(MatterType) { name="GT Diamond";	color="6f0f11ff"; 					helpText="Stands for \"Gilded-Treated\" Diamond, that is gilded with Sturdium and treated with Epoxy."; };
		new ScriptObject(MatterType) { name="PlaSteel";		color="ddb389ff";	placable=true;	health=4.0;	heatCapacity=999;	meteorImmune=true; 					helpText="A solid building material which can be produced in industrial amounts."; };
		new ScriptObject(MatterType) { name="Granite Polymer";		color="c1a872ff"; 			helpText="A clump of plastic, rocks, and stones to make PlaSteel."; };
		new ScriptObject(MatterType) { name="Quicklime";	color="ffaaaaff"; 					helpText="Doesn't actually make things quicker, but allows a more efficent method of steelmaking. It is lime colored though. (I know quicklime isn't lime irl shut up noob)"; };
		new ScriptObject(MatterType) { name="Asphalt";		color="444444ff";	placable=true;	health=4.0;	heatCapacity=999;	meteorImmune=true;	helpText="High friction surface which improves walking and climbing efficiency."; };
		//Nuclear
		new ScriptObject(MatterType) { name="LEU-Fuel";		color="ff0000ff";	fissionPower=64;  fissionWasteRate=1; 					helpText="Lower enriched uranium, to be fed into a MFR."; };
		new ScriptObject(MatterType) { name="HEU-Fuel";		color="ff7777ff";	fissionPower=256;  fissionWasteRate=2; 					helpText="Highly enriched uranium, to be fed into a MFR."; };
		new ScriptObject(MatterType) { name="Nuclear Waste";		color="605042ff"; 			helpText="Spent fuel from a MFR. Could probably be recycled into something quite useful."; };
		new ScriptObject(MatterType) { name="Plutonium";	color="F03232ff"; 					helpText="Radioactive metal for when Uranium-235 isn't spicy enough. Used in end-game crafting."; };
		new ScriptObject(MatterType) { name="Super-Heated Steam";	color="ffc1c88e"; 			turbinePower=2;	coolMatter="Steam";		helpText="Steam that is SUPER!! Will turn into normal steam after going through a turbine. Comes from nuclear reactors."; };
		//new ScriptObject(MatterType) { name="Heavy Water";	color="bcc1c88e"; 					helpText="Apparently heavy water is pottable, kind of. Just don't drink too much of it."; };
		//new ScriptObject(MatterType) { name="Deuterium";	color="EEEE00ff"; 					helpText="An uncommon isotope of hydrogen."; };
		//new ScriptObject(MatterType) { name="Tritium";		color="FF0000FF"; 					helpText="An extremely rare and radioactive isotope of hydrogen."; };
		//new ScriptObject(MatterType) { name="Helium";		color="DDDD00ff"; 					helpText="Ballons and high pitched voices."; };
		//Exotic
		new ScriptObject(MatterType) { name="Boss Essence";	color="ff00ffff"; 					helpText="Magic is real, and this is it. Used in the Void Drill."; };
		new ScriptObject(MatterType) { name="Rare Earths";	color="DCFADCff"; 					helpText="An assortment of valuable metals not naturally found on this planet."; };
		new ScriptObject(MatterType) { name="dog";			color="00ffffff"; 					helpText="dog"; };
		//Ammunition
		new ScriptObject(MatterType) { name="Rifle Round";	color="ffffffff";	bulletType="machineGunProjectile"; 							helpText="Individual bullets to be used in a Machine Gun or Turret."; };
		new ScriptObject(MatterType) { name="Shotgun Pellet";	color="ffffffff";	bulletType="basicShotgunProjectile"; 					helpText="Individual BBs to be used in a Shotgun or Turret."; };
		new ScriptObject(MatterType) { name="Launcher Load";	color="ffffffff";	bulletType="gLauncherProjectile"; 						helpText="Individual explosives to be used in a Launcher or Turret."; };
		new ScriptObject(MatterType) { name="Crystal Matrix";	color="ffffffff";	bulletType="crystalBowProjectile"; 						helpText="An immaculate array of crystal, used to power crystal weaponry."; };
		new ScriptObject(MatterType) { name="Nuke";			color="ffffffff";	bulletType="nukeProjectile"; 								helpText="Take cover!"; };
		//Potion matter
		new ScriptObject(MatterType) { name="Healium";		color="bcc1c88e";	potionType="potionHealingImage"; 							helpText="Fluid with healing properties. Can be applied via Chem Diffuser or Potion."; };
		new ScriptObject(MatterType) { name="Gatherium";	color="bcc1c88e";	potionType="potionGatheringImage"; 							helpText="Fluid that assists in gathering materials. Can be applied via Chem Diffuser or Potion."; };
		new ScriptObject(MatterType) { name="Adrenlium";	color="bcc1c88e";	potionType="potionSpeedImage"; 								helpText="Fluid that gets your heart and legs pumping. Can be applied via Chem Diffuser or Potion."; };
		new ScriptObject(MatterType) { name="Rangium";		color="bcc1c88e";	potionType="potionRangedImage"; 							helpText="Fluid which helps your firearm handling. Can be applied via Chem Diffuser or Potion."; };
		new ScriptObject(MatterType) { name="Salvinorin";	color="bcc1c88e";	potionType=""; 												helpText="Mind numbing halluciogens. Gets your mind off the pain."; };
		//Crafted Items
		new ScriptObject(MatterType) { name="Piping";			color="00ffffff"; 			helpText="Because manually carrying things suck. Used to build pipe networks."; };
		new ScriptObject(MatterType) { name="Wiring";			color="00ffffff"; 			helpText="You are full of unorganized wires. Pull them out. Used to build power cable networks."; };
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

////Old code
// function GetMatterType(%type)
// {
// 	if (!isObject($EOTW::MatterType[%type]))
// 		for (%i = 0; %i < MatterData.getCount(); %i++)
// 			if (MatterData.getObject(%i).name $= %type)
// 				$EOTW::MatterType[%type] = MatterData.getObject(%i);

// 	return $EOTW::MatterType[%type];
// }

function GetMatterType (%partialName)
{
	if (%partialName $= "")
		return 0;
		
	if (!isObject($EOTW::MatterType[%partialName]))
	{
		%pnLen = strlen (%partialName);
		%matterIndex = 0;
		%bestMatter = -1;
		%bestPos = 9999;
		while (%matterIndex < MatterData.getCount())
		{
			%matter = MatterData.getObject (%matterIndex);
			%pos = -1;
			%name = strlwr (%matter.name);
			%pos = strstr (%name, strlwr(%partialName));
			if (%pos != -1)
			{
				%bestMatter = %matter;
				if (%pos == 0)
				{
					$EOTW::MatterType[%partialName] = %bestMatter;
					break;
				}
				if (%pos < %bestPos)
				{
					%bestPos = %pos;
					%bestMatter = %bestMatter;
				}
			}
			%matterIndex += 1;
		}

		if (%bestMatter != -1)
			$EOTW::MatterType[%partialName] = %bestMatter;
		else 
			return 0;
	}
	
	return $EOTW::MatterType[%partialName];
}

function getMatterTextColor(%type)
{
	if (isObject(%matter = GetMatterType(%type)))
		return "<color:" @ getSubStr(%matter.color, 0, 6) @ ">";

	return 0;
}

function SetupRecipes()
{
	if (isObject(RecipeData))
	{
		RecipeData.deleteAll();
		RecipeData.delete();
	}

	new SimSet(RecipeData)
	{
		//Pyrolysis? Coke oven? Are they the same thing? I don't think so but who cares?
		new ScriptObject(Recipe_Coke) {	
			recipeType="Pyrolysis";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=2;	
			input[0]="Coal\t1";	output[0]="Coke\t1";	};
		new ScriptObject(Recipe_Charcoal) {	
			recipeType="Pyrolysis";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Wood\t1";	output[0]="Charcoal\t1";	};
		//T1 Alloys
		new ScriptObject(Recipe_Electrum) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Silver\t1";	input[1]="Gold\t3";	output[0]="Electrum\t4";	};
		new ScriptObject(Recipe_Red_Gold) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Copper\t1";	input[1]="Gold\t3";	output[0]="Red Gold\t4";	};
		new ScriptObject(Recipe_Steel) {	
			recipeType="Blasting";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=4;	
			input[0]="Coke\t1";	input[1]="Iron\t2";	output[0]="Steel\t2";	};
		new ScriptObject(Recipe_Steel_Oxygen) {	
			recipeType="Blasting";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=2;	minTier=1;
			input[0]="Oxygen\t4";	input[1]="Iron\t2";	output[0]="Steel\t3";	};
		new ScriptObject(Recipe_Steel_Boosted) {	
			recipeType="Blasting";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	minTier=2;
			input[0]="Quicklime\t4";	input[1]="Iron\t2";	output[0]="Steel\t4";	};
		//Intermediate Alloys
		new ScriptObject(Recipe_Granite_Polymer) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Granite\t3";	input[1]="Plastic\t1";	output[0]="Granite Polymer\t4";	};
		new ScriptObject(Recipe_GT_Diamond) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=4;	minTier=2;
			input[0]="Diamond\t1";	input[1]="Sturdium\t1";	input[2]="Epoxy\t1";	output[0]="GT Diamond\t3";	};
		new ScriptObject(Recipe_Quicklime) {	
			recipeType="Blasting";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=2;
			input[0]="Calcium\t1";	input[1]="Oxygen\t1";	output[0]="Quicklime\t2";	};
		//T2 Alloys
		new ScriptObject(Recipe_Energium) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=4;	minTier=1;
			input[0]="Electrum\t1";	input[1]="Teflon\t1";	output[0]="Energium\t2";	};
		new ScriptObject(Recipe_Naturum) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=4;	minTier=1;
			input[0]="Red Gold\t1";	input[1]="Teflon\t1";	output[0]="Naturum\t2";	};
		new ScriptObject(Recipe_Crappy_PlaSteel) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Steel\t2";	input[1]="Granite Polymer\t2";	output[0]="PlaSteel\t1";	};
		new ScriptObject(Recipe_PlaSteel) {	
			recipeType="Blasting";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Steel\t1";	input[1]="Granite Polymer\t1";	output[0]="PlaSteel\t4";	};
		new ScriptObject(Recipe_Adamantine) {	
			recipeType="Blasting";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=6;	minTier=2;
			input[0]="Steel\t1";	input[1]="GT Diamond\t1";	output[0]="Adamantine\t2";	};
		//Pipes and wires
		new ScriptObject(Recipe_T1_Piping) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=8;
			input[0]="Silver\t128";	input[1]="Lead\t128";	input[2]="Rubber\t128";	output[0]="Piping\t8";	};
		new ScriptObject(Recipe_T2_Piping) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=6;	minTier=1;
			input[0]="Red Gold\t128";	input[1]="Lead\t256";	input[2]="Plastic\t128";	output[0]="Piping\t32";	};
		new ScriptObject(Recipe_T3_Piping) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=2;	minTier=2;
			input[0]="Naturum\t128";	input[1]="Lead\t512";	input[2]="Epoxy\t128";	output[0]="Piping\t128";	};
		new ScriptObject(Recipe_T1_Wiring) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=8;
			input[0]="Copper\t128";	input[1]="Lead\t128";	input[2]="Rubber\t128";	output[0]="Wiring\t8";	};
		new ScriptObject(Recipe_T2_Wiring) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=6;	minTier=1;
			input[0]="Electrum\t128";	input[1]="Lead\t256";	input[2]="Plastic\t128";	output[0]="Wiring\t32";	};
		new ScriptObject(Recipe_T3_Wiring) {	
			recipeType="Alloying";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=2;	minTier=2;
			input[0]="Energium\t128";	input[1]="Lead\t512";	input[2]="Epoxy\t128";	output[0]="Wiring\t128";	};
		//Basic Processed Materials
		new ScriptObject(Recipe_Brimstone) {	
			recipeType="Burning";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Crude Oil\t2";	output[0]="Brimstone\t1";	};
		//new ScriptObject(Recipe_Uranic_Processing) {	
		//	recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=2;
		//	input[0]="Uranic Dust\t128";	input[1]="Sulfuric Acid\t32";	output[0]="Uranium-238\t128";	output[1]="Uranium-235\t1";	};
		//new ScriptObject(Recipe_Fluoric_Processing) {	
		//	recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
		//	input[0]="Fluoric Dust\t3";	input[1]="Ethanol\t1";	output[0]="Calcium\t1";	output[1]="Fluorine\t2";	};
		new ScriptObject(Recipe_Rubber) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Wood\t2";	input[1]="Sulfuric Acid\t1";	output[0]="Rubber\t1";	};
		new ScriptObject(Recipe_Salt) {	
			recipeType="Burning";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	minTier=1;
			input[0]="Water\t64";	output[0]="Salt\t1";	};
		//Chemistry
		new ScriptObject(Recipe_Wood_Crisping) {	
			recipeType="Burning";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;
			input[0]="Wood\t16";	output[0]="Coal\t1";	};
		new ScriptObject(Recipe_Water_Electrolysis) {	
			recipeType="Seperation";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Water\t3";	output[0]="Hydrogen\t2";	output[1]="Oxygen\t1";	};
		new ScriptObject(Recipe_Biomass) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Vines\t1";	input[1]="Moss\t1";	input[2]="Cacti\t1";	output[0]="Biomass\t3";	};
		new ScriptObject(Recipe_Ethanol) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Biomass\t1";	input[1]="Flesh\t1";	input[2]="Water\t1";	output[0]="Ethanol\t3";	};
		new ScriptObject(Recipe_Sulfuric_Acid) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Brimstone\t1";	input[1]="Water\t2";	output[0]="Sulfuric Acid\t3";	};
		new ScriptObject(Recipe_Hydrofluoric_Acid) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=2;	
			input[0]="Hydrogen\t1";	input[1]="Fluorine\t1";	output[0]="Hydrofluoric Acid\t2";	};
		new ScriptObject(Recipe_Ethylene) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	
			input[0]="Sulfuric Acid\t1";	input[1]="Ethanol\t1";	output[0]="Ethylene\t2";	};
		new ScriptObject(Recipe_Boosted_Ethylene) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Sulfuric Acid\t1";	input[1]="Ethanol\t1";	input[2]="Propylene\t2";	output[0]="Ethylene\t4";	};	
		//Petrochemistry
		new ScriptObject(Recipe_Oil_Refining) {	
			recipeType="Seperation";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Crude Oil\t3";	output[0]="Light Oil\t1";	output[1]="Naphata\t1";	output[2]="Heavy Oil\t1";	};
		new ScriptObject(Recipe_Diesel) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Light Oil\t32";	input[1]="Heavy Oil\t32";	output[0]="Diesel\t64";	output[1]="Toulene\t1";	};
		new ScriptObject(Recipe_Jet_Fuel) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Light Oil\t1";	input[1]="Hydrogen\t2";	output[0]="Jet Fuel\t3";	};
		new ScriptObject(Recipe_Paraffin) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Light Oil\t1";	input[1]="Coal\t1";	input[1]="Wood\t2";	output[0]="Paraffin\t3";	};
		new ScriptObject(Recipe_Lubricant) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Heavy Oil\t1";	input[1]="Water\t2";	output[0]="Lubricant\t3";	};
		new ScriptObject(Recipe_Asphalt) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Heavy Oil\t1";	input[1]="Coal\t1";	input[2]="Granite\t2";	output[0]="Asphalt\t3";	};
		//Advanced Petrochemistry
		new ScriptObject(Recipe_Propylene) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Naphata\t1";	input[1]="Steam\t1";	output[0]="Propylene\t1";	};
		new ScriptObject(Recipe_Toulene_from_Coal) {	
			recipeType="Burning";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=2;
			input[0]="Coal\t2";	output[0]="Toulene\t1";	};
		new ScriptObject(Recipe_Toulene_from_Ethylene) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Ethylene\t1";	input[1]="Steam\t2";	output[0]="Toulene\t1";	};
		new ScriptObject(Recipe_Explosives) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=2;
			input[0]="Toulene\t2";	input[1]="Brimstone\t1";	input[2]="Paraffin\t1";	output[0]="Explosives\t4";	};
		new ScriptObject(Recipe_Hock_Process) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Toulene\t2";	input[1]="Propylene\t1";	input[2]="Oxygen\t1";	output[0]="Acetone\t2";	output[1]="Phenol\t2";	};
		new ScriptObject(Recipe_Phenol) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=2;
			input[0]="Propylene\t2";	input[1]="Hydrogen\t1";	output[0]="Phenol\t3";	};
		new ScriptObject(Recipe_Bisphenol) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Acetone\t1";	input[1]="Phenol\t2";	output[0]="Bisphenol\t3";	};
		new ScriptObject(Recipe_Epichlorohydrin) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Propylene\t1";	input[1]="Sulfuric Acid\t1";	input[2]="Salt\t1";	output[0]="Epichlorohydrin\t3";	};
		//Plastics
		new ScriptObject(Recipe_Plastic) {	
			recipeType="Burning";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;	minTier=1;
			input[0]="Ethylene\t2";	output[0]="Plastic\t1";	};
		new ScriptObject(Recipe_Teflon) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[1];	powerCostMulti=1;	minTier=1;
			input[0]="Fluorine\t2";	input[1]="Hydrogen\t1";	input[2]="Ethylene\t1";	output[0]="Teflon\t4";	};
		new ScriptObject(Recipe_Epoxy) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=1;
			input[0]="Epichlorohydrin\t1";	input[1]="Bisphenol\t1";	input[2]="Oxygen\t16";	output[0]="Epoxy\t2";	};
		new ScriptObject(Recipe_Implanting_Polymer) {	
			recipeType="Chemistry";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=8;	minTier=1;
			input[0]="Teflon\t128";	input[1]="Salvinorin\t128";	input[2]="Sturdium\t128";	output[0]="Implanting Polymer\t1";	};
		//Biodome
		new ScriptObject(Recipe_Vines) {	
			recipeType="Biodome";	powerDrain=$EOTW::PowerLevel[0]>>2;	powerCostMulti=1;	
			input[0]="Water\t128";	input[1]="";	output[0]="Vines\t1";	};
		new ScriptObject(Recipe_Vines_Boosted) {	
			recipeType="Biodome";	powerDrain=$EOTW::PowerLevel[0]>>2;	powerCostMulti=1;	minTier=1;
			input[0]="Water\t256";	input[1]="Ethylene\t1";	output[0]="Vines\t8";	};
		new ScriptObject(Recipe_Moss) {	
			recipeType="Biodome";	powerDrain=$EOTW::PowerLevel[0]>>2;	powerCostMulti=1;	
			input[0]="Water\t128";	input[1]="";	output[0]="Moss\t1";	};
		new ScriptObject(Recipe_Moss_Boosted) {	
			recipeType="Biodome";	powerDrain=$EOTW::PowerLevel[0]>>2;	powerCostMulti=1;	minTier=1;
			input[0]="Water\t256";	input[1]="Ethylene\t1";	output[0]="Moss\t8";	};
		new ScriptObject(Recipe_Cacti) {	
			recipeType="Biodome";	powerDrain=$EOTW::PowerLevel[0]>>2;	powerCostMulti=1;	
			input[0]="Water\t128";	input[1]="";	output[0]="Cacti\t1";	};
		new ScriptObject(Recipe_Cacti_Boosted) {	
			recipeType="Biodome";	powerDrain=$EOTW::PowerLevel[0]>>2;	powerCostMulti=1;	minTier=1;
			input[0]="Water\t256";	input[1]="Ethylene\t1";	output[0]="Cacti\t8";	};
		new ScriptObject(Recipe_Wood) {	
			recipeType="Biodome";	powerDrain=$EOTW::PowerLevel[0]>>2;	powerCostMulti=1;	
			input[0]="Water\t128";	input[1]="";	output[0]="Wood\t8";	};
		//Brewing
		new ScriptObject(Recipe_Healium) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[0]>>3;	powerCostMulti=1;	
			input[0]="Water\t4";	input[1]="Vines\t1";	input[2]="Moss\t1";	input[3]="Cacti\t1";	output[0]="Healium\t4";	};
		new ScriptObject(Recipe_Gatherium) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[0]>>3;	powerCostMulti=1;	
			input[0]="Water\t4";	input[1]="Salt\t1";	input[2]="Moss\t1";	input[3]="Lead\t2";	output[0]="Gatherium\t4";	};
		new ScriptObject(Recipe_Adrenlium) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[0]>>3;	powerCostMulti=1;	
			input[0]="Water\t4";	input[1]="Salt\t1";	input[2]="Vines\t1";	input[3]="Quartz\t2";	output[0]="Adrenlium\t4";	};
		new ScriptObject(Recipe_Rangium) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[0]>>3;	powerCostMulti=1;	
			input[0]="Water\t4";	input[1]="Salt\t1";	input[2]="Cacti\t1";	input[3]="Gold\t2";	output[0]="Rangium\t4";	};
		new ScriptObject(Recipe_Salvinorin) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[0]>>3;	powerCostMulti=1;	minTier=1;
			input[0]="Biomass\t2";	input[1]="Acetone\t1";	input[2]="Naphata\t1";	input[3]="Propylene\t1";	output[0]="Salvinorin\t2";	};
		//Drilling
		new ScriptObject(Recipe_Iron) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	
			input[0]="Boss Essence\t1";	output[0]="Iron\t1";	};
		new ScriptObject(Recipe_Quartz) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	
			input[0]="Boss Essence\t1";	output[0]="Quartz\t1";	};
		new ScriptObject(Recipe_Copper) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	
			input[0]="Boss Essence\t2";	output[0]="Copper\t1";	};
		new ScriptObject(Recipe_Silver) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	
			input[0]="Boss Essence\t2";	output[0]="Silver\t1";	};
		new ScriptObject(Recipe_Lead) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	
			input[0]="Boss Essence\t4";	output[0]="Lead\t1";	};
		new ScriptObject(Recipe_Gold) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	
			input[0]="Boss Essence\t4";	output[0]="Gold\t1";	};
		new ScriptObject(Recipe_Uraninite) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=1;
			input[0]="Boss Essence\t8";	output[0]="Uraninite\t1";	};
		new ScriptObject(Recipe_Fluorspar) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=1;
			input[0]="Boss Essence\t8";	output[0]="Fluorspar\t1";	};
		new ScriptObject(Recipe_Diamond) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=2;
			input[0]="Boss Essence\t16";	output[0]="Diamond\t1";	};
		new ScriptObject(Recipe_Sturdium) {	
			recipeType="Drilling";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=2;
			input[0]="Boss Essence\t16";	output[0]="Sturdium\t1";	};
		//Nuclear
		new ScriptObject(Recipe_TorvaNex_Process) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=2;
			input[0]="Uranium-238\t128";	input[1]="Uranium-235\t16";	input[2]="Fluorine\t128";	input[3]="Calcium\t64";	output[0]="Uranium-235\t18";	output[1]="Uranium-238\t64";	};
		new ScriptObject(Recipe_LEU_Fuel) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=2;
			input[0]="Uranium-238\t128";	input[1]="Uranium-235\t4";	input[2]="Fluorine\t64";	input[3]="Sulfuric Acid\t64";	output[0]="LEU-Fuel\t128";	};
		new ScriptObject(Recipe_HEU_Fuel) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=2;
			input[0]="Uranium-238\t128";	input[1]="Uranium-235\t16";	input[2]="Fluorine\t64";	input[3]="Sulfuric Acid\t64";	output[0]="HEU-Fuel\t128";	};
		new ScriptObject(Recipe_Waste_Recycling) {	
			recipeType="Brewing";	powerDrain=$EOTW::PowerLevel[2];	powerCostMulti=1;	minTier=2;
			input[0]="Nuclear Waste\t128";	input[1]="Coal\t16";		input[2]="Fluorine\t128";	input[3]="Calcium\t64";	output[0]="Uranium-235\t3";	output[1]="Uranium-238\t24";	};
	};

	//Ore Processing (Automatic)
	for (%i = 0; %i < MatterData.getCount(); %i++)
	{
		%matter = MatterData.getObject(%i);
		if ((%output = %matter.oreOutput) !$= "")
		{
			%input = %matter.name;
			%crushed = "Crushed" SPC %matter.name;
			%washed = "Purified" SPC %matter.name;
			%slurry = %matter.name SPC "Slurry";
			MatterData.add(new ScriptObject(MatterType) { name = %crushed; color = %matter.color; });
			MatterData.add(new ScriptObject(MatterType) { name = %washed; color = %matter.color; });
			MatterData.add(new ScriptObject(MatterType) { name = %slurry; color = %matter.color; });

			//strReplace(%crushed, " ", "_")

			//Raw Ore
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%input, " ", "_") @ "_Smelting") {	
				recipeType="Refining";	powerDrain=$EOTW::PowerLevel[0] >> 3;	powerCostMulti=1;
				input[0]=(%input TAB 1);	output[0]=(%output TAB 1);	});
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%input, " ", "_") @ "_Crushing") {	
				recipeType="Crushing";	powerDrain=$EOTW::PowerLevel[0] >> 3;	powerCostMulti=1;
				input[0]=(%input TAB 1);	output[0]=(%crushed TAB 1);	});
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%input, " ", "_") @ "_Washing") {	
				recipeType="Washing";	powerDrain=$EOTW::PowerLevel[0] >> 3;	powerCostMulti=1;
				input[0]=(%input TAB 2);	input[1]=("Water" TAB 8);	output[0]=(%crushed TAB 1);	output[1]=("Sludge" TAB 1);	});

			//Dust
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%crushed, " ", "_") @ "_Smelting") {	
				recipeType="Refining";	powerDrain=$EOTW::PowerLevel[0] >> 2;	powerCostMulti=1;
				input[0]=(%crushed TAB 4);	output[0]=(%output TAB 5);	});
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%crushed, " ", "_") @ "_Washing") {	
				recipeType="Washing";	powerDrain=$EOTW::PowerLevel[0] >> 2;	powerCostMulti=1;
				input[0]=(%crushed TAB 1);	input[1]=("Water" TAB 4);	output[0]=(%washed TAB 1);	});
			
			//Washed
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%washed, " ", "_") @ "_Smelting") {	
				recipeType="Refining";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;
				input[0]=(%washed TAB 4);	output[0]=(%output TAB 7);	});
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%washed, " ", "_") @ "_Slurry") {	
				recipeType="Frothing";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;
				input[0]=(%washed TAB 4);	input[1]=(%matter.flotationChemical TAB 1);	output[0]=(%slurry TAB 3);	output[1]=(%washed TAB 1);	});
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%washed, " ", "_") @ "_Seperation") {	
				recipeType="Seperation";	powerDrain=$EOTW::PowerLevel[0];	powerCostMulti=1;
				input[0]=(%washed TAB 2);	output[0]=(%crushed TAB 1);	output[1]=("Sludge" TAB 1);	});

			//Slurry
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%slurry, " ", "_") @ "_Smelting") {	
				recipeType="Refining";	powerDrain=$EOTW::PowerLevel[0] << 1;	powerCostMulti=1;
				input[0]=(%slurry TAB 4);	output[0]=(%output TAB 10);	});
			RecipeData.add(new ScriptObject("Recipe_" @ strReplace(%slurry, " ", "_") @ "_Seperation") {	
				recipeType="Seperation";	powerDrain=$EOTW::PowerLevel[0] << 1;	powerCostMulti=1;
				input[0]=(%slurry TAB 4);	output[0]=(%output TAB 9);	output[1]=(%matter.flotationChemical TAB 1);	});
		}
	}
}
SetupRecipes();

function cleanRecipeName(%name)
{
	return strReplace(getSubStr(%name, 7, strLen(%name)), "_", " ");
}

function getRecipePowerCost(%recipe)
{
	if (%recipe.powerCost > 0)
		return %recipe.powerCost * $EOTW::GlobalPowerCostMultiplier;
	%powerCost = 0;
		for (%k = 0; %recipe.output[%k] !$= ""; %k++)
			%powerCost += getField(%recipe.output[%k], 1);
	%powerCost *= %recipe.powerCostMulti * %recipe.powerDrain;

	%recipe.powerCost = %powerCost;
	return %recipe.powerCost * $EOTW::GlobalPowerCostMultiplier;
}

function getRecipeText(%recipe)
{
	if (!isObject(%recipe))
		return "---";

	for (%i = 0; %recipe.input[%i] !$= ""; %i++)
		%input = trim(%input SPC "+" SPC getField(%recipe.input[%i], 1) SPC getField(%recipe.input[%i], 0));
	%input = getSubStr(%input, 2, strLen(%input));

	for (%i = 0; %recipe.output[%i] !$= ""; %i++)
		%output = trim(%output SPC "+" SPC getField(%recipe.output[%i], 1) SPC getField(%recipe.output[%i], 0));
	%output = getSubStr(%output, 2, strLen(%output));

	return "\c6" @ %input SPC "==>" SPC %output @ "<br>(" @ getRecipePowerCost(%recipe) SPC "EU Cost @" SPC %recipe.powerDrain SPC "EU/tick)";
}

function ServerCmdM(%client, %typeA, %typeB, %typeC, %typeD) { ServerCmdMaterial(%client, %typeA, %typeB, %typeC, %typeD); }
function ServerCmdMat(%client, %typeA, %typeB, %typeC, %typeD) { ServerCmdMaterial(%client, %typeA, %typeB, %typeC, %typeD); }
function ServerCmdMaterial(%client, %typeA, %typeB, %typeC, %typeD)
{
	%type = trim(%typeA SPC %typeB SPC %typeC SPC %typeD);
	%matter = GetMatterType(%type);

	if (!isObject(%matter))
	{
		if (%type $= "")
			%client.chatMessage("Usage: /m <Material Name>");
		else
			%client.chatMessage("Material \"" @ %type @ "\" was not found!");

		return;
	}
	
	%sourceCount = 0;
	%productCount = 0;
	//Get machine crafting sources and products
	for (%i = 0; %i < RecipeData.getCount(); %i++)
	{
		%recipe = RecipeData.getObject(%i);

		for (%j = 0; %recipe.output[%j] !$= ""; %j++)
			if (getField(%recipe.output[%j], 0) $= %matter.name)
				%sources[%sourceCount++] = cleanRecipeName(%recipe.getName()) @ " (Tier " @ (%recipe.minTier + 1) SPC %recipe.recipeType @ ") - " @ getRecipeText(%recipe);

		for (%j = 0; %recipe.input[%j] !$= ""; %j++)
			if (getField(%recipe.input[%j], 0) $= %matter.name)
				%products[%productCount++] = cleanRecipeName(%recipe.getName()) @ " (Tier " @ (%recipe.minTier + 1) SPC %recipe.recipeType @ ") - " @ getRecipeText(%recipe);
	}

	//Material related stuff
	for (%i = 0; %i < MatterData.getCount(); %i++)
	{
		%targetMatter = MatterData.getObject(%i);
		if ($EOTW::SieveOutput[%targetMatter.name] !$= "" && hasField($EOTW::SieveOutput[%targetMatter.name], %matter.name))
			%sources[%sourceCount++] = "Sieving (" @ %targetMatter.name @ ")";
	}

	//---
	if (%matter.spawnWeight > 0)
		%client.chatMessage("\c6> Gatherable Brick");

	if (%matter.obtainText !$= "")
		%client.chatMessage("\c6> " @ %matter.obtainText);

	for (%i = 1; %i <= %sourceCount; %i++)
		%client.chatMessage("\c6> " @ %sources[%i]);

	%client.chatMessage("\c6^^ [\c5Sources\c6] ^^");
	//---
	for (%i = 1; %i <= %productCount; %i++)
		%client.chatMessage("\c1>\c6 " @ %products[%i]);

	for (%i = 0; %i < DataBlockGroup.getCount(); %i++)
	{
		%data = DataBlockGroup.getObject(%i);
		%className = %data.getClassName();
		switch$ (%className)
		{
			case "fxDTSBrickData":
				%cost = $EOTW::CustomBrickCost[%data.getName()];
				for (%j = 3; %j < getFieldCount(%cost); %j += 2)
					if (getField(%cost, %j) $= %matter.name)
						%client.chatMessage("\c2>\c6 " @ %data.uiName @ " (Brick)");
			case "itemData":
				%cost = $EOTW::ItemCrafting[%data.getName()];
				for (%j = 1; %j < getFieldCount(%cost); %j += 2)
					if (getField(%cost, %j) $= %matter.name)
						%client.chatMessage("\c3>\c6 " @ %data.uiName @ " (Item)");
				//TODO: Also show machine upgrades
		}
	}

	if (%matter.placable)
		%client.chatMessage("\c6> Building Material");

	if ($EOTW::SieveOutput[%matter.name] !$= "")
	{
		%sieve = $EOTW::SieveOutput[%matter.name];
		%sieveText = "\c6> Sievable: ";

		for (%i = 0; %i < getFieldCount(%sieve); %i += 2)
			%sieveText = %sieveText @ getField(%sieve, %i) @ ", ";

		%sieveText = getSubStr(%sieveText, 0, strLen(%sieveText) - 2);

		%client.chatMessage(%sieveText);
	}

	%client.chatMessage("\c6^^ [\c4Products\c6] ^^");
	//---
	if (%matter.helpText !$= "")
		%client.chatMessage("\c6" @ %matter.helpText);

	%client.chatMessage("\c6--- [" @ getMatterTextColor(%matter.name) @ %matter.name @ "\c6]");
	%client.chatMessage("\c6------");
}