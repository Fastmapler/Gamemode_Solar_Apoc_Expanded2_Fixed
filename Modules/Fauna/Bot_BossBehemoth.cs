datablock PlayerData(BehemothHoleBot : UnfleshedHoleBot)
{
	mass				= 50000;
	runforce			= 40 * 50000;
	jumpForce			= 0;
	maxForwardSpeed		= 0;
	maxBackwardSpeed	= 0;
	maxSideSpeed		= 0;
	airControl			= 0;
	maxForwardCrouchSpeed		= 0;
	maxBackwardCrouchSpeed		= 0;
	maxSideCrouchSpeed			= 0;
	maxUnderwaterForwardSpeed	= 0;
	maxUnderwaterBackwardSpeed	= 0;
	maxUnderwaterSideSpeed		= 0;
	maxDamage			= 5000;
	lavaImmune			= true;
	sunImmune			= true;

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Behemoth";				//cannot contain spaces
	hTickRate = 3000;

	//Searching options
	hSearch	= 1;						//Search for Players
		hSearchRadius = 256;			//in brick units
		hSight = 0;						//Require bot to see player before pursuing
		hStrafe = 1;					//Randomly strafe while following player
	hSearchFOV = 1;						//if enabled disables normal hSearch
		hFOVRadius = 64;				//max 10

	//Attack Options
	hMelee = 0;							//Melee
		hAttackDamage = 24;				//Melee Damage
		hDamageType = "";
	hShoot = 1;
		hWep = "BehemothBossWeaponImage";
		hShootTimes = 1;				//Number of times the bot will shoot between each tick
		hMaxShootRange = 999;			//The range in which the bot will shoot the player
		hAvoidCloseRange = 0;
			hTooCloseRange = 7;			//in brick units
		isChargeWeapon = 0;				//If weapons should be charged to fire (ie spears)

	//Misc options
	hAvoidObstacles = 0;
	hSuperStacker = 1;
	hSpazJump = 1;						//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 0.1;					//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things
	hIdle = 1;							//Enables use of idle actions, actions which are done when the bot is not doing anything else
		hIdleAnimation = 0;				//Plays random animations/emotes, sit, click, love/hate/etc
		hIdleLookAtOthers = 1;			//Randomly looks at other players/bots when not doing anything else
			hIdleSpam = 0;				//Makes them spam click and spam hammer/spraycan
		hSpasticLook = 1;				//Makes them look around their environment a bit more.
	hEmote = 1;

	hPlayerscale = "2.0 2.0 2.0";		//The size of the bot

	isBoss = true;
	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 1.0 TAB 0.0;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 1.0 TAB 64 TAB 64 TAB "Wood";
};

datablock shapeBaseImageData(BehemothBossWeaponImage)
{
	shapeFile = "base/data/shapes/empty.dts";
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
	stateTimeoutValue[2]			= 3.0;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "Fire";
};

function BehemothBossWeaponImage::onFire(%this, %obj, %slot)
{
	if (!isObject(%target = %obj.hFollowing) || %target.getState() $= "DEAD" || %obj.getState() $= "DEAD" || getSimTime() - %obj.lastAttackTime < 2400)
		return;

	%obj.lastAttackTime = getSimTime();

	if (%obj.autoCount > 4)
	{
		//Special
		%obj.autoCount = 0;
	}
	else
	{
		//Auto attack
		if (%obj.getDamagePercent() > 0.5)
		{
			BehemothBossAttack_Auto1(%obj, 3);
			BehemothBossAttack_Auto2(%obj, 3);
		}
		else if (getRandom() < 0.5)
			BehemothBossAttack_Auto2(%obj, 3);
		else
			BehemothBossAttack_Auto1(%obj, 3);
		
		%obj.autoCount++;
	}
}

datablock AudioProfile(BehemothAgilityOrbSound)
{
    filename    = "./Sounds/BehemothAgilityOrb.wav";
    description = AudioHeirophant;
    preload = true;
};

datablock AudioProfile(BehemothTankOrbSound)
{
    filename    = "./Sounds/BehemothTankOrb.wav";
    description = AudioHeirophant;
    preload = true;
};

datablock AudioProfile(BehemothStandardOrbSound)
{
    filename    = "./Sounds/BehemothStandardOrb.wav";
    description = AudioHeirophant;
    preload = true;
};

function BehemothBossAttack_Auto1(%obj, %count)
{
	if (getRandom() < 0.1)
	{
		ServerPlay3D(BehemothStandardOrbSound, %obj.getPosition());
		%projectile = HeirophantStandardOrbProjectile;
	}

	else if (getRandom() < 0.5)
	{
		ServerPlay3D(BehemothAgilityOrbSound, %obj.getPosition());
		%projectile = HeirophantAgilityOrbProjectile;
	}
	else
	{
		ServerPlay3D(BehemothTankOrbSound, %obj.getPosition());
		%projectile = HeirophantTankOrbProjectile;
	}

	BehemothSummonHomingOrbs(%obj, %projectile);

	if (%count > 1)
		schedule(600, %obj, "BehemothBossAttack_Auto1", %obj, %count - 1);
}

function BehemothBossAttack_Auto2(%obj, %count)
{
	%projectile = RocketLauncherProjectile;
	%shellcount = 3;
	initContainerRadiusSearch(%obj.getPosition(), 64, $Typemasks::PlayerObjectType);
	while(isObject(%hit = containerSearchNext()))
    {
		if (%hit.getClassName() !$= "Player")
			continue;

		for(%shell=0; %shell<%shellcount; %shell++)
		{
			%position = vectorAdd(%obj.getEyePoint(), "0 0 4");
			%vector = vectorNormalize(vectorSub(%hit.getPosition(), %position));
			%objectVelocity = %obj.getVelocity();
			%vector1 = VectorScale(%vector, %projectile.muzzleVelocity * 0.4);
			%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
			if (%shell == 0)
				%velocity = VectorAdd(VectorAdd(%vector1,%vector2), %hit.getVelocity());
			else if (%shell == 1)
				%velocity = VectorSub(VectorAdd(%vector1,%vector2), %hit.getVelocity());
			else
				%velocity = VectorAdd(%vector1,%vector2);
			%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
			%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
			%velocity = MatrixMulVector(%mat, %velocity);
			
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

	if (%count > 1)
		schedule(600, %obj, "BehemothBossAttack_Auto2", %obj, %count - 1);
}

function BehemothSummonHomingOrbs(%obj, %projectile)
{
	%spread = 0.0;
	%shellcount = 1;

	//Summon an orb for each nearby player
	initContainerRadiusSearch(%obj.getPosition(), 64, $Typemasks::PlayerObjectType);
	while(isObject(%hit = containerSearchNext()))
    {
		if (%hit.getClassName() !$= "Player")
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