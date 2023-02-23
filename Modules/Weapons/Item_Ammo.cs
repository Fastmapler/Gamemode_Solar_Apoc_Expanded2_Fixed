function PickupAmmo(%this, %obj, %player, %amt)
{
	if (!isObject(%client = %player.client)) return;

	%client.chatMessage("\c6+" @ %this.ammoCount SPC %this.ammoType);
	%client.play2d(BrickChangeSound);
	%player.ChangeMatterCount(%this.ammoType, %this.ammoCount);
	
	%obj.delete();
}

//Machine Gun

$EOTW::ItemCrafting["AmmoPack_Bullet1"] = (128 TAB "Copper") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Bullet1"] = "A primitively made pack of Machine Gun ammo. Contains 30 round worth of ammo.";
datablock ItemData(AmmoPack_Bullet1 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_mg.dts";
	uiName = "Ammo - Rifle Rounds (30x)";
	colorShiftColor = "0.400 0.400 0.400 1.000";
    ammoType = "Rifle Round";
	ammoCount = 30;
};
function AmmoPack_Bullet1::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

$EOTW::ItemCrafting["AmmoPack_Bullet2"] = (128 TAB "Red Gold") TAB (64 TAB "Plastic") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Bullet2"] = "An efficently made pack of Machine Gun ammo. Contains 90 round worth of ammo.";
datablock ItemData(AmmoPack_Bullet2 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_mg.dts";
	uiName = "Ammo - Rifle Rounds (90x)";
	colorShiftColor = "0.400 0.400 0.800 1.000";
    ammoType = "Rifle Round";
	ammoCount = 90;
};
function AmmoPack_Bullet2::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

$EOTW::ItemCrafting["AmmoPack_Bullet3"] = (128 TAB "Naturum") TAB (64 TAB "Explosives") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Bullet3"] = "An exquisitely made pack of Machine Gun ammo. Contains 270 round worth of ammo.";
datablock ItemData(AmmoPack_Bullet3 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_mg.dts";
	uiName = "Ammo - Rifle Rounds (270x)";
	colorShiftColor = "0.400 0.800 0.400 1.000";
    ammoType = "Rifle Round";
	ammoCount = 270;
};
function AmmoPack_Bullet3::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

//Shotgun

$EOTW::ItemCrafting["AmmoPack_Shotgun1"] = (128 TAB "Silver") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Shotgun1"] = "A random pack of various materials used to fire a Shotgun. Contains 40 *Individual Pellets* worth of ammo.";
datablock ItemData(AmmoPack_Shotgun1 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_sg.dts";
	uiName = "Ammo - Shotgun Pellets (40x)";
	colorShiftColor = "0.400 0.400 0.400 1.000";
    ammoType = "Shotgun Pellet";
	ammoCount = 40;
};
function AmmoPack_Shotgun1::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

$EOTW::ItemCrafting["AmmoPack_Shotgun2"] = (128 TAB "Electrum") TAB (64 TAB "Plastic") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Shotgun2"] = "Useful supplies used to fire rounds from a Shotgun. Contains 120 *Individual Pellets* worth of ammo.";
datablock ItemData(AmmoPack_Shotgun2 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_sg.dts";
	uiName = "Ammo - Shotgun Pellets (120x)";
	colorShiftColor = "0.400 0.400 0.800 1.000";
    ammoType = "Shotgun Pellet";
	ammoCount = 120;
};
function AmmoPack_Shotgun2::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

$EOTW::ItemCrafting["AmmoPack_Shotgun3"] = (128 TAB "Energium") TAB (64 TAB "Explosives") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Shotgun3"] = "Meticulously made material to be fired from a Shotgun. Contains 360 *Individual Pellets* worth of ammo.";
datablock ItemData(AmmoPack_Shotgun3 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_sg.dts";
	uiName = "Ammo - Shotgun Pellets (360x)";
	colorShiftColor = "0.400 0.800 0.400 1.000";
    ammoType = "Shotgun Pellet";
	ammoCount = 360;
};
function AmmoPack_Shotgun3::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

//Launcher

$EOTW::ItemCrafting["AmmoPack_Launcher1"] = (128 TAB "Plastic") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Launcher1"] = "Crude explosives used to blow things up with the Launcher. Contains 10 *Individual Loads* worth of ammo.";
datablock ItemData(AmmoPack_Launcher1 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_gl.dts";
	uiName = "Ammo - Launcher Loads (10x)";
	colorShiftColor = "0.400 0.400 0.400 1.000";
    ammoType = "Launcher Load";
	ammoCount = 10;
};
function AmmoPack_Launcher1::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

$EOTW::ItemCrafting["AmmoPack_Launcher2"] = (128 TAB "Teflon") TAB (64 TAB "Plastic") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Launcher2"] = "Explosive shells to be applied via the Launcher. Contains 30 *Individual Loads* worth of ammo.";
datablock ItemData(AmmoPack_Launcher2 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_gl.dts";
	uiName = "Ammo - Launcher Loads (30x)";
	colorShiftColor = "0.400 0.400 0.800 1.000";
    ammoType = "Launcher Load";
	ammoCount = 30;
};
function AmmoPack_Launcher2::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }

$EOTW::ItemCrafting["AmmoPack_Launcher3"] = (128 TAB "Epoxy") TAB (64 TAB "Explosives") TAB (64 TAB "Lead");
$EOTW::ItemDescription["AmmoPack_Launcher3"] = "Master crafted payloads to blow your enemies using the Launcher. Contains 90 *Individual Loads* worth of ammo.";
datablock ItemData(AmmoPack_Launcher3 : EOTW_OreDrop)
{
	shapeFile = "./Shapes/ammo_gl.dts";
	uiName = "Ammo - Launcher Loads (90x)";
	colorShiftColor = "0.400 0.800 0.400 1.000";
    ammoType = "Launcher Load";
	ammoCount = 90;
};
function AmmoPack_Launcher3::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }


//Exotic

$EOTW::ItemCrafting["AmmoPack_Crystal"] = (128 TAB "Energium") TAB (64 TAB "Explosives") TAB (64 TAB "Lead");
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

$EOTW::ItemCrafting["AmmoPack_Nuke"] = (256 TAB "Jet Fuel") TAB (128 TAB "Uraninite") TAB (64 TAB "Plutonium") TAB (32 TAB "Boss Essence");
$EOTW::ItemDescription["AmmoPack_Nuke"] = "This is a bad idea. One use missile for the Nuke Cannon.";
datablock ItemData(AmmoPack_Nuke : EOTW_OreDrop)
{
	shapeFile = "./Shapes/Nuke.dts";
	uiName = "Ammo - Nuke (1x)";
	colorShiftColor = "0.471 0.471 0.471 1.000";
    ammoType = "Crystal Matrix";
	ammoCount = 1;
};
function AmmoPack_Nuke::OnPickup(%this, %obj, %player, %amt) { PickupAmmo(%this, %obj, %player, %amt); }