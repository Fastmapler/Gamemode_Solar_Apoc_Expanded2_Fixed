function Player::applyPotionEffect(%obj, %type, %ticks, %keepItem)
{
    %obj.appliedEffect[%type] += %ticks * $EOTW::PlayerLoopRate;
    if (!hasField(%obj.effectList, %type))
        %obj.effectList = trim(%obj.effectList TAB %type);

    if (%removeItem)
        return;
    
    %obj.setWhiteOut(0.6);
    %currSlot = %obj.currTool;
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);
}

function Player::hasEffect(%obj, %type)
{
    return %obj.appliedEffect[%type] > 0;
}

$EOTW::ItemCrafting["potionHealingItem"] = (64 TAB "Healium") TAB (32 TAB "Quartz");
$EOTW::ItemDescription["potionHealingItem"] = "Grants a temporary boost to health regeneration.";
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
    potionTime = 15;
};

function potionHealingImage::onFire(%this,%obj,%slot) { %obj.applyPotionEffect(%this.potionType, %this.potionTime); }

$EOTW::ItemCrafting["potionGatheringItem"] = (64 TAB "Gatherium") TAB (32 TAB "Quartz");
$EOTW::ItemDescription["potionGatheringItem"] = "Grants a temporary boost to gather speed.";
datablock ItemData(potionGatheringItem : potionHealingItem)
{
    uiName = "Flask (Gathering)";
    colorShiftColor = "0.40 0.85 0.40 1.00";
    image = potionGatheringImage;
};

datablock ShapeBaseImageData(potionGatheringImage : potionHealingImage)
{
    item = potionGatheringItem;
    colorShiftColor = potionGatheringItem.colorShiftColor;

    potionType = "Gathering";
    potionTime = 300;
};

function potionGatheringImage::onFire(%this,%obj,%slot) { %obj.applyPotionEffect(%this.potionType, %this.potionTime); }

$EOTW::ItemCrafting["potionSpeedItem"] = (64 TAB "Adrenlium") TAB (32 TAB "Quartz");
$EOTW::ItemDescription["potionSpeedItem"] = "Grants a temporary boost to movement speed.";
datablock ItemData(potionSpeedItem : potionHealingItem)
{
    uiName = "Flask (Speed)";
    colorShiftColor = "0.85 0.85 0.40 1.00";
    image = potionSpeedImage;
};

datablock ShapeBaseImageData(potionSpeedImage : potionHealingImage)
{
    item = potionSpeedItem;
    colorShiftColor = potionSpeedItem.colorShiftColor;

    potionType = "Speed";
    potionTime = 8;
};

function potionSpeedImage::onFire(%this,%obj,%slot) { %obj.applyPotionEffect(%this.potionType, %this.potionTime); }

$EOTW::ItemCrafting["potionRangedItem"] = (64 TAB "Rangium") TAB (32 TAB "Quartz");
$EOTW::ItemDescription["potionRangedItem"] = "Grants a temporary boost to ammo conversion.";
datablock ItemData(potionRangedItem : potionHealingItem)
{
    uiName = "Flask (Ranging)";
    colorShiftColor = "0.85 0.85 0.40 1.00";
    image = potionRangedImage;
};

datablock ShapeBaseImageData(potionRangedImage : potionHealingImage)
{
    item = potionRangedItem;
    colorShiftColor = potionRangedItem.colorShiftColor;

    potionType = "Ranging";
    potionTime = 120;
};

function potionRangedImage::onFire(%this,%obj,%slot) { %obj.applyPotionEffect(%this.potionType, %this.potionTime); }