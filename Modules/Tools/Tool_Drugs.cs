$EOTW::ItemCrafting["mixLeanItem"] = (16 TAB "Healing Mix") TAB (16 TAB "Adrenline Mix") TAB (32 TAB "Plastic");
$EOTW::ItemDescription["mixLeanItem"] = "Randomly increases or decreases max move speed on use. Makes you purple. lasts until death.";
datablock ItemData(mixLeanItem : mixFlaskHealingItem)
{
    uiName = "Mix - Illicit Lean";
    colorShiftColor = "1.00 0.00 1.00 1.00";
    image = mixLeanImage;
};

datablock ShapeBaseImageData(mixLeanImage : mixFlaskHealingImage)
{
    item = mixLeanItem;
    doColorShift = mixLeanItem.doColorShift;
    colorShiftColor = mixLeanItem.colorShiftColor;
};

function mixLeanImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskLean();
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

function Player::PotionTick_FlaskLean(%obj, %tick)
{
    if (!isObject(%client = %obj.client))
        return;

    servercmdupdatebodycolors(%client, "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1", "1 0 1 1");

    if (getRandom() < 0.3)
    {
         %obj.ChangeSpeedMulti(-0.25);
         %client.chatMessage("\cp<color:ff00ff>You feel slightly slower...\co");
         return;
    }

    %obj.ChangeSpeedMulti(0.25);
    %client.chatMessage("\cp<color:ff00ff>You feel slightly faster!\co");
}

$EOTW::ItemCrafting["mixMethItem"] = (16 TAB "Leatherskin Mix") TAB (16 TAB "Overload Mix") TAB (32 TAB "Plastic");
$EOTW::ItemDescription["mixMethItem"] = "Gives extreme damage reduction, but gives very disorienting visual effects.";
datablock ItemData(mixMethItem : mixSyringeHealingItem)
{
    uiName = "Mix - Illicit Meth";
    colorShiftColor = "1.00 1.00 1.00 1.00";
    image = mixMethImage;
};

datablock ShapeBaseImageData(mixMethImage : mixSyringeHealingImage)
{
    item = mixMethItem;
    doColorShift = mixMethItem.doColorShift;
    colorShiftColor = mixMethItem.colorShiftColor;
};

function mixMethImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskMeth();
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

function Player::PotionTick_FlaskMeth(%obj, %tick)
{
	if (!isObject(%client = %obj.client))
        return;
	
	if (%tick >= 60)
    {
		%client.chatMessage("\c6The effect of the Meth dose wears off.");
		%client.setControlCameraFOV(120);
        %obj.damageReduction--;
        return;
    }
    else if (%tick == 0)
    {
        %obj.damageReduction++;
    }
	
	%obj.setWhiteout(getRandom() / 2);
	%client.setControlCameraFOV(getRandom(75, 150));

    %obj.PotionSchedule["PotionTick_FlaskMeth"] = %obj.schedule(250, "PotionTick_FlaskMeth", %tick + 0.25);
}

$EOTW::ItemCrafting["mixBathSaltsItem"] = (16 TAB "Gatherer Mix") TAB (16 TAB "Steroid Mix") TAB (32 TAB "Plastic");
$EOTW::ItemDescription["mixBathSaltsItem"] = "Forces you to wield extremely powerful melee fists.";
datablock ItemData(mixBathSaltsItem : mixFlaskHealingItem)
{
    uiName = "Mix - Illicit Bath Salts";
    colorShiftColor = "1.00 1.00 1.00 1.00";
    image = mixBathSaltsImage;
};

datablock ShapeBaseImageData(mixBathSaltsImage : mixFlaskHealingImage)
{
    item = mixBathSaltsItem;
    doColorShift = mixBathSaltsItem.doColorShift;
    colorShiftColor = mixBathSaltsItem.colorShiftColor;
};

function mixBathSaltsImage::onFire(%this,%obj,%slot)
{
	%currSlot = %obj.currTool;

	%obj.PotionTick_FlaskBathSalts();
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

function Player::PotionTick_FlaskBathSalts(%obj, %tick)
{
	if (!isObject(%client = %obj.client))
        return;
	
	if (%tick >= 60)
    {
		swolMelee_UnMount(%obj, %obj.meleeHand);
		%client.chatMessage("\c6The effect of the Bath Salts dose wears off.");
		%obj.steroidlevel -= 10;
		return;
    }
	else if (%tick == 0)
	{
		%obj.steroidlevel += 10;
	}

	if (isObject(%image = %obj.getMountedImage(0)) || !isObject(%obj.meleeHand) || %obj.meleeHand.getDatablock().getID() != meleeFistsPlayer.getID())
	{
		%obj.unMountImage(0);
		swolMelee_Mount(%obj, fistImage);
	}

    %obj.PotionSchedule["PotionTick_FlaskBathSalts"] = %obj.schedule(100, "PotionTick_FlaskBathSalts", %tick + 0.1);
}