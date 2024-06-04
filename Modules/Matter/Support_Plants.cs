function PlantLife_TickLoop()
{
    cancel($EOTW::PlantLifeLoop);
    if (isObject(EOTWPlants) && EOTWPlants.getCount() >= 1)
    {
        %totalTicks = 0;
        for (%i = getMax(mLog(EOTWPlants.getCount() / 100), getMin(mLog(EOTWPlants.getCount() / 100), 1)); %i > 0; %i--)
            if (getRandom() < %i)
                %totalTicks++;

        for (%i = 0; %i < %totalTicks; %i++)
        {
            %plant = EOTWPlants.getObject(getRandom(0, EOTWPlants.getCount() - 1));

            if (%groupTicked[%plant.getGroup().bl_id] < 5)
            {
                %groupTicked[%plant.getGroup().bl_id]++;
                %plant.EOTW_PlantLifeTick();
            }
        }
            
    }
    $EOTW::PlantLifeLoop = schedule(1000, 0, "PlantLife_TickLoop");
}
$EOTW::PlantLifeLoop = schedule(1000, 0, "PlantLife_TickLoop");

function fxDtsBrick::EOTW_PlantLifeTick(%obj)
{
    //Vines will attempt to grow along walls
    //Moss will attempt to grow along floors and ceilings. Will not stack vertically.
    //Cacti will grow only grow upwards, up to a stack limit. Will not grow if horizontally adjacent to another cacti block
    //All types will only grow on blocks owned by the same bl_id
    //bl_id 888888 and 1337 are blacklisted completely

    if (!isObject(%client = %obj.getGroup().client))
        return;

    %data = %obj.getDatablock();

    if (%data.getName() $= "brickEOTWVinesData")
    {
        if (isObject(%obj.getDownBrick(0)) && !%obj.isplanted)
            return;

        if(%obj.length > 16)
            return;

        %output = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), "0 0 -0.2"), %obj.getColorID(), %angleID, true);
        %brick = getField(%output, 0);
        %err = getField(%output, 1);
        if (isObject(%brick))
        {
            if (%err > 0)
            {
                %brick.dontRefund = true;
                %brick.delete();
            }
            else
            {
                %brick.Material = "Custom";
                %brick.length = %obj.length + 1;
            }
        }
    }
    else if (%data.getName() $= "brickEOTWMossData")
    {
        %angleID = getRandom(0, 3);

        switch (%angleID)
        {
             case 0: %dir = "0.5 0 0";
             case 1: %dir = "-0.5 0 0";
             case 2: %dir = "0 0.5 0";
             case 3: %dir = "0 -0.5 0";
        }

        %output = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), %dir), %obj.getColorID(), %angleID, true);
        %brick = getField(%output, 0);
        %err = getField(%output, 1);
        if (isObject(%brick))
        {
            for (%i = 0; isObject(%colBrick = %brick.getDownBrick(%i)); %i++)
            {
                if (%colBrick.getGroup() == %brick.getGroup())
                {
                    %winCount++;
                    break;
                }
            }
            for (%i = 0; isObject(%colBrick = %brick.getUpBrick(%i)); %i++)
            {
                if (%colBrick.getGroup() == %brick.getGroup())
                {
                    %winCount++;
                    break;
                }
            }
            %brick.Material = "Custom";

            if (%winCount < 1 || %err > 0)
            {
                %brick.dontRefund = true;
                %brick.delete();
            }
        }
        
    }
    else if (%data.getName() $= "brickEOTWCactiData")
    {
        if (isObject(%obj.getUpBrick(0)) && !%obj.isplanted)
            return;

        if(%obj.length > 16)
            return;

        %output = CreateBrick(%client, %data, vectorAdd(%obj.getPosition(), "0 0 0.2"), %obj.getColorID(), %angleID, true);
        %brick = getField(%output, 0);
        %err = getField(%output, 1);
        if (isObject(%brick))
        {
            if (%err > 0)
            {
                %brick.dontRefund = true;
                %brick.delete();
            }
            else
            {
                %brick.Material = "Custom";
                %brick.length = %obj.length + 1;
            }
        }
        
    }
    else if (%data.getName() $= "brickEOTWDeadPlantData")
    {
        %obj.energy--;
        if (%obj.energy < -10)
        {
            %obj.dontRefund = true;
            %obj.killBrick();
        }
    }
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
$EOTW::CustomBrickCost["brickEOTWVinesData"] = (1/8) TAB "264b38ff" TAB 128 TAB "Vines";

datablock fxDTSBrickData (brickEOTWMossData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Moss";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWMossData"] = (1/8) TAB "446942ff" TAB 128 TAB "Moss";

datablock fxDTSBrickData (brickEOTWCactiData)
{
	brickFile = "base/data/bricks/flats/1x1f.blb";
	category = "Solar Apoc";
	subCategory = "Plant Life";
	uiName = "Cacti";
	iconName = "base/client/ui/brickIcons/1x1F";

    isPlantBrick = true;
};
$EOTW::CustomBrickCost["brickEOTWCactiData"] = (1/8) TAB "56643bff" TAB 128 TAB "Cacti";

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

package EOTW_Plants
{
    function fxDtsBrick::onPlant(%obj,%b)
	{
		parent::onPlant(%obj,%b);

        if (%obj.getDatablock().isPlantBrick)
        {
            if (!isObject(EOTWPlants))
            {
                new SimSet(EOTWPlants);
                PlantLife_TickLoop();
            }
            EOTWPlants.add(%obj);
        }
	}

	function fxDtsBrick::onLoadPlant(%obj,%b)
	{
		parent::onLoadPlant(%obj,%b);

        if (%obj.getDatablock().isPlantBrick)
        {
            if (!isObject(EOTWPlants))
            {
                new SimSet(EOTWPlants);
                PlantLife_TickLoop();
            }
            EOTWPlants.add(%obj);
        }
	}
};
activatePackage("EOTW_Plants");