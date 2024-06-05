//goal is to remake plant life and make them more interesting
//the plant uses paint colors to decide what that part of the plant can do
//plants that have determined they can't grow will be removed from the plant set to prevent the likelyhood of dead plant ticks
//this solution saves and is easily stopped from being modified by players

$EOTW::PlantGrowthChance = 1/30;
$EOTW::MaxPlantTicks = 50;
function PlantLife_TickLoop()
{
    cancel($EOTW::PlantLifeLoop);

    %clientGroup = ClientGroup;
    %count = %clientGroup.getCount();

    %totalPlants = 0;
    for(%i = 0; %i < %count; %i++)
    {
        %brickGroup = %clientGroup.getObject(%i).brickgroup;
        if(!isObject(%brickGroup.EOTWPlants) || %brickGroup.EOTWPlants.getCount() == 0)
        {
            continue;
        }

        %totalPlants += %brickGroup.EOTWPlants.getCount();
        %hasPlants = %hasPlants SPC %brickGroup; 
    }

    %hasPlants = lTrim(%hasPlants);
    %hasPlantsCount = getWordCount(%hasPlants);

    if(%hasPlantsCount != 0)
    {
        %plantTicks = %totalPlants * $EOTW::PlantGrowthChance;
        %plantTicks = getMin(mFloor(%plantTicks) + (%plantTicks > (mFloor(%plantTicks) + getRandom())),$EOTW::MaxPlantTicks);
        
        for(%i = 0; %i < %plantTicks; %i++)
        {
            %plantGroup = getWord(%hasPlants,getRandom(0,%hasPlantsCount - 1)).EOTWPlants;
            %plant = %plantGroup.getObject(getRandom(0, %plantGroup.getCount() - 1));
            %plant.getDatablock().grow(%plant);
        }
    }
    
    $EOTW::PlantLifeLoop = schedule(1000, 0, "PlantLife_TickLoop");
}
$EOTW::PlantLifeLoop = schedule(1000, 0, "PlantLife_TickLoop");

function Plants_FindWall(%obj)
{
    %plantPosition = %obj.getPosition();
    %plantGroup = %obj.getGroup();
    //find wall
    %c = -1;
    %vec[%c++] = vectorAdd(%plantPosition,"0.5 0 0");
    %vec[%c++] = vectorAdd(%plantPosition,"0 0.5 0");
    %vec[%c++] = vectorAdd(%plantPosition,"-0.5 0 0");
    %vec[%c++] = vectorAdd(%plantPosition,"0 -0.5 0");
    for(%i = 0; %i < 4; %i++)
    {
        %hit = containerRaycast(%plantPosition,%vec[%i],$TypeMasks::FxBrickAlwaysObjectType,%obj);
        if(%hit != 0 && !%hit.getDatablock().isPlantBrick && %hit.getGroup() == %plantGroup)
        {
            %walls = %walls TAB %hit;
        }
    }

    %walls = lTrim(%walls);

    if(%walls $= "")
    {
        return "";
    }

    //to prevent from prefering a certain wall
    return getField(%walls,getRandom(0,getFieldCount(%walls) - 1));
}

function Plants_FindFloor(%obj,%canBePlant)
{
    %plantPosition = %obj.getPosition();
    %plantGroup = %obj.getGroup();
    //find floor
    %hit = containerRaycast(%plantPosition,vectorAdd(%plantPosition,"0 0 -0.15"),$TypeMasks::FxBrickAlwaysObjectType,%obj);
    if(%hit != 0 && (%canBePlant || !%hit.getDatablock().isPlantBrick) && %hit.getGroup() == %plantGroup)
    {
        return %hit;
    }

    return "";
}

function Plants_Grow(%obj,%floating,%surfaceNormal,%possiblePositions,%color,%alwaysAge,%ageColor)
{
    if(%possiblePositions $= "")
    {
        //this part of the plant is finished growing
        %obj.getGroup().EOTWPlants.remove(%obj);
        return "";
    }

    %positionsNormal = getRecord(%possiblePositions,2);
    %target = vectorAdd(matrixMultiply("0 0 0" SPC vectorNormalize(vectorCross(%surfaceNormal,%positionsNormal)) SPC mAcos(VectorDot(%surfaceNormal,%positionsNormal)),getField(%possiblePositions,getRandom(0,getRecord(%possiblePositions,1) - 1))),%obj.getPosition());

    if(%alwaysAge)
    {
        %obj.setcolor(%ageColor);
    }

    %output = CreateBrick(%obj.getGroup().client, %obj.getDatablock(),  %target, %color, 0, true);
    %brick = getField(%output, 0);
    %err = getField(%output, 1);
    if (!isObject(%brick))
    {
        return;
    }

    if (%err > 0 && (!%floating && %err == 2))
    {
        %brick.dontRefund = true;
        %brick.delete();
        return;
    }

    if(!%alwaysAge)
    {
        %obj.setcolor(%ageColor);
    }
 
    %brick.isBaseplate = %floating && %brick.getNumDownBricks() == 0 && !%brick.hasPathToGround();
    %brick.willCauseChainKill();
    %brick.Material = "Custom";

    return %brick;
}

