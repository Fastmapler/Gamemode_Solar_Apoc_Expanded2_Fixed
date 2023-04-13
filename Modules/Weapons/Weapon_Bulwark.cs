$EOTW::ItemCrafting["BulwarkItem"] = (1 TAB "dog");
$EOTW::ItemDescription["BulwarkItem"] = "A quite massive shield.";

datablock itemData(BulwarkItem)
{
	uiName = "Bulwark Shield";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.10 0.10 0.10 1.00";
	
	shapeFile = "base/data/shapes/printGun.dts";
	image = BulwarkImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(BulwarkImage)
{
	shapeFile = "./Shapes/Bulwark.dts";
	item = BulwarkItem;
	
	mountPoint = 0;
	offset = "0 0 0";
	rotation = 0;
	
	eyeOffset = "0 1 -2";
	eyeRotation = 0;
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = BulwarkItem.doColorShift;
	colorShiftColor = BulwarkItem.colorShiftColor;
	
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