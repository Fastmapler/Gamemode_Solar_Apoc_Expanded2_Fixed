datablock PlayerData(InfernalMageHoleBot : PlayerStandardArmor)
{
	shapeFile = "./EnemyShapes/LANDdragon4.dts";
	uiName = "";
	minJetEnergy = 0;
	jetEnergyDrain = 0;
	canJet = 0;
	maxItems   = 0;
	maxWeapons = 0;
	maxTools = 0;
	jumpForce = 12 * 90;
	runforce = 40 * 90;
	maxForwardSpeed = 7;
	maxBackwardSpeed = 7;
	maxSideSpeed = 7;
	attackpower = 10; //What does this do?
	rideable = false;
	canRide = false;
	maxdamage = 2200;//Health
	jumpSound = "";
	
	
	boundingBox			= vectorScale("3.5 3.5 3.8", 4);
	crouchBoundingBox	= vectorScale("3.5 3.5 2.55", 4);
   
	useCustomPainEffects = true;
	PainHighImage = "PainHighImage";
	PainMidImage  = "PainMidImage";
	PainLowImage  = "PainLowImage";
	painSound     = "DragonPainSound";
	deathSound    = "DragonDeathSound";
	
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
	hName = "Dragon";//cannot contain spaces
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
	  hAttackDamage = 35;//15;//Melee Damage
	  hDamageType = "InfernalMageMelee";
	hShoot = 1;
	  hWep = InfernalMageMagicImage;
	  hShootTimes = 4;//Number of times the bot will shoot between each tick
	  hMaxShootRange = 2048;//The range in which the bot will shoot the player
	  hAvoidCloseRange = 1;//
		hTooCloseRange = 7;//in brick units
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
	
	scoreModifier = 0.6875;
	xpDrop = 3200;
};

datablock TSShapeConstructor(InfernalMageDts)
{
	baseShape  = "./EnemyShapes/LANDdragon4.dts";
	sequence0  = "./EnemyShapes/Ldrag_root.dsq root";

	sequence1  = "./EnemyShapes/Ldrag_run.dsq run";
	sequence2  = "./EnemyShapes/Ldrag_run.dsq walk";
	sequence3  = "./EnemyShapes/Ldrag_back.dsq back";
	sequence4  = "./EnemyShapes/Ldrag_side.dsq side";

	sequence5  = "./EnemyShapes/Ldrag_crouch.dsq crouch";
	sequence6  = "./EnemyShapes/Ldrag_crouchRun.dsq crouchRun";
	sequence7  = "./EnemyShapes/Ldrag_crouchback.dsq crouchBack";
	sequence8  = "./EnemyShapes/Ldrag_crouchSide.dsq crouchSide";

	sequence9  = "./EnemyShapes/Ldrag_look.dsq look";
	sequence10 = "./EnemyShapes/Ldrag_headside.dsq headside";
	sequence11 = "./EnemyShapes/Ldrag_root.dsq headUp";

	sequence12 = "./EnemyShapes/Ldrag_jump.dsq jump";
	sequence13 = "./EnemyShapes/Ldrag_jump.dsq standjump";
	sequence14 = "./EnemyShapes/Ldrag_fall.dsq fall";
	sequence15 = "./EnemyShapes/Ldrag_root.dsq land";

	sequence16 = "./EnemyShapes/Ldrag_root.dsq armAttack";
	sequence17 = "./EnemyShapes/Ldrag_mouthReady.dsq armReadyLeft";
	sequence18 = "./EnemyShapes/Ldrag_mouthReady.dsq armReadyRight";
	sequence19 = "./EnemyShapes/Ldrag_mouthReady.dsq armReadyBoth";
	sequence20 = "./EnemyShapes/Ldrag_root.dsq spearready";  
	sequence21 = "./EnemyShapes/Ldrag_fire1.dsq spearThrow";

	sequence22 = "./EnemyShapes/Ldrag_root.dsq talk";  

	sequence23 = "./EnemyShapes/Ldrag_death.dsq death1"; 
	
	sequence24 = "./EnemyShapes/Ldrag_fire1.dsq shiftUp";
	sequence25 = "./EnemyShapes/Ldrag_root.dsq shiftDown";
	sequence26 = "./EnemyShapes/Ldrag_fire1.dsq shiftAway";
	sequence27 = "./EnemyShapes/Ldrag_root.dsq shiftTo";
	sequence28 = "./EnemyShapes/Ldrag_root.dsq shiftLeft";
	sequence29 = "./EnemyShapes/Ldrag_root.dsq shiftRight";
	sequence30 = "./EnemyShapes/Ldrag_root.dsq rotCW";
	sequence31 = "./EnemyShapes/Ldrag_root.dsq rotCCW";

	sequence32 = "./EnemyShapes/Ldrag_root.dsq undo";
	sequence33 = "./EnemyShapes/Ldrag_fire1.dsq plant";

	sequence34 = "./EnemyShapes/Ldrag_pose.dsq sit";

	sequence35 = "./EnemyShapes/Ldrag_root.dsq wrench";

   sequence36 = "./EnemyShapes/Ldrag_fire1.dsq activate";
   sequence37 = "./EnemyShapes/Ldrag_fire1.dsq activate2";

   sequence38 = "./EnemyShapes/Ldrag_root.dsq leftrecoil";
   
   sequence39 = "./EnemyShapes/Ldrag_Rswipe.dsq LDattack1";
   sequence40 = "./EnemyShapes/Ldrag_Lslash.dsq LDattack2";
   sequence41 = "./EnemyShapes/Ldrag_FinalBite.dsq LDattack3";
   sequence42 = "./EnemyShapes/Ldrag_RswipeEND.dsq LDattack1END";
   sequence43 = "./EnemyShapes/Ldrag_LslashEND.dsq LDattack2END";
};