function Plants_Kill(%obj)
{
    %obj.getGroup().EOTWPlants.remove(%obj);
}

datablock fxDTSBrickData (brickEOTWVinesData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Vines";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWVinesData"] = (1/8) TAB "007c3fff" TAB 128 TAB "Vines";

//vines grow along walls and form pretty patterns :)
//against a wall
$Vines::PositionsLookup[false] = "0 0 0.2" TAB "-0.5 0 0.2" TAB "-0.5 0 0" TAB "-0.5 0 -0.2" TAB "0 0 -0.2" TAB "0.5 0 -0.2" TAB "0.5 0 0" TAB "0.5 0 0.2" 
    NL 8
    NL "0 1 0";
//against a wall and on a floor
$Vines::PositionsLookup[true] = "0.5 0 0.2" TAB "0 0 0.2" TAB "-0.5 0 0.2"
    NL 3
    NL "0 1 0"; 
//against a wall
$VinesLeaves::PositionsLookup[false] = "0 0.5 0.2" TAB "-0.5 0.5 0.2" TAB "-0.5 0.5 0" TAB "-0.5 0.5 -0.2" TAB "0 0.5 -0.2" TAB "0.5 0.5 -0.2" TAB "0.5 0.5 0" TAB "0.5 0.5 0.2" TAB "0 0.5 0" 
    NL 9
    NL "0 1 0";
//against a wall and on a floor
$VinesLeaves::PositionsLookup[true] = "0.5 0.5 0.2" TAB "0 0.5 0.2" TAB "-0.5 0.5 0.2"
    NL 3
    NL "0 1 0";

$c = -1;
$type = "Vines";
$PlantAge[$type,$c++] = 6;
$PlantAge[$type,$c++] = 33;
$PlantAge[$type,$c++] = 15;
$PlantsAgeCount[$type] = $c++;
function brickEOTWVinesData::Grow(%data,%obj)
{
    if(%obj.colorid == $PlantAgeVines_2)
    {
        Plants_Kill(%obj);
        return "";
    }  
    %wall = Plants_FindWall(%obj);
    %surfaceNormal = getWords(%wall,4,6);

    //no wall
    if(%wall $= "")
    {
        Plants_Kill(%obj);
        return "";
    }

    //find floor
    %hasFloor = Plants_FindFloor(%obj) !$= "";

    %alwaysAge = true;
    //is a new vine will always age and attempt to grow
    if(%obj.colorid == $PlantAgeVines_0)
    {
        %ageColor = $PlantAgeVines_1;
        %possiblePositions = $Vines::PositionsLookup[%hasFloor];
    }

    //is an older vine and has a chance for leaves or splitting
    else if(%obj.colorid == $PlantAgeVines_1)
    {   
        %ageColor = $PlantAgeVines_2;
        //older vines have a chance to split
        if(getRandom() > 0.20)
        {
            %alwaysAge = false;
            %possiblePositions = $Vines::PositionsLookup[%hasFloor];
        }
        else
        {
            %possiblePositions = $VinesLeaves::PositionsLookup[%hasFloor];
        }
    }  

    Plants_Grow(%obj,true,%surfaceNormal,%possiblePositions,6,%alwaysAge,%ageColor);
}

datablock fxDTSBrickData (brickEOTWMossData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Moss";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWMossData"] = (1/8) TAB "899549ff" TAB 128 TAB "Moss";

//moss can grow around like moss does but can also grow up a plate
//not against a wall
$Moss::PositionsLookup[false] = "0.5 0 0" TAB "0.5 0.5 0" TAB "0 0.5 0" TAB "-0.5 0.5 0" TAB "-0.5 0 0" TAB "-0.5 -0.5 0" TAB "0 -0.5 0" TAB "0.5 -0.5 0" 
    NL 8
    NL "0 0 1";

//against a wall
$Moss::PositionsLookup[true] = "0 -0.5 0.2"
    NL 1
    NL "0 1 0";

$c = -1;
$type = "Moss";
$PlantAge[$type,$c++] = 4;
$PlantAge[$type,$c++] = 13;
$PlantAge[$type,$c++] = 31;
$PlantsAgeCount[$type] = $c++;
function brickEOTWMossData::Grow(%data,%obj)
{
    if(%obj.colorid == $PlantAgeMoss_2)
    {
        Plants_Kill(%obj);
        return "";
    }

    //find floor
    %floor = Plants_FindFloor(%obj,true);
    %surfaceNormal = getWords(%floor,4,6);

    if(%floor $= "")
    {
        Plants_Kill(%obj);
        return "";
    }

    %possiblePositions = $Moss::PositionsLookup[false];



    %alwaysAge = true;
    if(%obj.colorid == $PlantAgeMoss_0)
    {
        %agecolor = $PlantAgeMoss_1;

        %wall = Plants_FindWall(%obj);

        if(%wall !$= "")
        {
            %surfaceNormal = getWords(%wall,4,6);
            %possiblePositions = $Moss::PositionsLookup[true];
        }
    }
    else if(%obj.colorid == $PlantAgeMoss_1)
    {
        %agecolor = $PlantAgeMoss_2;
        %alwaysAge = getRandom() < 0.20;
    }

    Plants_Grow(%obj,false,%surfaceNormal,%possiblePositions,4,%alwaysAge,%agecolor);
}

