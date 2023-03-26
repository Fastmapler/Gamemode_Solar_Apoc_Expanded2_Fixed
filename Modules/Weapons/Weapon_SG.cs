//audio
datablock AudioProfile(gunReload2Sound)
{
   filename    = "./Sounds/gunReload2.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(shotgunFire1Sound)
{
   filename    = "./Sounds/shotgun_01.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(shotgunFire2Sound)
{
   filename    = "./Sounds/shotgun_02.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(shotgunFire3Sound)
{
   filename    = "./Sounds/shotgun_03.wav";
   description = AudioClose3d;
   preload = true;
};

//shell
datablock DebrisData(ShotgunShellDebris)
{
	shapeFile = "./Shapes/shotgun_shell.dts";
	lifetime = 2.0;
	minSpinSpeed = -400.0;
	maxSpinSpeed = 200.0;
	elasticity = 0.5;
	friction = 0.2;
	numBounces = 3;
	staticOnMaxBounce = true;
	snapOnMaxBounce = false;
	fade = true;

	gravModifier = 4;
};

AddDamageType("basicShotgun",   '<bitmap:add-ons/Weapon_OldSchool/icons/ci_Shotgun> %1',    '%2 <bitmap:add-ons/Weapon_OldSchool/icons/ci_Shotgun> %1',0.75,1);
datablock ProjectileData(basicShotgunProjectile)
{
   projectileShapeName = "add-ons/Weapon_Gun/bullet.dts";
   directDamage        = 18;
   directDamageType    = $DamageType::basicShotgun;
   radiusDamageType    = $DamageType::basicShotgun;

   brickExplosionRadius = 0.2;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 15;
   brickExplosionMaxVolume = 20;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 30;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 50;
   verticalImpulse     = 400;
   explosion           = machineGunExplosion;
   particleEmitter     = ""; //bulletTrailEmitter;

   muzzleVelocity      = 100;
   velInheritFactor    = 0.5;

   armingDelay         = 0;
   lifetime            = 4000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = true;
   gravityMod = 0.999;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";
   
   uiName = "Shotgun Bullet ";
};

datablock ProjectileData(pointblankProjectile : basicShotgunProjectile)
{
   projectileShapeName = "add-ons/Vehicle_Tank/Tankbullet.dts";
   directDamage        = 48;
   directDamageType    = $DamageType::basicShotgun;
   radiusDamageType    = $DamageType::basicShotgun;

   brickExplosionRadius = 0.4;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 30;
   brickExplosionMaxVolume = 25;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 35;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 350;
   verticalImpulse     = 50;

   muzzleVelocity      = 100;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 75;
   fadeDelay           = 75;
   isBallistic         = false;
   gravityMod = 0.0;
};


//////////
// item //
//////////

$EOTW::ItemCrafting["basicShotgunItem"] = (256 TAB "Iron") TAB (256 TAB "Copper") TAB (128 TAB "Lead");
$EOTW::ItemDescription["basicShotgunItem"] = "A simple design of a shotgun. Fires multiple Shotgun Pellets per shot. 4 Rounds/Shot, ~30° Spread.";
datablock ItemData(basicShotgunItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Shapes/Shotgun.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Shotgun I";
	iconName = "./Icons/icon_Shotgun";
	doColorShift = true;
	colorShiftColor = "0.400 0.400 0.400 1.000";

	 // Dynamic properties defined by the scripts
	image = basicShotgunImage;
	canDrop = true;
	
	maxAmmo = 1;
	canReload = 0;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(basicShotgunImage)
{
   // Basic Item properties
   shapeFile = "./Shapes/Shotgun.dts";
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
   item = basicShotgunItem;
   ammo = " ";
   projectile = basicShotgunProjectile;
   projectileType = Projectile;
   ammoType = "Shotgun Pellet";

   casing = ShotgunShellDebris;
   shellExitDir        = "1.0 0.1 1.0";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 10.0;	
   shellVelocity       = 5.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;
   minShotTime = 1000;

   doColorShift = true;
   colorShiftColor = basicShotgunItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.15;
	stateSequence[0]		= "Activate";
	stateTransitionOnTimeout[0]     = "LoadCheckA";
	stateSound[0]			= weaponSwitchSound;

	stateName[1]                    = "Ready";
	stateTransitionOnNoAmmo[1]      = "Reload";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]        = true;
	stateSequence[1]	        = "ready";

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]   = "Smoke";
	stateTimeoutValue[2]            = 0.28;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]		  = true;
	stateEmitter[2]			  = gunFlashEmitter;
	stateEmitterTime[2]		  = 0.05;
	stateEmitterNode[2]		  = "muzzleNode";

	stateName[3] 			  = "Smoke";
	stateEmitter[3]			  = gunSmokeEmitter;
	stateEmitterTime[3]		  = 0.42;
	stateEmitterNode[3]		  = "muzzleNode";
	stateTimeoutValue[3]            = 0.07;
	stateTransitionOnTimeout[3]     = "LoadCheckA";

	stateName[4]				= "LoadCheckA";
	stateScript[4]				= "onLoadCheck";
	stateTimeoutValue[4]			= 0.01;
	stateTransitionOnTimeout[4]		= "LoadCheckB";
	
	stateName[5]				= "LoadCheckB";
	stateTransitionOnAmmo[5]		= "Ready";
	stateTransitionOnNoAmmo[5]		= "Reload";

	stateName[6]				= "Reload";
	stateTimeoutValue[6]			= 0.42;
	stateScript[6]				= "onReloadStart";
	stateTransitionOnTimeout[6]		= "Reloaded";
	stateEjectShell[6]                      = true;
	stateWaitForTimeout[6]			= true;
	stateSequence[6]	                = "reload";
	stateSound[6]				= gunReload2Sound;
	
	stateName[7]				= "FireLoadCheckA";
	stateScript[7]				= "onLoadCheck";
	stateTimeoutValue[7]			= 0.01;
	stateTransitionOnTimeout[7]		= "FireLoadCheckB";
	
	stateName[8]				= "FireLoadCheckB";
	stateTransitionOnAmmo[8]		= "Ready";
	stateTransitionOnNoAmmo[8]		= "ReloadSmoke";
	
	stateName[9] 				= "ReloadSmoke";
	stateEmitter[9]			        = gunSmokeEmitter;
	stateEmitterTime[9]			= 0.42;
	stateEmitterNode[9]			= "muzzleNode";
	stateTimeoutValue[9]			= 0.14;
	stateTransitionOnTimeout[9]		= "Reload";
	
	stateName[10]				= "Reloaded";
	stateTimeoutValue[10]			= 0.07;
	stateScript[10]				= "onReloaded";
	stateTransitionOnTimeout[10]		= "Ready";
};


function basicShotgunImage::onFire(%this,%obj,%slot)
{
	ShotgunImageFire(%this,%obj,%slot,0.0030,4);
}

function basicShotgunImage::onReloadStart(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = 0;
}

function basicShotgunImage::onReloaded(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
	%obj.setImageAmmo(%slot,1);
}

function ShotgunImageFire(%this,%obj,%slot,%spread,%shellcount)
{
	%ammoType = "Shotgun Pellet";
	%shellcount = getMin($EOTW::Material[%obj.client.bl_id, %ammoType], %shellcount);
	if (%shellcount < 1)
	{
		%obj.unMountImage(0);
		%obj.client.chatMessage("Not enough ammo!");
		return;
	}
	if (!%obj.hasEffect("Ranging") || getRandom() > 0.6)
		$EOTW::Material[%obj.client.bl_id, %ammoType] -= %shellcount;
	%obj.client.PrintEOTWInfo();

	%obj.playAudio(2, "shotgunFire" @ getRandom(1, 3) @ "Sound");
	%fvec = %obj.getForwardVector();
	%fX = getWord(%fvec,0);
	%fY = getWord(%fvec,1);

	%evec = %obj.getEyeVector();
	%eX = getWord(%evec,0);
	%eY = getWord(%evec,1);
	%eZ = getWord(%evec,2);

	%eXY = mSqrt(%eX*%eX+%eY*%eY);

	%aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;

	%obj.setVelocity(VectorAdd(%obj.getVelocity(),VectorScale(%aimVec,-2 * %shellcount)));
	%obj.playThread(2, shiftaway);
	%obj.toolAmmo[%obj.currTool]--;

	%projectile = %this.projectile;

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
	}

	for(%shell=0; %shell<mCeil(%shellcount/2); %shell++)
	{
		%projectile = "pointblankProjectile";

		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1,%vector2);

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
		return %p;
	}
}

