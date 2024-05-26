function GameConnection::hasImplant(%client, %implant)
{
    return hasField(%client.implantList, %implant);
}

function Player::hasImplant(%player, %implant)
{
    return isObject(%player.client) && %player.client.hasImplant(%implant);
}

function GameConnection::grantImplant(%client, %implant, %keepItem)
{
    if (hasField(%client.implantList, %implant))
    {
        %client.centerPrint("You already have this implant applied!", 2);
        return false;
    }

    %client.implantList = trim(%client.implantList TAB %implant);

    if (%keepItem || !isObject(%player = %client.player))
        return;

    EOTW_applyBooze(%player, 18);
    
    %player.setWhiteOut(0.6);
    %currSlot = %player.currTool;
	%player.tool[%currSlot] = 0;
	%player.weaponCount--;
	messageClient(%client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%client);

    return true;
}


//Mending
$EOTW::ItemCrafting["implantMendingItem"] = (256 TAB "Healium") TAB (1 TAB "Implanting Polymer");
$EOTW::ItemDescription["implantMendingItem"] = "Grants greater natural regeneration to health and drunkness. Lost on death.";
datablock ItemData(implantMendingItem)
{
    category = "Weapon";
    className = "Weapon";

    shapeFile = "./Shapes/Syringe.dts";
    rotate = false;
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    uiName = "Implant (Mending)";
    iconName = "./Icons/Syringe";
    doColorShift = true;
    colorShiftColor = "0.85 0.40 0.40 1.00";

    image = implantMendingImage;
    canDrop = true;
};

datablock ShapeBaseImageData(implantMendingImage)
{
    shapeFile = "./Shapes/Syringe.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";
    eyeOffset = 0;
    rotation = eulerToMatrix( "0 0 0" );

    className = "WeaponImage";
    item = implantMendingItem;

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

    implantType = "Mending";
};

function implantMendingImage::onFire(%this,%obj,%slot) { %obj.client.grantImplant(%this.implantType, false); }

//Adrenline
$EOTW::ItemCrafting["implantAdrenlineItem"] = (256 TAB "Adrenlium") TAB (1 TAB "Implanting Polymer");
$EOTW::ItemDescription["implantAdrenlineItem"] = "Increases base movement speed and stamina regeneration. Lost on death.";
datablock ItemData(implantAdrenlineItem : implantMendingItem)
{
    uiName = "Implant (Adrenline)";
    colorShiftColor = "0.85 0.85 0.40 1.00";
    image = implantAdrenlineImage;
};
datablock ShapeBaseImageData(implantAdrenlineImage : implantMendingImage)
{
    item = implantAdrenlineItem;
    colorShiftColor = implantAdrenlineItem.colorShiftColor;
    implantType = "Adrenline";
};
function implantAdrenlineImage::onFire(%this,%obj,%slot) { %obj.client.grantImplant(%this.implantType, false); }

//Smelting
$EOTW::ItemCrafting["implantSmeltingItem"] = (256 TAB "Gatherium") TAB (1 TAB "Implanting Polymer");
$EOTW::ItemDescription["implantSmeltingItem"] = "Grants a small yield boost when gathering smeltable ores. Lost on death.";
datablock ItemData(implantSmeltingItem : implantMendingItem)
{
    uiName = "Implant (Smelting)";
    colorShiftColor = "0.40 0.85 0.40 1.00";
    image = implantSmeltingImage;
};
datablock ShapeBaseImageData(implantSmeltingImage : implantMendingImage)
{
    item = implantSmeltingItem;
    colorShiftColor = implantSmeltingItem.colorShiftColor;
    implantType = "Smelting";
};
function implantSmeltingImage::onFire(%this,%obj,%slot) { %obj.client.grantImplant(%this.implantType, false); }

//Leatherskin
$EOTW::ItemCrafting["implantLeatherskinItem"] = (128 TAB "Healium") TAB (128 TAB "Lead") TAB (1 TAB "Implanting Polymer");
$EOTW::ItemDescription["implantLeatherskinItem"] = "Grants damage reduction against the sun. Lost on death.";
datablock ItemData(implantLeatherskinItem : implantMendingItem)
{
    uiName = "Implant (Leatherskin)";
    colorShiftColor = "0.85 0.85 0.40 1.00";
    image = implanLeatherskinImage;
};
datablock ShapeBaseImageData(implanLeatherskinImage : implantMendingImage)
{
    item = implantLeatherskinItem;
    colorShiftColor = implantLeatherskinItem.colorShiftColor;
    implantType = "Leatherskin";
};
function implanLeatherskinImage::onFire(%this,%obj,%slot) { %obj.client.grantImplant(%this.implantType, false); }