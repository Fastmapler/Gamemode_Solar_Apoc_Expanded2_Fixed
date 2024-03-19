$EOTW::ItemCrafting["EOTWSoftHammerItem"] = (1024 TAB "Wood") TAB (128 TAB "Rubber");
$EOTW::ItemDescription["EOTWSoftHammerItem"] = "Hit a machine to toggle the machine off or on. Same effect as the event toggle.";

datablock AudioProfile(SoftHammerSound)
{
   filename    = "./Sounds/SoftHammer.wav";
   description = AudioClosest3d;
   preload = true;
};

datablock ExplosionData(EOTWSoftHammerExplosion : HammerExplosion)
{
   soundProfile = SoftHammerSound;
};

datablock ProjectileData(EOTWSoftHammerProjectile)
{
   directDamage         = 5;
   directDamageType     = $DamageType::Hammer;
   radiusDamageType     = $DamageType::Hammer;
   explosion            = EOTWSoftHammerExplosion;

   muzzleVelocity      = 50;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 100;
   fadeDelay           = 70;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod = 0.0;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";

   uiName = "";
};

datablock ItemData(EOTWSoftHammerItem : swordItem)
{
	shapeFile = "./Shapes/SoftHammer.dts";
	uiName = "Soft Hammer";
	doColorShift = true;
	colorShiftColor = "0.400 0.400 0.400 1.000";

	image = EOTWSoftHammerImage;
	canDrop = true;
	iconName = "./Icons/icon_SoftHammer";
};

datablock ShapeBaseImageData(EOTWSoftHammerImage)
{
   shapeFile = "./Shapes/SoftHammer.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = EOTWSoftHammerItem;
   ammo = " ";
   projectile = EOTWSoftHammerProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = EOTWSoftHammerItem.doColorShift;
   colorShiftColor = EOTWSoftHammerItem.colorShiftColor;

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.1;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = weaponSwitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "Fire";
	stateAllowImageChange[1]         = true;

	stateName[2]                     = "Fire";
	stateTransitionOnTimeout[2]      = "CheckFire";
	stateTimeoutValue[2]             = 0.1;
	stateFire[2]                     = true;
	stateAllowImageChange[2]         = false;
	stateSequence[2]                 = "Fire";
	stateScript[2]                   = "onFire";
	stateWaitForTimeout[2]           = true;

	stateName[3]                     = "CheckFire";
	stateTransitionOnTriggerUp[3]    = "Ready";


};

function EOTWSoftHammerImage::onFire(%this, %obj, %slot)
{
   %obj.playthread(2, shiftDown);
   Parent::onFire(%this, %obj, %slot);
}

function EOTWSoftHammerProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	if(%col.getClassName() $= "fxDTSBrick" && %col.getDatablock().isPowered)
   {
      if (getTrustLevel(%obj.sourceObject, %col) < $TrustLevel::Hammer)
		{
			if (%col.stackBL_ID $= "" || %col.stackBL_ID != %client.getBLID())
			{
				%client.chatMessage("The owner of that object does not trust you enough.");
				return;
			}
		}

      %col.SetMachinePowered(0);
      %output = %col.machineDisabled ? "\c1DISABLED" : "\c2ENABLED";
      %obj.sourceObject.client.chatMessage("\c6The " @ %col.getDatablock().uiName @ " is now " @ %output);
   }
}