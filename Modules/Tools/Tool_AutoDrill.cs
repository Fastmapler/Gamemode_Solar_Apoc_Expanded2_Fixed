$EOTW::ItemCrafting["AutoDrillItem"] = (256 TAB "Plasteel") TAB (128 TAB "Granite Polymer") TAB (64 TAB "Lubricant");
$EOTW::ItemDescription["AutoDrillItem"] = "An EU-powered drill which gathers a material for you. Base speed of 1.5%.";

datablock itemData(AutoDrillItem)
{
	uiName = "Auto-Drill I";
	iconName = "./Icons/icon_Drill";
	doColorShift = true;
	colorShiftColor = "0.400 0.400 0.400 1.000";
	
	shapeFile = "./Shapes/Drill.dts";
	image = AutoDrillImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(AutoDrillImage)
{
	shapeFile = "./Shapes/Drill.dts";
	item = AutoDrillItem;
	
	mountPoint = 0;
	offset = "0 0 0";
	rotation = 0;
	
	eyeOffset = "0 0 0";
	eyeRotation = 0;
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	printPlayerBattery = true;

	doColorShift = AutoDrillItem.doColorShift;
	colorShiftColor = AutoDrillItem.colorShiftColor;
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.5;
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

function AutoDrillImage::onFire(%this, %obj, %slot) { %obj.spawnDrill(%this, 1.5); }

$EOTW::ItemCrafting["AutoDrill2Item"] = (256 TAB "Adamantine") TAB (128 TAB "GT Diamond") TAB (256 TAB "Lubricant");
$EOTW::ItemDescription["AutoDrill2Item"] = "An EU-powered drill which gathers a material for you. Base speed of 225%.";

datablock itemData(AutoDrill2Item : AutoDrillItem)
{
	uiName = "Auto-Drill II";
	colorShiftColor = "0.400 0.400 0.800 1.000";
	image = AutoDrill2Image;
};

datablock shapeBaseImageData(AutoDrill2Image : AutoDrillImage)
{
	item = AutoDrill2Item;
	colorShiftColor = AutoDrill2Item.colorShiftColor;
};

function AutoDrill2Image::onFire(%this, %obj, %slot) { %obj.spawnDrill(%this, 2.25); }

datablock StaticShapeData(AutoDrillStatic)
{
    shapeFile = "./Shapes/Drill.dts";
};

function Player::spawnDrill(%obj, %image, %multiplier)
{
	//get the gatherable brick the player is looking at
	//Account thumpers but not gathering potion buff
	//Place drill and remove item from inventory

	if (!isObject(%client = %obj.client) || !isObject(%hit = %obj.whatBrickAmILookingAt()) || !%hit.isCollectable)
		return;

	if (%client.GetBatteryEnergy() == 0)
	{
		%client.chatMessage("\c6You need battery power to use this drill!");
		return;
	}

	if (%multiplier == 0)
		%multiplier = 1;

	%currSlot = %obj.currTool;
	%item = %obj.tool[%currSlot];
	%obj.tool[%currSlot] = 0;
	%obj.weaponCount--;
	messageClient(%obj.client,'MsgItemPickup','',%currSlot,0);
	serverCmdUnUseTool(%obj.client);

	%drill = new StaticShape()
	{
		datablock = AutoDrillStatic;
		client = %client;
		player = %obj;
		item = %item;
	};

	%hit.lastGatherTick = getSimTime();
	
	initContainerRadiusSearch(%hit.getPosition(), 64, $Typemasks::fxBrickAlwaysObjectType);
	while(isObject(%scan = containerSearchNext()))
		if(%scan.getClassName() $= "fxDtsBrick" && getSimTime() - %scan.lastThump < 1000)
			%multiplier++;

	%drill.setTransform(vectorAdd(%hit.getPosition(), "0 0 0.6") SPC "1 0 0 1.57");
	%drill.origin = %drill.getTransform();
	%drill.setShapeNameDistance(64);
	%drill.schedule(20, "drillCollectLoop", %hit, %multiplier);
}

function StaticShape::drillCollectLoop(%obj, %brick, %multiplier)
{
	cancel(%obj.collectLoop);
	if(!isObject(%client = %obj.client) || !isObject(%player = %client.player)) return %obj.StopDrill();
	if(!isObject(%brick) || %brick.isDead()) return %obj.StopDrill();
	if(%brick && %brick.material !$= "")
	{
		if (!isObject(%brick.matterType))
			%brick.matterType = getMatterType(%brick.material);
			
		cancel(%brick.cancelCollecting);

		//Jiggle physics
		%obj.setTransform(vectorAdd(getWords(%obj.origin, 0, 2), vectorScale((getRandom() - 0.5) SPC (getRandom() - 0.5) SPC (getRandom() - 0.5), 0.25 * %multiplier)) SPC getWords(%obj.origin, 3, 6));
		
		%reqFuel = %brick.matterType.requiredCollectFuel;
		%powerCost = randomRound((getSimTime() - %brick.lastGatherTick) * %multiplier * -0.04);
		if (%reqFuel !$= "" && %player.GetMatterCount(getField(%reqFuel, 0)) < getField(%reqFuel, 1))
		{
			%client.chatMessage("\c6You need atleast " @ getField(%reqFuel, 1) SPC getField(%reqFuel, 0) @ " to drill this " @ %brick.matterType.name @ "!");
			return %obj.StopDrill();
		}

		%change = %client.ChangeBatteryEnergy(%powerCost);
		if (%powerCost < %change)
		{
			%client.chatMessage("\c6You need more battery power to drill this " @ %brick.matterType.name @ "!");
			return %obj.StopDrill();
		}
		if (%brick.gatherProcess >= %brick.matterType.collectTime)
		{
			if (%reqFuel !$= "")
				%player.ChangeMatterCount(getField(%reqFuel, 0), getField(%reqFuel, 1) * -1);

			%oreValue = GetMatterValueData(%brick.matterType.name);
			if (%oreValue != -1)
				%client.incScore(getField(%oreValue, 1));
				
			$EOTW::Material[%client.bl_id, %brick.matterType.name] += %brick.matterType.spawnValue;
			%brick.killBrick();
			return %obj.StopDrill();
		}
		else
		{
			%brick.cancelCollecting = %brick.schedule(10000, "cancelCollecting");
			%brick.beingCollected = %client.bl_id;
			%obj.collectLoop = %obj.schedule(20, "drillCollectLoop", %brick, %multiplier);
			%obj.setShapeName(mFloor((%brick.gatherProcess / %brick.matterType.collectTime) * 100) @ "%");
			%brick.gatherProcess += (getSimTime() - %brick.lastGatherTick) * %multiplier;
			%brick.lastGatherTick = getSimTime();
		}
		
	}
}

function StaticShape::StopDrill(%obj)
{
	if (isObject(%itemData = %obj.item))
	{
		if (isObject(%player = %obj.player))
			%position = %player.getTransform();
		else
			%position = %obj.getTransform();

		%item = new Item()
		{
			datablock = %itemData;
			position  = %position;
		};

		if (isObject(%player))
			%player.pickup(%item);
	}

	%obj.delete();
	return 0;
}