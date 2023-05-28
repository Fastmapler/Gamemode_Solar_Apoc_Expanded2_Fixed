$EOTW::ItemCrafting["InvExpanderItem"] = (128 TAB "Copper") TAB (128 TAB "Steel") TAB (1024 TAB "Wood");
$EOTW::ItemDescription["InvExpanderItem"] = "A reusable tool to upgrade your maxinum inventory slots. Requires additional resources to use!";

datablock ItemData(InvExpanderItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Shapes/ISBackPackItem.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Inventory Expander";
	iconName = "./BackpackIcon";
	doColorShift = false;

	 // Dynamic properties defined by the scripts
	image = InvExpanderImage;
	canDrop = true;
};

datablock ShapeBaseImageData(InvExpanderImage)
{
   // Basic Item properties
   shapeFile = "./Shapes/ISBackPackItem.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0.09 -0.07 -0.2";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   className = "WeaponImage";
   item = InvExpanderItem;

   //raise your arm up or not
   armReady = true;

   doColorShift = false;

   // Initial start up state
   
    stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 1.0;
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

$EOTW::UpgradeCost["Inventory", 5] = (1024 TAB "Steel") TAB (1024 TAB "Wood");
$EOTW::UpgradeCost["Inventory", 6] = (1024 TAB "Plastic") TAB (1024 TAB "Steel");
$EOTW::UpgradeCost["Inventory", 7] = (1024 TAB "Adamantine") TAB (1024 TAB "Plastic");
$EOTW::UpgradeCost["Inventory", 8] = (1024 TAB "Plutonium") TAB (1024 TAB "Adamantine");

function InvExpanderImage::onMount(%this,%obj,%slot)
{
    if (isObject(%client = %obj.client))
    {
        %invCount = %client.GetMaxInvSlots();
        %cost = $EOTW::UpgradeCost["Inventory", %invCount];

        for (%i = 0; %i < getFieldCount(%cost); %i += 2)
        {
            %type = getField(%cost, %i + 1);
            %amount = getField(%cost, %i);

            %displayCost = %displayCost @ ($EOTW::Material[%client.bl_id, %type] + 0) @ "/" @ %amount SPC getMatterTextColor(%type) @ %type @ "<br>\c6"; 
        }

        %client.centerPrint("\c6Upgrade Cost for " @ (%invCount + 1) @ " slots:<br>\c6" @ %displayCost, 10);
    }
}

function InvExpanderImage::onUnmount(%this,%obj,%slot)
{
    if (isObject(%client = %obj.client))
        %client.centerPrint("", 0);
}

function InvExpanderImage::onFire(%this,%obj,%slot)
{
    if (!isObject(%client = %obj.client))
        return;

    %invCount = %client.GetMaxInvSlots();
	%costData = $EOTW::UpgradeCost["Inventory", %invCount];
	if (%costData $= "")
	{
		%client.centerPrint("\c0Whoops!<br>\c6You cannot upgrade your inventory further! Good work!",3);
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
	
    %client.SetMaxInvSlots(%client.GetMaxInvSlots() + 1);
    %client.schedule(33, "centerPrint", "\c4Upgrade Successful!", 2);
    %client.play2d(rewardSound);
    %obj.playThread(2, shiftaway);
	%obj.emote(InvExpandImage,1);
    %obj.unMountImage(0);
}

datablock ParticleData(InvExpandParticle)
{
   dragCoefficient      = 5.0;
   gravityCoefficient   = -0.2;
   inheritedVelFactor   = 0.0;
   constantAcceleration = 0.0;
   lifetimeMS           = 1000;
   lifetimeVarianceMS   = 500;
   useInvAlpha          = false;
   textureName          = "./Icons/InvExpand";
   colors[0]     = "1.0 1.0 1.0 1";
   colors[1]     = "1.0 1.0 1.0 1";
   colors[2]     = "0.0 0.0 0.0 0";
   sizes[0]      = 0.4;
   sizes[1]      = 0.6;
   sizes[2]      = 0.4;
   times[0]      = 0.0;
   times[1]      = 0.2;
   times[2]      = 1.0;
};

datablock ParticleEmitterData(InvExpandEmitter)
{
   ejectionPeriodMS = 35;
   periodVarianceMS = 0;
   ejectionVelocity = 0.5;
   ejectionOffset   = 1.0;
   velocityVariance = 0.49;
   thetaMin         = 0;
   thetaMax         = 120;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "InvExpandParticle";

   uiName = "Emote - Expand Inventory";
};

datablock ShapeBaseImageData(InvExpandImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	emap = false;

	mountPoint = $HeadSlot;

	stateName[0]					= "Ready";
	stateTransitionOnTimeout[0]		= "FireA";
	stateTimeoutValue[0]			= 0.01;

	stateName[1]					= "FireA";
	stateTransitionOnTimeout[1]		= "Done";
	stateWaitForTimeout[1]			= True;
	stateTimeoutValue[1]			= 0.350;
	stateEmitter[1]					= InvExpandEmitter;
	stateEmitterTime[1]				= 0.350;

	stateName[2]					= "Done";
	stateScript[2]					= "onDone";
};
function InvExpandImage::onDone(%this,%obj,%slot)
{
	%obj.unMountImage(%slot);
}



//Battery
$EOTW::ItemCrafting["BatteryExpanderItem"] = (128 TAB "Copper") TAB (128 TAB "Steel") TAB (1024 TAB "Wood");
$EOTW::ItemDescription["BatteryExpanderItem"] = "A reusable tool to upgrade your maxinum inventory slots. Requires additional resources to use!";

datablock ItemData(BatteryExpanderItem : InvExpanderItem)
{
	shapeFile = "./Shapes/Battery.dts";
	uiName = "Battery Expander";
	iconName = "";
	image = BatteryExpanderImage;
};

datablock ShapeBaseImageData(BatteryExpanderImage : InvExpanderImage)
{
   shapeFile = "./Shapes/Battery.dts";
   item = BatteryExpanderItem;
   BatteryExpanderImage.printPlayerBattery = true;
};

function getBatteryUpgradeCost(%amount)
{
    if (%amount >= 100000)
        return "";

    %amount += 5000;

    %copper  = 256 * mFloor(%amount / 7500);
    %silver  = 512 * mFloor(%amount / 15000);

    %plastic = 128 * mFloor(%amount / 5000);
    %teflon  = 256 * mFloor(%amount / 10000);
    %epoxy   = 512 * mFloor(%amount / 20000);

    return %copper TAB "Copper" TAB %silver TAB "Silver" TAB %plastic TAB "Plastic" TAB %teflon TAB "Teflon" TAB %epoxy TAB "Epoxy";
}

function BatteryExpanderImage::onMount(%this,%obj,%slot)
{
    if (isObject(%client = %obj.client))
    {
        %maxBattery = %client.GetMaxBatteryEnergy();
        %cost = getBatteryUpgradeCost(%maxBattery);

        for (%i = 0; %i < getFieldCount(%cost); %i += 2)
        {
            %type = getField(%cost, %i + 1);
            %amount = getField(%cost, %i);

            %displayCost = %displayCost @ ($EOTW::Material[%client.bl_id, %type] + 0) @ "/" @ %amount SPC getMatterTextColor(%type) @ %type @ "<br>\c6"; 
        }

        %client.centerPrint("\c6Upgrade Cost for " @ (%maxBattery + 5000) @ " Max EU:<br>\c6" @ %displayCost, 10);
    }
}

function BatteryExpanderImage::onUnmount(%this,%obj,%slot)
{
    if (isObject(%client = %obj.client))
        %client.centerPrint("", 0);
}

function BatteryExpanderImage::onFire(%this,%obj,%slot)
{
    if (!isObject(%client = %obj.client))
        return;

    %maxBattery = %client.GetMaxBatteryEnergy();
    %costData = getBatteryUpgradeCost(%maxBattery);
	if (%costData $= "")
	{
		%client.centerPrint("\c0Whoops!<br>\c6You cannot upgrade your battery further! Good work!",3);
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
	
    %client.SetMaxBatteryEnergy(%client.GetMaxBatteryEnergy() + 5000);
    %client.schedule(33, "centerPrint", "\c4Upgrade Successful!", 2);
    %client.play2d(rewardSound);
    %obj.emote(winStarProjectile, 1);
    %obj.playThread(2, shiftaway);
    %obj.unMountImage(0);
}