datablock fxDTSBrickData(brickEOTWWaterPumpData)
{
	brickFile = "./Shapes/WaterPump.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Water Pump";
	//iconName = "";

    isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Output"] = 1;
    inspectMode = 1;
};
$EOTW::CustomBrickCost["brickEOTWWaterPumpData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Iron" TAB 128 TAB "Copper" TAB 128 TAB "Lead";
$EOTW::BrickDescription["brickEOTWWaterPumpData"] = "A device that draws water deep within the ground. Can be operated manually.";

function brickEOTWWaterPumpData::onTick(%this, %obj) {
	if (getSimTime() - %obj.lastDrawSuccess >= 100 && %obj.GetMatter("Water", "Output") < 128 && %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 4))
	{
        %obj.ChangeMatter("Water", 4, "Output");
	}
}

function brickEOTWWaterPumpData::onInspect(%this, %obj, %client) {
    if (%obj.GetMatter("Water", "Output") < 128 && getSimTime() - %obj.lastDrawSuccess >= 100)
    {
        %obj.lastDrawTime = getSimTime();
		%obj.lastDrawSuccess = getSimTime();
        %obj.ChangeMatter("Water", 4, "Output");
    }
}

datablock fxDTSBrickData(brickEOTWOilRigData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Oil Rig";

	matterSize = 16;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 2;

	isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWOilRigData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Rubber" TAB 256 TAB "Steel" TAB 256 TAB "Copper";
$EOTW::BrickDescription["brickEOTWOilRigData"] = "A large construct which slowly pumps crude oil. Needs lubricant to function. Also periodically spits out Granite.";

function brickEOTWOilRigData::onTick(%this, %obj)
{
	if (%obj.GetMatter("Lubricant", "Input") > 0 && %obj.GetMatter("Crude Oil", "Output") < 16 && %obj.attemptPowerDraw($EOTW::PowerLevel[1] >> 1))
	{
		%obj.ChangeMatter("Crude Oil", 1, "Output");

		if (getRandom() < 1/16)
		{
			%obj.ChangeMatter("Granite", 2, "Output");
			%obj.ChangeMatter("Lubricant", -1, "Input");
		}
	}
}

datablock fxDTSBrickData(brickEOTWThumperData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Mining Thumper";

	matterSize = 16;
	matterSlots["Input"] = 1;

	isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWThumperData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Adamantine" TAB 256 TAB "Lead" TAB 128 TAB "Teflon";
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

datablock fxDTSBrickData(brickEOTWSupersonicSpeakerData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Super-sonic Speaker";

	isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWSupersonicSpeakerData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Rubber" TAB 256 TAB "Steel" TAB 256 TAB "Copper";
$EOTW::BrickDescription["brickEOTWSupersonicSpeakerData"] = "Prevents enemies from spawning in its 64 stud radius. Enemies can still wander in, however.";

function brickEOTWSupersonicSpeakerData::onTick(%this, %obj)
{
	if (%obj.attemptPowerDraw($EOTW::PowerLevel[1] >> 1))
		for (%i = 0; %i < ClientGroup.getCount(); %i++)
			if (isObject(%player = ClientGroup.getObject(%i).player) && vectorDist(%player.getPosition(), %obj.getPosition()) < 32)
				%player.lastSupersonicTick = getSimTime();
}

datablock fxDTSBrickData(brickEOTWChemDiffuserData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Chemical Diffuser";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 1;
};
$EOTW::CustomBrickCost["brickEOTWChemDiffuserData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Rubber" TAB 256 TAB "Steel" TAB 256 TAB "Copper";
$EOTW::BrickDescription["brickEOTWChemDiffuserData"] = "Disperses held potion matter to nearby players. Uses the same amount of matter regardless of player count.";

function brickEOTWChemDiffuserData::onTick(%this, %obj)
{
	%matterData = %obj.matter[%type, %i];
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
					%cost = getMax((%cost - mFloor(%cost) > getRandom() ? mCeil(%cost), mFloor(%cost)), 1);
					%obj.ChangeMatter(%matter.name, %cost * -1, "Input");
				}
				
				%player.applyPotionEffect(%image.potionType, 1);
			}
			
		}
			
	}
}

