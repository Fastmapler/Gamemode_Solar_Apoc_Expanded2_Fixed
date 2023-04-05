datablock ExplosionData(EOTWPickaxeExplosion)
{
   lifeTimeMS = 400;

   soundProfile = swordHitSound;

   particleEmitter = swordExplosionEmitter;
   particleDensity = 10;
   particleRadius = 0.2;

   faceViewer     = true;
   explosionScale = "1 1 1";

   shakeCamera = true;
   camShakeFreq = "10.0 11.0 10.0";
   camShakeAmp = "0.5 0.5 0.5";
   camShakeDuration = 0.25;
   camShakeRadius = 5.0;

   lightStartRadius = 1.5;
   lightEndRadius = 0;
   lightStartColor = "00.0 0.2 0.6";
   lightEndColor = "0 0 0";
};

//Tier 1
$EOTW::ItemCrafting["EOTWPickaxeItem"] = (1024 TAB "Wood") TAB (128 TAB "Steel");
$EOTW::ItemDescription["EOTWPickaxeItem"] = "Hit a gatherable object for 125% base gather speed.";

datablock ProjectileData(EOTWPickaxeProjectile)
{
   directDamage        = 10;
   directDamageType  = $DamageType::EOTWPickaxe;
   radiusDamageType  = $DamageType::EOTWPickaxe;
   explosion           = EOTWPickaxeExplosion;

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

datablock ItemData(EOTWPickaxeItem : swordItem)
{
	shapeFile = "./Shapes/Pickaxe.dts";
	uiName = "Pickaxe I";
	doColorShift = true;
	colorShiftColor = "0.400 0.400 0.400 1.000";

	image = EOTWPickaxeImage;
	canDrop = true;
	iconName = "./Icons/icon_EOTWPickaxe";
};

AddDamageType("EOTWPickaxe", '<bitmap:Add-ons/Gamemode_Solar_Apoc_Expanded2/Modules/Tools/Icons/CI_EOTWPickaxe> %1', '%2 <bitmap:Add-ons/Gamemode_Solar_Apoc_Expanded2/Modules/Tools/Icons/CI_EOTWPickaxe> %1',0.75,1);

datablock ShapeBaseImageData(EOTWPickaxeImage)
{
   shapeFile = "./Shapes/Pickaxe.dts";
   emap = true;

   mountPoint = 0;
   offset = "0 0 0";

   correctMuzzleVector = false;

   className = "WeaponImage";

   item = EOTWPickaxeItem;
   ammo = " ";
   projectile = EOTWPickaxeProjectile;
   projectileType = Projectile;


   melee = true;
   doRetraction = false;

   armReady = true;


   doColorShift = EOTWPickaxeItem.doColorShift;
   colorShiftColor = EOTWPickaxeItem.colorShiftColor;

	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.5;
	stateTransitionOnTimeout[0]      = "Ready";
	stateSound[0]                    = swordDrawSound;

	stateName[1]                     = "Ready";
	stateTransitionOnTriggerDown[1]  = "PreFire";
	stateAllowImageChange[1]         = true;

	stateName[2]			        = "PreFire";
	stateScript[2]                  = "onPreFire";
	stateAllowImageChange[2]        = false;
	stateTimeoutValue[2]            = 0.1;
	stateTransitionOnTimeout[2]     = "Fire";

	stateName[3]                    = "Fire";
	stateTransitionOnTimeout[3]     = "CheckFire";
	stateTimeoutValue[3]            = 0.2;
	stateFire[3]                    = true;
	stateAllowImageChange[3]        = false;
	stateSequence[3]                = "Fire";
	stateScript[3]                  = "onFire";
	stateWaitForTimeout[3]	    	= true;

	stateName[4]		        	= "CheckFire";
	stateTransitionOnTriggerUp[4]	= "StopFire";
	stateTransitionOnTriggerDown[4]	= "Fire";

	
	stateName[5]                    = "StopFire";
	stateTransitionOnTimeout[5]     = "Ready";
	stateTimeoutValue[5]            = 0.2;
	stateAllowImageChange[5]        = false;
	stateWaitForTimeout[5]	    	= true;
	stateSequence[5]                = "StopFire";
	stateScript[5]                  = "onStopFire";


};

function EOTWPickaxeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function EOTWPickaxeImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function EOTWPickaxeProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	if(%col.getClassName() $= "fxDTSBrick")
        %obj.sourceObject.attemptGather(%col, 1.25);
}


