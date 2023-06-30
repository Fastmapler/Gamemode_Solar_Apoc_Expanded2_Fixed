$EOTW::ModuleTickRate = 20;

function Player::ModuleTick(%player)
{
    cancel(%player.ModuleTickSchedule);

    if (!isObject(%client = %player.client) || getFieldCount(%player.EOTW_ActivatedModules) < 1)
        return;

    %mods = %player.EOTW_ActivatedModules;
    for (%i = 0; %i < getFieldCount(%mods); %i++)
    {
        %funcName = "EOTW_Module" @ getField(%mods, %i);
        if (!isFunction(%funcName))
            continue;

        %ret = call(%funcName, %player);

        if (!%ret)
        {
            %fail = true;
            break;
        }
    }

	if (%fail || %player.GetBatteryEnergy() <= 0)
	{
		%player.EOTW_ActivatedModules = "";
		%client.chatMessage("You ran out of power!");
		%player.playAudio(0, EOTWModuleOffSound);
	}

    %player.ModuleTickSchedule = %player.schedule(1000 / $EOTW::ModuleTickRate, "ModuleTick");
}

function Player::ToggleModule(%player, %mod)
{
    if (!isObject(%client = %player.client))
        return;

    if (!%player.ChangeBatteryEnergy(-10))
    {
        %client.chatMessage("You have no power!");
        %player.playAudio(0, errorSound);
        return;
    }
    %player.playThread(0, "plant");
    if (hasField(%player.EOTW_ActivatedModules, %mod))
    {
        %player.EOTW_ActivatedModules = removeFieldText(%player.EOTW_ActivatedModules, %mod);
        %client.chatMessage("\c6Module [\c3" @ %mod @ "\c6] is now \c0OFF");
        %player.playAudio(0, EOTWModuleOffSound);
    }
    else
    {
        %player.EOTW_ActivatedModules = trim(%player.EOTW_ActivatedModules TAB %mod);
        %client.chatMessage("\c6Module [\c3" @ %mod @ "\c6] is now \c2ON");
        %player.playAudio(0, EOTWModuleOnSound);

        if (!isEventPending(%player.ModuleTickSchedule))
            %player.ModuleTick();
    }
}

datablock AudioProfile(EOTWModuleOnSound)
{
    filename    = "./Sounds/module_on.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(EOTWModuleOffSound)
{
    filename    = "./Sounds/module_off.wav";
    description = AudioClosest3d;
    preload = true;
};

$EOTW::ItemCrafting["EOTWModuleSolarShieldItem"] = (128 TAB "Sturdium") TAB (128 TAB "Rare Earths") TAB (128 TAB "Plutonium");
$EOTW::ItemDescription["EOTWModuleSolarShieldItem"] = "Generates a solar shield around you! Uses 100 EU/s.";
datablock itemData(EOTWModuleSolarShieldItem)
{
	uiName = "Po. Sol. Shield";
	iconName = "./Shapes/icon_Module";
	doColorShift = true;
	colorShiftColor = "1.00 0.00 0.00 1.00";
	
	shapeFile = "./Shapes/Module.dts";
	image = EOTWModuleSolarShieldImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(EOTWModuleSolarShieldImage)
{
	shapeFile = "./Shapes/Module.dts";
	item = EOTWModuleSolarShieldItem;
	
	mountPoint = 0;
	offset = "0 0.5 0";
	rotation = 0;
	
	eyeOffset = "";
	eyeRotation = "";
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
    printPlayerBattery = true;
    
	melee = false;
	armReady = true;

	doColorShift = EOTWModuleSolarShieldItem.doColorShift;
	colorShiftColor = EOTWModuleSolarShieldItem.colorShiftColor;
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.1;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]		= true;
	
	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.1;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "checkFire";
	
	stateName[3]					= "checkFire";
	stateTransitionOnTriggerUp[3] 	= "Ready";
};

function EOTWModuleSolarShieldImage::onFire(%this, %obj, %slot) { %obj.ToggleModule("SolarShield"); }

function EOTW_ModuleSolarShield(%obj)
{
    %gotBat = %obj.GetBatteryEnergy() >= 100;
    if ($EOTW::Time < 12 && %gotBat)
    {
        if (!isObject(%obj.shieldShape))
		{
			%obj.shieldShape = new StaticShape()
			{
				datablock = SolarShieldProjectorShieldShape;
				position = %obj.getPosition();
				scale = "1 1 1";
			};
			%obj.shieldShape.setTransform(getWords(%obj.getPosition(), 0, 2));
			%obj.shieldShape.EOTW_SetShieldLevel(18);

			if (!isObject(SolarShieldGroup))
				new SimSet(SolarShieldGroup);
		
			SolarShieldGroup.add(%obj.shieldShape);
		}
        else
            %obj.shieldShape.setTransform(%obj.getTransform());

		//Using schedules to make sure we stop the projector if we get shutoff via events
		//Or if the bricks get hammered.
		cancel(%obj.shieldShape.shieldSchedule);
		%obj.shieldShape.shieldSchedule = %obj.shieldShape.schedule(2000 / $EOTW::ModuleTickRate, "delete");

        return %obj.ChangeBatteryEnergy(-100 / $EOTW::ModuleTickRate);
    }

    return %gotBat;
}