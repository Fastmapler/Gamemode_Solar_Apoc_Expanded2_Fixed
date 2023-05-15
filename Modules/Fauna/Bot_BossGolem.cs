datablock PlayerData(InfernalRangerHoleBot : PlayerStandardArmor)
{
	mass = 900;
	shapeFile = "./EnemyShapes/zTank.dts";
	uiName = "";
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;
	maxItems   = 0;
	maxWeapons = 0;
	maxTools = 0;
	jumpForce = 12 * 900;
	runforce = 40 * 900;
	maxForwardSpeed = 9;
	maxBackwardSpeed = 9;
	maxSideSpeed = 9;
	attackpower = 10; //What does this do?
	rideable = false;
	canRide = false;
	maxdamage = 1300;//Health
	jumpSound = "";
	
	useCustomPainEffects = true;
	PainSound		= "";
	DeathSound		= InfernalRangerDeath_Sound;
	PainHighImage		= "PainHighImage";
	PainMidImage		= "PainMidImage";
	PainLowImage		= "PainLowImage";
	
	//Hole Attributes
	isHoleBot = 1;
	
	//Spawning option
	hSpawnTooClose = 0;//Doesn't spawn when player is too close and can see it
	  hSpawnTCRange = 8;//above range, set in brick units
	hSpawnClose = 0;//Only spawn when close to a player, can be used with above function as long as hSCRange is higher than hSpawnTCRange
	  hSpawnCRange = 64;//above range, set in brick units

	hType = enemy; //Enemy,Friendly, Neutral
	  hNeutralAttackChance = 100;
	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Golem";//cannot contain spaces
	hTickRate = 3000;
	
	//Wander Options
	hWander = 1;//Enables random walking
	  hSmoothWander = 1;
	  //hReturnToSpawn = 1;//Returns to spawn when too far //Always false
	  //hSpawnDist = 32;//Defines the distance bot can travel away from spawnbrick //Always 10000
	
	//Searching options
	hSearch = 1;//Search for Players
	  hSearchRadius = 2048;//in brick units
	  hSight = 1;//Require bot to see player before pursuing
	  hStrafe = 1;//Randomly strafe while following player
	hSearchFOV = 0;//if enabled disables normal hSearch
	  hFOVRadius = 32;//max 10
	   hHearing = 1;//If it hears a player it'll look in the direction of the sound

	  hAlertOtherBots = 1;//Alerts other bots when he sees a player, or gets attacked

	//Attack Options
	hMelee = 1;//Melee
	  hAttackDamage = 23;//15;//Melee Damage
	  hDamageType = "InfernalRangerMelee";
	hShoot = 1;
	  hWep = InfernalRangerStoneImage;
	  hShootTimes = 4;//Number of times the bot will shoot between each tick
	  hMaxShootRange = 2048;//The range in which the bot will shoot the player
	  hAvoidCloseRange = 1;//
		hTooCloseRange = 16;//in brick units
	//hHerding = 0;
	//hSound = 1;
	//hSpawnDetect = -1;//Will not spawn when user is too close and can see spawn
	

	
	//Misc options
	hAvoidObstacles = 1;
	hSuperStacker = 0;
	hSpazJump = 0;//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 1;//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things
	hIdle = 0;// Enables use of idle actions, actions which are done when the bot is not doing anything else
	  hIdleAnimation = 0;//Plays random animations/emotes, sit, click, love/hate/etc
	  hIdleLookAtOthers = 1;//Randomly looks at other players/bots when not doing anything else
	    hIdleSpam = 0;//Makes them spam click and spam hammer/spraycan
	  hSpasticLook = 1;//Makes them look around their environment a bit more.
	hEmote = 1;
	
	isBoss = true;

	EOTWLootTableData = 1.0 TAB 0.0;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 1.0 TAB 256 TAB 256 TAB "Diamond";
	EOTWLootTable[1] = 1.0 TAB 256 TAB 256 TAB "Sturdium";
};

package Ranger_ProjectileDeflect {
	function InfernalRangerHoleBot::Damage(%data, %obj, %sourceObject, %position, %damage, %damageType)
	{
		if (isObject(%sourceObject))
		{
			if (%sourceObject.client == %obj)
				%damage = 0;
			else if (%sourceObject.getClassName() $= "Projectile")
				%sourceObject.BounceTeleport(1.0, 1 / vectorLen(%sourceObject), %obj);
		}

		
		return parent::Damage(%data, %obj, %sourceObject, %position, %damage, %damageType);
	}
};
activatePackage("Ranger_ProjectileDeflect");