//Tier 2
datablock ProjectileData(EOTWPickaxe2Projectile : EOTWPickaxeProjectile)
{
   directDamage        = 12;
   muzzleVelocity      = 60;
};

$EOTW::ItemCrafting["EOTWPickaxe2Item"] = (2048 TAB "Wood") TAB (256 TAB "Steel") TAB (128 TAB "Diamond");
$EOTW::ItemDescription["EOTWPickaxe2Item"] = "Hit a gatherable object for 175% base gather speed.";
datablock ItemData(EOTWPickaxe2Item : EOTWPickaxeItem)
{
	uiName = "Pickaxe II";
	colorShiftColor = "0.400 0.400 0.800 1.000";
	image = EOTWPickaxe2Image;
};

datablock ShapeBaseImageData(EOTWPickaxe2Image : EOTWPickaxeImage)
{
   item = EOTWPickaxe2Item;
   projectile = EOTWPickaxe2Projectile;
   colorShiftColor = EOTWPickaxe2Item.colorShiftColor;
};

function EOTWPickaxe2Image::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function EOTWPickaxe2Image::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function EOTWPickaxe2Projectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	if(%col.getClassName() $= "fxDTSBrick")
        %obj.sourceObject.attemptGather(%col, 1.75);
}

//Tier3
datablock ProjectileData(EOTWPickaxe3Projectile : EOTWPickaxeProjectile)
{
   directDamage        = 14;
   muzzleVelocity      = 70;
};

$EOTW::ItemCrafting["EOTWPickaxe3Item"] = (4096 TAB "Wood") TAB (512 TAB "Steel") TAB (128 TAB "Adamantine");
$EOTW::ItemDescription["EOTWPickaxe3Item"] = "Hit a gatherable object for 250% base gather speed.";
datablock ItemData(EOTWPickaxe3Item : EOTWPickaxeItem)
{
	uiName = "Pickaxe III";
	colorShiftColor = "0.400 0.800 0.400 1.000";
	image = EOTWPickaxe3Image;
};

datablock ShapeBaseImageData(EOTWPickaxe3Image : EOTWPickaxeImage)
{
   item = EOTWPickaxe3Item;
   projectile = EOTWPickaxe3Projectile;
   colorShiftColor = EOTWPickaxe3Item.colorShiftColor;
};

function EOTWPickaxe3Image::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function EOTWPickaxe3Image::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function EOTWPickaxe3Projectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	if(%col.getClassName() $= "fxDTSBrick")
        %obj.sourceObject.attemptGather(%col, 2.5);
}

//Tier0
datablock ProjectileData(EOTWPickaxe0Projectile : EOTWPickaxeProjectile)
{
   directDamage        = 8;
   muzzleVelocity      = 70;
};

$EOTW::ItemCrafting["EOTWPickaxe0Item"] = (256 TAB "Wood") TAB (128 TAB "Granite");
$EOTW::ItemDescription["EOTWPickaxe0Item"] = "Hit a gatherable object for 100% base gather speed.";
datablock ItemData(EOTWPickaxe0Item : EOTWPickaxeItem)
{
	uiName = "Pickaxe 0";
	colorShiftColor = "0.757 0.659 0.447 1.000";
	image = EOTWPickaxe0Image;
};

datablock ShapeBaseImageData(EOTWPickaxe0Image : EOTWPickaxeImage)
{
   item = EOTWPickaxe0Item;
   projectile = EOTWPickaxe0Projectile;
   colorShiftColor = EOTWPickaxe0Item.colorShiftColor;
};

function EOTWPickaxe0Image::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, armattack);
}

function EOTWPickaxe0Image::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function EOTWPickaxe0Projectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
	parent::onCollision(%this,%obj,%col,%fade,%pos,%normal);
	if(%col.getClassName() $= "fxDTSBrick")
        %obj.sourceObject.attemptGather(%col, 1.0);
}