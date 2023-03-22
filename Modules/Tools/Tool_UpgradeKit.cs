$EOTW::ItemCrafting["UpgradeToolItem"] = (128 TAB "Steel") TAB (64 TAB "Gold");
$EOTW::ItemDescription["UpgradeToolItem"] = "Allows the upgrading of some machines to run faster. Requires additional resources to use!";

datablock itemData(UpgradeToolItem)
{
	uiName = "Upgrade Tool";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.10 0.10 0.10 1.00";
	
	shapeFile = "./Shapes/HandDrll.dts";
	image = UpgradeToolImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(UpgradeToolImage)
{
	shapeFile = "./Shapes/HandDrll.dts";
	item = UpgradeToolItem;
	
	mountPoint = 0;
	offset = "0 0 0";
	rotation = 0;
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = UpgradeToolItem.doColorShift;
	colorShiftColor = UpgradeToolItem.colorShiftColor;
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]		= true;
	
	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.2;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "checkFire";
	
	stateName[3]					= "checkFire";
	stateTransitionOnTriggerUp[3] 	= "Ready";
};


function UpgradeToolImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);
   
   if (!isObject(%client = %obj.client))
		return;
	
	%obj.UpgradeToolMessage();
}

function UpgradeToolImage::onUnMount(%this, %obj, %slot)
{
	Parent::onUnMount(%this, %obj, %slot);
	
	cancel(%obj.UpgradeToolMessageLoop);

    if(isObject(%client = %obj.client))
        %client.centerPrint("", 0);
   
}

function UpgradeToolImage::onFire(%this, %obj, %slot)
{
	if(!isObject(%client = %obj.client))
		return;
	
	%pos = %obj.getEyePoint();
	%vector = vectorAdd(%pos, vectorScale(%obj.getEyeVector(), 8));
	%targets = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = ContainerRayCast(%pos, %vector, %targets, %obj);
	%col = getWord(%ray, 0);
	%hitpos = getWords(%ray, 1, 3);
	
	if (isObject(%col) && (%col.getType() & $TypeMasks::FxBrickObjectType))
	{
		%db = %col.getDatablock();
		%target = %db.uiName;
        %costData = $EOTW::BrickUpgrade[%db.getName(), %col.upgradeTier + 0];
        if (%costData $= "")
        {
            if ($EOTW::BrickUpgrade[%db.getName(), "MaxTier"] > 0)
                %client.chatMessage("The machine is fully upgraded!");
            else
                %client.chatMessage("You cannot use the upgrade tool on this!");

            %client.play2d(errorSound);
            return;
        }

        for (%i = 0; %i < getFieldCount(%costData); %i += 2)
        {
            if (%obj.GetMatterCount(getField(%costData, %i + 1)) < getField(%costData, %i))
            {
                %client.chatMessage("You need more " @ getField(%costData, %i + 1) @ "!");
                return;
            }
        }

        for (%i = 0; %i < getFieldCount(%costData); %i += 2)
            %obj.ChangeMatterCount(getField(%costData, %i + 1), getField(%costData, %i) * -1);

		%obj.playThread(2, shiftaway);
        %col.upgradeTier++;
        %col.spawnExplosion(upgradeExplosionProjectile, 1.0);
        %col.playSound(UpgradePickaxeSound);
    }
}

function Player::UpgradeToolMessage(%obj)
{
	cancel(%obj.UpgradeToolMessageLoop);
	
	if (!isObject(%client = %obj.client))
		return;
		
	%pos = %obj.getEyePoint();
	%vector = vectorAdd(%pos, vectorScale(%obj.getEyeVector(), 8));
	%targets = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::TerrainObjectType | $TypeMasks::StaticShapeObjectType;
	%ray = ContainerRayCast(%pos, %vector, %targets, %obj);
	%col = getWord(%ray, 0);
	%hitpos = getWords(%ray, 1, 3);
	
	%target = "--";
	%tier = "--";
	
	if (isObject(%col) && (%col.getType() & $TypeMasks::FxBrickObjectType))
	{
		%db = %col.getDatablock();
		%target = %db.uiName;

        if ($EOTW::BrickUpgrade[%db.getName(), "MaxTier"] > 0)
        {
            %tier = (%col.upgradeTier + 1) @ "/" @ ($EOTW::BrickUpgrade[%db.getName(), "MaxTier"] + 1);
            %cost = $EOTW::BrickUpgrade[%db.getName(), %col.upgradeTier + 0];

            for (%i = 0; %i < getFieldCount(%cost); %i += 2)
            {
                %type = getField(%cost, %i + 1);
                %amount = getField(%cost, %i);

                %displayCost = $EOTW::Material[%client.bl_id, %type] @ "/" @ %amount SPC getMatterTextColor(%type) @ %type @ "<br>Upgrading costs...<br>\c6"; 
            }
        }
        
	}
	
	%client.centerPrint("<just:left>\c6[\c3" @ %target @ "\c6] (Tier " @ %tier @ ")<br>" @ %displayCost, 1);
		
	%obj.UpgradeToolMessageLoop = %obj.schedule(100, "UpgradeToolMessage");
}
