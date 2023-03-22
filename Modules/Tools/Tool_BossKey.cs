$EOTW::ItemCrafting["BossKeyItem"] = (150 TAB "Boss Essence") TAB (128 TAB "Naturum") TAB (128 TAB "Steel");
$EOTW::ItemDescription["BossKeyItem"] = "A reusable key to activate the Boss Altar. Also an uncommon drop by enemies.";
datablock ItemData(BossKeyItem)
{
    // Basic Item Properties
    shapeFile = "./Shapes/skele_key.dts";
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    //gui properties
    uiName = "Boss Key";
    iconName = "./Icons/icon_skeleton";
    doColorShift = true;
    colorShiftColor = "1.0 0.0 0.0 0.75";

    // Dynamic properties defined by the scripts
    image = BossKeyImage;
    canDrop = true;
};

datablock ShapeBaseImageData(BossKeyImage)
{
    shapeFile = "./Shapes/skele_key.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";

    correctMuzzleVector = false;
    className = "WeaponImage";

    item = BossKeyItem;
    ammo = " ";
    projectile = "";
    projectileType = "";

    melee = true;
    doRetraction = false;
    armReady = true;

    showBricks = false;

    doColorShift = BossKeyItem.doColorShift;
    colorShiftColor = BossKeyItem.colorShiftColor;

    stateName[0]                     = "Activate";
    stateTimeoutValue[0]             = 0.0;
    stateTransitionOnTimeout[0]      = "Ready";

    stateName[1]                     = "Ready";
    stateTransitionOnTriggerDown[1]  = "PreFire";
    stateAllowImageChange[1]         = true;

    stateName[2]                    = "PreFire";
    stateScript[2]                  = "onPreFire";
    stateAllowImageChange[2]        = true;
    stateTimeoutValue[2]            = 0.01;
    stateTransitionOnTimeout[2]     = "Fire";

    stateName[3]                    = "Fire";
    stateTransitionOnTimeout[3]     = "CheckFire";
    stateTimeoutValue[3]            = 0.15;
    stateFire[3]                    = true;
    stateAllowImageChange[3]        = true;
    stateSequence[3]                = "Fire";
    stateScript[3]                  = "onFire";
    stateWaitForTimeout[3]		     = true;
    stateSequence[3]                = "Fire";

    stateName[4]                    = "CheckFire";
    stateTransitionOnTriggerUp[4]   = "StopFire";

    stateName[5]                    = "StopFire";
    stateTransitionOnTimeout[5]     = "Ready";
    stateTimeoutValue[5]            = 0.01;
    stateAllowImageChange[5]        = true;
    stateWaitForTimeout[5]          = true;
    stateSequence[5]                = "StopFire";
    stateScript[5]                  = "onStopFire";
};

function BossKeyImage::onPreFire(%this, %obj, %slot)
{
	%obj.playthread(2, shiftLeft);
}

function BossKeyImage::onStopFire(%this, %obj, %slot)
{	
	%obj.playthread(2, root);
}

function BossKeyImage::onFire(%this, %player, %slot)
{
    if (!isObject(%client = %player.client))
        return;

    %start = %player.getEyePoint();
    %vec = vectorScale(%player.getMuzzleVector(%slot), 10  * getWord(%player.getScale(), 2) );
    %end = vectorAdd(%start, %vec);
    %mask = $TypeMasks::FxBrickObjectType;

    %rayCast = containerRayCast(%start,%end,%mask);

    %hitObj = getWord(%rayCast, 0);
    %hitPos = getWords(%rayCast, 1, 3);
    %hitNormal = getWords(%rayCast, 4, 6);

    if(!isObject(%hitObj))
        return;

    %data = %hitObj.getDatablock();
    
    if (%data.BossSpawnData !$= "")
    {
        if (isObject(%hitObj.bossSpawn) && %hitObj.bossSpawn.getState() !$= "DEAD")
        {
            %client.chatMessage("The boss is already lurking!");
            return;
        }

        if (getSimTime() - %hitObj.lastBossSpawn < 60000)
        {
            %client.chatMessage("The last boss summon was too recent! Wait a minute between summons.");
            return;
        }

        %client.chatMessage("The boss is coming! Stand back!");
        %hitObj.lastBossSpawn = getSimTime();
        %hitObj.bossSpawn = spawnNewFauna(vectorAdd(%hitObj.getPosition(), "0 0 15"), %data.BossSpawnData);
    }
}

datablock fxDTSBrickData(EOTWBossAltar)
{
	brickFile = "./Shapes/Altar.blb";
    category = "Solar Apoc";
	subCategory = "Special";
    uiName = "Boss Altar";
	BossSpawnData = HeirophantHoleBot;
};
$EOTW::CustomBrickCost["EOTWBossAltar"] = 0.75 TAB "75502eff" TAB 25600 TAB "Wood" TAB 1280 TAB "Flesh";
$EOTW::BrickDescription["EOTWBossAltar"] = "Allows the summoning of the Heirophant, a zealous obelisk of fire.";