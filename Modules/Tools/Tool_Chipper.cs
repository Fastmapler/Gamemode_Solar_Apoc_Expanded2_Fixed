//Requires Item_PlayingCards!!

$canPickUpChips = true;
//Enable pickup of all chips

$MatterChipTypeCount = 5;
$MatterChipType0 = getColorDecimalFromHex("c1a872ff");
$MatterChipType0Cost = "1";

$MatterChipType1 = getColorDecimalFromHex("7a7a7aff");
$MatterChipType1Cost = "8";

$MatterChipType2 = getColorDecimalFromHex("d36b04ff");
$MatterChipType2Cost = "32";

$MatterChipType3 = getColorDecimalFromHex("e2af14ff");
$MatterChipType3Cost = "256";

$MatterChipType4 = getColorDecimalFromHex("646defff");
$MatterChipType4Cost = "2048";

function Player::placeMatterChips(%pl, %value, %type, %loc) {
	%chipVector = getChipCounts(%value);
	%count = 0;
	%largestChipCount = 0;
	for (%i = 0; %i < getWordCount(%chipVector); %i++) {
		%chipCount = getWord(%chipVector, %i);

		if (%chipCount > 0) {
			%chip[%count] = new StaticShape(MatterChipShapes) {
				datablock = ChipShape;
			};
		}

		if (!isObject(%chip[%count])) {
			continue;
		}
		%chip[%count].setNodeColor("ALL", $MatterChipType[%i]);
		%chip[%count].setScale("1 1 " @ %chipCount);

		if (%chipCount > %largestChipCount) {
			%largestChipCount = %chipCount;
			%largest = %count;
		}
		%chip[%count].setShapeName("");
		%chip[%count].setShapeNameColor("1 1 1");

		%chip[%count].setTransform(vectorAdd($offset[%count], %loc));
		%count++;
	}

	if (%count > 0) {
		%group = new ScriptGroup(MatterChipShapes) { 
			center = %loc;
			matterValue = %value;
			type = %type;
			sourceObject = %pl;
			sourceClient = %pl.client;
		};

		for (%i = 0; %i < %count; %i++) {
			%group.add(%chip[%i]);
		}
	}

	if (isObject(%chip[%largest])) {
		%chip[%largest].setShapeName(%value @ "x " @ %type);
	}
}

function pickUpMatterChips(%chip, %cl) {
	%g = %chip.getGroup();
	if (!isObject(%g) || %g.matterValue <= 0) {
		return 0;
	}

	$EOTW::Material[%cl.bl_id, %g.type] += %g.matterValue;
	%cl.player.emote(winStarProjectile, 1);

	%g.chainDeleteAll();
	%g.delete();
	return 1;
}

function serverCmdClearAllPlacedChips(%cl) {
	if (!%cl.isAdmin) {
		return;
	}

	while(isObject(MatterChipShapes)) {
		MatterChipShapes.getGroup().schedule(1, delete);
		MatterChipShapes.getGroup().chainDeleteAll();
	}
	messageClient(%cl, '', "All matter chips cleared");
}

datablock ItemData(MatterChipItem : ChipItem) {
	image = MatterChipImage;
	uiName = "Chips (Matter)";
	colorShiftColor = getColorDecimalFromHex("7a7a7aff");
};

datablock ShapeBaseImageData(MatterChipImage : ChipImage) {
	shapeFile = "./tex/chip.dts";

	rotation = eulerToMatrix("0 90 0");
	offset = "-0.04 0.05 0.08";
	eyeOffset = "";
  
	item = MatterChipItem;

	projectile = "";
	colorShiftColor = getColorDecimalFromHex("7a7a7aff");
  
	stateName[0] = "Activate";
	stateTimeout[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";
	stateSequence[0] = "";

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "";
	stateTransitionOnTriggerDown[1] = "";
	stateTransitionOnTriggerUp[1] = "";
	stateSequence[1] = "";
};

function MatterChipImage::onUnmount(%this, %obj, %slot) {
	%obj.canPickUpMatterChips = 0;
}

function MatterChipImage::onMount(%this, %obj, %slot) {
	%obj.canPickUpMatterChips = $canPickUpChips || %obj.permToPickUpChips;
}

package MatterChips {
	function Armor::onTrigger(%this, %obj, %trig, %val) {
		%pl = %obj;
		%cl = %pl.client;
		%s = getWords(%obj.getEyeTransform(), 0, 2);
		%masks = $TypeMasks::fxBrickObjectType | $TypeMasks::StaticObjectType | $TypeMasks::TerrainObjectType;

		if (%trig == $RIGHTCLICK && %val == 1)
		{
			if (%obj.canPickUpMatterChips && getSimTime() - %obj.lastChipPickupTime > 200) {
				%e = vectorAdd(vectorScale(%obj.getEyeVector(), 8), %s);
				%ray = containerRaycast(%s, %e, %masks, %obj);
				%hitloc = getWords(%ray, 1, 3);

				if (!isObject(getWord(%ray, 0)))
					return;

				initContainerBoxSearch(%hitloc, "0.5 0.5 0.5", $TypeMasks::StaticObjectType | $TypeMasks::ItemObjectType);
				%next = containerSearchNext();

				if (isObject(%next) && pickUpMatterChips(%next, %cl)) {
					%obj.lastChipPickupTime = getSimTime();
					return;
				}
			}
		}
			

		return parent::onTrigger(%this, %obj, %trig, %val);
	}
};
activatePackage(MatterChips);