datablock fxDTSBrickData (brickEOTWCactiData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Cacti";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWCactiData"] = (1/8) TAB "5d803fff" TAB 128 TAB "Cacti";

$Cactus::PositionsLookup[false] = "0 0 0.2"
    NL 1
    NL "0 0 1";
$Cactus::PositionsLookup[true] = "0.5 0 0" TAB "0 0.5 0" TAB "-0.5 0 0" TAB "0 -0.5 0"
    NL 4
    NL "0 0 1";

$c = -1;
$type = "Cacti";
$PlantAge[$type,$c++] = 5; //0 growth before split
$PlantAge[$type,$c++] = 13; //1 split
$PlantAge[$type,$c++] = 4; //2 growth after split
$PlantAge[$type,$c++] = 32; //3 dead
$PlantsAgeCount[$type] = $c++;
function brickEOTWCactiData::Grow(%data,%obj)
{
    if(%obj.colorid == $PlantAgeCacti_3)
    {
        Plants_Kill(%obj);
        return "";
    }

    %floating = false;
    %alwaysAge = true;
    if(%obj.colorid == $PlantAgeCacti_0)
    {
        %possiblePositions = $Cactus::PositionsLookup[false];
        %agecolor = $PlantAgeCacti_3;
        %color = $PlantAgeCacti_0;
        if(getRandom() < 0.0666)
        {
            %possiblePositions = $Cactus::PositionsLookup[true];
            %color = $PlantAgeCacti_1;
            %agecolor = $PlantAgeCacti_2;
            %alwaysAge = false;
            %floating = true;
        }
    }
    else if (%obj.colorid == $PlantAgeCacti_2)
    {
        %possiblePositions = $Cactus::PositionsLookup[false];
        %agecolor = $PlantAgeCacti_3;
        %color = $PlantAgeCacti_2;

        if(getRandom() < 0.025)
        {
            %possiblePositions = $Cactus::PositionsLookup[true];
            %color = $PlantAgeCacti_1;
            %agecolor = $PlantAgeCacti_2;
            %alwaysAge = false;
            %floating = true;
        }
        else if(getRandom() < 0.1)
        {
            %color = $PlantAgeCacti_3;
        }

    }
    else if(%obj.colorid == $PlantAgeCacti_1)
    {
        %agecolor = $PlantAgeCacti_3;
        %color = $PlantAgeCacti_2;
        %possiblePositions = $Cactus::PositionsLookup[true];
        %alwaysAge = getRandom() < 0.20;

        %floating = true;
    }

    Plants_Grow(%obj,%floating,"0 0 1",%possiblePositions,%color,%alwaysAge,%agecolor);
}

datablock fxDTSBrickData (brickEOTWDeadPlantData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Dead Plant";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWDeadPlantData"] = (1/16) TAB "75502eff" TAB 16 TAB "Wood";

function brickEOTWDeadPlantData::Grow(%data,%obj) // unchanged behaviour
{
    %obj.energy--;
    if (%obj.energy < -10)
    {
        %obj.dontRefund = true;
        %obj.killBrick();
    }
}

function Plants_Add(%obj)
{
    if(!isObject(%obj))
    {
        return;
    }

    %group = %obj.getGroup();
    if (!isObject(%group.EOTWPlants))
    {
        %group.EOTWPlants = new SimSet();
    }

    %group.EOTWPlants.add(%obj);
}

package EOTW_Plants
{
    function fxDtsBrick::onPlant(%obj,%b)
	{
        %brick.isFloatingBrick = true;
		parent::onPlant(%obj,%b);

        if (%obj.getDatablock().isPlantBrick)
        {
            schedule(0,%obj,"Plants_Add",%obj);
        }
	}

	function fxDtsBrick::onLoadPlant(%obj,%b)
	{
		parent::onLoadPlant(%obj,%b);

        if (%obj.getDatablock().isPlantBrick)
        {
            if (!isObject(%group.EOTWPlants))
            {
                %group.EOTWPlants = new SimSet();
            }
            %group.EOTWPlants.add(%obj);
        }
	}

    function paintProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
    {
        if(%col.getDatablock().isPlantBrick)
        {
            return;
        }
        return Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
    }

};
activatePackage("EOTW_Plants");