datablock AudioProfile(DragonPainSound)
{
   fileName = "./drpain.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(DragonDeathSound)
{
   fileName = "./drdeath1.wav";
   description = AudioClose3d;
   preload = true;
};

//<Magic/Healing Weapon>
datablock ParticleData(DragonbreathfireParticle)
{
	dragCoefficient      = 2.5;
	gravityCoefficient   = -0.17;
	inheritedVelFactor   = 0.8;
	constantAcceleration = 0;
	lifetimeMS           = 950;
	lifetimeVarianceMS   = 500;
	textureName          = "base/data/particles/cloud";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]	= "0.1 0.1 1 1";
	colors[1]	= "1 0.5 0 0.5";
	colors[2]	= "1 0.25 0 0.25";
	colors[3]	= "1 0 0 0";
	sizes[0]	= 0.4;
	sizes[1]	= 1.0;
	sizes[2]	= 1.2;
	sizes[3]	= 1.5;
	times[0]	= 0;
	times[1]	= 0.2;
	times[2]	= 0.8;
	times[3]	= 1;

	useInvAlpha = false;
};
datablock ParticleEmitterData(DragonbreathfireEmitter)
{
   ejectionPeriodMS = 2;
   periodVarianceMS = 0;
   ejectionVelocity = 15;
   velocityVariance = 5;
   ejectionOffset   = 0;
   thetaMin         = 0;
   thetaMax         = 5;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "DragonbreathfireParticle";

   uiName = "Dragonbreath Flame";
};

AddDamageType("InfernalMageMelee",   '%1 sacrificed itself.',    '%2 sacrificed %1.',0.2,1);
AddDamageType("InfernalMageMagic",   '%1 incinerated itself.',    '%2 incinerated %1.',0.2,1);
datablock ProjectileData(InfernalMageMagicProjectile)
{
   projectileShapeName = "base/data/shapes/empty.dts";
   directDamage        = 4;
   directDamageType    = $DamageType::InfernalMageMagic;
   radiusDamageType    = $DamageType::InfernalMageMagic;

   brickExplosionRadius = 0;
   brickExplosionImpact = false;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 10;
   brickExplosionMaxVolume = 1;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 2;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 400;
   verticalImpulse	  = 400;
   explosion           = "";
   particleEmitter     = "DragonbreathfireEmitter";
   sound = rocketLoopSound;

   muzzleVelocity      = 30;
   velInheritFactor    = 1;

   armingDelay         = 00;
   lifetime            = 10000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = true;
   gravityMod = 1.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "Mage Fireball";
};

datablock ItemData(InfernalMageMagicItem)
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
	uiName = "Mage Magic";
	iconName = "Add-Ons/Weapon_Gun/icon_gun";
	doColorShift = true;
	colorShiftColor = "0.25 0.25 0.25 1.000";

	 // Dynamic properties defined by the scripts
	image = InfernalMageMagicImage;
	canDrop = true;
};

