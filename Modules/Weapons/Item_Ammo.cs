function PickupAmmo(%this, %obj, %player, %amt)
{
	if (!isObject(%client = %player.client)) return;

	%client.chatMessage("\c6+" @ %this.ammoCount SPC %this.ammoType);
	%client.play2d(BrickChangeSound);
	%player.ChangeMatterCount(%this.ammoType, %this.ammoCount);
	
	%obj.delete();
}

//Machine Gun

$EOTW::ItemCrafting["AmmoPack_Bullet1"] = (256 TAB "Copper") TAB (128 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Bullet1"] = "A primitively made pack of Machine Gun ammo. Contains 30 round worth of ammo.";
datablock ItemData(AmmoPack_Bullet1 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_mg.dts";
	uiName = "Ammo - Rifle Rounds (30x)";
	colorShiftColor = "0.471 0.471 0.471 1.000";
    ammoType = "Rifle Round";
	ammoCount = 30;
};

function AmmoPack_Bullet1::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

//Shotgun

$EOTW::ItemCrafting["AmmoPack_Shotgun1"] = (256 TAB "Silver") TAB (128 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Shotgun1"] = "A pack of various materials used to fire a Shotgun. Contains 40 *Individual Pellets* worth of ammo.";
datablock ItemData(AmmoPack_Shotgun1 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_sg.dts";
	uiName = "Ammo - Shotgun Pellets (40x)";
	colorShiftColor = "0.471 0.471 0.471 1.000";
    ammoType = "Shotgun Pellet";
	ammoCount = 40;
};

function AmmoPack_Shotgun1::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

//Launcher

$EOTW::ItemCrafting["AmmoPack_Launcher1"] = (256 TAB "Coal") TAB (128 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Launcher1"] = "Crude explosives used to blow things up with the Launcher. Contains 10 *Individual Loads* worth of ammo.";
datablock ItemData(AmmoPack_Launcher1 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_gl.dts";
	uiName = "Ammo - Launcher Loads (10x)";
	colorShiftColor = "0.471 0.471 0.471 1.000";
    ammoType = "Launcher Load";
	ammoCount = 10;
};

function AmmoPack_Launcher1::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

//Exotic

$EOTW::ItemCrafting["AmmoPack_Crystal"] = (256 TAB "Quartz") TAB (128 TAB "Flurospar") TAB (64 TAB "Diamond") TAB (32 TAB "Boss Essence");
$EOTW::ItemDescription["AmmoPack_Crystal"] = "Energized crystal matrices used for crystal weaponry. Contains 32 attacks worth of ammo.";
datablock ItemData(AmmoPack_Crystal : EOTW_OreDrop)
{
	shapeFile = "./Shapes/skull.dts";
	uiName = "Ammo - Crystal Matrix (32x)";
	colorShiftColor = "0.471 0.471 0.471 1.000";
    ammoType = "Crystal Matrix";
	ammoCount = 32;
};

function AmmoPack_Crystal::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }