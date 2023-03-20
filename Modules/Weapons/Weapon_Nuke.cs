$EOTW::ItemCrafting["EOTW_nukeCannonItem"] = (256 TAB "PlaSteel") TAB (256 TAB "Epoxy") TAB (256 TAB "Energium") TAB (256 TAB "Naturum");
$EOTW::ItemDescription["EOTW_nukeCannonItem"] = "Take cover! Requires Nuke ammo!";
datablock ItemData(EOTW_nukeCannonItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Shapes/nuke_cannon.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Nuke Cannon";
	iconName = "./Icons/icon_Nuke";
	doColorShift = false;
	colorShiftColor = "0.400 0.400 0.400 1.000";

	 // Dynamic properties defined by the scripts
	image = EOTW_nukeCannonImage;
	canDrop = true;
	
	maxAmmo = 1;
	canReload = 0;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(EOTW_nukeCannonImage)
{
   // Basic Item properties
   shapeFile = "./Shapes/nuke_cannon.dts";
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
   item = EOTW_nukeCannonItem;
   ammo = " ";
   projectile = gLauncherProjectile;
   projectileType = Projectile;
   ammoType = "Nuke";

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
   colorShiftColor = EOTW_nukeCannonItem.colorShiftColor;

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
	stateTransitionOnTimeout[2]   = "LoadCheckA";
	stateTimeoutValue[2]            = 0.28;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	stateWaitForTimeout[2]		  = true;
	stateEmitter[2]			  = gunSmokeEmitter;
	stateEmitterTime[2]		  = 0.05;
	stateEmitterNode[2]		  = "muzzleNode";

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

function EOTW_nukeCannonImage::onFire(%this,%obj,%slot)
{
	EOTW_nukeCannonFire(%this,%obj,%slot,1);
}

function EOTW_nukeCannonImage::onReloadStart(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = 0;
}

function EOTW_nukeCannonImage::onReloaded(%this,%obj,%slot)
{
	%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
	%obj.setImageAmmo(%slot,1);
}

function EOTW_nukeCannonFire(%this,%obj,%slot,%shellcount)
{
	%ammoType = "Nuke";
	%shellcount = getMin($EOTW::Material[%obj.client.bl_id, %ammoType], %shellcount);
	if (%shellcount < 1)
	{
		%obj.unMountImage(0);
		%obj.client.chatMessage("Not enough ammo!");
		return;
	}
	if (!%obj.hasEffect("Speed") || getRandom() > 0.6)
		$EOTW::Material[%obj.client.bl_id, %ammoType] -= %shellcount;
	%obj.client.PrintEOTWInfo();
		
	%obj.stopAudio(2);
	%obj.playAudio(2, "gLauncherFire" @ getRandom(1, 2) @ "Sound");
	%obj.toolAmmo[%obj.currTool]--;
	%obj.playThread(2, shiftaway);
	%projectile = %this.projectile;
	%spread = 0.0;
	%velmod = getRandom(075, 125) / 100;

	for(%shell=0; %shell<%shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity * %velmod);
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