datablock ShapeBaseImageData(InfernalMageMagicImage)
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
   item = InfernalMageMagicItem;
   ammo = " ";
   projectile = InfernalMageMagicProjectile;
   projectileType = Projectile;

	casing = gunShellDebris;
	shellExitDir        = "1.0 -1.3 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 15.0;	
	shellVelocity       = 7.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
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
	stateSound[3]					= rocketFireSound;

	stateName[4]					= "Reload";
	stateTransitionOnTimeout[4]     = "Ready";
	stateTransitionOnNoAmmo[4]		= "reviveCharge";
	stateTimeoutValue[4]            = 1.2;
	stateWaitForTimeout[4]			= true;
	
	stateName[5]					= "reviveCharge";
	stateTransitionOnTimeout[5]     = "Revive";
	stateScript[5]                  = "onReviveCharge";
	stateTimeoutValue[5]            = 1.2;
	stateWaitForTimeout[5]			= true;
	
	stateName[6]					= "Revive";
	stateTransitionOnTimeout[6]     = "Ready";
	stateScript[6]                  = "onRevive";
	stateTimeoutValue[6]            = 1.2;
	stateWaitForTimeout[6]			= true;


};

function InfernalMageMagicImage::onCharge(%this,%obj,%slot)
{
	%obj.playThread(2, "crouch");
	
	if (!%obj.infernalChargingAttack)
	{
		%obj.infernalChargingAttack = true;
		%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() / 4);
		%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() / 4);
		%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() / 4);
	}
}

function InfernalMageMagicImage::onFire(%this,%obj,%slot,%loopCount)
{
	if((%obj.lastFireTime+%this.minShotTime) > getSimTime() || %obj.getState() $= "DEAD")
		return;
		
	%obj.toRevive = "";
	
	%obj.lastFireTime = getSimTime();
	
	%obj.playThread(2, "activate2");

	%projectile = %this.projectile;
	%spread = 0.005;
	%shellcount = 10;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		if (%this.melee)
			%vector = %obj.getEyeVector();
		else
			%vector = %obj.getMuzzleVector(%slot);
			
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		
		if (getRandom() < 0.5)
			%x = ((getRandom() / 2) + 0.5) * 10 * 3.1415926 * %spread;
		else
			%x = ((getRandom() / -2) - 0.5) * 10 * 3.1415926 * %spread;
			
		if (getRandom() < 0.5)
			%y = ((getRandom() / 2) + 0.5) * 10 * 3.1415926 * %spread;
		else
			%y = ((getRandom() / -2) - 0.5) * 10 * 3.1415926 * %spread;
			
		if (getRandom() < 0.5)
			%z = ((getRandom() / 2) + 0.5) * 10 * 3.1415926 * %spread;
		else
			%z = ((getRandom() / -2) - 0.5) * 10 * 3.1415926 * %spread;
		
		%mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		if (%this.melee)
			%position = %obj.getEyePoint();
		else
			%position = %obj.getMuzzlePoint(%slot);
		
		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %position;
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
	
	if (%loopCount < 10)
	{
		%this.schedule(50,"onFire",%obj,%slot,%loopCount + 1);
		return %p;
	}
	
	initContainerRadiusSearch(%obj.getPosition(), 15, $TypeMasks::CorpseObjectType);
	
	for (%i = 0; isObject(%corpse = containerSearchNext()); %i++)
	{
		if (%corpse.getClassName() $= "AIPlayer" && !%corpse.isRevived && !%corpse.getDatablock().isBoss)
		{
			%obj.setImageAmmo(0,0);
			%obj.toRevive = %corpse;
			break;
		}
	}
	
	if (!isObject(%obj.toRevive))
	{
		%positionHit = %obj.getHackPosition();
    	InitContainerRadiusSearch(%positionHit, 16,     $TypeMasks::PlayerObjectType);
		%didHeal = false;
		
    	while(%hit = containerSearchNext())
		{
        	if ((!minigameCanDamage(%obj, %hit) || %hit.getClassName() $= "AIPlayer") && %obj != %hit && %hit.getDatablock().getName() !$= "InfernalMageHoleBot")
			{
				%hit.addHealth(50); //Change this to change the amount the AoE spell heals by.
				%didHeal = true;
				
				%p = new Projectile()
        		{
            			dataBlock = CureProjectile;
            			initialPosition = %hit.getHackPosition();
            			initialVelocity = 0;
            			sourceObject = %obj;
            			client = %obj.client;
           			sourceSlot = %slot;
           			scale = "0.7 0.7 0.7";
				}; 
				missionCleanup.add(%p); 
			}	
    	}
		
		if (%didHeal)
		{
			%p = new Projectile()
			{
				dataBlock = MedicaCastProjectile;
				initialVelocity = "0 0 5";
				initialPosition = %obj.getMuzzlePoint(%slot);
				sourceObject = %obj;
				sourceSlot = %slot;
				client = %obj.client;
			};
			MissionCleanup.add(%p);
			%obj.playThread(2, shiftUp);
			
			serverPlay3d(MedicaSound,%obj.getposition());
		}
	}
	
	if (%obj.infernalChargingAttack && !isObject(%obj.toRevive))
	{
		%obj.infernalChargingAttack = false;
		%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() * 4);
		%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() * 4);
		%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() * 4);
	}
	
	return %p;
}

