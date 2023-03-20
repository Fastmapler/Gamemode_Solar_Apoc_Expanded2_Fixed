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