//Healing
$EOTW::ItemCrafting["mixSyringeHealingItem"] = (32 TAB "Healing Mix") TAB (32 TAB "Iron");
$EOTW::ItemDescription["mixSyringeHealingItem"] = "Upon consumption heals 80 HP in 5 seconds.";
datablock ItemData(mixSyringeHealingItem)
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

    uiName = "MIX - Syringe Healing";
    //iconName = "./potion";
    doColorShift = true;
    colorShiftColor = "0.85 0.40 0.40 1.00";

    image = mixSyringeHealingImage;
    canDrop = true;
};

datablock ShapeBaseImageData(mixSyringeHealingImage)
{
    shapeFile = "./Shapes/Syringe.dts";
    emap = true;

    mountPoint = 0;
    offset = "0 0 0";
    eyeOffset = 0;
    rotation = eulerToMatrix( "0 0 0" );

    className = "WeaponImage";
    item = mixSyringeHealingItem;

    armReady = true;

    doColorShift = mixSyringeHealingItem.doColorShift;
    colorShiftColor = mixSyringeHealingItem.colorShiftColor;

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

function mixSyringeHealingImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_SyringeHealing();
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

function Player::PotionTick_SyringeHealing(%obj, %tick)
{
    if (%tick >= 5)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Healing dose wears off.");
        return;
    }

    %obj.addHealth(16);
    %obj.PotionSchedule["PotionTick_SyringeHealing"] = %obj.schedule(1000, "PotionTick_SyringeHealing", %tick + 1);
}

//Steroids
$EOTW::ItemCrafting["mixSyringeSteroidItem"] = (32 TAB "Steroid Mix") TAB (32 TAB "Iron");
$EOTW::ItemDescription["mixSyringeSteroidItem"] = "Upon consumption increases melee damge by 300% for 15 seconds.";
datablock ItemData(mixSyringeSteroidItem : mixSyringeHealingItem)
{
    uiName = "MIX - Syringe Steroids";
    colorShiftColor = "0.25 0.00 0.00 1.00";
    image = mixSyringeSteroidImage;
};

datablock ShapeBaseImageData(mixSyringeSteroidImage : mixSyringeHealingImage)
{
    item = mixSyringeSteroidItem;
    doColorShift = mixSyringeSteroidItem.doColorShift;
    colorShiftColor = mixSyringeSteroidItem.colorShiftColor;
};

function mixSyringeSteroidImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_SyringeSteroid();
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

function Player::PotionTick_SyringeSteroid(%obj, %tick)
{
    if (%tick >= 15)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Steroids dose wears off.");
        %obj.steroidlevel -= 3;
        return;
    }
    else if (%tick == 0)
    {
        %obj.steroidlevel += 3;
    }

    %obj.PotionSchedule["PotionTick_SyringeSteroid"] = %obj.schedule(1000, "PotionTick_SyringeSteroid", %tick + 1);
}

//Adrenline
$EOTW::ItemCrafting["mixSyringeAdrenlineItem"] = (32 TAB "Adrenline Mix") TAB (32 TAB "Iron");
$EOTW::ItemDescription["mixSyringeAdrenlineItem"] = "Upon consumption increases max movement speed by 250% for 10 seconds.";
datablock ItemData(mixSyringeAdrenlineItem : mixSyringeHealingItem)
{
    uiName = "MIX - Syringe Adrenline";
    colorShiftColor = "0.85 0.85 0.40 1.00";
    image = mixSyringeAdrenlineImage;
};

datablock ShapeBaseImageData(mixSyringeAdrenlineImage : mixSyringeHealingImage)
{
    item = mixSyringeAdrenlineItem;
    doColorShift = mixSyringeAdrenlineItem.doColorShift;
    colorShiftColor = mixSyringeAdrenlineItem.colorShiftColor;
};

function mixSyringeAdrenlineImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_SyringeAdrenline();
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

function Player::PotionTick_SyringeAdrenline(%obj, %tick)
{
    if (%tick >= 10)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Adrenline dose wears off.");
        %obj.ChangeSpeedMulti(-2.5);
        return;
    }
    else if (%tick == 0)
    {
        %obj.ChangeSpeedMulti(2.5);
    }

    %obj.PotionSchedule["PotionTick_SyringeAdrenline"] = %obj.schedule(1000, "PotionTick_SyringeAdrenline", %tick + 1);
}

