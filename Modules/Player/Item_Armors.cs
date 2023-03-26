function Player::SwapKitDatablock(%obj, %this)
{
	cancel(%obj.peggstep);
	%data = %this.playertype;
	%oldData = %obj.getDatablock();

	if (%obj.moveRunning)
		%obj.StopRunMove();

	%obj.changeDatablock(%data);
	
	%obj.setWhiteOut(0.25);
	if (%this.playerscale > 0.2) %obj.setScale(%this.playerscale SPC %this.playerscale SPC %this.playerscale);
	if (%data.uiName !$= "") %obj.client.centerPrint("\c2You are now a \c3" @ %data.uiName @ "\c2!", 2);
	else %obj.client.centerPrint("\c2You changed your playertype!", 2);

	%slot = %obj.currTool;
	%obj.tool[%slot] = 0;
    %obj.weaponCount--;
    messageClient(%obj.client, 'MsgItemPickup', '', %slot, 0);
    serverCmdUnUseTool(%obj.client);
	if (isObject(%kit = %oldData.kitDatablock))
	{
		%obj.weaponCount++;
		%obj.tool[%slot] = %kit.getID();
		messageClient(%obj.client, 'MsgItemPickup', '', %slot, %kit.getID());
	}

	if (%data.maxTools < %oldData.maxTools)
		for (%i = %data.maxTools; %i < %oldData.maxTools; %i++)
			ServerCmdDropTool(%obj.client, %i);
}

//Defense
//Squire
$EOTW::ItemCrafting["SquirePlayerKitItem"] = (512 TAB "Iron") TAB (48 TAB "Lead");
$EOTW::ItemDescription["SquirePlayerKitItem"] = "125% Max HP.";
datablock ItemData(SquirePlayerKitItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "base/data/shapes/brickweapon.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	uiName = "Armor I (Tank)";
	iconName = "./Icons/icon_brickTool";
	doColorShift = true;
	colorShiftColor = "1.0 0.0 0.0 1.000";

	image = SquirePlayerKitImage;
	canDrop = true;
};

datablock ShapeBaseImageData(SquirePlayerKitImage)
{
	shapeFile = "base/data/shapes/brickweapon.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0.4 0";
	eyeOffset = 0;
	rotation = eulerToMatrix("0 0 180");

	className = "WeaponImage";
	item = SquirePlayerKitItem;

	armReady = true;

	doColorShift = SquirePlayerKitItem.doColorShift;
	colorShiftColor = SquirePlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocSquire;
	playerscale = 1.0;

	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.15;
	stateSequence[0]				= "Activate";
	stateTransitionOnTimeout[0]     = "Safety";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]					= "Safety";
	stateTransitionOnTriggerUp[1]	= "Ready";
	stateAllowImageChange[1]		= true;

	stateName[2]					= "Ready";
	stateTransitionOnTriggerDown[2]	= "Fire";
	stateAllowImageChange[2]		= true;

	stateName[3]					= "Fire";
	stateTransitionOnTimeout[3]		= "Ready";
	stateAllowImageChange[3]		= true;
	stateScript[3]					= "onFire";
	stateTimeoutValue[3]			= 1;
};

function SquirePlayerKitImage::onFire(%this, %obj, %slot)
{
	%obj.SwapKitDatablock(%this, %slot);
}

datablock PlayerData(PlayerSolarApocSquire : PlayerSolarApoc)
{
	maxDamage = 125;
	uiName = "Armor I (Tank)";
	kitDatablock = SquirePlayerKitItem;
	protectType = "Tank";
};

//Knight
$EOTW::ItemCrafting["KnightPlayerKitItem"] = (384 TAB "Steel") TAB (96 TAB "Lead");
$EOTW::ItemDescription["KnightPlayerKitItem"] = "175% Max HP, Lava Immunity. -10% Max Stamina";
datablock ItemData(KnightPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "Armor II (Tank)";
	colorShiftColor = "1.0 0.0 0.0 1.000";
	image = KnightPlayerKitImage;
};

datablock ShapeBaseImageData(KnightPlayerKitImage : SquirePlayerKitImage)
{
	item = KnightPlayerKitItem;
	doColorShift = KnightPlayerKitItem.doColorShift;
	colorShiftColor = KnightPlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocKnight;
	playerscale = 1.0;
};

function KnightPlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }

datablock PlayerData(PlayerSolarApocKnight : PlayerSolarApoc)
{
	maxDamage = 175;
	maxEnergy = 90;
	lavaImmune = true;
	uiName = "Armor II (Tank)";
	kitDatablock = KnightPlayerKitItem;
	protectType = "Tank";
};

//King
$EOTW::ItemCrafting["KingPlayerKitItem"] = (512 TAB "Adamantine") TAB (192 TAB "Lead");
$EOTW::ItemDescription["KingPlayerKitItem"] = "250% Max HP, Lava Immunity, 50% DR Against Sun. -20% Max Stamina";
datablock ItemData(KingPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "Armor III (Tank)";
	colorShiftColor = "1.0 0.0 0.0 1.000";
	image = KingPlayerKitImage;
};

