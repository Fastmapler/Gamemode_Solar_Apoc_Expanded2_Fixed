AddDamageType("greenGLauncher",   '<bitmap:add-ons/Weapon_OldSchool/icons/ci_grenade> %1',    '%2 <bitmap:add-ons/Weapon_OldSchool/icons/ci_grenade> %1',0.75,1);
AddDamageType("greenGLauncherRadius",   '<bitmap:add-ons/Weapon_OldSchool/icons/ci_grenadeRadius> %1',    '%2 <bitmap:add-ons/Weapon_OldSchool/icons/ci_grenadeRadius> %1',1,0);
datablock ProjectileData(greenGLauncherProjectile)
{
   projectileShapeName = "add-ons/Vehicle_Tank/Tankbullet.dts";
   directDamage        = 20;
   directDamageType    = $DamageType::greenGLauncher;
   radiusDamageType    = $DamageType::greenGLauncherRadius;

   brickExplosionRadius = 5;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 50;
   brickExplosionMaxVolume = 60;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 120;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 1250;
   verticalImpulse     = 750;
   explosion           = tankShellExplosion;
   particleEmitter     = rocketTrailEmitter;
   explodeOnDeath        = true;

   sound = rocketLoopSound;

   muzzleVelocity      = 75;
   velInheritFactor    = 0.5;

   armingDelay         = 00;
   lifetime            = 20000;
   fadeDelay           = 19500;
   bounceElasticity    = 0.05;
   bounceFriction      = 0.05;
   isBallistic         = true;
   gravityMod = 1;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";
   
   uiName = "Launched Grenade";
};

//////////
// item //
//////////
datablock ItemData(greenGLauncherItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Shapes/Grenade_Launcher.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Grenade L.";
	iconName = "./Icons/icon_GLauncher";
	doColorShift = true;
	colorShiftColor = "0.1 0.5 0.25 1.000";

	 // Dynamic properties defined by the scripts
	image = greenGLauncherImage;
	canDrop = true;
	
	maxAmmo = 1;
	canReload = 0;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(greenGLauncherImage)
{
   // Basic Item properties
   shapeFile = "./Shapes/Grenade_Launcher.dts";
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
   item = greenGLauncherItem;
   ammo = " ";
   projectile = greenGLauncherProjectile;
   projectileType = Projectile;

   casing = GunShellDebris;
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
   colorShiftColor = greenGLauncherItem.colorShiftColor;

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

	stateName[2]                    = "Fire";
	stateTransitionOnTriggerUp[2]   = "LoadCheckA";
	stateTimeoutValue[2]            = 0.28;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]		  = true;
	stateEmitter[2]			  = gunSmokeEmitter;
	stateEmitterTime[2]		  = 0.05;
	stateEmitterNode[2]		  = "muzzleNode";
	stateSound[2]			  = rocketFireSound;

	stateName[3]				= "LoadCheckA";
	stateScript[3]				= "onLoadCheck";
	stateTimeoutValue[3]			= 0.01;
	stateTransitionOnTimeout[3]		= "LoadCheckB";
	
	stateName[4]				= "LoadCheckB";
	stateTransitionOnAmmo[4]		= "Ready";
	stateTransitionOnNoAmmo[4]		= "Reload";

	stateName[5]				= "Reload";
	stateTimeoutValue[5]			= 0.7;
	stateScript[5]				= "onReloadStart";
	stateTransitionOnTimeout[5]		= "Reloaded";
	stateEmitter[5]			  = gunSmokeEmitter;
	stateEmitterTime[5]		  = 0.7;
	stateEmitterNode[5]		  = "muzzleNode";
	stateWaitForTimeout[5]			= true;
	
	stateName[6]				= "FireLoadCheckA";
	stateScript[6]				= "onLoadCheck";
	stateTimeoutValue[6]			= 0.01;
	stateTransitionOnTimeout[6]		= "FireLoadCheckB";
	
	stateName[9]				= "FireLoadCheckB";
	stateTransitionOnAmmo[9]		= "Ready";
	stateTransitionOnNoAmmo[9]		= "ReloadSmoke";
	
	stateName[10] 				= "ReloadSmoke";
	stateEmitter[10]			= gunSmokeEmitter;
	stateEmitterTime[10]			= 0.35;
	stateEmitterNode[10]			= "muzzleNode";
	stateTimeoutValue[10]			= 0.14;
	stateTransitionOnTimeout[10]		= "Reload";
	
	stateName[11]				= "Reloaded";
	stateTimeoutValue[11]			= 0.14;
	stateScript[11]				= "onReloaded";
	stateTransitionOnTimeout[11]		= "Ready";
};

function greenGLauncherImage::onFire(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool]--;

	%obj.playThread(2, shiftaway);
	%projectile = %this.projectile;
	%shellcount = 1;

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
			scale = "0.5 0.5 0.5";
			client = %obj.client;
		};
		MissionCleanup.add(%p);
	}
}

function greenGLauncherImage::onReloadStart(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = 0;
}

function greenGLauncherImage::onReloaded(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
	%obj.setImageAmmo(%slot,1);
}