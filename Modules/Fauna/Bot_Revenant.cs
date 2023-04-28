datablock PlayerData(RevenantHoleBot : UnfleshedHoleBot)
{
	runforce			= 50 * 90;
	maxForwardSpeed		= 4;
	maxBackwardSpeed	= 4;
	maxSideSpeed		= 4;
	maxDamage			= 150;
	lavaImmune			= false;

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Revenant";				//cannot contain spaces
	hTickRate = 3000;

	//Searching options
	hSearch	= 1;						//Search for Players
		hSearchRadius = 256;			//in brick units
		hSight = 1;						//Require bot to see player before pursuing
		hStrafe = 0;					//Randomly strafe while following player
	hSearchFOV = 1;						//if enabled disables normal hSearch
		hFOVRadius = 32;				//max 10

	//Attack Options
	hMelee = 1;							//Melee
		hAttackDamage = 20;				//Melee Damage
		hDamageType = "";
	hShoot = 1;
		hWep = "bioRifleImage";
		hShootTimes = 4;				//Number of times the bot will shoot between each tick
		hMaxShootRange = 32;			//The range in which the bot will shoot the player
		hAvoidCloseRange = 1;
			hTooCloseRange = 8;			//in brick units
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

	hPlayerscale = "0.7 0.7 1.2";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 2.2 TAB 0.4;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 1.0 TAB 16 TAB 16 TAB "Fluorspar";
	EOTWLootTable[1] = 1.0 TAB 16 TAB 16 TAB "Uraninite";
	EOTWLootTable[2] = 0.2 TAB "ITEM" TAB BossKeyItem;
};

//Datablocks for special bullet system.
datablock ProjectileData(BioRifleAgilityProjectile : BioRifleProjectile)
{
   projectileShapeName	= "./Shapes/OrbAgility.dts";
   directDamage			= 20;
   uiName 				= "";
   protectType			= "Agility";
};

datablock ProjectileData(BioRifleTankProjectile : BioRifleProjectile)
{
   projectileShapeName	= "./Shapes/OrbTank.dts";
   directDamage			= 20;
   uiName 				= "";
   protectType			= "Tank";
};

function BioRifleImage::onFire(%this, %obj, %slot)
{
	%obj.stopAudio(2);
	%obj.playThread(2, plant);
	
	%projectile = BioRifleAgilityProjectile;
	if (%obj.creationTime % 1000 < 500)
		%projectile = BioRifleTankProjectile;

	%shellcount = 1;
	%spread = 0;
	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
		%p.setScale(%obj.getScale());
	}

	%obj.setVelocity(VectorAdd(%obj.getVelocity(), VectorScale(%obj.getEyeVector(),"-0.8")));
}