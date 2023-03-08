$EOTW::ItemCrafting["potionHealingItem"] = (64 TAB "Healium") TAB (32 TAB "Glass");
$EOTW::ItemDescription["potionHealingItem"] = "Upon consumption heals 150 HP over 5 seconds.";
datablock ItemData(potionHealingItem)
{
    category = "Weapon";
    className = "Weapon";

    shapeFile = "./Shapes/Potion.dts";
    rotate = false;
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    uiName = "Flask (Healing)";
    //iconName = "./potion";
    doColorShift = true;
    colorShiftColor = "0.85 0.40 0.40 1.00";

    image = potionHealingImage;
    canDrop = true;
};

datablock ShapeBaseImageData(potionHealingImage)
{
    shapeFile = "./Shapes/Potion.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";
    eyeOffset = 0;
    rotation = eulerToMatrix( "0 0 0" );

    className = "WeaponImage";
    item = potionHealingItem;

    armReady = true;

    doColorShift = potionHealingItem.doColorShift;
    colorShiftColor = potionHealingItem.colorShiftColor;

    stateName[0]                     = "Activate";
    stateTimeoutValue[0]             = 0.15;
    stateTransitionOnTimeout[0]      = "Ready";

    stateName[1]                     = "Ready";
    stateTransitionOnTriggerDown[1]  = "Fire";
    stateAllowImageChange[1]         = true;

    stateName[2]                     = "Fire";
    stateTransitionOnTimeout[2]      = "Ready";
    stateAllowImageChange[2]         = true;
    stateScript[2]                   = "onFire";
    stateTimeoutValue[2]		     = 1.0;

    potionType = "Healing";
    potionTime = 5000;
};

function potionHealingImage::onFire(%this,%obj,%slot) { %obj.applyPotionEffect(%this.potionType, %this.potionTime); }