function InfernalMageMagicImage::onReviveCharge(%this,%obj,%slot)
{
	if((%obj.lastFireTime+%this.minShotTime) > getSimTime() || %obj.getState() $= "DEAD" || !isObject(%obj.toRevive))
		return;
		
	%obj.setImageAmmo(0,1);
	%obj.playThread(2, "crouch");
	
	serverPlay3d(InfernalMiniboss_rangeFire,%obj.getposition());
	schedule(200,ClientGroup,"spawnBeam",%obj.getHackPosition(),%obj.toRevive.getPosition(),1);
	schedule(400,ClientGroup,"spawnBeam",%obj.getHackPosition(),%obj.toRevive.getPosition(),2);
	schedule(600,ClientGroup,"spawnBeam",%obj.getHackPosition(),%obj.toRevive.getPosition(),3);
	schedule(800,ClientGroup,"spawnBeam",%obj.getHackPosition(),%obj.toRevive.getPosition(),4);
	schedule(1000,ClientGroup,"spawnBeam",%obj.getHackPosition(),%obj.toRevive.getPosition(),5);
	schedule(1200,ClientGroup,"spawnBeam",%obj.getHackPosition(),%obj.toRevive.getPosition(),6);
	%obj.toRevive.schedule(200,"mountImage",healImage,1);
	%obj.toRevive.schedule(600,"mountImage",healImage,1);
	%obj.toRevive.schedule(1000,"mountImage",healImage,1);
}

function InfernalMageMagicImage::onRevive(%this,%obj,%slot)
{
	if((%obj.lastFireTime+%this.minShotTime) > getSimTime() || %obj.getState() $= "DEAD")
		return;
		
	%obj.playThread(2, "activate");
	reviveCorpse(%obj.toRevive);
	
	if (%obj.infernalChargingAttack)
	{
		%obj.infernalChargingAttack = false;
		%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() * 4);
		%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() * 4);
		%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() * 4);
	}
}

function reviveCorpse(%corpse)
{
	if (!isObject(%corpse))
		return;
		
	%enemy = spawnNewEnemy(%corpse.getPosition(),%corpse.getDatablock());
	%enemy.setDamageLevel(%enemy.getDatablock().maxDamage / 2);
	%enemy.isRevived = true;
	
	%corpse.delete();
}

datablock StaticShapeData(InfernoHealBeamStatic) { shapeFile = "Add-Ons/Weapon_Lightning_Gun/bullettrail.dts"; };
function spawnBeam(%startpos,%endpos,%size)
{
	%p = new StaticShape() { dataBlock = InfernoHealBeamStatic; };
	MissionCleanup.add(%p);
	
	%vel = vectorNormalize(vectorSub(%startpos,%endpos));
	%x = getWord(%vel,0)/2;
	%y = (getWord(%vel,1) + 1)/2;
	%z = getWord(%vel,2)/2;
	%p.setTransform(%endpos SPC VectorNormalize(%x SPC %y SPC %z) SPC mDegToRad(180));
	%p.setScale(%size SPC vectorDist(%startpos,%endpos) SPC %size);
}

function InfernoHealBeamStatic::onAdd(%this,%obj)
{
	%obj.playThread(0,root);
	%obj.schedule(1000,delete);
	//%obj.setVelocity(%obj.initalVelocity);
}