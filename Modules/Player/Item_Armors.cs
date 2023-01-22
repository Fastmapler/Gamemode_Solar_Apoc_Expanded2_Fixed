function Player::SwapKitDatablock(%obj, %this)
{
	%data = %this.playertype;
	%oldData = %obj.getDatablock();

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
$EOTW::ItemDescription["SquirePlayerKitItem"] = "Max HP+, Mobility-.";
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

	uiName = "ARMOR - Plating Squire";
	iconName = "./Icons/icon_brickTool";
	doColorShift = true;
	colorShiftColor = "1.0 1.0 1.0 1.000";

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
	stateTransitionOnTimeout[0]     = "Ready";
	stateSound[0]					= weaponSwitchSound;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1]	= "Fire";
	stateAllowImageChange[1]		= true;

	stateName[2]					= "Fire";
	stateTransitionOnTimeout[2]		= "Ready";
	stateAllowImageChange[2]		= true;
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 1;
};

function SquirePlayerKitImage::onFire(%this, %obj, %slot)
{
	%obj.SwapKitDatablock(%this, %slot);
}

datablock PlayerData(PlayerSolarApocSquire : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 125;
	maxEnergy = 100;
	rechargeRate = 0.5;
	runForce = 48 * 90 * 1.5;
	
	airControl = 0.25;
	
	maxTools = 5;
	maxWeapons = 5;

	uiName = "Solar Apoc Player Squire";
	showEnergyBar = true;

	kitDatablock = SquirePlayerKitItem;
};

