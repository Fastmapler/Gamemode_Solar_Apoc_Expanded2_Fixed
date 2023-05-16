AddDamageType("Sickle",   '<bitmap:Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Tools/Shapes/ci_Sickle> %1',    '%2 <bitmap:Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Modules/Tools/Shapes/ci_Sickle> %1',0.75,1);
datablock ProjectileData(SickleProjectile)
{
    shapeFile = "base/data/shapes/empty.dts";
    directDamage        = 9;
    directDamageType  = $DamageType::Sickle;
    radiusDamageType  = $DamageType::Sickle;

    brickExplosionRadius = 0;
    brickExplosionImpact = true;
    brickExplosionForce = 0;
    brickExplosionMaxVolume = 1;
    brickExplosionMaxVolumeFloating = 2;
    explosion           = KnifethrownExplosion;

    muzzleVelocity      = 80;
    velInheritFactor    = 1;

    armingDelay         = 0;
    lifetime            = 80;
    fadeDelay           = 70;
    bounceElasticity    = 0;
    bounceFriction      = 0;
    isBallistic         = true;
    gravityMod = 0.50;

    hasLight    = false;
    lightRadius = 3.0;
    lightColor  = "1 0 0";

    uiName = "Scythe Slice";
};

$EOTW::ItemCrafting["SickleItem"] = (128 TAB "Steel") TAB (64 TAB "Silver");
$EOTW::ItemDescription["SickleItem"] = "Used against plants to mass harvest them.";
datablock ItemData(SickleItem)
{
    category = "Weapon";  // Mission editor category
    className = "Weapon"; // For inventory system

    // Basic Item Properties
    shapeFile = "./Shapes/scythe.dts";
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    //gui stuff
    uiName = "Scythe";
    iconName = "./Icons/ScytheIcon";
	doColorShift = false;
	colorShiftColor = "0.40 0.40 0.40 1.00";

    // Dynamic properties defined by the scripts
    image = SickleImage;
    canDrop = true;
};

datablock ShapeBaseImageData(SickleImage)
{
    // Basic Item properties
    shapeFile = "./Shapes/scythe.dts";
    emap = true;

    // Specify mount point & offset for 3rd person, and eye offset
    // for first person rendering.
    mountPoint = 0;
    offset = "0 0 0";

    // When firing from a point offset from the eye, muzzle correction
    // will adjust the muzzle vector to point to the eye LOS point.
    // Since this weapon doesn't actually fire from the muzzle point,
    // we need to turn this off.  
    correctMuzzleVector = false;

    eyeOffset = "0 0 0";

    // Add the WeaponImage namespace as a parent, WeaponImage namespace
    // provides some hooks into the inventory system.
    className = "WeaponImage";

    // Projectile && Ammo.
    item = SickleItem;
    ammo = " ";
    projectile = SickleProjectile;
    projectileType = Projectile;

    //melee particles shoot from eye node for consistancy
    melee = true;
    doRetraction = false;
    //raise your arm up or not
    armReady = true;

    //casing = " ";
    doColorShift = true;
    colorShiftColor = "0.25 0.70 0.25 1.00";

    // Images have a state system which controls how the animations
    // are run, which sounds are played, script callbacks, etc. This
    // state system is downloaded to the client so that clients can
    // predict state changes and animate accordingly.  The following
    // system supports basic ready->fire->reload transitions as
    // well as a no-ammo->dryfire idle state.

    // Initial start up state
    stateName[0]                     = "Activate";
    stateTimeoutValue[0]             = 0.5;
    stateTransitionOnTimeout[0]      = "Ready";
    stateSound[0]                    = knifeDrawSound;

    stateName[1]                     = "Ready";
    stateTransitionOnTriggerDown[1]  = "PreFire";
    stateAllowImageChange[1]         = true;

    stateName[2]                    = "PreFire";
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
    stateWaitForTimeout[3]          = true;


    stateName[4]                    = "CheckFire";
    stateTransitionOnTriggerUp[4]   = "StopFire";
    stateTransitionOnTriggerDown[4] = "Fire";


    stateName[5]                    = "StopFire";
    stateTransitionOnTimeout[5]     = "Ready";
    stateTimeoutValue[5]            = 0.2;
    stateAllowImageChange[5]        = false;
    stateWaitForTimeout[5]          = true;
    stateSequence[5]                = "StopFire";
    stateScript[5]                  = "onStopFire";
};

function SickleImage::onPreFire(%this, %obj, %slot)
{
    %obj.playthread(2, armattack);
}

function SickleImage::onStopFire(%this, %obj, %slot)
{      
    %obj.playthread(2, root);
}

function SickleImage::onFire(%this, %obj, %slot)
{
    Parent::onFire(%this, %obj, %slot);
}

function SickleProjectile::onCollision(%data, %proj, %col, %fade, %pos, %norm)
{
    Parent::onCollision(%data, %proj, %col, %fade, %pos, %norm);

    if (!isObject(%client = %proj.client) || !isObject(%player = %client.player)) return;

    initContainerRadiusSearch(%pos, 1, $Typemasks::fxBrickAlwaysObjectType);

    while(isObject(%hit = containerSearchNext()))
    {
        if(%hit.getClassName() $= "fxDtsBrick" && %hit.refundbl_id $= "")
        {
            %data = %hit.getDataBlock();
            if (!%data.isPlantBrick || getTrustLevel(getBrickGroupFromObject(%client),%hit.getGroup()) < $TrustLevel::Hammer)
                continue;

            %hit.refundbl_id = %client.bl_id;
            ServerPlay3D(brickBreakSound,%hit.getPosition());
            %hit.fakeKillBrick(getRandom(-10,10) SPC getRandom(-10,10) SPC getRandom(0,10),3);
            %hit.scheduleNoQuota(500,delete);
        }
    }
}