function Projectile::BounceTeleport(%obj, %factor, %teleScale, %client)
{
	%vel = %obj.getLastImpactVelocity ();
	%norm = %obj.getLastImpactNormal ();
	%bounceVel = VectorSub (%vel, VectorScale (%norm, VectorDot (%vel, %norm) * 2));
	%bounceVel = VectorScale (%bounceVel, %factor);
	if (VectorLen (%bounceVel) > 200)
	{
		%bounceVel = VectorScale (VectorNormalize (%bounceVel), 200);
	}
	%p = new Projectile ("")
	{
		dataBlock = %obj.getDataBlock ();
		initialPosition = vectorAdd(%obj.getLastImpactPosition(), vectorScale(%bounceVel, %teleScale));
		initialVelocity = %bounceVel;
		sourceObject = 0;
		sourceSlot = %obj.sourceSlot;
		client = %client;
	};
	if (%p)
	{
		MissionCleanup.add (%p);
		%p.setScale (%obj.getScale ());
		%p.spawnBrick = %obj.spawnBrick;
	}
	%obj.delete ();
}

// Load dts shapes and merge animations
datablock TSShapeConstructor(InfernalRangerDts)
{
	baseShape  = "./EnemyShapes/zTank.dts";
	sequence0  = "./EnemyShapes/zTankRoot.dsq root";

	sequence1  = "./EnemyShapes/zTankRun.dsq run";
	sequence2  = "./EnemyShapes/zTankRun.dsq walk";
	sequence3  = "./EnemyShapes/zTankRoot.dsq back";
	sequence4  = "./EnemyShapes/zTankRoot.dsq side";

	sequence5  = "./EnemyShapes/zTankCrouch.dsq crouch";
	sequence6  = "./EnemyShapes/zTankCrouchRun.dsq crouchRun";
	sequence7  = "./EnemyShapes/zTankRoot.dsq crouchBack";
	sequence8  = "./EnemyShapes/zTankRoot.dsq crouchSide";

	sequence9  = "./EnemyShapes/zTankRoot.dsq look";
	sequence10 = "./EnemyShapes/zTankRoot.dsq headside";
	sequence11 = "./EnemyShapes/zTankRoot.dsq headUp";

	sequence12 = "./EnemyShapes/zTankJump.dsq jump";
	sequence13 = "./EnemyShapes/zTankRoot.dsq standjump";
	sequence14 = "./EnemyShapes/zTankRoot.dsq fall";
	sequence15 = "./EnemyShapes/zTankRoot.dsq land";

	sequence16 = "./EnemyShapes/zTankRoot.dsq armAttack";
	sequence17 = "./EnemyShapes/zTankRoot.dsq armReadyLeft";
	sequence18 = "./EnemyShapes/zTankRoot.dsq armReadyRight";
	sequence19 = "./EnemyShapes/zTankRoot.dsq armReadyBoth";
	sequence20 = "./EnemyShapes/zTankRoot.dsq spearready";  
	sequence21 = "./EnemyShapes/zTankRoot.dsq spearThrow";

	sequence22 = "./EnemyShapes/zTankRoot.dsq talk";  

	sequence23 = "./EnemyShapes/zTankDie.dsq death1"; 
	
	sequence24 = "./EnemyShapes/zTankRoot.dsq shiftUp";
	sequence25 = "./EnemyShapes/zTankRoot.dsq shiftDown";
	sequence26 = "./EnemyShapes/zTankRoot.dsq shiftAway";
	sequence27 = "./EnemyShapes/zTankRoot.dsq shiftTo";
	sequence28 = "./EnemyShapes/zTankRoot.dsq shiftLeft";
	sequence29 = "./EnemyShapes/zTankRoot.dsq shiftRight";
	sequence30 = "./EnemyShapes/zTankRoot.dsq rotCW";
	sequence31 = "./EnemyShapes/zTankRoot.dsq rotCCW";

	sequence32 = "./EnemyShapes/zTankRoot.dsq undo";
	sequence33 = "./EnemyShapes/zTankRoot.dsq plant";

	sequence34 = "./EnemyShapes/zTankRoot.dsq sit";

	sequence35 = "./EnemyShapes/zTankRoot.dsq wrench";

   sequence36 = "./EnemyShapes/zTankRoot.dsq activate";
   sequence37 = "./EnemyShapes/zTankActivate2.dsq activate2";

   sequence38 = "./EnemyShapes/zTankRoot.dsq leftrecoil";
};  
datablock AudioProfile(InfernalRangerDeath_Sound)
{
	fileName = "./EnemyShapes/TankZombieDeath.wav";
	description = AudioClose3d;
	preload = true;
};