//Knight
$EOTW::ItemCrafting["KnightPlayerKitItem"] = (384 TAB "Steel") TAB (96 TAB "Lead");
$EOTW::ItemDescription["KnightPlayerKitItem"] = "Max HP++, Mobility--, Lava Immunity.";
datablock ItemData(KnightPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "ARMOR - Plating Knight";
	colorShiftColor = "1.0 1.0 1.0 1.000";
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

datablock PlayerData(PlayerSolarApocKnight : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 175;
	maxEnergy = 100;
	rechargeRate = 0.5;
	runForce = 48 * 90 * 1;
	
	airControl = 0.15;
	
	maxTools = 6;
	maxWeapons = 6;

	lavaImmune = true;

	uiName = "Solar Apoc Player Knight";
	showEnergyBar = true;

	kitDatablock = KnightPlayerKitItem;
};

//King
$EOTW::ItemCrafting["KingPlayerKitItem"] = (512 TAB "Adamantine") TAB (192 TAB "Lead");
$EOTW::ItemDescription["KingPlayerKitItem"] = "Max HP+++, Mobility---, Lava Immunity.";
datablock ItemData(KingPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "ARMOR - Plating King";
	colorShiftColor = "1.0 1.0 1.0 1.000";
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

datablock PlayerData(PlayerSolarApocKing : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 250;
	maxEnergy = 100;
	rechargeRate = 0.5;
	runForce = 48 * 90 * 0.5;
	
	airControl = 0.05;
	
	maxTools = 7;
	maxWeapons = 7;

	lavaImmune = true;

	uiName = "Solar Apoc Player King";
	showEnergyBar = true;

	kitDatablock = KingPlayerKitItem;
};

//Mobility
//Mobile
$EOTW::ItemCrafting["MobilePlayerKitItem"] = (64 TAB "Silver") TAB (16 TAB "Rubber");
$EOTW::ItemDescription["MobilePlayerKitItem"] = "Max HP-, Mobility+.";
datablock ItemData(MobilePlayerKitItem : SquirePlayerKitItem)
{
	uiName = "ARMOR - Light Mobile";
	colorShiftColor = "1.0 1.0 1.0 1.000";
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

datablock PlayerData(PlayerSolarApocMobile : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 100;
	maxEnergy = 125;
	rechargeRate = 0.6;
	runForce = 48 * 90 * 2;
	
	airControl = 0.5;
	
	maxTools = 5;
	maxWeapons = 5;

	uiName = "Solar Apoc Player Mobile";
	showEnergyBar = true;

	kitDatablock = MobilePlayerKitItem;
};

//Ninja
$EOTW::ItemCrafting["NinjaPlayerKitItem"] = (512 TAB "Electrum") TAB (32 TAB "Rubber");
$EOTW::ItemDescription["NinjaPlayerKitItem"] = "Max HP--, Mobility++, Fall Damage Resist.";
datablock ItemData(NinjaPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "ARMOR - Light Ninja";
	colorShiftColor = "1.0 1.0 1.0 1.000";
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

datablock PlayerData(PlayerSolarApocNinja : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 90;
	maxEnergy = 150;
	rechargeRate = 0.8;
	runForce = 48 * 90 * 2.5;
	jumpForce = 1080 * 1.5;
	
	airControl = 0.75;
	
	maxTools = 5;
	maxWeapons = 5;

	minImpactSpeed = 30;
	speedDamageScale = 1.0;

	uiName = "Solar Apoc Player Ninja";
	showEnergyBar = true;

	kitDatablock = NinjaPlayerKitItem;
};

//Ethereal
$EOTW::ItemCrafting["EtherealPlayerKitItem"] = (512 TAB "Energium") TAB (64 TAB "Rubber");
$EOTW::ItemDescription["EtherealPlayerKitItem"] = "Max HP---, Mobility+++, Fall Damage Immunity.";
datablock ItemData(EtherealPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "ARMOR - Light Ethereal";
	colorShiftColor = "1.0 1.0 1.0 1.000";
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

datablock PlayerData(PlayerSolarApocEthereal : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 80;
	maxEnergy = 200;
	rechargeRate = 1.0;
	runForce = 48 * 90 * 3;
	jumpForce = 1080 * 2;
	
	airControl = 1.0;
	
	maxTools = 6;
	maxWeapons = 6;

	minImpactSpeed = 999;
	speedDamageScale = 1.0;

	uiName = "Solar Apoc Player Ethereal";
	showEnergyBar = true;

	kitDatablock = EtherealPlayerKitItem;
};

//Inventory
//Satchel
$EOTW::ItemCrafting["SatchelPlayerKitItem"] = (128 TAB "Copper") TAB (64 TAB "Leather");
$EOTW::ItemDescription["SatchelPlayerKitItem"] = "Inventory+, Stamina Recovery-.";
datablock ItemData(SatchelPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "ARMOR - Backpack Satchel";
	colorShiftColor = "1.0 1.0 1.0 1.000";
	image = SatchelPlayerKitImage;
};

datablock ShapeBaseImageData(SatchelPlayerKitImage : SquirePlayerKitImage)
{
	item = SatchelPlayerKitItem;
	doColorShift = SatchelPlayerKitItem.doColorShift;
	colorShiftColor = SatchelPlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocSatchel;
	playerscale = 1.0;
};

function SatchelPlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }

datablock PlayerData(PlayerSolarApocSatchel : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 100;
	maxEnergy = 100;
	rechargeRate = 0.5;
	runForce = 48 * 90 * 2;
	
	airControl = 0.25;
	
	maxTools = 7;
	maxWeapons = 7;

	uiName = "Solar Apoc Player Satchel";
	showEnergyBar = true;

	kitDatablock = SatchelPlayerKitItem;
};

//Cargo
$EOTW::ItemCrafting["CargoPlayerKitItem"] = (512 TAB "Rosium") TAB (128 TAB "Leather");
$EOTW::ItemDescription["CargoPlayerKitItem"] = "Inventory++, Stamina Recovery--.";
datablock ItemData(CargoPlayerKitItem : SquirePlayerKitItem)
{
	uiName = "ARMOR - Backpack Cargo";
	colorShiftColor = "1.0 1.0 1.0 1.000";
	image = CargoPlayerKitImage;
};

datablock ShapeBaseImageData(CargoPlayerKitImage : SquirePlayerKitImage)
{
	item = CargoPlayerKitItem;
	doColorShift = CargoPlayerKitItem.doColorShift;
	colorShiftColor = CargoPlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocCargo;
	playerscale = 1.0;
};

function CargoPlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }

datablock PlayerData(PlayerSolarApocCargo : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 110;
	maxEnergy = 100;
	rechargeRate = 0.4;
	runForce = 48 * 90 * 2;
	
	airControl = 0.25;
	
	maxTools = 9;
	maxWeapons = 9;

	uiName = "Solar Apoc Player Cargo";
	showEnergyBar = true;

	kitDatablock = CargoPlayerKitItem;
};

//Wormhole
$EOTW::ItemCrafting["WormholePlayerKitItem"] = (512 TAB "Naturum") TAB (256 TAB "Leather");
$EOTW::ItemDescription["WormholePlayerKitItem"] = "Inventory+++, Stamina Recovery---.";
datablock ItemData(WormholePlayerKitItem : SquirePlayerKitItem)
{
	uiName = "ARMOR - Backpack Wormhole";
	colorShiftColor = "1.0 1.0 1.0 1.000";
	image = WormholePlayerKitImage;
};

datablock ShapeBaseImageData(WormholePlayerKitImage : SquirePlayerKitImage)
{
	item = WormholePlayerKitItem;
	doColorShift = WormholePlayerKitItem.doColorShift;
	colorShiftColor = WormholePlayerKitItem.colorShiftColor;
   
	playertype = PlayerSolarApocWormhole;
	playerscale = 1.0;
};

function WormholePlayerKitImage::onFire(%this, %obj, %slot) { %obj.SwapKitDatablock(%this, %slot); }

datablock PlayerData(PlayerSolarApocWormhole : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 120;
	maxEnergy = 100;
	rechargeRate = 0.3;
	runForce = 48 * 90 * 2;
	
	airControl = 0.25;
	
	maxTools = 12;
	maxWeapons = 12;

	uiName = "Solar Apoc Player Wormhole";
	showEnergyBar = true;

	kitDatablock = WormholePlayerKitItem;
};