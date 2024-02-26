datablock AudioProfile(MiningScannerUseSound)
{
    filename    = "./Sounds/ScannerUse.wav";
    description = AudioClosest3d;
    preload = true;
};

datablock AudioProfile(MiningScannerUse2Sound)
{
    filename    = "./Sounds/Tricorder.wav";
    description = AudioClosest3d;
    preload = true;
};

$EOTW::ItemCrafting["MiningScannerItem"] = (128 TAB "Steel") TAB (64 TAB "Copper");
$EOTW::ItemDescription["MiningScannerItem"] = "Shows surface gatherables or underground veins. Uses your personal battery. Crouch and scan to use UGVein mode";
datablock itemData(MiningScannerItem)
{
	uiName = "Mining Scanner";
	iconName = "./Icons/icon_glitchdetector";
	doColorShift = false;
	colorShiftColor = "0.40 0.40 0.40 1.00";
	
	shapeFile = "./Shapes/glitchdetector.dts";
	image = MiningScannerImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(MiningScannerImage)
{
	shapeFile = "./Shapes/glitchdetector.dts";
	item = MiningScannerItem;
	
	mountPoint = 0;
	offset = "0 0 0";
	//rotation = eulerToMatrix("0 5 70");
	
	//eyeOffset = "0.75 1.15 -0.24";
	//eyeRotation = eulerToMatrix("0 5 70");
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = MiningScannerItem.doColorShift;
	colorShiftColor = MiningScannerItem.colorShiftColor;

	printPlayerBattery = true;
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]		= true;
	
	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.5;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "checkFire";
	
	stateName[3]					= "checkFire";
	stateTransitionOnTriggerUp[3] 	= "Ready";
};

function MiningScannerImage::onFire(%this, %obj, %slot)
{
	if (!isObject(%client = %obj.client))
        return;

	if (%obj.isCrouched())
	{
		%energyCost = 125;
		if (%obj.GetBatteryEnergy() >= %energyCost)
		{
			ServerPlay3D(MiningScanner2UseSound,%obj.getPosition());
			%obj.ChangeBatteryEnergy(%energyCost * -1);

			%veinList = getUGVeins(%obj.getPosition());
			%veinCount = getFieldCount(%veinList);

			if (%veinCount > 0)
			{
				for (%i = 0; %i < %veinCount; %i++)
				{
					%vein = getField(%veinList, %i);

					%readyMessage = %vein.ready ? "\c2Exploitable!" : "\c0Still Developing";
					%client.chatMessage("\c6" @ %vein.matter @ " Vein, ~" @ getUGVeinComp(%vein, %obj.getPosition()) @ "u Sample." SPC %readyMessage);
					//Debug Scan
					//%client.chatMessage(%vein.matter SPC getUGVeinComp(%vein, %obj.getPosition()) SPC %vein.size SPC %vein.maxSize SPC %vein.ready SPC %vein.position);
					//%shape = createCylinderMarker(%vein.position SPC getWord(%obj.getPosition(), 2), "1 0 0", vectorScale(1 SPC %vein.size SPC %vein.size, 2));
					//%shape.setTransform(%shape.getPosition() SPC "0 1.366 0 1");
					//%shape.schedule(2000, "delete");
				}
			}
			else
				%client.chatMessage("No underground veins found here.");
		}
		else
		{
			%client.chatMessage("\c6You need atleast " @ %energyCost @ " EU to scan underground veins! Charge at a charge pad brick.");
		}
	}
	else
	{
		%energyCost = 500;
		if (%obj.GetBatteryEnergy() >= %energyCost)
		{
			ServerPlay3D(MiningScannerUseSound,%obj.getPosition());
			%obj.ChangeBatteryEnergy(%energyCost * -1);
			%obj.MiningScannerPing(128, 10000);
		}
		else
		{
			%client.chatMessage("\c6You need atleast " @ %energyCost @ " EU to scan surface gatherables! Charge at a charge pad brick.");
		}
	}
}

function Player::MiningScannerPing(%obj, %range, %time)
{
    initContainerRadiusSearch(%obj.getPosition(), %range, $TypeMasks::FxBrickAlwaysObjectType);
    while(isObject(%hit = containerSearchNext()))
	{
		if (%hit.isCollectable && getSimTime() - %hit.lastScanTime > 10000)
		{
			%hit.lastScanTime = getSimTime();
			%hit.schedule(vectorDist(%obj.getPosition(), %hit.getPosition()) * 25, "TempColorFX", 3, %time, true);
		}
	}       
}

function fxDtsBrick::TempColorFX(%obj, %fx, %time, %makeArrow)
{
    if (%obj.TempColorFxSchedule !$= "")
        return;

	if (%makeArrow)
	{
		%pos = %obj.getPosition();
		%marker = drawArrow2(vectorAdd(%pos, "0 0 20"), vectorAdd(%pos, "0 0 0.5"), getColorIDTable(%obj.getColorID()), 1, "");
		%marker.schedule(%time, "delete");
	}
	%obj.TempColorFxSchedule = %obj.schedule(%time, "TempColorFxEnd", %obj.getColorFXID());
	%obj.setColorFx(%fx);
}

function fxDtsBrick::TempColorFxEnd(%obj, %fx)
{
	%obj.TempColorFxSchedule = "";
	%obj.setColorFx(%fx);
}