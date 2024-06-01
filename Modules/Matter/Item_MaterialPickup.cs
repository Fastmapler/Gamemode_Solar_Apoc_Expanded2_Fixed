datablock ItemData(EOTW_OreDrop)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system
	shapeFile = "./Shapes/Ore.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	uiName = "";
	doColorShift = true;
	colorShiftColor = "0.471 0.471 0.471 1.000";
	canDrop = true;
};

function EOTW_OreDrop::onAdd(%this, %obj)
{
	if (%obj.material !$= "" && isObject(%matter = getMatterType(%obj.material)))
	{
		%obj.matter = %matter;
		%obj.setShapeName(%matter.name SPC "x" @ %obj.matAmt);
        %obj.setShapeNameColor(vectorScale(HexToRGB(getSubStr(%matter.color, 0, 6)), 1 / 255));
        %obj.setShapeNameDistance(64);
		%obj.schedulePop();
	}
	
	Parent::onAdd(%this, %obj);
}

function EOTW_OreDrop::OnPickup(%this, %obj, %player, %amt)
{
	if (!isObject(%client = %player.client)) return;

	if (%client.tutorialStep < 10)
	{
		messageClient(%client, '', "You don't need to worry about material pickups until after the tutorial.");
		return;
	}
	
	if (%obj.material !$= "")
	{
		%client.chatMessage("\c6+" @ %obj.matAmt SPC getMatterTextColor(%obj.material) @ %obj.material);
		%client.play2d(BrickChangeSound);
		%player.ChangeMatterCount(%obj.material,%obj.matAmt);
	}
	
	%obj.delete();
}

function EOTW_SpawnOreDrop(%amt, %type, %loc)
{
	%amt = mFloor(%amt);

	if (%amt < 1)
		return;

	%item = new Item()
	{
		datablock = EOTW_OreDrop;
		static    = "0";
		position  = %loc;
		rotation = EulerToAxis(getRandom(0,359) SPC getRandom(0,359) SPC getRandom(0,359)); //Todo: Get this to work.
		craftedItem = true;
		
		material = %type;
		matAmt = %amt;
	};
	
	%item.setVelocity(getRandom(-7,7) SPC getRandom(-7,7) SPC 7);
	
	return %item;
}