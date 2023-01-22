datablock AudioProfile(OilPumpEquipSound)
{
    filename    = "./Sounds/PumpEquip.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(OilPumpTickSound)
{
    filename    = "./Sounds/PumpTick.wav";
    description = AudioClosest3d;
    preload = true;
};

$EOTW::ItemCrafting["OilPumpItem"] = (48 TAB "Steel") TAB (16 TAB "Rubber");
$EOTW::ItemDescription["OilPumpItem"] = "Can be used on oil wells to suck oil. Requires 50 EU/s. Charge at a charge pad.";
datablock itemData(OilPumpItem)
{
	uiName = "TLS - Oil Pump";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.70 0.70 0.25 1.00";
	
	shapeFile = "base/data/shapes/printGun.dts";
	image = OilPumpImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(OilPumpImage)
{
	shapeFile = "base/data/shapes/printGun.dts";
	item = OilPumpItem;
	
	mountPoint = 0;
	offset = "0 0.25 0.15";
	rotation = eulerToMatrix("0 5 70");
	
	eyeOffset = "0.75 1.15 -0.24";
	eyeRotation = eulerToMatrix("0 5 70");
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = OilPumpItem.doColorShift;
	colorShiftColor = OilPumpItem.colorShiftColor;

	printPlayerBattery = true;
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= OilPumpEquipSound;
	
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

function OilPumpImage::onFire(%this, %obj, %slot)
{
    if (!isObject(%client = %obj.client))
        return;

    %eye = %obj.getEyePoint();
    %dir = %obj.getEyeVector();
    %for = %obj.getForwardVector();
    %face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
    %mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
    %ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
    if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick")
    {
        if (%hit.getDataBlock().getName() $= "brickEOTWOilGeyserData" && %hit.OilCapacity > 0)
        {
            if(%hit.beingCollected > 0 && %hit.beingCollected != %client.bl_id)
                %hit.centerPrint("<color:FFFFFF>Someone is already draining that oil geyser!", 3);
            else
            {
                %hit.lastGatherTick = getSimTime();
                %hit.beingCollected = %client.bl_id;
                %hit.cancelCollecting = %hit.schedule(10000, "cancelCollecting");
                %obj.collectOilLoop(%hit);
            }
        }
    }
}

function Player::collectOilLoop(%obj, %target)
{
	cancel(%obj.collectOilSchedule);
	
    if (!isObject(%client = %obj.client))
        return;

    %eye = %obj.getEyePoint();
    %dir = %obj.getEyeVector();
    %for = %obj.getForwardVector();
    %face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
    %mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
    %ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
    if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "fxDtsBrick" && %hit == %target)
    {
		%energyPerTick = 1;
		%PowerTickRate = 50;
		if (%obj.GetBatteryEnergy() < %energyPerTick)
		{
			%client.chatMessage("\c6You need to sustain " @ (%energyPerTick * %PowerTickRate) @ " EU drain per second to drain the well! Charge at a charge pad brick.");
		}
		else
		{
			%target.gatherProcess += getSimTime() - %hit.lastGatherTick;

			%obj.ChangeBatteryEnergy(%energyPerTick * -1);

			%totalTime = mClamp(2000 - (%target.OilCapacity * 2), 100, 2000);
			if (%target.gatherProcess >= %totalTime)
			{
				%oilPerCycle = getMin(8, %target.OilCapacity);
				%obj.ChangeMatterCount("Crude Oil", %oilPerCycle);
				//%client.chatMessage("\c6Sapped " @ %oilPerCycle @ " crude oil.");
				ServerPlay3D(OilPumpTickSound,%target.getPosition());

				%target.gatherProcess = 0;

				%target.OilCapacity -= %oilPerCycle;

				if (%target.OilCapacity <= 0)
				{
					%target.killBrick();
					return;
				}
			}

			%client.centerPrint("\c6Sucking Crude Oil... (" @ %target.OilCapacity @ "u crude oil left)<br>\c6" @ mFloor((%target.gatherProcess / %totalTime) * 100) @ "% done.",1);

			%hit.lastGatherTick = getSimTime();
			%hit.beingCollected = %client.bl_id;
			cancel(%hit.cancelCollecting);
			%hit.cancelCollecting = %hit.schedule(10000, "cancelCollecting");
			%obj.collectOilSchedule = %obj.schedule(1000 / %PowerTickRate, "collectOilLoop", %target);
		}
		
    }
}