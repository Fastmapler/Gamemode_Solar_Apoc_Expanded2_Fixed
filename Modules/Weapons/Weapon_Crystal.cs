//Halberd
AddDamageType("crystalHalberd",   '<bitmap:add-ons/Weapon_Sword/CI_sword> %1',    '%2 <bitmap:add-ons/Weapon_Sword/CI_sword> %1',0.75,1);
datablock ProjectileData(crystalHalberdProjectile)
{
   directDamage			= 40;
   directDamageType		= $DamageType::crystalHalberd;
   radiusDamageType		= $DamageType::crystalHalberd;
   explosion			= swordExplosion;
   DamageStyle			= "Melee";
   
   impactImpulse		= 600;
   verticalImpulse		= 600;

   muzzleVelocity		= 75;
   velInheritFactor		= 1;

   armingDelay			= 0;
   lifetime				= 100;
   fadeDelay			= 70;
   bounceElasticity		= 0;
   bounceFriction		= 0;
   isBallistic			= false;
   gravityMod			= 0.0;

   hasLight				= false;
   lightRadius			= 3.0;
   lightColor			= "0 0 0.5";
};

datablock ItemData(crystalHalberdItem : swordItem)
{
	shapeFile = "./Shapes/Crystal_Halberd.dts";
	uiName = "Crystal Halberd";
	doColorShift = true;
	colorShiftColor = "0.875 0.875 0.875 1.0";

	image = crystalHalberdImage;
	canDrop = true;
	//iconName = "";
};

datablock ShapeBaseImageData(crystalHalberdImage)
{
	shapeFile = "./Shapes/Crystal_Halberd.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = crystalHalberdItem;
	ammo = " ";
	projectile = crystalHalberdProjectile;
	projectileType = Projectile;

	melee = true;
	doRetraction = false;

	armReady = true;

	doColorShift = true;
	colorShiftColor = crystalHalberdItem.colorShiftColor;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSound[0]					= swordDrawSound;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "CheckFire";
	stateTimeoutValue[2]            = 0.5;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;

	stateName[3]					= "CheckFire";
	stateTransitionOnTriggerUp[3]	= "Ready";
	stateTransitionOnTriggerDown[3]	= "Fire";
};

function crystalHalberdImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	Parent::onFire(%this, %obj, %slot);
	%this.schedule(75, "onReFire", %obj, %slot);
	%this.schedule(150, "onReFire", %obj, %slot);
}

function crystalHalberdImage::onReFire(%this, %obj, %slot)
{
	Parent::onFire(%this, %obj, %slot);
}

//Bow
AddDamageType("crystalBow",   '<bitmap:add-ons/Weapon_Bow/CI_arrow> %1',    '%2 <bitmap:add-ons/Weapon_Bow/CI_arrow> %1',0.5,1);
datablock ProjectileData(crystalBowProjectile)
{
   projectileShapeName = "./Shapes/Crystal_Arrow.dts";

   directDamage        = 30;
   directDamageType    = $DamageType::crystalBow;

   radiusDamage        = 0;
   damageRadius        = 0;
   radiusDamageType    = $DamageType::crystalBow;

   explosion             = arrowExplosion;
   stickExplosion        = arrowStickExplosion;
   bloodExplosion        = arrowStickExplosion;
   particleEmitter       = arrowTrailEmitter;
   explodeOnPlayerImpact = true;
   explodeOnDeath        = true;  

   armingDelay         = 0;
   lifetime            = 4000;
   fadeDelay           = 4000;

   isBallistic         = false;
   bounceAngle         = 170; //stick almost all the time
   minStickVelocity    = 10;
   bounceElasticity    = 0.2;
   bounceFriction      = 0.01;   
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   muzzleVelocity      = 65;
   velInheritFactor    = 1;

   isHoming = 1;
   homingTurn = 1/30;
};

datablock ItemData(crystalBowItem : swordItem)
{
	shapeFile = "./Shapes/Crystal_Bow.dts";
	uiName = "Crystal Bow";
	doColorShift = true;
	colorShiftColor = "0.875 0.875 0.875 1.0";
	
	iconName = "Add-Ons/Weapon_Bow/icon_bow";

	image = crystalBowImage;
	canDrop = true;
	//iconName = "";
};

datablock ShapeBaseImageData(crystalBowImage)
{
	shapeFile = "./Shapes/Crystal_Bow.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = crystalBowItem;
	ammo = " ";
	projectile = crystalBowProjectile;
	projectileType = Projectile;

	melee = false;
	doRetraction = false;

	armReady = true;

	doColorShift = true;
	colorShiftColor = crystalBowItem.colorShiftColor;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSound[0]					= playerMountSound;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "CheckFire";
	stateTimeoutValue[2]            = 1.0;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;
	stateSound[2]					= bowFireSound;

	stateName[3]					= "CheckFire";
	stateTransitionOnTriggerUp[3]	= "Ready";
	stateTransitionOnTriggerDown[3]	= "Fire";
};

function crystalBowImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, plant);
	%projectile = %this.Projectile;
	%shellcount = 4;
	%spread = 0.001;
	

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		

		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);
		// %x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		// %y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		// %z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		// %mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
		switch (%shell)
		{
			case 0: %mat = MatrixCreateFromEuler("0 0 0");
			case 1: %mat = MatrixCreateFromEuler(vectorScale("-15.7 15.7 -15.7", %spread));
			case 2: %mat = MatrixCreateFromEuler(vectorScale("15.7 -15.7 -15.7", %spread));
			case 3: %mat = MatrixCreateFromEuler(vectorScale("0 0 15.7", %spread));
		}
		
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
	}
}

