//Yes, it may technically deal damage, but its primary function is to gib enemies for gibs.

datablock AudioProfile(SurvivalKnifeBrickSound)
{
    filename    = "./Sounds/KnifeBrick.wav";
    description = AudioClosest3d;
    preload = true;
};
datablock AudioProfile(SurvivalKnifePlayerSound)
{
    filename    = "./Sounds/KnifePlayer.wav";
    description = AudioClosest3d;
    preload = true;
};
datablock AudioProfile(knifeDrawSound)
{
    filename    = "./Sounds/knifeDraw.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock ParticleData(KnifethrownExplosionParticle)
{
    dragCoefficient      = 5;
    gravityCoefficient   = 0.1;
    inheritedVelFactor   = 0.2;
    constantAcceleration = 0.0;
    lifetimeMS           = 500;
    lifetimeVarianceMS   = 300;
    textureName          = "base/data/particles/chunk";
    spinSpeed               = 10.0;
    spinRandomMin           = -50.0;
    spinRandomMax           = 50.0;
    colors[0]     = "0.9 0.9 0.6 0.9";
    colors[1]     = "0.9 0.5 0.6 0.0";
    sizes[0]      = 0.25;
    sizes[1]      = 0.0;
};

datablock ParticleEmitterData(KnifethrownExplosionEmitter)
{
    ejectionPeriodMS = 1;
    periodVarianceMS = 0;
    ejectionVelocity = 5;
    velocityVariance = 0.0;
    ejectionOffset   = 0.0;
    thetaMin         = 80;
    thetaMax         = 80;
    phiReferenceVel  = 0;
    phiVariance      = 360;
    overrideAdvance = false;
    particles = "KnifethrownExplosionParticle";

    uiName = "Survival Knife Hit";
};

datablock ExplosionData(KnifethrownExplosion)
{
    //explosionShape = "";
    lifeTimeMS = 500;

    soundProfile = SurvivalKnifeBrickSound;

    particleEmitter = KnifethrownExplosionEmitter;
    particleDensity = 10;
    particleRadius = 0.2;

    faceViewer     = true;
    explosionScale = "0.5 1 1";

    shakeCamera = true;
    camShakeFreq = "20.0 22.0 20.0";
    camShakeAmp = "1.0 1.0 1.0";
    camShakeDuration = 0.5;
    camShakeRadius = 10.0;

    // Dynamic light
    lightStartRadius = 2;
    lightEndRadius = 0;
    lightStartColor = "1 0 0";
    lightEndColor = "0 0 0";
};

//Tier 1
AddDamageType("Knifestab",   '<bitmap:Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Tools/Shapes/ci_knifestab> %1',    '%2 <bitmap:Add-Ons/Gamemode_Solar_Apoc_Expanded2/Modules/Tools/Shapes/ci_knifestab> %1',0.75,1);
datablock ProjectileData(SurvivalKnifeStabProjectile)
{
    shapeFile = "base/data/shapes/empty.dts";
    directDamage        = 11;
    directDamageType  = $DamageType::knifestab;
    radiusDamageType  = $DamageType::knifestab;

    brickExplosionRadius = 0;
    brickExplosionImpact = true;
    brickExplosionForce = 0;
    brickExplosionMaxVolume = 1;
    brickExplosionMaxVolumeFloating = 2;
    explosion           = KnifethrownExplosion;

    muzzleVelocity      = 60;
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

    uiName = "Survival Knife Stab";
};

$EOTW::ItemCrafting["SurvivalKnifeItem"] = (128 TAB "Steel") TAB (128 TAB "Granite");
$EOTW::ItemDescription["SurvivalKnifeItem"] = "Allows valid corpses to be gutted for flesh.";
datablock ItemData(SurvivalKnifeItem)
{
    category = "Weapon";  // Mission editor category
    className = "Weapon"; // For inventory system

    // Basic Item Properties
    shapeFile = "./Shapes/SurvivalKnife.dts";
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    //gui stuff
    uiName = "Fliet Knife";
    iconName = "./Shapes/icon_SurvivalKnife";
    doColorShift = true;
    colorShiftColor = "0.400 0.400 0.400 1.000";

    // Dynamic properties defined by the scripts
    image = SurvivalKnifeImage;
    canDrop = true;
};

datablock ShapeBaseImageData(SurvivalKnifeImage)
{
    // Basic Item properties
    shapeFile = "./Shapes/SurvivalKnife.dts";
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
    item = SurvivalKnifeItem;
    ammo = " ";
    projectile = SurvivalKnifeStabProjectile;
    projectileType = Projectile;

    //melee particles shoot from eye node for consistancy
    melee = true;
    doRetraction = false;
    //raise your arm up or not
    armReady = true;

    //casing = " ";
    doColorShift = true;
    colorShiftColor = "0.400 0.400 0.400 1.000";

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

function SurvivalKnifeImage::onPreFire(%this, %obj, %slot) { %obj.playthread(2, armattack); }
function SurvivalKnifeImage::onStopFire(%this, %obj, %slot) { %obj.playthread(2, root); }

function SurvivalKnifeStabProjectile::onCollision(%data, %proj, %col, %fade, %pos, %norm)
{
    Parent::onCollision(%data, %proj, %col, %fade, %pos, %norm);
    SurvivalKnifeStabProcess(%data, %proj, %col, %fade, %pos, %norm);
}

//Tier 2
datablock ProjectileData(SurvivalKnifeStabProjectile2 : SurvivalKnifeStabProjectile)
{
    directDamage        = 13;
    muzzleVelocity      = 70;
    gibBoost            = 2;
};

$EOTW::ItemCrafting["SurvivalKnife2Item"] = (128 TAB "Plasteel") TAB (256 TAB "Granite");
$EOTW::ItemDescription["SurvivalKnife2Item"] = "An upgraded filet knife with 3x gibbing speed.";
datablock ItemData(SurvivalKnife2Item : SurvivalKnifeItem)
{
    uiName = "Fliet Knife II";
    doColorShift = true;
    colorShiftColor = "0.400 0.400 0.800 1.000";
    image = SurvivalKnife2Image;
};

datablock ShapeBaseImageData(SurvivalKnife2Image : SurvivalKnifeImage)
{
    item = SurvivalKnife2Item;
    projectile = SurvivalKnifeStabProjectile2;
    colorShiftColor = "0.400 0.400 0.800 1.000";
};

function SurvivalKnife2Image::onPreFire(%this, %obj, %slot) { %obj.playthread(2, armattack); }
function SurvivalKnife2Image::onStopFire(%this, %obj, %slot) { %obj.playthread(2, root); }

function SurvivalKnifeStabProjectile2::onCollision(%data, %proj, %col, %fade, %pos, %norm)
{
    Parent::onCollision(%data, %proj, %col, %fade, %pos, %norm);
    SurvivalKnifeStabProcess(%data, %proj, %col, %fade, %pos, %norm);
}

//Functions

function SurvivalKnifeStabProcess(%data, %proj, %col, %fade, %pos, %norm)
{
    if (!isObject(%client = %proj.client) || !isObject(%player = %client.player)) return;

    //Vanilla pumpkin carving mechanic that I never knew existed for 10 years WTF??

    if(%col.getDataBlock().getName() $= "brickPumpkinBaseData" && getTrustLevel(%col, %player) >= $TrustLevel::Build)
        carvePumpkin(%col);

    //Attempt to gib monsters
    initContainerRadiusSearch(%pos, 0.3, $TypeMasks::CorpseObjectType);

    while(isObject(%hit = containerSearchNext()))
    {
        if(%hit.getClassName() $= "AIPlayer")
        {
            if (%hit.isGibbable && %hit.getState() $= "DEAD")
            {
                if(%hit.beingCollected > 0 && %hit.beingCollected != %client.bl_id)
                    %client.centerPrint("<color:FFFFFF>Someone is already gibbing this monster!", 3);
                else
                {
                    %hit.lastFaunaGibTick = getSimTime();
                    %hit.beingCollected = %client.bl_id;
                    %hit.cancelFaunaCollecting = %hit.schedule(10000, "cancelFaunaGib");
                    %player.GibFaunaLoop(%hit, %data.gibBoost + 0);
                    
                    break;
                }
            }
        }
    }
}

function Player::GibFaunaLoop(%obj, %target, %boost)
{
	if(!isObject(%client = %obj.client) || %obj.getState() $= "DEAD" || !isObject(%target)) return;

    cancel(%target.cancelFaunaCollecting);
    cancel(%target.RemoveBodySchedule);
    cancel(%obj.faunaCollectLoop);

    %eye = %obj.getEyePoint();
    %dir = %obj.getEyeVector();
    %for = %obj.getForwardVector();
    %face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
    %mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType | $TypeMasks::PlayerObjectType;
    %ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
    initContainerRadiusSearch(getWords(%ray, 1, 3), 0.3, $TypeMasks::CorpseObjectType);
    while(isObject(%hit = containerSearchNext()))
    {
        if(%hit == %target && %hit.isGibbable)
        {
            %totalTime = (%target.getDatablock().maxDamage * 100);
            if (%target.gatherProcess >= %totalTime)
            {
                $EOTW::Material[%client.bl_id, "Flesh"] += mCeil(%target.getDatablock().maxDamage / 4);
                %client.centerPrint("<br><color:FFFFFF>Gibbed the " @ %target.getDataBlock().hName @ ".<br>100% complete.<br>You now have " @ $EOTW::Material[%client.bl_id, "Flesh"] @ " Flesh.", 3);
                %target.removeBody(true);
            }
            else
            {
                %client.centerPrint("<br><color:FFFFFF>Gibbing the " @ %target.getDataBlock().hName @ ".<br>" @ mFloor((%target.gatherProcess / %totalTime) * 100) @ "% complete.", 3);

                %target.gatherProcess += (getSimTime() - %target.lastFaunaGibTick) * (1 + %boost);
                %hit.lastFaunaGibTick = getSimTime();
                %target.RemoveBodySchedule = %target.schedule(1000 * 60, "RemoveBody", true);
                %target.cancelFaunaCollecting = %target.schedule(10000, "cancelFaunaGib");
                %obj.faunaCollectLoop = %obj.schedule(16, "GibFaunaLoop", %target, %boost);
            }

            break;
        }
    }
}

function AIPlayer::cancelFaunaGib(%obj) { %obj.beingCollected = 0; }