//<Weapon>
datablock ExplosionData(InfernalRangerStoneExplosion : spearExplosion)
{
   lifeTimeMS = 300;

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
   impulseRadius = 4;
   impulseForce = 2000;

   //radius damage
   radiusDamage        = 15;
   damageRadius        = 2.0;
};

AddDamageType("InfernalRangerMelee",   '%1 opened itself.',    '%2 opened %1.',0.2,1);
AddDamageType("InfernalRangerStone",   '%1 gored itself.',    '%2 gored %1.',0.2,1);
datablock ProjectileData(InfernalRangerStoneProjectile)
{
   projectileShapeName = "./EnemyShapes/Stone.dts";
   directDamage        = 5;
   directDamageType    = $DamageType::InfernalRangerStone;
   radiusDamageType    = $DamageType::InfernalRangerStone;

   brickExplosionRadius = 0;
   brickExplosionImpact = false;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 10;
   brickExplosionMaxVolume = 1;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 2;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 400;
   verticalImpulse	  = 400;
   explosion           = InfernalRangerStoneExplosion;
   particleEmitter     = ""; //bulletTrailEmitter;

   muzzleVelocity      = 40;
   velInheritFactor    = 0;

   armingDelay         = 00;
   lifetime            = 10000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = false;
   gravityMod = 1.00;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "Ranger Stone";
};

datablock ItemData(InfernalRangerStoneItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "Add-Ons/Weapon_Gun/pistol.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Ranger Stone";
	iconName = "Add-Ons/Weapon_Gun/icon_gun";
	doColorShift = true;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	 // Dynamic properties defined by the scripts
	image = InfernalRangerStoneImage;
	canDrop = true;
};

datablock ShapeBaseImageData(InfernalRangerStoneImage)
{
   // Basic Item properties
   shapeFile = "base/data/shapes/empty.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = InfernalRangerStoneItem;
   ammo = " ";
   projectile = InfernalRangerStoneProjectile;
   projectileType = Projectile;

	casing = gunShellDebris;
	shellExitDir        = "1.0 -1.3 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 15.0;	
	shellVelocity       = 7.0;

   //melee particles shoot from eye node for consistancy
   melee = true;
   //raise your arm up or not
   armReady = false;

   doColorShift = true;
   colorShiftColor = gunItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.15;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Charge";
	stateAllowImageChange[1]		= true;
	stateSequence[1]				= "Ready";
	
	stateName[2]					= "Charge";
	stateTransitionOnTimeout[2]     = "Fire";
	stateTimeoutValue[2]            = 0.6;
	stateWaitForTimeout[2]			= true;
	stateScript[2]                  = "onCharge";
	stateAllowImageChange[2]        = false;

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "Reload";
	stateTimeoutValue[3]            = 0.6;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]			= true;
	stateSound[3]					= spearFireSound;

	stateName[4]					= "Reload";
	stateTransitionOnTimeout[4]     = "Ready";
	stateTimeoutValue[4]            = 1.2;
	stateWaitForTimeout[4]			= true;

};

function InfernalRangerStoneImage::onCharge(%this,%obj,%slot)
{
	%obj.playThread(2, "talk");
	
	if (!%obj.infernalChargingAttack)
	{
		%obj.infernalChargingAttack = true;
		%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() / 4);
		%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() / 4);
		%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() / 4);
	}
}

function InfernalRangerStoneImage::onFire(%this,%obj,%slot)
{
	if((%obj.lastFireTime+%this.minShotTime) > getSimTime() || %obj.getState() $= "DEAD")
		return;
		
	%obj.lastFireTime = getSimTime();
	
	%obj.playThread(2, "activate2");
	
	if (%obj.infernalChargingAttack)
	{
		%obj.infernalChargingAttack = false;
		%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() * 4);
		%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() * 4);
		%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() * 4);
	}

	%projectile = %this.projectile;
	
	%shellcount = 9;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		if (%this.melee)
			%vector = %obj.getEyeVector();
		else
			%vector = %obj.getMuzzleVector(%slot);

		if (%shell == 0)
			%spread = 0.0;
		else
			%spread = 0.01;
			
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		if (%this.melee)
			%position = %obj.getEyePoint();
		else
			%position = %obj.getMuzzlePoint(%slot);

		if (isObject(%followTarget = %obj.hFollowing) && %followTarget.getType() & ($TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType))
			%velocity = vectorAdd(%followTarget.getVelocity(), %velocity);
		
		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %position;
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		%p.setScale("0.5 0.5 0.5");
		MissionCleanup.add(%p);
	}
	%this.schedule(50,"onFireFissure",%obj,%slot,0);
	
	return %p;
}

