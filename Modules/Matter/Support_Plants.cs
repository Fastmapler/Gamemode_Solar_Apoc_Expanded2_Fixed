//goal is to remake plant life and make them more interesting
//the plant uses paint colors to decide what that part of the plant can do
//plants that have determined they can't grow will be removed from the plant set to prevent the likelyhood of dead plant ticks
//this solution saves and is easily stopped from being modified by players

$EOTW::PlantGrowthChance = 1/15;
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
        %plantTicks = mFloor(%plantTicks) + (%plantTicks > (mFloor(%plantTicks) + getRandom()));
        
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

// function fxDtsBrick::EOTW_PlantLifeTick(%obj)
// {
//     //Vines will attempt to grow along walls
//     //Moss will attempt to grow along floors and ceilings. Will not stack vertically.
//     //Cacti will grow only grow upwards, up to a stack limit. Will not grow if horizontally adjacent to another cacti block
//     //All types will only grow on blocks owned by the same bl_id
//     //bl_id 888888 and 1337 are blacklisted completely
//     if (!isObject(%client = %obj.getGroup().client))
//         return;

//     %data = %obj.getDatablock();

//     if (%data.getName() $= "brickEOTWVinesData")
//     {
       
//     }
//     else if (%data.getName() $= "brickEOTWMossData")
//     {

//     }
//     else if (%data.getName() $= "brickEOTWCactiData")
//     {

        
//     }
//     else if (%data.getName() $= "brickEOTWDeadPlantData")
//     {
//         %obj.energy--;
//         if (%obj.energy < -10)
//         {
//             %obj.dontRefund = true;
//             %obj.killBrick();
//         }
//     }
// }

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

function Plants_FindFloor(%obj)
{
    %plantPosition = %obj.getPosition();
    %plantGroup = %obj.getGroup();
    //find floor
    %hit = containerRaycast(%plantPosition,vectorAdd(%plantPosition,"0 0 -0.15"),$TypeMasks::FxBrickAlwaysObjectType,%obj);
    if(%hit != 0 && !%hit.getDatablock().isPlantBrick && %hit.getGroup() == %plantGroup)
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
function brickEOTWVinesData::Grow(%data,%obj)
{
    if(%obj.colorid == 33)
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
    if(%obj.colorid == 6)
    {
        %ageColor = 15;
        %possiblePositions = $Vines::PositionsLookup[%hasFloor];
    }

    //is an older vine and has a chance for leaves or splitting
    else if(%obj.colorid == 15)
    {   
        %ageColor = 33;
        //older vines have a chance to split
        if(getRandom() < 0.75)
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

function brickEOTWMossData::Grow(%data,%obj)
{
    if(%obj.colorid == 31)
    {
        Plants_Kill(%obj);
        return "";
    }

    //find floor
    %floor = Plants_FindFloor(%obj);
    %surfaceNormal = getWords(%floor,4,6);

    if(%floor $= "")
    {
        Plants_Kill(%obj);
        return "";
    }

    %possiblePositions = $Moss::PositionsLookup[false];

    %wall = Plants_FindWall(%obj);
    if(%wall !$= "" && getRandom() < 0.5)
    {
        %surfaceNormal = getWords(%wall,4,6);
        %possiblePositions = $Moss::PositionsLookup[true];
    }

    %alwaysAge = true;
    if(%obj.colorid == 4)
    {
        %agecolor = 13;
    }
    else if(%obj.colorid >= 13)
    {
        %agecolor = 31;
        if(getRandom() < 0.5)
        {
            %alwaysAge = false;
        }
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

function brickEOTWCactiData::Grow(%data,%obj)
{
    if(%obj.colorid == 32)
    {
        Plants_Kill(%obj);
        return "";
    }

    %floating = false;
    %alwaysAge = true;
    if(%obj.colorid == 5)
    {
        %possiblePositions = $Cactus::PositionsLookup[false];
        %agecolor = 32;
        %color = 5;
        if(getRandom() < 0.0666)
        {
            Plants_Grow(%obj,true,"0 0 1",$Cactus::PositionsLookup[true],13,false,4);
            %color = 4;
        }
    }
    else if (%obj.colorid == 4)
    {
        %possiblePositions = $Cactus::PositionsLookup[false];
        %agecolor = 32;
        %color = 4;

        if(getRandom() < 0.025)
        {
            Plants_Grow(%obj,true,"0 0 1",$Cactus::PositionsLookup[true],13,false,4);
            %color = 4;
        }
        else if(getRandom() < 0.1)
        {
            %color = 32;
        }

    }
    else if(%obj.colorid == 13)
    {
        %agecolor = 32;
        %color = 4;
        %possiblePositions = $Cactus::PositionsLookup[true];
        %alwaysAge = false;
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

function brickEOTWDeadPlantData::Grow(%data,%obj)
{
    return;
}

package EOTW_Plants
{
    function fxDtsBrick::onPlant(%obj,%b)
	{
        %brick.isFloatingBrick = true;
		parent::onPlant(%obj,%b);

        if (%obj.getDatablock().isPlantBrick)
        {
            %group = %obj.getGroup();
            if (!isObject(%group.EOTWPlants))
            {
                %group.EOTWPlants = new SimSet();
            }
            %group.EOTWPlants.add(%obj);
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