datablock fxDTSBrickData(brickEOTWTurretData)
{
	brickFile = "./Shapes/Generator.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Combat Turret";

	matterSize = 256;
	matterSlots["Input"] = 1;
	matterSlots["Output"] = 1;

	isPowered = true;
	powerType = "Machine";
};
$EOTW::CustomBrickCost["brickEOTWTurretData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Steel" TAB 256 TAB "Electrum" TAB 128 TAB "Diamond";
$EOTW::BrickDescription["brickEOTWTurretData"] = "Fires at enemies using whatever ammo it is loaded with. Will also gather flesh when possible.";

function fxDtsBrick::RetickTurret(%obj)
{
	cancel(%obj.RetickTurretSchedule);
	%obj.doingRetick = true;
	%obj.getDatablock().onTick(%obj);
}

function brickEOTWTurretData::onTick(%this, %obj)
{
	if (%obj.doingRetick || %obj.attemptPowerDraw($EOTW::PowerLevel[0] >> 2))
	{
		%obj.doingRetick = false;
		%range = 8;

		if (isObject(%obj.turretTarget))
		{
			if (%obj.turretTarget.getState() $= "DEAD" || vectorDist(%obj.getPosition(), %obj.turretTarget.getPosition()) > %range)
				%obj.turretTarget = "";
			else
			{
				%ray = firstWord(containerRaycast(%obj.getPosition(), %obj.turretTarget.getPosition(), $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType));
				if (isObject(%ray))
					%obj.turretTarget = "";
			}
		}
		else
		{
			initContainerRadiusSearch(%obj.getPosition(), %range, $TypeMasks::PlayerObjectType);
			while(isObject(%hit = containerSearchNext()))
			{
				//TODO: Auto gib nearby monsters for loot

				if(%hit.getClassName() $= "AIPlayer" && %hit.getState() !$= "DEAD" && %hit.getDatablock().hType $= "enemy")
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
			%matterData = %obj.matter[%type, %i];
			%matter = getMatterType(getField(%matterData, 0));
			%projectile = %matter.bulletType;
			switch$ (%matter.name)
			{
				case "Rifle Round":
					%bulletcount = 1;
					%spread = 0.00066;
					%cooldown = 100;
				case "Shotgun Pellet":
					%bulletcount = 7;
					%spread = 0.0013;
					%cooldown = 800;
				case "Launcher Load":
					%bulletcount = 3;
					%spread = 0.0005;
					%cooldown = 500;
				default:
					%bulletcount = 1;
					%spread = 0.001;
					%cooldown = 1200;
			}

			if (isObject(%projectile))
			{
				for (%i = 0; %i < %bulletcount; %i++)
				{
					%vector = VectorNormalize(vectorSub(%obj.turretTarget.getPosition(), %obj.getPosition()));
					%velocity = VectorScale(%vector, %projectile.muzzleVelocity);
					%velocity = vectorAdd(%velocity, %obj.turretTarget.getVelocity());
					%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
					%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
					%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
					%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
					%velocity = MatrixMulVector(%mat, %velocity);

					if (isObject(%client = getClientFromBL_ID(%obj.getGroup().bl_id)))
						%sourceObject = %client.player;
					else
						%sourceObject = %obj;

					%p = new (Projectile)()
					{
						dataBlock = %projectile;
						initialVelocity = %velocity;
						initialPosition = %obj.getPosition();
						sourceObject = %sourceObject;
						client = %client;
					};
					MissionCleanup.add(%p);
				}

				ServerPlay3D(TurretHeavyFireSound,%obj.getPosition());
			}

			%obj.turretCooldown = uint_add(getSimTime(), %cooldown - 5);
			%obj.RetickTurretSchedule = %obj.schedule(%cooldown, "RetickTurret");
		}
	}
}

datablock fxDTSBrickData(brickEOTWBiodomeData)
{
	brickFile = "./Shapes/Bioreactor.blb";
	category = "Solar Apoc";
	subCategory = "Machines";
	uiName = "Biodome";

	isPowered = true;
	powerType = "Machine";

	matterSize = 128;
	matterSlots["Input"] = 2;
	matterSlots["Output"] = 1;

	isProcessingMachine = true;
	processingType = "Biodome";
};
$EOTW::CustomBrickCost["brickEOTWBiodomeData"] = 1.00 TAB "7a7a7aff" TAB 256 TAB "Quartz" TAB 256 TAB "Plastic" TAB 256 TAB "Uranium-238";
$EOTW::BrickDescription["brickEOTWBiodomeData"] = "Slowly grows plant life of your choice. Needs water. Speed and production can be boosted with ethylene.";

function brickEOTWBiodomeData::onTick(%this, %obj) { %obj.runProcessingTick(); }

function brickEOTWBiodomeData::getProcessingText(%this, %obj) {
    if (isObject(%obj.processingRecipe))
		return "Recipe:\c3" SPC cleanRecipeName(%obj.processingRecipe);
	else
		return "\c0No Recipe (/SetRecipe)";
}