//Stave
AddDamageType("crystalStave",   '<bitmap:add-ons/Weapon_Bow/CI_arrow> %1',    '%2 <bitmap:add-ons/Weapon_Bow/CI_arrow> %1',0.5,1);

//ParticleData
datablock ParticleData(crystalStaveExplosionParticle)
{
	dragCoefficient      = 3;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 250;
	lifetimeVarianceMS   = 50;
	textureName          = "base/data/particles/dot";
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	colors[0]     = "0.0 1.0 1.0 0.9";
	colors[1]     = "0.0 0.2 1.0 0.0";
	sizes[0]      = 2.0;
	sizes[1]      = 0.0;

	useInvAlpha = true;
};

datablock ParticleEmitterData(crystalStaveExplosionEmitter)
{
   ejectionPeriodMS = 2;
   periodVarianceMS = 0;
   ejectionVelocity = 20;
   velocityVariance = 5.0;
   ejectionOffset   = 3.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "crystalStaveExplosionParticle";

   uiName = "";
   emitterNode = TenthEmitterNode;
};


datablock ParticleData(crystalStaveExplosionRingParticle)
{
	dragCoefficient      = 8;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 40;
	lifetimeVarianceMS   = 10;
	textureName          = "base/data/particles/star1";
	spinSpeed		= 10.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "1.0 1.0 1.0 0.5";
	colors[1]     = "0.0 0.0 0.0 0.0";
	sizes[0]      = 4;
	sizes[1]      = 0;

	useInvAlpha = false;
};
datablock ParticleEmitterData(crystalStaveExplosionRingEmitter)
{
	lifeTimeMS = 50;

   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 5;
   velocityVariance = 0.0;
   ejectionOffset   = 3.0;
   thetaMin         = 0;
   thetaMax         = 180;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "crystalStaveExplosionRingParticle";

   uiName = "";
};

datablock ExplosionData(crystalStaveExplosion)
{
	explosionShape = "base/data/shapes/empty.dts";
	soundProfile = BFG10kSoundExplode;
	
	lifeTimeMS = 350;

	particleEmitter = crystalStaveExplosionEmitter;
	particleDensity = 7;
	particleRadius = 0.2;

	emitter[0] = crystalStaveExplosionRingEmitter;

	faceViewer     = true;
	explosionScale = ".33 .33 .33";

	shakeCamera = true;
	camShakeFreq = "6.6 7.3 6.6";
	camShakeAmp = "2.0 6.6 2.0";
	camShakeDuration = 0.33;
	camShakeRadius = 14.0;

	// Dynamic light
	lightStartRadius = 10;
	lightEndRadius = 25;
	lightStartColor = "0 1 1 1";
	lightEndColor = "0 0 0 1";

	damageRadius = 3;
	radiusDamage = 69;

	impulseRadius = 4;
	impulseForce = 500;
   
	uiName = "Stave Blast (Basic)";
};

datablock ProjectileData(crystalStaveProjectile)
{
   projectileShapeName = "./Shapes/Skull.dts";

   directDamage        = 2;
   directDamageType    = $DamageType::crystalStave;

   explosion             = crystalStaveExplosion;
   stickExplosion        = crystalStaveExplosion;
   bloodExplosion        = crystalStaveExplosion;
   particleEmitter       = arrowTrailEmitter;
   explodeOnPlayerImpact = false;
   explodeOnDeath        = true;  

   armingDelay         = 4000;
   lifetime            = 4000;
   fadeDelay           = 4000;

   isBallistic         = false;
   bounceAngle         = 170; //stick almost all the time
   minStickVelocity    = 10;
   bounceElasticity    = 0.2;
   bounceFriction      = 0.01;   
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   muzzleVelocity      = 40;
   velInheritFactor    = 1;

   isHoming = 1;
   homingTurn = 1/40;
};

datablock ItemData(crystalStaveItem : swordItem)
{
	shapeFile = "./Shapes/Crystal_Stave.dts";
	uiName = "Crystal Stave";
	doColorShift = true;
	colorShiftColor = "0.875 0.875 0.875 1.0";
	
	iconName = "base/client/ui/itemIcons/Wand";

	image = crystalStaveImage;
	canDrop = true;
	//iconName = "";
};

datablock ShapeBaseImageData(crystalStaveImage)
{
	shapeFile = "./Shapes/Crystal_Stave.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";

	correctMuzzleVector = false;

	className = "WeaponImage";

	item = crystalStaveItem;
	ammo = " ";
	projectile = crystalStaveProjectile;
	projectileType = Projectile;

	melee = false;
	doRetraction = false;

	armReady = true;

	doColorShift = true;
	colorShiftColor = crystalStaveItem.colorShiftColor;

	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSound[0]					= playerMountSound;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "CheckFire";
	stateTimeoutValue[2]            = 1.2;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;
	stateSound[2]					= printFireSound;

	stateName[3]					= "CheckFire";
	stateTransitionOnTriggerUp[3]	= "Ready";
	stateTransitionOnTriggerDown[3]	= "Fire";
};

function crystalStaveImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2,shiftTo);
	Parent::onFire(%this, %obj, %slot);
}