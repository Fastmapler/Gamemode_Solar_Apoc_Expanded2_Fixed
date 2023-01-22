//Healing
$EOTW::ItemCrafting["mixFlaskHealingItem"] = (32 TAB "Healing Mix") TAB (16 TAB "Glass");
$EOTW::ItemDescription["mixFlaskHealingItem"] = "Upon consumption heals 2 hp/s over 2 minutes.";
datablock ItemData(mixFlaskHealingItem)
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

    uiName = "MIX - Flask Healing";
    //iconName = "./potion";
    doColorShift = true;
    colorShiftColor = "0.85 0.40 0.40 1.00";

    image = mixFlaskHealingImage;
    canDrop = true;
};

datablock ShapeBaseImageData(mixFlaskHealingImage)
{
    shapeFile = "./Shapes/Potion.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";
    eyeOffset = 0;
    rotation = eulerToMatrix( "0 0 0" );

    className = "WeaponImage";
    item = mixFlaskHealingItem;

    armReady = true;

    doColorShift = mixFlaskHealingItem.doColorShift;
    colorShiftColor = mixFlaskHealingItem.colorShiftColor;

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
};

function mixFlaskHealingImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskHealing();
	%obj.setWhiteOut(0.7);
	
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	
	if(isObject(%obj.client))
	{
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
	else
		%obj.unMountImage(%slot);
}

function Player::PotionTick_FlaskHealing(%obj, %tick)
{
    if (%tick >= 120)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Healing dose wears off.");
        return;
    }

    %obj.addHealth(2);
    %obj.PotionSchedule["PotionTick_FlaskHealing"] = %obj.schedule(1000, "PotionTick_FlaskHealing", %tick + 1);
}

//Steroids
$EOTW::ItemCrafting["mixFlaskSteroidItem"] = (32 TAB "Steroid Mix") TAB (16 TAB "Glass");
$EOTW::ItemDescription["mixFlaskSteroidItem"] = "Upon consumption increases melee damge by 100% for 1 minute.";
datablock ItemData(mixFlaskSteroidItem : mixFlaskHealingItem)
{
    uiName = "MIX - Flask Steroids";
    colorShiftColor = "0.25 0.00 0.00 1.00";
    image = mixFlaskSteroidImage;
};

datablock ShapeBaseImageData(mixFlaskSteroidImage : mixFlaskHealingImage)
{
    item = mixFlaskSteroidItem;
    doColorShift = mixFlaskSteroidItem.doColorShift;
    colorShiftColor = mixFlaskSteroidItem.colorShiftColor;
};

function mixFlaskSteroidImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskSteroid();
	%obj.setWhiteOut(0.7);
	
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	
	if(isObject(%obj.client))
	{
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
	else
		%obj.unMountImage(%slot);
}

function Player::PotionTick_FlaskSteroid(%obj, %tick)
{
    if (%tick >= 60)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Steroids dose wears off.");
        %obj.steroidlevel--;
        return;
    }
    else if (%tick == 0)
    {
        %obj.steroidlevel++;
    }

    %obj.PotionSchedule["PotionTick_FlaskSteroid"] = %obj.schedule(1000, "PotionTick_FlaskSteroid", %tick + 1);
}

//Adrenline
$EOTW::ItemCrafting["mixFlaskAdrenlineItem"] = (32 TAB "Adrenline Mix") TAB (16 TAB "Glass");
$EOTW::ItemDescription["mixFlaskAdrenlineItem"] = "Upon consumption increases max movement speed by 100% for 30 seconds.";
datablock ItemData(mixFlaskAdrenlineItem : mixFlaskHealingItem)
{
    uiName = "MIX - Flask Adrenline";
    colorShiftColor = "0.85 0.85 0.40 1.00";
    image = mixFlaskAdrenlineImage;
};

datablock ShapeBaseImageData(mixFlaskAdrenlineImage : mixFlaskHealingImage)
{
    item = mixFlaskAdrenlineItem;
    doColorShift = mixFlaskAdrenlineItem.doColorShift;
    colorShiftColor = mixFlaskAdrenlineItem.colorShiftColor;
};

function mixFlaskAdrenlineImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskAdrenline();
	%obj.setWhiteOut(0.7);
	
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	
	if(isObject(%obj.client))
	{
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
	else
		%obj.unMountImage(%slot);
}

function Player::PotionTick_FlaskAdrenline(%obj, %tick)
{
    if (%tick >= 30)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Adrenline dose wears off.");
        %obj.ChangeSpeedMulti(-1);
        return;
    }
    else if (%tick == 0)
    {
        %obj.ChangeSpeedMulti(1);
    }

    %obj.PotionSchedule["PotionTick_FlaskAdrenline"] = %obj.schedule(1000, "PotionTick_FlaskAdrenline", %tick + 1);
}