//T2
$EOTW::ItemCrafting["improvedShotgunItem"] = (256 TAB "Steel") TAB (256 TAB "Red Gold") TAB (256 TAB "Lead");
$EOTW::ItemDescription["improvedShotgunItem"] = "An improved shotgun with better reliability. Fires multiple Shotgun Pellets per shot. 5 Rounds/Shot, ~20° Spread.";

datablock ItemData(improvedShotgunItem : basicShotgunItem)
{
	uiName = "Shotgun II";
	colorShiftColor = "0.400 0.400 0.800 1.000";
	image = improvedShotgunImage;
};

datablock ShapeBaseImageData(improvedShotgunImage : basicShotgunImage)
{
   item = improvedShotgunItem;
   colorShiftColor = improvedShotgunItem.colorShiftColor;
};


function improvedShotgunImage::onFire(%this,%obj,%slot)
{
	ShotgunImageFire(%this,%obj,%slot,0.0020,5);
}

function improvedShotgunImage::onReloadStart(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = 0;
}

function improvedShotgunImage::onReloaded(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
	%obj.setImageAmmo(%slot,1);
}

//T3
$EOTW::ItemCrafting["superiorShotgunItem"] = (256 TAB "Adamantine") TAB (256 TAB "Naturum") TAB (512 TAB "Lead");
$EOTW::ItemDescription["superiorShotgunItem"] = "A powerful and professionally made shotgun. Fires multiple Shotgun Pellets per shot. 7 Rounds/Shot, ~13° Spread.";
datablock ItemData(superiorShotgunItem : basicShotgunItem)
{
	uiName = "Shotgun III";
	colorShiftColor = "0.400 0.800 0.400 1.000";
	image = superiorShotgunImage;
};

datablock ShapeBaseImageData(superiorShotgunImage : basicShotgunImage)
{
   item = superiorShotgunItem;
   colorShiftColor = superiorShotgunItem.colorShiftColor;
};


function superiorShotgunImage::onFire(%this,%obj,%slot)
{
	ShotgunImageFire(%this,%obj,%slot,0.0013,7);
}

function superiorShotgunImage::onReloadStart(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = 0;
}

function superiorShotgunImage::onReloaded(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
	%obj.setImageAmmo(%slot,1);
}