datablock AudioProfile(InfernalRangerFissure_Sound)
{
	fileName = "./EnemyShapes/Fissure_pop.wav";
	description = AudioDefault3d;
	preload = true;
};

datablock ParticleData(RangerFissureParticle)
{
   dragCoefficient      = 0.2;
   gravityCoefficient   = -0.2;
   inheritedVelFactor   = 0.2;
   constantAcceleration = 0.0;
   spinRandomMin = -20;
   spinRandomMax = 20;
   lifetimeMS           = 500;
   lifetimeVarianceMS   = 300;
   textureName          = "base/data/particles/star1";
   colors[0]     = "0.9 0.9 0.9 0.0";
   colors[1]     = "0.9 0.9 0.9 1.0";
   colors[2]     = "0.9 0.9 0.9 0.0";
   sizes[0]      = 0.25;
   sizes[1]      = 0.75;
   sizes[2]      = 0.25;
   times[0]		 = 0.0;
   times[1]		 = 0.5;
   times[2]		 = 1.0;
};

datablock ParticleEmitterData(RangerFissureEmitter)
{
   lifetimeMS		= 2000;
   ejectionPeriodMS = 20;
   periodVarianceMS = 0;
   ejectionVelocity = 1;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 60;
   phiReferenceVel  = 0;
   phiVariance      = 0;
   overrideAdvance = false;
   particles = "RangerFissureParticle";

   uiName = "";
};

AddDamageType("RangerFissure",   'Fissure %1',    '%2 Fissure %1',0.75,1);
datablock ProjectileData(RangerFissureProjectile)
{
	projectileShapeName = "Add-Ons/Weapon_Gun/bullet.dts";
   directDamage        = 10;
   directDamageType  = $DamageType::RangerFissure;
   radiusDamageType  = $DamageType::RangerFissure;
   particleEmitter     = RangerFissureEmitter;

	sound = InfernalRangerFissure_Sound;
	
   muzzleVelocity      = 30;
   velInheritFactor    = 0;
   
   impactImpulse       = 1000;
   verticalImpulse     = 500; 

   armingDelay         = 2000;
   lifetime            = 2000;
   fadeDelay           = 70;
   bounceElasticity    = 0.75;
   bounceFriction      = 0;
   isBallistic         = true;
   gravityMod = 1.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "";
};

function RangerFissureProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity)
{
	if (isObject(%obj.sourceObject) && %col.getID() == %obj.sourceObject.getID())
		return;

	Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal, %velocity);
}

function InfernalRangerStoneImage::onFireFissure(%this,%obj,%slot,%amt) //Fissure Attack
{
	%amt = %amt + 0;
	if ((%obj.usingFissureAttack && %amt == 0) || %obj.getState() $= "DEAD") return;
	
	%obj.usingFissureAttack = true;
	
	%obj.setMaxForwardSpeed(0);
	%obj.setMaxSideSpeed(0);
	%obj.setMaxBackwardSpeed(0);
	%obj.setMaxCrouchForwardSpeed(0);
	%obj.setMaxCrouchSideSpeed(0);
	%obj.setMaxCrouchBackwardSpeed(0);
	
	%projectile = RangerFissureProjectile;
	%spread = 0.1;
	%shellcount = 25;
	for(%i=1;%i<=%shellcount;%i++)
	{
		if (!%obj.isGroundedSport() || %amt == 0) break;
		%obj.playThread(2, shiftDown);
		%vector = %obj.getEyeVector();
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);
		%velocity = setWord(%velocity,2,0);
		
		if (!%obj.isCrouched()) %initPos = vectorSub(%obj.getEyePoint(),"0 0 2");
		else %initPos = %obj.getEyePoint();
		
		%p = new Projectile()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %initPos;
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	if (%amt < 3) %this.schedule(333,"onFireFissure",%obj,%slot,%amt + 1);
	else
	{
		%obj.setMaxForwardSpeed(%obj.getDataBlock().MaxForwardSpeed);
		%obj.setMaxSideSpeed(%obj.getDataBlock().MaxSideSpeed);
		%obj.setMaxBackwardSpeed(%obj.getDataBlock().MaxBackwardSpeed);
		%obj.setMaxCrouchForwardSpeed(%obj.getDataBlock().MaxForwardCrouchSpeed);
		%obj.setMaxCrouchSideSpeed(%obj.getDataBlock().MaxSideCrouchSpeed);
		%obj.setMaxCrouchBackwardSpeed(%obj.getDataBlock().MaxBackwardCrouchSpeed);
		%obj.usingFissureAttack = false;
	}
}