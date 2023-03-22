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

function getMatterChipCounts(%value) {
	if (%value <= 0) {
		return "0 0 0 0 0";
	}

	for (%i = $MatterChipTypeCount - 1; %i >= 0; %i--) {
		%ret = mFloor(%value / $MatterChipType[%i @ "Cost"]) SPC %ret;
		%value = %value - mFloor(%value / $MatterChipType[%i @ "Cost"]) * $MatterChipType[%i @ "Cost"];
	}
	return trim(%ret);
}

function mergeNearbyMatterChips(%startChip, %mergeMultiple, %loc, %radius) {
	// talk(" ");
	// talk("StartChip: " @ %startChip);
	%og = %startChip.getGroup();
	%so = %og.sourceClient;
	%type = %og.type;
	// %value = 0;

	// %og.chainDeleteAll();
	// %og.delete();

	if (%radius > 0) {
		%bounds = %radius SPC %radius SPC %radius;
	} else {
		%bounds = "1 1 1";
	}
	initContainerBoxSearch(%loc, %bounds, $TypeMasks::StaticObjectType);
	%next = containerSearchNext();

	// talk("Search Loc: " @ %loc);
	%count = 0;
	while (isObject(%next) && %count < 100) {
		// talk("obj" @ %count @ ": " @ %next SPC %next.getClassName());
		
		if (!isObject(%next.getGroup()) || %next.getGroup().matterValue <= 0) {
			// talk("    No value, skipping");
			%next = containerSearchNext();
			continue;
		}
		// talk("    Client: " @ %next.getGroup().sourceClient);

		if (%next.getGroup().sourceClient == %so && %next.getGroup() != %og) {
			%singleSourceFound++;
		}

		%found[%count] = %next.getGroup();
		%next = containerSearchNext();
		%count++;
	}

	%addedCount = 1;
	if (!%mergeMultiple) { //(%singleSourceFound > 1) {
		for (%i = 0; %i < %count; %i++) {
			if (isObject(%found[%i]) && %found[%i].sourceClient == %so && %found[%i].type $= %type) {
				%found[%i].chainDeleteAll();
				%value += %found[%i].matterValue;
				%found[%i].delete();
			}
		}
	} else {
		for (%i = 0; %i < %count; %i++) {
			if (isObject(%found[%i]) && %found[%i].type $= %type) {
				%found[%i].chainDeleteAll();
				%value += %found[%i].matterValue;
				%found[%i].delete();
			}
		}
	}

	if (!%mergeMultiple) { //(%singleSourceFound > 1) {
		if (!isObject(%so.player)) {
			%temp = new ScriptObject() { client = %so; };
		} else {
			%temp = %so.player;
		}
		Player::placeMatterChips(%temp, %value, %type, %loc);
		if (!isObject(%so.player)) {
			%temp.delete();
		}
	} else {
		Player::placeMatterChips("", %value, %type, %loc);
	}
}

function Player::placeMatterChips(%pl, %value, %type, %loc) {
	%chipVector = getMatterChipCounts(%value);
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
		%chip[%count].setShapeNameColor(getColorDecimalFromHex(getMatterType(%type).color));

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
	item = MatterChipItem;
	colorShiftColor = getColorDecimalFromHex("7a7a7aff");
	showMatterBetting = true;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1] = 1;

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateFire[2] = 1;
	stateAllowImageChange[2] = 1;
	stateTimeoutValue[2] = 0.25;
	stateTransitionOnTimeout[2] = "Reload";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateEmitter[2] = "";
	stateEmitterTime[2] = 0.0;
	stateEmitterNode[2] = "muzzleNode";
	stateSound[2] = "";
	stateSequence[2] = "fire";

	stateName[3] = "Reload";
	stateTransitionOnTriggerUp[3] = "Ready";
};

function MatterChipImage::onFire(%this, %obj, %slot) {

	if (%obj.matterBet !$= "") {
		%pl = %obj;
		%cl = %pl.client;
		%s = getWords(%obj.getEyeTransform(), 0, 2);
		%masks = $TypeMasks::fxBrickObjectType | $TypeMasks::StaticObjectType | $TypeMasks::TerrainObjectType;
		%e = vectorAdd(vectorScale(%obj.getEyeVector(), 5), %s);
		%ray = containerRaycast(%s, %e, %masks, %obj);
		%hitloc = getWords(%ray, 1, 3);

		if (!isObject(getWord(%ray, 0)) || getWords(%ray, 4, 6) !$= "0 0 1") {
			return;
		}

		%amount = getField(%obj.matterBet, 0);
		%type = getField(%obj.matterBet, 1);

		%amount = getMin($EOTW::Material[%cl.bl_id, %type], %amount);
		
		%obj.placeMatterChips(%amount, %type, %hitloc);
		%obj.matterBet = "";

		$EOTW::Material[%cl.bl_id, %type] -= %amount;

		initContainerBoxSearch(%hitloc, "0.5 0.5 0.5", $TypeMasks::StaticObjectType | $TypeMasks::ItemObjectType);
		%next = containerSearchNext();

		mergeNearbyMatterChips(%next, 0, %hitloc, 0.2);
		return;
	}
}

function MatterChipImage::onUnmount(%this, %obj, %slot) {
	%obj.canPickUpMatterChips = 0;
}

function MatterChipImage::onMount(%this, %obj, %slot) {
	%obj.canPickUpMatterChips = $canPickUpChips || %obj.permToPickUpChips;
}

function serverCmdBetMatter(%cl, %val, %type1, %type2, %type3, %type4) {
	%type = trim(%type1 SPC %type2 SPC %type3 SPC %type4);
	%val = mFloor(%val);
	%pl = %cl.player;
	if (!isObject(%pl)) {
		messageClient(%cl, '', "You cannot bet while dead!");
		return;
	} else if (%val <= 0) {
		messageClient(%cl, '', "You cannot bet a value lower or equal to 0!");
		return;
	} else if (!getMatterType(%type)) {
		messageClient(%cl, '', "Specify an existing matter type!");
		return;
	}
	
	
	%type = getMatterType(%type).name;
	%val = getMin(%val, $EOTW::Material[%cl.bl_id, %type]);

	%cl.player.matterBet = %val TAB %type;
	if (!%pl.isChipsVisible) {
		messageClient(%cl, '', "\c6Bet set at " @ %val SPC %type @ "!");
	}
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