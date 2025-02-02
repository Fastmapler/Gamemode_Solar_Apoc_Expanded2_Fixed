datablock AudioProfile(WaterPumpLoopSound)
{
   filename    = "./Sounds/WaterPump.wav";
   description = AudioCloseLooping3d;
   preload = true;
   ReferenceDistance = 0;
   maxDistance = 1;
};

datablock fxDTSBrickData(brickEOTWWaterPumpData)
{
	brickFile = "./Shapes/WaterPump.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Water Pump";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/WaterPump";

    isPowered = true;
	powerType = "Machine";

	matterSize = 512;
	matterSlots["Output"] = 1;
    inspectMode = 1;

	processSound = WaterPumpLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWWaterPumpData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Iron" TAB 256 TAB "Copper" TAB 256 TAB "Rubber";
$EOTW::BrickDescription["brickEOTWWaterPumpData"] = "A device that draws water deep within the ground. Can be operated manually.";

function brickEOTWWaterPumpData::onTick(%this, %obj) {
	if (%obj.GetMatter("Water", "Output") < 128 && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{ //4*1=4 8*2=16 16*3=48 32*4=128
        %obj.ChangeMatter("Water", mPow(2, %obj.upgradeTier + 2), "Output");
	}
}

$EOTW::BrickUpgrade["brickEOTWWaterPumpData", "MaxTier"] = 3;
$EOTW::BrickUpgrade["brickEOTWWaterPumpData", 0] = 256 TAB "Silver" TAB 256 TAB "Gold" TAB 256 TAB "Lead";
$EOTW::BrickUpgrade["brickEOTWWaterPumpData", 1] = 256 TAB "Steel" TAB 256 TAB "Granite Polymer" TAB 128 TAB "Lubricant";
$EOTW::BrickUpgrade["brickEOTWWaterPumpData", 2] = 256 TAB "Bisphenol" TAB 256 TAB "Epichlorohydrin" TAB 256 TAB "Lubricant";

function brickEOTWWaterPumpData::onInspect(%this, %obj, %client) {
	return;
    if (%obj.GetMatter("Water", "Output") < 128 && getSimTime() - %obj.lastDrawSuccess >= 100)
    {
        %obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
        %obj.ChangeMatter("Water", 4, "Output");
    }
}

datablock AudioProfile(DrillingRigLoopSound)
{
   filename    = "./Sounds/OilRig.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWDrillingRigData)
{
	brickFile = "./Shapes/OilRig.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Drilling Rig";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/OilRig";

	matterSize = 16;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 2;

	isPowered = true;
	powerType = "Machine";
	isProcessingMachine = true;

	processSound = DrillingRigLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWDrillingRigData"] = 1.00 TAB "7a7a7aff" TAB 1024 TAB "PlaSteel" TAB 512 TAB "Steel" TAB 128 TAB "Piping";
$EOTW::BrickDescription["brickEOTWDrillingRigData"] = "A large construct which extracts from underground veins. Needs lubricant to function. Find veins with the scanner tool.";

$EOTW::BrickUpgrade["brickEOTWDrillingRigData", "MaxTier"] = 3;
$EOTW::BrickUpgrade["brickEOTWDrillingRigData", 0] = 256 TAB "PlaSteel" TAB 128 TAB "Piping";
$EOTW::BrickUpgrade["brickEOTWDrillingRigData", 1] = 256 TAB "PlaSteel" TAB 128 TAB "Piping";
$EOTW::BrickUpgrade["brickEOTWDrillingRigData", 2] = 256 TAB "PlaSteel" TAB 128 TAB "Piping";

function brickEOTWDrillingRigData::onTick(%this, %obj)
{
	if (!isObject(%vein = %obj.drillingVein))
	{
		%veinList = getUGVeins(%obj.getPosition());
		
		for (%i = 0; %i < getFieldCount(%veinList); %i++)
		{
			%testVein = getField(%veinList, getRandom(0, getFieldCount(%veinList)));
			if (%testVein.ready && (!isObject(%obj.drillingVein) || vectorDist(%obj.getPosition(), %obj.drillingVein.position) > vectorDist(%obj.getPosition(), %testVein.position)))
				%obj.drillingVein = %testVein;
		}
		
		return;
	}
	else if (%obj.GetMatter("Lubricant", "Input") > 0 && %obj.GetMatter(%vein.matter, "Output") < 16 && %obj.attemptPowerDraw($EOTW::PowerLevel[1]))
	{
		%amount = 1 + %obj.upgradeTier;
		%actualChange = %obj.ChangeMatter(%vein.matter, %amount, "Output");
		removeUGVeinOre(%vein, %actualChange);

		if (getUGVeinComp(%vein, %obj.getPosition()) <= 0)
			%obj.drillingVein = 0;
			
		if (%actualChange > 0 && getRandom() < 1/16)
		{
			%obj.ChangeMatter("Granite", 2, "Output");
			%obj.ChangeMatter("Lubricant", -1, "Input");
		}
	}
}

function brickEOTWDrillingRigData::getProcessingText(%this, %obj) {

	%veinSize = %obj.getUGVeinComp();
	%veinType = %obj.getUGVeinType();

    if (%veinSize > 0)
		return "\c6~" @ %veinSize SPC %veinType SPC "Left";
	else
		return "\c0No vein detected!";
}

function fxDtsBrick::getUGVeinComp(%obj)
{
	if (!isObject(%obj.drillingVein))
		return 0;

	return getUGVeinComp(%obj.drillingVein, %obj.getPosition());
}

function fxDtsBrick::getUGVeinType(%obj)
{
	if (!isObject(%obj.drillingVein))
		return "None";

	return %obj.drillingVein.matter;
}

datablock AudioProfile(ThumperLoopSound)
{
   filename    = "./Sounds/Thumper.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWThumperData)
{
	brickFile = "./Shapes/Thumper.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Mining Thumper";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Thumper";

	matterSize = 16;
	matterSlots["Input"] = 1;

	isPowered = true;
	powerType = "Machine";

	processSound = ThumperLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWThumperData"] = 1.00 TAB "7a7a7aff" TAB 512 TAB "Electrum" TAB 512 TAB "Energium" TAB 256 TAB "Teflon";
$EOTW::BrickDescription["brickEOTWThumperData"] = "When active gives a 100% speed boost (128 stud radius) to gathering nearby resources. Stacks. Requires lubricant.";

function brickEOTWThumperData::onTick(%this, %obj)
{
	if (%obj.GetMatter("Lubricant", "Input") > 0 && %obj.attemptPowerDraw($EOTW::PowerLevel[1] >> 1))
	{
		%obj.lastThump = getSimTime();
		if (getRandom() < 1/16)
			%obj.ChangeMatter("Lubricant", -1, "Input");
	}
}

datablock AudioProfile(HypersonicSpeakerLoopSound)
{
   filename    = "./Sounds/Speaker.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWHypersonicSpeakerData)
{
	brickFile = "./Shapes/Supersonic.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Hyper-sonic Speaker";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/SuperSonic";

	isPowered = true;
	powerType = "Machine";

	processSound = HypersonicSpeakerLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWHypersonicSpeakerData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Red Gold" TAB 256 TAB "Copper";
$EOTW::BrickDescription["brickEOTWHypersonicSpeakerData"] = "Prevents enemies from spawning on players within its 64 stud radius. Enemies can still wander in, however.";

$EOTW::BrickUpgrade["brickEOTWHypersonicSpeakerData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWHypersonicSpeakerData", 0] = 256 TAB "PlaSteel" TAB 128 TAB "Teflon";
$EOTW::BrickUpgrade["brickEOTWHypersonicSpeakerData", 1] = 256 TAB "Adamantine" TAB 128 TAB "Epoxy";

function brickEOTWHypersonicSpeakerData::onTick(%this, %obj)
{
	%multi = mPow(2, %obj.upgradeTier + 0);

	if (%obj.attemptPowerDraw($EOTW::PowerLevel[0] * %multi >> 1))
		for (%i = 0; %i < ClientGroup.getCount(); %i++)
			if (isObject(%player = ClientGroup.getObject(%i).player) && vectorDist(%player.getPosition(), %obj.getPosition()) < 32 * %multi)
				%player.lastSupersonicTick = getSimTime();
}

datablock AudioProfile(ChemDiffuserLoopSound)
{
   filename    = "./Sounds/ChemDiffuser.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWChemDiffuserData)
{
	brickFile = "./Shapes/Diffuser.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Chemical Diffuser";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Diffuser";

	isPowered = true;
	powerType = "Machine";

	matterSize = 512;
	matterSlots["Input"] = 1;

	processSound = ChemDiffuserLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWChemDiffuserData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Naturum" TAB 256 TAB "Quartz" TAB 128 TAB "Diamond";
$EOTW::BrickDescription["brickEOTWChemDiffuserData"] = "Disperses held potion matter to nearby players. Uses the same amount of matter regardless of player count.";

function brickEOTWChemDiffuserData::onTick(%this, %obj)
{
	%matterData = %obj.matter["Input", 0];
	%matter = getMatterType(getField(%matterData, 0));

	if (isObject(%image = %matter.potionType))
	{
		for (%i = 0; %i < ClientGroup.getCount(); %i++)
		{
			if (isObject(%player = ClientGroup.getObject(%i).player) && vectorDist(%player.getPosition(), %obj.getPosition()) < 8)
			{
				if (!%hasConsumed)
				{
					if (!%obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 2))
						return;

					%cost = 64 / %image.potionTime;
					%cost = getMax((%cost - mFloor(%cost) > getRandom() ? mCeil(%cost) : mFloor(%cost)), 1);
					%obj.ChangeMatter(%matter.name, %cost * -1, "Input");
					%hasConsumed = true;
				}
				
				%player.applyPotionEffect(%image.potionType, 1, true);
			}
			
		}
			
	}
}

datablock AudioProfile(TurretLoopSound)
{
   filename    = "./Sounds/Turret.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWTurretData)
{
	brickFile = "./Shapes/Turret.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Combat Turret";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Turret";

	matterSize = 512;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isPowered = true;
	powerType = "Machine";

	//processSound = TurretLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWTurretData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Red Gold" TAB 128 TAB "Diamond";
$EOTW::BrickDescription["brickEOTWTurretData"] = "Fires at enemies using whatever ammo it is loaded with.";

function fxDtsBrick::RetickTurret(%obj)
{
	cancel(%obj.RetickTurretSchedule);
	%obj.doingRetick = true;
	%obj.getDatablock().onTick(%obj);
}

function brickEOTWTurretData::onTick(%this, %obj)
{
	if (isObject(%client = findClientByBL_ID(%obj.getGroup().bl_id)) && (%obj.doingRetick || %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 2)))
	{
		%obj.doingRetick = false;
		%range = 8;

		if (!%obj.machineMuffled && getSimTime() - %obj.lastFleshCheck > 2000 && %obj.getMatter("Flesh", "Output") < %this.matterSize)
		{
			%obj.lastFleshCheck = getSimTime();
			initContainerRadiusSearch(%obj.getPosition(), %range * 1.5, $TypeMasks::CorpseObjectType);
			while(isObject(%hit = containerSearchNext()))
			{
				if (%hit.getClassName() $= "AIPlayer" && %hit.isGibbable && %hit.getState() $= "DEAD")
				{
					%fleshAmount = mCeil(%hit.getDatablock().maxDamage / $EOTW::GibHarvestDivider);

					if (%obj.getMatter("Flesh", "Output") + %fleshAmount > %this.matterSize)
						continue;

					%obj.ChangeMatter("Flesh", mCeil(%hit.getDatablock().maxDamage / $EOTW::GibHarvestDivider), "Output");
					%hit.removeBody(true);
				}
			}
		}

		if (isObject(%obj.turretTarget))
		{
			if (%obj.turretTarget.getState() $= "DEAD" || vectorDist(%obj.getPosition(), %obj.turretTarget.getPosition()) > (%range * 1.2))
			{
				%obj.turretTarget = "";
			}
			else
			{
				%ray = firstWord(containerRaycast(%obj.getPosition(), %obj.turretTarget.getPosition(), $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType));
				if (isObject(%ray))
				{
					%obj.turretTarget = "";
				}
			}
		}

		if (!isObject(%obj.turretTarget))
		{
			initContainerRadiusSearch(%obj.getPosition(), %range, $TypeMasks::PlayerObjectType);
			while(isObject(%hit = containerSearchNext()))
			{
				if (%hit.getClassName() !$= "AIPlayer" || %hit.getDatablock().hType !$= "enemy")
					continue;

				if(%hit.getState() !$= "DEAD")
				{
					%ray = firstWord(containerRaycast(%obj.getPosition(), %hit.getPosition(), $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType));
					if (!isObject(%ray))
					{
						%obj.turretTarget = %hit;
						break;
					}
				}
			}
		}

		if (isObject(%obj.turretTarget) && getSimTime() >= %obj.turretCooldown)
		{
			%matterData = %obj.matter["Input", 0];
			%matter = getMatterType(getField(%matterData, 0));
			%projectile = %matter.bulletType;
			if (isObject(%projectile))
			{
				switch$ (%matter.name)
				{
					case "Rifle Round":
						%bulletcount = 1;
						%spread = 0.00066;
						%cooldown = 50;
						ServerPlay3D("machineGunFire" @ getRandom(1, 4) @ "Sound",%obj.getPosition());
					case "Shotgun Pellet":
						%bulletcount = 7;
						%spread = 0.0013;
						%cooldown = 250;
						ServerPlay3D("shotgunFire" @ getRandom(1, 3) @ "Sound",%obj.getPosition());
					case "Launcher Load":
						%bulletcount = 3;
						%spread = 0.0005;
						%cooldown = 500;
						ServerPlay3D("gLauncherFire" @ getRandom(1, 2) @ "Sound",%obj.getPosition());
					default:
						%bulletcount = 1;
						%spread = 0.001;
						%cooldown = 600;
				}

				for (%i = 0; %i < %bulletcount; %i++)
				{
					%vector = VectorNormalize(vectorSub(vectorAdd(%obj.turretTarget.getPosition(), "0 0 " @ getWord(%obj.turretTarget.getDatablock().boundingBox, 2) / 8), %obj.getPosition()));
					%velocity = VectorScale(%vector, %projectile.muzzleVelocity);
					%velocity = vectorAdd(%velocity, %obj.turretTarget.getVelocity());
					%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
					%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
					%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
					%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
					%velocity = MatrixMulVector(%mat, %velocity);
					
					%p = new (Projectile)()
					{
						dataBlock = %projectile;
						initialVelocity = %velocity;
						initialPosition = %obj.getPosition();
						sourceObject = %client.player;
						client = %client;
					};
					MissionCleanup.add(%p);

					if (getRandom() > 0.6)
						%obj.ChangeMatter(%matter.name, -1, "Input");

					if (%obj.GetMatter(%matter.name, "Input") < 1)
						break;
				}
			}

			%obj.turretCooldown = uint_add(getSimTime(), %cooldown - 5);
			%obj.RetickTurretSchedule = %obj.schedule(%cooldown, "RetickTurret");
		}
	}
}

datablock AudioProfile(BiodomeLoopSound)
{
   filename    = "./Sounds/Bioreactor.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock fxDTSBrickData(brickEOTWBiodomeData)
{
	brickFile = "./Shapes/Bioreactor.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Biodome";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/Biodome";

	isPowered = true;
	powerType = "Machine";

	matterSize = 512;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Biodome";
	processSound = BiodomeLoopSound;
};
$EOTW::CustomBrickCost["brickEOTWBiodomeData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Quartz" TAB 256 TAB "Plastic" TAB 256 TAB "Uranium-238";
$EOTW::BrickDescription["brickEOTWBiodomeData"] = "Slowly grows plant life of your choice. Needs water. Speed and production can be boosted with ethylene.";

$EOTW::BrickUpgrade["brickEOTWBiodomeData", "MaxTier"] = 1;
$EOTW::BrickUpgrade["brickEOTWBiodomeData", 0] = 2048 TAB "Wood" TAB 256 TAB "Teflon" TAB 16 TAB "Uranium-235";

function brickEOTWBiodomeData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWBiodomeData::getProcessingText(%this, %obj) {
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
	subCategory = "Machines";
	uiName = "Void Drill";
	iconName = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Power/Icons/VoidDrill";

	isPowered = true;
	powerType = "Machine";

	matterSize = 512;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Drilling";
	processSound = VoidDrillSound;
};
$EOTW::CustomBrickCost["brickEOTWVoidDrillData"] = 1.00 TAB "7a7a7aff" TAB 666 TAB "Boss Essence" TAB 1920 TAB "Steel" TAB 12800 TAB "Granite";
$EOTW::BrickDescription["brickEOTWVoidDrillData"] = "Uses Boss Essence and tons of power to synthesize most raw materials.";

$EOTW::BrickUpgrade["brickEOTWVoidDrillData", "MaxTier"] = 2;
$EOTW::BrickUpgrade["brickEOTWVoidDrillData", 0] = 6660 TAB "Boss Essence" TAB 1280 TAB "Fluorspar" TAB 1280 TAB "Uraninite";
$EOTW::BrickUpgrade["brickEOTWVoidDrillData", 1] = 22200 TAB "Boss Essence" TAB 1280 TAB "Diamond" TAB 1280 TAB "Sturdium";

function brickEOTWVoidDrillData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWVoidDrillData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}