datablock ShapeBaseImageData(KingPlayerKitImage : SquirePlayerKitImage)
{
	item = KingPlayerKitItem;
	doColorShift = KingPlayerKitItem.doColorShift;
	colorShiftColor = KingPlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocKing;
	playerscale = 1.0;
};

function KingPlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }

datablock PlayerData(PlayerSolarApocKing : PlayerSolarApoc)
{
	maxDamage = 250;
	maxEnergy = 80;
	lavaImmune = true;
	sunResist = 0.5;
	uiName = "Armor III (Tank)";
	kitDatablock = KingPlayerKitItem;
	protectType = "Tank";
};

//Mobility
//Mobile
$EOTW::ItemCrafting["MobilePlayerKitItem"] = (64 TAB "Silver") TAB (16 TAB "Rubber");
$EOTW::ItemDescription["MobilePlayerKitItem"] = "125% Max Stamina.";
datablock ItemData(MobilePlayerKitItem : SquirePlayerKitItem)
{
	uiName = "Armor I (Agility)";
	colorShiftColor = "0.0 1.0 1.0 1.000";
	image = MobilePlayerKitImage;
};

datablock ShapeBaseImageData(MobilePlayerKitImage : SquirePlayerKitImage)
{
	item = MobilePlayerKitItem;
	doColorShift = MobilePlayerKitItem.doColorShift;
	colorShiftColor = MobilePlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocMobile;
	playerscale = 1.0;
};

function MobilePlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }

datablock PlayerData(PlayerSolarApocMobile : PlayerSolarApoc)
{
	maxEnergy = 125;
	uiName = "Armor I (Agility)";
	kitDatablock = MobilePlayerKitItem;
	protectType = "Agility";
};

//Ninja
$EOTW::ItemCrafting["NinjaPlayerKitItem"] = (512 TAB "Electrum") TAB (32 TAB "Rubber");
$EOTW::ItemDescription["NinjaPlayerKitItem"] = "175% Max Stamina. Impact Damage Immunity. -10 Max HP.";
datablock ItemData(NinjaPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "Armor II (Agility)";
	colorShiftColor = "0.0 1.0 1.0 1.000";
	image = NinjaPlayerKitImage;
};

datablock ShapeBaseImageData(NinjaPlayerKitImage : SquirePlayerKitImage)
{
	item = NinjaPlayerKitItem;
	doColorShift = NinjaPlayerKitItem.doColorShift;
	colorShiftColor = NinjaPlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocNinja;
	playerscale = 1.0;
};

function NinjaPlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }

datablock PlayerData(PlayerSolarApocNinja : PlayerSolarApoc)
{
	maxDamage = 90;
	maxEnergy = 75;
	minImpactSpeed = 1337;
	uiName = "Armor II (Agility)";
	kitDatablock = NinjaPlayerKitItem;
	protectType = "Agility";
};

//Ethereal
$EOTW::ItemCrafting["EtherealPlayerKitItem"] = (512 TAB "Energium") TAB (64 TAB "Rubber");
$EOTW::ItemDescription["EtherealPlayerKitItem"] = "250% Max Stamina, Impact Immunity, 50% Stronger Sprint Boost. -20 Max HP.";
datablock ItemData(EtherealPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "Armor III (Agility)";
	colorShiftColor = "0.0 1.0 1.0 1.000";
	image = EtherealPlayerKitImage;
};

datablock ShapeBaseImageData(EtherealPlayerKitImage : SquirePlayerKitImage)
{
	item = EtherealPlayerKitItem;
	doColorShift = EtherealPlayerKitItem.doColorShift;
	colorShiftColor = EtherealPlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocEthereal;
	playerscale = 1.0;
};

function EtherealPlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }

datablock PlayerData(PlayerSolarApocEthereal : PlayerSolarApoc)
{
	maxDamage = 80;
	maxEnergy = 250;
	minImpactSpeed = 1337;
	runBoost = 1.5;
	uiName = "Armor III (Agility)";
	kitDatablock = EtherealPlayerKitItem;
	protectType = "Agility";
};

//Default
//Knight
$EOTW::ItemCrafting["DefaultPlayerKitItem"] = (256 TAB "Granite");
$EOTW::ItemDescription["DefaultPlayerKitItem"] = "Basic, default armor. You spawn with this.";
datablock ItemData(DefaultPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "Armor 0 (Default)";
	colorShiftColor = "1.0 1.0 1.0 1.000";
	image = DefaultPlayerKitImage;
};

datablock ShapeBaseImageData(DefaultPlayerKitImage : SquirePlayerKitImage)
{
	item = DefaultPlayerKitItem;
	doColorShift = DefaultPlayerKitItem.doColorShift;
	colorShiftColor = DefaultPlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApoc;
	playerscale = 1.0;
};

function DefaultPlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }