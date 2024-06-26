//spawnNewFauna(vectorAdd(%player.getPosition(), "0 0 15"), HeirophantHoleBot)
datablock PlayerData(HeirophantHoleBot : UnfleshedHoleBot)
{
	mass				= 900;
	runforce			= 40 * 900;
	maxForwardSpeed		= 4;
	maxBackwardSpeed	= 2;
	maxSideSpeed		= 1;
	maxDamage			= 6000;
	lavaImmune			= true;
	sunImmune			= true;
	hideBody			= true;

	boundingBox = VectorScale ("3.00 3.00 6.50", 4);
	crouchBoundingBox = VectorScale ("3.00 3.00 6.50", 4);

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Heirophant";				//cannot contain spaces
	hTickRate = 1500;

	//Searching options
	hSearch	= 1;						//Search for Players
		hSearchRadius = 256;			//in brick units
		hSight = 0;						//Require bot to see player before pursuing
		hStrafe = 1;					//Randomly strafe while following player
	hSearchFOV = 0;						//if enabled disables normal hSearch
		hFOVRadius = 256;				//max 10

	//Attack Options
	hMelee = 1;							//Melee
		hAttackDamage = 35;				//Melee Damage
		hDamageType = "";
	hShoot = 1;
		hWep = "HeirophantBossWeaponImage";
		hShootTimes = 4;				//Number of times the bot will shoot between each tick
		hMaxShootRange = 256;			//The range in which the bot will shoot the player
		hAvoidCloseRange = 0;
			hTooCloseRange = 0;			//in brick units
		isChargeWeapon = true;				//If weapons should be charged to fire (ie spears)

	//Misc options
	hAvoidObstacles = 0;
	hSuperStacker = 0;
	hSpazJump = 0;						//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 0.0;					//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things
	hIdle = 0;							//Enables use of idle actions, actions which are done when the bot is not doing anything else
		hIdleAnimation = 0;				//Plays random animations/emotes, sit, click, love/hate/etc
		hIdleLookAtOthers = 1;			//Randomly looks at other players/bots when not doing anything else
			hIdleSpam = 0;				//Makes them spam click and spam hammer/spraycan
		hSpasticLook = 1;				//Makes them look around their environment a bit more.
	hEmote = 0;

	hPlayerscale = "1.0 1.0 1.0";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death, Amount of bonus rerolls
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 100.0 TAB 0.0 TAB 2;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 97.0 TAB 222 TAB 222 TAB "Boss Essence";
	EOTWLootTable[1] = 1.0 TAB "ITEM" TAB crystalHalberdItem;
	EOTWLootTable[2] = 1.0 TAB "ITEM" TAB crystalBowItem;
	EOTWLootTable[3] = 1.0 TAB "ITEM" TAB crystalStaveItem;

	isBoss = true;
};

datablock shapeBaseImageData(HeirophantBossWeaponImage)
{
	shapeFile = "./Shapes/Heirophant.dts";
	item = "";
	
	mountPoint = 2;
	offset = "0 0 0";
	rotation = 0;
	
	eyeOffset = "0 0 0";
	eyeRotation = 0;
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = false;

	doColorShift = false;
	colorShiftColor = "0.0 0.0 0.0 1.0";
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]		= true;
	
	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.1;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "Ready";
};

function AIPlayer::CalculateBossAnger(%obj)
{
	return getMax(%obj.getDamagePercent(), 0.01);
}

datablock AudioDescription(AudioHeirophant)
{
	volume = 1;
	isLooping = 0;
	is3D = 0;
	ReferenceDistance = 64;
	maxDistance = 64;
	type = $SimAudioType;
};

datablock AudioProfile(HeirophantHomingSound)
{
   filename    = "./Sounds/HeiroOrbLoop.wav";
   description = AudioCloseLooping3d;
   preload = true;
};

datablock AudioProfile(HeirophantAttackSound)
{
    filename    = "./Sounds/HeiroFire.wav";
    description = AudioHeirophant;
    preload = true;
};

datablock AudioProfile(HeirophantCrossSound)
{
    filename    = "./Sounds/HeiroCross.wav";
    description = AudioHeirophant;
    preload = true;
};

datablock AudioProfile(HeirophantWarpSound)
{
    filename    = "./Sounds/HeiroWarp.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock StaticShapeData(EOTWDeathPillarStatic) { shapeFile = "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Fauna/Shapes/deathpillar.dts"; };
function EOTWDeathPillarStatic::onAdd(%this,%obj)
{
	%obj.playThread(0,root);
	%obj.schedule(750,delete);
}

AddDamageType("Heirophant", '[Smited] %1', '%2 [Smited] %1', 1, 1);
function DeathPillarKillCheck(%source, %pos)
{
	initContainerBoxSearch(%pos, "2 2 32", $TypeMasks::PlayerObjectType); //For some reason this search ends up being 10x10 studs horizontally instead of 8x8.
	while (%hit = containerSearchNext())
	{	
		if (getSimTime() - %hit.lastPillarHit > 100 && %hit != %source)
		{
			%hit.lastPillarHit = getSimTime();
			//%hit.Damage(%source, %hit.getPosition(), 10 + mCeil(%source.CalculateBossAnger() * 30), $DamageType::Heirophant);
			%hit.Damage(%source, %hit.getPosition(), 20, $DamageType::Heirophant);
		}
	}
}

//SpawnDeathPillar('',%player.getPosition());
function SpawnDeathPillar(%source, %pos)
{
	%p = new StaticShape() { dataBlock = EOTWDeathPillarStatic; };
	MissionCleanup.add(%p);
	%p.setTransform(%pos);
	%p.setScale("1.5 1.5 1");
	ServerPlay3D(HeirophantAttackSound, %pos);
	schedule(450, %p, "DeathPillarKillCheck", %source, %pos);
	return %p;
}

function SpawnDeathPillarArray(%source, %pos, %dir, %count)
{
	ServerPlay3D(HeirophantCrossSound, %pos);
	%spawnPos = vectorAdd(%pos, vectorScale(%dir, -1 * ((%count - 1) / 2)));
	for (%i = 0; %i < %count; %i++)
	{
		SpawnDeathPillar(%source, %spawnPos);
		%spawnPos = vectorAdd(%spawnPos, %dir);
	}
}

function SpawnDeathPillarChaser(%source, %pos, %target, %life, %delay)
{
	if (!isObject(%target) || getSimTime() > %life)
		return;

	%dir = vectorNormalize(getWords(vectorSub(%pos, %target.getPosition()), 0, 1) SPC "0");
	//%dir = mRound(getWord(%dir, 0)) SPC mRound(getWord(%dir, 1)) SPC "0";
	if (mAbs(getWord(%dir, 0)) > mAbs(getWord(%dir, 1)))
		%dir = getWord(%dir, 0) SPC "0" SPC "0";
	else
		%dir = "0" SPC getWord(%dir, 1) SPC "0";

	%dir = vectorScale(%dir, -5);
	schedule(%delay, %target, "SpawnDeathPillar", %source, vectorAdd(%pos, %dir)); %pos = vectorAdd(%pos, %dir);
	schedule(%delay * 2, %target, "SpawnDeathPillar", %source, vectorAdd(%pos, %dir)); %pos = vectorAdd(%pos, %dir);
	schedule(%delay * 3, %target, "SpawnDeathPillar", %source, vectorAdd(%pos, %dir)); %pos = vectorAdd(%pos, %dir);
	schedule(%delay * 3, %target, "SpawnDeathPillarChaser", %source, %pos, %target, %life, %delay);
}

function DeathPillarWarp(%source, %target)
{
	SpawnDeathPillar(%source, %source.getPosition());
	SpawnDeathPillar(%source, %target.getPosition());
	%source.schedule(450, "setTransform", %target.getTransform());
	schedule(540, %source, "spawnBeam", %source.getPosition(), %target.getPosition(), 8);

	ServerPlay3D(HeirophantWarpSound, %source.getPosition());
	ServerPlay3D(HeirophantWarpSound, %target.getPosition());
}

function DeathPillarCross(%source)
{
	%anger = %source.CalculateBossAnger();
	if (getRandom() < %anger)
	{
		%ignore = getRandom(0, 3);
		if (%ignore != 0) SpawnDeathPillarArray(%source, %source.getPosition(), "5 0 0", mCeil(%anger * 9) + 1);
		if (%ignore != 1) SpawnDeathPillarArray(%source, %source.getPosition(), "0 5 0", mCeil(%anger * 9) + 1);
		if (%ignore != 2) SpawnDeathPillarArray(%source, %source.getPosition(), "5 5 0", mCeil(%anger * 9) + 1);
		if (%ignore != 3) SpawnDeathPillarArray(%source, %source.getPosition(), "5 -5 0", mCeil(%anger * 9) + 1);
	}
	else if (getRandom() < 0.5)
	{
		SpawnDeathPillarArray(%source, %source.getPosition(), "5 0 0", mCeil(%anger * 9) + 1);
		SpawnDeathPillarArray(%source, %source.getPosition(), "0 5 0", mCeil(%anger * 9) + 1);
	}
	else
	{
		SpawnDeathPillarArray(%source, %source.getPosition(), "5 5 0", mCeil(%anger * 9) + 1);
		SpawnDeathPillarArray(%source, %source.getPosition(), "5 -5 0", mCeil(%anger * 9) + 1);
	}
}

function HeirophantBossWeaponImage::onFire(%this, %obj, %slot)
{
	if (!isObject(%target = %obj.hFollowing) || %target.getState() $= "DEAD" || %obj.getState() $= "DEAD" || getSimTime() - %obj.lastPillarFire < 6000 - (4000 * %obj.CalculateBossAnger()))
		return;

	%anger = %obj.CalculateBossAnger();

	%obj.lastPillarFire = getSimTime();

	%obj.attackCycle++;
	switch (%obj.attackCycle - 1)
	{
		case 0: //Swarmer shots
			for (%i = 0; %i < mCeil(%anger * 4); %i++)
				schedule(500 * %i, 0, "SpawnDeathPillarChaser", %obj, %obj.getPosition(), %target, getSimTime() + (4000 * (%i + 1)), 500 - (%anger * 300));
				if (%anger > 0.8)
					BeginSummonHomingOrbs(%obj);
		case 1: //Warp Attack
			for (%i = 0; %i < mCeil(%anger * 3); %i++)
				schedule(1000 * %i, 0, "DeathPillarWarp", %obj, %target);
				if (%anger > 0.4)
					BeginSummonHomingOrbs(%obj);
		case 2: //Cross Attacks
			for (%i = 0; %i < mCeil(%anger * 3); %i++)
				schedule(1000 * %i, 0, "DeathPillarCross", %obj);
				if (%anger > 0.6)
					BeginSummonHomingOrbs(%obj);
		default: //Orb attack
			BeginSummonHomingOrbs(%obj);
			%obj.attackCycle = 0;

	}
}
datablock AudioProfile(HeirophantAgilityOrbSound)
{
    filename    = "./Sounds/HeiroAgilityOrb.wav";
    description = AudioHeirophant;
    preload = true;
};

datablock AudioProfile(HeirophantTankOrbSound)
{
    filename    = "./Sounds/HeiroTankOrb.wav";
    description = AudioHeirophant;
    preload = true;
};

datablock AudioProfile(HeirophantOrbHitSound)
{
    filename    = "./Sounds/HeiroOrbHit.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock ExplosionData(HomingOrbExplosion : spearExplosion)
{
   explosionShape = "./Shapes/rangeExplosion.dts";
   soundProfile = HeirophantOrbHitSound;
   lifeTimeMS = 333;
   faceViewer = false;
   
   shakeCamera = true;
   camShakeFreq = "7.0 8.0 7.0";
   camShakeAmp = "1.5 1.5 1.5";
   camShakeDuration = 1.0;
   camShakeRadius = 15.0;

   // Dynamic light
   lightStartRadius = 4;
   lightEndRadius = 3;
   lightStartColor = "0.45 0.3 0.1";
   lightEndColor = "0 0 0";

   //impulse
   impulseRadius = 3.5;
   impulseForce = 1000;

   //radius damage
   radiusDamage        = 0;
   damageRadius        = 2.0;
};

datablock ProjectileData(HeirophantAgilityOrbProjectile)
{
	projectileShapeName = "./Shapes/OrbAgility.dts";
	directDamage        = 35;
	directDamageType    = $DamageType::Heirophant;

	explosion             = HomingOrbExplosion;
	stickExplosion        = HomingOrbExplosion;
	bloodExplosion        = HomingOrbExplosion;
	particleEmitter       = arrowTrailEmitter;
	explodeOnPlayerImpact = true;
	explodeOnDeath        = true;  

	armingDelay         = 8000;
	lifetime            = 8000;
	fadeDelay           = 8000;

	sound = HeirophantHomingSound;

	isBallistic         = false;
	bounceAngle         = 170; //stick almost all the time
	minStickVelocity    = 10;
	bounceElasticity    = 0.2;
	bounceFriction      = 0.01;   
	gravityMod = 0.0;

	hasLight    = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";

	muzzleVelocity      = 20;
	velInheritFactor    = 1;

	isHoming = 1;
	homingTurn = 1/20;

	protectType = "Agility";
};

datablock ProjectileData(HeirophantTankOrbProjectile : HeirophantAgilityOrbProjectile)
{
	projectileShapeName = "./Shapes/OrbTank.dts";
	protectType = "Tank";
};

datablock ProjectileData(HeirophantStandardOrbProjectile : HeirophantAgilityOrbProjectile)
{
	projectileShapeName = "./Shapes/OrbStandard.dts";
	protectType = "Standard";
};


function BeginSummonHomingOrbs(%obj)
{
	if (getSimTime() - %obj.lastOrbSummon < 4000)
		return;

	%obj.lastOrbSummon = getSimTime();

	if (getRandom() < 0.5)
	{
		%projectile = HeirophantAgilityOrbProjectile;
		initContainerRadiusSearch(%obj.getPosition(), 64, $Typemasks::PlayerObjectType);
		while(isObject(%hit = containerSearchNext())) if (isObject(%client = %hit.client)) %client.play2d(HeirophantAgilityOrbSound);
		schedule(33, %obj, "SummonHomingOrbs", %obj, %projectile);
	}
	else
	{
		%projectile = HeirophantTankOrbProjectile;
		initContainerRadiusSearch(%obj.getPosition(), 64, $Typemasks::PlayerObjectType);
		while(isObject(%hit = containerSearchNext())) if (isObject(%client = %hit.client)) %client.play2d(HeirophantTankOrbSound);
		schedule(2000, %obj, "SummonHomingOrbs", %obj, %projectile);
	}
}

function SummonHomingOrbs(%obj, %projectile)
{
	%spread = 0.0;
	%shellcount = 1;

	//Summon an orb for each nearby player
	initContainerRadiusSearch(%obj.getPosition(), 64, $Typemasks::PlayerObjectType);
	while(isObject(%hit = containerSearchNext()))
    {
		if (%hit.getClassName() !$= "Player" || !%obj.injuredBy[%hit])
			continue;

		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%vector = vectorNormalize(vectorSub(%hit.getPosition(), %obj.getPosition()));
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);
			%position = vectorAdd(%obj.getEyePoint(), "0 0 4");
			
			%p = new Projectile()
			{
				dataBlock = %projectile;
				initialVelocity = %velocity;
				initialPosition = %position;
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
				target = %hit;
			};
			MissionCleanup.add(%p);
		}
    }
}

package HomingOrbDamage
{
	function ProjectileData::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity)
	{
		if (%this.protectType !$= "")
		{
			if (%col.getDatablock().protectType $= %this.protectType)
			{
				%obj.explode();
				return;
			}
			else if (%col.getType() & $TypeMasks::PlayerObjectType && isObject(%client = %col.client))
			{
				%client.chatMessage("The projectile penetrates through your non-\c3" @ %this.protectType @ " \c0armor!");
			}
			else
			{
				return;
			}
		}

		parent::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity);
	}
};
activatePackage("HomingOrbDamage");
