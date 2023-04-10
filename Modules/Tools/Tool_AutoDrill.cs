$EOTW::ItemCrafting["AutoDrillItem"] = (128 TAB "Steel") TAB (64 TAB "Gold");
$EOTW::ItemDescription["AutoDrillItem"] = "An EU-powered drill which gathers a material for you. Base speed of 80%.";

datablock itemData(AutoDrillItem)
{
	uiName = "Auto-Drill I";
	iconName = "./Icons/icon_Drill";
	doColorShift = true;
	colorShiftColor = "0.10 0.10 0.10 1.00";
	
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
	offset = "0 0.25 0.15";
	rotation = eulerToMatrix("0 5 70");
	
	eyeOffset = "0.75 1.15 -0.24";
	eyeRotation = eulerToMatrix("0 5 70");
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

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

function AutoDrillImage::onFire(%this, %obj, %slot)
{
	if(!isObject(%client = %obj.client))
		return;
}