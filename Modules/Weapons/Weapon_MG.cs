//audio
datablock AudioProfile(machineGunFire1Sound)
{
   filename    = "./Sounds/smg_01.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(machineGunFire2Sound)
{
   filename    = "./Sounds/smg_02.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(machineGunFire3Sound)
{
   filename    = "./Sounds/smg_03.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(machineGunFire4Sound)
{
   filename    = "./Sounds/smg_04.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(machineGunHitSound)
{
   filename    = "./Sounds/explosion_miniscule.wav";
   description = AudioClose3d;
   preload = true;
};

datablock ExplosionData(machineGunExplosion : gunExplosion)
{
	soundProfile = machineGunHitSound;
};

AddDamageType("machineGun",   '<bitmap:add-ons/Weapon_OldSchool/icons/ci_MG> %1',    '%2 <bitmap:add-ons/Weapon_OldSchool/icons/ci_MG> %1',0.2,1);
datablock ProjectileData(machineGunProjectile)
{
   projectileShapeName = "add-ons/Weapon_Gun/bullet.dts";
   directDamage        = 24;
   directDamageType    = $DamageType::machineGun;
   radiusDamageType    = $DamageType::machineGun;

   brickExplosionRadius = 0;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 10;
   brickExplosionMaxVolume = 1;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 2;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 400;
   verticalImpulse		= 50;
   explosion           = machineGunExplosion;
   particleEmitter     = "";

   muzzleVelocity      = 125;
   velInheritFactor    = 1;

   armingDelay         = 00;
   lifetime            = 4000;
   fadeDelay           = 3500;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = true;
   gravityMod = 0.999;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "Machine Gun Bullet";
};

//////////
// item //
//////////

$EOTW::ItemCrafting["basicMachineGunItem"] = (256 TAB "Iron") TAB (256 TAB "Silver") TAB (128 TAB "Lead");
$EOTW::ItemDescription["basicMachineGunItem"] = "A rather crude but useful assault rifle. Fires Rifle Rounds. ~4 Rounds/Sec, ~15° Spread.";
datablock ItemData(basicMachineGunItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Shapes/Machine_Gun.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Machine Gun I";
	iconName = "./Icons/icon_MG";
	doColorShift = true;
	colorShiftColor = "0.400 0.400 0.400 1.000";

	 // Dynamic properties defined by the scripts
	image = basicMachineGunImage;
	canDrop = true;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(basicMachineGunImage)
{
   // Basic Item properties
   shapeFile = "./Shapes/Machine_Gun.dts";
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
   item = basicMachineGunItem;
   ammo = " ";
   projectile = machineGunProjectile;
   projectileType = Projectile;
   ammoType = "Rifle Round";

	casing = gunShellDebris;
	shellExitDir        = "1.0 -1.3 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 15.0;	
	shellVelocity       = 7.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = true;
   colorShiftColor = basicMachineGunItem.colorShiftColor;//"0.400 0.196 0 1.000";

   //casing = " ";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.15;
	stateTransitionOnTimeout[0]       = "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;
	stateSequence[1]	= "Ready";

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Smoke";
	stateTimeoutValue[2]            = 0.14;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateSequence[2]                = "Fire";
	stateEjectShell[2]              = true;
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]			= true;
	stateEmitter[2]					= gunFlashEmitter;
	stateEmitterTime[2]				= 0.05;
	stateEmitterNode[2]				= "muzzleNode";

	stateName[3] = "Smoke";
	stateEmitter[3]					= gunSmokeEmitter;
	stateEmitterTime[3]				= 0.12;
	stateEmitterNode[3]				= "muzzleNode";
	stateTimeoutValue[3]            = 0.10;
	stateTransitionOnTimeout[3]     = "Ready";

};

function basicMachineGunImage::onFire(%this,%obj,%slot) { machineGunImageFire(%this,%obj,%slot,0.0015); }	

function machineGunImageFire(%this,%obj,%slot,%spread)
{
	%shellcount = 1;

	%ammoType = "Rifle Round";
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

	%obj.stopAudio(2);
	%obj.playAudio(2, "machineGunFire" @ getRandom(1, 4) @ "Sound");
	%obj.playThread(2, plant);
	%projectile = machineGunProjectile;

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

//T2
$EOTW::ItemCrafting["improvedMachineGunItem"] = (256 TAB "Steel") TAB (256 TAB "Electrum") TAB (256 TAB "Lead");
$EOTW::ItemDescription["improvedMachineGunItem"] = "A well built assault rifle. Fires Rifle Rounds. ~6.6 Rounds/Sec, ~10° Spread.";
datablock ItemData(improvedMachineGunItem : basicMachineGunItem)
{
	uiName = "Machine Gun II";
	colorShiftColor = "0.400 0.400 0.800 1.000";
	image = improvedMachineGunImage;
};

datablock ShapeBaseImageData(improvedMachineGunImage : basicMachineGunImage)
{
	item = improvedMachineGunItem;
	colorShiftColor = improvedMachineGunItem.colorShiftColor;

	stateTimeoutValue[2]            = 0.08;
	stateTimeoutValue[3]            = 0.07;

};

function improvedMachineGunImage::onFire(%this,%obj,%slot) { machineGunImageFire(%this,%obj,%slot,0.001); }

//T3
$EOTW::ItemCrafting["superiorMachineGunItem"] = (256 TAB "Adamantine") TAB (256 TAB "Energium") TAB (512 TAB "Lead");
$EOTW::ItemDescription["superiorMachineGunItem"] = "An exceptional assault rifle. Fires Rifle Rounds. ~11.1 Rounds/Sec, ~6.6° Spread.";
datablock ItemData(superiorMachineGunItem : basicMachineGunItem)
{
	uiName = "Machine Gun III";
	colorShiftColor = "0.400 0.800 0.400 1.000";
	image = superiorMachineGunImage;
};

datablock ShapeBaseImageData(superiorMachineGunImage : basicMachineGunImage)
{
	item = superiorMachineGunItem;
	colorShiftColor = superiorMachineGunItem.colorShiftColor;

	stateTimeoutValue[2]            = 0.05;
	stateTimeoutValue[3]            = 0.04;

};

function superiorMachineGunImage::onFire(%this,%obj,%slot) { machineGunImageFire(%this,%obj,%slot,0.00066); }	