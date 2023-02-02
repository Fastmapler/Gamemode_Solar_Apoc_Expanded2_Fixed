datablock ExplosionData(EOTWDynamiteExplosion : rocketExplosion)
{
	soundProfile = rocketExplodeSound;

	lifeTimeMS = 350;

	particleEmitter = rocketExplosionEmitter;
	particleDensity = 10;
	particleRadius = 0.2;

	emitter[0] = rocketExplosionRingEmitter;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "3.0 10.0 3.0";
	camShakeDuration = 0.5;
	camShakeRadius = 20.0;

	// Dynamic light
	lightStartRadius = 10;
	lightEndRadius = 25;
	lightStartColor = "1 1 1 1";
	lightEndColor = "0 0 0 1";

	damageRadius = 3;
	radiusDamage = 100;

	impulseRadius = 6;
	impulseForce = 4000;
};

AddDamageType("Dynamite",   '<bitmap:base/client/ui/CI/bomb> %1',    '%2 <bitmap:base/client/ui/CI/bomb> %1',1,0);
datablock ProjectileData(EOTWDynamiteProjectile)
{
	projectileShapeName = "./Shapes/Dynamite.dts";
	directDamage        = 0;
	directDamageType  = $DamageType::Dynamite;
	radiusDamageType  = $DamageType::Dynamite;
	impactImpulse	   = 0;
	verticalImpulse	   = 0;
	explosion           = EOTWDynamiteExplosion;
	//particleEmitter     = fragGrenadeTrailEmitter;

	brickExplosionRadius = 10;
	brickExplosionImpact = false; //destroy a brick if we hit it directly?
	brickExplosionForce  = 0;             
	brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
	brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground (should always be >= brickExplosionMaxVolume)

	muzzleVelocity      = 30;
	velInheritFactor    = 1;
	explodeOnDeath		= true;

	armingDelay         = 4000; //4 second fuse 
	lifetime            = 4000;
	fadeDelay           = 3500;
	bounceAngle         = 170; //stick almost all the time
	minStickVelocity    = 5;
	bounceElasticity    = 0.2;
	bounceFriction      = 0.01;   
	isBallistic         = true;
	gravityMod = 1.0;

	hasLight    = false;
	lightRadius = 3.0;
	lightColor  = "0 0 0.5";
};

$EOTW::ItemCrafting["EOTWDynamiteItem"] = (64 TAB "Wood") TAB (64 TAB "Paraffin") TAB (256 TAB "Explosives");
$EOTW::ItemDescription["EOTWDynamiteItem"] = "A one use throwable that instantly breaks gatherable bricks on detonation.";
datablock ItemData(EOTWDynamiteItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Shapes/Dynamite.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Dynamite";
	iconName = "./Icons/Icon_dynamite";
	doColorShift = false;
	colorShiftColor = "0.000 0.000 0.000 1.000";

	 // Dynamic properties defined by the scripts
	image = EOTWDynamiteImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(EOTWDynamiteImage)
{
   // Basic Item properties
   shapeFile = "./Shapes/Dynamite.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   rotation = eulerToMatrix( "90 0 0" );
   //eyeOffset = "0.1 0.2 -0.55";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = EOTWDynamiteItem;
   ammo = " ";
   projectile = EOTWDynamiteProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   //casing = " ";
   doColorShift = false;
   colorShiftColor = "0.400 0.196 0 1.000";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.1;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSequence[0]				= "ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Charge";
	stateAllowImageChange[1]		= true;
	
	stateName[2]                    = "Charge";
	stateTransitionOnTimeout[2]		= "Armed";
	stateTimeoutValue[2]            = 0.7;
	stateWaitForTimeout[2]			= false;
	stateTransitionOnTriggerUp[2]	= "AbortCharge";
	stateScript[2]                  = "onCharge";
	stateAllowImageChange[2]        = false;
	
	stateName[3]					= "AbortCharge";
	stateTransitionOnTimeout[3]		= "Ready";
	stateTimeoutValue[3]			= 0.3;
	stateWaitForTimeout[3]			= true;
	stateScript[3]					= "onAbortCharge";
	stateAllowImageChange[3]		= false;

	stateName[4]					= "Armed";
	stateTransitionOnTriggerUp[4]	= "Fire";
	stateAllowImageChange[4]		= false;

	stateName[5]					= "Fire";
	stateTransitionOnTimeout[5]		= "Ready";
	stateTimeoutValue[5]			= 0.5;
	stateFire[5]					= true;
	stateSequence[5]				= "fire";
	stateScript[5]					= "onFire";
	stateWaitForTimeout[5]			= true;
	stateAllowImageChange[5]		= false;
};

function EOTWDynamiteImage::onCharge(%this, %obj, %slot)
{
	%obj.playthread(2, spearReady);
}

function EOTWDynamiteImage::onAbortCharge(%this, %obj, %slot)
{
	%obj.playthread(2, root);
}

function EOTWDynamiteImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	Parent::onFire(%this, %obj, %slot);
	
	%currSlot = %obj.currTool;
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);
}

function EOTWDynamiteProjectile::onExplode(%this, %obj, %pos)
{		
    initContainerRadiusSearch(%pos, %this.explosion.impulseRadius, $Typemasks::fxBrickAlwaysObjectType);

    while(isObject(%hit = containerSearchNext()))
    {
        if(%hit.getClassName() $= "fxDtsBrick" && %hit.isCollectable && %hit.material !$= "" && isObject(%matter = getMatterType(%hit.material)) && (%hit.beingCollected <= 0 || %hit.beingCollected == %hit.sourceClient.bl_id))
        {
            EOTW_SpawnOreDrop(%matter.spawnValue, %matter.name, vectorAdd(%hit.getPosition(), "0 0 1"));
            %hit.beingCollected = 1337; //So people can't dump multiple bombs at the same time to get multiple drops
            %hit.killBrick();
        }
    }

	return Parent::onExplode(%this, %obj, %pos);
}