//Gatherer
$EOTW::ItemCrafting["mixFlaskGathererItem"] = (32 TAB "Gatherer Mix") TAB (16 TAB "Glass");
$EOTW::ItemDescription["mixFlaskGathererItem"] = "Upon consumption increases gathering speed by 50% for 2 minutes.";
datablock ItemData(mixFlaskGathererItem : mixFlaskHealingItem)
{
    uiName = "MIX - Flask Gatherer";
    colorShiftColor = "0.40 0.40 0.85 1.00";
    image = mixFlaskGathererImage;
};

datablock ShapeBaseImageData(mixFlaskGathererImage : mixFlaskHealingImage)
{
    item = mixFlaskGathererItem;
    doColorShift = mixFlaskGathererItem.doColorShift;
    colorShiftColor = mixFlaskGathererItem.colorShiftColor;
};

function mixFlaskGathererImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskGatherer();
	%obj.setWhiteOut(0.7);
	
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	
	if(isObject(%obj.client))
	{
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
	else
		%obj.unMountImage(%slot);
}

function Player::PotionTick_FlaskGatherer(%obj, %tick)
{
    if (%tick >= 120)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Gatherer dose wears off.");
        %obj.Gathererlevel -= 0.5;
        return;
    }
    else if (%tick == 0)
    {
        %obj.Gathererlevel += 0.5;
    }

    %obj.PotionSchedule["PotionTick_FlaskGatherer"] = %obj.schedule(1000, "PotionTick_FlaskGatherer", %tick + 1);
}

//Overload
$EOTW::ItemCrafting["mixFlaskOverloadItem"] = (32 TAB "Overload Mix") TAB (16 TAB "Glass");
$EOTW::ItemDescription["mixFlaskOverloadItem"] = "Upon consumption gives chance to subatract firearm ammo used, but inficts 2hp/s of self-harm all over 30 seconds.";
datablock ItemData(mixFlaskOverloadItem : mixFlaskHealingItem)
{
    uiName = "MIX - Flask Overload";
    colorShiftColor = "0.25 0.25 0.25 1.00";
    image = mixFlaskOverloadImage;
};

datablock ShapeBaseImageData(mixFlaskOverloadImage : mixFlaskHealingImage)
{
    item = mixFlaskOverloadItem;
    doColorShift = mixFlaskOverloadItem.doColorShift;
    colorShiftColor = mixFlaskOverloadItem.colorShiftColor;
};

function mixFlaskOverloadImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskOverload();
	%obj.setWhiteOut(0.7);
	
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	
	if(isObject(%obj.client))
	{
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
	else
		%obj.unMountImage(%slot);
}

function Player::PotionTick_FlaskOverload(%obj, %tick)
{
    if (%tick >= 30)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Overload dose wears off.");
        %obj.steroidlevel -= 2;
        %obj.ammoReturnLevel--;
        return;
    }
    else if (%tick == 0)
    {
        %obj.steroidlevel += 2;
        %obj.ammoReturnLevel++;
    }
    %obj.addHealth(-1);

    %obj.PotionSchedule["PotionTick_FlaskOverload"] = %obj.schedule(1000, "PotionTick_FlaskOverload", %tick + 1);
}

//Leatherskin
$EOTW::ItemCrafting["mixFlaskLeatherskinItem"] = (32 TAB "Leatherskin Mix") TAB (16 TAB "Glass");
$EOTW::ItemDescription["mixFlaskLeatherskinItem"] = "Upon consumption reduces sun damage by 1.5 points for 1 minute.";
datablock ItemData(mixFlaskLeatherskinItem : mixFlaskHealingItem)
{
    uiName = "MIX - Flask Leatherskin";
    colorShiftColor = "1.00 0.50 0.00 1.00";
    image = mixFlaskLeatherskinImage;
};

datablock ShapeBaseImageData(mixFlaskLeatherskinImage : mixFlaskHealingImage)
{
    item = mixFlaskLeatherskinItem;
    doColorShift = mixFlaskLeatherskinItem.doColorShift;
    colorShiftColor = mixFlaskLeatherskinItem.colorShiftColor;
};

function mixFlaskLeatherskinImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskLeatherskin();
	%obj.setWhiteOut(0.7);
	
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	
	if(isObject(%obj.client))
	{
		messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
		serverCmdUnUseTool(%obj.client);
	}
	else
		%obj.unMountImage(%slot);
}

function Player::PotionTick_FlaskLeatherskin(%obj, %tick)
{
    if (%tick >= 60)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Leathersin dose wears off.");
        %obj.sunResistance -= 1.5;
        return;
    }
    else if (%tick == 0)
    {
        %obj.sunResistance += 1.5;
    }

    %obj.PotionSchedule["PotionTick_FlaskLeatherskin"] = %obj.schedule(1000, "PotionTick_FlaskLeatherskin", %tick + 1);
}