//Gatherer
$EOTW::ItemCrafting["mixSyringeGathererItem"] = (32 TAB "Gatherer Mix") TAB (32 TAB "Iron");
$EOTW::ItemDescription["mixSyringeGathererItem"] = "Upon consumption increases gathering speed by 150% for 30 seconds.";
datablock ItemData(mixSyringeGathererItem : mixSyringeHealingItem)
{
    uiName = "MIX - Syringe Gatherer";
    colorShiftColor = "0.40 0.40 0.85 1.00";
    image = mixSyringeGathererImage;
};

datablock ShapeBaseImageData(mixSyringeGathererImage : mixSyringeHealingImage)
{
    item = mixSyringeGathererItem;
    doColorShift = mixSyringeGathererItem.doColorShift;
    colorShiftColor = mixSyringeGathererItem.colorShiftColor;
};

function mixSyringeGathererImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_SyringeGatherer();
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

function Player::PotionTick_SyringeGatherer(%obj, %tick)
{
    if (%tick >= 30)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Gatherer dose wears off.");
        %obj.Gathererlevel -= 1.5;
        return;
    }
    else if (%tick == 0)
    {
        %obj.Gathererlevel += 1.5;
    }

    %obj.PotionSchedule["PotionTick_SyringeGatherer"] = %obj.schedule(1000, "PotionTick_SyringeGatherer", %tick + 1);
}

//Overload
$EOTW::ItemCrafting["mixSyringeOverloadItem"] = (32 TAB "Overload Mix") TAB (32 TAB "Iron");
$EOTW::ItemDescription["mixSyringeOverloadItem"] = "Makes weapons very likely to give ammo, but inficts 5hp/s of self-harm all over 10 seconds.";
datablock ItemData(mixSyringeOverloadItem : mixSyringeHealingItem)
{
    uiName = "MIX - Syringe Overload";
    colorShiftColor = "0.25 0.25 0.25 1.00";
    image = mixSyringeOverloadImage;
};

datablock ShapeBaseImageData(mixSyringeOverloadImage : mixSyringeHealingImage)
{
    item = mixSyringeOverloadItem;
    doColorShift = mixSyringeOverloadItem.doColorShift;
    colorShiftColor = mixSyringeOverloadItem.colorShiftColor;
};

function mixSyringeOverloadImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_SyringeOverload();
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

function Player::PotionTick_SyringeOverload(%obj, %tick)
{
    if (%tick >= 10)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Overload dose wears off.");
        %obj.steroidlevel -= 2;
        %obj.ammoReturnLevel -= 5;
        return;
    }
    else if (%tick == 0)
    {
        %obj.steroidlevel += 2;
        %obj.ammoReturnLevel += 5;
    }
    %obj.addHealth(-2);

    %obj.PotionSchedule["PotionTick_SyringeOverload"] = %obj.schedule(1000, "PotionTick_SyringeOverload", %tick + 1);
}

//Leatherskin
$EOTW::ItemCrafting["mixSyringeLeatherskinItem"] = (32 TAB "Leatherskin Mix") TAB (32 TAB "Iron");
$EOTW::ItemDescription["mixSyringeLeatherskinItem"] = "Upon consumption reduces sun damage by 4 points for 20 seconds.";
datablock ItemData(mixSyringeLeatherskinItem : mixSyringeHealingItem)
{
    uiName = "MIX - Syringe Leatherskin";
    colorShiftColor = "1.00 0.50 0.00 1.00";
    image = mixSyringeLeatherskinImage;
};

datablock ShapeBaseImageData(mixSyringeLeatherskinImage : mixSyringeHealingImage)
{
    item = mixSyringeLeatherskinItem;
    doColorShift = mixSyringeLeatherskinItem.doColorShift;
    colorShiftColor = mixSyringeLeatherskinItem.colorShiftColor;
};

function mixSyringeLeatherskinImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_SyringeLeatherskin();
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

function Player::PotionTick_SyringeLeatherskin(%obj, %tick)
{
    if (%tick >= 20)
    {
        if(isObject(%client = %obj.client))
            %client.chatMessage("\c6The effect of the Leathersin dose wears off.");
        %obj.sunResistance -= 4;
        return;
    }
    else if (%tick == 0)
    {
        %obj.sunResistance += 4;
    }

    %obj.PotionSchedule["PotionTick_SyringeLeatherskin"] = %obj.schedule(1000, "PotionTick_SyringeLeatherskin", %tick + 1);
}